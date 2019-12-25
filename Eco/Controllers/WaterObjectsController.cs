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
    public class WaterObjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public WaterObjectsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: WaterObjects
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string NameKK, string NameRU, int? Page)
        {
            var waterObjects = _context.WaterObject
                .Where(w => true);

            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(NameKK))
            {
                waterObjects = waterObjects.Where(w => w.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                waterObjects = waterObjects.Where(w => w.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameKK":
                    waterObjects = waterObjects.OrderBy(w => w.NameKK);
                    break;
                case "NameKKDesc":
                    waterObjects = waterObjects.OrderByDescending(w => w.NameKK);
                    break;
                case "NameRU":
                    waterObjects = waterObjects.OrderBy(w => w.NameRU);
                    break;
                case "NameRUDesc":
                    waterObjects = waterObjects.OrderByDescending(w => w.NameRU);
                    break;
                default:
                    waterObjects = waterObjects.OrderBy(w => w.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(waterObjects.Count(), Page);

            var viewModel = new WaterObjectIndexPageViewModel
            {
                Items = waterObjects.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: WaterObjects/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterObject = await _context.WaterObject
                .SingleOrDefaultAsync(m => m.Id == id);
            if (waterObject == null)
            {
                return NotFound();
            }

            return View(waterObject);
        }

        // GET: WaterObjects/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: WaterObjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,NameKK,NameRU")] WaterObject waterObject)
        {
            var waterObjects = _context.WaterObject.AsNoTracking().ToList();
            if (waterObjects.Select(w => w.NameKK).Contains(waterObject.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (waterObjects.Select(w => w.NameRU).Contains(waterObject.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(waterObject.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(waterObject.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(waterObject);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "WaterObject",
                    New = waterObject.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(waterObject);
        }

        // GET: WaterObjects/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterObject = await _context.WaterObject.SingleOrDefaultAsync(m => m.Id == id);
            if (waterObject == null)
            {
                return NotFound();
            }
            return View(waterObject);
        }

        // POST: WaterObjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameKK,NameRU")] WaterObject waterObject)
        {
            if (id != waterObject.Id)
            {
                return NotFound();
            }
            var waterObjects = _context.WaterObject.AsNoTracking().ToList();
            if (waterObjects.Where(w => w.Id != waterObject.Id).Select(w => w.NameKK).Contains(waterObject.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (waterObjects.Where(w => w.Id != waterObject.Id).Select(w => w.NameRU).Contains(waterObject.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(waterObject.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(waterObject.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var waterObject_old = _context.WaterObject.AsNoTracking().FirstOrDefault(w => w.Id == waterObject.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "WaterObject",
                        Operation = "Edit",
                        New = waterObject.ToString(),
                        Old = waterObject_old.ToString()
                    });
                    _context.Update(waterObject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WaterObjectExists(waterObject.Id))
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
            return View(waterObject);
        }

        // GET: WaterObjects/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterObject = await _context.WaterObject
                .SingleOrDefaultAsync(m => m.Id == id);
            if (waterObject == null)
            {
                return NotFound();
            }
            ViewBag.WaterSurfacePosts = _context.WaterSurfacePost
                .AsNoTracking()
                .Where(k => k.WaterObjectId == id)
                .OrderBy(k => k.Number);
            return View(waterObject);
        }

        // POST: WaterObjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var waterObject = await _context.WaterObject.SingleOrDefaultAsync(m => m.Id == id);
            if (_context.WaterSurfacePost
                .AsNoTracking()
                .FirstOrDefault(k => k.WaterObjectId == id) == null)
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "WaterObject",
                    Operation = "Delete",
                    New = "",
                    Old = waterObject.ToString()
                });
                _context.WaterObject.Remove(waterObject);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool WaterObjectExists(int id)
        {
            return _context.WaterObject.Any(e => e.Id == id);
        }
    }
}
