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
    public class GreenPlantationsAreaAndSpeciesDiversitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public GreenPlantationsAreaAndSpeciesDiversitiesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: GreenPlantationsAreaAndSpeciesDiversities
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            int? CityDistrictId,
            int? Year,
            int? Page)
        {
            var greenPlantationsAreaAndSpeciesDiversities = _context.GreenPlantationsAreaAndSpeciesDiversity
                .Include(g => g.CityDistrict)
                .Where(c => true);

            ViewBag.CityDistrictIdFilter = CityDistrictId;
            ViewBag.YearFilter = Year;
            
            ViewBag.CityDistrictIdSort = SortOrder == "CityDistrictId" ? "CityDistrictIdDesc" : "CityDistrictId";
            ViewBag.YearSort = SortOrder == "Year" ? "YearDesc" : "Year";

            if (CityDistrictId != null)
            {
                greenPlantationsAreaAndSpeciesDiversities = greenPlantationsAreaAndSpeciesDiversities.Where(c => c.CityDistrictId == CityDistrictId);
            }
            if (Year != null)
            {
                greenPlantationsAreaAndSpeciesDiversities = greenPlantationsAreaAndSpeciesDiversities.Where(c => c.Year == Year);
            }

            switch (SortOrder)
            {
                case "CityDistrictId":
                    greenPlantationsAreaAndSpeciesDiversities = greenPlantationsAreaAndSpeciesDiversities.OrderBy(c => c.CityDistrict.Name);
                    break;
                case "CityDistrictIdDesc":
                    greenPlantationsAreaAndSpeciesDiversities = greenPlantationsAreaAndSpeciesDiversities.OrderByDescending(c => c.CityDistrict.Name);
                    break;
                case "Year":
                    greenPlantationsAreaAndSpeciesDiversities = greenPlantationsAreaAndSpeciesDiversities.OrderBy(c => c.Year);
                    break;
                case "YearDesc":
                    greenPlantationsAreaAndSpeciesDiversities = greenPlantationsAreaAndSpeciesDiversities.OrderByDescending(c => c.Year);
                    break;
                default:
                    greenPlantationsAreaAndSpeciesDiversities = greenPlantationsAreaAndSpeciesDiversities.OrderBy(c => c.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(greenPlantationsAreaAndSpeciesDiversities.Count(), Page);

            var viewModel = new GreenPlantationsAreaAndSpeciesDiversityIndexPageViewModel
            {
                Items = greenPlantationsAreaAndSpeciesDiversities.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };
            
            ViewBag.CityDistrictId = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name");
            ViewBag.Year = new SelectList(Enumerable.Range(Constants.YearDataMin, DateTime.Today.Year - Constants.YearDataMin + 1).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");

            return View(viewModel);
        }

        // GET: GreenPlantationsAreaAndSpeciesDiversities/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var greenPlantationsAreaAndSpeciesDiversity = await _context.GreenPlantationsAreaAndSpeciesDiversity
                .Include(g => g.CityDistrict)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (greenPlantationsAreaAndSpeciesDiversity == null)
            {
                return NotFound();
            }

            return View(greenPlantationsAreaAndSpeciesDiversity);
        }

        // GET: GreenPlantationsAreaAndSpeciesDiversities/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name");
            return View();
        }

        // POST: GreenPlantationsAreaAndSpeciesDiversities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,CityDistrictId,Year,AreaOfGreenCommonAreas,AreaOfGreenPlantationsOfLimitedUse,AreaOfGreenPlantationsOfSpecialUse,NumberOfTreeSpecies,AdditionalInformationKK,AdditionalInformationRU")] GreenPlantationsAreaAndSpeciesDiversity greenPlantationsAreaAndSpeciesDiversity)
        {
            if (greenPlantationsAreaAndSpeciesDiversity.Year > DateTime.Today.Year || greenPlantationsAreaAndSpeciesDiversity.Year < Constants.YearDataMin)
            {
                ModelState.AddModelError("Year", String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["Year"], Constants.YearDataMin.ToString(), DateTime.Today.Year.ToString()));
            }
            if (ModelState.IsValid)
            {
                _context.Add(greenPlantationsAreaAndSpeciesDiversity);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "GreenPlantationsAreaAndSpeciesDiversity",
                    New = greenPlantationsAreaAndSpeciesDiversity.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name", greenPlantationsAreaAndSpeciesDiversity.CityDistrictId);
            return View(greenPlantationsAreaAndSpeciesDiversity);
        }

        // GET: GreenPlantationsAreaAndSpeciesDiversities/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var greenPlantationsAreaAndSpeciesDiversity = await _context.GreenPlantationsAreaAndSpeciesDiversity.SingleOrDefaultAsync(m => m.Id == id);
            if (greenPlantationsAreaAndSpeciesDiversity == null)
            {
                return NotFound();
            }
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name", greenPlantationsAreaAndSpeciesDiversity.CityDistrictId);
            return View(greenPlantationsAreaAndSpeciesDiversity);
        }

        // POST: GreenPlantationsAreaAndSpeciesDiversities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CityDistrictId,Year,AreaOfGreenCommonAreas,AreaOfGreenPlantationsOfLimitedUse,AreaOfGreenPlantationsOfSpecialUse,NumberOfTreeSpecies,AdditionalInformationKK,AdditionalInformationRU")] GreenPlantationsAreaAndSpeciesDiversity greenPlantationsAreaAndSpeciesDiversity)
        {
            if (id != greenPlantationsAreaAndSpeciesDiversity.Id)
            {
                return NotFound();
            }
            if (greenPlantationsAreaAndSpeciesDiversity.Year > DateTime.Today.Year || greenPlantationsAreaAndSpeciesDiversity.Year < Constants.YearDataMin)
            {
                ModelState.AddModelError("Year", String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["Year"], Constants.YearDataMin.ToString(), DateTime.Today.Year.ToString()));
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var greenPlantationsAreaAndSpeciesDiversity_old = _context.GreenPlantationsAreaAndSpeciesDiversity.AsNoTracking().FirstOrDefault(c => c.Id == greenPlantationsAreaAndSpeciesDiversity.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "GreenPlantationsAreaAndSpeciesDiversity",
                        Operation = "Edit",
                        New = greenPlantationsAreaAndSpeciesDiversity.ToString(),
                        Old = greenPlantationsAreaAndSpeciesDiversity_old.ToString()
                    });
                    _context.Update(greenPlantationsAreaAndSpeciesDiversity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GreenPlantationsAreaAndSpeciesDiversityExists(greenPlantationsAreaAndSpeciesDiversity.Id))
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
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name", greenPlantationsAreaAndSpeciesDiversity.CityDistrictId);
            return View(greenPlantationsAreaAndSpeciesDiversity);
        }

        // GET: GreenPlantationsAreaAndSpeciesDiversities/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var greenPlantationsAreaAndSpeciesDiversity = await _context.GreenPlantationsAreaAndSpeciesDiversity
                .Include(g => g.CityDistrict)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (greenPlantationsAreaAndSpeciesDiversity == null)
            {
                return NotFound();
            }

            return View(greenPlantationsAreaAndSpeciesDiversity);
        }

        // POST: GreenPlantationsAreaAndSpeciesDiversities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var greenPlantationsAreaAndSpeciesDiversity = await _context.GreenPlantationsAreaAndSpeciesDiversity.SingleOrDefaultAsync(m => m.Id == id);
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Class = "GreenPlantationsAreaAndSpeciesDiversity",
                Operation = "Delete",
                New = "",
                Old = greenPlantationsAreaAndSpeciesDiversity.ToString()
            });
            _context.GreenPlantationsAreaAndSpeciesDiversity.Remove(greenPlantationsAreaAndSpeciesDiversity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GreenPlantationsAreaAndSpeciesDiversityExists(int id)
        {
            return _context.GreenPlantationsAreaAndSpeciesDiversity.Any(e => e.Id == id);
        }
    }
}
