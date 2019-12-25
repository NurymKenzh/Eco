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
    public class LimitingSoilIndicatorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public LimitingSoilIndicatorsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: LimitingSoilIndicators
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string NameKK, string NameRU, int? Page)
        {
            var limitingSoilIndicators = _context.LimitingSoilIndicator
                .Where(t => true);

            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(NameKK))
            {
                limitingSoilIndicators = limitingSoilIndicators.Where(t => t.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                limitingSoilIndicators = limitingSoilIndicators.Where(t => t.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameKK":
                    limitingSoilIndicators = limitingSoilIndicators.OrderBy(t => t.NameKK);
                    break;
                case "NameKKDesc":
                    limitingSoilIndicators = limitingSoilIndicators.OrderByDescending(t => t.NameKK);
                    break;
                case "NameRU":
                    limitingSoilIndicators = limitingSoilIndicators.OrderBy(t => t.NameRU);
                    break;
                case "NameRUDesc":
                    limitingSoilIndicators = limitingSoilIndicators.OrderByDescending(t => t.NameRU);
                    break;
                default:
                    limitingSoilIndicators = limitingSoilIndicators.OrderBy(t => t.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(limitingSoilIndicators.Count(), Page);

            var viewModel = new LimitingSoilIndicatorIndexPageViewModel
            {
                Items = limitingSoilIndicators.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: LimitingSoilIndicators/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var limitingSoilIndicator = await _context.LimitingSoilIndicator
                .SingleOrDefaultAsync(m => m.Id == id);
            if (limitingSoilIndicator == null)
            {
                return NotFound();
            }

            return View(limitingSoilIndicator);
        }

        // GET: LimitingSoilIndicators/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: LimitingSoilIndicators/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,NameKK,NameRU")] LimitingSoilIndicator limitingSoilIndicator)
        {
            var limitingSoilIndicators = _context.LimitingSoilIndicator.AsNoTracking().ToList();
            if (limitingSoilIndicators.Select(t => t.NameKK).Contains(limitingSoilIndicator.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (limitingSoilIndicators.Select(t => t.NameRU).Contains(limitingSoilIndicator.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(limitingSoilIndicator.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(limitingSoilIndicator.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(limitingSoilIndicator);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "LimitingSoilIndicator",
                    New = limitingSoilIndicator.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(limitingSoilIndicator);
        }

        // GET: LimitingSoilIndicators/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var limitingSoilIndicator = await _context.LimitingSoilIndicator.SingleOrDefaultAsync(m => m.Id == id);
            if (limitingSoilIndicator == null)
            {
                return NotFound();
            }
            return View(limitingSoilIndicator);
        }

        // POST: LimitingSoilIndicators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameKK,NameRU")] LimitingSoilIndicator limitingSoilIndicator)
        {
            if (id != limitingSoilIndicator.Id)
            {
                return NotFound();
            }
            var limitingSoilIndicators = _context.LimitingSoilIndicator.AsNoTracking().ToList();
            if (limitingSoilIndicators.Where(t => t.Id != limitingSoilIndicator.Id).Select(t => t.NameKK).Contains(limitingSoilIndicator.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (limitingSoilIndicators.Where(t => t.Id != limitingSoilIndicator.Id).Select(t => t.NameRU).Contains(limitingSoilIndicator.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(limitingSoilIndicator.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(limitingSoilIndicator.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var limitingSoilIndicator_old = _context.LimitingSoilIndicator.AsNoTracking().FirstOrDefault(l => l.Id == limitingSoilIndicator.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "LimitingSoilIndicator",
                        Operation = "Edit",
                        New = limitingSoilIndicator.ToString(),
                        Old = limitingSoilIndicator_old.ToString()
                    });
                    _context.Update(limitingSoilIndicator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LimitingSoilIndicatorExists(limitingSoilIndicator.Id))
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
            return View(limitingSoilIndicator);
        }

        // GET: LimitingSoilIndicators/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var limitingSoilIndicator = await _context.LimitingSoilIndicator
                .SingleOrDefaultAsync(m => m.Id == id);
            if (limitingSoilIndicator == null)
            {
                return NotFound();
            }
            //ViewBag.SoilContaminants = _context.SoilContaminant
            //    .AsNoTracking()
            //    .Where(s => s.LimitingSoilIndicatorId == id)
            //    .OrderBy(s => s.Name);
            return View(limitingSoilIndicator);
        }

        // POST: LimitingSoilIndicators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var limitingSoilIndicator = await _context.LimitingSoilIndicator.SingleOrDefaultAsync(m => m.Id == id);
            //if ((_context.SoilContaminant
            //    .AsNoTracking()
            //    .FirstOrDefault(s => s.LimitingSoilIndicatorId == id) == null))
            //{
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "LimitingSoilIndicator",
                    Operation = "Delete",
                    New = "",
                    Old = limitingSoilIndicator.ToString()
                });
                _context.LimitingSoilIndicator.Remove(limitingSoilIndicator);
                await _context.SaveChangesAsync();
            //}
            return RedirectToAction(nameof(Index));
        }

        private bool LimitingSoilIndicatorExists(int id)
        {
            return _context.LimitingSoilIndicator.Any(e => e.Id == id);
        }
    }
}
