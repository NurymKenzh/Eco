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
    public class AirPollutionIndicatorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public AirPollutionIndicatorsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: AirPollutionIndicators
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            int? TypeOfAirPollutionIndicatorId,
            string Name,
            int? Page)
        {
            var airPollutionIndicators = _context.AirPollutionIndicator
                .Include(a => a.TypeOfAirPollutionIndicator)
                .Where(a => true);

            ViewBag.TypeOfAirPollutionIndicatorIdFilter = TypeOfAirPollutionIndicatorId;
            ViewBag.NameFilter = Name;

            ViewBag.TypeOfAirPollutionIndicatorIdSort = SortOrder == "TypeOfAirPollutionIndicatorId" ? "TypeOfAirPollutionIndicatorIdDesc" : "TypeOfAirPollutionIndicatorId";
            ViewBag.NameSort = SortOrder == "Name" ? "NameDesc" : "Name";

            if (TypeOfAirPollutionIndicatorId != null)
            {
                airPollutionIndicators = airPollutionIndicators.Where(a => a.TypeOfAirPollutionIndicatorId == TypeOfAirPollutionIndicatorId);
            }
            if (!string.IsNullOrEmpty(Name))
            {
                airPollutionIndicators = airPollutionIndicators.Where(a => a.Name.ToLower().Contains(Name.ToLower()));
            }

            switch (SortOrder)
            {
                case "TypeOfAirPollutionIndicatorId":
                    airPollutionIndicators = airPollutionIndicators.OrderBy(a => a.TypeOfAirPollutionIndicator.Name);
                    break;
                case "TypeOfAirPollutionIndicatorIdDesc":
                    airPollutionIndicators = airPollutionIndicators.OrderByDescending(a => a.TypeOfAirPollutionIndicator.Name);
                    break;
                case "Name":
                    airPollutionIndicators = airPollutionIndicators.OrderBy(a => a.Name);
                    break;
                case "NameDesc":
                    airPollutionIndicators = airPollutionIndicators.OrderByDescending(a => a.Name);
                    break;
                default:
                    airPollutionIndicators = airPollutionIndicators.OrderBy(a => a.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(airPollutionIndicators.Count(), Page);

            var viewModel = new AirPollutionIndicatorIndexPageViewModel
            {
                Items = airPollutionIndicators.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewBag.TypeOfAirPollutionIndicatorId = new SelectList(_context.TypeOfAirPollutionIndicator.OrderBy(a => a.Name), "Id", "Name");

            return View(viewModel);
        }

        // GET: AirPollutionIndicators/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airPollutionIndicator = await _context.AirPollutionIndicator
                .Include(a => a.TypeOfAirPollutionIndicator)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (airPollutionIndicator == null)
            {
                return NotFound();
            }

            return View(airPollutionIndicator);
        }

        // GET: AirPollutionIndicators/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["TypeOfAirPollutionIndicatorId"] = new SelectList(_context.TypeOfAirPollutionIndicator, "Id", "Name");
            return View();
        }

        // POST: AirPollutionIndicators/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,TypeOfAirPollutionIndicatorId,Name,Description")] AirPollutionIndicator airPollutionIndicator)
        {
            var airPollutionIndicators = _context.AirPollutionIndicator.AsNoTracking().Include(a => a.TypeOfAirPollutionIndicator).ToList();
            if (string.IsNullOrWhiteSpace(airPollutionIndicator.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (airPollutionIndicators.Select(t => t.Name).Contains(airPollutionIndicator.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(airPollutionIndicator);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "AirPollutionIndicator",
                    New = airPollutionIndicator.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeOfAirPollutionIndicatorId"] = new SelectList(_context.TypeOfAirPollutionIndicator, "Id", "Name", airPollutionIndicator.TypeOfAirPollutionIndicatorId);
            return View(airPollutionIndicator);
        }

        // GET: AirPollutionIndicators/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var airPollutionIndicator = await _context.AirPollutionIndicator.SingleOrDefaultAsync(m => m.Id == id);
            if (airPollutionIndicator == null)
            {
                return NotFound();
            }
            ViewData["TypeOfAirPollutionIndicatorId"] = new SelectList(_context.TypeOfAirPollutionIndicator, "Id", "Name", airPollutionIndicator.TypeOfAirPollutionIndicatorId);
            return View(airPollutionIndicator);
        }

        // POST: AirPollutionIndicators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypeOfAirPollutionIndicatorId,Name,Description")] AirPollutionIndicator airPollutionIndicator)
        {
            if (id != airPollutionIndicator.Id)
            {
                return NotFound();
            }
            var airPollutionIndicators = _context.AirPollutionIndicator.AsNoTracking().Where(a => a.Id != airPollutionIndicator.Id).Include(a => a.TypeOfAirPollutionIndicator).ToList();
            if (string.IsNullOrWhiteSpace(airPollutionIndicator.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (airPollutionIndicators.Select(t => t.Name).Contains(airPollutionIndicator.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var airPollutionIndicator_old = _context.AirPollutionIndicator.AsNoTracking().FirstOrDefault(a => a.Id == airPollutionIndicator.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "AirPollutionIndicator",
                        Operation = "Edit",
                        New = airPollutionIndicator.ToString(),
                        Old = airPollutionIndicator_old.ToString()
                    });
                    _context.Update(airPollutionIndicator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AirPollutionIndicatorExists(airPollutionIndicator.Id))
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
            ViewData["TypeOfAirPollutionIndicatorId"] = new SelectList(_context.TypeOfAirPollutionIndicator, "Id", "Name", airPollutionIndicator.TypeOfAirPollutionIndicatorId);
            return View(airPollutionIndicator);
        }

        // GET: AirPollutionIndicators/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airPollutionIndicator = await _context.AirPollutionIndicator
                .Include(a => a.TypeOfAirPollutionIndicator)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (airPollutionIndicator == null)
            {
                return NotFound();
            }

            return View(airPollutionIndicator);
        }

        // POST: AirPollutionIndicators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var airPollutionIndicator = await _context.AirPollutionIndicator.SingleOrDefaultAsync(m => m.Id == id);
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Class = "AirPollutionIndicator",
                Operation = "Delete",
                New = "",
                Old = airPollutionIndicator.ToString()
            });
            _context.AirPollutionIndicator.Remove(airPollutionIndicator);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AirPollutionIndicatorExists(int id)
        {
            return _context.AirPollutionIndicator.Any(e => e.Id == id);
        }
    }
}
