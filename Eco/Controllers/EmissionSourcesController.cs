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
    public class EmissionSourcesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public EmissionSourcesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: EmissionSources
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            string CompanyAbbreviatedName,
            string EmissionSourceName,
            int? Page)
        {
            var emissionSources = _context.EmissionSource
                .Include(e => e.Company)
                .Include(e => e.SubsidiaryCompany)
                .Where(e => true);

            ViewBag.CompanyAbbreviatedNameFilter = CompanyAbbreviatedName;
            ViewBag.EmissionSourceNameFilter = EmissionSourceName;

            ViewBag.CompanyAbbreviatedNameSort = SortOrder == "CompanyAbbreviatedName" ? "CompanyAbbreviatedNameDesc" : "CompanyAbbreviatedName";
            ViewBag.EmissionSourceNameSort = SortOrder == "EmissionSourceName" ? "EmissionSourceNameDesc" : "EmissionSourceName";

            if (!string.IsNullOrEmpty(CompanyAbbreviatedName))
            {
                emissionSources = emissionSources.Where(e => e.Company.AbbreviatedName==CompanyAbbreviatedName
                    || e.SubsidiaryCompany.AbbreviatedName==CompanyAbbreviatedName);
            }
            if (!string.IsNullOrEmpty(EmissionSourceName))
            {
                emissionSources = emissionSources.Where(e => e.EmissionSourceName.ToLower().Contains(EmissionSourceName.ToLower()));
            }

            switch (SortOrder)
            {
                case "CompanyAbbreviatedName":
                    emissionSources = emissionSources.OrderBy(e => e.CompanyOrSubsidiaryCompanyAbbreviatedName);
                    break;
                case "CompanyAbbreviatedNameDesc":
                    emissionSources = emissionSources.OrderByDescending(e => e.CompanyOrSubsidiaryCompanyAbbreviatedName);
                    break;
                case "EmissionSourceName":
                    emissionSources = emissionSources.OrderBy(e => e.EmissionSourceName);
                    break;
                case "EmissionSourceNameDesc":
                    emissionSources = emissionSources.OrderByDescending(e => e.EmissionSourceName);
                    break;
                default:
                    emissionSources = emissionSources.OrderBy(e => e.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(emissionSources.Count(), Page);

            var viewModel = new EmissionSourceIndexPageViewModel
            {
                Items = emissionSources.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewData["CompanyAbbreviatedName"] = new SelectList(_context.Company.OrderBy(a => a.AbbreviatedName).GroupBy(k => k.AbbreviatedName).Select(g => g.First()), "AbbreviatedName", "AbbreviatedName");

            return View(viewModel);
        }

        // GET: EmissionSources/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emissionSource = await _context.EmissionSource
                .Include(e => e.Company)
                .Include(e => e.EmissionSourceType)
                .Include(e => e.SubsidiaryCompany)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (emissionSource == null)
            {
                return NotFound();
            }

            return View(emissionSource);
        }

        // GET: EmissionSources/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            List<int> companiesWithEmissionIds = _context.AnnualMaximumPermissibleEmissionsVolume
                .Where(a => a.SubsidiaryCompanyId == null)
                .Select(a => a.CompanyId)
                .ToList();
            List<int> subsidiaryCompaniesWithEmissionIds = _context.AnnualMaximumPermissibleEmissionsVolume
                .Where(a => a.SubsidiaryCompanyId != null)
                .Select(a => (int)a.SubsidiaryCompanyId)
                .ToList();
            List<SubsidiaryCompany> subsidiaryCompaniesWithEmissions = _context.SubsidiaryCompany
                .Where(s => subsidiaryCompaniesWithEmissionIds.Contains(s.Id))
                .ToList();
            List<int> companiesWithSubsidiaryWithEmissionIds = _context.Company
                .Where(c => c.GetType() == typeof(Company))
                .Where(c => subsidiaryCompaniesWithEmissions.Select(s => s.CompanyId).Contains(c.Id))
                .Select(c => c.Id)
                .ToList();
            companiesWithEmissionIds.AddRange(subsidiaryCompaniesWithEmissionIds);
            ViewData["EmissionSourceTypeId"] = new SelectList(_context.EmissionSourceType.OrderBy(e => e.Name), "Id", "Name");
            ViewData["CompanyId"] = new SelectList(_context.Company
                .Where(c => c.GetType() == typeof(Company) && (companiesWithEmissionIds.Contains(c.Id) || companiesWithSubsidiaryWithEmissionIds.Contains(c.Id)))
                .OrderBy(c => c.AbbreviatedName),
                "Id", "AbbreviatedName");
            ViewData["SubsidiaryCompanyId"] = new SelectList(_context.SubsidiaryCompany.Where(s => s.CompanyId == _context.Company.OrderBy(c => c.AbbreviatedName).FirstOrDefault().Id && subsidiaryCompaniesWithEmissionIds.Contains(s.Id)).OrderBy(s => s.AbbreviatedName), "Id", "AbbreviatedName");
            return View();
        }

        // POST: EmissionSources/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,CompanyId,SubsidiaryCompanyId,EmissionSourceName,EmissionSourceMapNumber,WorkHoursPerYear,SourcesNumber,EmissionSourceTypeId,NorthLatitude1,EastLongitude1,NorthLatitude2,EastLongitude2,NorthLatitude3,EastLongitude3,NorthLatitude4,EastLongitude4,EmissionSourceHeight,LengthOfMouth,DiameterOfMouthOfPipesOrWidth,SpeedOfGasAirMixture,VolumeOfGasAirMixture,TemperatureOfMixture,NameOfGasTreatmentPlantsTypeAndMeasuresToReduceEmissions")] EmissionSource emissionSource)
        {
            EmissionSourceType emissionSourceType = _context.EmissionSourceType.FirstOrDefault(e => e.Id == emissionSource.EmissionSourceTypeId);
            if (emissionSourceType.PointsAmount == 2)
            {
                if(emissionSource.NorthLatitude2==null)
                {
                    ModelState.AddModelError("NorthLatitude2", _sharedLocalizer["ErrorNeedToInput"]);
                }
                if (emissionSource.EastLongitude2 == null)
                {
                    ModelState.AddModelError("EastLongitude2", _sharedLocalizer["ErrorNeedToInput"]);
                }
            }
            if (emissionSourceType.PointsAmount == 4)
            {
                if (emissionSource.NorthLatitude2 == null)
                {
                    ModelState.AddModelError("NorthLatitude2", _sharedLocalizer["ErrorNeedToInput"]);
                }
                if (emissionSource.EastLongitude2 == null)
                {
                    ModelState.AddModelError("EastLongitude2", _sharedLocalizer["ErrorNeedToInput"]);
                }
                if (emissionSource.NorthLatitude3 == null)
                {
                    ModelState.AddModelError("NorthLatitude3", _sharedLocalizer["ErrorNeedToInput"]);
                }
                if (emissionSource.EastLongitude3 == null)
                {
                    ModelState.AddModelError("EastLongitude3", _sharedLocalizer["ErrorNeedToInput"]);
                }
                if (emissionSource.NorthLatitude4 == null)
                {
                    ModelState.AddModelError("NorthLatitude4", _sharedLocalizer["ErrorNeedToInput"]);
                }
                if (emissionSource.EastLongitude4 == null)
                {
                    ModelState.AddModelError("EastLongitude4", _sharedLocalizer["ErrorNeedToInput"]);
                }
            }
            if (emissionSourceType.PointsAmount == 1)
            {
                if (emissionSource.LengthOfMouth == null)
                {
                    ModelState.AddModelError("LengthOfMouth", _sharedLocalizer["ErrorNeedToInput"]);
                }
            }
            if (emissionSourceType.NameRU == "Т (точечный)" || emissionSourceType.NameRU == "Л2 (линейный источник второго типа)" || emissionSourceType.NameRU == "П2 (площадной источник второго типа)")
            {
                if (emissionSource.DiameterOfMouthOfPipesOrWidth == null)
                {
                    ModelState.AddModelError("DiameterOfMouthOfPipesOrWidth", _sharedLocalizer["ErrorNeedToInput"]);
                }
            }
            if (emissionSourceType.NameRU == "П1 (площадной источник первого типа) ")
            {
                emissionSource.DiameterOfMouthOfPipesOrWidth = 0;
            }
            if (emissionSourceType.NameRU == "Т (точечный)" || emissionSourceType.NameRU == "Л1 (линейный источник первого типа)" || emissionSourceType.NameRU == "Л2 (линейный источник второго типа)" || emissionSourceType.NameRU == "П2 (площадной источник второго типа)")
            {
                if (emissionSource.SpeedOfGasAirMixture == null)
                {
                    ModelState.AddModelError("SpeedOfGasAirMixture", _sharedLocalizer["ErrorNeedToInput"]);
                }
                if (emissionSource.VolumeOfGasAirMixture == null)
                {
                    ModelState.AddModelError("VolumeOfGasAirMixture", _sharedLocalizer["ErrorNeedToInput"]);
                }
            }
            if (emissionSourceType.NameRU == "П1 (площадной источник первого типа)")
            {
                emissionSource.SpeedOfGasAirMixture = 0;
                emissionSource.VolumeOfGasAirMixture = 0;
            }
            if (ModelState.IsValid)
            {
                _context.Add(emissionSource);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "EmissionSource",
                    New = emissionSource.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            List<int> companiesWithEmissionIds = _context.AnnualMaximumPermissibleEmissionsVolume
                .Where(a => a.SubsidiaryCompanyId == null)
                .Select(a => a.CompanyId)
                .ToList();
            List<int> subsidiaryCompaniesWithEmissionIds = _context.AnnualMaximumPermissibleEmissionsVolume
                .Where(a => a.SubsidiaryCompanyId != null)
                .Select(a => (int)a.SubsidiaryCompanyId)
                .ToList();
            List<SubsidiaryCompany> subsidiaryCompaniesWithEmissions = _context.SubsidiaryCompany
                .Where(s => subsidiaryCompaniesWithEmissionIds.Contains(s.Id))
                .ToList();
            List<int> companiesWithSubsidiaryWithEmissionIds = _context.Company
                .Where(c => c.GetType() == typeof(Company))
                .Where(c => subsidiaryCompaniesWithEmissions.Select(s => s.CompanyId).Contains(c.Id))
                .Select(c => c.Id)
                .ToList();
            companiesWithEmissionIds.AddRange(subsidiaryCompaniesWithEmissionIds);
            ViewData["EmissionSourceTypeId"] = new SelectList(_context.EmissionSourceType.OrderBy(e => e.Name), "Id", "Name", emissionSource.EmissionSourceTypeId);
            ViewData["CompanyId"] = new SelectList(_context.Company
                .Where(c => c.GetType() == typeof(Company) && (companiesWithEmissionIds.Contains(c.Id) || companiesWithSubsidiaryWithEmissionIds.Contains(c.Id)))
                .OrderBy(c => c.AbbreviatedName),
                "Id", "AbbreviatedName", emissionSource.CompanyId);
            ViewData["SubsidiaryCompanyId"] = new SelectList(_context.SubsidiaryCompany
                .Where(s => s.CompanyId == _context.Company.OrderBy(c => c.AbbreviatedName).FirstOrDefault().Id && subsidiaryCompaniesWithEmissionIds.Contains(s.Id))
                .OrderBy(s => s.AbbreviatedName),
                "Id", "AbbreviatedName", emissionSource.SubsidiaryCompanyId);
            return View(emissionSource);
        }

        // GET: EmissionSources/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emissionSource = await _context.EmissionSource.SingleOrDefaultAsync(m => m.Id == id);
            if (emissionSource == null)
            {
                return NotFound();
            }
            List<int> companiesWithEmissionIds = _context.AnnualMaximumPermissibleEmissionsVolume
                .Where(a => a.SubsidiaryCompanyId == null)
                .Select(a => a.CompanyId)
                .ToList();
            List<int> subsidiaryCompaniesWithEmissionIds = _context.AnnualMaximumPermissibleEmissionsVolume
                .Where(a => a.SubsidiaryCompanyId != null)
                .Select(a => (int)a.SubsidiaryCompanyId)
                .ToList();
            List<SubsidiaryCompany> subsidiaryCompaniesWithEmissions = _context.SubsidiaryCompany
                .Where(s => subsidiaryCompaniesWithEmissionIds.Contains(s.Id))
                .ToList();
            List<int> companiesWithSubsidiaryWithEmissionIds = _context.Company
                .Where(c => c.GetType() == typeof(Company))
                .Where(c => subsidiaryCompaniesWithEmissions.Select(s => s.CompanyId).Contains(c.Id))
                .Select(c => c.Id)
                .ToList();
            companiesWithEmissionIds.AddRange(subsidiaryCompaniesWithEmissionIds);
            ViewData["EmissionSourceTypeId"] = new SelectList(_context.EmissionSourceType.OrderBy(e => e.Name), "Id", "Name", emissionSource.EmissionSourceTypeId);
            ViewData["CompanyId"] = new SelectList(_context.Company
                .Where(c => c.GetType() == typeof(Company) && (companiesWithEmissionIds.Contains(c.Id) || companiesWithSubsidiaryWithEmissionIds.Contains(c.Id)))
                .OrderBy(c => c.AbbreviatedName),
                "Id", "AbbreviatedName", emissionSource.CompanyId);
            ViewData["SubsidiaryCompanyId"] = new SelectList(_context.SubsidiaryCompany
                .Where(s => s.CompanyId == _context.Company.OrderBy(c => c.AbbreviatedName).FirstOrDefault().Id && subsidiaryCompaniesWithEmissionIds.Contains(s.Id))
                .OrderBy(s => s.AbbreviatedName),
                "Id", "AbbreviatedName", emissionSource.SubsidiaryCompanyId);
            ViewBag.PointsAmount = _context.EmissionSourceType.FirstOrDefault(e => e.Id == emissionSource.EmissionSourceTypeId).PointsAmount;
            return View(emissionSource);
        }

        // POST: EmissionSources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyId,SubsidiaryCompanyId,EmissionSourceName,EmissionSourceMapNumber,WorkHoursPerYear,SourcesNumber,EmissionSourceTypeId,NorthLatitude1,EastLongitude1,NorthLatitude2,EastLongitude2,NorthLatitude3,EastLongitude3,NorthLatitude4,EastLongitude4,EmissionSourceHeight,LengthOfMouth,DiameterOfMouthOfPipesOrWidth,SpeedOfGasAirMixture,VolumeOfGasAirMixture,TemperatureOfMixture,NameOfGasTreatmentPlantsTypeAndMeasuresToReduceEmissions")] EmissionSource emissionSource)
        {
            if (id != emissionSource.Id)
            {
                return NotFound();
            }
            EmissionSourceType emissionSourceType = _context.EmissionSourceType.FirstOrDefault(e => e.Id == emissionSource.EmissionSourceTypeId);
            if (emissionSourceType.PointsAmount == 2)
            {
                if (emissionSource.NorthLatitude2 == null)
                {
                    ModelState.AddModelError("NorthLatitude2", _sharedLocalizer["ErrorNeedToInput"]);
                }
                if (emissionSource.EastLongitude2 == null)
                {
                    ModelState.AddModelError("EastLongitude2", _sharedLocalizer["ErrorNeedToInput"]);
                }
            }
            if (emissionSourceType.PointsAmount == 4)
            {
                if (emissionSource.NorthLatitude2 == null)
                {
                    ModelState.AddModelError("NorthLatitude2", _sharedLocalizer["ErrorNeedToInput"]);
                }
                if (emissionSource.EastLongitude2 == null)
                {
                    ModelState.AddModelError("EastLongitude2", _sharedLocalizer["ErrorNeedToInput"]);
                }
                if (emissionSource.NorthLatitude3 == null)
                {
                    ModelState.AddModelError("NorthLatitude3", _sharedLocalizer["ErrorNeedToInput"]);
                }
                if (emissionSource.EastLongitude3 == null)
                {
                    ModelState.AddModelError("EastLongitude3", _sharedLocalizer["ErrorNeedToInput"]);
                }
                if (emissionSource.NorthLatitude4 == null)
                {
                    ModelState.AddModelError("NorthLatitude4", _sharedLocalizer["ErrorNeedToInput"]);
                }
                if (emissionSource.EastLongitude4 == null)
                {
                    ModelState.AddModelError("EastLongitude4", _sharedLocalizer["ErrorNeedToInput"]);
                }
            }
            if (emissionSourceType.PointsAmount == 1)
            {
                if (emissionSource.LengthOfMouth == null)
                {
                    ModelState.AddModelError("LengthOfMouth", _sharedLocalizer["ErrorNeedToInput"]);
                }
            }
            if (emissionSourceType.NameRU == "Т (точечный)" || emissionSourceType.NameRU == "Л2 (линейный источник второго типа)" || emissionSourceType.NameRU == "П2 (площадной источник второго типа)")
            {
                if (emissionSource.DiameterOfMouthOfPipesOrWidth == null)
                {
                    ModelState.AddModelError("DiameterOfMouthOfPipesOrWidth", _sharedLocalizer["ErrorNeedToInput"]);
                }
            }
            if (emissionSourceType.NameRU == "П1 (площадной источник первого типа) ")
            {
                emissionSource.DiameterOfMouthOfPipesOrWidth = 0;
            }
            if (emissionSourceType.NameRU == "Т (точечный)" || emissionSourceType.NameRU == "Л1 (линейный источник первого типа)" || emissionSourceType.NameRU == "Л2 (линейный источник второго типа)" || emissionSourceType.NameRU == "П2 (площадной источник второго типа)")
            {
                if (emissionSource.SpeedOfGasAirMixture == null)
                {
                    ModelState.AddModelError("SpeedOfGasAirMixture", _sharedLocalizer["ErrorNeedToInput"]);
                }
                if (emissionSource.VolumeOfGasAirMixture == null)
                {
                    ModelState.AddModelError("VolumeOfGasAirMixture", _sharedLocalizer["ErrorNeedToInput"]);
                }
            }
            if (emissionSourceType.NameRU == "П1 (площадной источник первого типа)")
            {
                emissionSource.SpeedOfGasAirMixture = 0;
                emissionSource.VolumeOfGasAirMixture = 0;
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var emissionSource_old = _context.EmissionSource.AsNoTracking().FirstOrDefault(a => a.Id == emissionSource.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "EmissionSource",
                        Operation = "Edit",
                        New = emissionSource.ToString(),
                        Old = emissionSource_old.ToString()
                    });
                    _context.Update(emissionSource);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmissionSourceExists(emissionSource.Id))
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
            List<int> companiesWithEmissionIds = _context.AnnualMaximumPermissibleEmissionsVolume
                .Where(a => a.SubsidiaryCompanyId == null)
                .Select(a => a.CompanyId)
                .ToList();
            List<int> subsidiaryCompaniesWithEmissionIds = _context.AnnualMaximumPermissibleEmissionsVolume
                .Where(a => a.SubsidiaryCompanyId != null)
                .Select(a => (int)a.SubsidiaryCompanyId)
                .ToList();
            List<SubsidiaryCompany> subsidiaryCompaniesWithEmissions = _context.SubsidiaryCompany
                .Where(s => subsidiaryCompaniesWithEmissionIds.Contains(s.Id))
                .ToList();
            List<int> companiesWithSubsidiaryWithEmissionIds = _context.Company
                .Where(c => c.GetType() == typeof(Company))
                .Where(c => subsidiaryCompaniesWithEmissions.Select(s => s.CompanyId).Contains(c.Id))
                .Select(c => c.Id)
                .ToList();
            companiesWithEmissionIds.AddRange(subsidiaryCompaniesWithEmissionIds);
            ViewData["EmissionSourceTypeId"] = new SelectList(_context.EmissionSourceType.OrderBy(e => e.Name), "Id", "Name", emissionSource.EmissionSourceTypeId);
            ViewData["CompanyId"] = new SelectList(_context.Company
                .Where(c => c.GetType() == typeof(Company) && (companiesWithEmissionIds.Contains(c.Id) || companiesWithSubsidiaryWithEmissionIds.Contains(c.Id)))
                .OrderBy(c => c.AbbreviatedName),
                "Id", "AbbreviatedName", emissionSource.CompanyId);
            ViewData["SubsidiaryCompanyId"] = new SelectList(_context.SubsidiaryCompany
                .Where(s => s.CompanyId == _context.Company.OrderBy(c => c.AbbreviatedName).FirstOrDefault().Id && subsidiaryCompaniesWithEmissionIds.Contains(s.Id))
                .OrderBy(s => s.AbbreviatedName),
                "Id", "AbbreviatedName", emissionSource.SubsidiaryCompanyId);
            ViewBag.PointsAmount = _context.EmissionSourceType.FirstOrDefault(e => e.Id == emissionSource.EmissionSourceTypeId).PointsAmount;
            return View(emissionSource);
        }

        // GET: EmissionSources/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emissionSource = await _context.EmissionSource
                .Include(e => e.Company)
                .Include(e => e.EmissionSourceType)
                .Include(e => e.SubsidiaryCompany)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (emissionSource == null)
            {
                return NotFound();
            }
            ViewBag.CompanyEmissionsValues = _context.CompanyEmissionsValue
                .AsNoTracking()
                .Where(a => a.EmissionSourceId == id)
                .Include(a =>a.AirContaminant)
                .OrderBy(a => a.AirContaminant.Name);
            return View(emissionSource);
        }

        // POST: EmissionSources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emissionSource = await _context.EmissionSource.SingleOrDefaultAsync(m => m.Id == id);
            if (_context.CompanyEmissionsValue
                .AsNoTracking()
                .FirstOrDefault(a => a.EmissionSourceId == id) == null)
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "EmissionSource",
                    Operation = "Delete",
                    New = "",
                    Old = emissionSource.ToString()
                });
                _context.EmissionSource.Remove(emissionSource);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EmissionSourceExists(int id)
        {
            return _context.EmissionSource.Any(e => e.Id == id);
        }

        [HttpPost]
        public JsonResult HasCompanyEmission(int CompanyId)
        {
            bool has = false;
            if(_context.AnnualMaximumPermissibleEmissionsVolume
                .Where(a => a.SubsidiaryCompanyId == null)
                .Select(a => a.CompanyId)
                .Contains(CompanyId))
            {
                has = true;
            }
            JsonResult result = new JsonResult(has);
            return result;
        }

        [HttpPost]
        public JsonResult GetSubsidiaryCompaniesByCompanyId(int CompanyId)
        {
            List<int> subsidiaryCompaniesWithEmissionIds = _context.AnnualMaximumPermissibleEmissionsVolume
                .Where(a => a.SubsidiaryCompanyId != null)
                .Select(a => (int)a.SubsidiaryCompanyId)
                .ToList();
            var subsidiaryCompanies = _context.SubsidiaryCompany
                .Where(s => s.CompanyId == CompanyId && subsidiaryCompaniesWithEmissionIds.Contains(s.Id)).ToArray().OrderBy(s => s.AbbreviatedName);
            JsonResult result = new JsonResult(subsidiaryCompanies);
            return result;
        }

        [HttpPost]
        public JsonResult GetEmissionSourceTypePointsAmountById(int EmissionSourceTypeId)
        {
            int pointsAmount = _context.EmissionSourceType.FirstOrDefault(e => e.Id == EmissionSourceTypeId).PointsAmount;
            JsonResult result = new JsonResult(pointsAmount);
            return result;
        }
    }
}
