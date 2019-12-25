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
    public class SamplingTermsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public SamplingTermsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: SamplingTerms
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string NameKK, string NameRU, int? Page)
        {
            var samplingTerms = _context.SamplingTerm
                .Where(s => true);

            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(NameKK))
            {
                samplingTerms = samplingTerms.Where(s => s.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                samplingTerms = samplingTerms.Where(s => s.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameKK":
                    samplingTerms = samplingTerms.OrderBy(s => s.NameKK);
                    break;
                case "NameKKDesc":
                    samplingTerms = samplingTerms.OrderByDescending(s => s.NameKK);
                    break;
                case "NameRU":
                    samplingTerms = samplingTerms.OrderBy(s => s.NameRU);
                    break;
                case "NameRUDesc":
                    samplingTerms = samplingTerms.OrderByDescending(s => s.NameRU);
                    break;
                default:
                    samplingTerms = samplingTerms.OrderBy(s => s.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(samplingTerms.Count(), Page);

            var viewModel = new SamplingTermIndexPageViewModel
            {
                Items = samplingTerms.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: SamplingTerms/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var samplingTerm = await _context.SamplingTerm
                .SingleOrDefaultAsync(m => m.Id == id);
            if (samplingTerm == null)
            {
                return NotFound();
            }

            return View(samplingTerm);
        }

        // GET: SamplingTerms/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: SamplingTerms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,NameKK,NameRU")] SamplingTerm samplingTerm)
        {
            var samplingTerms = _context.SamplingTerm.AsNoTracking().ToList();
            if (samplingTerms.Select(s => s.NameKK).Contains(samplingTerm.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (samplingTerms.Select(s => s.NameRU).Contains(samplingTerm.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(samplingTerm.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(samplingTerm.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(samplingTerm);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "SamplingTerm",
                    New = samplingTerm.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(samplingTerm);
        }

        // GET: SamplingTerms/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var samplingTerm = await _context.SamplingTerm.SingleOrDefaultAsync(m => m.Id == id);
            if (samplingTerm == null)
            {
                return NotFound();
            }
            return View(samplingTerm);
        }

        // POST: SamplingTerms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameKK,NameRU")] SamplingTerm samplingTerm)
        {
            if (id != samplingTerm.Id)
            {
                return NotFound();
            }
            var samplingTerms = _context.SamplingTerm.AsNoTracking().ToList();
            if (samplingTerms.Where(s => s.Id != samplingTerm.Id).Select(s => s.NameKK).Contains(samplingTerm.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (samplingTerms.Where(s => s.Id != samplingTerm.Id).Select(s => s.NameRU).Contains(samplingTerm.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(samplingTerm.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(samplingTerm.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var samplingTerm_old = _context.SamplingTerm.AsNoTracking().FirstOrDefault(s => s.Id == samplingTerm.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "SamplingTerm",
                        Operation = "Edit",
                        New = samplingTerm.ToString(),
                        Old = samplingTerm_old.ToString()
                    });
                    _context.Update(samplingTerm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SamplingTermExists(samplingTerm.Id))
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
            return View(samplingTerm);
        }

        // GET: SamplingTerms/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var samplingTerm = await _context.SamplingTerm
                .SingleOrDefaultAsync(m => m.Id == id);
            if (samplingTerm == null)
            {
                return NotFound();
            }
            ViewBag.KazHydrometAirPosts = _context.KazHydrometAirPost
                .AsNoTracking()
                .Where(k => k.SamplingTermId == id)
                .OrderBy(k => k.Number);
            return View(samplingTerm);
        }

        // POST: SamplingTerms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var samplingTerm = await _context.SamplingTerm.SingleOrDefaultAsync(m => m.Id == id);
            if (_context.KazHydrometAirPost
                .AsNoTracking()
                .FirstOrDefault(k => k.SamplingTermId == id) == null)
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "SamplingTerm",
                    Operation = "Delete",
                    New = "",
                    Old = samplingTerm.ToString()
                });
                _context.SamplingTerm.Remove(samplingTerm);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SamplingTermExists(int id)
        {
            return _context.SamplingTerm.Any(e => e.Id == id);
        }
    }
}
