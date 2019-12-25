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
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public CompaniesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: Companies
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            string AbbreviatedName,
            string BIK,
            string KindOfActivity,
            int? HazardClassId,
            int? CityDistrictId,
            int? Page)
        {
            var companies = _context.Company
                //.Where(c => c.GetType() == typeof(Company))
                .Include(c => c.HazardClass)
                .Include(c => c.CityDistrict)
                .Where(c => true);

            ViewBag.AbbreviatedNameFilter = AbbreviatedName;
            ViewBag.BIKFilter = BIK;
            ViewBag.KindOfActivityFilter = KindOfActivity;
            ViewBag.HazardClassIdFilter = HazardClassId;
            ViewBag.CityDistrictIdFilter = CityDistrictId;

            ViewBag.AbbreviatedNameSort = SortOrder == "AbbreviatedName" ? "AbbreviatedNameDesc" : "AbbreviatedName";
            ViewBag.BIKSort = SortOrder == "BIK" ? "BIKDesc" : "BIK";
            ViewBag.KindOfActivitySort = SortOrder == "KindOfActivity" ? "KindOfActivityDesc" : "KindOfActivity";
            ViewBag.HazardClassIdSort = SortOrder == "HazardClassId" ? "HazardClassIdDesc" : "HazardClassId";
            ViewBag.CityDistrictIdSort = SortOrder == "CityDistrictId" ? "CityDistrictIdDesc" : "CityDistrictId";

            if (!string.IsNullOrEmpty(AbbreviatedName))
            {
                companies = companies.Where(c => c.AbbreviatedName.ToLower().Contains(AbbreviatedName.ToLower()));
            }
            if (!string.IsNullOrEmpty(BIK))
            {
                companies = companies.Where(c => c.BIK.ToLower().Contains(BIK.ToLower()));
            }
            if (!string.IsNullOrEmpty(KindOfActivity))
            {
                companies = companies.Where(c => c.KindOfActivity.ToLower().Contains(KindOfActivity.ToLower()));
            }
            if (HazardClassId != null)
            {
                companies = companies.Where(c => c.HazardClassId == HazardClassId);
            }
            if (CityDistrictId != null)
            {
                companies = companies.Where(c => c.CityDistrictId == CityDistrictId);
            }

            switch (SortOrder)
            {
                case "AbbreviatedName":
                    companies = companies.OrderBy(c => c.AbbreviatedName);
                    break;
                case "AbbreviatedNameDesc":
                    companies = companies.OrderByDescending(c => c.AbbreviatedName);
                    break;
                case "BIK":
                    companies = companies.OrderBy(c => c.BIK);
                    break;
                case "BIKDesc":
                    companies = companies.OrderByDescending(c => c.BIK);
                    break;
                case "KindOfActivity":
                    companies = companies.OrderBy(c => c.KindOfActivity);
                    break;
                case "KindOfActivityDesc":
                    companies = companies.OrderByDescending(c => c.KindOfActivity);
                    break;
                case "HazardClassId":
                    companies = companies.OrderBy(c => c.HazardClass.Name);
                    break;
                case "HazardClassIdDesc":
                    companies = companies.OrderByDescending(c => c.HazardClass.Name);
                    break;
                case "CityDistrictId":
                    companies = companies.OrderBy(c => c.CityDistrict.Name);
                    break;
                case "CityDistrictIdDesc":
                    companies = companies.OrderByDescending(c => c.CityDistrict.Name);
                    break;
                default:
                    companies = companies.OrderBy(c => c.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(companies.Count(), Page);

            var viewModel = new CompanyIndexPageViewModel
            {
                Items = companies.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewBag.HazardClassId = new SelectList(_context.HazardClass.OrderBy(h => h.Name), "Id", "Name");
            ViewBag.CityDistrictId = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name");

            ViewData["AbbreviatedName"] = new SelectList(_context.Company.OrderBy(a => a.AbbreviatedName).GroupBy(k => k.AbbreviatedName).Select(g => g.First()), "AbbreviatedName", "AbbreviatedName");

            return View(viewModel);
        }

        // GET: Companies/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company
                .Include(c => c.CityDistrict)
                .Include(c => c.HazardClass)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            //if (company.GetType() != typeof(Company))
            //{
            //    return NotFound();
            //}

            return View(company);
        }

        // GET: Companies/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name");
            ViewData["HazardClassId"] = new SelectList(_context.HazardClass.OrderBy(h => h.Name), "Id", "Name");

            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,FullName,AbbreviatedName,BIK,KindOfActivity,HazardClassId,HierarchicalStructure,CityDistrictId,LegalAddress,ActualAddress,AdditionalInformation,NorthLatitude,EastLongitude")] Company company)
        {
            var companies = _context.Company.AsNoTracking().Include(c => c.HazardClass).Include(c => c.CityDistrict).ToList();
            if (string.IsNullOrWhiteSpace(company.FullName))
            {
                ModelState.AddModelError("FullName", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(company.AbbreviatedName))
            {
                ModelState.AddModelError("AbbreviatedName", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(company.BIK))
            {
                ModelState.AddModelError("BIK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(company.KindOfActivity))
            {
                ModelState.AddModelError("KindOfActivity", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (companies.Select(c => c.FullName).Contains(company.FullName))
            {
                ModelState.AddModelError("FullName", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (companies.Select(c => c.AbbreviatedName).Contains(company.AbbreviatedName))
            {
                ModelState.AddModelError("AbbreviatedName", _sharedLocalizer["ErrorDublicateValue"]);
            }
            //if (companies.Select(c => c.BIK).Contains(company.BIK))
            //{
            //    ModelState.AddModelError("BIK", _sharedLocalizer["ErrorDublicateValue"]);
            //}
            if (ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "Company",
                    New = company.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name", company.CityDistrictId);
            ViewData["HazardClassId"] = new SelectList(_context.HazardClass.OrderBy(h => h.Name), "Id", "Name", company.HazardClassId);
            return View(company);
        }

        // GET: Companies/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company.SingleOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            //if (company.GetType() != typeof(Company))
            //{
            //    return NotFound();
            //}
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name", company.CityDistrictId);
            ViewData["HazardClassId"] = new SelectList(_context.HazardClass.OrderBy(h => h.Name), "Id", "Name", company.HazardClassId);
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,AbbreviatedName,BIK,KindOfActivity,HazardClassId,HierarchicalStructure,CityDistrictId,LegalAddress,ActualAddress,AdditionalInformation,NorthLatitude,EastLongitude")] Company company)
        {
            if (id != company.Id)
            {
                return NotFound();
            }
            var companies = _context.Company.AsNoTracking().Include(c => c.HazardClass).Include(c => c.CityDistrict).Where(c => c.Id != company.Id).ToList();
            if (string.IsNullOrWhiteSpace(company.FullName))
            {
                ModelState.AddModelError("FullName", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(company.AbbreviatedName))
            {
                ModelState.AddModelError("AbbreviatedName", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(company.BIK))
            {
                ModelState.AddModelError("BIK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(company.KindOfActivity))
            {
                ModelState.AddModelError("KindOfActivity", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (companies.Select(c => c.FullName).Contains(company.FullName))
            {
                ModelState.AddModelError("FullName", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (companies.Select(c => c.AbbreviatedName).Contains(company.AbbreviatedName))
            {
                ModelState.AddModelError("AbbreviatedName", _sharedLocalizer["ErrorDublicateValue"]);
            }
            //if (companies.Select(c => c.BIK).Contains(company.BIK))
            //{
            //    ModelState.AddModelError("BIK", _sharedLocalizer["ErrorDublicateValue"]);
            //}
            if (ModelState.IsValid)
            {
                try
                {
                    var company_old = _context.Company.AsNoTracking().FirstOrDefault(c => c.Id == company.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "Company",
                        Operation = "Edit",
                        New = company.ToString(),
                        Old = company_old.ToString()
                    });
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
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
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name", company.CityDistrictId);
            ViewData["HazardClassId"] = new SelectList(_context.HazardClass.OrderBy(h => h.Name), "Id", "Name", company.HazardClassId);
            return View(company);
        }

        // GET: Companies/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company
                .Include(c => c.CityDistrict)
                .Include(c => c.HazardClass)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            //if (company.GetType() != typeof(Company))
            //{
            //    return NotFound();
            //}
            ViewBag.IndustrialSites = _context.IndustrialSite
                .AsNoTracking()
                .Where(i => i.CompanyId == id)
                .OrderBy(i => i.AbbreviatedName);
            ViewBag.SubsidiaryCompanies = _context.SubsidiaryCompany
                .AsNoTracking()
                .Where(s => s.CompanyId == id)
                .OrderBy(s => s.AbbreviatedName);
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _context.Company.SingleOrDefaultAsync(m => m.Id == id);
            if ((_context.IndustrialSite
                .AsNoTracking()
                .FirstOrDefault(i => i.CompanyId == id) == null)
                && (_context.SubsidiaryCompany
                .AsNoTracking()
                .FirstOrDefault(s => s.CompanyId == id) == null))
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "Company",
                    Operation = "Delete",
                    New = "",
                    Old = company.ToString()
                });
                _context.Company.Remove(company);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
            return _context.Company.Any(e => e.Id == id);
        }
    }
}
