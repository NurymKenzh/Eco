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

namespace Eco.Controllers
{
    public class PlantationsStatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlantationsStatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PlantationsStates
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            int? CityDistrictId,
            int? PlantationsStateTypeId,
            int? Page)
        {
            var plantationsStates = _context.PlantationsState
                .Include(p => p.CityDistrict)
                .Include(p => p.PlantationsStateType)
                .Where(p => true);

            ViewBag.CityDistrictIdFilter = CityDistrictId;
            ViewBag.PlantationsStateTypeIdFilter = PlantationsStateTypeId;

            ViewBag.CityDistrictNameSort = SortOrder == "CityDistrictName" ? "CityDistrictNameDesc" : "CityDistrictName";
            ViewBag.PlantationsStateTypeNameSort = SortOrder == "PlantationsStateTypeName" ? "PlantationsStateTypeNameDesc" : "PlantationsStateTypeName";

            if (CityDistrictId != null)
            {
                plantationsStates = plantationsStates.Where(p => p.CityDistrictId == CityDistrictId);
            }
            if (PlantationsStateTypeId != null)
            {
                plantationsStates = plantationsStates.Where(p => p.PlantationsStateTypeId == PlantationsStateTypeId);
            }

            switch (SortOrder)
            {
                case "CityDistrictName":
                    plantationsStates = plantationsStates.OrderBy(p => p.CityDistrict.Name);
                    break;
                case "CityDistrictNameDesc":
                    plantationsStates = plantationsStates.OrderByDescending(p => p.CityDistrict.Name);
                    break;
                case "PlantationsStateTypeName":
                    plantationsStates = plantationsStates.OrderBy(p => p.PlantationsStateType.Name);
                    break;
                case "PlantationsStateTypeNameDesc":
                    plantationsStates = plantationsStates.OrderByDescending(p => p.PlantationsStateType.Name);
                    break;
                default:
                    plantationsStates = plantationsStates.OrderBy(p => p.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(plantationsStates.Count(), Page);

            var viewModel = new PlantationsStateIndexPageViewModel
            {
                Items = plantationsStates.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewBag.CityDistrictId = new SelectList(_context.CityDistrict.OrderBy(t => t.Name), "Id", "Name");
            ViewBag.PlantationsStateTypeId = new SelectList(_context.PlantationsStateType.OrderBy(t => t.Name), "Id", "Name");

            return View(viewModel);
        }

        // GET: PlantationsStates/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantationsState = await _context.PlantationsState
                .Include(p => p.CityDistrict)
                .Include(p => p.PlantationsStateType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (plantationsState == null)
            {
                return NotFound();
            }

            return View(plantationsState);
        }

        // GET: PlantationsStates/Create
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public IActionResult Create()
        {
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(k => k.Name), "Id", "Name");
            ViewData["PlantationsStateTypeId"] = new SelectList(_context.PlantationsStateType.OrderBy(k => k.Name), "Id", "Name");
            return View();
        }

        // POST: PlantationsStates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Create([Bind("Id,CityDistrictId,PlantationsStateTypeId,TreesNumber")] PlantationsState plantationsState)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plantationsState);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "PlantationsState",
                    New = plantationsState.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(k => k.Name), "Id", "Name", plantationsState.CityDistrictId);
            ViewData["PlantationsStateTypeId"] = new SelectList(_context.PlantationsStateType.OrderBy(k => k.Name), "Id", "Name", plantationsState.PlantationsStateTypeId);
            return View(plantationsState);
        }

        // GET: PlantationsStates/Edit/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantationsState = await _context.PlantationsState.SingleOrDefaultAsync(m => m.Id == id);
            if (plantationsState == null)
            {
                return NotFound();
            }
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(k => k.Name), "Id", "Name", plantationsState.CityDistrictId);
            ViewData["PlantationsStateTypeId"] = new SelectList(_context.PlantationsStateType.OrderBy(k => k.Name), "Id", "Name", plantationsState.PlantationsStateTypeId);
            return View(plantationsState);
        }

        // POST: PlantationsStates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CityDistrictId,PlantationsStateTypeId,TreesNumber")] PlantationsState plantationsState)
        {
            if (id != plantationsState.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var plantationsState_old = _context.PlantationsState.AsNoTracking().FirstOrDefault(t => t.Id == plantationsState.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "PlantationsState",
                        Operation = "Edit",
                        New = plantationsState.ToString(),
                        Old = plantationsState_old.ToString()
                    });
                    _context.Update(plantationsState);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlantationsStateExists(plantationsState.Id))
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
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(k => k.Name), "Id", "Name", plantationsState.CityDistrictId);
            ViewData["PlantationsStateTypeId"] = new SelectList(_context.PlantationsStateType.OrderBy(k => k.Name), "Id", "Name", plantationsState.PlantationsStateTypeId);
            return View(plantationsState);
        }

        // GET: PlantationsStates/Delete/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantationsState = await _context.PlantationsState
                .Include(p => p.CityDistrict)
                .Include(p => p.PlantationsStateType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (plantationsState == null)
            {
                return NotFound();
            }

            return View(plantationsState);
        }

        // POST: PlantationsStates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plantationsState = await _context.PlantationsState.SingleOrDefaultAsync(m => m.Id == id);
            _context.PlantationsState.Remove(plantationsState);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlantationsStateExists(int id)
        {
            return _context.PlantationsState.Any(e => e.Id == id);
        }
    }
}
