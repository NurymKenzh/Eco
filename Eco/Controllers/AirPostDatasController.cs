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
    public class AirPostDatasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;
        private readonly IHostingEnvironment _hostingEnvironment;

        public AirPostDatasController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer,
            IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: AirPostDatas
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            string AirPostNumber,
            string AirPostName,
            int? AirContaminantId,
            int? Year,
            int? Month,
            int? Day,
            int? Page)
        {
            var airPostDatas = _context.AirPostData
                .Include(a => a.AirContaminant)
                .Include(a => a.AirPost)
                .Where(a => true);

            ViewBag.AirPostNumberFilter = AirPostNumber;
            ViewBag.AirPostNameFilter = AirPostName;
            ViewBag.AirContaminantIdFilter = AirContaminantId;
            ViewBag.YearFilter = Year;
            ViewBag.MonthFilter = Month;
            ViewBag.DayFilter = Month;

            ViewBag.AirPostNumberSort = SortOrder == "AirPostNumber" ? "AirPostNumberDesc" : "AirPostNumber";
            ViewBag.AirPostNameSort = SortOrder == "AirPostName" ? "AirPostNameDesc" : "AirPostName";
            ViewBag.AirContaminantNameSort = SortOrder == "AirContaminantName" ? "AirContaminantNameDesc" : "AirContaminantName";
            ViewBag.YearSort = SortOrder == "Year" ? "YearDesc" : "Year";
            ViewBag.MonthSort = SortOrder == "Month" ? "MonthDesc" : "Month";
            ViewBag.DaySort = SortOrder == "Day" ? "DayDesc" : "Day";
            
            if (!string.IsNullOrEmpty(AirPostNumber))
            {
                airPostDatas = airPostDatas.Where(k => k.AirPost.Number.ToString()==AirPostNumber);
            }
            if (!string.IsNullOrEmpty(AirPostName))
            {
                airPostDatas = airPostDatas.Where(k => k.AirPost.Name==AirPostName);
            }
            if (AirContaminantId != null)
            {
                airPostDatas = airPostDatas.Where(k => k.AirContaminantId == AirContaminantId);
            }
            if (Year != null)
            {
                airPostDatas = airPostDatas.Where(k => k.Year == Year);
            }
            if (Month != null)
            {
                airPostDatas = airPostDatas.Where(k => k.Month == Month);
            }
            if (Day != null)
            {
                airPostDatas = airPostDatas.Where(k => k.Day == Day);
            }

            switch (SortOrder)
            {
                case "AirPostNumber":
                    airPostDatas = airPostDatas.OrderBy(k => k.AirPost.Number);
                    break;
                case "AirPostNumberDesc":
                    airPostDatas = airPostDatas.OrderByDescending(k => k.AirPost.Number);
                    break;
                case "AirPostName":
                    airPostDatas = airPostDatas.OrderBy(k => k.AirPost.Name);
                    break;
                case "AirPostNameDesc":
                    airPostDatas = airPostDatas.OrderByDescending(k => k.AirPost.Name);
                    break;
                case "AirContaminantName":
                    airPostDatas = airPostDatas.OrderBy(k => k.AirContaminant.Name);
                    break;
                case "AirContaminantNameDesc":
                    airPostDatas = airPostDatas.OrderByDescending(k => k.AirContaminant.Name);
                    break;
                case "Year":
                    airPostDatas = airPostDatas.OrderBy(k => k.Year);
                    break;
                case "YearDesc":
                    airPostDatas = airPostDatas.OrderByDescending(k => k.Year);
                    break;
                case "Month":
                    airPostDatas = airPostDatas.OrderBy(k => k.Month);
                    break;
                case "MonthDesc":
                    airPostDatas = airPostDatas.OrderByDescending(k => k.Month);
                    break;
                case "Day":
                    airPostDatas = airPostDatas.OrderBy(k => k.Day);
                    break;
                case "DayDesc":
                    airPostDatas = airPostDatas.OrderByDescending(k => k.Day);
                    break;
                default:
                    airPostDatas = airPostDatas.OrderBy(k => k.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(airPostDatas.Count(), Page);

            var viewModel = new AirPostDataIndexPageViewModel
            {
                Items = airPostDatas.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            var years = _context.AirPostData.Select(k => k.DateTime.Year).Distinct().ToList();
            ViewBag.Year = new SelectList(Enumerable.Range(Constants.YearMin, Constants.YearMax - Constants.YearMin + 1).Where(i => years.Contains(i)).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");
            ViewBag.Month = new SelectList(Enumerable.Range(1, 12).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");
            ViewBag.Day = new SelectList(Enumerable.Range(1, 31).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");

            ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.Where(a => _context.AirPostData.Include(k => k.AirContaminant).Select(k => k.AirContaminantId).Contains(a.Id)).OrderBy(a => a.Name), "Id", "Name");
            ViewData["AirPostNumber"] = new SelectList(_context.AirPost.OrderBy(a => a.Number).GroupBy(k => k.Number).Select(g => g.First()), "Number", "Number");
            ViewData["AirPostName"] = new SelectList(_context.AirPost.OrderBy(a => a.Name).GroupBy(k => k.Name).Select(g => g.First()), "Name", "Name");

            return View(viewModel);
        }

        // GET: AirPostDatas/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airPostData = await _context.AirPostData
                .Include(a => a.AirContaminant)
                .Include(a => a.AirPost)
                .Include(a => a.GeneralWeatherCondition)
                .Include(a => a.WindDirection)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (airPostData == null)
            {
                return NotFound();
            }

            return View(airPostData);
        }

        // GET: AirPostDatas/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name");
            ViewData["AirPostId"] = new SelectList(_context.AirPost.OrderBy(a => a.Number), "Id", "Number");
            ViewData["GeneralWeatherConditionId"] = new SelectList(_context.GeneralWeatherCondition.OrderBy(g => g.Name), "Id", "Name");
            ViewData["WindDirectionId"] = new SelectList(_context.WindDirection.OrderBy(w => w.Name), "Id", "Name");
            AirPostData model = new AirPostData();
            model.DateTime = DateTime.Now;
            model.Year = model.DateTime.Year;
            model.Month = model.DateTime.Month;
            model.Day = model.DateTime.Day;
            model.Hour = model.DateTime.Hour;
            model.Minute = model.DateTime.Minute;
            model.Second = model.DateTime.Second;
            return View(model);
        }

        // POST: AirPostDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,AirPostId,AirContaminantId,Year,Month,Day,Hour,Minute,Second,TemperatureC,AtmosphericPressurekPa,Humidity,WindSpeedms,WindDirectionId,GeneralWeatherConditionId,Value")] AirPostData airPostData)
        {
            if (ModelState.IsValid)
            {
                while(airPostData.Day > 28)
                {
                    try
                    {
                        airPostData.DateTime = new DateTime(
                            airPostData.Year,
                            airPostData.Month,
                            airPostData.Day,
                            airPostData.Hour,
                            airPostData.Minute,
                            airPostData.Second);
                    }
                    catch
                    {
                        airPostData.Day--;
                    }
                }
                try
                {
                    airPostData.DateTime = new DateTime(
                        airPostData.Year,
                        airPostData.Month,
                        airPostData.Day,
                        airPostData.Hour,
                        airPostData.Minute,
                        airPostData.Second);
                }
                catch
                {

                }
                _context.Add(airPostData);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "AirPostData",
                    New = airPostData.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            try
            {
                airPostData.Year = airPostData.DateTime.Year;
                airPostData.Month = airPostData.DateTime.Month;
                airPostData.Day = airPostData.DateTime.Day;
                airPostData.Hour = airPostData.DateTime.Hour;
                airPostData.Minute = airPostData.DateTime.Minute;
                airPostData.Second = airPostData.DateTime.Second;
            }
            catch
            {

            }
            ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name", airPostData.AirContaminantId);
            ViewData["AirPostId"] = new SelectList(_context.AirPost.OrderBy(a => a.Number), "Id", "Number", airPostData.AirPostId);
            ViewData["GeneralWeatherConditionId"] = new SelectList(_context.GeneralWeatherCondition.OrderBy(g => g.Name), "Id", "Name", airPostData.GeneralWeatherConditionId);
            ViewData["WindDirectionId"] = new SelectList(_context.WindDirection.OrderBy(w => w.Name), "Id", "Name", airPostData.WindDirectionId);
            return View(airPostData);
        }

        // GET: AirPostDatas/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airPostData = await _context.AirPostData.SingleOrDefaultAsync(m => m.Id == id);
            if (airPostData == null)
            {
                return NotFound();
            }
            try
            {
                airPostData.Year = airPostData.DateTime.Year;
                airPostData.Month = airPostData.DateTime.Month;
                airPostData.Day = airPostData.DateTime.Day;
                airPostData.Hour = airPostData.DateTime.Hour;
                airPostData.Minute = airPostData.DateTime.Minute;
                airPostData.Second = airPostData.DateTime.Second;
            }
            catch
            {

            }
            ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name", airPostData.AirContaminantId);
            ViewData["AirPostId"] = new SelectList(_context.AirPost.OrderBy(a => a.Number), "Id", "Number", airPostData.AirPostId);
            ViewData["GeneralWeatherConditionId"] = new SelectList(_context.GeneralWeatherCondition.OrderBy(g => g.Name), "Id", "Name", airPostData.GeneralWeatherConditionId);
            ViewData["WindDirectionId"] = new SelectList(_context.WindDirection.OrderBy(w => w.Name), "Id", "Name", airPostData.WindDirectionId);
            return View(airPostData);
        }

        // POST: AirPostDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AirPostId,AirContaminantId,Year,Month,Day,Hour,Minute,Second,TemperatureC,AtmosphericPressurekPa,Humidity,WindSpeedms,WindDirectionId,GeneralWeatherConditionId,Value")] AirPostData airPostData)
        {
            if (id != airPostData.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                while (airPostData.Day > 28)
                {
                    try
                    {
                        airPostData.DateTime = new DateTime(
                            airPostData.Year,
                            airPostData.Month,
                            airPostData.Day,
                            airPostData.Hour,
                            airPostData.Minute,
                            airPostData.Second);
                    }
                    catch
                    {
                        airPostData.Day--;
                    }
                }
                try
                {
                    airPostData.DateTime = new DateTime(
                        airPostData.Year,
                        airPostData.Month,
                        airPostData.Day,
                        airPostData.Hour,
                        airPostData.Minute,
                        airPostData.Second);
                }
                catch
                {

                }
                try
                {
                    var airPostData_old = _context.AirPostData.AsNoTracking().FirstOrDefault(k => k.Id == airPostData.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "AirPostData",
                        Operation = "Edit",
                        New = airPostData.ToString(),
                        Old = airPostData_old.ToString()
                    });
                    _context.Update(airPostData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AirPostDataExists(airPostData.Id))
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
                airPostData.Year = airPostData.DateTime.Year;
                airPostData.Month = airPostData.DateTime.Month;
                airPostData.Day = airPostData.DateTime.Day;
                airPostData.Hour = airPostData.DateTime.Hour;
                airPostData.Minute = airPostData.DateTime.Minute;
                airPostData.Second = airPostData.DateTime.Second;
            }
            catch
            {

            }
            ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name", airPostData.AirContaminantId);
            ViewData["AirPostId"] = new SelectList(_context.AirPost.OrderBy(a => a.Number), "Id", "Number", airPostData.AirPostId);
            ViewData["GeneralWeatherConditionId"] = new SelectList(_context.GeneralWeatherCondition.OrderBy(g => g.Name), "Id", "Name", airPostData.GeneralWeatherConditionId);
            ViewData["WindDirectionId"] = new SelectList(_context.WindDirection.OrderBy(w => w.Name), "Id", "Name", airPostData.WindDirectionId);
            return View(airPostData);
        }

        // GET: AirPostDatas/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airPostData = await _context.AirPostData
                .Include(a => a.AirContaminant)
                .Include(a => a.AirPost)
                .Include(a => a.GeneralWeatherCondition)
                .Include(a => a.WindDirection)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (airPostData == null)
            {
                return NotFound();
            }

            return View(airPostData);
        }

        // POST: AirPostDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var airPostData = await _context.AirPostData.SingleOrDefaultAsync(m => m.Id == id);
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Class = "AirPostData",
                Operation = "Delete",
                New = "",
                Old = airPostData.ToString()
            });
            _context.AirPostData.Remove(airPostData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AirPostDataExists(int id)
        {
            return _context.AirPostData.Any(e => e.Id == id);
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Upload()
        {
            ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name");
            ViewData["AirPostId"] = new SelectList(_context.AirPost.OrderBy(k => k.Number), "Id", "Number");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Upload(int AirPostId, int AirContaminantId, bool FirstRowHeader, IFormFile File)
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
                    List<AirPostData> airPostDatas = new List<AirPostData>();
                    var windDirections = _context.WindDirection.ToList();
                    var generalWeatherConditions = _context.GeneralWeatherCondition.ToList();
                    for (int i = start_row; ; i++)
                    {
                        if (package.Workbook.Worksheets.FirstOrDefault().Cells[i, 1].Value == null)
                        {
                            break;
                        }
                        DateTime DateTime = new DateTime();
                        int WindDirectionId = 0,
                            GeneralWeatherConditionId = 0;
                        decimal TemperatureC = 0,
                            AtmosphericPressurekPa = 0,
                            WindSpeedms = 0,
                            Value = 0;
                        int Humidity = 0;

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
                            TemperatureC = Convert.ToDecimal(package.Workbook.Worksheets.FirstOrDefault().Cells[i, 3].Value);
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
                        if ((TemperatureC < Constants.AirPostDataTemperatureCMin) || (TemperatureC > Constants.AirPostDataTemperatureCMax))
                        {
                            ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 3: " + String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["TemperatureC"], Constants.AirPostDataTemperatureCMin.ToString(), Constants.AirPostDataTemperatureCMax.ToString());
                            break;
                        }

                        try
                        {
                            AtmosphericPressurekPa = Convert.ToDecimal(package.Workbook.Worksheets.FirstOrDefault().Cells[i, 4].Value);
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
                        if ((AtmosphericPressurekPa < Constants.AirPostDataAtmosphericPressurekPaMin) || (AtmosphericPressurekPa > Constants.AirPostDataAtmosphericPressurekPaMax))
                        {
                            ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 4: " + String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["AtmosphericPressurekPa"], Constants.AirPostDataAtmosphericPressurekPaMin.ToString(), Constants.AirPostDataAtmosphericPressurekPaMax.ToString());
                            break;
                        }

                        try
                        {
                            Humidity = Convert.ToInt32(package.Workbook.Worksheets.FirstOrDefault().Cells[i, 5].Value);
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
                        if ((Humidity < Constants.AirPostDataHumidityMin) || (Humidity > Constants.AirPostDataHumidityMax))
                        {
                            ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 5: " + String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["Humidity"], Constants.AirPostDataHumidityMin.ToString(), Constants.AirPostDataHumidityMax.ToString());
                            break;
                        }

                        try
                        {
                            WindSpeedms = Convert.ToDecimal(package.Workbook.Worksheets.FirstOrDefault().Cells[i, 6].Value);
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
                        if ((WindSpeedms < Constants.AirPostDataWindSpeedmsMin) || (WindSpeedms > Constants.AirPostDataWindSpeedmsMax))
                        {
                            ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 6: " + String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["WindSpeedms"], Constants.AirPostDataWindSpeedmsMin.ToString(), Constants.AirPostDataWindSpeedmsMax.ToString());
                            break;
                        }

                        WindDirection windDirection = windDirections.FirstOrDefault(w => w.Name.ToLower() == package.Workbook.Worksheets.FirstOrDefault().Cells[i, 7].Value.ToString().ToLower());
                        if(windDirection==null)
                        {
                            ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 7: " + _sharedLocalizer["ErrorValueNotFound"];
                            break;
                        }
                        else
                        {
                            WindDirectionId = windDirection.Id;
                        }

                        GeneralWeatherCondition generalWeatherCondition = generalWeatherConditions.FirstOrDefault(g => g.Name.ToLower() == package.Workbook.Worksheets.FirstOrDefault().Cells[i, 8].Value.ToString().ToLower());
                        if (generalWeatherCondition == null)
                        {
                            ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 8: " + _sharedLocalizer["ErrorValueNotFound"];
                            break;
                        }
                        else
                        {
                            GeneralWeatherConditionId = generalWeatherCondition.Id;
                        }

                        try
                        {
                            Value = Convert.ToDecimal(package.Workbook.Worksheets.FirstOrDefault().Cells[i, 9].Value);
                        }
                        catch (Exception e)
                        {
                            ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 9: " + e.Message + (e.InnerException == null ? "" : ": " + e.InnerException.Message);
                            if (((string)ViewBag.Error).Contains("Input string was not in a correct format."))
                            {
                                ViewBag.Error = ((string)ViewBag.Error).Replace("Input string was not in a correct format.", _sharedLocalizer["ErrorInputStringWasNotInACorrectFormat"]);
                            }
                            break;
                        }
                        if ((Value < Constants.AirPostDataValueMin) || (Value > Constants.AirPostDataValueMax))
                        {
                            ViewBag.Error = $"{_sharedLocalizer["Row"]} {i.ToString()}, {_sharedLocalizer["Column"]} 9: " + String.Format(_sharedLocalizer["ErrorNumberRangeMustBe"], _sharedLocalizer["Value"], Constants.AirPostDataValueMin.ToString(), Constants.AirPostDataValueMax.ToString());
                            break;
                        }

                        airPostDatas.Add(new AirPostData()
                        {
                            AirPostId = AirPostId,
                            AirContaminantId = AirContaminantId,
                            DateTime = DateTime,
                            TemperatureC = TemperatureC,
                            AtmosphericPressurekPa = AtmosphericPressurekPa,
                            Humidity = Humidity,
                            WindSpeedms = WindSpeedms,
                            WindDirectionId = WindDirectionId,
                            GeneralWeatherConditionId = GeneralWeatherConditionId,
                            Value = Value
                        });
                        _context.Add(airPostDatas.LastOrDefault());
                    }
                    if (string.IsNullOrEmpty(ViewBag.Error))
                    {
                        _context.SaveChanges();
                        _context.Log.Add(new Log()
                        {
                            DateTime = DateTime.Now,
                            Email = User.Identity.Name,
                            Operation = "Upload",
                            Class = "AirPostData",
                            New = String.Join("\r\n\r\n", airPostDatas.Select(a => a.ToString()).ToArray()),
                            Old = ""
                        });
                        _context.SaveChanges();
                        ViewBag.Report = $"{_sharedLocalizer["Uploaded"]}: {airPostDatas.Count()}";
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
            ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name", AirContaminantId);
            ViewData["AirPostId"] = new SelectList(_context.AirPost.OrderBy(k => k.Number), "Id", "Number", AirPostId);
            return View();
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult DeleteMany()
        {
            ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name");
            ViewData["AirPostId"] = new SelectList(_context.AirPost.OrderBy(k => k.Number), "Id", "Number");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteMany(int AirPostId, int AirContaminantId)
        {
            List<AirPostData> airPostDatas = _context.AirPostData
                .Where(k => k.AirPostId == AirPostId && k.AirContaminantId == AirContaminantId)
                .ToList();
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Operation = "DeleteMany",
                Class = "AirPostData",
                New = "",
                Old = String.Join("\r\n\r\n", airPostDatas.Select(k => k.ToString()).ToArray())
            });
            ViewBag.Report = $"{_sharedLocalizer["Deleted"]}: {airPostDatas.Count()}";
            _context.AirPostData.RemoveRange(airPostDatas);
            await _context.SaveChangesAsync();
            ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name", AirContaminantId);
            ViewData["AirPostId"] = new SelectList(_context.AirPost.OrderBy(k => k.Number), "Id", "Number", AirPostId);
            return View();
        }
    }
}
