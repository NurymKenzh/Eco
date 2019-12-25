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
    public class KazHydrometWaterPostDatasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public KazHydrometWaterPostDatasController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: KazHydrometWaterPostDatas
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            string KazHydrometWaterPostNumber,
            string KazHydrometWaterPostName,
            int? WaterContaminantId,
            int? Year,
            int? Month,
            int? Page)
        {
            var kazHydrometWaterPostDatas = _context.KazHydrometWaterPostData
                .Include(k => k.KazHydrometWaterPost)
                .Include(k => k.WaterContaminant)
                .Where(k => true);

            ViewBag.KazHydrometWaterPostNumberFilter = KazHydrometWaterPostNumber;
            ViewBag.KazHydrometWaterPostNameFilter = KazHydrometWaterPostName;
            ViewBag.WaterContaminantIdFilter = WaterContaminantId;
            ViewBag.YearFilter = Year;
            ViewBag.MonthFilter = Month;

            ViewBag.KazHydrometWaterPostNumberSort = SortOrder == "KazHydrometWaterPostNumber" ? "KazHydrometWaterPostNumberDesc" : "KazHydrometWaterPostNumber";
            ViewBag.KazHydrometWaterPostNameSort = SortOrder == "KazHydrometWaterPostName" ? "KazHydrometWaterPostNameDesc" : "KazHydrometWaterPostName";
            ViewBag.WaterContaminantNameSort = SortOrder == "WaterContaminantName" ? "WaterContaminantNameDesc" : "WaterContaminantName";
            ViewBag.YearSort = SortOrder == "Year" ? "YearDesc" : "Year";
            ViewBag.MonthSort = SortOrder == "Month" ? "MonthDesc" : "Month";

            if (!string.IsNullOrEmpty(KazHydrometWaterPostNumber))
            {
                kazHydrometWaterPostDatas = kazHydrometWaterPostDatas.Where(k => k.KazHydrometWaterPost.Number.ToString()==KazHydrometWaterPostNumber);
            }
            if (!string.IsNullOrEmpty(KazHydrometWaterPostName))
            {
                kazHydrometWaterPostDatas = kazHydrometWaterPostDatas.Where(k => k.KazHydrometWaterPost.Name==KazHydrometWaterPostName);
            }
            if (WaterContaminantId != null)
            {
                kazHydrometWaterPostDatas = kazHydrometWaterPostDatas.Where(k => k.WaterContaminantId == WaterContaminantId);
            }
            if (Year != null)
            {
                kazHydrometWaterPostDatas = kazHydrometWaterPostDatas.Where(k => k.Year == Year);
            }
            if (Month != null)
            {
                kazHydrometWaterPostDatas = kazHydrometWaterPostDatas.Where(k => k.Month == Month);
            }

            switch (SortOrder)
            {
                case "KazHydrometWaterPostNumber":
                    kazHydrometWaterPostDatas = kazHydrometWaterPostDatas.OrderBy(k => k.KazHydrometWaterPost.Number);
                    break;
                case "KazHydrometWaterPostNumberDesc":
                    kazHydrometWaterPostDatas = kazHydrometWaterPostDatas.OrderByDescending(k => k.KazHydrometWaterPost.Number);
                    break;
                case "KazHydrometWaterPostName":
                    kazHydrometWaterPostDatas = kazHydrometWaterPostDatas.OrderBy(k => k.KazHydrometWaterPost.Name);
                    break;
                case "KazHydrometWaterPostNameDesc":
                    kazHydrometWaterPostDatas = kazHydrometWaterPostDatas.OrderByDescending(k => k.KazHydrometWaterPost.Name);
                    break;
                case "WaterContaminantName":
                    kazHydrometWaterPostDatas = kazHydrometWaterPostDatas.OrderBy(k => k.WaterContaminant.Name);
                    break;
                case "WaterContaminantNameDesc":
                    kazHydrometWaterPostDatas = kazHydrometWaterPostDatas.OrderByDescending(k => k.WaterContaminant.Name);
                    break;
                case "Year":
                    kazHydrometWaterPostDatas = kazHydrometWaterPostDatas.OrderBy(k => k.Year);
                    break;
                case "YearDesc":
                    kazHydrometWaterPostDatas = kazHydrometWaterPostDatas.OrderByDescending(k => k.Year);
                    break;
                case "Month":
                    kazHydrometWaterPostDatas = kazHydrometWaterPostDatas.OrderBy(k => k.Month);
                    break;
                case "MonthDesc":
                    kazHydrometWaterPostDatas = kazHydrometWaterPostDatas.OrderByDescending(k => k.Month);
                    break;
                default:
                    kazHydrometWaterPostDatas = kazHydrometWaterPostDatas.OrderBy(k => k.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(kazHydrometWaterPostDatas.Count(), Page);

            var viewModel = new KazHydrometWaterPostDataIndexPageViewModel
            {
                Items = kazHydrometWaterPostDatas.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            var years = _context.KazHydrometWaterPostData.Select(k => k.Year).Distinct().ToList();
            ViewBag.Year = new SelectList(Enumerable.Range(Constants.YearDataMin, Constants.YearMax - Constants.YearDataMin + 1).Where(i => years.Contains(i)).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");
            ViewBag.Month = new SelectList(Enumerable.Range(1, 12).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");

            ViewData["WaterContaminantId"] = new SelectList(_context.WaterContaminant.Where(a => _context.KazHydrometWaterPostData.Include(k => k.WaterContaminant).Select(k => k.WaterContaminantId).Contains(a.Id)).OrderBy(a => a.Name), "Id", "Name");
            ViewData["KazHydrometWaterPostNumber"] = new SelectList(_context.KazHydrometWaterPost.OrderBy(a => a.Number).GroupBy(k => k.Number).Select(g => g.First()), "Number", "Number");
            ViewData["KazHydrometWaterPostName"] = new SelectList(_context.KazHydrometWaterPost.OrderBy(a => a.Name).GroupBy(k => k.Name).Select(g => g.First()), "Name", "Name");

            return View(viewModel);
        }

        // GET: KazHydrometWaterPostDatas/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kazHydrometWaterPostData = await _context.KazHydrometWaterPostData
                .Include(k => k.KazHydrometWaterPost)
                .Include(k => k.WaterContaminant)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (kazHydrometWaterPostData == null)
            {
                return NotFound();
            }

            return View(kazHydrometWaterPostData);
        }

        // GET: KazHydrometWaterPostDatas/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["WaterContaminantId"] = new SelectList(_context.WaterContaminant.OrderBy(a => a.Name), "Id", "Name");
            ViewData["KazHydrometWaterPostId"] = new SelectList(_context.KazHydrometWaterPost.OrderBy(k => k.Number), "Id", "Number");
            return View();
        }

        // POST: KazHydrometWaterPostDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,KazHydrometWaterPostId,WaterContaminantId,Year,Month,PollutantConcentrationmgl")] KazHydrometWaterPostData kazHydrometWaterPostData)
        {
            if (_context.KazHydrometWaterPostData.AsNoTracking().FirstOrDefault(k => k.KazHydrometWaterPostId == kazHydrometWaterPostData.KazHydrometWaterPostId
                && k.WaterContaminantId == kazHydrometWaterPostData.WaterContaminantId
                && k.Year == kazHydrometWaterPostData.Year
                && k.Month == kazHydrometWaterPostData.Month) != null)
            {
                ModelState.AddModelError("", $"{_sharedLocalizer["ErrorDublicateValue"]} " +
                    $"({_sharedLocalizer["KazHydrometWaterPost"]}, " +
                    $"{_sharedLocalizer["WaterContaminant"]}, " +
                    $"{_sharedLocalizer["Year"]}, " +
                    $"{_sharedLocalizer["Month"]})");
            }
            if (kazHydrometWaterPostData.Year > DateTime.Today.Year || kazHydrometWaterPostData.Year < Constants.YearDataMin)
            {
                ModelState.AddModelError("Year", String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["Year"], Constants.YearDataMin.ToString(), DateTime.Today.Year.ToString()));
            }
            if (ModelState.IsValid)
            {
                _context.Add(kazHydrometWaterPostData);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "KazHydrometWaterPostData",
                    New = kazHydrometWaterPostData.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["WaterContaminantId"] = new SelectList(_context.WaterContaminant.OrderBy(a => a.Name), "Id", "Name", kazHydrometWaterPostData.WaterContaminantId);
            ViewData["KazHydrometWaterPostId"] = new SelectList(_context.KazHydrometWaterPost.OrderBy(k => k.Number), "Id", "Number", kazHydrometWaterPostData.KazHydrometWaterPostId);
            return View(kazHydrometWaterPostData);
        }

        // GET: KazHydrometWaterPostDatas/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kazHydrometWaterPostData = await _context.KazHydrometWaterPostData.SingleOrDefaultAsync(m => m.Id == id);
            if (kazHydrometWaterPostData == null)
            {
                return NotFound();
            }
            ViewData["WaterContaminantId"] = new SelectList(_context.WaterContaminant.OrderBy(a => a.Name), "Id", "Name", kazHydrometWaterPostData.WaterContaminantId);
            ViewData["KazHydrometWaterPostId"] = new SelectList(_context.KazHydrometWaterPost.OrderBy(k => k.Number), "Id", "Number", kazHydrometWaterPostData.KazHydrometWaterPostId);
            return View(kazHydrometWaterPostData);
        }

        // POST: KazHydrometWaterPostDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KazHydrometWaterPostId,WaterContaminantId,Year,Month,PollutantConcentrationmgl")] KazHydrometWaterPostData kazHydrometWaterPostData)
        {
            if (id != kazHydrometWaterPostData.Id)
            {
                return NotFound();
            }
            if (_context.KazHydrometWaterPostData.AsNoTracking().FirstOrDefault(k => k.Id != id
                && k.KazHydrometWaterPostId == kazHydrometWaterPostData.KazHydrometWaterPostId
                && k.WaterContaminantId == kazHydrometWaterPostData.WaterContaminantId
                && k.Year == kazHydrometWaterPostData.Year
                && k.Month == kazHydrometWaterPostData.Month) != null)
            {
                ModelState.AddModelError("", $"{_sharedLocalizer["ErrorDublicateValue"]} " +
                    $"({_sharedLocalizer["KazHydrometWaterPost"]}, " +
                    $"{_sharedLocalizer["WaterContaminant"]}, " +
                    $"{_sharedLocalizer["Year"]}, " +
                    $"{_sharedLocalizer["Month"]})");
            }
            if (kazHydrometWaterPostData.Year > DateTime.Today.Year || kazHydrometWaterPostData.Year < Constants.YearDataMin)
            {
                ModelState.AddModelError("Year", String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["Year"], Constants.YearDataMin.ToString(), DateTime.Today.Year.ToString()));
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var kazHydrometWaterPostData_old = _context.KazHydrometWaterPostData.AsNoTracking().FirstOrDefault(k => k.Id == kazHydrometWaterPostData.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "KazHydrometWaterPostData",
                        Operation = "Edit",
                        New = kazHydrometWaterPostData.ToString(),
                        Old = kazHydrometWaterPostData_old.ToString()
                    });
                    _context.Update(kazHydrometWaterPostData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KazHydrometWaterPostDataExists(kazHydrometWaterPostData.Id))
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
            ViewData["WaterContaminantId"] = new SelectList(_context.WaterContaminant.OrderBy(a => a.Name), "Id", "Name", kazHydrometWaterPostData.WaterContaminantId);
            ViewData["KazHydrometWaterPostId"] = new SelectList(_context.KazHydrometWaterPost.OrderBy(k => k.Number), "Id", "Number", kazHydrometWaterPostData.KazHydrometWaterPostId);
            return View(kazHydrometWaterPostData);
        }

        // GET: KazHydrometWaterPostDatas/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kazHydrometWaterPostData = await _context.KazHydrometWaterPostData
                .Include(k => k.KazHydrometWaterPost)
                .Include(k => k.WaterContaminant)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (kazHydrometWaterPostData == null)
            {
                return NotFound();
            }

            return View(kazHydrometWaterPostData);
        }

        // POST: KazHydrometWaterPostDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kazHydrometWaterPostData = await _context.KazHydrometWaterPostData.SingleOrDefaultAsync(m => m.Id == id);
            _context.KazHydrometWaterPostData.Remove(kazHydrometWaterPostData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KazHydrometWaterPostDataExists(int id)
        {
            return _context.KazHydrometWaterPostData.Any(e => e.Id == id);
        }
    }
}
