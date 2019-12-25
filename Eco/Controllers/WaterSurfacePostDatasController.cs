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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using OfficeOpenXml;

namespace Eco.Controllers
{
    public class WaterSurfacePostDatasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;
        private readonly IHostingEnvironment _hostingEnvironment;

        public WaterSurfacePostDatasController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer,
            IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: WaterSurfacePostDatas
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            int? WaterObjectId,
            string WaterSurfacePostNumber,
            int? WaterContaminantId,
            int? YearOfSampling,
            int? MonthOfSampling,
            int? DayOfSampling,
            int? Page)
        {
            var waterSurfacePostDatas = _context.WaterSurfacePostData
                .Include(w => w.WaterContaminant)
                .Include(w => w.WaterSurfacePost)
                .Include(w => w.WaterSurfacePost.WaterObject)
                .Where(w => true);

            ViewBag.WaterObjectIdFilter = WaterObjectId;
            ViewBag.WaterSurfacePostNumberFilter = WaterSurfacePostNumber;
            ViewBag.WaterContaminantIdFilter = WaterContaminantId;
            ViewBag.YearOfSamplingFilter = YearOfSampling;
            ViewBag.MonthOfSamplingFilter = MonthOfSampling;
            ViewBag.DayOfSamplingFilter = MonthOfSampling;

            ViewBag.WaterObjectNameSort = SortOrder == "WaterObjectName" ? "WaterObjectNameDesc" : "WaterObjectName";
            ViewBag.WaterSurfacePostNumberSort = SortOrder == "WaterSurfacePostNumber" ? "WaterSurfacePostNumberDesc" : "WaterSurfacePostNumber";
            ViewBag.WaterContaminantNameSort = SortOrder == "WaterContaminantName" ? "WaterContaminantNameDesc" : "WaterContaminantName";
            ViewBag.YearOfSamplingSort = SortOrder == "YearOfSampling" ? "YearOfSamplingDesc" : "YearOfSampling";
            ViewBag.MonthOfSamplingSort = SortOrder == "MonthOfSampling" ? "MonthOfSamplingDesc" : "MonthOfSampling";
            ViewBag.DayOfSamplingSort = SortOrder == "DayOfSampling" ? "DayOfSamplingDesc" : "DayOfSampling";

            if (WaterObjectId != null)
            {
                waterSurfacePostDatas = waterSurfacePostDatas.Where(k => k.WaterSurfacePost.WaterObjectId == WaterObjectId);
            }
            if (!string.IsNullOrEmpty(WaterSurfacePostNumber))
            {
                waterSurfacePostDatas = waterSurfacePostDatas.Where(k => k.WaterSurfacePost.Number.ToString().ToString().ToLower().Contains(WaterSurfacePostNumber.ToLower()));
            }
            if (WaterContaminantId != null)
            {
                waterSurfacePostDatas = waterSurfacePostDatas.Where(k => k.WaterContaminantId == WaterContaminantId);
            }
            if (YearOfSampling != null)
            {
                waterSurfacePostDatas = waterSurfacePostDatas.Where(k => k.YearOfSampling == YearOfSampling);
            }
            if (MonthOfSampling != null)
            {
                waterSurfacePostDatas = waterSurfacePostDatas.Where(k => k.MonthOfSampling == MonthOfSampling);
            }
            if (DayOfSampling != null)
            {
                waterSurfacePostDatas = waterSurfacePostDatas.Where(k => k.DayOfSampling == DayOfSampling);
            }

            switch (SortOrder)
            {
                case "WaterObjectName":
                    waterSurfacePostDatas = waterSurfacePostDatas.OrderBy(k => k.WaterSurfacePost.WaterObject.Name);
                    break;
                case "WaterObjectNameDesc":
                    waterSurfacePostDatas = waterSurfacePostDatas.OrderByDescending(k => k.WaterSurfacePost.WaterObject.Name);
                    break;
                case "WaterSurfacePostNumber":
                    waterSurfacePostDatas = waterSurfacePostDatas.OrderBy(k => k.WaterSurfacePost.Number);
                    break;
                case "WaterSurfacePostNumberDesc":
                    waterSurfacePostDatas = waterSurfacePostDatas.OrderByDescending(k => k.WaterSurfacePost.Number);
                    break;
                case "WaterContaminantName":
                    waterSurfacePostDatas = waterSurfacePostDatas.OrderBy(k => k.WaterContaminant.Name);
                    break;
                case "WaterContaminantNameDesc":
                    waterSurfacePostDatas = waterSurfacePostDatas.OrderByDescending(k => k.WaterContaminant.Name);
                    break;
                case "YearOfSampling":
                    waterSurfacePostDatas = waterSurfacePostDatas.OrderBy(k => k.YearOfSampling);
                    break;
                case "YearOfSamplingDesc":
                    waterSurfacePostDatas = waterSurfacePostDatas.OrderByDescending(k => k.YearOfSampling);
                    break;
                case "MonthOfSampling":
                    waterSurfacePostDatas = waterSurfacePostDatas.OrderBy(k => k.MonthOfSampling);
                    break;
                case "MonthOfSamplingDesc":
                    waterSurfacePostDatas = waterSurfacePostDatas.OrderByDescending(k => k.MonthOfSampling);
                    break;
                case "DayOfSampling":
                    waterSurfacePostDatas = waterSurfacePostDatas.OrderBy(k => k.DayOfSampling);
                    break;
                case "DayOfSamplingDesc":
                    waterSurfacePostDatas = waterSurfacePostDatas.OrderByDescending(k => k.DayOfSampling);
                    break;
                default:
                    waterSurfacePostDatas = waterSurfacePostDatas.OrderBy(k => k.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(waterSurfacePostDatas.Count(), Page);

            var viewModel = new WaterSurfacePostDataIndexPageViewModel
            {
                Items = waterSurfacePostDatas.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            var years = _context.WaterSurfacePostData.Select(k => k.DateOfSampling.Year).Distinct().ToList();
            ViewBag.YearOfSampling = new SelectList(Enumerable.Range(Constants.YearMin, Constants.YearMax - Constants.YearMin + 1).Where(i => years.Contains(i)).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");
            ViewBag.MonthOfSampling = new SelectList(Enumerable.Range(1, 12).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");
            ViewBag.DayOfSampling = new SelectList(Enumerable.Range(1, 31).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");

            ViewData["WaterObjectId"] = new SelectList(_context.WaterObject.OrderBy(a => a.Name), "Id", "Name");
            ViewData["WaterContaminantId"] = new SelectList(_context.WaterContaminant.Where(a => _context.WaterSurfacePostData.Include(k => k.WaterContaminant).Select(k => k.WaterContaminantId).Contains(a.Id)).OrderBy(a => a.Name), "Id", "Name");

            return View(viewModel);
        }

        // GET: WaterSurfacePostDatas/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterSurfacePostData = await _context.WaterSurfacePostData
                .Include(w => w.WaterContaminant)
                .Include(w => w.WaterSurfacePost)
                .Include(w => w.WaterSurfacePost.WaterObject)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (waterSurfacePostData == null)
            {
                return NotFound();
            }

            return View(waterSurfacePostData);
        }

        // GET: WaterSurfacePostDatas/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["WaterContaminantId"] = new SelectList(_context.WaterContaminant.OrderBy(w => w.Name), "Id", "Name");
            var WaterObjectId = _context.WaterObject.OrderBy(k => k.Name);
            ViewData["WaterObjectId"] = new SelectList(WaterObjectId, "Id", "Name");
            ViewData["WaterSurfacePostId"] = new SelectList(_context.WaterSurfacePost.OrderBy(k => k.Number).Where(w => w.WaterObjectId == WaterObjectId.FirstOrDefault().Id), "Id", "Number");
            WaterSurfacePostData model = new WaterSurfacePostData();
            model.DateOfSampling = DateTime.Now;
            model.YearOfSampling = model.DateOfSampling.Year;
            model.MonthOfSampling = model.DateOfSampling.Month;
            model.DayOfSampling = model.DateOfSampling.Day;
            model.DateOfAnalysis = DateTime.Now;
            model.YearOfAnalysis = model.DateOfAnalysis.Year;
            model.MonthOfAnalysis = model.DateOfAnalysis.Month;
            model.DayOfAnalysis = model.DateOfAnalysis.Day;
            return View(model);
        }

        // POST: WaterSurfacePostDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,WaterSurfacePostId,WaterContaminantId,YearOfSampling,MonthOfSampling,DayOfSampling,YearOfAnalysis,MonthOfAnalysis,DayOfAnalysis,Value")] WaterSurfacePostData waterSurfacePostData)
        {
            if (waterSurfacePostData.WaterSurfacePostId == 0)
            {
                ModelState.AddModelError("WaterSurfacePostId", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                while (waterSurfacePostData.DayOfSampling > 28)
                {
                    try
                    {
                        waterSurfacePostData.DateOfSampling = new DateTime(
                            waterSurfacePostData.YearOfSampling,
                            waterSurfacePostData.MonthOfSampling,
                            waterSurfacePostData.DayOfSampling);
                    }
                    catch
                    {
                        waterSurfacePostData.DayOfSampling--;
                    }
                }
                try
                {
                    waterSurfacePostData.DateOfSampling = new DateTime(
                        waterSurfacePostData.YearOfSampling,
                        waterSurfacePostData.MonthOfSampling,
                        waterSurfacePostData.DayOfSampling);
                }
                catch
                {

                }
                while (waterSurfacePostData.DayOfAnalysis > 28)
                {
                    try
                    {
                        waterSurfacePostData.DateOfAnalysis = new DateTime(
                            waterSurfacePostData.YearOfAnalysis,
                            waterSurfacePostData.MonthOfAnalysis,
                            waterSurfacePostData.DayOfAnalysis);
                    }
                    catch
                    {
                        waterSurfacePostData.DayOfAnalysis--;
                    }
                }
                try
                {
                    waterSurfacePostData.DateOfAnalysis = new DateTime(
                        waterSurfacePostData.YearOfAnalysis,
                        waterSurfacePostData.MonthOfAnalysis,
                        waterSurfacePostData.DayOfAnalysis);
                }
                catch
                {

                }
                _context.Add(waterSurfacePostData);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "WaterSurfacePostData",
                    New = waterSurfacePostData.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            try
            {
                waterSurfacePostData.YearOfSampling = waterSurfacePostData.DateOfSampling.Year;
                waterSurfacePostData.MonthOfSampling = waterSurfacePostData.DateOfSampling.Month;
                waterSurfacePostData.DayOfSampling = waterSurfacePostData.DateOfSampling.Day;
            }
            catch
            {

            }
            try
            {
                waterSurfacePostData.YearOfAnalysis = waterSurfacePostData.DateOfAnalysis.Year;
                waterSurfacePostData.MonthOfAnalysis = waterSurfacePostData.DateOfAnalysis.Month;
                waterSurfacePostData.DayOfAnalysis = waterSurfacePostData.DateOfAnalysis.Day;
            }
            catch
            {

            }
            ViewData["WaterContaminantId"] = new SelectList(_context.WaterContaminant.OrderBy(w => w.Name), "Id", "Name", waterSurfacePostData.WaterContaminantId);
            var WaterObjectId = _context.WaterObject.OrderBy(k => k.Name);
            waterSurfacePostData.WaterSurfacePost = _context.WaterSurfacePost.FirstOrDefault(w => w.Id == waterSurfacePostData.WaterSurfacePostId);
            ViewData["WaterObjectId"] = new SelectList(WaterObjectId, "Id", "Name", waterSurfacePostData.WaterSurfacePost?.WaterObjectId);
            ViewData["WaterSurfacePostId"] = new SelectList(_context.WaterSurfacePost.OrderBy(k => k.Number).Where(w => w.WaterObjectId == WaterObjectId.FirstOrDefault().Id), "Id", "Number", waterSurfacePostData.WaterSurfacePostId);
            return View(waterSurfacePostData);
        }

        // GET: WaterSurfacePostDatas/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterSurfacePostData = await _context.WaterSurfacePostData.SingleOrDefaultAsync(m => m.Id == id);
            if (waterSurfacePostData == null)
            {
                return NotFound();
            }
            try
            {
                waterSurfacePostData.YearOfSampling = waterSurfacePostData.DateOfSampling.Year;
                waterSurfacePostData.MonthOfSampling = waterSurfacePostData.DateOfSampling.Month;
                waterSurfacePostData.DayOfSampling = waterSurfacePostData.DateOfSampling.Day;
            }
            catch
            {

            }
            try
            {
                waterSurfacePostData.YearOfAnalysis = waterSurfacePostData.DateOfAnalysis.Year;
                waterSurfacePostData.MonthOfAnalysis = waterSurfacePostData.DateOfAnalysis.Month;
                waterSurfacePostData.DayOfAnalysis = waterSurfacePostData.DateOfAnalysis.Day;
            }
            catch
            {

            }
            ViewData["WaterContaminantId"] = new SelectList(_context.WaterContaminant.OrderBy(w => w.Name), "Id", "Name", waterSurfacePostData.WaterContaminantId);
            var WaterObjectId = _context.WaterObject.OrderBy(k => k.Name);
            waterSurfacePostData.WaterSurfacePost = _context.WaterSurfacePost.FirstOrDefault(w => w.Id == waterSurfacePostData.WaterSurfacePostId);
            ViewData["WaterObjectId"] = new SelectList(WaterObjectId, "Id", "Name", waterSurfacePostData.WaterSurfacePost?.WaterObjectId);
            ViewData["WaterSurfacePostId"] = new SelectList(_context.WaterSurfacePost.OrderBy(k => k.Number).Where(w => w.WaterObjectId == waterSurfacePostData.WaterSurfacePost.WaterObjectId), "Id", "Number", waterSurfacePostData.WaterSurfacePostId);
            return View(waterSurfacePostData);
        }

        // POST: WaterSurfacePostDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WaterSurfacePostId,WaterContaminantId,YearOfSampling,MonthOfSampling,DayOfSampling,YearOfAnalysis,MonthOfAnalysis,DayOfAnalysis,Value")] WaterSurfacePostData waterSurfacePostData)
        {
            if (id != waterSurfacePostData.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                while (waterSurfacePostData.DayOfSampling > 28)
                {
                    try
                    {
                        waterSurfacePostData.DateOfSampling = new DateTime(
                            waterSurfacePostData.YearOfSampling,
                            waterSurfacePostData.MonthOfSampling,
                            waterSurfacePostData.DayOfSampling);
                    }
                    catch
                    {
                        waterSurfacePostData.DayOfSampling--;
                    }
                }
                try
                {
                    waterSurfacePostData.DateOfSampling = new DateTime(
                        waterSurfacePostData.YearOfSampling,
                        waterSurfacePostData.MonthOfSampling,
                        waterSurfacePostData.DayOfSampling);
                }
                catch
                {

                }
                while (waterSurfacePostData.DayOfAnalysis > 28)
                {
                    try
                    {
                        waterSurfacePostData.DateOfAnalysis = new DateTime(
                            waterSurfacePostData.YearOfAnalysis,
                            waterSurfacePostData.MonthOfAnalysis,
                            waterSurfacePostData.DayOfAnalysis);
                    }
                    catch
                    {
                        waterSurfacePostData.DayOfAnalysis--;
                    }
                }
                try
                {
                    waterSurfacePostData.DateOfAnalysis = new DateTime(
                        waterSurfacePostData.YearOfAnalysis,
                        waterSurfacePostData.MonthOfAnalysis,
                        waterSurfacePostData.DayOfAnalysis);
                }
                catch
                {

                }
                try
                {
                    var waterSurfacePostData_old = _context.WaterSurfacePostData.AsNoTracking().FirstOrDefault(k => k.Id == waterSurfacePostData.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "WaterSurfacePostData",
                        Operation = "Edit",
                        New = waterSurfacePostData.ToString(),
                        Old = waterSurfacePostData_old.ToString()
                    });
                    _context.Update(waterSurfacePostData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WaterSurfacePostDataExists(waterSurfacePostData.Id))
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
                waterSurfacePostData.YearOfSampling = waterSurfacePostData.DateOfSampling.Year;
                waterSurfacePostData.MonthOfSampling = waterSurfacePostData.DateOfSampling.Month;
                waterSurfacePostData.DayOfSampling = waterSurfacePostData.DateOfSampling.Day;
            }
            catch
            {

            }
            try
            {
                waterSurfacePostData.YearOfAnalysis = waterSurfacePostData.DateOfAnalysis.Year;
                waterSurfacePostData.MonthOfAnalysis = waterSurfacePostData.DateOfAnalysis.Month;
                waterSurfacePostData.DayOfAnalysis = waterSurfacePostData.DateOfAnalysis.Day;
            }
            catch
            {

            }
            ViewData["WaterContaminantId"] = new SelectList(_context.WaterContaminant.OrderBy(w => w.Name), "Id", "Name", waterSurfacePostData.WaterContaminantId);
            var WaterObjectId = _context.WaterObject.OrderBy(k => k.Name);
            waterSurfacePostData.WaterSurfacePost = _context.WaterSurfacePost.FirstOrDefault(w => w.Id == waterSurfacePostData.WaterSurfacePostId);
            ViewData["WaterObjectId"] = new SelectList(WaterObjectId, "Id", "Name", waterSurfacePostData.WaterSurfacePost?.WaterObjectId);
            ViewData["WaterSurfacePostId"] = new SelectList(_context.WaterSurfacePost.OrderBy(k => k.Number).Where(w => w.WaterObjectId == WaterObjectId.FirstOrDefault().Id), "Id", "Number", waterSurfacePostData.WaterSurfacePostId);
            return View(waterSurfacePostData);
        }

        // GET: WaterSurfacePostDatas/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterSurfacePostData = await _context.WaterSurfacePostData
                .Include(w => w.WaterContaminant)
                .Include(w => w.WaterSurfacePost)
                .Include(w => w.WaterSurfacePost.WaterObject)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (waterSurfacePostData == null)
            {
                return NotFound();
            }

            return View(waterSurfacePostData);
        }

        // POST: WaterSurfacePostDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var waterSurfacePostData = await _context.WaterSurfacePostData.SingleOrDefaultAsync(m => m.Id == id);
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Class = "WaterSurfacePostData",
                Operation = "Delete",
                New = "",
                Old = waterSurfacePostData.ToString()
            });
            _context.WaterSurfacePostData.Remove(waterSurfacePostData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WaterSurfacePostDataExists(int id)
        {
            return _context.WaterSurfacePostData.Any(e => e.Id == id);
        }

        [HttpPost]
        public JsonResult GetWaterSurfacePostsByWaterObjectId(int WaterObjectId)
        {
            var waterSurfacePosts = _context.WaterSurfacePost
                .Where(s => s.WaterObjectId == WaterObjectId).ToArray().OrderBy(s => s.Number);
            JsonResult result = new JsonResult(waterSurfacePosts);
            return result;
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Upload()
        {
            ViewData["WaterContaminantId"] = new SelectList(_context.WaterContaminant.OrderBy(w => w.Name), "Id", "Name");
            var WaterObjectId = _context.WaterObject.OrderBy(k => k.Name);
            ViewData["WaterObjectId"] = new SelectList(WaterObjectId, "Id", "Name");
            ViewData["WaterSurfacePostId"] = new SelectList(_context.WaterSurfacePost.OrderBy(k => k.Number).Where(w => w.WaterObjectId == WaterObjectId.FirstOrDefault().Id), "Id", "Number");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Upload(int WaterObjectId, int WaterSurfacePostId, int WaterContaminantId, bool FirstRowHeader, IFormFile File)
        {
            try
            {
                string sContentRootPath = _hostingEnvironment.WebRootPath;
                sContentRootPath = Path.Combine(sContentRootPath, "Upload");
                DirectoryInfo di = new DirectoryInfo(sContentRootPath);
                foreach (FileInfo filed in di.GetFiles())
                {
                    try
                    {
                        filed.Delete();
                    }
                    catch
                    {
                    }
                }
                string path_filename = Path.Combine(sContentRootPath, Path.GetFileName(File.FileName));
                using (var stream = new FileStream(Path.GetFullPath(path_filename), FileMode.Create))
                {
                    await File.CopyToAsync(stream);
                }
                FileInfo fileinfo = new FileInfo(Path.Combine(sContentRootPath, Path.GetFileName(path_filename)));
                using (ExcelPackage package = new ExcelPackage(fileinfo))
                {
                    int start_row = 1;
                    if (FirstRowHeader)
                    {
                        start_row++;
                    }
                    List<WaterSurfacePostData> waterSurfacePostDatas = new List<WaterSurfacePostData>();
                    for (int i = start_row; ; i++)
                    {
                        if (package.Workbook.Worksheets.FirstOrDefault().Cells[i, 1].Value == null)
                        {
                            break;
                        }
                        DateTime DateOfSampling = new DateTime(),
                            DateOfAnalysis = new DateTime();
                        int Value = 0;

                        try
                        {
                            DateOfSampling = Convert.ToDateTime(package.Workbook.Worksheets.FirstOrDefault().Cells[i, 1].Value);
                        }
                        catch (Exception e)
                        {
                            ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 1: " + e.Message + (e.InnerException == null ? "" : ": " + e.InnerException.Message);
                            if (((string)ViewBag.Error).Contains("Input string was not in a correct format."))
                            {
                                ViewBag.Error = ((string)ViewBag.Error).Replace("Input string was not in a correct format.", _sharedLocalizer["ErrorInputStringWasNotInACorrectFormat"]);
                            }
                            if (((string)ViewBag.Error).Contains("String was not recognized as a valid DateTime"))
                            {
                                ViewBag.Error = ((string)ViewBag.Error).Replace("String was not recognized as a valid DateTime", _sharedLocalizer["ErrorInputStringWasNotInACorrectFormat"]);
                            }
                            break;
                        }
                        if ((DateOfSampling.Year < Constants.YearDataMin) || (DateOfSampling > DateTime.Now))
                        {
                            ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 1: " + String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["DateOfSampling"], (new DateTime(Constants.YearDataMin, 1, 1)).ToShortDateString(), DateTime.Now.ToShortDateString());
                            break;
                        }

                        try
                        {
                            DateOfAnalysis = Convert.ToDateTime(package.Workbook.Worksheets.FirstOrDefault().Cells[i, 2].Value);
                        }
                        catch (Exception e)
                        {
                            ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 2: " + e.Message + (e.InnerException == null ? "" : ": " + e.InnerException.Message);
                            if (((string)ViewBag.Error).Contains("Input string was not in a correct format."))
                            {
                                ViewBag.Error = ((string)ViewBag.Error).Replace("Input string was not in a correct format.", _sharedLocalizer["ErrorInputStringWasNotInACorrectFormat"]);
                            }
                            if (((string)ViewBag.Error).Contains("String was not recognized as a valid DateTime"))
                            {
                                ViewBag.Error = ((string)ViewBag.Error).Replace("String was not recognized as a valid DateTime", _sharedLocalizer["ErrorInputStringWasNotInACorrectFormat"]);
                            }
                            break;
                        }
                        if ((DateOfAnalysis.Year < Constants.YearDataMin) || (DateOfAnalysis > DateTime.Now))
                        {
                            ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 2: " + String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["DateOfAnalysis"], (new DateTime(Constants.YearDataMin, 1, 1)).ToShortDateString(), DateTime.Now.ToShortDateString());
                            break;
                        }

                        try
                        {
                            Value = Convert.ToInt32(package.Workbook.Worksheets.FirstOrDefault().Cells[i, 3].Value);
                        }
                        catch (Exception e)
                        {
                            ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 3: " + e.Message + (e.InnerException == null ? "" : ": " + e.InnerException.Message);
                            if (((string)ViewBag.Error).Contains("Input string was not in a correct format."))
                            {
                                ViewBag.Error = ((string)ViewBag.Error).Replace("Input string was not in a correct format.", _sharedLocalizer["ErrorInputStringWasNotInACorrectFormat"]);
                            }
                            break;
                        }
                        if ((Value < Constants.WaterSurfacePostDataValueMin) || (Value > Constants.WaterSurfacePostDataValueMax))
                        {
                            ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 3: " + String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["Value"], Constants.WaterSurfacePostDataValueMin.ToString(), Constants.WaterSurfacePostDataValueMax.ToString());
                            break;
                        }

                        waterSurfacePostDatas.Add(new WaterSurfacePostData()
                        {
                            WaterSurfacePostId = WaterSurfacePostId,
                            WaterContaminantId = WaterContaminantId,
                            DateOfSampling = DateOfSampling,
                            DateOfAnalysis = DateOfAnalysis,
                            Value = Value
                        });
                        _context.Add(waterSurfacePostDatas.LastOrDefault());
                    }
                    if (string.IsNullOrEmpty(ViewBag.Error))
                    {
                        _context.SaveChanges();
                        _context.Log.Add(new Log()
                        {
                            DateTime = DateTime.Now,
                            Email = User.Identity.Name,
                            Operation = "Upload",
                            Class = "WaterSurfacePostData",
                            New = String.Join("\r\n\r\n", waterSurfacePostDatas.Select(a => a.ToString()).ToArray()),
                            Old = ""
                        });
                        _context.SaveChanges();
                        ViewBag.Report = $"{_sharedLocalizer["Uploaded"]}: {waterSurfacePostDatas.Count()}";
                    }
                }
                foreach (FileInfo filed in di.GetFiles())
                {
                    try
                    {
                        filed.Delete();
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception e)
            {
                if (File != null)
                {
                    ViewBag.Error = e.Message + (e.InnerException == null ? "" : ": " + e.InnerException.Message);
                }
            }
            ViewData["WaterContaminantId"] = new SelectList(_context.WaterContaminant.OrderBy(w => w.Name), "Id", "Name", WaterContaminantId);
            var WaterObjects = _context.WaterObject.OrderBy(k => k.Name);
            ViewData["WaterObjectId"] = new SelectList(WaterObjects, "Id", "Name", WaterObjectId);
            ViewData["WaterSurfacePostId"] = new SelectList(_context.WaterSurfacePost.OrderBy(k => k.Number).Where(w => w.WaterObjectId == WaterObjects.FirstOrDefault().Id), "Id", "Number", WaterSurfacePostId);
            return View();
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult DeleteMany()
        {
            ViewData["WaterContaminantId"] = new SelectList(_context.WaterContaminant.OrderBy(w => w.Name), "Id", "Name");
            var WaterObjectId = _context.WaterObject.OrderBy(k => k.Name);
            ViewData["WaterObjectId"] = new SelectList(WaterObjectId, "Id", "Name");
            ViewData["WaterSurfacePostId"] = new SelectList(_context.WaterSurfacePost.OrderBy(k => k.Number).Where(w => w.WaterObjectId == WaterObjectId.FirstOrDefault().Id), "Id", "Number");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteMany(int WaterObjectId, int WaterSurfacePostId, int WaterContaminantId)
        {
            List<WaterSurfacePostData> waterSurfacePostDatas = _context.WaterSurfacePostData
                .Where(k => k.WaterSurfacePostId == WaterSurfacePostId &&  k.WaterContaminantId == WaterContaminantId)
                .ToList();
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Operation = "DeleteMany",
                Class = "WaterSurfacePostData",
                New = "",
                Old = String.Join("\r\n\r\n", waterSurfacePostDatas.Select(k => k.ToString()).ToArray())
            });
            ViewBag.Report = $"{_sharedLocalizer["Deleted"]}: {waterSurfacePostDatas.Count()}";
            _context.WaterSurfacePostData.RemoveRange(waterSurfacePostDatas);
            await _context.SaveChangesAsync();
            ViewData["WaterContaminantId"] = new SelectList(_context.WaterContaminant.OrderBy(w => w.Name), "Id", "Name", WaterContaminantId);
            var WaterObjects = _context.WaterObject.OrderBy(k => k.Name);
            ViewData["WaterObjectId"] = new SelectList(WaterObjects, "Id", "Name", WaterObjectId);
            ViewData["WaterSurfacePostId"] = new SelectList(_context.WaterSurfacePost.OrderBy(k => k.Number).Where(w => w.WaterObjectId == WaterObjects.FirstOrDefault().Id), "Id", "Number", WaterSurfacePostId);
            return View();
        }
    }
}
