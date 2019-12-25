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
    public class SpeciallyProtectedNaturalTerritoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public SpeciallyProtectedNaturalTerritoriesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: SpeciallyProtectedNaturalTerritories
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string NameKK, string NameRU, int? Page)
        {
            var speciallyProtectedNaturalTerritories = _context.SpeciallyProtectedNaturalTerritory
                .Where(h => true);

            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(NameKK))
            {
                speciallyProtectedNaturalTerritories = speciallyProtectedNaturalTerritories.Where(h => h.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                speciallyProtectedNaturalTerritories = speciallyProtectedNaturalTerritories.Where(h => h.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameKK":
                    speciallyProtectedNaturalTerritories = speciallyProtectedNaturalTerritories.OrderBy(h => h.NameKK);
                    break;
                case "NameKKDesc":
                    speciallyProtectedNaturalTerritories = speciallyProtectedNaturalTerritories.OrderByDescending(h => h.NameKK);
                    break;
                case "NameRU":
                    speciallyProtectedNaturalTerritories = speciallyProtectedNaturalTerritories.OrderBy(h => h.NameRU);
                    break;
                case "NameRUDesc":
                    speciallyProtectedNaturalTerritories = speciallyProtectedNaturalTerritories.OrderByDescending(h => h.NameRU);
                    break;
                default:
                    speciallyProtectedNaturalTerritories = speciallyProtectedNaturalTerritories.OrderBy(h => h.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(speciallyProtectedNaturalTerritories.Count(), Page);

            var viewModel = new SpeciallyProtectedNaturalTerritoryIndexPageViewModel
            {
                Items = speciallyProtectedNaturalTerritories.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewData["NameKK"] = new SelectList(_context.SpeciallyProtectedNaturalTerritory.OrderBy(a => a.NameKK).GroupBy(k => k.NameKK).Select(g => g.First()), "NameKK", "NameKK");
            ViewData["NameRU"] = new SelectList(_context.SpeciallyProtectedNaturalTerritory.OrderBy(a => a.NameRU).GroupBy(k => k.NameRU).Select(g => g.First()), "NameRU", "NameRU");

            return View(viewModel);
        }

        // GET: SpeciallyProtectedNaturalTerritories/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speciallyProtectedNaturalTerritory = await _context.SpeciallyProtectedNaturalTerritory
                .Include(s => s.AuthorizedAuthority)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (speciallyProtectedNaturalTerritory == null)
            {
                return NotFound();
            }

            return View(speciallyProtectedNaturalTerritory);
        }

        // GET: SpeciallyProtectedNaturalTerritories/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["AuthorizedAuthorityId"] = new SelectList(_context.AuthorizedAuthority.OrderBy(a => a.Name), "Id", "Name");
            return View();
        }

        // POST: SpeciallyProtectedNaturalTerritories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,NameKK,NameRU,AuthorizedAuthorityId,Areahectares")] SpeciallyProtectedNaturalTerritory speciallyProtectedNaturalTerritory)
        {
            var speciallyProtectedNaturalTerritories = _context.SpeciallyProtectedNaturalTerritory.AsNoTracking().ToList();
            if (speciallyProtectedNaturalTerritories.Select(h => h.NameKK).Contains(speciallyProtectedNaturalTerritory.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (speciallyProtectedNaturalTerritories.Select(h => h.NameRU).Contains(speciallyProtectedNaturalTerritory.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(speciallyProtectedNaturalTerritory.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(speciallyProtectedNaturalTerritory.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(speciallyProtectedNaturalTerritory);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "SpeciallyProtectedNaturalTerritory",
                    New = speciallyProtectedNaturalTerritory.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorizedAuthorityId"] = new SelectList(_context.AuthorizedAuthority.OrderBy(a => a.Name), "Id", "Name", speciallyProtectedNaturalTerritory.AuthorizedAuthorityId);
            return View(speciallyProtectedNaturalTerritory);
        }

        // GET: SpeciallyProtectedNaturalTerritories/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speciallyProtectedNaturalTerritory = await _context.SpeciallyProtectedNaturalTerritory.SingleOrDefaultAsync(m => m.Id == id);
            if (speciallyProtectedNaturalTerritory == null)
            {
                return NotFound();
            }
            ViewData["AuthorizedAuthorityId"] = new SelectList(_context.AuthorizedAuthority.OrderBy(a => a.Name), "Id", "Name", speciallyProtectedNaturalTerritory.AuthorizedAuthorityId);
            return View(speciallyProtectedNaturalTerritory);
        }

        // POST: SpeciallyProtectedNaturalTerritories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameKK,NameRU,AuthorizedAuthorityId,Areahectares")] SpeciallyProtectedNaturalTerritory speciallyProtectedNaturalTerritory)
        {
            if (id != speciallyProtectedNaturalTerritory.Id)
            {
                return NotFound();
            }
            var speciallyProtectedNaturalTerritories = _context.SpeciallyProtectedNaturalTerritory.AsNoTracking().ToList();
            if (speciallyProtectedNaturalTerritories.Where(h => h.Id != speciallyProtectedNaturalTerritory.Id).Select(h => h.NameKK).Contains(speciallyProtectedNaturalTerritory.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (speciallyProtectedNaturalTerritories.Where(h => h.Id != speciallyProtectedNaturalTerritory.Id).Select(h => h.NameRU).Contains(speciallyProtectedNaturalTerritory.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(speciallyProtectedNaturalTerritory.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(speciallyProtectedNaturalTerritory.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var speciallyProtectedNaturalTerritory_old = _context.SpeciallyProtectedNaturalTerritory.AsNoTracking().FirstOrDefault(h => h.Id == speciallyProtectedNaturalTerritory.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "SpeciallyProtectedNaturalTerritory",
                        Operation = "Edit",
                        New = speciallyProtectedNaturalTerritory.ToString(),
                        Old = speciallyProtectedNaturalTerritory_old.ToString()
                    });
                    _context.Update(speciallyProtectedNaturalTerritory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpeciallyProtectedNaturalTerritoryExists(speciallyProtectedNaturalTerritory.Id))
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
            ViewData["AuthorizedAuthorityId"] = new SelectList(_context.AuthorizedAuthority.OrderBy(a => a.Name), "Id", "Name", speciallyProtectedNaturalTerritory.AuthorizedAuthorityId);
            return View(speciallyProtectedNaturalTerritory);
        }

        // GET: SpeciallyProtectedNaturalTerritories/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speciallyProtectedNaturalTerritory = await _context.SpeciallyProtectedNaturalTerritory
                .Include(s => s.AuthorizedAuthority)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (speciallyProtectedNaturalTerritory == null)
            {
                return NotFound();
            }

            return View(speciallyProtectedNaturalTerritory);
        }

        // POST: SpeciallyProtectedNaturalTerritories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var speciallyProtectedNaturalTerritory = await _context.SpeciallyProtectedNaturalTerritory.SingleOrDefaultAsync(m => m.Id == id);
            _context.SpeciallyProtectedNaturalTerritory.Remove(speciallyProtectedNaturalTerritory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpeciallyProtectedNaturalTerritoryExists(int id)
        {
            return _context.SpeciallyProtectedNaturalTerritory.Any(e => e.Id == id);
        }
    }
}
