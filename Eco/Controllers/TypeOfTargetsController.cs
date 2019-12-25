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
    public class TypeOfTargetsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public TypeOfTargetsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: TypeOfTargets
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string NameKK, string NameRU, int? Page)
        {
            var typesOfTargets = _context.TypeOfTarget
                .Where(t => true);
            
            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;
            
            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";
            
            if (!string.IsNullOrEmpty(NameKK))
            {
                typesOfTargets = typesOfTargets.Where(t => t.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                typesOfTargets = typesOfTargets.Where(t => t.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameKK":
                    typesOfTargets = typesOfTargets.OrderBy(t => t.NameKK);
                    break;
                case "NameKKDesc":
                    typesOfTargets = typesOfTargets.OrderByDescending(t => t.NameKK);
                    break;
                case "NameRU":
                    typesOfTargets = typesOfTargets.OrderBy(t => t.NameRU);
                    break;
                case "NameRUDesc":
                    typesOfTargets = typesOfTargets.OrderByDescending(t => t.NameRU);
                    break;
                default:
                    typesOfTargets = typesOfTargets.OrderBy(t => t.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(typesOfTargets.Count(), Page);

            var viewModel = new TypeOfTargetIndexPageViewModel
            {
                Items = typesOfTargets.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: TypeOfTargets/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfTarget = await _context.TypeOfTarget
                .SingleOrDefaultAsync(m => m.Id == id);
            if (typeOfTarget == null)
            {
                return NotFound();
            }

            return View(typeOfTarget);
        }

        // GET: TypeOfTargets/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypeOfTargets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,NameKK,NameRU")] TypeOfTarget typeOfTarget)
        {
            var typesOfTargets = _context.TypeOfTarget.AsNoTracking().ToList();
            if (typesOfTargets.Select(t => t.NameKK).Contains(typeOfTarget.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (typesOfTargets.Select(t => t.NameRU).Contains(typeOfTarget.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(typeOfTarget.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(typeOfTarget.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(typeOfTarget);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "TypeOfTarget",
                    New = typeOfTarget.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typeOfTarget);
        }

        // GET: TypeOfTargets/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfTarget = await _context.TypeOfTarget.SingleOrDefaultAsync(m => m.Id == id);
            if (typeOfTarget == null)
            {
                return NotFound();
            }
            return View(typeOfTarget);
        }

        // POST: TypeOfTargets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameKK,NameRU")] TypeOfTarget typeOfTarget)
        {
            if (id != typeOfTarget.Id)
            {
                return NotFound();
            }
            var typesOfTargets = _context.TypeOfTarget.AsNoTracking().ToList();
            if (typesOfTargets.Where(t => t.Id != typeOfTarget.Id).Select(t => t.NameKK).Contains(typeOfTarget.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (typesOfTargets.Where(t => t.Id != typeOfTarget.Id).Select(t => t.NameRU).Contains(typeOfTarget.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(typeOfTarget.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(typeOfTarget.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var typeOfTarget_old = _context.TypeOfTarget.AsNoTracking().FirstOrDefault(t => t.Id == typeOfTarget.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "TypeOfTarget",
                        Operation = "Edit",
                        New = typeOfTarget.ToString(),
                        Old = typeOfTarget_old.ToString()
                    });
                    _context.Update(typeOfTarget);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeOfTargetExists(typeOfTarget.Id))
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
            return View(typeOfTarget);
        }

        // GET: TypeOfTargets/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfTarget = await _context.TypeOfTarget
                .SingleOrDefaultAsync(m => m.Id == id);
            if (typeOfTarget == null)
            {
                return NotFound();
            }
            ViewBag.Targets = _context.Target
                .AsNoTracking()
                .Where(t => t.TypeOfTargetId == id)
                .OrderBy(t => t.Name);
            return View(typeOfTarget);
        }

        // POST: TypeOfTargets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var typeOfTarget = await _context.TypeOfTarget.SingleOrDefaultAsync(m => m.Id == id);
            if ((_context.Target
                .AsNoTracking()
                .FirstOrDefault(t => t.TypeOfTargetId == id) == null))
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "TypeOfTarget",
                    Operation = "Delete",
                    New = "",
                    Old = typeOfTarget.ToString()
                });
                _context.TypeOfTarget.Remove(typeOfTarget);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TypeOfTargetExists(int id)
        {
            return _context.TypeOfTarget.Any(e => e.Id == id);
        }
    }
}
