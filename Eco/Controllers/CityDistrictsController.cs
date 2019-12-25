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
    public class CityDistrictsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public CityDistrictsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: CityDistricts
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string CATO, string NameKK, string NameRU, int? Page)
        {
            var cityDistricts = _context.CityDistrict
                .Where(c => true);

            ViewBag.CATOFilter = CATO;
            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.CATOSort = SortOrder == "CATO" ? "CATODesc" : "CATO";
            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(CATO))
            {
                cityDistricts = cityDistricts.Where(c => c.CATO.ToLower().Contains(CATO.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameKK))
            {
                cityDistricts = cityDistricts.Where(c => c.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                cityDistricts = cityDistricts.Where(c => c.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "CATO":
                    cityDistricts = cityDistricts.OrderBy(c => c.CATO);
                    break;
                case "CATODesc":
                    cityDistricts = cityDistricts.OrderByDescending(c => c.CATO);
                    break;
                case "NameKK":
                    cityDistricts = cityDistricts.OrderBy(c => c.NameKK);
                    break;
                case "NameKKDesc":
                    cityDistricts = cityDistricts.OrderByDescending(c => c.NameKK);
                    break;
                case "NameRU":
                    cityDistricts = cityDistricts.OrderBy(c => c.NameRU);
                    break;
                case "NameRUDesc":
                    cityDistricts = cityDistricts.OrderByDescending(c => c.NameRU);
                    break;
                default:
                    cityDistricts = cityDistricts.OrderBy(c => c.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(cityDistricts.Count(), Page);

            var viewModel = new CityDistrictIndexPageViewModel
            {
                Items = cityDistricts.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: CityDistricts/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cityDistrict = await _context.CityDistrict
                .SingleOrDefaultAsync(m => m.Id == id);
            if (cityDistrict == null)
            {
                return NotFound();
            }

            return View(cityDistrict);
        }

        // GET: CityDistricts/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            CityDistrict cityDistrict = new CityDistrict();
            return View(cityDistrict);
        }

        // POST: CityDistricts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,CATO,NameKK,NameRU,Area,Years,Populations")] CityDistrict cityDistrict)
        {
            if(cityDistrict.Years==null)
            {
                cityDistrict.Years = new int[0];
            }
            if (cityDistrict.Populations == null)
            {
                cityDistrict.Populations = new int[0];
            }
            List<int> years = new List<int>(),
                populations = new List<int>();
            for(int i=0;i<cityDistrict.Years.Count();i++)
            {
                if(cityDistrict.Years[i]!=0)
                {
                    years.Add(cityDistrict.Years[i]);
                    populations.Add(cityDistrict.Populations[i]);
                }
            }
            cityDistrict.Years = years.ToArray();
            cityDistrict.Populations = populations.ToArray();
            for(int i=0;i<cityDistrict.Years.Count();i++)
            {
                if ((cityDistrict.Years[i] < Constants.YearCityDistrictMin) || (cityDistrict.Years[i] > DateTime.Now.Year))
                {
                    ModelState.AddModelError($"Years[{i.ToString()}]", String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["Year"], Constants.YearCityDistrictMin.ToString(), DateTime.Now.Year.ToString()));
                }
            }
            var cityDistricts = _context.CityDistrict.AsNoTracking().ToList();
            if (cityDistricts.Select(c => c.NameKK).Contains(cityDistrict.CATO))
            {
                ModelState.AddModelError("CATO", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (cityDistricts.Select(c => c.NameKK).Contains(cityDistrict.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (cityDistricts.Select(c => c.NameRU).Contains(cityDistrict.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(cityDistrict.CATO))
            {
                ModelState.AddModelError("CATO", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(cityDistrict.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(cityDistrict.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                for(int i=0;i<cityDistrict.Years.Count();i++)
                {
                    for (int j = 0; j < cityDistrict.Years.Count()-1; j++)
                    {
                        if(cityDistrict.Years[j]> cityDistrict.Years[j+1])
                        {
                            int bufer = cityDistrict.Years[j];
                            cityDistrict.Years[j] = cityDistrict.Years[j + 1];
                            cityDistrict.Years[j + 1] = bufer;
                            bufer = cityDistrict.Populations[j];
                            cityDistrict.Populations[j] = cityDistrict.Populations[j + 1];
                            cityDistrict.Populations[j + 1] = bufer;
                        }
                    }
                }
                _context.Add(cityDistrict);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "CityDistrict",
                    New = cityDistrict.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cityDistrict);
        }

        // GET: CityDistricts/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cityDistrict = await _context.CityDistrict.SingleOrDefaultAsync(m => m.Id == id);
            if (cityDistrict == null)
            {
                return NotFound();
            }
            return View(cityDistrict);
        }

        // POST: CityDistricts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CATO,NameKK,NameRU,Area,Years,Populations")] CityDistrict cityDistrict)
        {
            if (id != cityDistrict.Id)
            {
                return NotFound();
            }
            if (cityDistrict.Years == null)
            {
                cityDistrict.Years = new int[0];
            }
            if (cityDistrict.Populations == null)
            {
                cityDistrict.Populations = new int[0];
            }
            List<int> years = new List<int>(),
                populations = new List<int>();
            for (int i = 0; i < cityDistrict.Years.Count(); i++)
            {
                if (cityDistrict.Years[i] != 0)
                {
                    years.Add(cityDistrict.Years[i]);
                    populations.Add(cityDistrict.Populations[i]);
                }
            }
            cityDistrict.Years = years.ToArray();
            cityDistrict.Populations = populations.ToArray();
            for (int i = 0; i < cityDistrict.Years.Count(); i++)
            {
                if ((cityDistrict.Years[i] < Constants.YearCityDistrictMin) || (cityDistrict.Years[i] > DateTime.Now.Year))
                {
                    ModelState.AddModelError($"Years[{i.ToString()}]", String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["Year"], Constants.YearCityDistrictMin.ToString(), DateTime.Now.Year.ToString()));
                }
            }
            var cityDistricts = _context.CityDistrict.AsNoTracking().ToList();
            if (cityDistricts.Where(c => c.Id != cityDistrict.Id).Select(c => c.NameKK).Contains(cityDistrict.CATO))
            {
                ModelState.AddModelError("CATO", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (cityDistricts.Where(c => c.Id != cityDistrict.Id).Select(c => c.NameKK).Contains(cityDistrict.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (cityDistricts.Where(c => c.Id != cityDistrict.Id).Select(c => c.NameRU).Contains(cityDistrict.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(cityDistrict.CATO))
            {
                ModelState.AddModelError("CATO", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(cityDistrict.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(cityDistrict.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                for (int i = 0; i < cityDistrict.Years.Count(); i++)
                {
                    for (int j = 0; j < cityDistrict.Years.Count() - 1; j++)
                    {
                        if (cityDistrict.Years[j] > cityDistrict.Years[j + 1])
                        {
                            int bufer = cityDistrict.Years[j];
                            cityDistrict.Years[j] = cityDistrict.Years[j + 1];
                            cityDistrict.Years[j + 1] = bufer;
                            bufer = cityDistrict.Populations[j];
                            cityDistrict.Populations[j] = cityDistrict.Populations[j + 1];
                            cityDistrict.Populations[j + 1] = bufer;
                        }
                    }
                }
                try
                {
                    var cityDistrict_old = _context.CityDistrict.AsNoTracking().FirstOrDefault(c => c.Id == cityDistrict.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "CityDistrict",
                        Operation = "Edit",
                        New = cityDistrict.ToString(),
                        Old = cityDistrict_old.ToString()
                    });
                    _context.Update(cityDistrict);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityDistrictExists(cityDistrict.Id))
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
            return View(cityDistrict);
        }

        // GET: CityDistricts/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cityDistrict = await _context.CityDistrict
                .SingleOrDefaultAsync(m => m.Id == id);
            if (cityDistrict == null)
            {
                return NotFound();
            }
            ViewBag.Companies = _context.Company
                .AsNoTracking()
                .Where(c => c.CityDistrictId == id)
                .OrderBy(c => c.AbbreviatedName);
            ViewBag.IndustrialSites = _context.IndustrialSite
                .AsNoTracking()
                .Where(i => i.CityDistrictId == id)
                .OrderBy(i => i.AbbreviatedName);
            ViewBag.SubsidiaryCompanies = _context.SubsidiaryCompany
                .AsNoTracking()
                .Where(s => s.CityDistrictId == id)
                .OrderBy(s => s.AbbreviatedName);
            ViewBag.TargetTerritories = _context.TargetTerritory
                .AsNoTracking()
                .Where(t => t.CityDistrictId == id)
                .OrderBy(t => t.TerritoryName);
            ViewBag.GreenPlantationsAreaAndSpeciesDiversities = _context.GreenPlantationsAreaAndSpeciesDiversity
                .AsNoTracking()
                .Where(t => t.CityDistrictId == id)
                .OrderBy(t => t.Year);
            ViewBag.GreenPlantationsStates = _context.GreenPlantationsState
                .AsNoTracking()
                .Where(t => t.CityDistrictId == id)
                .OrderBy(t => t.Name);
            return View(cityDistrict);
        }

        // POST: CityDistricts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cityDistrict = await _context.CityDistrict.SingleOrDefaultAsync(m => m.Id == id);
            if ((_context.Company
                .AsNoTracking()
                .FirstOrDefault(c => c.CityDistrictId == id) == null)
                &&(_context.IndustrialSite
                .AsNoTracking()
                .FirstOrDefault(i => i.CityDistrictId == id) == null)
                &&(_context.SubsidiaryCompany
                .AsNoTracking()
                .FirstOrDefault(s => s.CityDistrictId == id) == null)
                &&(_context.TargetTerritory
                .AsNoTracking()
                .FirstOrDefault(t => t.CityDistrictId == id) == null)
                &&(_context.GreenPlantationsAreaAndSpeciesDiversity
                .AsNoTracking()
                .FirstOrDefault(t => t.CityDistrictId == id) == null)
                &&(_context.GreenPlantationsState
                .AsNoTracking()
                .FirstOrDefault(t => t.CityDistrictId == id) == null))
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "CityDistrict",
                    Operation = "Delete",
                    New = "",
                    Old = cityDistrict.ToString()
                });
                _context.CityDistrict.Remove(cityDistrict);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CityDistrictExists(int id)
        {
            return _context.CityDistrict.Any(e => e.Id == id);
        }
    }
}
