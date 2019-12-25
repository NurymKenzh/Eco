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
    public class SubstanceHazardClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public SubstanceHazardClassesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: SubstanceHazardClasses
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string NameKK, string NameRU, int? Page)
        {
            var substanceHazardClasses = _context.SubstanceHazardClass
                .Where(s => true);

            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(NameKK))
            {
                substanceHazardClasses = substanceHazardClasses.Where(s => s.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                substanceHazardClasses = substanceHazardClasses.Where(s => s.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameKK":
                    substanceHazardClasses = substanceHazardClasses.OrderBy(s => s.NameKK);
                    break;
                case "NameKKDesc":
                    substanceHazardClasses = substanceHazardClasses.OrderByDescending(s => s.NameKK);
                    break;
                case "NameRU":
                    substanceHazardClasses = substanceHazardClasses.OrderBy(s => s.NameRU);
                    break;
                case "NameRUDesc":
                    substanceHazardClasses = substanceHazardClasses.OrderByDescending(s => s.NameRU);
                    break;
                default:
                    substanceHazardClasses = substanceHazardClasses.OrderBy(s => s.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(substanceHazardClasses.Count(), Page);

            var viewModel = new SubstanceHazardClassIndexPageViewModel
            {
                Items = substanceHazardClasses.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: SubstanceHazardClasses/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var substanceHazardClass = await _context.SubstanceHazardClass
                .SingleOrDefaultAsync(m => m.Id == id);
            if (substanceHazardClass == null)
            {
                return NotFound();
            }

            return View(substanceHazardClass);
        }

        // GET: SubstanceHazardClasses/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: SubstanceHazardClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,NameKK,NameRU")] SubstanceHazardClass substanceHazardClass)
        {
            var substanceHazardClasses = _context.TypeOfTarget.AsNoTracking().ToList();
            if (substanceHazardClasses.Select(s => s.NameKK).Contains(substanceHazardClass.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (substanceHazardClasses.Select(s => s.NameRU).Contains(substanceHazardClass.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(substanceHazardClass.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(substanceHazardClass.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(substanceHazardClass);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "SubstanceHazardClass",
                    New = substanceHazardClass.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(substanceHazardClass);
        }

        // GET: SubstanceHazardClasses/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var substanceHazardClass = await _context.SubstanceHazardClass.SingleOrDefaultAsync(m => m.Id == id);
            if (substanceHazardClass == null)
            {
                return NotFound();
            }
            return View(substanceHazardClass);
        }

        // POST: SubstanceHazardClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameKK,NameRU")] SubstanceHazardClass substanceHazardClass)
        {
            if (id != substanceHazardClass.Id)
            {
                return NotFound();
            }
            var substanceHazardClasses = _context.SubstanceHazardClass.AsNoTracking().ToList();
            if (substanceHazardClasses.Where(s => s.Id != substanceHazardClass.Id).Select(s => s.NameKK).Contains(substanceHazardClass.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (substanceHazardClasses.Where(s => s.Id != substanceHazardClass.Id).Select(s => s.NameRU).Contains(substanceHazardClass.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(substanceHazardClass.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(substanceHazardClass.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var substanceHazardClass_old = _context.SubstanceHazardClass.AsNoTracking().FirstOrDefault(s => s.Id == substanceHazardClass.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "SubstanceHazardClass",
                        Operation = "Edit",
                        New = substanceHazardClass.ToString(),
                        Old = substanceHazardClass_old.ToString()
                    });
                    _context.Update(substanceHazardClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubstanceHazardClassExists(substanceHazardClass.Id))
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
            return View(substanceHazardClass);
        }

        // GET: SubstanceHazardClasses/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var substanceHazardClass = await _context.SubstanceHazardClass
                .SingleOrDefaultAsync(m => m.Id == id);
            if (substanceHazardClass == null)
            {
                return NotFound();
            }
            ViewBag.AirContaminants = _context.AirContaminant
                .AsNoTracking()
                .Where(a => a.SubstanceHazardClassId == id)
                .OrderBy(a => a.Name);
            return View(substanceHazardClass);
        }

        // POST: SubstanceHazardClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var substanceHazardClass = await _context.SubstanceHazardClass.SingleOrDefaultAsync(m => m.Id == id);
            if (_context.AirContaminant
                .AsNoTracking()
                .FirstOrDefault(a => a.SubstanceHazardClassId == id) == null)
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "SubstanceHazardClass",
                    Operation = "Delete",
                    New = "",
                    Old = substanceHazardClass.ToString()
                });
                _context.SubstanceHazardClass.Remove(substanceHazardClass);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SubstanceHazardClassExists(int id)
        {
            return _context.SubstanceHazardClass.Any(e => e.Id == id);
        }
    }
}
