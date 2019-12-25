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
    public class PlantationsTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public PlantationsTypesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: PlantationsTypes
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string NameKK, string NameRU, int? Page)
        {
            var plantationsTypes = _context.PlantationsType
                .Where(e => true);

            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(NameKK))
            {
                plantationsTypes = plantationsTypes.Where(e => e.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                plantationsTypes = plantationsTypes.Where(e => e.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameKK":
                    plantationsTypes = plantationsTypes.OrderBy(e => e.NameKK);
                    break;
                case "NameKKDesc":
                    plantationsTypes = plantationsTypes.OrderByDescending(e => e.NameKK);
                    break;
                case "NameRU":
                    plantationsTypes = plantationsTypes.OrderBy(e => e.NameRU);
                    break;
                case "NameRUDesc":
                    plantationsTypes = plantationsTypes.OrderByDescending(e => e.NameRU);
                    break;
                default:
                    plantationsTypes = plantationsTypes.OrderBy(e => e.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(plantationsTypes.Count(), Page);

            var viewModel = new PlantationsTypeIndexPageViewModel
            {
                Items = plantationsTypes.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: PlantationsTypes/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantationsType = await _context.PlantationsType
                .SingleOrDefaultAsync(m => m.Id == id);
            if (plantationsType == null)
            {
                return NotFound();
            }

            return View(plantationsType);
        }

        // GET: PlantationsTypes/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlantationsTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,NameKK,NameRU")] PlantationsType plantationsType)
        {
            var plantationsTypes = _context.PlantationsType.AsNoTracking().ToList();
            if (plantationsTypes.Select(e => e.NameKK).Contains(plantationsType.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (plantationsTypes.Select(e => e.NameRU).Contains(plantationsType.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(plantationsType.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(plantationsType.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(plantationsType);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "PlantationsType",
                    New = plantationsType.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(plantationsType);
        }

        // GET: PlantationsTypes/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantationsType = await _context.PlantationsType.SingleOrDefaultAsync(m => m.Id == id);
            if (plantationsType == null)
            {
                return NotFound();
            }
            return View(plantationsType);
        }

        // POST: PlantationsTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameKK,NameRU")] PlantationsType plantationsType)
        {
            if (id != plantationsType.Id)
            {
                return NotFound();
            }
            var plantationsTypes = _context.PlantationsType.AsNoTracking().ToList();
            if (plantationsTypes.Where(e => e.Id != plantationsType.Id).Select(e => e.NameKK).Contains(plantationsType.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (plantationsTypes.Where(e => e.Id != plantationsType.Id).Select(e => e.NameRU).Contains(plantationsType.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(plantationsType.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(plantationsType.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var plantationsType_old = _context.PlantationsType.AsNoTracking().FirstOrDefault(e => e.Id == plantationsType.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "PlantationsType",
                        Operation = "Edit",
                        New = plantationsType.ToString(),
                        Old = plantationsType_old.ToString()
                    });
                    _context.Update(plantationsType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlantationsTypeExists(plantationsType.Id))
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
            return View(plantationsType);
        }

        // GET: PlantationsTypes/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantationsType = await _context.PlantationsType
                .SingleOrDefaultAsync(m => m.Id == id);
            if (plantationsType == null)
            {
                return NotFound();
            }
            //ViewBag.AActivities = _context.AActivity
            //    .AsNoTracking()
            //    .Where(a => a.PlantationsTypeId == id);
            return View(plantationsType);
        }

        // POST: PlantationsTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plantationsType = await _context.PlantationsType.SingleOrDefaultAsync(m => m.Id == id);
            //if (_context.AActivity
            //    .AsNoTracking()
            //    .FirstOrDefault(a => a.PlantationsTypeId == id) == null)
            //{
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "PlantationsType",
                    Operation = "Delete",
                    New = "",
                    Old = plantationsType.ToString()
                });
                _context.PlantationsType.Remove(plantationsType);
                await _context.SaveChangesAsync();
            //}
            return RedirectToAction(nameof(Index));
        }

        private bool PlantationsTypeExists(int id)
        {
            return _context.PlantationsType.Any(e => e.Id == id);
        }
    }
}
