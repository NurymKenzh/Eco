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
    public class RawMaterialsCompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public RawMaterialsCompaniesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: RawMaterialsCompanies
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            string Name,
            string BIK,
            int? WasteTypeId,
            int? Page)
        {
            var rawMaterialsCompanies = _context.RawMaterialsCompany
                .Include(r => r.WasteType)
                .Where(r => true);

            ViewBag.NameFilter = Name;
            ViewBag.BIKFilter = BIK;
            ViewBag.WasteTypeIdFilter = WasteTypeId;

            ViewBag.NameSort = SortOrder == "Name" ? "NameDesc" : "Name";
            ViewBag.BIKSort = SortOrder == "BIK" ? "BIKDesc" : "BIK";
            ViewBag.WasteTypeIdSort = SortOrder == "WasteTypeId" ? "WasteTypeIdDesc" : "WasteTypeId";

            if (!string.IsNullOrEmpty(Name))
            {
                rawMaterialsCompanies = rawMaterialsCompanies.Where(c => c.Name.ToLower().Contains(Name.ToLower()));
            }
            if (!string.IsNullOrEmpty(BIK))
            {
                rawMaterialsCompanies = rawMaterialsCompanies.Where(c => c.BIK.ToLower().Contains(BIK.ToLower()));
            }
            if (WasteTypeId != null)
            {
                rawMaterialsCompanies = rawMaterialsCompanies.Where(c => c.WasteTypeId == WasteTypeId);
            }

            switch (SortOrder)
            {
                case "Name":
                    rawMaterialsCompanies = rawMaterialsCompanies.OrderBy(c => c.Name);
                    break;
                case "NameDesc":
                    rawMaterialsCompanies = rawMaterialsCompanies.OrderByDescending(c => c.Name);
                    break;
                case "BIK":
                    rawMaterialsCompanies = rawMaterialsCompanies.OrderBy(c => c.BIK);
                    break;
                case "BIKDesc":
                    rawMaterialsCompanies = rawMaterialsCompanies.OrderByDescending(c => c.BIK);
                    break;
                case "WasteTypeId":
                    rawMaterialsCompanies = rawMaterialsCompanies.OrderBy(c => c.WasteType.Name);
                    break;
                case "WasteTypeIdDesc":
                    rawMaterialsCompanies = rawMaterialsCompanies.OrderByDescending(c => c.WasteType.Name);
                    break;
                default:
                    rawMaterialsCompanies = rawMaterialsCompanies.OrderBy(c => c.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(rawMaterialsCompanies.Count(), Page);

            var viewModel = new RawMaterialsCompanyIndexPageViewModel
            {
                Items = rawMaterialsCompanies.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewBag.WasteTypeId = new SelectList(_context.WasteType.OrderBy(h => h.Name), "Id", "Name");
            ViewData["Name"] = new SelectList(_context.RawMaterialsCompany.OrderBy(a => a.Name).GroupBy(k => k.Name).Select(g => g.First()), "Name", "Name");
            return View(viewModel);
        }

        // GET: RawMaterialsCompanies/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rawMaterialsCompany = await _context.RawMaterialsCompany
                .Include(r => r.WasteType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (rawMaterialsCompany == null)
            {
                return NotFound();
            }

            return View(rawMaterialsCompany);
        }

        // GET: RawMaterialsCompanies/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["WasteTypeId"] = new SelectList(_context.WasteType.OrderBy(c => c.Name), "Id", "Name");
            RawMaterialsCompany model = new RawMaterialsCompany();
            return View(model);
        }

        // POST: RawMaterialsCompanies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Name,BIK,ReceiptPointNumber,AddressContactInformation,NorthLatitude,EastLongitude,WasteTypeId,Status,AdditionalInformationKK,AdditionalInformationRU")] RawMaterialsCompany rawMaterialsCompany)
        {
            var rawMaterialsCompanies = _context.RawMaterialsCompany.AsNoTracking()
                .Include(c => c.WasteType)
                .ToList();
            if (string.IsNullOrWhiteSpace(rawMaterialsCompany.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(rawMaterialsCompany.BIK))
            {
                ModelState.AddModelError("BIK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(rawMaterialsCompany.AddressContactInformation))
            {
                ModelState.AddModelError("AddressContactInformation", _sharedLocalizer["ErrorNeedToInput"]);
            }
            //if (rawMaterialsCompanies.Select(c => c.Name).Contains(rawMaterialsCompany.Name))
            //{
            //    ModelState.AddModelError("Name", _sharedLocalizer["ErrorDublicateValue"]);
            //}
            //if (rawMaterialsCompanies.Select(c => c.Name).Contains(rawMaterialsCompany.BIK))
            //{
            //    ModelState.AddModelError("BIK", _sharedLocalizer["ErrorDublicateValue"]);
            //}
            if (ModelState.IsValid)
            {
                _context.Add(rawMaterialsCompany);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "RawMaterialsCompany",
                    New = rawMaterialsCompany.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["WasteTypeId"] = new SelectList(_context.WasteType.OrderBy(c => c.Name), "Id", "Name", rawMaterialsCompany.WasteTypeId);
            return View(rawMaterialsCompany);
        }

        // GET: RawMaterialsCompanies/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rawMaterialsCompany = await _context.RawMaterialsCompany.SingleOrDefaultAsync(m => m.Id == id);
            if (rawMaterialsCompany == null)
            {
                return NotFound();
            }
            ViewData["WasteTypeId"] = new SelectList(_context.WasteType.OrderBy(c => c.Name), "Id", "Name", rawMaterialsCompany.WasteTypeId);
            return View(rawMaterialsCompany);
        }

        // POST: RawMaterialsCompanies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BIK,ReceiptPointNumber,AddressContactInformation,NorthLatitude,EastLongitude,WasteTypeId,Status,AdditionalInformationKK,AdditionalInformationRU")] RawMaterialsCompany rawMaterialsCompany)
        {
            if (id != rawMaterialsCompany.Id)
            {
                return NotFound();
            }
            var rawMaterialsCompanies = _context.RawMaterialsCompany.AsNoTracking()
                .Include(c => c.WasteType)
                .Where(c => c.Id != rawMaterialsCompany.Id)
                .ToList();
            if (string.IsNullOrWhiteSpace(rawMaterialsCompany.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(rawMaterialsCompany.BIK))
            {
                ModelState.AddModelError("BIK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(rawMaterialsCompany.AddressContactInformation))
            {
                ModelState.AddModelError("AddressContactInformation", _sharedLocalizer["ErrorNeedToInput"]);
            }
            //if (rawMaterialsCompanies.Select(c => c.Name).Contains(rawMaterialsCompany.Name))
            //{
            //    ModelState.AddModelError("Name", _sharedLocalizer["ErrorDublicateValue"]);
            //}
            //if (rawMaterialsCompanies.Select(c => c.Name).Contains(rawMaterialsCompany.BIK))
            //{
            //    ModelState.AddModelError("BIK", _sharedLocalizer["ErrorDublicateValue"]);
            //}
            if (ModelState.IsValid)
            {
                try
                {
                    var rawMaterialsCompany_old = _context.RawMaterialsCompany.AsNoTracking().FirstOrDefault(c => c.Id == rawMaterialsCompany.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "RawMaterialsCompany",
                        Operation = "Edit",
                        New = rawMaterialsCompany.ToString(),
                        Old = rawMaterialsCompany_old.ToString()
                    });
                    _context.Update(rawMaterialsCompany);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RawMaterialsCompanyExists(rawMaterialsCompany.Id))
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
            ViewData["WasteTypeId"] = new SelectList(_context.WasteType.OrderBy(c => c.Name), "Id", "Name", rawMaterialsCompany.WasteTypeId);
            return View(rawMaterialsCompany);
        }

        // GET: RawMaterialsCompanies/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rawMaterialsCompany = await _context.RawMaterialsCompany
                .Include(r => r.WasteType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (rawMaterialsCompany == null)
            {
                return NotFound();
            }

            return View(rawMaterialsCompany);
        }

        // POST: RawMaterialsCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rawMaterialsCompany = await _context.RawMaterialsCompany.SingleOrDefaultAsync(m => m.Id == id);
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Class = "RawMaterialsCompany",
                Operation = "Delete",
                New = "",
                Old = rawMaterialsCompany.ToString()
            });
            _context.RawMaterialsCompany.Remove(rawMaterialsCompany);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RawMaterialsCompanyExists(int id)
        {
            return _context.RawMaterialsCompany.Any(e => e.Id == id);
        }
    }
}
