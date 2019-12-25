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
    public class MeasurementUnitsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public MeasurementUnitsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: MeasurementUnits
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string NameKK, string NameRU, int? Page)
        {
            var measurementUnits = _context.MeasurementUnit
                .Where(h => true);

            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(NameKK))
            {
                measurementUnits = measurementUnits.Where(h => h.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                measurementUnits = measurementUnits.Where(h => h.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameKK":
                    measurementUnits = measurementUnits.OrderBy(h => h.NameKK);
                    break;
                case "NameKKDesc":
                    measurementUnits = measurementUnits.OrderByDescending(h => h.NameKK);
                    break;
                case "NameRU":
                    measurementUnits = measurementUnits.OrderBy(h => h.NameRU);
                    break;
                case "NameRUDesc":
                    measurementUnits = measurementUnits.OrderByDescending(h => h.NameRU);
                    break;
                default:
                    measurementUnits = measurementUnits.OrderBy(h => h.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(measurementUnits.Count(), Page);

            var viewModel = new MeasurementUnitIndexPageViewModel
            {
                Items = measurementUnits.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: MeasurementUnits/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var measurementUnit = await _context.MeasurementUnit
                .SingleOrDefaultAsync(m => m.Id == id);
            if (measurementUnit == null)
            {
                return NotFound();
            }

            return View(measurementUnit);
        }

        // GET: MeasurementUnits/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: MeasurementUnits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,NameKK,NameRU")] MeasurementUnit measurementUnit)
        {
            var measurementUnits = _context.MeasurementUnit.AsNoTracking().ToList();
            if (measurementUnits.Select(h => h.NameKK).Contains(measurementUnit.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (measurementUnits.Select(h => h.NameRU).Contains(measurementUnit.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(measurementUnit.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(measurementUnit.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(measurementUnit);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "MeasurementUnit",
                    New = measurementUnit.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(measurementUnit);
        }

        // GET: MeasurementUnits/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var measurementUnit = await _context.MeasurementUnit.SingleOrDefaultAsync(m => m.Id == id);
            if (measurementUnit == null)
            {
                return NotFound();
            }
            return View(measurementUnit);
        }

        // POST: MeasurementUnits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameKK,NameRU")] MeasurementUnit measurementUnit)
        {
            if (id != measurementUnit.Id)
            {
                return NotFound();
            }
            var measurementUnits = _context.MeasurementUnit.AsNoTracking().ToList();
            if (measurementUnits.Where(h => h.Id != measurementUnit.Id).Select(h => h.NameKK).Contains(measurementUnit.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (measurementUnits.Where(h => h.Id != measurementUnit.Id).Select(h => h.NameRU).Contains(measurementUnit.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(measurementUnit.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(measurementUnit.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var measurementUnit_old = _context.MeasurementUnit.AsNoTracking().FirstOrDefault(m => m.Id == measurementUnit.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "MeasurementUnit",
                        Operation = "Edit",
                        New = measurementUnit.ToString(),
                        Old = measurementUnit_old.ToString()
                    });
                    _context.Update(measurementUnit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeasurementUnitExists(measurementUnit.Id))
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
            return View(measurementUnit);
        }

        // GET: MeasurementUnits/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var measurementUnit = await _context.MeasurementUnit
                .SingleOrDefaultAsync(m => m.Id == id);
            if (measurementUnit == null)
            {
                return NotFound();
            }
            ViewBag.Targets = _context.Target
                .AsNoTracking()
                .Where(t => t.MeasurementUnitId == id)
                .OrderBy(t => t.Name);
            return View(measurementUnit);
        }

        // POST: MeasurementUnits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var measurementUnit = await _context.MeasurementUnit.SingleOrDefaultAsync(m => m.Id == id);
            if (_context.Target
                .AsNoTracking()
                .FirstOrDefault(t => t.MeasurementUnitId == id) == null)
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "MeasurementUnit",
                    Operation = "Delete",
                    New = "",
                    Old = measurementUnit.ToString()
                });
                _context.MeasurementUnit.Remove(measurementUnit);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MeasurementUnitExists(int id)
        {
            return _context.MeasurementUnit.Any(e => e.Id == id);
        }
    }
}
