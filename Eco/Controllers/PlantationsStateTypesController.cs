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
    public class PlantationsStateTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public PlantationsStateTypesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: PlantationsStateTypes
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string NameKK, string NameRU, int? Page)
        {
            var plantationsStateTypes = _context.PlantationsStateType
                .Where(s => true);

            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(NameKK))
            {
                plantationsStateTypes = plantationsStateTypes.Where(s => s.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                plantationsStateTypes = plantationsStateTypes.Where(s => s.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameKK":
                    plantationsStateTypes = plantationsStateTypes.OrderBy(s => s.NameKK);
                    break;
                case "NameKKDesc":
                    plantationsStateTypes = plantationsStateTypes.OrderByDescending(s => s.NameKK);
                    break;
                case "NameRU":
                    plantationsStateTypes = plantationsStateTypes.OrderBy(s => s.NameRU);
                    break;
                case "NameRUDesc":
                    plantationsStateTypes = plantationsStateTypes.OrderByDescending(s => s.NameRU);
                    break;
                default:
                    plantationsStateTypes = plantationsStateTypes.OrderBy(s => s.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(plantationsStateTypes.Count(), Page);

            var viewModel = new PlantationsStateTypeIndexPageViewModel
            {
                Items = plantationsStateTypes.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: PlantationsStateTypes/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantationsStateType = await _context.PlantationsStateType
                .SingleOrDefaultAsync(m => m.Id == id);
            if (plantationsStateType == null)
            {
                return NotFound();
            }

            return View(plantationsStateType);
        }

        // GET: PlantationsStateTypes/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlantationsStateTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,NameKK,NameRU")] PlantationsStateType plantationsStateType)
        {
            var plantationsStateTypes = _context.PlantationsStateType.AsNoTracking().ToList();
            if (plantationsStateTypes.Select(s => s.NameKK).Contains(plantationsStateType.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (plantationsStateTypes.Select(s => s.NameRU).Contains(plantationsStateType.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(plantationsStateType.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(plantationsStateType.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(plantationsStateType);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "PlantationsStateType",
                    New = plantationsStateType.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(plantationsStateType);
        }

        // GET: PlantationsStateTypes/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantationsStateType = await _context.PlantationsStateType.SingleOrDefaultAsync(m => m.Id == id);
            if (plantationsStateType == null)
            {
                return NotFound();
            }
            return View(plantationsStateType);
        }

        // POST: PlantationsStateTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameKK,NameRU")] PlantationsStateType plantationsStateType)
        {
            if (id != plantationsStateType.Id)
            {
                return NotFound();
            }
            var plantationsStateTypes = _context.PlantationsStateType.AsNoTracking().ToList();
            if (plantationsStateTypes.Where(s => s.Id != plantationsStateType.Id).Select(s => s.NameKK).Contains(plantationsStateType.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (plantationsStateTypes.Where(s => s.Id != plantationsStateType.Id).Select(s => s.NameRU).Contains(plantationsStateType.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(plantationsStateType.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(plantationsStateType.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var plantationsStateType_old = _context.PlantationsStateType.AsNoTracking().FirstOrDefault(s => s.Id == plantationsStateType.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "PlantationsStateType",
                        Operation = "Edit",
                        New = plantationsStateType.ToString(),
                        Old = plantationsStateType_old.ToString()
                    });
                    _context.Update(plantationsStateType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlantationsStateTypeExists(plantationsStateType.Id))
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
            return View(plantationsStateType);
        }

        // GET: PlantationsStateTypes/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantationsStateType = await _context.PlantationsStateType
                .SingleOrDefaultAsync(m => m.Id == id);
            if (plantationsStateType == null)
            {
                return NotFound();
            }
            //ViewBag.KazHydrometAirPosts = _context.KazHydrometAirPost
            //    .AsNoTracking()
            //    .Where(k => k.PlantationsStateTypeId == id)
            //    .OrderBy(k => k.Number);
            return View(plantationsStateType);
        }

        // POST: PlantationsStateTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plantationsStateType = await _context.PlantationsStateType.SingleOrDefaultAsync(m => m.Id == id);
            //if (_context.KazHydrometAirPost
            //    .AsNoTracking()
            //    .FirstOrDefault(k => k.PlantationsStateTypeId == id) == null)
            //{
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "PlantationsStateType",
                    Operation = "Delete",
                    New = "",
                    Old = plantationsStateType.ToString()
                });
                _context.PlantationsStateType.Remove(plantationsStateType);
                await _context.SaveChangesAsync();
            //}
            return RedirectToAction(nameof(Index));
        }

        private bool PlantationsStateTypeExists(int id)
        {
            return _context.PlantationsStateType.Any(e => e.Id == id);
        }
    }
}
