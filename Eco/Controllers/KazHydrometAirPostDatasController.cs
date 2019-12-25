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
    public class KazHydrometAirPostDatasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;
        private readonly IHostingEnvironment _hostingEnvironment;

        public KazHydrometAirPostDatasController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer,
            IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: KazHydrometAirPostDatas
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            string KazHydrometAirPostNumber,
            string KazHydrometAirPostName,
            int? AirContaminantId,
            int? Year,
            int? Month,
            int? Page)
        {
            var kazHydrometAirPostDatas = _context.KazHydrometAirPostData
                .Include(k => k.KazHydrometAirPost)
                .Include(k => k.AirContaminant)
                .Where(k => true);
            
            ViewBag.KazHydrometAirPostNumberFilter = KazHydrometAirPostNumber;
            ViewBag.KazHydrometAirPostNameFilter = KazHydrometAirPostName;
            ViewBag.AirContaminantIdFilter = AirContaminantId;
            ViewBag.YearFilter = Year;
            ViewBag.MonthFilter = Month;

            ViewBag.KazHydrometAirPostNumberSort = SortOrder == "KazHydrometAirPostNumber" ? "KazHydrometAirPostNumberDesc" : "KazHydrometAirPostNumber";
            ViewBag.KazHydrometAirPostNameSort = SortOrder == "KazHydrometAirPostName" ? "KazHydrometAirPostNameDesc" : "KazHydrometAirPostName";
            ViewBag.AirContaminantNameSort = SortOrder == "AirContaminantName" ? "AirContaminantNameDesc" : "AirContaminantName";
            ViewBag.YearSort = SortOrder == "Year" ? "YearDesc" : "Year";
            ViewBag.MonthSort = SortOrder == "Month" ? "MonthDesc" : "Month";

            if (!string.IsNullOrEmpty(KazHydrometAirPostNumber))
            {
                kazHydrometAirPostDatas = kazHydrometAirPostDatas.Where(k => k.KazHydrometAirPost.Number.ToString()==KazHydrometAirPostNumber);
            }
            if (!string.IsNullOrEmpty(KazHydrometAirPostName))
            {
                kazHydrometAirPostDatas = kazHydrometAirPostDatas.Where(k => k.KazHydrometAirPost.Name==KazHydrometAirPostName);
            }
            if (AirContaminantId != null)
            {
                kazHydrometAirPostDatas = kazHydrometAirPostDatas.Where(k => k.AirContaminantId == AirContaminantId);
            }
            if (Year!=null)
            {
                kazHydrometAirPostDatas = kazHydrometAirPostDatas.Where(k => k.Year == Year);
            }
            if (Month != null)
            {
                kazHydrometAirPostDatas = kazHydrometAirPostDatas.Where(k => k.Month == Month);
            }

            switch (SortOrder)
            {
                case "KazHydrometAirPostNumber":
                    kazHydrometAirPostDatas = kazHydrometAirPostDatas.OrderBy(k => k.KazHydrometAirPost.Number);
                    break;
                case "KazHydrometAirPostNumberDesc":
                    kazHydrometAirPostDatas = kazHydrometAirPostDatas.OrderByDescending(k => k.KazHydrometAirPost.Number);
                    break;
                case "KazHydrometAirPostName":
                    kazHydrometAirPostDatas = kazHydrometAirPostDatas.OrderBy(k => k.KazHydrometAirPost.Name);
                    break;
                case "KazHydrometAirPostNameDesc":
                    kazHydrometAirPostDatas = kazHydrometAirPostDatas.OrderByDescending(k => k.KazHydrometAirPost.Name);
                    break;
                case "AirContaminantName":
                    kazHydrometAirPostDatas = kazHydrometAirPostDatas.OrderBy(k => k.AirContaminant.Name);
                    break;
                case "AirContaminantNameDesc":
                    kazHydrometAirPostDatas = kazHydrometAirPostDatas.OrderByDescending(k => k.AirContaminant.Name);
                    break;
                case "Year":
                    kazHydrometAirPostDatas = kazHydrometAirPostDatas.OrderBy(k => k.Year);
                    break;
                case "YearDesc":
                    kazHydrometAirPostDatas = kazHydrometAirPostDatas.OrderByDescending(k => k.Year);
                    break;
                case "Month":
                    kazHydrometAirPostDatas = kazHydrometAirPostDatas.OrderBy(k => k.Month);
                    break;
                case "MonthDesc":
                    kazHydrometAirPostDatas = kazHydrometAirPostDatas.OrderByDescending(k => k.Month);
                    break;
                default:
                    kazHydrometAirPostDatas = kazHydrometAirPostDatas.OrderBy(k => k.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(kazHydrometAirPostDatas.Count(), Page);

            var viewModel = new KazHydrometAirPostDataIndexPageViewModel
            {
                Items = kazHydrometAirPostDatas.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            var years = _context.KazHydrometAirPostData.Select(k => k.Year).Distinct().ToList();

            ViewBag.Year = new SelectList(Enumerable.Range(Constants.YearDataMin, Constants.YearMax - Constants.YearDataMin + 1).Where(i => years.Contains(i)).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");
            ViewBag.Month = new SelectList(Enumerable.Range(1, 12).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");

            ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.Where(a => _context.KazHydrometAirPostData.Include(k => k.AirContaminant).Select(k => k.AirContaminantId).Contains(a.Id)).OrderBy(a => a.Name), "Id", "Name");
            ViewData["KazHydrometAirPostNumber"] = new SelectList(_context.KazHydrometAirPost.OrderBy(a => a.Number).GroupBy(k => k.Number).Select(g => g.First()), "Number", "Number");
            ViewData["KazHydrometAirPostName"] = new SelectList(_context.KazHydrometAirPost.OrderBy(a => a.Name).GroupBy(k => k.Name).Select(g => g.First()), "Name", "Name");

            return View(viewModel);
        }

        // GET: KazHydrometAirPostDatas/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kazHydrometAirPostData = await _context.KazHydrometAirPostData
                .Include(k => k.AirContaminant)
                .Include(k => k.KazHydrometAirPost)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (kazHydrometAirPostData == null)
            {
                return NotFound();
            }

            return View(kazHydrometAirPostData);
        }

        // GET: KazHydrometAirPostDatas/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name");
            ViewData["KazHydrometAirPostId"] = new SelectList(_context.KazHydrometAirPost.OrderBy(k => k.Number), "Id", "Number");
            return View();
        }

        // POST: KazHydrometAirPostDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,KazHydrometAirPostId,AirContaminantId,Year,Month,PollutantConcentrationMonthlyAverage,PollutantConcentrationMaximumOneTimePerMonth,PollutantConcentrationMaximumOneTimePerMonthDay,PollutantConcentrationMaximumOneTimeMonth,PollutantConcentrationMaximumOneTimePerYear,PollutantConcentrationYearlyAverage")] KazHydrometAirPostData kazHydrometAirPostData)
        {
            if (_context.KazHydrometAirPostData.AsNoTracking().FirstOrDefault(k => k.KazHydrometAirPostId == kazHydrometAirPostData.KazHydrometAirPostId
                && k.AirContaminantId == kazHydrometAirPostData.AirContaminantId
                && k.Year == kazHydrometAirPostData.Year
                && k.Month == kazHydrometAirPostData.Month) != null)
            {
                ModelState.AddModelError("", $"{_sharedLocalizer["ErrorDublicateValue"]} " +
                    $"({_sharedLocalizer["KazHydrometAirPost"]}, " +
                    $"{_sharedLocalizer["AirContaminant"]}, " +
                    $"{_sharedLocalizer["Year"]}, " +
                    $"{_sharedLocalizer["Month"]})");
            }
            if (kazHydrometAirPostData.Year > DateTime.Today.Year || kazHydrometAirPostData.Year < Constants.YearDataMin)
            {
                ModelState.AddModelError("Year", String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["Year"], Constants.YearDataMin.ToString(), DateTime.Today.Year.ToString()));
            }
            if (ModelState.IsValid)
            {
                _context.Add(kazHydrometAirPostData);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "KazHydrometAirPostData",
                    New = kazHydrometAirPostData.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name", kazHydrometAirPostData.AirContaminantId);
            ViewData["KazHydrometAirPostId"] = new SelectList(_context.KazHydrometAirPost.OrderBy(k => k.Number), "Id", "Number", kazHydrometAirPostData.KazHydrometAirPostId);
            return View(kazHydrometAirPostData);
        }

        // GET: KazHydrometAirPostDatas/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kazHydrometAirPostData = await _context.KazHydrometAirPostData.SingleOrDefaultAsync(m => m.Id == id);
            if (kazHydrometAirPostData == null)
            {
                return NotFound();
            }
            ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name", kazHydrometAirPostData.AirContaminantId);
            ViewData["KazHydrometAirPostId"] = new SelectList(_context.KazHydrometAirPost.OrderBy(k => k.Number), "Id", "Number", kazHydrometAirPostData.KazHydrometAirPostId);
            return View(kazHydrometAirPostData);
        }

        // POST: KazHydrometAirPostDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KazHydrometAirPostId,AirContaminantId,Year,Month,PollutantConcentrationMonthlyAverage,PollutantConcentrationMaximumOneTimePerMonth,PollutantConcentrationMaximumOneTimePerMonthDay,PollutantConcentrationMaximumOneTimeMonth,PollutantConcentrationMaximumOneTimePerYear,PollutantConcentrationYearlyAverage")] KazHydrometAirPostData kazHydrometAirPostData)
        {
            if (id != kazHydrometAirPostData.Id)
            {
                return NotFound();
            }
            if (_context.KazHydrometAirPostData.AsNoTracking().FirstOrDefault(k => k.Id != id
                && k.KazHydrometAirPostId == kazHydrometAirPostData.KazHydrometAirPostId
                && k.AirContaminantId == kazHydrometAirPostData.AirContaminantId
                && k.Year == kazHydrometAirPostData.Year
                && k.Month == kazHydrometAirPostData.Month) != null)
            {
                ModelState.AddModelError("", $"{_sharedLocalizer["ErrorDublicateValue"]} " +
                    $"({_sharedLocalizer["KazHydrometAirPost"]}, " +
                    $"{_sharedLocalizer["AirContaminant"]}, " +
                    $"{_sharedLocalizer["Year"]}, " +
                    $"{_sharedLocalizer["Month"]})");
            }
            if (kazHydrometAirPostData.Year > DateTime.Today.Year || kazHydrometAirPostData.Year < Constants.YearDataMin)
            {
                ModelState.AddModelError("Year", String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["Year"], Constants.YearDataMin.ToString(), DateTime.Today.Year.ToString()));
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var kazHydrometAirPostData_old = _context.KazHydrometAirPostData.AsNoTracking().FirstOrDefault(k => k.Id == kazHydrometAirPostData.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "KazHydrometAirPostData",
                        Operation = "Edit",
                        New = kazHydrometAirPostData.ToString(),
                        Old = kazHydrometAirPostData_old.ToString()
                    });
                    _context.Update(kazHydrometAirPostData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KazHydrometAirPostDataExists(kazHydrometAirPostData.Id))
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
            ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name", kazHydrometAirPostData.AirContaminantId);
            ViewData["KazHydrometAirPostId"] = new SelectList(_context.KazHydrometAirPost.OrderBy(k => k.Number), "Id", "Number", kazHydrometAirPostData.KazHydrometAirPostId);
            return View(kazHydrometAirPostData);
        }

        // GET: KazHydrometAirPostDatas/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kazHydrometAirPostData = await _context.KazHydrometAirPostData
                .Include(k => k.AirContaminant)
                .Include(k => k.KazHydrometAirPost)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (kazHydrometAirPostData == null)
            {
                return NotFound();
            }

            return View(kazHydrometAirPostData);
        }

        // POST: KazHydrometAirPostDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kazHydrometAirPostData = await _context.KazHydrometAirPostData.SingleOrDefaultAsync(m => m.Id == id);
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Class = "KazHydrometAirPostData",
                Operation = "Delete",
                New = "",
                Old = kazHydrometAirPostData.ToString()
            });
            _context.KazHydrometAirPostData.Remove(kazHydrometAirPostData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KazHydrometAirPostDataExists(int id)
        {
            return _context.KazHydrometAirPostData.Any(e => e.Id == id);
        }

        //[Authorize(Roles = "Administrator, Moderator")]
        //public IActionResult Upload()
        //{
        //    ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name");
        //    ViewData["KazHydrometAirPostId"] = new SelectList(_context.KazHydrometAirPost.OrderBy(k => k.Number), "Id", "Number");
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Administrator, Moderator")]
        //public async Task<IActionResult> Upload(int KazHydrometAirPostId, int AirContaminantId, bool FirstRowHeader, IFormFile File)
        //{
        //    try
        //    {
        //        string sContentRootPath = _hostingEnvironment.WebRootPath;
        //        sContentRootPath = Path.Combine(sContentRootPath, "Upload");
        //        DirectoryInfo di = new DirectoryInfo(sContentRootPath);
        //        foreach (FileInfo filed in di.GetFiles())
        //        {
        //            try
        //            {
        //                filed.Delete();
        //            }
        //            catch
        //            {
        //            }
        //        }
        //        string path_filename = Path.Combine(sContentRootPath, Path.GetFileName(File.FileName));
        //        using (var stream = new FileStream(Path.GetFullPath(path_filename), FileMode.Create))
        //        {
        //            await File.CopyToAsync(stream);
        //        }
        //        FileInfo fileinfo = new FileInfo(Path.Combine(sContentRootPath, Path.GetFileName(path_filename)));
        //        using (ExcelPackage package = new ExcelPackage(fileinfo))
        //        {
        //            int start_row = 1;
        //            if(FirstRowHeader)
        //            {
        //                start_row++;
        //            }
        //            List<KazHydrometAirPostData> kazHydrometAirPostDatas = new List<KazHydrometAirPostData>();
        //            for (int i = start_row; ; i++)
        //            {
        //                if (package.Workbook.Worksheets.FirstOrDefault().Cells[i, 1].Value == null)
        //                {
        //                    break;
        //                }
        //                int Year = 0,
        //                    Month = 0;
        //                decimal PollutantConcentrationMonthlyAverage = 0,
        //                    PollutantConcentrationMaximumOneTimePerMonth = 0;
        //                int PollutantConcentrationMaximumOneTimePerMonthDay = 0;
        //                try
        //                {
        //                    Year = Convert.ToInt32(package.Workbook.Worksheets.FirstOrDefault().Cells[i, 1].Value);
        //                }
        //                catch(Exception e)
        //                {
        //                    ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 1: " + e.Message + (e.InnerException == null ? "" : ": " + e.InnerException.Message);
        //                    if(((string)ViewBag.Error).Contains("Input string was not in a correct format."))
        //                    {
        //                        ViewBag.Error = ((string)ViewBag.Error).Replace("Input string was not in a correct format.", _sharedLocalizer["ErrorInputStringWasNotInACorrectFormat"]);
        //                    }
        //                    break;
        //                }
        //                if((Year < Constants.YearDataMin) || (Year > Constants.YearMax))
        //                {
        //                    ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 1: " + String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["Year"], Constants.YearDataMin.ToString(), Constants.YearMax.ToString());
        //                    break;
        //                }
        //                try
        //                {
        //                    Month = Convert.ToInt32(package.Workbook.Worksheets.FirstOrDefault().Cells[i, 2].Value);
        //                }
        //                catch (Exception e)
        //                {
        //                    ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 2: " + e.Message + (e.InnerException == null ? "" : ": " + e.InnerException.Message);
        //                    if (((string)ViewBag.Error).Contains("Input string was not in a correct format."))
        //                    {
        //                        ViewBag.Error = ((string)ViewBag.Error).Replace("Input string was not in a correct format.", _sharedLocalizer["ErrorInputStringWasNotInACorrectFormat"]);
        //                    }
        //                    break;
        //                }
        //                if ((Month < 1) || (Month > 12))
        //                {
        //                    ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 2: " + String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["Month"], 1.ToString(), 12.ToString());
        //                    break;
        //                }
        //                try
        //                {
        //                    PollutantConcentrationMonthlyAverage = Convert.ToDecimal(package.Workbook.Worksheets.FirstOrDefault().Cells[i, 3].Value);
        //                }
        //                catch (Exception e)
        //                {
        //                    ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 3: " + e.Message + (e.InnerException == null ? "" : ": " + e.InnerException.Message);
        //                    if (((string)ViewBag.Error).Contains("Input string was not in a correct format."))
        //                    {
        //                        ViewBag.Error = ((string)ViewBag.Error).Replace("Input string was not in a correct format.", _sharedLocalizer["ErrorInputStringWasNotInACorrectFormat"]);
        //                    }
        //                    break;
        //                }
        //                if ((PollutantConcentrationMonthlyAverage < Constants.KazHydrometAirPostDataPollutantConcentrationMonthlyAverageMin) || (PollutantConcentrationMonthlyAverage > Constants.KazHydrometAirPostDataPollutantConcentrationMonthlyAverageMax))
        //                {
        //                    ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 3: " + String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["PollutantConcentrationMonthlyAverage"], Constants.KazHydrometAirPostDataPollutantConcentrationMonthlyAverageMin.ToString(), Constants.KazHydrometAirPostDataPollutantConcentrationMonthlyAverageMax.ToString());
        //                    break;
        //                }
        //                try
        //                {
        //                    PollutantConcentrationMaximumOneTimePerMonth = Convert.ToDecimal(package.Workbook.Worksheets.FirstOrDefault().Cells[i, 4].Value);
        //                }
        //                catch (Exception e)
        //                {
        //                    ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 4: " + e.Message + (e.InnerException == null ? "" : ": " + e.InnerException.Message);
        //                    if (((string)ViewBag.Error).Contains("Input string was not in a correct format."))
        //                    {
        //                        ViewBag.Error = ((string)ViewBag.Error).Replace("Input string was not in a correct format.", _sharedLocalizer["ErrorInputStringWasNotInACorrectFormat"]);
        //                    }
        //                    break;
        //                }
        //                if ((PollutantConcentrationMaximumOneTimePerMonth < Constants.KazHydrometAirPostDataPollutantConcentrationMaximumOneTimePerMonthMin) || (PollutantConcentrationMaximumOneTimePerMonth > Constants.KazHydrometAirPostDataPollutantConcentrationMaximumOneTimePerMonthMax))
        //                {
        //                    ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 4: " + String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["PollutantConcentrationMaximumOneTimePerMonth"], Constants.KazHydrometAirPostDataPollutantConcentrationMaximumOneTimePerMonthMin.ToString(), Constants.KazHydrometAirPostDataPollutantConcentrationMaximumOneTimePerMonthMax.ToString());
        //                    break;
        //                }
        //                try
        //                {
        //                    PollutantConcentrationMaximumOneTimePerMonthDay = Convert.ToInt32(package.Workbook.Worksheets.FirstOrDefault().Cells[i, 5].Value);
        //                }
        //                catch (Exception e)
        //                {
        //                    ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 5: " + e.Message + (e.InnerException == null ? "" : ": " + e.InnerException.Message);
        //                    if (((string)ViewBag.Error).Contains("Input string was not in a correct format."))
        //                    {
        //                        ViewBag.Error = ((string)ViewBag.Error).Replace("Input string was not in a correct format.", _sharedLocalizer["ErrorInputStringWasNotInACorrectFormat"]);
        //                    }
        //                    break;
        //                }
        //                if ((PollutantConcentrationMaximumOneTimePerMonthDay < Constants.DayMin) || (PollutantConcentrationMaximumOneTimePerMonthDay > Constants.DayMax))
        //                {
        //                    ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 5: " + String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["PollutantConcentrationMaximumOneTimePerMonthDay"], Constants.DayMin.ToString(), Constants.DayMax.ToString());
        //                    break;
        //                }
        //                if ((_context.KazHydrometAirPostData.AsNoTracking().FirstOrDefault(k => k.KazHydrometAirPostId == KazHydrometAirPostId
        //                    && k.AirContaminantId == AirContaminantId
        //                    && k.Year == Year
        //                    && k.Month == Month) != null)
        //                    || (kazHydrometAirPostDatas.FirstOrDefault(k => k.KazHydrometAirPostId == KazHydrometAirPostId
        //                    && k.AirContaminantId == AirContaminantId
        //                    && k.Year == Year
        //                    && k.Month == Month) != null))
        //                {
        //                    ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}: " + $"{_sharedLocalizer["ErrorDublicateValue"]} " +
        //                        $"({_sharedLocalizer["KazHydrometAirPost"]}, " +
        //                        $"{_sharedLocalizer["AirContaminant"]}, " +
        //                        $"{_sharedLocalizer["Year"]}, " +
        //                        $"{_sharedLocalizer["Month"]})";
        //                    break;
        //                }
        //                kazHydrometAirPostDatas.Add(new KazHydrometAirPostData()
        //                {
        //                    KazHydrometAirPostId = KazHydrometAirPostId,
        //                    AirContaminantId = AirContaminantId,
        //                    Year = Year,
        //                    Month = Month,
        //                    PollutantConcentrationMonthlyAverage = PollutantConcentrationMonthlyAverage,
        //                    PollutantConcentrationMaximumOneTimePerMonth = PollutantConcentrationMaximumOneTimePerMonth,
        //                    PollutantConcentrationMaximumOneTimePerMonthDay = PollutantConcentrationMaximumOneTimePerMonthDay
        //                });
        //                _context.Add(kazHydrometAirPostDatas.LastOrDefault());
        //            }
        //            if (string.IsNullOrEmpty(ViewBag.Error))
        //            {
        //                _context.SaveChanges();
        //                _context.Log.Add(new Log()
        //                {
        //                    DateTime = DateTime.Now,
        //                    Email = User.Identity.Name,
        //                    Operation = "Upload",
        //                    Class = "KazHydrometAirPostData",
        //                    New = String.Join("\r\n\r\n", kazHydrometAirPostDatas.Select(k => k.ToString()).ToArray()),
        //                    Old = ""
        //                });
        //                _context.SaveChanges();
        //                ViewBag.Report = $"{_sharedLocalizer["Uploaded"]}: {kazHydrometAirPostDatas.Count()}";
        //            }
        //        }
        //        foreach (FileInfo filed in di.GetFiles())
        //        {
        //            try
        //            {
        //                filed.Delete();
        //            }
        //            catch
        //            {
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        if(File != null)
        //        {
        //            ViewBag.Error = e.Message + (e.InnerException == null ? "" : ": " + e.InnerException.Message);
        //        }
        //    }
        //    ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name", AirContaminantId);
        //    ViewData["KazHydrometAirPostId"] = new SelectList(_context.KazHydrometAirPost.OrderBy(k => k.Number), "Id", "Number", KazHydrometAirPostId);
        //    return View();
        //}

        //[Authorize(Roles = "Administrator, Moderator")]
        //public IActionResult DeleteMany()
        //{
        //    ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name");
        //    ViewData["KazHydrometAirPostId"] = new SelectList(_context.KazHydrometAirPost.OrderBy(k => k.Number), "Id", "Number");
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Administrator, Moderator")]
        //public async Task<IActionResult> DeleteMany(int KazHydrometAirPostId, int AirContaminantId)
        //{
        //    List<KazHydrometAirPostData> kazHydrometAirPostDatas = _context.KazHydrometAirPostData
        //        .Where(k => k.KazHydrometAirPostId == KazHydrometAirPostId && k.AirContaminantId == AirContaminantId)
        //        .ToList();
        //    _context.Log.Add(new Log()
        //    {
        //        DateTime = DateTime.Now,
        //        Email = User.Identity.Name,
        //        Operation = "DeleteMany",
        //        Class = "KazHydrometAirPostData",
        //        New = "",
        //        Old = String.Join("\r\n\r\n", kazHydrometAirPostDatas.Select(k => k.ToString()).ToArray())
        //    });
        //    ViewBag.Report = $"{_sharedLocalizer["Deleted"]}: {kazHydrometAirPostDatas.Count()}";
        //    _context.KazHydrometAirPostData.RemoveRange(kazHydrometAirPostDatas);
        //    await _context.SaveChangesAsync();
        //    ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name", AirContaminantId);
        //    ViewData["KazHydrometAirPostId"] = new SelectList(_context.KazHydrometAirPost.OrderBy(k => k.Number), "Id", "Number", KazHydrometAirPostId);
        //    return View();
        //}
    }
}
