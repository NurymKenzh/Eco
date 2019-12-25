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
    public class IndustrialSitesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public IndustrialSitesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: IndustrialSites
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            string CompanyAbbreviatedName,
            string AbbreviatedName,
            int? HazardClassId,
            int? CityDistrictId,
            int? Page)
        {
            var industrialSites = _context.IndustrialSite
                .Include(i => i.Company)
                .Include(i => i.SubsidiaryCompany)
                .Include(i => i.HazardClass)
                .Include(i => i.CityDistrict)
                .Where(i => true);

            ViewBag.CompanyAbbreviatedNameFilter = CompanyAbbreviatedName;
            ViewBag.AbbreviatedNameFilter = AbbreviatedName;
            ViewBag.HazardClassIdFilter = HazardClassId;
            ViewBag.CityDistrictIdFilter = CityDistrictId;

            ViewBag.CompanyAbbreviatedNameSort = SortOrder == "CompanyAbbreviatedName" ? "CompanyAbbreviatedNameDesc" : "CompanyAbbreviatedName";
            ViewBag.AbbreviatedNameSort = SortOrder == "AbbreviatedName" ? "AbbreviatedNameDesc" : "AbbreviatedName";
            ViewBag.HazardClassIdSort = SortOrder == "HazardClassId" ? "HazardClassIdDesc" : "HazardClassId";
            ViewBag.CityDistrictIdSort = SortOrder == "CityDistrictId" ? "CityDistrictIdDesc" : "CityDistrictId";

            if (!string.IsNullOrEmpty(CompanyAbbreviatedName))
            {
                industrialSites = industrialSites.Where(i => i.Company.AbbreviatedName.ToLower().Contains(CompanyAbbreviatedName.ToLower())
                    || i.SubsidiaryCompany.AbbreviatedName.ToLower().Contains(CompanyAbbreviatedName.ToLower()));
            }
            if (!string.IsNullOrEmpty(AbbreviatedName))
            {
                industrialSites = industrialSites.Where(i => i.CompanyOrSubsidiaryCompanyAbbreviatedName.ToLower().Contains(AbbreviatedName.ToLower()));
            }
            if (HazardClassId != null)
            {
                industrialSites = industrialSites.Where(i => i.HazardClassId == HazardClassId);
            }
            if (CityDistrictId != null)
            {
                industrialSites = industrialSites.Where(i => i.CityDistrictId == CityDistrictId);
            }

            switch (SortOrder)
            {
                case "CompanyAbbreviatedName":
                    industrialSites = industrialSites.OrderBy(i => i.CompanyOrSubsidiaryCompanyAbbreviatedName);
                    break;
                case "CompanyAbbreviatedNameDesc":
                    industrialSites = industrialSites.OrderByDescending(i => i.CompanyOrSubsidiaryCompanyAbbreviatedName);
                    break;
                case "AbbreviatedName":
                    industrialSites = industrialSites.OrderBy(i => i.AbbreviatedName);
                    break;
                case "AbbreviatedNameDesc":
                    industrialSites = industrialSites.OrderByDescending(i => i.AbbreviatedName);
                    break;
                case "HazardClassId":
                    industrialSites = industrialSites.OrderBy(i => i.HazardClass.Name);
                    break;
                case "HazardClassIdDesc":
                    industrialSites = industrialSites.OrderByDescending(i => i.HazardClass.Name);
                    break;
                case "CityDistrictId":
                    industrialSites = industrialSites.OrderBy(i => i.CityDistrict.Name);
                    break;
                case "CityDistrictIdDesc":
                    industrialSites = industrialSites.OrderByDescending(i => i.CityDistrict.Name);
                    break;
                default:
                    industrialSites = industrialSites.OrderBy(i => i.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(industrialSites.Count(), Page);

            var viewModel = new IndustrialSiteIndexPageViewModel
            {
                Items = industrialSites.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewBag.HazardClassId = new SelectList(_context.HazardClass.OrderBy(h => h.Name), "Id", "Name");
            ViewBag.CityDistrictId = new SelectList(_context.CityDistrict.OrderBy(i => i.Name), "Id", "Name");

            return View(viewModel);
        }

        // GET: IndustrialSites/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var industrialSite = await _context.IndustrialSite
                .Include(i => i.CityDistrict)
                .Include(i => i.Company)
                .Include(i => i.HazardClass)
                .Include(i => i.SubsidiaryCompany)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (industrialSite == null)
            {
                return NotFound();
            }

            return View(industrialSite);
        }

        // GET: IndustrialSites/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name");
            ViewData["HazardClassId"] = new SelectList(_context.HazardClass.OrderBy(h => h.Name), "Id", "Name");
            ViewData["CompanyId"] = new SelectList(_context.Company.Where(c => c.GetType() == typeof(Company)).OrderBy(c => c.AbbreviatedName), "Id", "AbbreviatedName");
            ViewData["SubsidiaryCompanyId"] = new SelectList(_context.SubsidiaryCompany.Where(s => s.CompanyId == _context.Company.OrderBy(c => c.AbbreviatedName).FirstOrDefault().Id).OrderBy(s => s.AbbreviatedName), "Id", "AbbreviatedName");
            return View();
        }

        // POST: IndustrialSites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,CompanyId,SubsidiaryCompanyId,FullName,AbbreviatedName,HazardClassId,CityDistrictId,Street,House,NorthLatitude,EastLongitude")] IndustrialSite industrialSite)
        {
            var industrialSites = _context.IndustrialSite.AsNoTracking()
                .Include(i => i.Company)
                .Include(i => i.SubsidiaryCompany)
                .Include(i => i.HazardClass)
                .Include(i => i.CityDistrict)
                .ToList();
            if (string.IsNullOrWhiteSpace(industrialSite.FullName))
            {
                ModelState.AddModelError("FullName", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(industrialSite.AbbreviatedName))
            {
                ModelState.AddModelError("AbbreviatedName", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(industrialSite.Street))
            {
                ModelState.AddModelError("Street", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(industrialSite.House))
            {
                ModelState.AddModelError("House", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (industrialSites.Select(i => i.FullName).Contains(industrialSite.FullName))
            {
                ModelState.AddModelError("FullName", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (industrialSites.Select(i => i.AbbreviatedName).Contains(industrialSite.AbbreviatedName))
            {
                ModelState.AddModelError("AbbreviatedName", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(industrialSite);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "IndustrialSite",
                    New = industrialSite.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name", industrialSite.CityDistrictId);
            ViewData["HazardClassId"] = new SelectList(_context.HazardClass.OrderBy(h => h.Name), "Id", "Name", industrialSite.HazardClassId);
            ViewData["CompanyId"] = new SelectList(_context.Company.Where(c => c.GetType() == typeof(Company)).OrderBy(c => c.AbbreviatedName), "Id", "AbbreviatedName", industrialSite.CompanyId);
            var subsidiaryCompanies = _context.SubsidiaryCompany.Where(s => s.CompanyId == industrialSite.CompanyId).OrderBy(s => s.AbbreviatedName).ToList();
            subsidiaryCompanies.Insert(0, new SubsidiaryCompany() { Id = 0, AbbreviatedName = "" });
            ViewData["SubsidiaryCompanyId"] = new SelectList(subsidiaryCompanies, "Id", "AbbreviatedName", industrialSite.SubsidiaryCompanyId);
            return View(industrialSite);
        }

        // GET: IndustrialSites/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var industrialSite = await _context.IndustrialSite.SingleOrDefaultAsync(m => m.Id == id);
            if (industrialSite == null)
            {
                return NotFound();
            }
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name", industrialSite.CityDistrictId);
            ViewData["HazardClassId"] = new SelectList(_context.HazardClass.OrderBy(h => h.Name), "Id", "Name", industrialSite.HazardClassId);
            ViewData["CompanyId"] = new SelectList(_context.Company.Where(c => c.GetType() == typeof(Company)).OrderBy(c => c.AbbreviatedName), "Id", "AbbreviatedName", industrialSite.CompanyId);
            var subsidiaryCompanies = _context.SubsidiaryCompany.Where(s => s.CompanyId == industrialSite.CompanyId).OrderBy(s => s.AbbreviatedName).ToList();
            subsidiaryCompanies.Insert(0, new SubsidiaryCompany() { Id = 0, AbbreviatedName = "" });
            ViewData["SubsidiaryCompanyId"] = new SelectList(subsidiaryCompanies, "Id", "AbbreviatedName", industrialSite.SubsidiaryCompanyId);
            return View(industrialSite);
        }

        // POST: IndustrialSites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyId,SubsidiaryCompanyId,FullName,AbbreviatedName,HazardClassId,CityDistrictId,Street,House,NorthLatitude,EastLongitude")] IndustrialSite industrialSite)
        {
            if (id != industrialSite.Id)
            {
                return NotFound();
            }
            var industrialSites = _context.IndustrialSite.AsNoTracking()
                .Where(i => i.Id != industrialSite.Id)
                .Include(i => i.Company)
                .Include(i => i.SubsidiaryCompany)
                .Include(i => i.HazardClass)
                .Include(i => i.CityDistrict)
                .ToList();
            if (string.IsNullOrWhiteSpace(industrialSite.FullName))
            {
                ModelState.AddModelError("FullName", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(industrialSite.AbbreviatedName))
            {
                ModelState.AddModelError("AbbreviatedName", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(industrialSite.Street))
            {
                ModelState.AddModelError("Street", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(industrialSite.House))
            {
                ModelState.AddModelError("House", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (industrialSites.Select(i => i.FullName).Contains(industrialSite.FullName))
            {
                ModelState.AddModelError("FullName", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (industrialSites.Select(i => i.AbbreviatedName).Contains(industrialSite.AbbreviatedName))
            {
                ModelState.AddModelError("AbbreviatedName", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var industrialSite_old = _context.IndustrialSite.AsNoTracking().FirstOrDefault(i => i.Id == industrialSite.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "IndustrialSite",
                        Operation = "Edit",
                        New = industrialSite.ToString(),
                        Old = industrialSite_old.ToString()
                    });
                    _context.Update(industrialSite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IndustrialSiteExists(industrialSite.Id))
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
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name", industrialSite.CityDistrictId);
            ViewData["HazardClassId"] = new SelectList(_context.HazardClass.OrderBy(h => h.Name), "Id", "Name", industrialSite.HazardClassId);
            ViewData["CompanyId"] = new SelectList(_context.Company.Where(c => c.GetType() == typeof(Company)).OrderBy(c => c.AbbreviatedName), "Id", "AbbreviatedName", industrialSite.CompanyId);
            var subsidiaryCompanies = _context.SubsidiaryCompany.Where(s => s.CompanyId == industrialSite.CompanyId).OrderBy(s => s.AbbreviatedName).ToList();
            subsidiaryCompanies.Insert(0, new SubsidiaryCompany() { Id = 0, AbbreviatedName = "" });
            ViewData["SubsidiaryCompanyId"] = new SelectList(subsidiaryCompanies, "Id", "AbbreviatedName", industrialSite.SubsidiaryCompanyId);
            return View(industrialSite);
        }

        // GET: IndustrialSites/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var industrialSite = await _context.IndustrialSite
                .Include(i => i.CityDistrict)
                .Include(i => i.Company)
                .Include(i => i.HazardClass)
                .Include(i => i.SubsidiaryCompany)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (industrialSite == null)
            {
                return NotFound();
            }

            return View(industrialSite);
        }

        // POST: IndustrialSites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var industrialSite = await _context.IndustrialSite.SingleOrDefaultAsync(m => m.Id == id);
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Class = "IndustrialSite",
                Operation = "Delete",
                New = "",
                Old = industrialSite.ToString()
            });
            _context.IndustrialSite.Remove(industrialSite);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IndustrialSiteExists(int id)
        {
            return _context.IndustrialSite.Any(e => e.Id == id);
        }

        [HttpPost]
        public JsonResult GetSubsidiaryCompaniesByCompanyId(int CompanyId)
        {
            var subsidiaryCompanies = _context.SubsidiaryCompany
                .Where(s => s.CompanyId == CompanyId).ToArray().OrderBy(s => s.AbbreviatedName);
            JsonResult result = new JsonResult(subsidiaryCompanies);
            return result;
        }
    }
}
