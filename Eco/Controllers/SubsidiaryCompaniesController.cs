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
    public class SubsidiaryCompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public SubsidiaryCompaniesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: SubsidiaryCompanies
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            string CompanyAbbreviatedName,
            string AbbreviatedName,
            string BIK,
            string KindOfActivity,
            int? HazardClassId,
            int? CityDistrictId,
            int? Page)
        {
            var subsidiaryCompanies = _context.SubsidiaryCompany
                .Include(s => s.Company)
                .Include(s => s.HazardClass)
                .Include(s => s.CityDistrict)
                .Where(s => true);

            ViewBag.CompanyAbbreviatedNameFilter = CompanyAbbreviatedName;
            ViewBag.AbbreviatedNameFilter = AbbreviatedName;
            ViewBag.BIKFilter = BIK;
            ViewBag.KindOfActivityFilter = KindOfActivity;
            ViewBag.HazardClassIdFilter = HazardClassId;
            ViewBag.CityDistrictIdFilter = CityDistrictId;

            ViewBag.CompanyAbbreviatedNameSort = SortOrder == "CompanyAbbreviatedName" ? "CompanyAbbreviatedNameDesc" : "CompanyAbbreviatedName";
            ViewBag.AbbreviatedNameSort = SortOrder == "AbbreviatedName" ? "AbbreviatedNameDesc" : "AbbreviatedName";
            ViewBag.BIKSort = SortOrder == "BIK" ? "BIKDesc" : "BIK";
            ViewBag.KindOfActivitySort = SortOrder == "KindOfActivity" ? "KindOfActivityDesc" : "KindOfActivity";
            ViewBag.HazardClassIdSort = SortOrder == "HazardClassId" ? "HazardClassIdDesc" : "HazardClassId";
            ViewBag.CityDistrictIdSort = SortOrder == "CityDistrictId" ? "CityDistrictIdDesc" : "CityDistrictId";

            if (!string.IsNullOrEmpty(CompanyAbbreviatedName))
            {
                subsidiaryCompanies = subsidiaryCompanies.Where(s => s.Company.AbbreviatedName.ToLower().Contains(CompanyAbbreviatedName.ToLower()));
            }
            if (!string.IsNullOrEmpty(AbbreviatedName))
            {
                subsidiaryCompanies = subsidiaryCompanies.Where(s => s.AbbreviatedName.ToLower().Contains(AbbreviatedName.ToLower()));
            }
            if (!string.IsNullOrEmpty(BIK))
            {
                subsidiaryCompanies = subsidiaryCompanies.Where(s => s.BIK.ToLower().Contains(BIK.ToLower()));
            }
            if (!string.IsNullOrEmpty(KindOfActivity))
            {
                subsidiaryCompanies = subsidiaryCompanies.Where(s => s.KindOfActivity.ToLower().Contains(KindOfActivity.ToLower()));
            }
            if (HazardClassId != null)
            {
                subsidiaryCompanies = subsidiaryCompanies.Where(s => s.HazardClassId == HazardClassId);
            }
            if (CityDistrictId != null)
            {
                subsidiaryCompanies = subsidiaryCompanies.Where(s => s.CityDistrictId == CityDistrictId);
            }

            switch (SortOrder)
            {
                case "CompanyAbbreviatedName":
                    subsidiaryCompanies = subsidiaryCompanies.OrderBy(s => s.Company.AbbreviatedName);
                    break;
                case "CompanyAbbreviatedNameDesc":
                    subsidiaryCompanies = subsidiaryCompanies.OrderByDescending(s => s.Company.AbbreviatedName);
                    break;
                case "AbbreviatedName":
                    subsidiaryCompanies = subsidiaryCompanies.OrderBy(s => s.AbbreviatedName);
                    break;
                case "AbbreviatedNameDesc":
                    subsidiaryCompanies = subsidiaryCompanies.OrderByDescending(s => s.AbbreviatedName);
                    break;
                case "BIK":
                    subsidiaryCompanies = subsidiaryCompanies.OrderBy(s => s.BIK);
                    break;
                case "BIKDesc":
                    subsidiaryCompanies = subsidiaryCompanies.OrderByDescending(s => s.BIK);
                    break;
                case "KindOfActivity":
                    subsidiaryCompanies = subsidiaryCompanies.OrderBy(s => s.KindOfActivity);
                    break;
                case "KindOfActivityDesc":
                    subsidiaryCompanies = subsidiaryCompanies.OrderByDescending(s => s.KindOfActivity);
                    break;
                case "HazardClassId":
                    subsidiaryCompanies = subsidiaryCompanies.OrderBy(s => s.HazardClass.Name);
                    break;
                case "HazardClassIdDesc":
                    subsidiaryCompanies = subsidiaryCompanies.OrderByDescending(s => s.HazardClass.Name);
                    break;
                case "CityDistrictId":
                    subsidiaryCompanies = subsidiaryCompanies.OrderBy(s => s.CityDistrict.Name);
                    break;
                case "CityDistrictIdDesc":
                    subsidiaryCompanies = subsidiaryCompanies.OrderByDescending(s => s.CityDistrict.Name);
                    break;
                default:
                    subsidiaryCompanies = subsidiaryCompanies.OrderBy(s => s.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(subsidiaryCompanies.Count(), Page);

            var viewModel = new SubsidiaryCompanyIndexPageViewModel
            {
                Items = subsidiaryCompanies.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewBag.HazardClassId = new SelectList(_context.HazardClass.OrderBy(h => h.Name), "Id", "Name");
            ViewBag.CityDistrictId = new SelectList(_context.CityDistrict.OrderBy(s => s.Name), "Id", "Name");

            return View(viewModel);
        }

        // GET: SubsidiaryCompanies/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subsidiaryCompany = await _context.SubsidiaryCompany
                .Include(s => s.CityDistrict)
                .Include(s => s.HazardClass)
                .Include(s => s.Company)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (subsidiaryCompany == null)
            {
                return NotFound();
            }

            return View(subsidiaryCompany);
        }

        // GET: SubsidiaryCompanies/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name");
            ViewData["HazardClassId"] = new SelectList(_context.HazardClass.OrderBy(h => h.Name), "Id", "Name");
            ViewData["CompanyId"] = new SelectList(_context.Company.Where(c => c.HierarchicalStructure).OrderBy(c => c.AbbreviatedName), "Id", "AbbreviatedName");
            return View();
        }

        // POST: SubsidiaryCompanies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("CompanyId,FullName,AbbreviatedName,BIK,KindOfActivity,HazardClassId,HierarchicalStructure,CityDistrictId,LegalAddress,ActualAddress,AdditionalInformation,NorthLatitude,EastLongitude")] SubsidiaryCompany subsidiaryCompany)
        {
            var subsidiaryCompanies = _context.SubsidiaryCompany.AsNoTracking().Include(s => s.HazardClass).Include(s => s.CityDistrict).ToList();
            if (string.IsNullOrWhiteSpace(subsidiaryCompany.FullName))
            {
                ModelState.AddModelError("FullName", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(subsidiaryCompany.AbbreviatedName))
            {
                ModelState.AddModelError("AbbreviatedName", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(subsidiaryCompany.BIK))
            {
                ModelState.AddModelError("BIK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(subsidiaryCompany.KindOfActivity))
            {
                ModelState.AddModelError("KindOfActivity", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (subsidiaryCompanies.Select(s => s.FullName).Contains(subsidiaryCompany.FullName))
            {
                ModelState.AddModelError("FullName", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (subsidiaryCompanies.Select(s => s.AbbreviatedName).Contains(subsidiaryCompany.AbbreviatedName))
            {
                ModelState.AddModelError("AbbreviatedName", _sharedLocalizer["ErrorDublicateValue"]);
            }
            //if (subsidiaryCompanies.Select(s => s.BIK).Contains(subsidiaryCompany.BIK))
            //{
            //    ModelState.AddModelError("BIK", _sharedLocalizer["ErrorDublicateValue"]);
            //}
            if (ModelState.IsValid)
            {
                _context.Add(subsidiaryCompany);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "SubsidiaryCompany",
                    New = subsidiaryCompany.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name", subsidiaryCompany.CityDistrictId);
            ViewData["HazardClassId"] = new SelectList(_context.HazardClass.OrderBy(h => h.Name), "Id", "Name", subsidiaryCompany.HazardClassId);
            ViewData["CompanyId"] = new SelectList(_context.Company.Where(c => c.HierarchicalStructure).OrderBy(c => c.AbbreviatedName), "Id", "AbbreviatedName", subsidiaryCompany.CompanyId);
            return View(subsidiaryCompany);
        }

        // GET: SubsidiaryCompanies/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subsidiaryCompany = await _context.SubsidiaryCompany.SingleOrDefaultAsync(m => m.Id == id);
            if (subsidiaryCompany == null)
            {
                return NotFound();
            }
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name", subsidiaryCompany.CityDistrictId);
            ViewData["HazardClassId"] = new SelectList(_context.HazardClass.OrderBy(h => h.Name), "Id", "Name", subsidiaryCompany.HazardClassId);
            ViewData["CompanyId"] = new SelectList(_context.Company.Where(c => c.HierarchicalStructure).OrderBy(c => c.AbbreviatedName), "Id", "AbbreviatedName", subsidiaryCompany.CompanyId);
            return View(subsidiaryCompany);
        }

        // POST: SubsidiaryCompanies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("CompanyId,Id,FullName,AbbreviatedName,BIK,KindOfActivity,HazardClassId,HierarchicalStructure,CityDistrictId,LegalAddress,ActualAddress,AdditionalInformation,NorthLatitude,EastLongitude")] SubsidiaryCompany subsidiaryCompany)
        {
            if (id != subsidiaryCompany.Id)
            {
                return NotFound();
            }
            var subsidiaryCompanies = _context.SubsidiaryCompany.AsNoTracking().Include(c => c.HazardClass).Include(c => c.CityDistrict).Where(c => c.Id != subsidiaryCompany.Id).ToList();
            if (string.IsNullOrWhiteSpace(subsidiaryCompany.FullName))
            {
                ModelState.AddModelError("FullName", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(subsidiaryCompany.AbbreviatedName))
            {
                ModelState.AddModelError("AbbreviatedName", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(subsidiaryCompany.BIK))
            {
                ModelState.AddModelError("BIK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(subsidiaryCompany.KindOfActivity))
            {
                ModelState.AddModelError("KindOfActivity", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (subsidiaryCompanies.Select(c => c.FullName).Contains(subsidiaryCompany.FullName))
            {
                ModelState.AddModelError("FullName", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (subsidiaryCompanies.Select(c => c.AbbreviatedName).Contains(subsidiaryCompany.AbbreviatedName))
            {
                ModelState.AddModelError("AbbreviatedName", _sharedLocalizer["ErrorDublicateValue"]);
            }
            //if (subsidiaryCompanies.Select(c => c.BIK).Contains(subsidiaryCompany.BIK))
            //{
            //    ModelState.AddModelError("BIK", _sharedLocalizer["ErrorDublicateValue"]);
            //}
            if (ModelState.IsValid)
            {
                try
                {
                    var subsidiaryCompany_old = _context.SubsidiaryCompany.AsNoTracking().FirstOrDefault(s => s.Id == subsidiaryCompany.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "SubsidiaryCompany",
                        Operation = "Edit",
                        New = subsidiaryCompany.ToString(),
                        Old = subsidiaryCompany_old.ToString()
                    });
                    _context.Update(subsidiaryCompany);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubsidiaryCompanyExists(subsidiaryCompany.Id))
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
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name", subsidiaryCompany.CityDistrictId);
            ViewData["HazardClassId"] = new SelectList(_context.HazardClass.OrderBy(h => h.Name), "Id", "Name", subsidiaryCompany.HazardClassId);
            ViewData["CompanyId"] = new SelectList(_context.Company.Where(c => c.HierarchicalStructure).OrderBy(c => c.AbbreviatedName), "Id", "AbbreviatedName", subsidiaryCompany.CompanyId);
            return View(subsidiaryCompany);
        }

        // GET: SubsidiaryCompanies/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subsidiaryCompany = await _context.SubsidiaryCompany
                .Include(s => s.CityDistrict)
                .Include(s => s.HazardClass)
                .Include(s => s.Company)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (subsidiaryCompany == null)
            {
                return NotFound();
            }
            ViewBag.IndustrialSites = _context.IndustrialSite
                .AsNoTracking()
                .Where(i => i.SubsidiaryCompanyId == id)
                .OrderBy(i => i.AbbreviatedName);
            return View(subsidiaryCompany);
        }

        // POST: SubsidiaryCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subsidiaryCompany = await _context.SubsidiaryCompany.SingleOrDefaultAsync(m => m.Id == id);
            if ((_context.IndustrialSite
                .AsNoTracking()
                .FirstOrDefault(i => i.SubsidiaryCompanyId == id) == null))
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "SubsidiaryCompany",
                    Operation = "Delete",
                    New = "",
                    Old = subsidiaryCompany.ToString()
                });
                _context.SubsidiaryCompany.Remove(subsidiaryCompany);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SubsidiaryCompanyExists(int id)
        {
            return _context.SubsidiaryCompany.Any(e => e.Id == id);
        }
    }
}
