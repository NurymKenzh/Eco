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
    public class NormativeSoilTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public NormativeSoilTypesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: NormativeSoilTypes
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string NameKK, string NameRU, int? Page)
        {
            var normativeSoilTypes = _context.NormativeSoilType
                .Where(s => true);

            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(NameKK))
            {
                normativeSoilTypes = normativeSoilTypes.Where(s => s.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                normativeSoilTypes = normativeSoilTypes.Where(s => s.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameKK":
                    normativeSoilTypes = normativeSoilTypes.OrderBy(s => s.NameKK);
                    break;
                case "NameKKDesc":
                    normativeSoilTypes = normativeSoilTypes.OrderByDescending(s => s.NameKK);
                    break;
                case "NameRU":
                    normativeSoilTypes = normativeSoilTypes.OrderBy(s => s.NameRU);
                    break;
                case "NameRUDesc":
                    normativeSoilTypes = normativeSoilTypes.OrderByDescending(s => s.NameRU);
                    break;
                default:
                    normativeSoilTypes = normativeSoilTypes.OrderBy(s => s.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(normativeSoilTypes.Count(), Page);

            var viewModel = new NormativeSoilTypeIndexPageViewModel
            {
                Items = normativeSoilTypes.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: NormativeSoilTypes/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var normativeSoilType = await _context.NormativeSoilType
                .SingleOrDefaultAsync(m => m.Id == id);
            if (normativeSoilType == null)
            {
                return NotFound();
            }

            return View(normativeSoilType);
        }

        // GET: NormativeSoilTypes/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: NormativeSoilTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,NameKK,NameRU")] NormativeSoilType normativeSoilType)
        {
            var normativeSoilTypes = _context.NormativeSoilType.AsNoTracking().ToList();
            if (normativeSoilTypes.Select(s => s.NameKK).Contains(normativeSoilType.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (normativeSoilTypes.Select(s => s.NameRU).Contains(normativeSoilType.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(normativeSoilType.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(normativeSoilType.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(normativeSoilType);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "NormativeSoilType",
                    New = normativeSoilType.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(normativeSoilType);
        }

        // GET: NormativeSoilTypes/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var normativeSoilType = await _context.NormativeSoilType.SingleOrDefaultAsync(m => m.Id == id);
            if (normativeSoilType == null)
            {
                return NotFound();
            }
            return View(normativeSoilType);
        }

        // POST: NormativeSoilTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameKK,NameRU")] NormativeSoilType normativeSoilType)
        {
            if (id != normativeSoilType.Id)
            {
                return NotFound();
            }
            var normativeSoilTypes = _context.NormativeSoilType.AsNoTracking().ToList();
            if (normativeSoilTypes.Where(s => s.Id != normativeSoilType.Id).Select(s => s.NameKK).Contains(normativeSoilType.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (normativeSoilTypes.Where(s => s.Id != normativeSoilType.Id).Select(s => s.NameRU).Contains(normativeSoilType.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(normativeSoilType.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(normativeSoilType.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var normativeSoilType_old = _context.NormativeSoilType.AsNoTracking().FirstOrDefault(s => s.Id == normativeSoilType.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "NormativeSoilType",
                        Operation = "Edit",
                        New = normativeSoilType.ToString(),
                        Old = normativeSoilType_old.ToString()
                    });
                    _context.Update(normativeSoilType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NormativeSoilTypeExists(normativeSoilType.Id))
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
            return View(normativeSoilType);
        }

        // GET: NormativeSoilTypes/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var normativeSoilType = await _context.NormativeSoilType
                .SingleOrDefaultAsync(m => m.Id == id);
            if (normativeSoilType == null)
            {
                return NotFound();
            }
            ViewBag.SoilContaminants = _context.SoilContaminant
                .AsNoTracking()
                .Where(s => s.NormativeSoilTypeId == id)
                .OrderBy(s => s.Name);
            return View(normativeSoilType);
        }

        // POST: NormativeSoilTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var normativeSoilType = await _context.NormativeSoilType.SingleOrDefaultAsync(m => m.Id == id);
            if ((_context.SoilContaminant
                .AsNoTracking()
                .FirstOrDefault(s => s.NormativeSoilTypeId == id) == null))
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "NormativeSoilType",
                    Operation = "Delete",
                    New = "",
                    Old = normativeSoilType.ToString()
                });
                _context.NormativeSoilType.Remove(normativeSoilType);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool NormativeSoilTypeExists(int id)
        {
            return _context.NormativeSoilType.Any(e => e.Id == id);
        }
    }
}
