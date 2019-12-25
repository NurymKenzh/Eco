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
    public class KazHydrometSoilPostDatasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public KazHydrometSoilPostDatasController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: KazHydrometSoilPostDatas
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, 
            string KazHydrometSoilPostNumber,
            string KazHydrometSoilPostName,
            int? SoilContaminantId,
            int? Year,
            int? Page)
        {
            var kazHydrometSoilPostDatas = _context.KazHydrometSoilPostData
                .Include(k => k.KazHydrometSoilPost)
                .Include(k => k.SoilContaminant)
                .Where(k => true);

            ViewBag.KazHydrometSoilPostNumberFilter = KazHydrometSoilPostNumber;
            ViewBag.KazHydrometSoilPostNameFilter = KazHydrometSoilPostName;
            ViewBag.SoilContaminantIdFilter = SoilContaminantId;
            ViewBag.YearFilter = Year;

            ViewBag.KazHydrometSoilPostNumberSort = SortOrder == "KazHydrometSoilPostNumber" ? "KazHydrometSoilPostNumberDesc" : "KazHydrometSoilPostNumber";
            ViewBag.KazHydrometSoilPostNameSort = SortOrder == "KazHydrometSoilPostName" ? "KazHydrometSoilPostNameDesc" : "KazHydrometSoilPostName";
            ViewBag.SoilContaminantNameSort = SortOrder == "SoilContaminantName" ? "SoilContaminantNameDesc" : "SoilContaminantName";
            ViewBag.YearSort = SortOrder == "Year" ? "YearDesc" : "Year";

            if (!string.IsNullOrEmpty(KazHydrometSoilPostNumber))
            {
                kazHydrometSoilPostDatas = kazHydrometSoilPostDatas.Where(k => k.KazHydrometSoilPost.Number.ToString()==KazHydrometSoilPostNumber);
            }
            if (!string.IsNullOrEmpty(KazHydrometSoilPostName))
            {
                kazHydrometSoilPostDatas = kazHydrometSoilPostDatas.Where(k => k.KazHydrometSoilPost.Name==KazHydrometSoilPostName);
            }
            if (SoilContaminantId != null)
            {
                kazHydrometSoilPostDatas = kazHydrometSoilPostDatas.Where(k => k.SoilContaminantId == SoilContaminantId);
            }
            if (Year != null)
            {
                kazHydrometSoilPostDatas = kazHydrometSoilPostDatas.Where(k => k.Year == Year);
            }

            switch (SortOrder)
            {
                case "KazHydrometSoilPostNumber":
                    kazHydrometSoilPostDatas = kazHydrometSoilPostDatas.OrderBy(k => k.KazHydrometSoilPost.Number);
                    break;
                case "KazHydrometSoilPostNumberDesc":
                    kazHydrometSoilPostDatas = kazHydrometSoilPostDatas.OrderByDescending(k => k.KazHydrometSoilPost.Number);
                    break;
                case "KazHydrometSoilPostName":
                    kazHydrometSoilPostDatas = kazHydrometSoilPostDatas.OrderBy(k => k.KazHydrometSoilPost.Name);
                    break;
                case "KazHydrometSoilPostNameDesc":
                    kazHydrometSoilPostDatas = kazHydrometSoilPostDatas.OrderByDescending(k => k.KazHydrometSoilPost.Name);
                    break;
                case "SoilContaminantName":
                    kazHydrometSoilPostDatas = kazHydrometSoilPostDatas.OrderBy(k => k.SoilContaminant.Name);
                    break;
                case "SoilContaminantNameDesc":
                    kazHydrometSoilPostDatas = kazHydrometSoilPostDatas.OrderByDescending(k => k.SoilContaminant.Name);
                    break;
                case "Year":
                    kazHydrometSoilPostDatas = kazHydrometSoilPostDatas.OrderBy(k => k.Year);
                    break;
                case "YearDesc":
                    kazHydrometSoilPostDatas = kazHydrometSoilPostDatas.OrderByDescending(k => k.Year);
                    break;
                default:
                    kazHydrometSoilPostDatas = kazHydrometSoilPostDatas.OrderBy(k => k.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(kazHydrometSoilPostDatas.Count(), Page);

            var viewModel = new KazHydrometSoilPostDataIndexPageViewModel
            {
                Items = kazHydrometSoilPostDatas.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };
            var years = _context.KazHydrometSoilPostData.Select(k => k.Year).Distinct().ToList();
            ViewBag.Year = new SelectList(Enumerable.Range(Constants.YearDataMin, Constants.YearMax - Constants.YearDataMin + 1).Where(i => years.Contains(i)).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");
            ViewBag.Month = new SelectList(Enumerable.Range(1, 12).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");

            ViewData["SoilContaminantId"] = new SelectList(_context.SoilContaminant.Where(a => _context.KazHydrometSoilPostData.Include(k => k.SoilContaminant).Select(k => k.SoilContaminantId).Contains(a.Id)).OrderBy(a => a.Name), "Id", "Name");
            ViewData["KazHydrometSoilPostNumber"] = new SelectList(_context.KazHydrometSoilPost.OrderBy(a => a.Number).GroupBy(k => k.Number).Select(g => g.First()), "Number", "Number");
            ViewData["KazHydrometSoilPostName"] = new SelectList(_context.KazHydrometSoilPost.OrderBy(a => a.Name).GroupBy(k => k.Name).Select(g => g.First()), "Name", "Name");
            
            return View(viewModel);
        }

        // GET: KazHydrometSoilPostDatas/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kazHydrometSoilPostData = await _context.KazHydrometSoilPostData
                .Include(k => k.KazHydrometSoilPost)
                .Include(k => k.SoilContaminant)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (kazHydrometSoilPostData == null)
            {
                return NotFound();
            }

            return View(kazHydrometSoilPostData);
        }

        // GET: KazHydrometSoilPostDatas/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            KazHydrometSoilPostData kazHydrometSoilPostData = new KazHydrometSoilPostData();
            ViewData["SoilContaminantId"] = new SelectList(_context.SoilContaminant.OrderBy(a => a.Name), "Id", "Name");
            ViewData["KazHydrometSoilPostId"] = new SelectList(_context.KazHydrometSoilPost.OrderBy(k => k.Number), "Id", "Number");
            return View(kazHydrometSoilPostData);
        }

        // POST: KazHydrometSoilPostDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,KazHydrometSoilPostId,SoilContaminantId,Year,Season,ConcentrationValuemgkg")] KazHydrometSoilPostData kazHydrometSoilPostData)
        {
            if (_context.KazHydrometSoilPostData.AsNoTracking().FirstOrDefault(k => k.KazHydrometSoilPostId == kazHydrometSoilPostData.KazHydrometSoilPostId
                && k.SoilContaminantId == kazHydrometSoilPostData.SoilContaminantId
                && k.Year == kazHydrometSoilPostData.Year
                && k.Season == kazHydrometSoilPostData.Season) != null)
            {
                ModelState.AddModelError("", $"{_sharedLocalizer["ErrorDublicateValue"]} " +
                    $"({_sharedLocalizer["KazHydrometSoilPost"]}, " +
                    $"{_sharedLocalizer["SoilContaminant"]}, " +
                    $"{_sharedLocalizer["Year"]}, " +
                    $"{_sharedLocalizer["Season"]})");
            }
            if(kazHydrometSoilPostData.Year>DateTime.Today.Year || kazHydrometSoilPostData.Year<Constants.YearDataMin)
            {
                ModelState.AddModelError("Year", String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["Year"], Constants.YearDataMin.ToString(), DateTime.Today.Year.ToString()));
            }
            if (ModelState.IsValid)
            {
                _context.Add(kazHydrometSoilPostData);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "KazHydrometSoilPostData",
                    New = kazHydrometSoilPostData.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SoilContaminantId"] = new SelectList(_context.SoilContaminant.OrderBy(a => a.Name), "Id", "Name", kazHydrometSoilPostData.SoilContaminantId);
            ViewData["KazHydrometSoilPostId"] = new SelectList(_context.KazHydrometSoilPost.OrderBy(k => k.Number), "Id", "Number", kazHydrometSoilPostData.KazHydrometSoilPostId);
            return View(kazHydrometSoilPostData);
        }

        // GET: KazHydrometSoilPostDatas/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kazHydrometSoilPostData = await _context.KazHydrometSoilPostData.SingleOrDefaultAsync(m => m.Id == id);
            if (kazHydrometSoilPostData == null)
            {
                return NotFound();
            }
            ViewData["SoilContaminantId"] = new SelectList(_context.SoilContaminant.OrderBy(a => a.Name), "Id", "Name", kazHydrometSoilPostData.SoilContaminantId);
            ViewData["KazHydrometSoilPostId"] = new SelectList(_context.KazHydrometSoilPost.OrderBy(k => k.Number), "Id", "Number", kazHydrometSoilPostData.KazHydrometSoilPostId);
            return View(kazHydrometSoilPostData);
        }

        // POST: KazHydrometSoilPostDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KazHydrometSoilPostId,SoilContaminantId,Year,Season,ConcentrationValuemgkg")] KazHydrometSoilPostData kazHydrometSoilPostData)
        {
            if (id != kazHydrometSoilPostData.Id)
            {
                return NotFound();
            }
            if (_context.KazHydrometSoilPostData.AsNoTracking().FirstOrDefault(k => k.KazHydrometSoilPostId == kazHydrometSoilPostData.KazHydrometSoilPostId
                && k.SoilContaminantId == kazHydrometSoilPostData.SoilContaminantId
                && k.Year == kazHydrometSoilPostData.Year
                && k.Season == kazHydrometSoilPostData.Season) != null)
            {
                ModelState.AddModelError("", $"{_sharedLocalizer["ErrorDublicateValue"]} " +
                    $"({_sharedLocalizer["KazHydrometSoilPost"]}, " +
                    $"{_sharedLocalizer["SoilContaminant"]}, " +
                    $"{_sharedLocalizer["Year"]}, " +
                    $"{_sharedLocalizer["Season"]})");
            }
            if (kazHydrometSoilPostData.Year > DateTime.Today.Year || kazHydrometSoilPostData.Year < Constants.YearDataMin)
            {
                ModelState.AddModelError("Year", String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["Year"], Constants.YearDataMin.ToString(), DateTime.Today.Year.ToString()));
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var kazHydrometSoilPostData_old = _context.KazHydrometSoilPostData.AsNoTracking().FirstOrDefault(k => k.Id == kazHydrometSoilPostData.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "KazHydrometSoilPostData",
                        Operation = "Edit",
                        New = kazHydrometSoilPostData.ToString(),
                        Old = kazHydrometSoilPostData_old.ToString()
                    });
                    _context.Update(kazHydrometSoilPostData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KazHydrometSoilPostDataExists(kazHydrometSoilPostData.Id))
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
            ViewData["SoilContaminantId"] = new SelectList(_context.SoilContaminant.OrderBy(a => a.Name), "Id", "Name", kazHydrometSoilPostData.SoilContaminantId);
            ViewData["KazHydrometSoilPostId"] = new SelectList(_context.KazHydrometSoilPost.OrderBy(k => k.Number), "Id", "Number", kazHydrometSoilPostData.KazHydrometSoilPostId);
            return View(kazHydrometSoilPostData);
        }

        // GET: KazHydrometSoilPostDatas/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kazHydrometSoilPostData = await _context.KazHydrometSoilPostData
                .Include(k => k.KazHydrometSoilPost)
                .Include(k => k.SoilContaminant)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (kazHydrometSoilPostData == null)
            {
                return NotFound();
            }

            return View(kazHydrometSoilPostData);
        }

        // POST: KazHydrometSoilPostDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kazHydrometSoilPostData = await _context.KazHydrometSoilPostData.SingleOrDefaultAsync(m => m.Id == id);
            _context.KazHydrometSoilPostData.Remove(kazHydrometSoilPostData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KazHydrometSoilPostDataExists(int id)
        {
            return _context.KazHydrometSoilPostData.Any(e => e.Id == id);
        }
    }
}
