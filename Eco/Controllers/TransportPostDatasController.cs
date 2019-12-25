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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using OfficeOpenXml;

namespace Eco.Controllers
{
    public class TransportPostDatasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;
        private readonly IHostingEnvironment _hostingEnvironment;

        public TransportPostDatasController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer,
            IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: TransportPostDatas
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            string TransportPostName,
            int? Year,
            int? Month,
            int? Day,
            int? Page)
        {
            var transportPostDatas = _context.TransportPostData
                .Include(t => t.TransportPost)
                .Where(t => true);

            ViewBag.TransportPostNameFilter = TransportPostName;
            ViewBag.YearFilter = Year;
            ViewBag.MonthFilter = Month;
            ViewBag.DayFilter = Month;

            ViewBag.TransportPostNameSort = SortOrder == "TransportPostName" ? "TransportPostNameDesc" : "TransportPostName";
            ViewBag.YearSort = SortOrder == "Year" ? "YearDesc" : "Year";
            ViewBag.MonthSort = SortOrder == "Month" ? "MonthDesc" : "Month";
            ViewBag.DaySort = SortOrder == "Day" ? "DayDesc" : "Day";

            if (!string.IsNullOrEmpty(TransportPostName))
            {
                transportPostDatas = transportPostDatas.Where(k => k.TransportPost.Name.ToString().ToString().ToLower().Contains(TransportPostName.ToLower()));
            }
            if (Year != null)
            {
                transportPostDatas = transportPostDatas.Where(k => k.Year == Year);
            }
            if (Month != null)
            {
                transportPostDatas = transportPostDatas.Where(k => k.Month == Month);
            }
            if (Day != null)
            {
                transportPostDatas = transportPostDatas.Where(k => k.Day == Day);
            }

            switch (SortOrder)
            {
                case "TransportPostName":
                    transportPostDatas = transportPostDatas.OrderBy(k => k.TransportPost.Name);
                    break;
                case "TransportPostNameDesc":
                    transportPostDatas = transportPostDatas.OrderByDescending(k => k.TransportPost.Name);
                    break;
                case "Year":
                    transportPostDatas = transportPostDatas.OrderBy(k => k.Year);
                    break;
                case "YearDesc":
                    transportPostDatas = transportPostDatas.OrderByDescending(k => k.Year);
                    break;
                case "Month":
                    transportPostDatas = transportPostDatas.OrderBy(k => k.Month);
                    break;
                case "MonthDesc":
                    transportPostDatas = transportPostDatas.OrderByDescending(k => k.Month);
                    break;
                case "Day":
                    transportPostDatas = transportPostDatas.OrderBy(k => k.Day);
                    break;
                case "DayDesc":
                    transportPostDatas = transportPostDatas.OrderByDescending(k => k.Day);
                    break;
                default:
                    transportPostDatas = transportPostDatas.OrderBy(k => k.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(transportPostDatas.Count(), Page);

            var viewModel = new TransportPostDataIndexPageViewModel
            {
                Items = transportPostDatas.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            var years = _context.TransportPostData.Select(k => k.DateTime.Year).Distinct().ToList();
            ViewBag.Year = new SelectList(Enumerable.Range(Constants.YearMin, Constants.YearMax - Constants.YearMin + 1).Where(i => years.Contains(i)).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");
            ViewBag.Month = new SelectList(Enumerable.Range(1, 12).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");
            ViewBag.Day = new SelectList(Enumerable.Range(1, 31).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");

            return View(viewModel);
        }

        // GET: TransportPostDatas/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transportPostData = await _context.TransportPostData
                .Include(t => t.TransportPost)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (transportPostData == null)
            {
                return NotFound();
            }

            return View(transportPostData);
        }

        // GET: TransportPostDatas/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["TransportPostId"] = new SelectList(_context.TransportPost.OrderBy(t => t.Name), "Id", "Name");
            TransportPostData model = new TransportPostData();
            model.DateTime = DateTime.Now;
            model.Year = model.DateTime.Year;
            model.Month = model.DateTime.Month;
            model.Day = model.DateTime.Day;
            model.Hour = model.DateTime.Hour;
            model.Minute = model.DateTime.Minute;
            model.Second = model.DateTime.Second;
            return View(model);
        }

        // POST: TransportPostDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,TransportPostId,Year,Month,Day,Hour,Minute,Second,TheLengthOfTheInhibitorySignalSec,TotalNumberOfVehiclesIn20Minutes,RunningLengthm,AverageSpeedkmh")] TransportPostData transportPostData)
        {
            transportPostData.TransportPost = _context.TransportPost.FirstOrDefault(t => t.Id == transportPostData.TransportPostId);
            if ((transportPostData.TransportPost.Type)
                && (transportPostData.TheLengthOfTheInhibitorySignalSec == null))
            {
                ModelState.AddModelError("TheLengthOfTheInhibitorySignalSec", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if ((!transportPostData.TransportPost.Type)
                && (transportPostData.RunningLengthm == null))
            {
                ModelState.AddModelError("RunningLengthm", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if ((!transportPostData.TransportPost.Type)
                && (transportPostData.AverageSpeedkmh == null))
            {
                ModelState.AddModelError("AverageSpeedkmh", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                while (transportPostData.Day > 28)
                {
                    try
                    {
                        transportPostData.DateTime = new DateTime(
                            transportPostData.Year,
                            transportPostData.Month,
                            transportPostData.Day,
                            transportPostData.Hour,
                            transportPostData.Minute,
                            transportPostData.Second);
                    }
                    catch
                    {
                        transportPostData.Day--;
                    }
                }
                try
                {
                    transportPostData.DateTime = new DateTime(
                        transportPostData.Year,
                        transportPostData.Month,
                        transportPostData.Day,
                        transportPostData.Hour,
                        transportPostData.Minute,
                        transportPostData.Second);
                }
                catch
                {

                }
                _context.Add(transportPostData);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "TransportPostData",
                    New = transportPostData.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            try
            {
                transportPostData.Year = transportPostData.DateTime.Year;
                transportPostData.Month = transportPostData.DateTime.Month;
                transportPostData.Day = transportPostData.DateTime.Day;
                transportPostData.Hour = transportPostData.DateTime.Hour;
                transportPostData.Minute = transportPostData.DateTime.Minute;
                transportPostData.Second = transportPostData.DateTime.Second;
            }
            catch
            {

            }
            ViewData["TransportPostId"] = new SelectList(_context.TransportPost.OrderBy(t => t.Name), "Id", "Name", transportPostData.TransportPostId);
            return View(transportPostData);
        }

        // GET: TransportPostDatas/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transportPostData = await _context.TransportPostData.SingleOrDefaultAsync(m => m.Id == id);
            if (transportPostData == null)
            {
                return NotFound();
            }
            try
            {
                transportPostData.Year = transportPostData.DateTime.Year;
                transportPostData.Month = transportPostData.DateTime.Month;
                transportPostData.Day = transportPostData.DateTime.Day;
                transportPostData.Hour = transportPostData.DateTime.Hour;
                transportPostData.Minute = transportPostData.DateTime.Minute;
                transportPostData.Second = transportPostData.DateTime.Second;
            }
            catch
            {

            }
            ViewData["TransportPostId"] = new SelectList(_context.TransportPost.OrderBy(t => t.Name), "Id", "Name", transportPostData.TransportPostId);
            return View(transportPostData);
        }

        // POST: TransportPostDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TransportPostId,Year,Month,Day,Hour,Minute,Second,TheLengthOfTheInhibitorySignalSec,TotalNumberOfVehiclesIn20Minutes,RunningLengthm,AverageSpeedkmh")] TransportPostData transportPostData)
        {
            if (id != transportPostData.Id)
            {
                return NotFound();
            }
            transportPostData.TransportPost = _context.TransportPost.FirstOrDefault(t => t.Id == transportPostData.TransportPostId);
            if ((transportPostData.TransportPost.Type)
                && (transportPostData.TheLengthOfTheInhibitorySignalSec == null))
            {
                ModelState.AddModelError("TheLengthOfTheInhibitorySignalSec", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if ((!transportPostData.TransportPost.Type)
                && (transportPostData.RunningLengthm == null))
            {
                ModelState.AddModelError("RunningLengthm", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if ((!transportPostData.TransportPost.Type)
                && (transportPostData.AverageSpeedkmh == null))
            {
                ModelState.AddModelError("AverageSpeedkmh", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                while (transportPostData.Day > 28)
                {
                    try
                    {
                        transportPostData.DateTime = new DateTime(
                            transportPostData.Year,
                            transportPostData.Month,
                            transportPostData.Day,
                            transportPostData.Hour,
                            transportPostData.Minute,
                            transportPostData.Second);
                    }
                    catch
                    {
                        transportPostData.Day--;
                    }
                }
                try
                {
                    transportPostData.DateTime = new DateTime(
                        transportPostData.Year,
                        transportPostData.Month,
                        transportPostData.Day,
                        transportPostData.Hour,
                        transportPostData.Minute,
                        transportPostData.Second);
                }
                catch
                {

                }
                try
                {
                    var transportPostData_old = _context.TransportPostData.AsNoTracking().FirstOrDefault(k => k.Id == transportPostData.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "TransportPostData",
                        Operation = "Edit",
                        New = transportPostData.ToString(),
                        Old = transportPostData_old.ToString()
                    });
                    _context.Update(transportPostData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransportPostDataExists(transportPostData.Id))
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
                transportPostData.Year = transportPostData.DateTime.Year;
                transportPostData.Month = transportPostData.DateTime.Month;
                transportPostData.Day = transportPostData.DateTime.Day;
                transportPostData.Hour = transportPostData.DateTime.Hour;
                transportPostData.Minute = transportPostData.DateTime.Minute;
                transportPostData.Second = transportPostData.DateTime.Second;
            }
            catch
            {

            }
            ViewData["TransportPostId"] = new SelectList(_context.TransportPost.OrderBy(t => t.Name), "Id", "Name", transportPostData.TransportPostId);
            return View(transportPostData);
        }

        // GET: TransportPostDatas/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transportPostData = await _context.TransportPostData
                .Include(t => t.TransportPost)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (transportPostData == null)
            {
                return NotFound();
            }

            return View(transportPostData);
        }

        // POST: TransportPostDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transportPostData = await _context.TransportPostData.SingleOrDefaultAsync(m => m.Id == id);
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Class = "TransportPostData",
                Operation = "Delete",
                New = "",
                Old = transportPostData.ToString()
            });
            _context.TransportPostData.Remove(transportPostData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransportPostDataExists(int id)
        {
            return _context.TransportPostData.Any(e => e.Id == id);
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Upload()
        {
            ViewData["TransportPostId"] = new SelectList(_context.TransportPost.OrderBy(k => k.Name), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Upload(int TransportPostId, int AirContaminantId, bool FirstRowHeader, IFormFile File)
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
                    List<TransportPostData> transportPostDatas = new List<TransportPostData>();
                    for (int i = start_row; ; i++)
                    {
                        if (package.Workbook.Worksheets.FirstOrDefault().Cells[i, 1].Value == null)
                        {
                            break;
                        }
                        DateTime DateTime = new DateTime();
                        int? TheLengthOfTheInhibitorySignalSec = null;
                        int TotalNumberOfVehiclesIn20Minutes = 0;
                        int? RunningLengthm = null,
                            AverageSpeedkmh = null;

                        try
                        {
                            DateTime = Convert.ToDateTime(package.Workbook.Worksheets.FirstOrDefault().Cells[i, 1].Value);
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
                        if ((DateTime.Year < Constants.YearDataMin) || (DateTime.Year > Constants.YearMax))
                        {
                            ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 1: " + String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["Year"], Constants.YearDataMin.ToString(), Constants.YearMax.ToString());
                            break;
                        }

                        try
                        {
                            DateTime Time = Convert.ToDateTime(package.Workbook.Worksheets.FirstOrDefault().Cells[i, 2].Text);
                            DateTime = new DateTime(DateTime.Year, DateTime.Month, DateTime.Day, Time.Hour, Time.Minute, Time.Second);
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
                        if ((DateTime.Year < Constants.YearDataMin) || (DateTime.Year > Constants.YearMax))
                        {
                            ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 2: " + String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["Year"], Constants.YearDataMin.ToString(), Constants.YearMax.ToString());
                            break;
                        }

                        try
                        {
                            if (package.Workbook.Worksheets.FirstOrDefault().Cells[i, 3].Value != null)
                            {
                                TheLengthOfTheInhibitorySignalSec = Convert.ToInt32(package.Workbook.Worksheets.FirstOrDefault().Cells[i, 3].Value);
                            }
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
                        if(TheLengthOfTheInhibitorySignalSec!=null)
                        {
                            if ((TheLengthOfTheInhibitorySignalSec < Constants.TransportPostDataTheLengthOfTheInhibitorySignalSecMin) || (TheLengthOfTheInhibitorySignalSec > Constants.TransportPostDataTheLengthOfTheInhibitorySignalSecMax))
                            {
                                ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 3: " + String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["TheLengthOfTheInhibitorySignalSec"], Constants.TransportPostDataTheLengthOfTheInhibitorySignalSecMin.ToString(), Constants.TransportPostDataTheLengthOfTheInhibitorySignalSecMax.ToString());
                                break;
                            }
                        }

                        try
                        {
                            TotalNumberOfVehiclesIn20Minutes = Convert.ToInt32(package.Workbook.Worksheets.FirstOrDefault().Cells[i, 4].Value);
                        }
                        catch (Exception e)
                        {
                            ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 4: " + e.Message + (e.InnerException == null ? "" : ": " + e.InnerException.Message);
                            if (((string)ViewBag.Error).Contains("Input string was not in a correct format."))
                            {
                                ViewBag.Error = ((string)ViewBag.Error).Replace("Input string was not in a correct format.", _sharedLocalizer["ErrorInputStringWasNotInACorrectFormat"]);
                            }
                            break;
                        }
                        if ((TotalNumberOfVehiclesIn20Minutes < Constants.TransportPostDataTotalNumberOfVehiclesIn20MinutesMin) || (TotalNumberOfVehiclesIn20Minutes > Constants.TransportPostDataTotalNumberOfVehiclesIn20MinutesMax))
                        {
                            ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 4: " + String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["TotalNumberOfVehiclesIn20Minutes"], Constants.TransportPostDataTotalNumberOfVehiclesIn20MinutesMin.ToString(), Constants.TransportPostDataTotalNumberOfVehiclesIn20MinutesMax.ToString());
                            break;
                        }

                        try
                        {
                            if (package.Workbook.Worksheets.FirstOrDefault().Cells[i, 5].Value != null)
                            {
                                RunningLengthm = Convert.ToInt32(package.Workbook.Worksheets.FirstOrDefault().Cells[i, 5].Value);
                            }
                        }
                        catch (Exception e)
                        {
                            ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 5: " + e.Message + (e.InnerException == null ? "" : ": " + e.InnerException.Message);
                            if (((string)ViewBag.Error).Contains("Input string was not in a correct format."))
                            {
                                ViewBag.Error = ((string)ViewBag.Error).Replace("Input string was not in a correct format.", _sharedLocalizer["ErrorInputStringWasNotInACorrectFormat"]);
                            }
                            break;
                        }
                        if (RunningLengthm != null)
                        {
                            if ((RunningLengthm < Constants.TransportPostDataRunningLengthmMin) || (RunningLengthm > Constants.TransportPostDataRunningLengthmMax))
                            {
                                ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 5: " + String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["RunningLengthm"], Constants.TransportPostDataRunningLengthmMin.ToString(), Constants.TransportPostDataRunningLengthmMax.ToString());
                                break;
                            }
                        }

                        try
                        {
                            if (package.Workbook.Worksheets.FirstOrDefault().Cells[i, 6].Value != null)
                            {
                                AverageSpeedkmh = Convert.ToInt32(package.Workbook.Worksheets.FirstOrDefault().Cells[i, 6].Value);
                            }
                        }
                        catch (Exception e)
                        {
                            ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 6: " + e.Message + (e.InnerException == null ? "" : ": " + e.InnerException.Message);
                            if (((string)ViewBag.Error).Contains("Input string was not in a correct format."))
                            {
                                ViewBag.Error = ((string)ViewBag.Error).Replace("Input string was not in a correct format.", _sharedLocalizer["ErrorInputStringWasNotInACorrectFormat"]);
                            }
                            break;
                        }
                        if (AverageSpeedkmh != null)
                        {
                            if ((AverageSpeedkmh < Constants.TransportPostDataAverageSpeedkmhMin) || (AverageSpeedkmh > Constants.TransportPostDataAverageSpeedkmhMax))
                            {
                                ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 6: " + String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["AverageSpeedkmh"], Constants.TransportPostDataAverageSpeedkmhMin.ToString(), Constants.TransportPostDataAverageSpeedkmhMax.ToString());
                                break;
                            }
                        }

                        transportPostDatas.Add(new TransportPostData()
                        {
                            TransportPostId = TransportPostId,
                            DateTime = DateTime,
                            TheLengthOfTheInhibitorySignalSec = TheLengthOfTheInhibitorySignalSec,
                            TotalNumberOfVehiclesIn20Minutes = TotalNumberOfVehiclesIn20Minutes,
                            RunningLengthm = RunningLengthm,
                            AverageSpeedkmh = AverageSpeedkmh
                        });
                        _context.Add(transportPostDatas.LastOrDefault());
                    }
                    if (string.IsNullOrEmpty(ViewBag.Error))
                    {
                        _context.SaveChanges();
                        _context.Log.Add(new Log()
                        {
                            DateTime = DateTime.Now,
                            Email = User.Identity.Name,
                            Operation = "Upload",
                            Class = "TransportPostData",
                            New = String.Join("\r\n\r\n", transportPostDatas.Select(a => a.ToString()).ToArray()),
                            Old = ""
                        });
                        _context.SaveChanges();
                        ViewBag.Report = $"{_sharedLocalizer["Uploaded"]}: {transportPostDatas.Count()}";
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
            ViewData["TransportPostId"] = new SelectList(_context.TransportPost.OrderBy(k => k.Name), "Id", "Name", TransportPostId);
            return View();
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult DeleteMany()
        {
            ViewData["TransportPostId"] = new SelectList(_context.TransportPost.OrderBy(k => k.Name), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteMany(int TransportPostId)
        {
            List<TransportPostData> transportPostDatas = _context.TransportPostData
                .Where(k => k.TransportPostId == TransportPostId)
                .ToList();
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Operation = "DeleteMany",
                Class = "TransportPostData",
                New = "",
                Old = String.Join("\r\n\r\n", transportPostDatas.Select(k => k.ToString()).ToArray())
            });
            ViewBag.Report = $"{_sharedLocalizer["Deleted"]}: {transportPostDatas.Count()}";
            _context.TransportPostData.RemoveRange(transportPostDatas);
            await _context.SaveChangesAsync();
            ViewData["TransportPostId"] = new SelectList(_context.TransportPost.OrderBy(k => k.Name), "Id", "Name", TransportPostId);
            return View();
        }
    }
}
