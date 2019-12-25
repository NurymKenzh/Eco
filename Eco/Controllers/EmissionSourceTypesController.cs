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
    public class EmissionSourceTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public EmissionSourceTypesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: EmissionSourceTypes
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string NameKK, string NameRU, int? Page)
        {
            var emissionSourceTypes = _context.EmissionSourceType
                .Where(s => true);

            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(NameKK))
            {
                emissionSourceTypes = emissionSourceTypes.Where(s => s.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                emissionSourceTypes = emissionSourceTypes.Where(s => s.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameKK":
                    emissionSourceTypes = emissionSourceTypes.OrderBy(s => s.NameKK);
                    break;
                case "NameKKDesc":
                    emissionSourceTypes = emissionSourceTypes.OrderByDescending(s => s.NameKK);
                    break;
                case "NameRU":
                    emissionSourceTypes = emissionSourceTypes.OrderBy(s => s.NameRU);
                    break;
                case "NameRUDesc":
                    emissionSourceTypes = emissionSourceTypes.OrderByDescending(s => s.NameRU);
                    break;
                default:
                    emissionSourceTypes = emissionSourceTypes.OrderBy(s => s.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(emissionSourceTypes.Count(), Page);

            var viewModel = new EmissionSourceTypeIndexPageViewModel
            {
                Items = emissionSourceTypes.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: EmissionSourceTypes/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emissionSourceType = await _context.EmissionSourceType
                .SingleOrDefaultAsync(m => m.Id == id);
            if (emissionSourceType == null)
            {
                return NotFound();
            }

            return View(emissionSourceType);
        }

        ////// GET: EmissionSourceTypes/Create
        ////[Authorize(Roles = "Administrator, Moderator")]
        ////public IActionResult Create()
        ////{
        ////    return View();
        ////}

        ////// POST: EmissionSourceTypes/Create
        ////// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        ////// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        ////[HttpPost]
        ////[ValidateAntiForgeryToken]
        ////[Authorize(Roles = "Administrator, Moderator")]
        ////public async Task<IActionResult> Create([Bind("Id,NameKK,NameRU,PointsAmount")] EmissionSourceType emissionSourceType)
        ////{
        ////    var emissionSourceTypes = _context.EmissionSourceType.AsNoTracking().ToList();
        ////    if (emissionSourceTypes.Select(s => s.NameKK).Contains(emissionSourceType.NameKK))
        ////    {
        ////        ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
        ////    }
        ////    if (emissionSourceTypes.Select(s => s.NameRU).Contains(emissionSourceType.NameRU))
        ////    {
        ////        ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
        ////    }
        ////    if (string.IsNullOrWhiteSpace(emissionSourceType.NameKK))
        ////    {
        ////        ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
        ////    }
        ////    if (string.IsNullOrWhiteSpace(emissionSourceType.NameRU))
        ////    {
        ////        ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
        ////    }
        ////    if (ModelState.IsValid)
        ////    {
        ////        _context.Add(emissionSourceType);
        ////        await _context.SaveChangesAsync();
        ////        _context.Log.Add(new Log()
        ////        {
        ////            DateTime = DateTime.Now,
        ////            Email = User.Identity.Name,
        ////            Operation = "Create",
        ////            Class = "EmissionSourceType",
        ////            New = emissionSourceType.ToString(),
        ////            Old = ""
        ////        });
        ////        await _context.SaveChangesAsync();
        ////        return RedirectToAction(nameof(Index));
        ////    }
        ////    return View(emissionSourceType);
        ////}

        ////// GET: EmissionSourceTypes/Edit/5
        ////[Authorize(Roles = "Administrator, Moderator")]
        ////public async Task<IActionResult> Edit(int? id)
        ////{
        ////    if (id == null)
        ////    {
        ////        return NotFound();
        ////    }

        ////    var emissionSourceType = await _context.EmissionSourceType.SingleOrDefaultAsync(m => m.Id == id);
        ////    if (emissionSourceType == null)
        ////    {
        ////        return NotFound();
        ////    }
        ////    return View(emissionSourceType);
        ////}

        ////// POST: EmissionSourceTypes/Edit/5
        ////// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        ////// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        ////[HttpPost]
        ////[ValidateAntiForgeryToken]
        ////[Authorize(Roles = "Administrator, Moderator")]
        ////public async Task<IActionResult> Edit(int id, [Bind("Id,NameKK,NameRU,PointsAmount")] EmissionSourceType emissionSourceType)
        ////{
        ////    if (id != emissionSourceType.Id)
        ////    {
        ////        return NotFound();
        ////    }
        ////    var emissionSourceTypes = _context.EmissionSourceType.AsNoTracking().ToList();
        ////    if (emissionSourceTypes.Where(s => s.Id != emissionSourceType.Id).Select(s => s.NameKK).Contains(emissionSourceType.NameKK))
        ////    {
        ////        ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
        ////    }
        ////    if (emissionSourceTypes.Where(s => s.Id != emissionSourceType.Id).Select(s => s.NameRU).Contains(emissionSourceType.NameRU))
        ////    {
        ////        ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
        ////    }
        ////    if (string.IsNullOrWhiteSpace(emissionSourceType.NameKK))
        ////    {
        ////        ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
        ////    }
        ////    if (string.IsNullOrWhiteSpace(emissionSourceType.NameRU))
        ////    {
        ////        ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
        ////    }
        ////    if (ModelState.IsValid)
        ////    {
        ////        try
        ////        {
        ////            var emissionSourceType_old = _context.EmissionSourceType.AsNoTracking().FirstOrDefault(s => s.Id == emissionSourceType.Id);
        ////            _context.Log.Add(new Log()
        ////            {
        ////                DateTime = DateTime.Now,
        ////                Email = User.Identity.Name,
        ////                Class = "EmissionSourceType",
        ////                Operation = "Edit",
        ////                New = emissionSourceType.ToString(),
        ////                Old = emissionSourceType_old.ToString()
        ////            });
        ////            _context.Update(emissionSourceType);
        ////            await _context.SaveChangesAsync();
        ////        }
        ////        catch (DbUpdateConcurrencyException)
        ////        {
        ////            if (!EmissionSourceTypeExists(emissionSourceType.Id))
        ////            {
        ////                return NotFound();
        ////            }
        ////            else
        ////            {
        ////                throw;
        ////            }
        ////        }
        ////        return RedirectToAction(nameof(Index));
        ////    }
        ////    return View(emissionSourceType);
        ////}

        //// GET: EmissionSourceTypes/Delete/5
        //[Authorize(Roles = "Administrator, Moderator")]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var emissionSourceType = await _context.EmissionSourceType
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (emissionSourceType == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewBag.EmissionSources = _context.EmissionSource
        //        .AsNoTracking()
        //        .Where(a => a.EmissionSourceTypeId == id)
        //        .OrderBy(a => a.EmissionSourceName);
        //    return View(emissionSourceType);
        //}

        //// POST: EmissionSourceTypes/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Administrator, Moderator")]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var emissionSourceType = await _context.EmissionSourceType.SingleOrDefaultAsync(m => m.Id == id);
        //    if (_context.EmissionSource
        //        .AsNoTracking()
        //        .FirstOrDefault(a => a.EmissionSourceTypeId == id) == null)
        //    {
        //        _context.Log.Add(new Log()
        //        {
        //            DateTime = DateTime.Now,
        //            Email = User.Identity.Name,
        //            Class = "EmissionSourceType",
        //            Operation = "Delete",
        //            New = "",
        //            Old = emissionSourceType.ToString()
        //        });
        //        _context.EmissionSourceType.Remove(emissionSourceType);
        //        await _context.SaveChangesAsync();
        //    }
        //    return RedirectToAction(nameof(Index));
        //}

        private bool EmissionSourceTypeExists(int id)
        {
            return _context.EmissionSourceType.Any(e => e.Id == id);
        }
    }
}
