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
    public class GeneralWeatherConditionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public GeneralWeatherConditionsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: GeneralWeatherConditions
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string NameKK, string NameRU, int? Page)
        {
            var generalWeatherConditions = _context.GeneralWeatherCondition
                .Where(g => true);

            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(NameKK))
            {
                generalWeatherConditions = generalWeatherConditions.Where(g => g.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                generalWeatherConditions = generalWeatherConditions.Where(g => g.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameKK":
                    generalWeatherConditions = generalWeatherConditions.OrderBy(g => g.NameKK);
                    break;
                case "NameKKDesc":
                    generalWeatherConditions = generalWeatherConditions.OrderByDescending(g => g.NameKK);
                    break;
                case "NameRU":
                    generalWeatherConditions = generalWeatherConditions.OrderBy(g => g.NameRU);
                    break;
                case "NameRUDesc":
                    generalWeatherConditions = generalWeatherConditions.OrderByDescending(g => g.NameRU);
                    break;
                default:
                    generalWeatherConditions = generalWeatherConditions.OrderBy(g => g.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(generalWeatherConditions.Count(), Page);

            var viewModel = new GeneralWeatherConditionIndexPageViewModel
            {
                Items = generalWeatherConditions.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: GeneralWeatherConditions/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var generalWeatherCondition = await _context.GeneralWeatherCondition
                .SingleOrDefaultAsync(m => m.Id == id);
            if (generalWeatherCondition == null)
            {
                return NotFound();
            }

            return View(generalWeatherCondition);
        }

        // GET: GeneralWeatherConditions/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            GeneralWeatherCondition model = new GeneralWeatherCondition();
            return View(model);
        }

        // POST: GeneralWeatherConditions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,NameKK,NameRU,Default")] GeneralWeatherCondition generalWeatherCondition)
        {
            var generalWeatherConditions = _context.GeneralWeatherCondition.AsNoTracking().ToList();
            if (generalWeatherConditions.Select(g => g.NameKK).Contains(generalWeatherCondition.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (generalWeatherConditions.Select(g => g.NameRU).Contains(generalWeatherCondition.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(generalWeatherCondition.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(generalWeatherCondition.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(generalWeatherCondition);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "GeneralWeatherCondition",
                    New = generalWeatherCondition.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(generalWeatherCondition);
        }

        // GET: GeneralWeatherConditions/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var generalWeatherCondition = await _context.GeneralWeatherCondition.SingleOrDefaultAsync(m => m.Id == id);
            if (generalWeatherCondition == null)
            {
                return NotFound();
            }
            return View(generalWeatherCondition);
        }

        // POST: GeneralWeatherConditions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameKK,NameRU,Default")] GeneralWeatherCondition generalWeatherCondition)
        {
            if (id != generalWeatherCondition.Id)
            {
                return NotFound();
            }
            var generalWeatherConditions = _context.GeneralWeatherCondition.AsNoTracking().ToList();
            if (generalWeatherConditions.Where(g => g.Id != generalWeatherCondition.Id).Select(g => g.NameKK).Contains(generalWeatherCondition.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (generalWeatherConditions.Where(g => g.Id != generalWeatherCondition.Id).Select(g => g.NameRU).Contains(generalWeatherCondition.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(generalWeatherCondition.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(generalWeatherCondition.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var generalWeatherCondition_old = _context.GeneralWeatherCondition.AsNoTracking().FirstOrDefault(g => g.Id == generalWeatherCondition.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "GeneralWeatherCondition",
                        Operation = "Edit",
                        New = generalWeatherCondition.ToString(),
                        Old = generalWeatherCondition_old.ToString()
                    });
                    _context.Update(generalWeatherCondition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GeneralWeatherConditionExists(generalWeatherCondition.Id))
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
            return View(generalWeatherCondition);
        }

        // GET: GeneralWeatherConditions/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var generalWeatherCondition = await _context.GeneralWeatherCondition
                .SingleOrDefaultAsync(m => m.Id == id);
            if (generalWeatherCondition == null)
            {
                return NotFound();
            }
            ViewBag.AirPostDatas = _context.AirPostData
                .AsNoTracking()
                .Where(a => a.AirPostId == id)
                .Take(50)
                .OrderBy(a => a.DateTime.Year);
            return View(generalWeatherCondition);
        }

        // POST: GeneralWeatherConditions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var generalWeatherCondition = await _context.GeneralWeatherCondition.SingleOrDefaultAsync(m => m.Id == id);
            if (_context.AirPostData
                .AsNoTracking()
                .FirstOrDefault(a => a.AirPostId == id) == null)
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "GeneralWeatherCondition",
                    Operation = "Delete",
                    New = "",
                    Old = generalWeatherCondition.ToString()
                });
                _context.GeneralWeatherCondition.Remove(generalWeatherCondition);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool GeneralWeatherConditionExists(int id)
        {
            return _context.GeneralWeatherCondition.Any(e => e.Id == id);
        }
    }
}
