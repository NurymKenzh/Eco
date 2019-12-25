using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eco.Data;
using Eco.Models;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authorization;

namespace Eco.Controllers
{
    public class LimitingIndicatorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public LimitingIndicatorsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: LimitingIndicators
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string NameKK, string NameRU, int? Page)
        {
            var limitingIndicators = _context.LimitingIndicator
                .Where(l => true);

            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(NameKK))
            {
                limitingIndicators = limitingIndicators.Where(l => l.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                limitingIndicators = limitingIndicators.Where(l => l.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameKK":
                    limitingIndicators = limitingIndicators.OrderBy(l => l.NameKK);
                    break;
                case "NameKKDesc":
                    limitingIndicators = limitingIndicators.OrderByDescending(l => l.NameKK);
                    break;
                case "NameRU":
                    limitingIndicators = limitingIndicators.OrderBy(l => l.NameRU);
                    break;
                case "NameRUDesc":
                    limitingIndicators = limitingIndicators.OrderByDescending(l => l.NameRU);
                    break;
                default:
                    limitingIndicators = limitingIndicators.OrderBy(l => l.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(limitingIndicators.Count(), Page);

            var viewModel = new LimitingIndicatorIndexPageViewModel
            {
                Items = limitingIndicators.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: LimitingIndicators/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var limitingIndicator = await _context.LimitingIndicator
                .SingleOrDefaultAsync(m => m.Id == id);
            if (limitingIndicator == null)
            {
                return NotFound();
            }

            return View(limitingIndicator);
        }

        // GET: LimitingIndicators/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: LimitingIndicators/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,NameKK,NameRU")] LimitingIndicator limitingIndicator)
        {
            var limitingIndicators = _context.TypeOfTarget.AsNoTracking().ToList();
            if (limitingIndicators.Select(l => l.NameKK).Contains(limitingIndicator.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (limitingIndicators.Select(l => l.NameRU).Contains(limitingIndicator.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(limitingIndicator.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(limitingIndicator.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(limitingIndicator);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "LimitingIndicator",
                    New = limitingIndicator.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(limitingIndicator);
        }

        // GET: LimitingIndicators/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var limitingIndicator = await _context.LimitingIndicator.SingleOrDefaultAsync(m => m.Id == id);
            if (limitingIndicator == null)
            {
                return NotFound();
            }
            return View(limitingIndicator);
        }

        // POST: LimitingIndicators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameKK,NameRU")] LimitingIndicator limitingIndicator)
        {
            if (id != limitingIndicator.Id)
            {
                return NotFound();
            }
            var limitingIndicators = _context.TypeOfTarget.AsNoTracking().ToList();
            if (limitingIndicators.Where(t => t.Id != limitingIndicator.Id).Select(t => t.NameKK).Contains(limitingIndicator.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (limitingIndicators.Where(t => t.Id != limitingIndicator.Id).Select(t => t.NameRU).Contains(limitingIndicator.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(limitingIndicator.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(limitingIndicator.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var limitingIndicator_old = _context.LimitingIndicator.AsNoTracking().FirstOrDefault(l => l.Id == limitingIndicator.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "LimitingIndicator",
                        Operation = "Edit",
                        New = limitingIndicator.ToString(),
                        Old = limitingIndicator_old.ToString()
                    });
                    _context.Update(limitingIndicator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LimitingIndicatorExists(limitingIndicator.Id))
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
            return View(limitingIndicator);
        }

        // GET: LimitingIndicators/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var limitingIndicator = await _context.LimitingIndicator
                .SingleOrDefaultAsync(m => m.Id == id);
            if (limitingIndicator == null)
            {
                return NotFound();
            }
            ViewBag.AirContaminants = _context.AirContaminant
                .AsNoTracking()
                .Where(a => a.LimitingIndicatorId == id)
                .OrderBy(a => a.Name);
            return View(limitingIndicator);
        }

        // POST: LimitingIndicators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var limitingIndicator = await _context.LimitingIndicator.SingleOrDefaultAsync(m => m.Id == id);
            if (_context.AirContaminant
                .AsNoTracking()
                .FirstOrDefault(a => a.LimitingIndicatorId == id) == null)
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "LimitingIndicator",
                    Operation = "Delete",
                    New = "",
                    Old = limitingIndicator.ToString()
                });
                _context.LimitingIndicator.Remove(limitingIndicator);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool LimitingIndicatorExists(int id)
        {
            return _context.LimitingIndicator.Any(e => e.Id == id);
        }
    }
}
