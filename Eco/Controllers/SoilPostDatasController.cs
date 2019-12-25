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
    public class SoilPostDatasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public SoilPostDatasController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: SoilPostDatas
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            string SoilPostName,
            int? SoilContaminantId,
            int? YearOfSampling,
            int? MonthOfSampling,
            int? DayOfSampling,
            int? Page)
        {
            var soilPostDatas = _context.SoilPostData
                .Include(w => w.SoilContaminant)
                .Include(w => w.SoilPost)
                .Where(w => true);
            
            ViewBag.SoilPostNameFilter = SoilPostName;
            ViewBag.SoilContaminantIdFilter = SoilContaminantId;
            ViewBag.YearOfSamplingFilter = YearOfSampling;
            ViewBag.MonthOfSamplingFilter = MonthOfSampling;
            ViewBag.DayOfSamplingFilter = MonthOfSampling;
            
            ViewBag.SoilPostNameSort = SortOrder == "SoilPostName" ? "SoilPostNameDesc" : "SoilPostName";
            ViewBag.SoilContaminantNameSort = SortOrder == "SoilContaminantName" ? "SoilContaminantNameDesc" : "SoilContaminantName";
            ViewBag.YearOfSamplingSort = SortOrder == "YearOfSampling" ? "YearOfSamplingDesc" : "YearOfSampling";
            ViewBag.MonthOfSamplingSort = SortOrder == "MonthOfSampling" ? "MonthOfSamplingDesc" : "MonthOfSampling";
            ViewBag.DayOfSamplingSort = SortOrder == "DayOfSampling" ? "DayOfSamplingDesc" : "DayOfSampling";
            
            if (!string.IsNullOrEmpty(SoilPostName))
            {
                soilPostDatas = soilPostDatas.Where(k => k.SoilPost.Name.ToString().ToString().ToLower().Contains(SoilPostName.ToLower()));
            }
            if (SoilContaminantId != null)
            {
                soilPostDatas = soilPostDatas.Where(k => k.SoilContaminantId == SoilContaminantId);
            }
            if (YearOfSampling != null)
            {
                soilPostDatas = soilPostDatas.Where(k => k.YearOfSampling == YearOfSampling);
            }
            if (MonthOfSampling != null)
            {
                soilPostDatas = soilPostDatas.Where(k => k.MonthOfSampling == MonthOfSampling);
            }
            if (DayOfSampling != null)
            {
                soilPostDatas = soilPostDatas.Where(k => k.DayOfSampling == DayOfSampling);
            }

            switch (SortOrder)
            {
                case "SoilPostName":
                    soilPostDatas = soilPostDatas.OrderBy(k => k.SoilPost.Name);
                    break;
                case "SoilPostNameDesc":
                    soilPostDatas = soilPostDatas.OrderByDescending(k => k.SoilPost.Name);
                    break;
                case "SoilContaminantName":
                    soilPostDatas = soilPostDatas.OrderBy(k => k.SoilContaminant.Name);
                    break;
                case "SoilContaminantNameDesc":
                    soilPostDatas = soilPostDatas.OrderByDescending(k => k.SoilContaminant.Name);
                    break;
                case "YearOfSampling":
                    soilPostDatas = soilPostDatas.OrderBy(k => k.YearOfSampling);
                    break;
                case "YearOfSamplingDesc":
                    soilPostDatas = soilPostDatas.OrderByDescending(k => k.YearOfSampling);
                    break;
                case "MonthOfSampling":
                    soilPostDatas = soilPostDatas.OrderBy(k => k.MonthOfSampling);
                    break;
                case "MonthOfSamplingDesc":
                    soilPostDatas = soilPostDatas.OrderByDescending(k => k.MonthOfSampling);
                    break;
                case "DayOfSampling":
                    soilPostDatas = soilPostDatas.OrderBy(k => k.DayOfSampling);
                    break;
                case "DayOfSamplingDesc":
                    soilPostDatas = soilPostDatas.OrderByDescending(k => k.DayOfSampling);
                    break;
                default:
                    soilPostDatas = soilPostDatas.OrderBy(k => k.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(soilPostDatas.Count(), Page);

            var viewModel = new SoilPostDataIndexPageViewModel
            {
                Items = soilPostDatas.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            var years = _context.SoilPostData.Select(k => k.DateOfSampling.Year).Distinct().ToList();
            ViewBag.YearOfSampling = new SelectList(Enumerable.Range(Constants.YearMin, Constants.YearMax - Constants.YearMin + 1).Where(i => years.Contains(i)).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");
            ViewBag.MonthOfSampling = new SelectList(Enumerable.Range(1, 12).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");
            ViewBag.DayOfSampling = new SelectList(Enumerable.Range(1, 31).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");

            ViewData["SoilContaminantId"] = new SelectList(_context.SoilContaminant.Where(a => _context.SoilPostData.Include(k => k.SoilContaminant).Select(k => k.SoilContaminantId).Contains(a.Id)).OrderBy(a => a.Name), "Id", "Name");

            return View(viewModel);
        }

        // GET: SoilPostDatas/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soilPostData = await _context.SoilPostData
                .Include(s => s.SoilContaminant)
                .Include(s => s.SoilPost)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (soilPostData == null)
            {
                return NotFound();
            }

            return View(soilPostData);
        }

        // GET: SoilPostDatas/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            SoilPostData model = new SoilPostData();
            model.DateOfSampling = DateTime.Now;
            model.YearOfSampling = model.DateOfSampling.Year;
            model.MonthOfSampling = model.DateOfSampling.Month;
            model.DayOfSampling = model.DateOfSampling.Day;
            ViewData["SoilContaminantId"] = new SelectList(_context.SoilContaminant.OrderBy(w => w.Name), "Id", "Name");
            ViewData["SoilPostId"] = new SelectList(_context.SoilPost.OrderBy(w => w.Name), "Id", "Name");
            return View();
        }

        // POST: SoilPostDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,SoilPostId,SoilContaminantId,YearOfSampling,MonthOfSampling,DayOfSampling,GammaBackgroundOfTheSoil,ConcentrationValuemgkg")] SoilPostData soilPostData)
        {
            if (ModelState.IsValid)
            {
                while (soilPostData.DayOfSampling > 28)
                {
                    try
                    {
                        soilPostData.DateOfSampling = new DateTime(
                            soilPostData.YearOfSampling,
                            soilPostData.MonthOfSampling,
                            soilPostData.DayOfSampling);
                    }
                    catch
                    {
                        soilPostData.DayOfSampling--;
                    }
                }
                try
                {
                    soilPostData.DateOfSampling = new DateTime(
                        soilPostData.YearOfSampling,
                        soilPostData.MonthOfSampling,
                        soilPostData.DayOfSampling);
                }
                catch
                {

                }
                _context.Add(soilPostData);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "SoilPostData",
                    New = soilPostData.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            try
            {
                soilPostData.YearOfSampling = soilPostData.DateOfSampling.Year;
                soilPostData.MonthOfSampling = soilPostData.DateOfSampling.Month;
                soilPostData.DayOfSampling = soilPostData.DateOfSampling.Day;
            }
            catch
            {

            }
            ViewData["SoilContaminantId"] = new SelectList(_context.SoilContaminant.OrderBy(w => w.Name), "Id", "Name", soilPostData.SoilContaminantId);
            ViewData["SoilPostId"] = new SelectList(_context.SoilPost.OrderBy(w => w.Name), "Id", "Name", soilPostData.SoilPostId);
            return View(soilPostData);
        }

        // GET: SoilPostDatas/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soilPostData = await _context.SoilPostData.SingleOrDefaultAsync(m => m.Id == id);
            if (soilPostData == null)
            {
                return NotFound();
            }
            if (soilPostData == null)
            {
                return NotFound();
            }
            try
            {
                soilPostData.YearOfSampling = soilPostData.DateOfSampling.Year;
                soilPostData.MonthOfSampling = soilPostData.DateOfSampling.Month;
                soilPostData.DayOfSampling = soilPostData.DateOfSampling.Day;
            }
            catch
            {

            }
            ViewData["SoilContaminantId"] = new SelectList(_context.SoilContaminant.OrderBy(w => w.Name), "Id", "Name", soilPostData.SoilContaminantId);
            ViewData["SoilPostId"] = new SelectList(_context.SoilPost.OrderBy(w => w.Name), "Id", "Name", soilPostData.SoilPostId);
            return View(soilPostData);
        }

        // POST: SoilPostDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SoilPostId,SoilContaminantId,YearOfSampling,MonthOfSampling,DayOfSampling,GammaBackgroundOfTheSoil,ConcentrationValuemgkg")] SoilPostData soilPostData)
        {
            if (id != soilPostData.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                while (soilPostData.DayOfSampling > 28)
                {
                    try
                    {
                        soilPostData.DateOfSampling = new DateTime(
                            soilPostData.YearOfSampling,
                            soilPostData.MonthOfSampling,
                            soilPostData.DayOfSampling);
                    }
                    catch
                    {
                        soilPostData.DayOfSampling--;
                    }
                }
                try
                {
                    soilPostData.DateOfSampling = new DateTime(
                        soilPostData.YearOfSampling,
                        soilPostData.MonthOfSampling,
                        soilPostData.DayOfSampling);
                }
                catch
                {

                }
                try
                {
                    var soilPostData_old = _context.SoilPostData.AsNoTracking().FirstOrDefault(k => k.Id == soilPostData.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "SoilPostData",
                        Operation = "Edit",
                        New = soilPostData.ToString(),
                        Old = soilPostData_old.ToString()
                    });
                    _context.Update(soilPostData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SoilPostDataExists(soilPostData.Id))
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
            try
            {
                soilPostData.YearOfSampling = soilPostData.DateOfSampling.Year;
                soilPostData.MonthOfSampling = soilPostData.DateOfSampling.Month;
                soilPostData.DayOfSampling = soilPostData.DateOfSampling.Day;
            }
            catch
            {

            }
            ViewData["SoilContaminantId"] = new SelectList(_context.SoilContaminant.OrderBy(w => w.Name), "Id", "Name", soilPostData.SoilContaminantId);
            ViewData["SoilPostId"] = new SelectList(_context.SoilPost.OrderBy(w => w.Name), "Id", "Name", soilPostData.SoilPostId);
            return View(soilPostData);
        }

        // GET: SoilPostDatas/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soilPostData = await _context.SoilPostData
                .Include(s => s.SoilContaminant)
                .Include(s => s.SoilPost)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (soilPostData == null)
            {
                return NotFound();
            }

            return View(soilPostData);
        }

        // POST: SoilPostDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var soilPostData = await _context.SoilPostData.SingleOrDefaultAsync(m => m.Id == id);
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Class = "SoilPostData",
                Operation = "Delete",
                New = "",
                Old = soilPostData.ToString()
            });
            _context.SoilPostData.Remove(soilPostData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SoilPostDataExists(int id)
        {
            return _context.SoilPostData.Any(e => e.Id == id);
        }
    }
}
