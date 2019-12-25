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
    public class SpeciesDiversitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SpeciesDiversitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SpeciesDiversities
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            int? CityDistrictId,
            int? PlantationsTypeId,
            int? Page)
        {
            var speciesDiversitys = _context.SpeciesDiversity
                .Include(p => p.CityDistrict)
                .Include(p => p.PlantationsType)
                .Where(p => true);

            ViewBag.CityDistrictIdFilter = CityDistrictId;
            ViewBag.PlantationsTypeIdFilter = PlantationsTypeId;

            ViewBag.CityDistrictNameSort = SortOrder == "CityDistrictName" ? "CityDistrictNameDesc" : "CityDistrictName";
            ViewBag.PlantationsTypeNameSort = SortOrder == "PlantationsTypeName" ? "PlantationsTypeNameDesc" : "PlantationsTypeName";

            if (CityDistrictId != null)
            {
                speciesDiversitys = speciesDiversitys.Where(p => p.CityDistrictId == CityDistrictId);
            }
            if (PlantationsTypeId != null)
            {
                speciesDiversitys = speciesDiversitys.Where(p => p.PlantationsTypeId == PlantationsTypeId);
            }

            switch (SortOrder)
            {
                case "CityDistrictName":
                    speciesDiversitys = speciesDiversitys.OrderBy(p => p.CityDistrict.Name);
                    break;
                case "CityDistrictNameDesc":
                    speciesDiversitys = speciesDiversitys.OrderByDescending(p => p.CityDistrict.Name);
                    break;
                case "PlantationsTypeName":
                    speciesDiversitys = speciesDiversitys.OrderBy(p => p.PlantationsType.Name);
                    break;
                case "PlantationsTypeNameDesc":
                    speciesDiversitys = speciesDiversitys.OrderByDescending(p => p.PlantationsType.Name);
                    break;
                default:
                    speciesDiversitys = speciesDiversitys.OrderBy(p => p.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(speciesDiversitys.Count(), Page);

            var viewModel = new SpeciesDiversityIndexPageViewModel
            {
                Items = speciesDiversitys.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewBag.CityDistrictId = new SelectList(_context.CityDistrict.OrderBy(t => t.Name), "Id", "Name");
            ViewBag.PlantationsTypeId = new SelectList(_context.PlantationsType.OrderBy(t => t.Name), "Id", "Name");

            return View(viewModel);
        }

        // GET: SpeciesDiversities/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speciesDiversity = await _context.SpeciesDiversity
                .Include(s => s.CityDistrict)
                .Include(s => s.PlantationsType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (speciesDiversity == null)
            {
                return NotFound();
            }

            return View(speciesDiversity);
        }

        // GET: SpeciesDiversities/Create
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public IActionResult Create()
        {
            ViewBag.CityDistrictId = new SelectList(_context.CityDistrict.OrderBy(t => t.Name), "Id", "Name");
            ViewBag.PlantationsTypeId = new SelectList(_context.PlantationsType.OrderBy(t => t.Name), "Id", "Name");
            return View();
        }

        // POST: SpeciesDiversities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Create([Bind("Id,CityDistrictId,PlantationsTypeId,TreesNumber")] SpeciesDiversity speciesDiversity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(speciesDiversity);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "SpeciesDiversity",
                    New = speciesDiversity.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CityDistrictId = new SelectList(_context.CityDistrict.OrderBy(t => t.Name), "Id", "Name", speciesDiversity.CityDistrictId);
            ViewBag.PlantationsTypeId = new SelectList(_context.PlantationsType.OrderBy(t => t.Name), "Id", "Name", speciesDiversity.PlantationsTypeId);
            return View(speciesDiversity);
        }

        // GET: SpeciesDiversities/Edit/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speciesDiversity = await _context.SpeciesDiversity.SingleOrDefaultAsync(m => m.Id == id);
            if (speciesDiversity == null)
            {
                return NotFound();
            }
            ViewBag.CityDistrictId = new SelectList(_context.CityDistrict.OrderBy(t => t.Name), "Id", "Name", speciesDiversity.CityDistrictId);
            ViewBag.PlantationsTypeId = new SelectList(_context.PlantationsType.OrderBy(t => t.Name), "Id", "Name", speciesDiversity.PlantationsTypeId);
            return View(speciesDiversity);
        }

        // POST: SpeciesDiversities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CityDistrictId,PlantationsTypeId,TreesNumber")] SpeciesDiversity speciesDiversity)
        {
            if (id != speciesDiversity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var speciesDiversity_old = _context.SpeciesDiversity.AsNoTracking().FirstOrDefault(t => t.Id == speciesDiversity.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "SpeciesDiversity",
                        Operation = "Edit",
                        New = speciesDiversity.ToString(),
                        Old = speciesDiversity_old.ToString()
                    });
                    _context.Update(speciesDiversity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpeciesDiversityExists(speciesDiversity.Id))
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
            ViewBag.CityDistrictId = new SelectList(_context.CityDistrict.OrderBy(t => t.Name), "Id", "Name", speciesDiversity.CityDistrictId);
            ViewBag.PlantationsTypeId = new SelectList(_context.PlantationsType.OrderBy(t => t.Name), "Id", "Name", speciesDiversity.PlantationsTypeId);
            return View(speciesDiversity);
        }

        // GET: SpeciesDiversities/Delete/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speciesDiversity = await _context.SpeciesDiversity
                .Include(s => s.CityDistrict)
                .Include(s => s.PlantationsType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (speciesDiversity == null)
            {
                return NotFound();
            }

            return View(speciesDiversity);
        }

        // POST: SpeciesDiversities/Delete/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var speciesDiversity = await _context.SpeciesDiversity.SingleOrDefaultAsync(m => m.Id == id);
            _context.SpeciesDiversity.Remove(speciesDiversity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpeciesDiversityExists(int id)
        {
            return _context.SpeciesDiversity.Any(e => e.Id == id);
        }
    }
}
