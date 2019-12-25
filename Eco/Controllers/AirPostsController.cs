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
    public class AirPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public AirPostsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: AirPosts
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string Number, string NameKK, string NameRU, int? Page)
        {
            var airPosts = _context.AirPost
                .Where(a => true);

            ViewBag.NumberFilter = Number;
            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NumberSort = SortOrder == "Number" ? "NumberDesc" : "Number";
            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(Number))
            {
                airPosts = airPosts.Where(a => a.Number.ToString()==Number);
            }
            if (!string.IsNullOrEmpty(NameKK))
            {
                airPosts = airPosts.Where(a => a.NameKK==NameKK);
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                airPosts = airPosts.Where(a => a.NameRU==NameRU);
            }

            switch (SortOrder)
            {
                case "Number":
                    airPosts = airPosts.OrderBy(a => a.Number);
                    break;
                case "NumberDesc":
                    airPosts = airPosts.OrderByDescending(a => a.Number);
                    break;
                case "NameKK":
                    airPosts = airPosts.OrderBy(a => a.NameKK);
                    break;
                case "NameKKDesc":
                    airPosts = airPosts.OrderByDescending(a => a.NameKK);
                    break;
                case "NameRU":
                    airPosts = airPosts.OrderBy(a => a.NameRU);
                    break;
                case "NameRUDesc":
                    airPosts = airPosts.OrderByDescending(a => a.NameRU);
                    break;
                default:
                    airPosts = airPosts.OrderBy(a => a.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(airPosts.Count(), Page);

            var viewModel = new AirPostIndexPageViewModel
            {
                Items = airPosts.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewData["Number"] = new SelectList(_context.AirPost.OrderBy(a => a.Number).GroupBy(k => k.Number).Select(g => g.First()), "Number", "Number");
            ViewData["NameKK"] = new SelectList(_context.AirPost.OrderBy(a => a.NameKK).GroupBy(k => k.NameKK).Select(g => g.First()), "NameKK", "NameKK");
            ViewData["NameRU"] = new SelectList(_context.AirPost.OrderBy(a => a.NameRU).GroupBy(k => k.NameRU).Select(g => g.First()), "NameRU", "NameRU");

            return View(viewModel);
        }

        // GET: AirPosts/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airPost = await _context.AirPost
                .SingleOrDefaultAsync(m => m.Id == id);
            if (airPost == null)
            {
                return NotFound();
            }

            airPost.AirPostReport = new AirPostReport()
            {
                AirContaminantReports = new List<AirContaminantReport>()
            };
            foreach(var airContaminant in _context.AirContaminant.OrderBy(a => a.Name))
            {
                AirContaminantReport airContaminantReport = new AirContaminantReport()
                {
                    Name = airContaminant.Name
                };
                var airPostDatas = _context.AirPostData.Where(k => k.AirContaminantId == airContaminant.Id && k.AirPostId == id).Include(k => k.AirContaminant).ToList();
                if (airPostDatas.Count() > 0)
                {
                    AirPostData MaximumOneTimeValue = airPostDatas.OrderByDescending(k => k.Value).FirstOrDefault();
                    airContaminantReport.MaximumOneTimeValue = MaximumOneTimeValue.Value;
                    airContaminantReport.MaximumOneTimeValueDate = MaximumOneTimeValue.DateTime;
                    airContaminantReport.TheMaximumFrequencyOfExceedingTheMaximumPermissibleConcentrationIsMaximumOneTime = MaximumOneTimeValue.Value / airContaminant.MaximumPermissibleConcentrationOneTimemaximum;
                    airPost.AirPostReport.AirContaminantReports.Add(airContaminantReport);
                }
            }

            return View(airPost);
        }

        // GET: AirPosts/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            AirPost model = new AirPost();
            return View(model);
        }

        // POST: AirPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Number,NameKK,NameRU,PollutionSource,AdditionalInformationKK,AdditionalInformationRU,NorthLatitude,EastLongitude")] AirPost airPost)
        {
            var airPosts = _context.AirPost.AsNoTracking().ToList();
            if (airPosts.Select(a => a.Number).Contains(airPost.Number))
            {
                ModelState.AddModelError("Number", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (airPosts.Select(a => a.NameKK).Contains(airPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (airPosts.Select(a => a.NameRU).Contains(airPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(airPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(airPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(airPost);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "AirPost",
                    New = airPost.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(airPost);
        }

        // GET: AirPosts/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airPost = await _context.AirPost.SingleOrDefaultAsync(m => m.Id == id);
            if (airPost == null)
            {
                return NotFound();
            }
            return View(airPost);
        }

        // POST: AirPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,NameKK,NameRU,PollutionSource,AdditionalInformationKK,AdditionalInformationRU,NorthLatitude,EastLongitude")] AirPost airPost)
        {
            if (id != airPost.Id)
            {
                return NotFound();
            }
            var airPosts = _context.AirPost.AsNoTracking().Where(a => a.Id != airPost.Id).ToList();
            if (airPosts.Select(a => a.Number).Contains(airPost.Number))
            {
                ModelState.AddModelError("Number", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (airPosts.Select(a => a.NameKK).Contains(airPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (airPosts.Select(a => a.NameRU).Contains(airPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(airPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(airPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var airPost_old = _context.AirPost.AsNoTracking().FirstOrDefault(a => a.Id == airPost.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "AirPost",
                        Operation = "Edit",
                        New = airPost.ToString(),
                        Old = airPost_old.ToString()
                    });
                    _context.Update(airPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AirPostExists(airPost.Id))
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
            return View(airPost);
        }

        // GET: AirPosts/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airPost = await _context.AirPost
                .SingleOrDefaultAsync(m => m.Id == id);
            if (airPost == null)
            {
                return NotFound();
            }
            ViewBag.AirPostDatas = _context.AirPostData
                .AsNoTracking()
                .Where(a => a.AirPostId == id)
                .Take(50)
                .OrderBy(a => a.DateTime);
            ViewBag.TargetTerritories = _context.TargetTerritory
                .AsNoTracking()
                .Where(k => k.AirPostId == id)
                .OrderBy(k => k.TerritoryName);
            return View(airPost);
        }

        // POST: AirPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var airPost = await _context.AirPost.SingleOrDefaultAsync(m => m.Id == id);
            if ((_context.AirPostData
                .AsNoTracking()
                .FirstOrDefault(a => a.AirPostId == id) == null)
                &&(_context.TargetTerritory
                .AsNoTracking()
                .FirstOrDefault(k => k.AirPostId == id) == null))
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "AirPost",
                    Operation = "Delete",
                    New = "",
                    Old = airPost.ToString()
                });
                _context.AirPost.Remove(airPost);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AirPostExists(int id)
        {
            return _context.AirPost.Any(e => e.Id == id);
        }
    }
}
