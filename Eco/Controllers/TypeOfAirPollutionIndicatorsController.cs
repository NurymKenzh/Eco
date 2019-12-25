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
    public class TypeOfAirPollutionIndicatorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public TypeOfAirPollutionIndicatorsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: TypeOfAirPollutionIndicators
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string NameKK, string NameRU, int? Page)
        {
            var typeOfAirPollutionIndicators = _context.TypeOfAirPollutionIndicator
                .Where(t => true);

            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(NameKK))
            {
                typeOfAirPollutionIndicators = typeOfAirPollutionIndicators.Where(t => t.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                typeOfAirPollutionIndicators = typeOfAirPollutionIndicators.Where(t => t.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameKK":
                    typeOfAirPollutionIndicators = typeOfAirPollutionIndicators.OrderBy(t => t.NameKK);
                    break;
                case "NameKKDesc":
                    typeOfAirPollutionIndicators = typeOfAirPollutionIndicators.OrderByDescending(t => t.NameKK);
                    break;
                case "NameRU":
                    typeOfAirPollutionIndicators = typeOfAirPollutionIndicators.OrderBy(t => t.NameRU);
                    break;
                case "NameRUDesc":
                    typeOfAirPollutionIndicators = typeOfAirPollutionIndicators.OrderByDescending(t => t.NameRU);
                    break;
                default:
                    typeOfAirPollutionIndicators = typeOfAirPollutionIndicators.OrderBy(t => t.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(typeOfAirPollutionIndicators.Count(), Page);

            var viewModel = new TypeOfAirPollutionIndicatorIndexPageViewModel
            {
                Items = typeOfAirPollutionIndicators.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: TypeOfAirPollutionIndicators/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfAirPollutionIndicator = await _context.TypeOfAirPollutionIndicator
                .SingleOrDefaultAsync(m => m.Id == id);
            if (typeOfAirPollutionIndicator == null)
            {
                return NotFound();
            }

            return View(typeOfAirPollutionIndicator);
        }

        // GET: TypeOfAirPollutionIndicators/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypeOfAirPollutionIndicators/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,NameKK,NameRU")] TypeOfAirPollutionIndicator typeOfAirPollutionIndicator)
        {
            var typeOfAirPollutionIndicators = _context.TypeOfAirPollutionIndicator.AsNoTracking().ToList();
            if (typeOfAirPollutionIndicators.Select(t => t.NameKK).Contains(typeOfAirPollutionIndicator.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (typeOfAirPollutionIndicators.Select(t => t.NameRU).Contains(typeOfAirPollutionIndicator.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(typeOfAirPollutionIndicator.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(typeOfAirPollutionIndicator.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(typeOfAirPollutionIndicator);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "TypeOfAirPollutionIndicator",
                    New = typeOfAirPollutionIndicator.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typeOfAirPollutionIndicator);
        }

        // GET: TypeOfAirPollutionIndicators/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfAirPollutionIndicator = await _context.TypeOfAirPollutionIndicator.SingleOrDefaultAsync(m => m.Id == id);
            if (typeOfAirPollutionIndicator == null)
            {
                return NotFound();
            }
            return View(typeOfAirPollutionIndicator);
        }

        // POST: TypeOfAirPollutionIndicators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameKK,NameRU")] TypeOfAirPollutionIndicator typeOfAirPollutionIndicator)
        {
            if (id != typeOfAirPollutionIndicator.Id)
            {
                return NotFound();
            }
            var typeOfAirPollutionIndicators = _context.TypeOfAirPollutionIndicator.AsNoTracking().ToList();
            if (typeOfAirPollutionIndicators.Where(t => t.Id != typeOfAirPollutionIndicator.Id).Select(t => t.NameKK).Contains(typeOfAirPollutionIndicator.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (typeOfAirPollutionIndicators.Where(t => t.Id != typeOfAirPollutionIndicator.Id).Select(t => t.NameRU).Contains(typeOfAirPollutionIndicator.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(typeOfAirPollutionIndicator.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(typeOfAirPollutionIndicator.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var typeOfAirPollutionIndicator_old = _context.TypeOfAirPollutionIndicator.AsNoTracking().FirstOrDefault(t => t.Id == typeOfAirPollutionIndicator.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "TypeOfAirPollutionIndicator",
                        Operation = "Edit",
                        New = typeOfAirPollutionIndicator.ToString(),
                        Old = typeOfAirPollutionIndicator_old.ToString()
                    });
                    _context.Update(typeOfAirPollutionIndicator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeOfAirPollutionIndicatorExists(typeOfAirPollutionIndicator.Id))
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
            return View(typeOfAirPollutionIndicator);
        }

        // GET: TypeOfAirPollutionIndicators/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfAirPollutionIndicator = await _context.TypeOfAirPollutionIndicator
                .SingleOrDefaultAsync(m => m.Id == id);
            if (typeOfAirPollutionIndicator == null)
            {
                return NotFound();
            }
            ViewBag.AirPollutionIndicators = _context.AirPollutionIndicator
                .AsNoTracking()
                .Where(a => a.TypeOfAirPollutionIndicatorId == id)
                .OrderBy(a => a.Name);
            return View(typeOfAirPollutionIndicator);
        }

        // POST: TypeOfAirPollutionIndicators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var typeOfAirPollutionIndicator = await _context.TypeOfAirPollutionIndicator.SingleOrDefaultAsync(m => m.Id == id);
            if ((_context.AirPollutionIndicator
                .AsNoTracking()
                .FirstOrDefault(a => a.TypeOfAirPollutionIndicatorId == id) == null))
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "TypeOfAirPollutionIndicator",
                    Operation = "Delete",
                    New = "",
                    Old = typeOfAirPollutionIndicator.ToString()
                });
                _context.TypeOfAirPollutionIndicator.Remove(typeOfAirPollutionIndicator);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TypeOfAirPollutionIndicatorExists(int id)
        {
            return _context.TypeOfAirPollutionIndicator.Any(e => e.Id == id);
        }
    }
}
