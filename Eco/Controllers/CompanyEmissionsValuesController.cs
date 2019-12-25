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
    public class CompanyEmissionsValuesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public CompanyEmissionsValuesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: CompanyEmissionsValues
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            string CompanyAbbreviatedName,
            string EmissionSourceName,
            int? AirContaminantId,
            int? Page)
        {
            var companyEmissionsValues = _context.CompanyEmissionsValue
                .Include(c => c.AirContaminant)
                .Include(c => c.EmissionSource)
                .Include(c => c.EmissionSource.Company)
                .Include(c => c.EmissionSource.SubsidiaryCompany)
                .Where(c => true);

            ViewBag.CompanyAbbreviatedNameFilter = CompanyAbbreviatedName;
            ViewBag.EmissionSourceNameFilter = EmissionSourceName;
            ViewBag.AirContaminantIdFilter = AirContaminantId;

            ViewBag.CompanyAbbreviatedNameSort = SortOrder == "CompanyAbbreviatedName" ? "CompanyAbbreviatedNameDesc" : "CompanyAbbreviatedName";
            ViewBag.EmissionSourceNameSort = SortOrder == "EmissionSourceName" ? "EmissionSourceNameDesc" : "EmissionSourceName";
            ViewBag.AirContaminantNameSort = SortOrder == "AirContaminantName" ? "AirContaminantNameDesc" : "AirContaminantName";

            if (!string.IsNullOrEmpty(CompanyAbbreviatedName))
            {
                companyEmissionsValues = companyEmissionsValues.Where(c => c.EmissionSource.Company.AbbreviatedName==CompanyAbbreviatedName
                    || c.EmissionSource.SubsidiaryCompany.AbbreviatedName==CompanyAbbreviatedName);
            }
            if (!string.IsNullOrEmpty(EmissionSourceName))
            {
                companyEmissionsValues = companyEmissionsValues.Where(c => c.EmissionSource.EmissionSourceName.ToLower().Contains(EmissionSourceName.ToLower()));
            }
            if (AirContaminantId != null)
            {
                companyEmissionsValues = companyEmissionsValues.Where(c => c.AirContaminantId == AirContaminantId);
            }

            switch (SortOrder)
            {
                case "CompanyAbbreviatedName":
                    companyEmissionsValues = companyEmissionsValues.OrderBy(c => c.EmissionSource.CompanyOrSubsidiaryCompanyAbbreviatedName);
                    break;
                case "CompanyAbbreviatedNameDesc":
                    companyEmissionsValues = companyEmissionsValues.OrderByDescending(c => c.EmissionSource.CompanyOrSubsidiaryCompanyAbbreviatedName);
                    break;
                case "EmissionSourceName":
                    companyEmissionsValues = companyEmissionsValues.OrderBy(c => c.EmissionSource.EmissionSourceName);
                    break;
                case "EmissionSourceNameDesc":
                    companyEmissionsValues = companyEmissionsValues.OrderByDescending(c => c.EmissionSource.EmissionSourceName);
                    break;
                case "AirContaminantName":
                    companyEmissionsValues = companyEmissionsValues.OrderBy(c => c.AirContaminant.Name);
                    break;
                case "AirContaminantNameDesc":
                    companyEmissionsValues = companyEmissionsValues.OrderByDescending(c => c.AirContaminant.Name);
                    break;
                default:
                    companyEmissionsValues = companyEmissionsValues.OrderBy(c => c.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(companyEmissionsValues.Count(), Page);

            var viewModel = new CompanyEmissionsValueIndexPageViewModel
            {
                Items = companyEmissionsValues.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            //ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name");
            ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.Where(a => _context.CompanyEmissionsValue.Include(k => k.AirContaminant).Select(k => k.AirContaminantId).Contains(a.Id)).OrderBy(a => a.Name), "Id", "Name");
            ViewData["CompanyAbbreviatedName"] = new SelectList(_context.Company.OrderBy(a => a.AbbreviatedName).GroupBy(k => k.AbbreviatedName).Select(g => g.First()), "AbbreviatedName", "AbbreviatedName");

            return View(viewModel);
        }

        // GET: CompanyEmissionsValues/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyEmissionsValue = await _context.CompanyEmissionsValue
                .Include(c => c.AirContaminant)
                .Include(c => c.EmissionSource)
                .Include(c => c.EmissionSource.Company)
                .Include(c => c.EmissionSource.SubsidiaryCompany)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (companyEmissionsValue == null)
            {
                return NotFound();
            }

            return View(companyEmissionsValue);
        }

        // GET: CompanyEmissionsValues/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name");
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
            var companies = _context.Company
                .Where(c => c.GetType() == typeof(Company) && (companiesWithEmissionIds.Contains(c.Id) || companiesWithSubsidiaryWithEmissionIds.Contains(c.Id)))
                .OrderBy(c => c.AbbreviatedName)
                .ToList();
            ViewData["CompanyId"] = new SelectList(companies,
                "Id", "AbbreviatedName");
            var subsidiaryCompanies = _context.SubsidiaryCompany
                .Where(s => s.CompanyId == _context.Company.OrderBy(c => c.AbbreviatedName).FirstOrDefault().Id && subsidiaryCompaniesWithEmissionIds.Contains(s.Id))
                .OrderBy(s => s.AbbreviatedName)
                .ToList();
            ViewData["SubsidiaryCompanyId"] = new SelectList(subsidiaryCompanies, "Id", "AbbreviatedName");
            var emissionSources = _context.EmissionSource
                .Where(e => e.CompanyId == companies.FirstOrDefault().Id)
                .OrderBy(e => e.EmissionSourceName)
                .ToList();
            if(subsidiaryCompanies.Count()>0)
            {
                emissionSources = _context.EmissionSource
                    .Where(e => e.SubsidiaryCompanyId == subsidiaryCompanies.FirstOrDefault().Id)
                    .OrderBy(e => e.EmissionSourceName)
                    .ToList();
            }
            ViewData["EmissionSourceId"] = new SelectList(emissionSources, "Id", "EmissionSourceName");
            return View();
        }

        // POST: CompanyEmissionsValues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,EmissionSourceId,AirContaminantId,ValuesMaximumPermissibleEmissionsgs,ValuesMaximumPermissibleEmissionstyear,ValuesMaximumPermissibleEmissionsmgm3,YearOfAchievementMaximumPermissibleEmissions,CoefficientOfGasCleaningPlanned,CoefficientOfGasCleaningActual,AverageOperatingDegreeOfPurification,MaximumDegreeOfPurification")] CompanyEmissionsValue companyEmissionsValue)
        {
            if(companyEmissionsValue.EmissionSourceId==0)
            {
                ModelState.AddModelError("EmissionSourceId", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(companyEmissionsValue);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "CompanyEmissionsValue",
                    New = companyEmissionsValue.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name", companyEmissionsValue.AirContaminantId);
            companyEmissionsValue.EmissionSource = _context.EmissionSource.FirstOrDefault(e => e.Id == companyEmissionsValue.EmissionSourceId);
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
            var companies = _context.Company
                .Where(c => c.GetType() == typeof(Company) && (companiesWithEmissionIds.Contains(c.Id) || companiesWithSubsidiaryWithEmissionIds.Contains(c.Id)))
                .OrderBy(c => c.AbbreviatedName);
            ViewData["CompanyId"] = new SelectList(companies,
                "Id", "AbbreviatedName", companyEmissionsValue.EmissionSource?.CompanyId);
            var subsidiaryCompanies = _context.SubsidiaryCompany
                .Where(s => s.CompanyId == _context.Company.OrderBy(c => c.AbbreviatedName).FirstOrDefault().Id && subsidiaryCompaniesWithEmissionIds.Contains(s.Id))
                .OrderBy(s => s.AbbreviatedName);
            ViewData["SubsidiaryCompanyId"] = new SelectList(subsidiaryCompanies, "Id", "AbbreviatedName", companyEmissionsValue.EmissionSource?.SubsidiaryCompanyId);
            var emissionSources = _context.EmissionSource
                .Where(e => e.CompanyId == companies.FirstOrDefault().Id)
                .OrderBy(e => e.EmissionSourceName);
            if (subsidiaryCompanies.Count() > 0)
            {
                emissionSources = _context.EmissionSource
                    .Where(e => e.SubsidiaryCompanyId == subsidiaryCompanies.FirstOrDefault().Id)
                    .OrderBy(e => e.EmissionSourceName);
            }
            ViewData["EmissionSourceId"] = new SelectList(emissionSources, "Id", "EmissionSourceName", companyEmissionsValue.EmissionSourceId);
            return View(companyEmissionsValue);
        }

        // GET: CompanyEmissionsValues/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyEmissionsValue = await _context.CompanyEmissionsValue.SingleOrDefaultAsync(m => m.Id == id);
            if (companyEmissionsValue == null)
            {
                return NotFound();
            }
            ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name", companyEmissionsValue.AirContaminantId);
            companyEmissionsValue.EmissionSource = _context.EmissionSource.FirstOrDefault(e => e.Id == companyEmissionsValue.EmissionSourceId);
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
            var companies = _context.Company
                .Where(c => c.GetType() == typeof(Company) && (companiesWithEmissionIds.Contains(c.Id) || companiesWithSubsidiaryWithEmissionIds.Contains(c.Id)))
                .OrderBy(c => c.AbbreviatedName);
            ViewData["CompanyId"] = new SelectList(companies,
                "Id", "AbbreviatedName", companyEmissionsValue.EmissionSource?.CompanyId);
            var subsidiaryCompanies = _context.SubsidiaryCompany
                .Where(s => s.CompanyId == companyEmissionsValue.EmissionSource.CompanyId && subsidiaryCompaniesWithEmissionIds.Contains(s.Id))
                .OrderBy(s => s.AbbreviatedName);
            ViewData["SubsidiaryCompanyId"] = new SelectList(subsidiaryCompanies, "Id", "AbbreviatedName", companyEmissionsValue.EmissionSource?.SubsidiaryCompanyId);
            var emissionSources = _context.EmissionSource
                .Where(e => e.CompanyId == companies.FirstOrDefault().Id)
                .OrderBy(e => e.EmissionSourceName);
            if (subsidiaryCompanies.Count() > 0)
            {
                emissionSources = _context.EmissionSource
                    .Where(e => e.SubsidiaryCompanyId == subsidiaryCompanies.FirstOrDefault().Id)
                    .OrderBy(e => e.EmissionSourceName);
            }
            ViewData["EmissionSourceId"] = new SelectList(emissionSources, "Id", "EmissionSourceName", companyEmissionsValue.EmissionSourceId);
            return View(companyEmissionsValue);
        }

        // POST: CompanyEmissionsValues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmissionSourceId,AirContaminantId,ValuesMaximumPermissibleEmissionsgs,ValuesMaximumPermissibleEmissionstyear,ValuesMaximumPermissibleEmissionsmgm3,YearOfAchievementMaximumPermissibleEmissions,CoefficientOfGasCleaningPlanned,CoefficientOfGasCleaningActual,AverageOperatingDegreeOfPurification,MaximumDegreeOfPurification")] CompanyEmissionsValue companyEmissionsValue)
        {
            if (id != companyEmissionsValue.Id)
            {
                return NotFound();
            }
            if (companyEmissionsValue.EmissionSourceId == 0)
            {
                ModelState.AddModelError("EmissionSourceId", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var companyEmissionsValue_old = _context.CompanyEmissionsValue.AsNoTracking().FirstOrDefault(a => a.Id == companyEmissionsValue.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "CompanyEmissionsValue",
                        Operation = "Edit",
                        New = companyEmissionsValue.ToString(),
                        Old = companyEmissionsValue_old.ToString()
                    });
                    _context.Update(companyEmissionsValue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyEmissionsValueExists(companyEmissionsValue.Id))
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
            ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name", companyEmissionsValue.AirContaminantId);
            companyEmissionsValue.EmissionSource = _context.EmissionSource.FirstOrDefault(e => e.Id == companyEmissionsValue.EmissionSourceId);
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
            var companies = _context.Company
                .Where(c => c.GetType() == typeof(Company) && (companiesWithEmissionIds.Contains(c.Id) || companiesWithSubsidiaryWithEmissionIds.Contains(c.Id)))
                .OrderBy(c => c.AbbreviatedName);
            ViewData["CompanyId"] = new SelectList(companies,
                "Id", "AbbreviatedName", companyEmissionsValue.EmissionSource?.CompanyId);
            var subsidiaryCompanies = _context.SubsidiaryCompany
                .Where(s => s.CompanyId == companyEmissionsValue.EmissionSource.CompanyId && subsidiaryCompaniesWithEmissionIds.Contains(s.Id))
                .OrderBy(s => s.AbbreviatedName);
            ViewData["SubsidiaryCompanyId"] = new SelectList(subsidiaryCompanies, "Id", "AbbreviatedName", companyEmissionsValue.EmissionSource?.SubsidiaryCompanyId);
            var emissionSources = _context.EmissionSource
                .Where(e => e.CompanyId == companies.FirstOrDefault().Id)
                .OrderBy(e => e.EmissionSourceName);
            if (subsidiaryCompanies.Count() > 0)
            {
                emissionSources = _context.EmissionSource
                    .Where(e => e.SubsidiaryCompanyId == subsidiaryCompanies.FirstOrDefault().Id)
                    .OrderBy(e => e.EmissionSourceName);
            }
            ViewData["EmissionSourceId"] = new SelectList(emissionSources, "Id", "EmissionSourceName", companyEmissionsValue.EmissionSourceId);
            return View(companyEmissionsValue);
        }

        // GET: CompanyEmissionsValues/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyEmissionsValue = await _context.CompanyEmissionsValue
                .Include(c => c.AirContaminant)
                .Include(c => c.EmissionSource)
                .Include(c => c.EmissionSource.Company)
                .Include(c => c.EmissionSource.SubsidiaryCompany)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (companyEmissionsValue == null)
            {
                return NotFound();
            }

            return View(companyEmissionsValue);
        }

        // POST: CompanyEmissionsValues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companyEmissionsValue = await _context.CompanyEmissionsValue.SingleOrDefaultAsync(m => m.Id == id);
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Class = "CompanyEmissionsValue",
                Operation = "Delete",
                New = "",
                Old = companyEmissionsValue.ToString()
            });
            _context.CompanyEmissionsValue.Remove(companyEmissionsValue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyEmissionsValueExists(int id)
        {
            return _context.CompanyEmissionsValue.Any(e => e.Id == id);
        }

        [HttpPost]
        public JsonResult GetEmissionSourcesByCompanyOrSubsidiaryId(int CompanySubsidiaryId)
        {
            var emissionSources = _context.EmissionSource
                .Where(e => e.CompanyId == CompanySubsidiaryId || e.SubsidiaryCompanyId == CompanySubsidiaryId).ToArray().OrderBy(e => e.EmissionSourceName);
            JsonResult result = new JsonResult(emissionSources);
            return result;
        }
    }
}
