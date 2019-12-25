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
    public class AnnualMaximumPermissibleEmissionsVolumesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public AnnualMaximumPermissibleEmissionsVolumesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: AnnualMaximumPermissibleEmissionsVolumes
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            int? CompanyId,
            string CompanyAbbreviatedName,
            int? Page)
        {
            //var applicationDbContext = _context.AnnualMaximumPermissibleEmissionsVolume.Include(a => a.Company).Include(a => a.IssuingPermitsStateAuthority).Include(a => a.SubsidiaryCompany);
            //return View(await applicationDbContext.ToListAsync());
            var annualMaximumPermissibleEmissionsVolumes = _context.AnnualMaximumPermissibleEmissionsVolume
                .Include(a => a.Company)
                .Include(a => a.SubsidiaryCompany)
                .Where(a => true);

            ViewBag.CompanyAbbreviatedNameFilter = CompanyAbbreviatedName;

            ViewBag.CompanyAbbreviatedNameSort = SortOrder == "CompanyAbbreviatedName" ? "CompanyAbbreviatedNameDesc" : "CompanyAbbreviatedName";
            ViewBag.YearOfPermitSort = SortOrder == "YearOfPermit" ? "YearOfPermitDesc" : "YearOfPermit";
            ViewBag.EmissionsTonsPerYearSort = SortOrder == "EmissionsTonsPerYear" ? "EmissionsTonsPerYearDesc" : "EmissionsTonsPerYear";

            if (!string.IsNullOrEmpty(CompanyAbbreviatedName))
            {
                annualMaximumPermissibleEmissionsVolumes = annualMaximumPermissibleEmissionsVolumes.Where(i => i.Company.AbbreviatedName.ToLower().Contains(CompanyAbbreviatedName.ToLower())
                    || i.SubsidiaryCompany.AbbreviatedName.ToLower().Contains(CompanyAbbreviatedName.ToLower()));
            }
            if (CompanyId!=null)
            {
                annualMaximumPermissibleEmissionsVolumes = annualMaximumPermissibleEmissionsVolumes.Where(i => i.CompanyId == CompanyId
                    || i.SubsidiaryCompanyId == CompanyId);
            }

            switch (SortOrder)
            {
                case "CompanyAbbreviatedName":
                    annualMaximumPermissibleEmissionsVolumes = annualMaximumPermissibleEmissionsVolumes.OrderBy(i => i.CompanyOrSubsidiaryCompanyAbbreviatedName);
                    break;
                case "CompanyAbbreviatedNameDesc":
                    annualMaximumPermissibleEmissionsVolumes = annualMaximumPermissibleEmissionsVolumes.OrderByDescending(i => i.CompanyOrSubsidiaryCompanyAbbreviatedName);
                    break;
                case "YearOfPermit":
                    annualMaximumPermissibleEmissionsVolumes = annualMaximumPermissibleEmissionsVolumes.OrderBy(i => i.YearOfPermit);
                    break;
                case "YearOfPermitDesc":
                    annualMaximumPermissibleEmissionsVolumes = annualMaximumPermissibleEmissionsVolumes.OrderByDescending(i => i.YearOfPermit);
                    break;
                case "EmissionsTonsPerYear":
                    annualMaximumPermissibleEmissionsVolumes = annualMaximumPermissibleEmissionsVolumes.OrderBy(i => i.EmissionsTonsPerYear);
                    break;
                case "EmissionsTonsPerYearDesc":
                    annualMaximumPermissibleEmissionsVolumes = annualMaximumPermissibleEmissionsVolumes.OrderByDescending(i => i.EmissionsTonsPerYear);
                    break;
                default:
                    annualMaximumPermissibleEmissionsVolumes = annualMaximumPermissibleEmissionsVolumes.OrderBy(i => i.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(annualMaximumPermissibleEmissionsVolumes.Count(), Page);

            var viewModel = new AnnualMaximumPermissibleEmissionsVolumeIndexPageViewModel
            {
                Items = annualMaximumPermissibleEmissionsVolumes.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewBag.HazardClassId = new SelectList(_context.HazardClass.OrderBy(h => h.Name), "Id", "Name");
            ViewBag.CityDistrictId = new SelectList(_context.CityDistrict.OrderBy(i => i.Name), "Id", "Name");

            return View(viewModel);
        }

        // GET: AnnualMaximumPermissibleEmissionsVolumes/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annualMaximumPermissibleEmissionsVolume = await _context.AnnualMaximumPermissibleEmissionsVolume
                .Include(a => a.Company)
                .Include(a => a.IssuingPermitsStateAuthority)
                .Include(a => a.SubsidiaryCompany)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (annualMaximumPermissibleEmissionsVolume == null)
            {
                return NotFound();
            }

            return View(annualMaximumPermissibleEmissionsVolume);
        }

        // GET: AnnualMaximumPermissibleEmissionsVolumes/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["IssuingPermitsStateAuthorityId"] = new SelectList(_context.IssuingPermitsStateAuthority.OrderBy(i => i.Name), "Id", "Name");
            ViewData["CompanyId"] = new SelectList(_context.Company.Where(c => c.GetType() == typeof(Company)).OrderBy(c => c.AbbreviatedName), "Id", "AbbreviatedName");
            ViewData["SubsidiaryCompanyId"] = new SelectList(_context.SubsidiaryCompany.Where(s => s.CompanyId == _context.Company.OrderBy(c => c.AbbreviatedName).FirstOrDefault().Id).OrderBy(s => s.AbbreviatedName), "Id", "AbbreviatedName");
            return View();
        }

        // POST: AnnualMaximumPermissibleEmissionsVolumes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,CompanyId,SubsidiaryCompanyId,IssuingPermitsStateAuthorityId,Year,Month,Day,EmissionsTonsPerYear,YearOfPermit")] AnnualMaximumPermissibleEmissionsVolume annualMaximumPermissibleEmissionsVolume)
        {
            while (annualMaximumPermissibleEmissionsVolume.Day > 28)
            {
                try
                {
                    annualMaximumPermissibleEmissionsVolume.DateOfIssueOfPermit = new DateTime(
                        annualMaximumPermissibleEmissionsVolume.Year,
                        annualMaximumPermissibleEmissionsVolume.Month,
                        annualMaximumPermissibleEmissionsVolume.Day);
                }
                catch
                {
                    annualMaximumPermissibleEmissionsVolume.Day--;
                }
            }
            try
            {
                annualMaximumPermissibleEmissionsVolume.DateOfIssueOfPermit = new DateTime(
                    annualMaximumPermissibleEmissionsVolume.Year,
                    annualMaximumPermissibleEmissionsVolume.Month,
                    annualMaximumPermissibleEmissionsVolume.Day);
            }
            catch
            {

            }
            if(annualMaximumPermissibleEmissionsVolume.DateOfIssueOfPermit!=null)
            {
                if ((annualMaximumPermissibleEmissionsVolume.DateOfIssueOfPermit < new DateTime(Constants.YearDataMin, 1, 1)) || (annualMaximumPermissibleEmissionsVolume.DateOfIssueOfPermit > DateTime.Today))
                {
                    ModelState.AddModelError("DateOfIssueOfPermit", String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["Year"], (new DateTime(Constants.YearDataMin, 1, 1)).ToShortDateString(), DateTime.Today.ToShortDateString()));
                }
            }            
            if (ModelState.IsValid)
            {
                _context.Add(annualMaximumPermissibleEmissionsVolume);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "AnnualMaximumPermissibleEmissionsVolume",
                    New = annualMaximumPermissibleEmissionsVolume.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            try
            {
                annualMaximumPermissibleEmissionsVolume.Year = annualMaximumPermissibleEmissionsVolume.DateOfIssueOfPermit.Year;
                annualMaximumPermissibleEmissionsVolume.Month = annualMaximumPermissibleEmissionsVolume.DateOfIssueOfPermit.Month;
                annualMaximumPermissibleEmissionsVolume.Day = annualMaximumPermissibleEmissionsVolume.DateOfIssueOfPermit.Day;
            }
            catch
            {

            }
            ViewData["IssuingPermitsStateAuthorityId"] = new SelectList(_context.IssuingPermitsStateAuthority.OrderBy(i => i.Name), "Id", "Name", annualMaximumPermissibleEmissionsVolume.IssuingPermitsStateAuthorityId);
            ViewData["CompanyId"] = new SelectList(_context.Company.Where(c => c.GetType() == typeof(Company)).OrderBy(c => c.AbbreviatedName), "Id", "AbbreviatedName", annualMaximumPermissibleEmissionsVolume.CompanyId);
            var subsidiaryCompanies = _context.SubsidiaryCompany.Where(s => s.CompanyId == annualMaximumPermissibleEmissionsVolume.CompanyId).OrderBy(s => s.AbbreviatedName).ToList();
            subsidiaryCompanies.Insert(0, new SubsidiaryCompany() { Id = 0, AbbreviatedName = "" });
            ViewData["SubsidiaryCompanyId"] = new SelectList(subsidiaryCompanies, "Id", "AbbreviatedName", annualMaximumPermissibleEmissionsVolume.SubsidiaryCompanyId);
            return View(annualMaximumPermissibleEmissionsVolume);
        }

        // GET: AnnualMaximumPermissibleEmissionsVolumes/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annualMaximumPermissibleEmissionsVolume = await _context.AnnualMaximumPermissibleEmissionsVolume.SingleOrDefaultAsync(m => m.Id == id);
            if (annualMaximumPermissibleEmissionsVolume == null)
            {
                return NotFound();
            }
            try
            {
                annualMaximumPermissibleEmissionsVolume.Year = annualMaximumPermissibleEmissionsVolume.DateOfIssueOfPermit.Year;
                annualMaximumPermissibleEmissionsVolume.Month = annualMaximumPermissibleEmissionsVolume.DateOfIssueOfPermit.Month;
                annualMaximumPermissibleEmissionsVolume.Day = annualMaximumPermissibleEmissionsVolume.DateOfIssueOfPermit.Day;
            }
            catch
            {

            }
            ViewData["IssuingPermitsStateAuthorityId"] = new SelectList(_context.IssuingPermitsStateAuthority.OrderBy(i => i.Name), "Id", "Name", annualMaximumPermissibleEmissionsVolume.IssuingPermitsStateAuthorityId);
            ViewData["CompanyId"] = new SelectList(_context.Company.Where(c => c.GetType() == typeof(Company)).OrderBy(c => c.AbbreviatedName), "Id", "AbbreviatedName", annualMaximumPermissibleEmissionsVolume.CompanyId);
            var subsidiaryCompanies = _context.SubsidiaryCompany.Where(s => s.CompanyId == annualMaximumPermissibleEmissionsVolume.CompanyId).OrderBy(s => s.AbbreviatedName).ToList();
            subsidiaryCompanies.Insert(0, new SubsidiaryCompany() { Id = 0, AbbreviatedName = "" });
            ViewData["SubsidiaryCompanyId"] = new SelectList(subsidiaryCompanies, "Id", "AbbreviatedName", annualMaximumPermissibleEmissionsVolume.SubsidiaryCompanyId);
            return View(annualMaximumPermissibleEmissionsVolume);
        }

        // POST: AnnualMaximumPermissibleEmissionsVolumes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyId,SubsidiaryCompanyId,IssuingPermitsStateAuthorityId,Year,Month,Day,EmissionsTonsPerYear,YearOfPermit")] AnnualMaximumPermissibleEmissionsVolume annualMaximumPermissibleEmissionsVolume)
        {
            if (id != annualMaximumPermissibleEmissionsVolume.Id)
            {
                return NotFound();
            }
            while (annualMaximumPermissibleEmissionsVolume.Day > 28)
            {
                try
                {
                    annualMaximumPermissibleEmissionsVolume.DateOfIssueOfPermit = new DateTime(
                        annualMaximumPermissibleEmissionsVolume.Year,
                        annualMaximumPermissibleEmissionsVolume.Month,
                        annualMaximumPermissibleEmissionsVolume.Day);
                }
                catch
                {
                    annualMaximumPermissibleEmissionsVolume.Day--;
                }
            }
            try
            {
                annualMaximumPermissibleEmissionsVolume.DateOfIssueOfPermit = new DateTime(
                    annualMaximumPermissibleEmissionsVolume.Year,
                    annualMaximumPermissibleEmissionsVolume.Month,
                    annualMaximumPermissibleEmissionsVolume.Day);
            }
            catch
            {

            }
            if (annualMaximumPermissibleEmissionsVolume.DateOfIssueOfPermit != null)
            {
                if ((annualMaximumPermissibleEmissionsVolume.DateOfIssueOfPermit < new DateTime(Constants.YearDataMin, 1, 1)) || (annualMaximumPermissibleEmissionsVolume.DateOfIssueOfPermit > DateTime.Today))
                {
                    ModelState.AddModelError("DateOfIssueOfPermit", String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["Year"], (new DateTime(Constants.YearDataMin, 1, 1)).ToShortDateString(), DateTime.Today.ToShortDateString()));
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var annualMaximumPermissibleEmissionsVolume_old = _context.AnnualMaximumPermissibleEmissionsVolume.AsNoTracking().FirstOrDefault(i => i.Id == annualMaximumPermissibleEmissionsVolume.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "AnnualMaximumPermissibleEmissionsVolume",
                        Operation = "Edit",
                        New = annualMaximumPermissibleEmissionsVolume.ToString(),
                        Old = annualMaximumPermissibleEmissionsVolume_old.ToString()
                    });
                    if(annualMaximumPermissibleEmissionsVolume.SubsidiaryCompanyId == 0)
                    {
                        annualMaximumPermissibleEmissionsVolume.SubsidiaryCompanyId = null;
                    }
                    _context.Update(annualMaximumPermissibleEmissionsVolume);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnualMaximumPermissibleEmissionsVolumeExists(annualMaximumPermissibleEmissionsVolume.Id))
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
            try
            {
                annualMaximumPermissibleEmissionsVolume.Year = annualMaximumPermissibleEmissionsVolume.DateOfIssueOfPermit.Year;
                annualMaximumPermissibleEmissionsVolume.Month = annualMaximumPermissibleEmissionsVolume.DateOfIssueOfPermit.Month;
                annualMaximumPermissibleEmissionsVolume.Day = annualMaximumPermissibleEmissionsVolume.DateOfIssueOfPermit.Day;
                }
            catch
            {

            }
            ViewData["IssuingPermitsStateAuthorityId"] = new SelectList(_context.IssuingPermitsStateAuthority.OrderBy(i => i.Name), "Id", "Name", annualMaximumPermissibleEmissionsVolume.IssuingPermitsStateAuthorityId);
            ViewData["CompanyId"] = new SelectList(_context.Company.Where(c => c.GetType() == typeof(Company)).OrderBy(c => c.AbbreviatedName), "Id", "AbbreviatedName", annualMaximumPermissibleEmissionsVolume.CompanyId);
            var subsidiaryCompanies = _context.SubsidiaryCompany.Where(s => s.CompanyId == annualMaximumPermissibleEmissionsVolume.CompanyId).OrderBy(s => s.AbbreviatedName).ToList();
            subsidiaryCompanies.Insert(0, new SubsidiaryCompany() { Id = 0, AbbreviatedName = "" });
            ViewData["SubsidiaryCompanyId"] = new SelectList(subsidiaryCompanies, "Id", "AbbreviatedName", annualMaximumPermissibleEmissionsVolume.SubsidiaryCompanyId);
            return View(annualMaximumPermissibleEmissionsVolume);
        }

        // GET: AnnualMaximumPermissibleEmissionsVolumes/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annualMaximumPermissibleEmissionsVolume = await _context.AnnualMaximumPermissibleEmissionsVolume
                .Include(a => a.Company)
                .Include(a => a.IssuingPermitsStateAuthority)
                .Include(a => a.SubsidiaryCompany)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (annualMaximumPermissibleEmissionsVolume == null)
            {
                return NotFound();
            }

            return View(annualMaximumPermissibleEmissionsVolume);
        }

        // POST: AnnualMaximumPermissibleEmissionsVolumes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var annualMaximumPermissibleEmissionsVolume = await _context.AnnualMaximumPermissibleEmissionsVolume.SingleOrDefaultAsync(m => m.Id == id);
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Class = "AnnualMaximumPermissibleEmissionsVolume",
                Operation = "Delete",
                New = "",
                Old = annualMaximumPermissibleEmissionsVolume.ToString()
            });
            _context.AnnualMaximumPermissibleEmissionsVolume.Remove(annualMaximumPermissibleEmissionsVolume);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnnualMaximumPermissibleEmissionsVolumeExists(int id)
        {
            return _context.AnnualMaximumPermissibleEmissionsVolume.Any(e => e.Id == id);
        }
    }
}
