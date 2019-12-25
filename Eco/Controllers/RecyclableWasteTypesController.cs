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
    public class RecyclableWasteTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public RecyclableWasteTypesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: RecyclableWasteTypes
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string NameKK, string NameRU, int? Page)
        {
            var recyclableWasteTypes = _context.RecyclableWasteType
                .Where(h => true);

            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(NameKK))
            {
                recyclableWasteTypes = recyclableWasteTypes.Where(h => h.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                recyclableWasteTypes = recyclableWasteTypes.Where(h => h.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameKK":
                    recyclableWasteTypes = recyclableWasteTypes.OrderBy(h => h.NameKK);
                    break;
                case "NameKKDesc":
                    recyclableWasteTypes = recyclableWasteTypes.OrderByDescending(h => h.NameKK);
                    break;
                case "NameRU":
                    recyclableWasteTypes = recyclableWasteTypes.OrderBy(h => h.NameRU);
                    break;
                case "NameRUDesc":
                    recyclableWasteTypes = recyclableWasteTypes.OrderByDescending(h => h.NameRU);
                    break;
                default:
                    recyclableWasteTypes = recyclableWasteTypes.OrderBy(h => h.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(recyclableWasteTypes.Count(), Page);

            var viewModel = new RecyclableWasteTypeIndexPageViewModel
            {
                Items = recyclableWasteTypes.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: RecyclableWasteTypes/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recyclableWasteType = await _context.RecyclableWasteType
                .SingleOrDefaultAsync(m => m.Id == id);
            if (recyclableWasteType == null)
            {
                return NotFound();
            }

            return View(recyclableWasteType);
        }

        // GET: RecyclableWasteTypes/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: RecyclableWasteTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,NameKK,NameRU")] RecyclableWasteType recyclableWasteType)
        {
            var recyclableWasteTypes = _context.RecyclableWasteType.AsNoTracking().ToList();
            if (recyclableWasteTypes.Select(h => h.NameKK).Contains(recyclableWasteType.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (recyclableWasteTypes.Select(h => h.NameRU).Contains(recyclableWasteType.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(recyclableWasteType.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(recyclableWasteType.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(recyclableWasteType);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "RecyclableWasteType",
                    New = recyclableWasteType.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recyclableWasteType);
        }

        // GET: RecyclableWasteTypes/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recyclableWasteType = await _context.RecyclableWasteType.SingleOrDefaultAsync(m => m.Id == id);
            if (recyclableWasteType == null)
            {
                return NotFound();
            }
            return View(recyclableWasteType);
        }

        // POST: RecyclableWasteTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameKK,NameRU")] RecyclableWasteType recyclableWasteType)
        {
            if (id != recyclableWasteType.Id)
            {
                return NotFound();
            }
            var recyclableWasteTypes = _context.RecyclableWasteType.AsNoTracking().ToList();
            if (recyclableWasteTypes.Where(h => h.Id != recyclableWasteType.Id).Select(h => h.NameKK).Contains(recyclableWasteType.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (recyclableWasteTypes.Where(h => h.Id != recyclableWasteType.Id).Select(h => h.NameRU).Contains(recyclableWasteType.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(recyclableWasteType.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(recyclableWasteType.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var recyclableWasteType_old = _context.RecyclableWasteType.AsNoTracking().FirstOrDefault(h => h.Id == recyclableWasteType.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "RecyclableWasteType",
                        Operation = "Edit",
                        New = recyclableWasteType.ToString(),
                        Old = recyclableWasteType_old.ToString()
                    });
                    _context.Update(recyclableWasteType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecyclableWasteTypeExists(recyclableWasteType.Id))
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
            return View(recyclableWasteType);
        }

        // GET: RecyclableWasteTypes/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recyclableWasteType = await _context.RecyclableWasteType
                .SingleOrDefaultAsync(m => m.Id == id);
            if (recyclableWasteType == null)
            {
                return NotFound();
            }
            ViewBag.WasteRecyclingCompanies = _context.WasteRecyclingCompany
                .AsNoTracking()
                .Where(c => c.RecyclableWasteTypeId == id)
                .OrderBy(c => c.Name);
            return View(recyclableWasteType);
        }

        // POST: RecyclableWasteTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recyclableWasteType = await _context.RecyclableWasteType.SingleOrDefaultAsync(m => m.Id == id);
            if ((_context.WasteRecyclingCompany
                .AsNoTracking()
                .FirstOrDefault(c => c.RecyclableWasteTypeId == id) == null))
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "RecyclableWasteType",
                    Operation = "Delete",
                    New = "",
                    Old = recyclableWasteType.ToString()
                });
                _context.RecyclableWasteType.Remove(recyclableWasteType);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool RecyclableWasteTypeExists(int id)
        {
            return _context.RecyclableWasteType.Any(e => e.Id == id);
        }
    }
}
