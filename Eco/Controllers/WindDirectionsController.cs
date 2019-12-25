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
    public class WindDirectionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public WindDirectionsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: WindDirections
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string NameKK, string NameRU, int? Page)
        {
            var windDirections = _context.WindDirection
                .Where(w => true);

            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(NameKK))
            {
                windDirections = windDirections.Where(w => w.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                windDirections = windDirections.Where(w => w.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameKK":
                    windDirections = windDirections.OrderBy(w => w.NameKK);
                    break;
                case "NameKKDesc":
                    windDirections = windDirections.OrderByDescending(w => w.NameKK);
                    break;
                case "NameRU":
                    windDirections = windDirections.OrderBy(w => w.NameRU);
                    break;
                case "NameRUDesc":
                    windDirections = windDirections.OrderByDescending(w => w.NameRU);
                    break;
                default:
                    windDirections = windDirections.OrderBy(w => w.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(windDirections.Count(), Page);

            var viewModel = new WindDirectionIndexPageViewModel
            {
                Items = windDirections.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: WindDirections/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var windDirection = await _context.WindDirection
                .SingleOrDefaultAsync(m => m.Id == id);
            if (windDirection == null)
            {
                return NotFound();
            }

            return View(windDirection);
        }

        // GET: WindDirections/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: WindDirections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,NameKK,NameRU,Default")] WindDirection windDirection)
        {
            var windDirections = _context.WindDirection.AsNoTracking().ToList();
            if (windDirections.Select(w => w.NameKK).Contains(windDirection.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (windDirections.Select(w => w.NameRU).Contains(windDirection.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(windDirection.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(windDirection.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(windDirection);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "WindDirection",
                    New = windDirection.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(windDirection);
        }

        // GET: WindDirections/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var windDirection = await _context.WindDirection.SingleOrDefaultAsync(m => m.Id == id);
            if (windDirection == null)
            {
                return NotFound();
            }
            return View(windDirection);
        }

        // POST: WindDirections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameKK,NameRU,Default")] WindDirection windDirection)
        {
            if (id != windDirection.Id)
            {
                return NotFound();
            }
            var windDirections = _context.WindDirection.AsNoTracking().ToList();
            if (windDirections.Where(w => w.Id != windDirection.Id).Select(w => w.NameKK).Contains(windDirection.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (windDirections.Where(w => w.Id != windDirection.Id).Select(w => w.NameRU).Contains(windDirection.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(windDirection.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(windDirection.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var windDirection_old = _context.WindDirection.AsNoTracking().FirstOrDefault(w => w.Id == windDirection.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "WindDirection",
                        Operation = "Edit",
                        New = windDirection.ToString(),
                        Old = windDirection_old.ToString()
                    });
                    _context.Update(windDirection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WindDirectionExists(windDirection.Id))
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
            return View(windDirection);
        }

        // GET: WindDirections/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var windDirection = await _context.WindDirection
                .SingleOrDefaultAsync(m => m.Id == id);
            if (windDirection == null)
            {
                return NotFound();
            }
            ViewBag.AirPostDatas = _context.AirPostData
                .AsNoTracking()
                .Where(a => a.AirPostId == id)
                .Take(50)
                .OrderBy(a => a.DateTime.Year);
            return View(windDirection);
        }

        // POST: WindDirections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var windDirection = await _context.WindDirection.SingleOrDefaultAsync(m => m.Id == id);
            if (_context.AirPostData
                .AsNoTracking()
                .FirstOrDefault(a => a.AirPostId == id) == null)
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "WindDirection",
                    Operation = "Delete",
                    New = "",
                    Old = windDirection.ToString()
                });
                _context.WindDirection.Remove(windDirection);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool WindDirectionExists(int id)
        {
            return _context.WindDirection.Any(e => e.Id == id);
        }
    }
}
