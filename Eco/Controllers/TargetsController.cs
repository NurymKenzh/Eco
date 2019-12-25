using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eco.Data;
using Eco.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

namespace Eco.Controllers
{
    public class TargetsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public TargetsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: Targets
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            int? TypeOfTargetId,
            string NameKK,
            string NameRU,
            int? Page)
        {
            var targets = _context.Target
                .Include(t => t.TypeOfTarget)
                .Where(t => true);

            ViewBag.TypeOfTargetIdFilter = TypeOfTargetId;
            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.TypeOfTargetIdSort = SortOrder == "TypeOfTargetId" ? "TypeOfTargetIdDesc" : "TypeOfTargetId";
            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";
            
            if (TypeOfTargetId != null)
            {
                targets = targets.Where(t => t.TypeOfTargetId == TypeOfTargetId);
            }
            if (!string.IsNullOrEmpty(NameKK))
            {
                targets = targets.Where(t => t.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                targets = targets.Where(t => t.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "TypeOfTargetId":
                    targets = targets.OrderBy(t => t.TypeOfTarget.Name);
                    break;
                case "TypeOfTargetIdDesc":
                    targets = targets.OrderByDescending(t => t.TypeOfTarget.Name);
                    break;
                case "NameKK":
                    targets = targets.OrderBy(t => t.NameKK);
                    break;
                case "NameKKDesc":
                    targets = targets.OrderByDescending(t => t.NameKK);
                    break;
                case "NameRU":
                    targets = targets.OrderBy(t => t.NameRU);
                    break;
                case "NameRUDesc":
                    targets = targets.OrderByDescending(t => t.NameRU);
                    break;
                default:
                    targets = targets.OrderBy(t => t.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(targets.Count(), Page);

            var viewModel = new TargetIndexPageViewModel
            {
                Items = targets.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewBag.TypeOfTargetId = new SelectList(_context.TypeOfTarget.OrderBy(t => t.Name), "Id", "Name");

            return View(viewModel);
        }

        // GET: Targets/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var target = await _context.Target
                .Include(t => t.TypeOfTarget)
                .Include(t => t.MeasurementUnit)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (target == null)
            {
                return NotFound();
            }

            return View(target);
        }

        // GET: Targets/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["TypeOfTargetId"] = new SelectList(_context.TypeOfTarget.OrderBy(t => t.Name), "Id", "Name");
            ViewData["MeasurementUnitId"] = new SelectList(_context.MeasurementUnit.OrderBy(m => m.Name), "Id", "Name");
            Target model = new Target
            {
                TypeOfAchievement = false
            };
            return View(model);
        }

        // POST: Targets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,TypeOfTargetId,NameKK,NameRU,TypeOfAchievement,MeasurementUnitId")] Target target)
        {
            var targets = _context.Target.AsNoTracking().Include(t => t.TypeOfTarget).ToList();
            if (string.IsNullOrWhiteSpace(target.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(target.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (targets.Select(t => t.NameKK).Contains(target.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (targets.Select(t => t.NameRU).Contains(target.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(target);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "Target",
                    New = target.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeOfTargetId"] = new SelectList(_context.TypeOfTarget.OrderBy(t => t.Name), "Id", "Name", target.TypeOfTargetId);
            ViewData["MeasurementUnitId"] = new SelectList(_context.MeasurementUnit.OrderBy(m => m.Name), "Id", "Name", target.MeasurementUnitId);
            return View(target);
        }

        // GET: Targets/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var target = await _context.Target.SingleOrDefaultAsync(m => m.Id == id);
            if (target == null)
            {
                return NotFound();
            }
            ViewData["TypeOfTargetId"] = new SelectList(_context.TypeOfTarget.OrderBy(t => t.Name), "Id", "Name", target.TypeOfTargetId);
            ViewData["MeasurementUnitId"] = new SelectList(_context.MeasurementUnit.OrderBy(m => m.Name), "Id", "Name", target.MeasurementUnitId);
            return View(target);
        }

        // POST: Targets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypeOfTargetId,NameKK,NameRU,TypeOfAchievement,MeasurementUnitId")] Target target)
        {
            if (id != target.Id)
            {
                return NotFound();
            }
            var targets = _context.Target.AsNoTracking().Include(t => t.TypeOfTarget).Where(t => t.Id != target.Id).ToList();
            if (string.IsNullOrWhiteSpace(target.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(target.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (targets.Select(t => t.NameKK).Contains(target.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (targets.Select(t => t.NameRU).Contains(target.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var target_old = _context.Target.AsNoTracking().FirstOrDefault(t => t.Id == target.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "Target",
                        Operation = "Edit",
                        New = target.ToString(),
                        Old = target_old.ToString()
                    });
                    _context.Update(target);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TargetExists(target.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeOfTargetId"] = new SelectList(_context.TypeOfTarget.OrderBy(t => t.Name), "Id", "Name", target.TypeOfTargetId);
            ViewData["MeasurementUnitId"] = new SelectList(_context.MeasurementUnit.OrderBy(m => m.Name), "Id", "Name", target.MeasurementUnitId);
            return View(target);
        }

        // GET: Targets/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var target = await _context.Target
                .Include(t => t.TypeOfTarget)
                .Include(t => t.MeasurementUnit)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (target == null)
            {
                return NotFound();
            }
            ViewBag.TargetValues = _context.TargetValue
                .AsNoTracking()
                .Where(t => t.TargetId == id);
            ViewBag.AActivities = _context.AActivity
                .AsNoTracking()
                .Where(a => a.TargetId == id);
            return View(target);
        }

        // POST: Targets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var target = await _context.Target.SingleOrDefaultAsync(m => m.Id == id);
            if ((_context.TargetValue
                .AsNoTracking()
                .FirstOrDefault(t => t.TargetId == id) == null)
                && (_context.AActivity
                .AsNoTracking()
                .FirstOrDefault(a => a.TargetId == id) == null))
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "Target",
                    Operation = "Delete",
                    New = "",
                    Old = target.ToString()
                });
                _context.Target.Remove(target);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TargetExists(int id)
        {
            return _context.Target.Any(e => e.Id == id);
        }
    }
}
