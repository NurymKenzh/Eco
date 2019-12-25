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
    public class GreenPlantationsTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public GreenPlantationsTypesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: GreenPlantationsTypes
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string NameKK, string NameRU, int? Page)
        {
            var greenPlantationsTypes = _context.GreenPlantationsType
                .Where(s => true);

            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(NameKK))
            {
                greenPlantationsTypes = greenPlantationsTypes.Where(s => s.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                greenPlantationsTypes = greenPlantationsTypes.Where(s => s.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameKK":
                    greenPlantationsTypes = greenPlantationsTypes.OrderBy(s => s.NameKK);
                    break;
                case "NameKKDesc":
                    greenPlantationsTypes = greenPlantationsTypes.OrderByDescending(s => s.NameKK);
                    break;
                case "NameRU":
                    greenPlantationsTypes = greenPlantationsTypes.OrderBy(s => s.NameRU);
                    break;
                case "NameRUDesc":
                    greenPlantationsTypes = greenPlantationsTypes.OrderByDescending(s => s.NameRU);
                    break;
                default:
                    greenPlantationsTypes = greenPlantationsTypes.OrderBy(s => s.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(greenPlantationsTypes.Count(), Page);

            var viewModel = new GreenPlantationsTypeIndexPageViewModel
            {
                Items = greenPlantationsTypes.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: GreenPlantationsTypes/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var greenPlantationsType = await _context.GreenPlantationsType
                .SingleOrDefaultAsync(m => m.Id == id);
            if (greenPlantationsType == null)
            {
                return NotFound();
            }

            return View(greenPlantationsType);
        }

        // GET: GreenPlantationsTypes/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: GreenPlantationsTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,NameKK,NameRU")] GreenPlantationsType greenPlantationsType)
        {
            var greenPlantationsTypes = _context.GreenPlantationsType.AsNoTracking().ToList();
            if (greenPlantationsTypes.Select(s => s.NameKK).Contains(greenPlantationsType.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (greenPlantationsTypes.Select(s => s.NameRU).Contains(greenPlantationsType.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(greenPlantationsType.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(greenPlantationsType.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(greenPlantationsType);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "GreenPlantationsType",
                    New = greenPlantationsType.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(greenPlantationsType);
        }

        // GET: GreenPlantationsTypes/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var greenPlantationsType = await _context.GreenPlantationsType.SingleOrDefaultAsync(m => m.Id == id);
            if (greenPlantationsType == null)
            {
                return NotFound();
            }
            return View(greenPlantationsType);
        }

        // POST: GreenPlantationsTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameKK,NameRU")] GreenPlantationsType greenPlantationsType)
        {
            if (id != greenPlantationsType.Id)
            {
                return NotFound();
            }
            var greenPlantationsTypes = _context.GreenPlantationsType.AsNoTracking().ToList();
            if (greenPlantationsTypes.Where(s => s.Id != greenPlantationsType.Id).Select(s => s.NameKK).Contains(greenPlantationsType.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (greenPlantationsTypes.Where(s => s.Id != greenPlantationsType.Id).Select(s => s.NameRU).Contains(greenPlantationsType.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(greenPlantationsType.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(greenPlantationsType.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var greenPlantationsType_old = _context.GreenPlantationsType.AsNoTracking().FirstOrDefault(s => s.Id == greenPlantationsType.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "GreenPlantationsType",
                        Operation = "Edit",
                        New = greenPlantationsType.ToString(),
                        Old = greenPlantationsType_old.ToString()
                    });
                    _context.Update(greenPlantationsType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GreenPlantationsTypeExists(greenPlantationsType.Id))
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
            return View(greenPlantationsType);
        }

        // GET: GreenPlantationsTypes/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var greenPlantationsType = await _context.GreenPlantationsType
                .SingleOrDefaultAsync(m => m.Id == id);
            if (greenPlantationsType == null)
            {
                return NotFound();
            }
            ViewBag.GreenPlantationsStates = _context.GreenPlantationsState
                .AsNoTracking()
                .Where(s => s.GreenPlantationsTypeId == id)
                .OrderBy(s => s.Name);
            return View(greenPlantationsType);
        }

        // POST: GreenPlantationsTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var greenPlantationsType = await _context.GreenPlantationsType.SingleOrDefaultAsync(m => m.Id == id);
            if ((_context.GreenPlantationsState
                .AsNoTracking()
                .FirstOrDefault(s => s.GreenPlantationsTypeId == id) == null))
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "GreenPlantationsType",
                    Operation = "Delete",
                    New = "",
                    Old = greenPlantationsType.ToString()
                });
                _context.GreenPlantationsType.Remove(greenPlantationsType);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool GreenPlantationsTypeExists(int id)
        {
            return _context.GreenPlantationsType.Any(e => e.Id == id);
        }
    }
}
