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
    public class HazardClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public HazardClassesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: HazardClasses
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string NameKK, string NameRU, int? Page)
        {
            var hazardClasses = _context.HazardClass
                .Where(h => true);

            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(NameKK))
            {
                hazardClasses = hazardClasses.Where(h => h.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                hazardClasses = hazardClasses.Where(h => h.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameKK":
                    hazardClasses = hazardClasses.OrderBy(h => h.NameKK);
                    break;
                case "NameKKDesc":
                    hazardClasses = hazardClasses.OrderByDescending(h => h.NameKK);
                    break;
                case "NameRU":
                    hazardClasses = hazardClasses.OrderBy(h => h.NameRU);
                    break;
                case "NameRUDesc":
                    hazardClasses = hazardClasses.OrderByDescending(h => h.NameRU);
                    break;
                default:
                    hazardClasses = hazardClasses.OrderBy(h => h.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(hazardClasses.Count(), Page);

            var viewModel = new HazardClassIndexPageViewModel
            {
                Items = hazardClasses.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: HazardClasses/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hazardClass = await _context.HazardClass
                .SingleOrDefaultAsync(m => m.Id == id);
            if (hazardClass == null)
            {
                return NotFound();
            }

            return View(hazardClass);
        }

        // GET: HazardClasses/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: HazardClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,NameKK,NameRU")] HazardClass hazardClass)
        {
            var hazardClasses = _context.HazardClass.AsNoTracking().ToList();
            if (hazardClasses.Select(h => h.NameKK).Contains(hazardClass.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (hazardClasses.Select(h => h.NameRU).Contains(hazardClass.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(hazardClass.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(hazardClass.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(hazardClass);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "HazardClass",
                    New = hazardClass.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hazardClass);
        }

        // GET: HazardClasses/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hazardClass = await _context.HazardClass.SingleOrDefaultAsync(m => m.Id == id);
            if (hazardClass == null)
            {
                return NotFound();
            }
            return View(hazardClass);
        }

        // POST: HazardClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameKK,NameRU")] HazardClass hazardClass)
        {
            if (id != hazardClass.Id)
            {
                return NotFound();
            }
            var hazardClasses = _context.HazardClass.AsNoTracking().ToList();
            if (hazardClasses.Where(h => h.Id != hazardClass.Id).Select(h => h.NameKK).Contains(hazardClass.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (hazardClasses.Where(h => h.Id != hazardClass.Id).Select(h => h.NameRU).Contains(hazardClass.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(hazardClass.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(hazardClass.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var hazardClass_old = _context.HazardClass.AsNoTracking().FirstOrDefault(h => h.Id == hazardClass.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "HazardClass",
                        Operation = "Edit",
                        New = hazardClass.ToString(),
                        Old = hazardClass_old.ToString()
                    });
                    _context.Update(hazardClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HazardClassExists(hazardClass.Id))
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
            return View(hazardClass);
        }

        // GET: HazardClasses/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hazardClass = await _context.HazardClass
                .SingleOrDefaultAsync(m => m.Id == id);
            if (hazardClass == null)
            {
                return NotFound();
            }
            ViewBag.Companies = _context.Company
                .AsNoTracking()
                .Where(c => c.HazardClassId == id)
                .OrderBy(c => c.AbbreviatedName);
            ViewBag.IndustrialSites = _context.IndustrialSite
                .AsNoTracking()
                .Where(i => i.HazardClassId == id)
                .OrderBy(i => i.AbbreviatedName);
            ViewBag.SubsidiaryCompanies = _context.SubsidiaryCompany
                .AsNoTracking()
                .Where(s => s.HazardClassId == id)
                .OrderBy(s => s.AbbreviatedName);
            return View(hazardClass);
        }

        // POST: HazardClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hazardClass = await _context.HazardClass.SingleOrDefaultAsync(m => m.Id == id);
            if ((_context.Company
                .AsNoTracking()
                .FirstOrDefault(c => c.HazardClassId == id) == null)
                && (_context.IndustrialSite
                .AsNoTracking()
                .FirstOrDefault(i => i.HazardClassId == id) == null)
                && (_context.SubsidiaryCompany
                .AsNoTracking()
                .FirstOrDefault(s => s.HazardClassId == id) == null))
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "HazardClass",
                    Operation = "Delete",
                    New = "",
                    Old = hazardClass.ToString()
                });
                _context.HazardClass.Remove(hazardClass);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool HazardClassExists(int id)
        {
            return _context.HazardClass.Any(e => e.Id == id);
        }
    }
}
