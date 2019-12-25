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
    public class WaterContaminantsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public WaterContaminantsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: WaterContaminants
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            string Name,
            string Number104,
            int? Page)
        {
            var waterContaminants = _context.WaterContaminant
                .Include(w => w.MeasurementUnit)
                .Where(w => true);
            
            ViewBag.NameFilter = Name;
            ViewBag.Number104Filter = Number104;

            ViewBag.NameSort = SortOrder == "Name" ? "NameDesc" : "Name";
            ViewBag.Number104Sort = SortOrder == "Number104" ? "Number104Desc" : "Number104";

            if (!string.IsNullOrEmpty(Name))
            {
                waterContaminants = waterContaminants.Where(w => w.Name.ToLower().Contains(Name.ToLower()));
            }
            if (!string.IsNullOrEmpty(Number104))
            {
                waterContaminants = waterContaminants.Where(a => a.Number104.ToString().ToLower().Contains(Number104.ToLower()));
            }

            switch (SortOrder)
            {
                case "Name":
                    waterContaminants = waterContaminants.OrderBy(w => w.Name);
                    break;
                case "NameDesc":
                    waterContaminants = waterContaminants.OrderByDescending(w => w.Name);
                    break;
                case "Number104":
                    waterContaminants = waterContaminants.OrderBy(a => a.Number104);
                    break;
                case "Number104Desc":
                    waterContaminants = waterContaminants.OrderByDescending(a => a.Number104);
                    break;
                default:
                    waterContaminants = waterContaminants.OrderBy(w => w.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(waterContaminants.Count(), Page);

            var viewModel = new WaterContaminantIndexPageViewModel
            {
                Items = waterContaminants.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: WaterContaminants/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterContaminant = await _context.WaterContaminant
                .Include(w => w.MeasurementUnit)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (waterContaminant == null)
            {
                return NotFound();
            }

            return View(waterContaminant);
        }

        // GET: WaterContaminants/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["MeasurementUnitId"] = new SelectList(_context.MeasurementUnit.OrderBy(m => m.Name), "Id", "Name");
            return View();
        }

        // POST: WaterContaminants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Name,Number104,MeasurementUnitId,Class1From,Class1To,Class2From,Class2To,Class3From,Class3To,Class4From,Class4To,Class5From,Class5To")] WaterContaminant waterContaminant)
        {
            var waterContaminants = _context.WaterContaminant.AsNoTracking().ToList();
            if (waterContaminants.Select(w => w.Name).Contains(waterContaminant.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (waterContaminants.Select(a => a.Number104).Contains(waterContaminant.Number104))
            {
                ModelState.AddModelError("Number104", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(waterContaminant.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(waterContaminant);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "WaterContaminant",
                    New = waterContaminant.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MeasurementUnitId"] = new SelectList(_context.MeasurementUnit.OrderBy(m => m.Name), "Id", "Name", waterContaminant.MeasurementUnitId);
            return View(waterContaminant);
        }

        // GET: WaterContaminants/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterContaminant = await _context.WaterContaminant.SingleOrDefaultAsync(m => m.Id == id);
            if (waterContaminant == null)
            {
                return NotFound();
            }
            ViewData["MeasurementUnitId"] = new SelectList(_context.MeasurementUnit.OrderBy(m => m.Name), "Id", "Name", waterContaminant.MeasurementUnitId);
            return View(waterContaminant);
        }

        // POST: WaterContaminants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Number104,MeasurementUnitId,Class1From,Class1To,Class2From,Class2To,Class3From,Class3To,Class4From,Class4To,Class5From,Class5To")] WaterContaminant waterContaminant)
        {
            if (id != waterContaminant.Id)
            {
                return NotFound();
            }
            var waterContaminants = _context.WaterContaminant.AsNoTracking().Where(w => w.Id != waterContaminant.Id).ToList();
            if (waterContaminants.Select(w => w.Name).Contains(waterContaminant.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(waterContaminant.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (waterContaminants.Select(a => a.Number104).Contains(waterContaminant.Number104))
            {
                ModelState.AddModelError("Number104", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var waterContaminant_old = _context.WaterContaminant.AsNoTracking().FirstOrDefault(w => w.Id == waterContaminant.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "WaterContaminant",
                        Operation = "Edit",
                        New = waterContaminant.ToString(),
                        Old = waterContaminant_old.ToString()
                    });
                    _context.Update(waterContaminant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WaterContaminantExists(waterContaminant.Id))
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
            ViewData["MeasurementUnitId"] = new SelectList(_context.MeasurementUnit.OrderBy(m => m.Name), "Id", "Name", waterContaminant.MeasurementUnitId);
            return View(waterContaminant);
        }

        // GET: WaterContaminants/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterContaminant = await _context.WaterContaminant
                .Include(w => w.MeasurementUnit)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (waterContaminant == null)
            {
                return NotFound();
            }

            return View(waterContaminant);
        }

        // POST: WaterContaminants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var waterContaminant = await _context.WaterContaminant.SingleOrDefaultAsync(m => m.Id == id);
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Class = "WaterContaminant",
                Operation = "Delete",
                New = "",
                Old = waterContaminant.ToString()
            });
            _context.WaterContaminant.Remove(waterContaminant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WaterContaminantExists(int id)
        {
            return _context.WaterContaminant.Any(e => e.Id == id);
        }
    }
}
