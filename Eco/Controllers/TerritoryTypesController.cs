using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eco.Data;
using Eco.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

namespace Eco.Controllers
{
    public class TerritoryTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public TerritoryTypesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: TerritoryTypes
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string NameKK, string NameRU, int? Page)
        {
            var territoryTypes = _context.TerritoryType
                .Where(t => true);
            
            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;
            
            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";
            
            if (!string.IsNullOrEmpty(NameKK))
            {
                territoryTypes = territoryTypes.Where(t => t.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                territoryTypes = territoryTypes.Where(t => t.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameKK":
                    territoryTypes = territoryTypes.OrderBy(t => t.NameKK);
                    break;
                case "NameKKDesc":
                    territoryTypes = territoryTypes.OrderByDescending(t => t.NameKK);
                    break;
                case "NameRU":
                    territoryTypes = territoryTypes.OrderBy(t => t.NameRU);
                    break;
                case "NameRUDesc":
                    territoryTypes = territoryTypes.OrderByDescending(t => t.NameRU);
                    break;
                default:
                    territoryTypes = territoryTypes.OrderBy(t => t.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(territoryTypes.Count(), Page);

            var viewModel = new TerritoryTypeIndexPageViewModel
            {
                Items = territoryTypes.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: TerritoryTypes/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var territoryType = await _context.TerritoryType
                .SingleOrDefaultAsync(m => m.Id == id);
            if (territoryType == null)
            {
                return NotFound();
            }

            return View(territoryType);
        }

        //// GET: TerritoryTypes/Create
        //[Authorize(Roles = "Administrator")]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: TerritoryTypes/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Administrator")]
        //public async Task<IActionResult> Create([Bind("Id,NameKK,NameRU,AdditionalInformationKK,AdditionalInformationRU")] TerritoryType territoryType)
        //{
        //    var territoryTypes = _context.TerritoryType.AsNoTracking().ToList();
        //    if (territoryTypes.Select(t => t.NameKK).Contains(territoryType.NameKK))
        //    {
        //        ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
        //    }
        //    if (territoryTypes.Select(t => t.NameRU).Contains(territoryType.NameRU))
        //    {
        //        ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
        //    }
        //    if(string.IsNullOrWhiteSpace(territoryType.NameKK))
        //    {
        //        ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
        //    }
        //    if (string.IsNullOrWhiteSpace(territoryType.NameRU))
        //    {
        //        ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(territoryType);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(territoryType);
        //}

        //// GET: TerritoryTypes/Edit/5
        //[Authorize(Roles = "Administrator")]
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var territoryType = await _context.TerritoryType.SingleOrDefaultAsync(m => m.Id == id);
        //    if (territoryType == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(territoryType);
        //}

        //// POST: TerritoryTypes/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Administrator")]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,NameKK,NameRU,AdditionalInformationKK,AdditionalInformationRU")] TerritoryType territoryType)
        //{
        //    if (id != territoryType.Id)
        //    {
        //        return NotFound();
        //    }
        //    var territoryTypes = _context.TerritoryType.AsNoTracking().ToList();
        //    if (territoryTypes.Where(t => t.Id != territoryType.Id).Select(t => t.NameKK).Contains(territoryType.NameKK))
        //    {
        //        ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
        //    }
        //    if (territoryTypes.Where(t => t.Id != territoryType.Id).Select(t => t.NameRU).Contains(territoryType.NameRU))
        //    {
        //        ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
        //    }
        //    if (string.IsNullOrWhiteSpace(territoryType.NameKK))
        //    {
        //        ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
        //    }
        //    if (string.IsNullOrWhiteSpace(territoryType.NameRU))
        //    {
        //        ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(territoryType);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TerritoryTypeExists(territoryType.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(territoryType);
        //}

        //// GET: TerritoryTypes/Delete/5
        //[Authorize(Roles = "Administrator")]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var territoryType = await _context.TerritoryType
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (territoryType == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewBag.TargetTerritories = _context.TargetTerritory
        //        .AsNoTracking()
        //        .Where(t => t.TerritoryTypeId == id)
        //        .OrderBy(t => t.TerritoryName);
        //    return View(territoryType);
        //}

        //// POST: TerritoryTypes/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Administrator")]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var territoryType = await _context.TerritoryType.SingleOrDefaultAsync(m => m.Id == id);
        //    if ((_context.TargetTerritory
        //        .AsNoTracking()
        //        .FirstOrDefault(t => t.TerritoryTypeId == id) == null))
        //    {
        //        _context.TerritoryType.Remove(territoryType);
        //        await _context.SaveChangesAsync();
        //    }
        //    return RedirectToAction(nameof(Index));
        //}

        private bool TerritoryTypeExists(int id)
        {
            return _context.TerritoryType.Any(e => e.Id == id);
        }
    }
}
