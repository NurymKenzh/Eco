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
    public class WasteTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public WasteTypesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: WasteTypes
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string NameKK, string NameRU, int? Page)
        {
            var wasteTypes = _context.WasteType
                .Where(h => true);

            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(NameKK))
            {
                wasteTypes = wasteTypes.Where(h => h.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                wasteTypes = wasteTypes.Where(h => h.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameKK":
                    wasteTypes = wasteTypes.OrderBy(h => h.NameKK);
                    break;
                case "NameKKDesc":
                    wasteTypes = wasteTypes.OrderByDescending(h => h.NameKK);
                    break;
                case "NameRU":
                    wasteTypes = wasteTypes.OrderBy(h => h.NameRU);
                    break;
                case "NameRUDesc":
                    wasteTypes = wasteTypes.OrderByDescending(h => h.NameRU);
                    break;
                default:
                    wasteTypes = wasteTypes.OrderBy(h => h.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(wasteTypes.Count(), Page);

            var viewModel = new WasteTypeIndexPageViewModel
            {
                Items = wasteTypes.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: WasteTypes/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wasteType = await _context.WasteType
                .SingleOrDefaultAsync(m => m.Id == id);
            if (wasteType == null)
            {
                return NotFound();
            }

            return View(wasteType);
        }

        // GET: WasteTypes/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: WasteTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,NameKK,NameRU")] WasteType wasteType)
        {
            var wasteTypes = _context.WasteType.AsNoTracking().ToList();
            if (wasteTypes.Select(h => h.NameKK).Contains(wasteType.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (wasteTypes.Select(h => h.NameRU).Contains(wasteType.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(wasteType.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(wasteType.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(wasteType);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "WasteType",
                    New = wasteType.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wasteType);
        }

        // GET: WasteTypes/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wasteType = await _context.WasteType.SingleOrDefaultAsync(m => m.Id == id);
            if (wasteType == null)
            {
                return NotFound();
            }
            return View(wasteType);
        }

        // POST: WasteTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameKK,NameRU")] WasteType wasteType)
        {
            if (id != wasteType.Id)
            {
                return NotFound();
            }
            var wasteTypes = _context.WasteType.AsNoTracking().ToList();
            if (wasteTypes.Where(h => h.Id != wasteType.Id).Select(h => h.NameKK).Contains(wasteType.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (wasteTypes.Where(h => h.Id != wasteType.Id).Select(h => h.NameRU).Contains(wasteType.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(wasteType.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(wasteType.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var wasteType_old = _context.WasteType.AsNoTracking().FirstOrDefault(h => h.Id == wasteType.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "WasteType",
                        Operation = "Edit",
                        New = wasteType.ToString(),
                        Old = wasteType_old.ToString()
                    });
                    _context.Update(wasteType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WasteTypeExists(wasteType.Id))
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
            return View(wasteType);
        }

        // GET: WasteTypes/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wasteType = await _context.WasteType
                .SingleOrDefaultAsync(m => m.Id == id);
            if (wasteType == null)
            {
                return NotFound();
            }
            ViewBag.RawMaterialsCompanies = _context.RawMaterialsCompany
                .AsNoTracking()
                .Where(c => c.WasteTypeId == id)
                .OrderBy(c => c.Name);
            return View(wasteType);
        }

        // POST: WasteTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wasteType = await _context.WasteType.SingleOrDefaultAsync(m => m.Id == id);
            if ((_context.RawMaterialsCompany
                .AsNoTracking()
                .FirstOrDefault(c => c.WasteTypeId == id) == null))
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "WasteType",
                    Operation = "Delete",
                    New = "",
                    Old = wasteType.ToString()
                });
                _context.WasteType.Remove(wasteType);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool WasteTypeExists(int id)
        {
            return _context.WasteType.Any(e => e.Id == id);
        }
    }
}
