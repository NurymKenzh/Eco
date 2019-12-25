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
    public class SoilContaminantsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public SoilContaminantsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: SoilContaminants
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            string Name,
            int? Page)
        {
            var soilContaminants = _context.SoilContaminant
                .Where(s => true);
            
            ViewBag.NameFilter = Name;

            ViewBag.NameSort = SortOrder == "Name" ? "NameDesc" : "Name";
            
            if (!string.IsNullOrEmpty(Name))
            {
                soilContaminants = soilContaminants.Where(s => s.Name.ToLower().Contains(Name.ToLower()));
            }

            switch (SortOrder)
            {
                case "Name":
                    soilContaminants = soilContaminants.OrderBy(s => s.Name);
                    break;
                case "NameDesc":
                    soilContaminants = soilContaminants.OrderByDescending(s => s.Name);
                    break;
                default:
                    soilContaminants = soilContaminants.OrderBy(s => s.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(soilContaminants.Count(), Page);

            var viewModel = new SoilContaminantIndexPageViewModel
            {
                Items = soilContaminants.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: SoilContaminants/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soilContaminant = await _context.SoilContaminant
                .Include(s => s.NormativeSoilType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (soilContaminant == null)
            {
                return NotFound();
            }

            return View(soilContaminant);
        }

        // GET: SoilContaminants/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["NormativeSoilTypeId"] = new SelectList(_context.NormativeSoilType.OrderBy(l => l.Name), "Id", "Name");
            return View();
        }

        // POST: SoilContaminants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Name,MaximumPermissibleConcentrationSoil,NormativeSoilTypeId")] SoilContaminant soilContaminant)
        {
            var soilContaminants = _context.SoilContaminant.AsNoTracking().ToList();
            if (soilContaminants.Select(s => s.Name).Contains(soilContaminant.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(soilContaminant.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(soilContaminant);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "SoilContaminant",
                    New = soilContaminant.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NormativeSoilTypeId"] = new SelectList(_context.NormativeSoilType.OrderBy(l => l.Name), "Id", "Name", soilContaminant.NormativeSoilTypeId);
            return View(soilContaminant);
        }

        // GET: SoilContaminants/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soilContaminant = await _context.SoilContaminant.SingleOrDefaultAsync(m => m.Id == id);
            if (soilContaminant == null)
            {
                return NotFound();
            }
            ViewData["NormativeSoilTypeId"] = new SelectList(_context.NormativeSoilType.OrderBy(l => l.Name), "Id", "Name", soilContaminant.NormativeSoilTypeId);
            return View(soilContaminant);
        }

        // POST: SoilContaminants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,MaximumPermissibleConcentrationSoil,NormativeSoilTypeId")] SoilContaminant soilContaminant)
        {
            if (id != soilContaminant.Id)
            {
                return NotFound();
            }
            var soilContaminants = _context.SoilContaminant.AsNoTracking().Where(s => s.Id != soilContaminant.Id).ToList();
            if (soilContaminants.Select(s => s.Name).Contains(soilContaminant.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(soilContaminant.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var soilContaminant_old = _context.SoilContaminant.AsNoTracking().FirstOrDefault(s => s.Id == soilContaminant.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "SoilContaminant",
                        Operation = "Edit",
                        New = soilContaminant.ToString(),
                        Old = soilContaminant_old.ToString()
                    });
                    _context.Update(soilContaminant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SoilContaminantExists(soilContaminant.Id))
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
            ViewData["NormativeSoilTypeId"] = new SelectList(_context.NormativeSoilType.OrderBy(l => l.Name), "Id", "Name", soilContaminant.NormativeSoilTypeId);
            return View(soilContaminant);
        }

        // GET: SoilContaminants/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soilContaminant = await _context.SoilContaminant
                .Include(s => s.NormativeSoilType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (soilContaminant == null)
            {
                return NotFound();
            }

            return View(soilContaminant);
        }

        // POST: SoilContaminants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var soilContaminant = await _context.SoilContaminant.SingleOrDefaultAsync(m => m.Id == id);
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Class = "SoilContaminant",
                Operation = "Delete",
                New = "",
                Old = soilContaminant.ToString()
            });
            _context.SoilContaminant.Remove(soilContaminant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SoilContaminantExists(int id)
        {
            return _context.SoilContaminant.Any(e => e.Id == id);
        }
    }
}
