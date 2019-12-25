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
    public class KazHydrometWaterPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public KazHydrometWaterPostsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: KazHydrometWaterPosts
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string Number, string NameKK, string NameRU, int? Page)
        {
            var kazHydrometWaterPosts = _context.KazHydrometWaterPost
                .Where(k => true);

            ViewBag.NumberFilter = Number;
            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NumberSort = SortOrder == "Number" ? "NumberDesc" : "Number";
            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(Number))
            {
                kazHydrometWaterPosts = kazHydrometWaterPosts.Where(k => k.Number.ToString()==Number);
            }
            if (!string.IsNullOrEmpty(NameKK))
            {
                kazHydrometWaterPosts = kazHydrometWaterPosts.Where(k => k.NameKK==NameKK);
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                kazHydrometWaterPosts = kazHydrometWaterPosts.Where(k => k.NameRU==NameRU);
            }

            switch (SortOrder)
            {
                case "Number":
                    kazHydrometWaterPosts = kazHydrometWaterPosts.OrderBy(k => k.Number);
                    break;
                case "NumberDesc":
                    kazHydrometWaterPosts = kazHydrometWaterPosts.OrderByDescending(k => k.Number);
                    break;
                case "NameKK":
                    kazHydrometWaterPosts = kazHydrometWaterPosts.OrderBy(k => k.NameKK);
                    break;
                case "NameKKDesc":
                    kazHydrometWaterPosts = kazHydrometWaterPosts.OrderByDescending(k => k.NameKK);
                    break;
                case "NameRU":
                    kazHydrometWaterPosts = kazHydrometWaterPosts.OrderBy(k => k.NameRU);
                    break;
                case "NameRUDesc":
                    kazHydrometWaterPosts = kazHydrometWaterPosts.OrderByDescending(k => k.NameRU);
                    break;
                default:
                    kazHydrometWaterPosts = kazHydrometWaterPosts.OrderBy(k => k.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(kazHydrometWaterPosts.Count(), Page);

            var viewModel = new KazHydrometWaterPostIndexPageViewModel
            {
                Items = kazHydrometWaterPosts.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewData["Number"] = new SelectList(_context.KazHydrometWaterPost.OrderBy(a => a.Number).GroupBy(k => k.Number).Select(g => g.First()), "Number", "Number");
            ViewData["NameKK"] = new SelectList(_context.KazHydrometWaterPost.OrderBy(a => a.NameKK).GroupBy(k => k.NameKK).Select(g => g.First()), "NameKK", "NameKK");
            ViewData["NameRU"] = new SelectList(_context.KazHydrometWaterPost.OrderBy(a => a.NameRU).GroupBy(k => k.NameRU).Select(g => g.First()), "NameRU", "NameRU");

            return View(viewModel);
        }

        // GET: KazHydrometWaterPosts/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kazHydrometWaterPost = await _context.KazHydrometWaterPost
                .SingleOrDefaultAsync(m => m.Id == id);
            if (kazHydrometWaterPost == null)
            {
                return NotFound();
            }

            kazHydrometWaterPost.KazHydrometWaterPostReport = new KazHydrometWaterPostReport()
            {
                KazHydrometWaterContaminantReports = new List<KazHydrometWaterContaminantReport>(),
                StandardIndex = 0
            };
            foreach (var waterContaminant in _context.WaterContaminant.OrderBy(a => a.Name))
            {
                KazHydrometWaterContaminantReport kazHydrometWaterContaminantReport = new KazHydrometWaterContaminantReport()
                {
                    Name = waterContaminant.Name,
                    MaxConcentrationMonths = new decimal?[12]
                };
                var kazHydrometWaterPostDatas = _context.KazHydrometWaterPostData.Where(k => k.WaterContaminantId == waterContaminant.Id && k.KazHydrometWaterPostId == id).Include(k => k.WaterContaminant).ToList();
                if (kazHydrometWaterPostDatas.Count() > 0)
                {
                    var AverageSeasonalConcentrationWintermgm3 = kazHydrometWaterPostDatas.Where(k => (new int[3] { 12, 1, 2 }).Contains(k.Month)).Select(k => k.PollutantConcentrationmgl);
                    kazHydrometWaterContaminantReport.AverageSeasonalConcentrationWintermgl = AverageSeasonalConcentrationWintermgm3.Count() > 0 ? (decimal?)AverageSeasonalConcentrationWintermgm3.Average() : null;
                    var AverageSeasonalConcentrationSpringmgm3 = kazHydrometWaterPostDatas.Where(k => (new int[3] { 3, 4, 5 }).Contains(k.Month)).Select(k => k.PollutantConcentrationmgl);
                    kazHydrometWaterContaminantReport.AverageSeasonalConcentrationSpringmgl = AverageSeasonalConcentrationSpringmgm3.Count() > 0 ? (decimal?)AverageSeasonalConcentrationSpringmgm3.Average() : null;
                    var AverageSeasonalConcentrationSummermgm3 = kazHydrometWaterPostDatas.Where(k => (new int[3] { 6, 7, 8 }).Contains(k.Month)).Select(k => k.PollutantConcentrationmgl);
                    kazHydrometWaterContaminantReport.AverageSeasonalConcentrationSummermgl = AverageSeasonalConcentrationSummermgm3.Count() > 0 ? (decimal?)AverageSeasonalConcentrationSummermgm3.Average() : null;
                    var AverageSeasonalConcentrationAutumnmgm3 = kazHydrometWaterPostDatas.Where(k => (new int[3] { 9, 10, 11 }).Contains(k.Month)).Select(k => k.PollutantConcentrationmgl);
                    kazHydrometWaterContaminantReport.AverageSeasonalConcentrationAutumnmgl = AverageSeasonalConcentrationAutumnmgm3.Count() > 0 ? (decimal?)AverageSeasonalConcentrationAutumnmgm3.Average() : null;
                    var AverageAnnualConcentrationmgm3 = kazHydrometWaterPostDatas.Select(k => k.PollutantConcentrationmgl);
                    kazHydrometWaterContaminantReport.AverageAnnualConcentrationmgl = AverageAnnualConcentrationmgm3.Count() > 0 ? (decimal?)AverageAnnualConcentrationmgm3.Average() : null;
                    for (int month = 1; month <= 12; month++)
                    {
                        var kazHydrometWaterPostDatasMonth = kazHydrometWaterPostDatas.Where(k => k.Month == month).ToList();
                        if (kazHydrometWaterPostDatasMonth.Count() > 0)
                        {
                            kazHydrometWaterContaminantReport.MaxConcentrationMonths[month - 1] = kazHydrometWaterPostDatas.Where(k => k.Month == month).Max(k => k.PollutantConcentrationmgl);
                        }
                        else
                        {
                            kazHydrometWaterContaminantReport.MaxConcentrationMonths[month - 1] = null;
                        }
                    }
                    kazHydrometWaterPost.KazHydrometWaterPostReport.KazHydrometWaterContaminantReports.Add(kazHydrometWaterContaminantReport);
                }
            }

            return View(kazHydrometWaterPost);
        }

        // GET: KazHydrometWaterPosts/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: KazHydrometWaterPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Number,NameKK,NameRU,AdditionalInformationKK,AdditionalInformationRU,NorthLatitude,EastLongitude")] KazHydrometWaterPost kazHydrometWaterPost)
        {
            var kazHydrometWaterPosts = _context.KazHydrometWaterPost.AsNoTracking().ToList();
            if (kazHydrometWaterPosts.Select(k => k.Number).Contains(kazHydrometWaterPost.Number))
            {
                ModelState.AddModelError("Number", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (kazHydrometWaterPosts.Select(k => k.NameKK).Contains(kazHydrometWaterPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (kazHydrometWaterPosts.Select(k => k.NameRU).Contains(kazHydrometWaterPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(kazHydrometWaterPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(kazHydrometWaterPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(kazHydrometWaterPost);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "KazHydrometWaterPost",
                    New = kazHydrometWaterPost.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kazHydrometWaterPost);
        }

        // GET: KazHydrometWaterPosts/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kazHydrometWaterPost = await _context.KazHydrometWaterPost.SingleOrDefaultAsync(m => m.Id == id);
            if (kazHydrometWaterPost == null)
            {
                return NotFound();
            }
            return View(kazHydrometWaterPost);
        }

        // POST: KazHydrometWaterPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,NameKK,NameRU,AdditionalInformationKK,AdditionalInformationRU,NorthLatitude,EastLongitude")] KazHydrometWaterPost kazHydrometWaterPost)
        {
            if (id != kazHydrometWaterPost.Id)
            {
                return NotFound();
            }
            var kazHydrometWaterPosts = _context.KazHydrometWaterPost.AsNoTracking().Where(k => k.Id != kazHydrometWaterPost.Id).ToList();
            if (kazHydrometWaterPosts.Select(k => k.Number).Contains(kazHydrometWaterPost.Number))
            {
                ModelState.AddModelError("Number", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (kazHydrometWaterPosts.Select(k => k.NameKK).Contains(kazHydrometWaterPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (kazHydrometWaterPosts.Select(k => k.NameRU).Contains(kazHydrometWaterPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(kazHydrometWaterPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(kazHydrometWaterPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var kazHydrometWaterPost_old = _context.LimitingIndicator.AsNoTracking().FirstOrDefault(k => k.Id == kazHydrometWaterPost.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "KazHydrometWaterPost",
                        Operation = "Edit",
                        New = kazHydrometWaterPost.ToString(),
                        Old = kazHydrometWaterPost_old.ToString()
                    });
                    _context.Update(kazHydrometWaterPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KazHydrometWaterPostExists(kazHydrometWaterPost.Id))
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
            return View(kazHydrometWaterPost);
        }

        // GET: KazHydrometWaterPosts/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kazHydrometWaterPost = await _context.KazHydrometWaterPost
                .SingleOrDefaultAsync(m => m.Id == id);
            if (kazHydrometWaterPost == null)
            {
                return NotFound();
            }
            ViewBag.KazHydrometWaterPostDatas = _context.KazHydrometWaterPostData
                .AsNoTracking()
                .Where(k => k.KazHydrometWaterPostId == id)
                .Take(50)
                .OrderBy(k => k.Year);
            ViewBag.TargetTerritories = _context.TargetTerritory
                .AsNoTracking()
                .Where(k => k.KazHydrometWaterPostId == id)
                .OrderBy(k => k.TerritoryName);
            return View(kazHydrometWaterPost);
        }

        // POST: KazHydrometWaterPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kazHydrometWaterPost = await _context.KazHydrometWaterPost.SingleOrDefaultAsync(m => m.Id == id);
            if ((_context.KazHydrometWaterPostData
                .AsNoTracking()
                .FirstOrDefault(k => k.KazHydrometWaterPostId == id) == null)
                &&(_context.TargetTerritory
                .AsNoTracking()
                .FirstOrDefault(k => k.KazHydrometWaterPostId == id) == null))
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "KazHydrometWaterPost",
                    Operation = "Delete",
                    New = "",
                    Old = kazHydrometWaterPost.ToString()
                });
                _context.KazHydrometWaterPost.Remove(kazHydrometWaterPost);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool KazHydrometWaterPostExists(int id)
        {
            return _context.KazHydrometWaterPost.Any(e => e.Id == id);
        }
    }
}
