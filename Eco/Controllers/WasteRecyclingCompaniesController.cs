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
    public class WasteRecyclingCompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public WasteRecyclingCompaniesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: WasteRecyclingCompanies
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            string Name,
            string BIK,
            int? RecyclableWasteTypeId,
            int? Page)
        {
            var wasteRecyclingCompanies = _context.WasteRecyclingCompany
                .Include(w => w.RecyclableWasteType)
                .Where(w => true);

            ViewBag.NameFilter = Name;
            ViewBag.BIKFilter = BIK;
            ViewBag.RecyclableWasteTypeIdFilter = RecyclableWasteTypeId;

            ViewBag.NameSort = SortOrder == "Name" ? "NameDesc" : "Name";
            ViewBag.BIKSort = SortOrder == "BIK" ? "BIKDesc" : "BIK";
            ViewBag.RecyclableWasteTypeIdSort = SortOrder == "RecyclableWasteTypeId" ? "RecyclableWasteTypeIdDesc" : "RecyclableWasteTypeId";

            if (!string.IsNullOrEmpty(Name))
            {
                wasteRecyclingCompanies = wasteRecyclingCompanies.Where(c => c.Name.ToLower().Contains(Name.ToLower()));
            }
            if (!string.IsNullOrEmpty(BIK))
            {
                wasteRecyclingCompanies = wasteRecyclingCompanies.Where(c => c.BIK.ToLower().Contains(BIK.ToLower()));
            }
            if (RecyclableWasteTypeId != null)
            {
                wasteRecyclingCompanies = wasteRecyclingCompanies.Where(c => c.RecyclableWasteTypeId == RecyclableWasteTypeId);
            }

            switch (SortOrder)
            {
                case "Name":
                    wasteRecyclingCompanies = wasteRecyclingCompanies.OrderBy(c => c.Name);
                    break;
                case "NameDesc":
                    wasteRecyclingCompanies = wasteRecyclingCompanies.OrderByDescending(c => c.Name);
                    break;
                case "BIK":
                    wasteRecyclingCompanies = wasteRecyclingCompanies.OrderBy(c => c.BIK);
                    break;
                case "BIKDesc":
                    wasteRecyclingCompanies = wasteRecyclingCompanies.OrderByDescending(c => c.BIK);
                    break;
                case "RecyclableWasteTypeId":
                    wasteRecyclingCompanies = wasteRecyclingCompanies.OrderBy(c => c.RecyclableWasteType.Name);
                    break;
                case "RecyclableWasteTypeIdDesc":
                    wasteRecyclingCompanies = wasteRecyclingCompanies.OrderByDescending(c => c.RecyclableWasteType.Name);
                    break;
                default:
                    wasteRecyclingCompanies = wasteRecyclingCompanies.OrderBy(c => c.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(wasteRecyclingCompanies.Count(), Page);

            var viewModel = new WasteRecyclingCompanyIndexPageViewModel
            {
                Items = wasteRecyclingCompanies.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewBag.RecyclableWasteTypeId = new SelectList(_context.RecyclableWasteType.OrderBy(h => h.Name), "Id", "Name");
            ViewData["Name"] = new SelectList(_context.WasteRecyclingCompany.OrderBy(a => a.Name).GroupBy(k => k.Name).Select(g => g.First()), "Name", "Name");
            return View(viewModel);
        }

        // GET: WasteRecyclingCompanies/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wasteRecyclingCompany = await _context.WasteRecyclingCompany
                .Include(w => w.RecyclableWasteType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (wasteRecyclingCompany == null)
            {
                return NotFound();
            }

            return View(wasteRecyclingCompany);
        }

        // GET: WasteRecyclingCompanies/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["RecyclableWasteTypeId"] = new SelectList(_context.RecyclableWasteType.OrderBy(c => c.Name), "Id", "Name");
            WasteRecyclingCompany model = new WasteRecyclingCompany();
            return View(model);
        }

        // POST: WasteRecyclingCompanies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Name,BIK,AddressContactInformation,NorthLatitude,EastLongitude,RecyclableWasteTypeId,Status,AdditionalInformationKK,AdditionalInformationRU")] WasteRecyclingCompany wasteRecyclingCompany)
        {
            var wasteRecyclingCompanies = _context.WasteRecyclingCompany.AsNoTracking()
                .Include(c => c.RecyclableWasteType)
                .ToList();
            if (string.IsNullOrWhiteSpace(wasteRecyclingCompany.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(wasteRecyclingCompany.BIK))
            {
                ModelState.AddModelError("BIK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(wasteRecyclingCompany.AddressContactInformation))
            {
                ModelState.AddModelError("AddressContactInformation", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (wasteRecyclingCompanies.Select(c => c.Name).Contains(wasteRecyclingCompany.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (wasteRecyclingCompanies.Select(c => c.Name).Contains(wasteRecyclingCompany.BIK))
            {
                ModelState.AddModelError("BIK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(wasteRecyclingCompany);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "WasteRecyclingCompany",
                    New = wasteRecyclingCompany.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RecyclableWasteTypeId"] = new SelectList(_context.RecyclableWasteType.OrderBy(c => c.Name), "Id", "Name", wasteRecyclingCompany.RecyclableWasteTypeId);
            return View(wasteRecyclingCompany);
        }

        // GET: WasteRecyclingCompanies/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wasteRecyclingCompany = await _context.WasteRecyclingCompany.SingleOrDefaultAsync(m => m.Id == id);
            if (wasteRecyclingCompany == null)
            {
                return NotFound();
            }
            ViewData["RecyclableWasteTypeId"] = new SelectList(_context.RecyclableWasteType.OrderBy(c => c.Name), "Id", "Name", wasteRecyclingCompany.RecyclableWasteTypeId);
            return View(wasteRecyclingCompany);
        }

        // POST: WasteRecyclingCompanies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BIK,AddressContactInformation,NorthLatitude,EastLongitude,RecyclableWasteTypeId,Status,AdditionalInformationKK,AdditionalInformationRU")] WasteRecyclingCompany wasteRecyclingCompany)
        {
            if (id != wasteRecyclingCompany.Id)
            {
                return NotFound();
            }
            var wasteRecyclingCompanies = _context.WasteRecyclingCompany.AsNoTracking()
                .Include(c => c.RecyclableWasteType)
                .Where(c => c.Id != wasteRecyclingCompany.Id)
                .ToList();
            if (string.IsNullOrWhiteSpace(wasteRecyclingCompany.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(wasteRecyclingCompany.BIK))
            {
                ModelState.AddModelError("BIK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(wasteRecyclingCompany.AddressContactInformation))
            {
                ModelState.AddModelError("AddressContactInformation", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (wasteRecyclingCompanies.Select(c => c.Name).Contains(wasteRecyclingCompany.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (wasteRecyclingCompanies.Select(c => c.Name).Contains(wasteRecyclingCompany.BIK))
            {
                ModelState.AddModelError("BIK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var wasteRecyclingCompany_old = _context.WasteRecyclingCompany.AsNoTracking().FirstOrDefault(c => c.Id == wasteRecyclingCompany.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "WasteRecyclingCompany",
                        Operation = "Edit",
                        New = wasteRecyclingCompany.ToString(),
                        Old = wasteRecyclingCompany_old.ToString()
                    });
                    _context.Update(wasteRecyclingCompany);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WasteRecyclingCompanyExists(wasteRecyclingCompany.Id))
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
            ViewData["RecyclableWasteTypeId"] = new SelectList(_context.RecyclableWasteType.OrderBy(c => c.Name), "Id", "Name", wasteRecyclingCompany.RecyclableWasteTypeId);
            return View(wasteRecyclingCompany);
        }

        // GET: WasteRecyclingCompanies/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wasteRecyclingCompany = await _context.WasteRecyclingCompany
                .Include(w => w.RecyclableWasteType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (wasteRecyclingCompany == null)
            {
                return NotFound();
            }

            return View(wasteRecyclingCompany);
        }

        // POST: WasteRecyclingCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wasteRecyclingCompany = await _context.WasteRecyclingCompany.SingleOrDefaultAsync(m => m.Id == id);
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Class = "WasteRecyclingCompany",
                Operation = "Delete",
                New = "",
                Old = wasteRecyclingCompany.ToString()
            });
            _context.WasteRecyclingCompany.Remove(wasteRecyclingCompany);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WasteRecyclingCompanyExists(int id)
        {
            return _context.WasteRecyclingCompany.Any(e => e.Id == id);
        }
    }
}
