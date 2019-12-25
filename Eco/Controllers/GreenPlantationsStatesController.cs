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
    public class GreenPlantationsStatesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public GreenPlantationsStatesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: GreenPlantationsStates
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            int? CityDistrictId,
            string NameKK,
            string NameRU,
            int? GreenPlantationsTypeId,
            int? Page)
        {
            var greenPlantationsStates = _context.GreenPlantationsState
                .Include(g => g.CityDistrict)
                .Include(g => g.GreenPlantationsType)
                .Where(c => true);

            ViewBag.CityDistrictIdFilter = CityDistrictId;
            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;
            ViewBag.GreenPlantationsTypeIdFilter = GreenPlantationsTypeId;

            ViewBag.CityDistrictIdSort = SortOrder == "CityDistrictId" ? "CityDistrictIdDesc" : "CityDistrictId";
            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";
            ViewBag.GreenPlantationsTypeIdSort = SortOrder == "GreenPlantationsTypeId" ? "GreenPlantationsTypeIdDesc" : "GreenPlantationsTypeId";

            if (CityDistrictId != null)
            {
                greenPlantationsStates = greenPlantationsStates.Where(c => c.CityDistrictId == CityDistrictId);
            }
            if (!string.IsNullOrEmpty(NameKK))
            {
                greenPlantationsStates = greenPlantationsStates.Where(s => s.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                greenPlantationsStates = greenPlantationsStates.Where(s => s.NameRU.ToLower().Contains(NameRU.ToLower()));
            }
            if (GreenPlantationsTypeId != null)
            {
                greenPlantationsStates = greenPlantationsStates.Where(c => c.GreenPlantationsTypeId == GreenPlantationsTypeId);
            }

            switch (SortOrder)
            {
                case "CityDistrictId":
                    greenPlantationsStates = greenPlantationsStates.OrderBy(c => c.CityDistrict.Name);
                    break;
                case "CityDistrictIdDesc":
                    greenPlantationsStates = greenPlantationsStates.OrderByDescending(c => c.CityDistrict.Name);
                    break;
                case "NameKK":
                    greenPlantationsStates = greenPlantationsStates.OrderBy(c => c.NameKK);
                    break;
                case "NameKKDesc":
                    greenPlantationsStates = greenPlantationsStates.OrderByDescending(c => c.NameKK);
                    break;
                case "NameRU":
                    greenPlantationsStates = greenPlantationsStates.OrderBy(c => c.NameRU);
                    break;
                case "NameRUDesc":
                    greenPlantationsStates = greenPlantationsStates.OrderByDescending(c => c.NameRU);
                    break;
                case "GreenPlantationsTypeId":
                    greenPlantationsStates = greenPlantationsStates.OrderBy(c => c.GreenPlantationsType.Name);
                    break;
                case "GreenPlantationsTypeIdDesc":
                    greenPlantationsStates = greenPlantationsStates.OrderByDescending(c => c.GreenPlantationsType.Name);
                    break;
                default:
                    greenPlantationsStates = greenPlantationsStates.OrderBy(c => c.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(greenPlantationsStates.Count(), Page);

            var viewModel = new GreenPlantationsStateIndexPageViewModel
            {
                Items = greenPlantationsStates.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewBag.CityDistrictId = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name");
            ViewBag.GreenPlantationsTypeId = new SelectList(_context.GreenPlantationsType.OrderBy(c => c.Name), "Id", "Name");

            return View(viewModel);
        }

        // GET: GreenPlantationsStates/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var greenPlantationsState = await _context.GreenPlantationsState
                .Include(g => g.CityDistrict)
                .Include(g => g.GreenPlantationsType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (greenPlantationsState == null)
            {
                return NotFound();
            }

            return View(greenPlantationsState);
        }

        // GET: GreenPlantationsStates/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name");
            ViewData["GreenPlantationsTypeId"] = new SelectList(_context.GreenPlantationsType.OrderBy(c => c.Name), "Id", "Name");
            return View();
        }

        // POST: GreenPlantationsStates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,CityDistrictId,NameKK,NameRU,GreenPlantationsTypeId,Areahectares,AdditionalInformationKK,AdditionalInformationRU")] GreenPlantationsState greenPlantationsState)
        {
            var greenPlantationsStates = _context.GreenPlantationsState.AsNoTracking().ToList();
            if (greenPlantationsStates.Select(s => s.NameKK).Contains(greenPlantationsState.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (greenPlantationsStates.Select(s => s.NameRU).Contains(greenPlantationsState.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(greenPlantationsState.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(greenPlantationsState.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(greenPlantationsState);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "GreenPlantationsState",
                    New = greenPlantationsState.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name", greenPlantationsState.CityDistrictId);
            ViewData["GreenPlantationsTypeId"] = new SelectList(_context.GreenPlantationsType.OrderBy(c => c.Name), "Id", "Name", greenPlantationsState.GreenPlantationsTypeId);
            return View(greenPlantationsState);
        }

        // GET: GreenPlantationsStates/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var greenPlantationsState = await _context.GreenPlantationsState.SingleOrDefaultAsync(m => m.Id == id);
            if (greenPlantationsState == null)
            {
                return NotFound();
            }
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name", greenPlantationsState.CityDistrictId);
            ViewData["GreenPlantationsTypeId"] = new SelectList(_context.GreenPlantationsType.OrderBy(c => c.Name), "Id", "Name", greenPlantationsState.GreenPlantationsTypeId);
            return View(greenPlantationsState);
        }

        // POST: GreenPlantationsStates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CityDistrictId,NameKK,NameRU,GreenPlantationsTypeId,Areahectares,AdditionalInformationKK,AdditionalInformationRU")] GreenPlantationsState greenPlantationsState)
        {
            if (id != greenPlantationsState.Id)
            {
                return NotFound();
            }
            var greenPlantationsStates = _context.GreenPlantationsState.AsNoTracking().ToList();
            if (greenPlantationsStates.Where(s => s.Id != greenPlantationsState.Id).Select(s => s.NameKK).Contains(greenPlantationsState.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (greenPlantationsStates.Where(s => s.Id != greenPlantationsState.Id).Select(s => s.NameRU).Contains(greenPlantationsState.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(greenPlantationsState.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(greenPlantationsState.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var greenPlantationsState_old = _context.GreenPlantationsState.AsNoTracking().FirstOrDefault(c => c.Id == greenPlantationsState.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "GreenPlantationsState",
                        Operation = "Edit",
                        New = greenPlantationsState.ToString(),
                        Old = greenPlantationsState_old.ToString()
                    });
                    _context.Update(greenPlantationsState);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GreenPlantationsStateExists(greenPlantationsState.Id))
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
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name", greenPlantationsState.CityDistrictId);
            ViewData["GreenPlantationsTypeId"] = new SelectList(_context.GreenPlantationsType.OrderBy(c => c.Name), "Id", "Name", greenPlantationsState.GreenPlantationsTypeId);
            return View(greenPlantationsState);
        }

        // GET: GreenPlantationsStates/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var greenPlantationsState = await _context.GreenPlantationsState
                .Include(g => g.CityDistrict)
                .Include(g => g.GreenPlantationsType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (greenPlantationsState == null)
            {
                return NotFound();
            }

            return View(greenPlantationsState);
        }

        // POST: GreenPlantationsStates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var greenPlantationsState = await _context.GreenPlantationsState.SingleOrDefaultAsync(m => m.Id == id);
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Class = "GreenPlantationsState",
                Operation = "Delete",
                New = "",
                Old = greenPlantationsState.ToString()
            });
            _context.GreenPlantationsState.Remove(greenPlantationsState);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GreenPlantationsStateExists(int id)
        {
            return _context.GreenPlantationsState.Any(e => e.Id == id);
        }
    }
}
