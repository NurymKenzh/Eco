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
    public class KazHydrometAirPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public KazHydrometAirPostsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: KazHydrometAirPosts
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string Number, string NameKK, string NameRU, int? Page)
        {
            var kazHydrometAirPosts = _context.KazHydrometAirPost
                .Where(k => true);

            ViewBag.NumberFilter = Number;
            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NumberSort = SortOrder == "Number" ? "NumberDesc" : "Number";
            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(Number))
            {
                kazHydrometAirPosts = kazHydrometAirPosts.Where(k => k.Number.ToString() == Number);
            }
            if (!string.IsNullOrEmpty(NameKK))
            {
                kazHydrometAirPosts = kazHydrometAirPosts.Where(k => k.NameKK==NameKK);
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                kazHydrometAirPosts = kazHydrometAirPosts.Where(k => k.NameRU==NameRU);
            }

            switch (SortOrder)
            {
                case "Number":
                    kazHydrometAirPosts = kazHydrometAirPosts.OrderBy(k => k.Number);
                    break;
                case "NumberDesc":
                    kazHydrometAirPosts = kazHydrometAirPosts.OrderByDescending(k => k.Number);
                    break;
                case "NameKK":
                    kazHydrometAirPosts = kazHydrometAirPosts.OrderBy(k => k.NameKK);
                    break;
                case "NameKKDesc":
                    kazHydrometAirPosts = kazHydrometAirPosts.OrderByDescending(k => k.NameKK);
                    break;
                case "NameRU":
                    kazHydrometAirPosts = kazHydrometAirPosts.OrderBy(k => k.NameRU);
                    break;
                case "NameRUDesc":
                    kazHydrometAirPosts = kazHydrometAirPosts.OrderByDescending(k => k.NameRU);
                    break;
                default:
                    kazHydrometAirPosts = kazHydrometAirPosts.OrderBy(k => k.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(kazHydrometAirPosts.Count(), Page);

            var viewModel = new KazHydrometAirPostIndexPageViewModel
            {
                Items = kazHydrometAirPosts.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewData["Number"] = new SelectList(_context.KazHydrometAirPost.OrderBy(a => a.Number).GroupBy(k =>k.Number).Select(g => g.First()), "Number", "Number");
            ViewData["NameKK"] = new SelectList(_context.KazHydrometAirPost.OrderBy(a => a.NameKK).GroupBy(k => k.NameKK).Select(g => g.First()), "NameKK", "NameKK");
            ViewData["NameRU"] = new SelectList(_context.KazHydrometAirPost.OrderBy(a => a.NameRU).GroupBy(k => k.NameRU).Select(g => g.First()), "NameRU", "NameRU");

            return View(viewModel);
        }

        // GET: KazHydrometAirPosts/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kazHydrometAirPost = await _context.KazHydrometAirPost
                .Include(k => k.SamplingTerm)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (kazHydrometAirPost == null)
            {
                return NotFound();
            }

            kazHydrometAirPost.KazHydrometAirPostReport = new KazHydrometAirPostReport()
            {
                KazHydrometAirContaminantReports = new List<KazHydrometAirContaminantReport>(),
                StandardIndex = 0
            };
            foreach(var airContaminant in _context.AirContaminant.OrderBy(a => a.Name))
            {
                KazHydrometAirContaminantReport kazHydrometAirContaminantReport = new KazHydrometAirContaminantReport()
                {
                    Name = airContaminant.Name,
                    KazHydrometAirContaminantYearReports = new List<KazHydrometAirContaminantYearReport>()
                };
                var kazHydrometAirPostDatas = _context.KazHydrometAirPostData.Where(k => k.AirContaminantId == airContaminant.Id && k.KazHydrometAirPostId == id && k.Month != null).Include(k => k.AirContaminant).ToList();
                if(kazHydrometAirPostDatas.Count()>0)
                {
                    var AverageSeasonalConcentrationWintermgm3 = kazHydrometAirPostDatas.Where(k => (new int[3] { 12, 1, 2 }).Contains((int)k.Month))?.Select(k => k.PollutantConcentrationMonthlyAverage);
                    kazHydrometAirContaminantReport.AverageSeasonalConcentrationWintermgm3 = AverageSeasonalConcentrationWintermgm3.Count()>0 ? (decimal?)AverageSeasonalConcentrationWintermgm3.Average() : null;
                    var AverageSeasonalConcentrationSpringmgm3 = kazHydrometAirPostDatas.Where(k => (new int[3] { 3, 4, 5 }).Contains((int)k.Month)).Select(k => k.PollutantConcentrationMonthlyAverage);
                    kazHydrometAirContaminantReport.AverageSeasonalConcentrationSpringmgm3 = AverageSeasonalConcentrationSpringmgm3.Count() > 0 ? (decimal?)AverageSeasonalConcentrationSpringmgm3.Average() : null;
                    var AverageSeasonalConcentrationSummermgm3 = kazHydrometAirPostDatas.Where(k => (new int[3] { 6, 7, 8 }).Contains((int)k.Month)).Select(k => k.PollutantConcentrationMonthlyAverage);
                    kazHydrometAirContaminantReport.AverageSeasonalConcentrationSummermgm3 = AverageSeasonalConcentrationSummermgm3.Count()>0 ? (decimal?)AverageSeasonalConcentrationSummermgm3.Average() : null;
                    var AverageSeasonalConcentrationAutumnmgm3 = kazHydrometAirPostDatas.Where(k => (new int[3] { 9, 10, 11 }).Contains((int)k.Month)).Select(k => k.PollutantConcentrationMonthlyAverage);
                    kazHydrometAirContaminantReport.AverageSeasonalConcentrationAutumnmgm3 = AverageSeasonalConcentrationAutumnmgm3.Count()>0 ? (decimal?)AverageSeasonalConcentrationAutumnmgm3.Average() : null;
                    var AverageAnnualConcentrationmgm3 = kazHydrometAirPostDatas.Select(k => k.PollutantConcentrationMonthlyAverage);
                    kazHydrometAirContaminantReport.AverageAnnualConcentrationmgm3 = AverageAnnualConcentrationmgm3.Count() > 0 ? (decimal?)AverageAnnualConcentrationmgm3.Average() : null;
                    for(int year = Constants.YearDataMin; year <= DateTime.Now.Year; year++)
                    {
                        KazHydrometAirContaminantYearReport kazHydrometAirContaminantYearReport = new KazHydrometAirContaminantYearReport();
                        var kazHydrometAirPostDatasYear = kazHydrometAirPostDatas.Where(k => k.Year == year).ToList();
                        if(kazHydrometAirPostDatasYear.Count() > 0)
                        {
                            KazHydrometAirPostData KazHydrometAirPostDataMaximumOneTimeConcentrationPerYear = kazHydrometAirPostDatasYear.OrderByDescending(k => k.PollutantConcentrationMaximumOneTimePerMonth).FirstOrDefault();
                            kazHydrometAirContaminantYearReport.MaximumOneTimeConcentrationPerYearmgm3 = kazHydrometAirPostDatasYear.DefaultIfEmpty().Max(k => k.PollutantConcentrationMaximumOneTimePerMonth);
                            kazHydrometAirContaminantYearReport.MaximumOneTimeConcentrationPerYearDate = KazHydrometAirPostDataMaximumOneTimeConcentrationPerYear.PollutantConcentrationMaximumOneTimePerMonthDay != null ?
                                (KazHydrometAirPostDataMaximumOneTimeConcentrationPerYear != null ? ((DateTime?)new DateTime(year, (int)KazHydrometAirPostDataMaximumOneTimeConcentrationPerYear.Month, (int)KazHydrometAirPostDataMaximumOneTimeConcentrationPerYear.PollutantConcentrationMaximumOneTimePerMonthDay)) : null) :
                                null;
                            kazHydrometAirContaminantYearReport.TheMaximumFrequencyOfExceedingTheMaximumPermissibleConcentrationIsMaximumOneTime = kazHydrometAirPostDatasYear.DefaultIfEmpty().Max(k => k.MaximumPermissibleConcentrationExcessMultiplicityMaximumOneTime);
                        }
                        else
                        {
                            kazHydrometAirContaminantYearReport.MaximumOneTimeConcentrationPerYearmgm3 = null;
                            kazHydrometAirContaminantYearReport.MaximumOneTimeConcentrationPerYearDate = null;
                            kazHydrometAirContaminantYearReport.TheMaximumFrequencyOfExceedingTheMaximumPermissibleConcentrationIsMaximumOneTime = null;
                        }
                        kazHydrometAirContaminantYearReport.Year = year;
                        kazHydrometAirContaminantReport.KazHydrometAirContaminantYearReports.Add(kazHydrometAirContaminantYearReport);
                    }
                    kazHydrometAirPost.KazHydrometAirPostReport.KazHydrometAirContaminantReports.Add(kazHydrometAirContaminantReport);
                    decimal? MaximumPermissibleConcentrationExcessMultiplicityMaximumOneTimeYear = kazHydrometAirPostDatas.DefaultIfEmpty().Max(k => k.MaximumPermissibleConcentrationExcessMultiplicityMaximumOneTime);
                    if (kazHydrometAirPost.KazHydrometAirPostReport.StandardIndex < MaximumPermissibleConcentrationExcessMultiplicityMaximumOneTimeYear)
                    {
                        kazHydrometAirPost.KazHydrometAirPostReport.StandardIndex = MaximumPermissibleConcentrationExcessMultiplicityMaximumOneTimeYear;
                    }
                }
            }

            return View(kazHydrometAirPost);
        }

        // GET: KazHydrometAirPosts/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["SamplingTermId"] = new SelectList(_context.SamplingTerm.OrderBy(s => s.Name), "Id", "Name");
            KazHydrometAirPost model = new KazHydrometAirPost();
            return View(model);
        }

        // POST: KazHydrometAirPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Number,NameKK,NameRU,Type,SamplingType,SamplingTermId,AdditionalInformationKK,AdditionalInformationRU,NorthLatitude,EastLongitude")] KazHydrometAirPost kazHydrometAirPost)
        {
            var kazHydrometAirPosts = _context.KazHydrometAirPost.AsNoTracking().ToList();
            if (kazHydrometAirPosts.Select(k => k.Number).Contains(kazHydrometAirPost.Number))
            {
                ModelState.AddModelError("Number", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (kazHydrometAirPosts.Select(k => k.NameKK).Contains(kazHydrometAirPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (kazHydrometAirPosts.Select(k => k.NameRU).Contains(kazHydrometAirPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(kazHydrometAirPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(kazHydrometAirPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(kazHydrometAirPost);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "KazHydrometAirPost",
                    New = kazHydrometAirPost.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SamplingTermId"] = new SelectList(_context.SamplingTerm.OrderBy(s => s.Name), "Id", "Name", kazHydrometAirPost.SamplingTermId);
            return View(kazHydrometAirPost);
        }

        // GET: KazHydrometAirPosts/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kazHydrometAirPost = await _context.KazHydrometAirPost.SingleOrDefaultAsync(m => m.Id == id);
            if (kazHydrometAirPost == null)
            {
                return NotFound();
            }
            ViewData["SamplingTermId"] = new SelectList(_context.SamplingTerm.OrderBy(s => s.Name), "Id", "Name", kazHydrometAirPost.SamplingTermId);
            return View(kazHydrometAirPost);
        }

        // POST: KazHydrometAirPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,NameKK,NameRU,Type,SamplingType,SamplingTermId,AdditionalInformationKK,AdditionalInformationRU,NorthLatitude,EastLongitude")] KazHydrometAirPost kazHydrometAirPost)
        {
            if (id != kazHydrometAirPost.Id)
            {
                return NotFound();
            }
            var kazHydrometAirPosts = _context.KazHydrometAirPost.AsNoTracking().Where(k => k.Id != kazHydrometAirPost.Id).ToList();
            if (kazHydrometAirPosts.Select(k => k.Number).Contains(kazHydrometAirPost.Number))
            {
                ModelState.AddModelError("Number", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (kazHydrometAirPosts.Select(k => k.NameKK).Contains(kazHydrometAirPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (kazHydrometAirPosts.Select(k => k.NameRU).Contains(kazHydrometAirPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(kazHydrometAirPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(kazHydrometAirPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var kazHydrometAirPost_old = _context.KazHydrometAirPost.AsNoTracking().FirstOrDefault(k => k.Id == kazHydrometAirPost.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "KazHydrometAirPost",
                        Operation = "Edit",
                        New = kazHydrometAirPost.ToString(),
                        Old = kazHydrometAirPost_old.ToString()
                    });
                    _context.Update(kazHydrometAirPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KazHydrometAirPostExists(kazHydrometAirPost.Id))
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
            ViewData["SamplingTermId"] = new SelectList(_context.SamplingTerm.OrderBy(s => s.Name), "Id", "Name", kazHydrometAirPost.SamplingTermId);
            return View(kazHydrometAirPost);
        }

        // GET: KazHydrometAirPosts/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kazHydrometAirPost = await _context.KazHydrometAirPost
                .Include(k => k.SamplingTerm)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (kazHydrometAirPost == null)
            {
                return NotFound();
            }
            ViewBag.KazHydrometAirPostDatas = _context.KazHydrometAirPostData
                .AsNoTracking()
                .Where(k => k.KazHydrometAirPostId == id)
                .Take(50)
                .OrderBy(k => k.Year);
            ViewBag.TargetTerritories = _context.TargetTerritory
                .AsNoTracking()
                .Where(k => k.KazHydrometAirPostId == id)
                .OrderBy(k => k.TerritoryName);
            return View(kazHydrometAirPost);
        }

        // POST: KazHydrometAirPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kazHydrometAirPost = await _context.KazHydrometAirPost.SingleOrDefaultAsync(m => m.Id == id);
            if ((_context.KazHydrometAirPostData
                .AsNoTracking()
                .FirstOrDefault(k => k.KazHydrometAirPostId == id) == null)
                &&(_context.TargetTerritory
                .AsNoTracking()
                .FirstOrDefault(k => k.KazHydrometAirPostId == id) == null))
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "KazHydrometAirPost",
                    Operation = "Delete",
                    New = "",
                    Old = kazHydrometAirPost.ToString()
                });
                _context.KazHydrometAirPost.Remove(kazHydrometAirPost);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool KazHydrometAirPostExists(int id)
        {
            return _context.KazHydrometAirPost.Any(e => e.Id == id);
        }
    }
}
