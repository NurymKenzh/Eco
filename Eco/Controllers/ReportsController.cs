using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Eco.Data;
using Eco.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using OfficeOpenXml;

namespace Eco.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ReportsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer,
            IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
            _hostingEnvironment = hostingEnvironment;
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult TargetsReport()
        {
            ViewBag.TypeOfTargetId = new SelectList(_context.TypeOfTarget.OrderBy(a => a.Name), "Id", "Name");
            //ViewBag.TargetId = new SelectList(_context.Target.Where(a => a.TypeOfTargetId == _context.TypeOfTarget.OrderBy(tt => tt.Name).Select(tt => tt.Id).FirstOrDefault()).OrderBy(a => a.Name), "Id", "Name");
            ViewBag.TargetId = new SelectList(_context.Target.Where(a => false), "Id", "Name");
            ViewBag.MeasurementUnitId = new SelectList(_context.MeasurementUnit.OrderBy(m => m.Name), "Id", "Name", _context.Target.Where(a => a.TypeOfTargetId == _context.TypeOfTarget.OrderBy(tt => tt.Name).Select(tt => tt.Id).FirstOrDefault()).OrderBy(a => a.Name).FirstOrDefault().MeasurementUnitId);
            ViewBag.TerritoryTypeId = new SelectList(_context.TerritoryType.OrderBy(a => a.Name), "Id", "Name");
            ViewBag.TargetTerritoryId = new SelectList(_context.TargetTerritory.Where(a => a.TerritoryTypeId == _context.TerritoryType.OrderBy(tt => tt.Name).Select(tt => tt.Id).FirstOrDefault()).OrderBy(a => a.TerritoryName), "Id", "TerritoryName");
            ViewData["EventId"] = new SelectList(_context.Event, "Id", "Name");
            var years = _context.TargetValue.Select(k => k.Year).Distinct().ToList();
            ViewBag.Year = new SelectList(Enumerable.Range(Constants.YearMin, Constants.YearMax - Constants.YearMin + 1).Where(i => years.Contains(i)).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> TargetsReport(string[][] Report)
        {
            string sContentRootPath = _hostingEnvironment.WebRootPath;
            sContentRootPath = Path.Combine(sContentRootPath, "Download");
            sContentRootPath = Path.Combine(sContentRootPath, "Report");
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
            string sFileName = $"{_sharedLocalizer["Targets"]} {DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sContentRootPath, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sContentRootPath, sFileName));
            }
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(_sharedLocalizer["Targets"]);
                for(int i = 0; i < Report.Length; i++)
                {
                    for (int j = 0; j < Report[i].Length; j++)
                    {
                        decimal value;
                        bool number = decimal.TryParse(Report[i][j], out value);
                        if(number)
                        {
                            worksheet.Cells[i + 1, j + 1].Value = value;
                        }
                        else
                        {
                            worksheet.Cells[i + 1, j + 1].Value = Report[i][j];
                        }
                    }
                }
                for (int j = 0; j < Report[0].Length; j++)
                {
                    worksheet.Cells[1, j + 1].Style.Font.Bold = true;
                    worksheet.Column(j + 1).AutoFit();
                }

                package.Save();
            }
            var mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(sContentRootPath, file.Name));
            return File(fileBytes, mimeType, sFileName);
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult AirPostDatasReport()
        {
            ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.Where(a => _context.AirPostData.Include(k => k.AirContaminant).Select(k => k.AirContaminantId).Contains(a.Id)).OrderBy(a => a.Name), "Id", "Name");
            ViewData["AirPostId"] = new SelectList(_context.AirPost.OrderBy(a => a.Name), "Id", "Name");
            var years = _context.AirPostData.Select(k => k.DateTime.Year).Distinct().ToList();
            ViewBag.Year = new SelectList(Enumerable.Range(Constants.YearMin, Constants.YearMax - Constants.YearMin + 1).Where(i => years.Contains(i)).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");
            ViewBag.Month = new SelectList(Enumerable.Range(1, 12).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> AirPostDatasReport(string[][] Report)
        {
            string sContentRootPath = _hostingEnvironment.WebRootPath;
            sContentRootPath = Path.Combine(sContentRootPath, "Download");
            sContentRootPath = Path.Combine(sContentRootPath, "Report");
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
            string sFileName = $"{_sharedLocalizer["AirPostDatas"]} {DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sContentRootPath, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sContentRootPath, sFileName));
            }
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(_sharedLocalizer["AirPostDatas"]);
                for (int i = 0; i < Report.Length; i++)
                {
                    for (int j = 0; j < Report[i].Length; j++)
                    {
                        decimal value;
                        bool number = decimal.TryParse(Report[i][j], out value);
                        if (number)
                        {
                            worksheet.Cells[i + 1, j + 1].Value = value;
                        }
                        else
                        {
                            worksheet.Cells[i + 1, j + 1].Value = Report[i][j];
                        }
                    }
                }
                for (int j = 0; j < Report[0].Length; j++)
                {
                    worksheet.Cells[1, j + 1].Style.Font.Bold = true;
                    worksheet.Column(j + 1).AutoFit();
                }

                package.Save();
            }
            var mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(sContentRootPath, file.Name));
            return File(fileBytes, mimeType, sFileName);
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult KazHydrometAirPostDatasReport()
        {
            ViewData["AirContaminantId"] = new SelectList(_context.AirContaminant.Where(a => _context.KazHydrometAirPostData.Include(k => k.AirContaminant).Select(k => k.AirContaminantId).Contains(a.Id)).OrderBy(a => a.Name), "Id", "Name");
            ViewData["KazHydrometAirPostId"] = new SelectList(_context.KazHydrometAirPost.OrderBy(a => a.Number), "Id", "Number");
            var years = _context.KazHydrometAirPostData.Select(k => k.Year).Distinct().ToList();
            ViewBag.Year = new SelectList(Enumerable.Range(Constants.YearDataMin, Constants.YearMax - Constants.YearDataMin + 1).Where(i => years.Contains(i)).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");
            ViewBag.Month = new SelectList(Enumerable.Range(1, 12).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> KazHydrometAirPostDatasReport(string[][] Report)
        {
            string sContentRootPath = _hostingEnvironment.WebRootPath;
            sContentRootPath = Path.Combine(sContentRootPath, "Download");
            sContentRootPath = Path.Combine(sContentRootPath, "Report");
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
            string sFileName = $"{_sharedLocalizer["KazHydrometAirPostDatas"]} {DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sContentRootPath, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sContentRootPath, sFileName));
            }
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(_sharedLocalizer["KazHydrometAirPostDatas"]);
                for (int i = 0; i < Report.Length; i++)
                {
                    for (int j = 0; j < Report[i].Length; j++)
                    {
                        decimal value;
                        bool number = decimal.TryParse(Report[i][j], out value);
                        if (number)
                        {
                            worksheet.Cells[i + 1, j + 1].Value = value;
                        }
                        else
                        {
                            worksheet.Cells[i + 1, j + 1].Value = Report[i][j];
                        }
                    }
                }
                for (int j = 0; j < Report[0].Length; j++)
                {
                    worksheet.Cells[1, j + 1].Style.Font.Bold = true;
                    worksheet.Column(j + 1).AutoFit();
                }

                package.Save();
            }
            var mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(sContentRootPath, file.Name));
            return File(fileBytes, mimeType, sFileName);
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult TransportPostDatasReport()
        {
            ViewData["TransportPostId"] = new SelectList(_context.TransportPost.OrderBy(a => a.Name), "Id", "Name");
            var years = _context.TransportPostData.Select(k => k.DateTime.Year).Distinct().ToList();
            ViewBag.Year = new SelectList(Enumerable.Range(Constants.YearMin, Constants.YearMax - Constants.YearMin + 1).Where(i => years.Contains(i)).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");
            ViewBag.Month = new SelectList(Enumerable.Range(1, 12).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> TransportPostDatasReport(string[][] Report)
        {
            string sContentRootPath = _hostingEnvironment.WebRootPath;
            sContentRootPath = Path.Combine(sContentRootPath, "Download");
            sContentRootPath = Path.Combine(sContentRootPath, "Report");
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
            string sFileName = $"{_sharedLocalizer["TransportPostDatas"]} {DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sContentRootPath, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sContentRootPath, sFileName));
            }
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(_sharedLocalizer["TransportPostDatas"]);
                for (int i = 0; i < Report.Length; i++)
                {
                    for (int j = 0; j < Report[i].Length; j++)
                    {
                        decimal value;
                        bool number = decimal.TryParse(Report[i][j], out value);
                        if (number)
                        {
                            worksheet.Cells[i + 1, j + 1].Value = value;
                        }
                        else
                        {
                            worksheet.Cells[i + 1, j + 1].Value = Report[i][j];
                        }
                    }
                }
                for (int j = 0; j < Report[0].Length; j++)
                {
                    worksheet.Cells[1, j + 1].Style.Font.Bold = true;
                    worksheet.Column(j + 1).AutoFit();
                }

                package.Save();
            }
            var mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(sContentRootPath, file.Name));
            return File(fileBytes, mimeType, sFileName);
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult WaterSurfacePostDatasReport()
        {
            ViewData["WaterContaminantId"] = new SelectList(_context.WaterContaminant.Where(a => _context.WaterSurfacePostData.Include(k => k.WaterContaminant).Select(k => k.WaterContaminantId).Contains(a.Id)).OrderBy(a => a.Name), "Id", "Name");
            ViewData["WaterObjectId"] = new SelectList(_context.WaterObject.OrderBy(a => a.Name), "Id", "Name");
            var years = _context.WaterSurfacePostData.Select(k => k.DateOfSampling.Year).Distinct().ToList();
            ViewBag.Year = new SelectList(Enumerable.Range(Constants.YearMin, Constants.YearMax - Constants.YearMin + 1).Where(i => years.Contains(i)).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");
            ViewBag.Month = new SelectList(Enumerable.Range(1, 12).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> WaterSurfacePostDatasReport(string[][] Report)
        {
            string sContentRootPath = _hostingEnvironment.WebRootPath;
            sContentRootPath = Path.Combine(sContentRootPath, "Download");
            sContentRootPath = Path.Combine(sContentRootPath, "Report");
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
            string sFileName = $"{_sharedLocalizer["WaterSurfacePostDatas"]} {DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sContentRootPath, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sContentRootPath, sFileName));
            }
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(_sharedLocalizer["WaterSurfacePostDatas"]);
                for (int i = 0; i < Report.Length; i++)
                {
                    for (int j = 0; j < Report[i].Length; j++)
                    {
                        decimal value;
                        bool number = decimal.TryParse(Report[i][j], out value);
                        if (number)
                        {
                            worksheet.Cells[i + 1, j + 1].Value = value;
                        }
                        else
                        {
                            worksheet.Cells[i + 1, j + 1].Value = Report[i][j];
                        }
                    }
                }
                for (int j = 0; j < Report[0].Length; j++)
                {
                    worksheet.Cells[1, j + 1].Style.Font.Bold = true;
                    worksheet.Column(j + 1).AutoFit();
                }

                package.Save();
            }
            var mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(sContentRootPath, file.Name));
            return File(fileBytes, mimeType, sFileName);
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult KazHydrometWaterPostDatasReport()
        {
            ViewData["WaterContaminantId"] = new SelectList(_context.WaterContaminant.Where(a => _context.KazHydrometWaterPostData.Include(k => k.WaterContaminant).Select(k => k.WaterContaminantId).Contains(a.Id)).OrderBy(a => a.Name), "Id", "Name");
            ViewData["KazHydrometWaterPostId"] = new SelectList(_context.KazHydrometWaterPost.OrderBy(a => a.Name), "Id", "Name");
            var years = _context.KazHydrometWaterPostData.Select(k => k.Year).Distinct().ToList();
            ViewBag.Year = new SelectList(Enumerable.Range(Constants.YearDataMin, Constants.YearMax - Constants.YearDataMin + 1).Where(i => years.Contains(i)).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");
            ViewBag.Month = new SelectList(Enumerable.Range(1, 12).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> KazHydrometWaterPostDatasReport(string[][] Report)
        {
            string sContentRootPath = _hostingEnvironment.WebRootPath;
            sContentRootPath = Path.Combine(sContentRootPath, "Download");
            sContentRootPath = Path.Combine(sContentRootPath, "Report");
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
            string sFileName = $"{_sharedLocalizer["KazHydrometWaterPostDatasReport"]} {DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sContentRootPath, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sContentRootPath, sFileName));
            }
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(_sharedLocalizer["KazHydrometWaterPostDatasReport"]);
                for (int i = 0; i < Report.Length; i++)
                {
                    for (int j = 0; j < Report[i].Length; j++)
                    {
                        decimal value;
                        bool number = decimal.TryParse(Report[i][j], out value);
                        if (number)
                        {
                            worksheet.Cells[i + 1, j + 1].Value = value;
                        }
                        else
                        {
                            worksheet.Cells[i + 1, j + 1].Value = Report[i][j];
                        }
                    }
                }
                for (int j = 0; j < Report[0].Length; j++)
                {
                    worksheet.Cells[1, j + 1].Style.Font.Bold = true;
                    worksheet.Column(j + 1).AutoFit();
                }

                package.Save();
            }
            var mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(sContentRootPath, file.Name));
            return File(fileBytes, mimeType, sFileName);
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult KazHydrometSoilPostDatasReport()
        {
            ViewData["SoilContaminantId"] = new SelectList(_context.SoilContaminant.Where(a => _context.KazHydrometSoilPostData.Include(k => k.SoilContaminant).Select(k => k.SoilContaminantId).Contains(a.Id)).OrderBy(a => a.Name), "Id", "Name");
            ViewData["KazHydrometSoilPostId"] = new SelectList(_context.KazHydrometSoilPost.OrderBy(a => a.Name), "Id", "Name");
            var years = _context.KazHydrometSoilPostData.Select(k => k.Year).Distinct().ToList();
            ViewBag.Year = new SelectList(Enumerable.Range(Constants.YearDataMin, Constants.YearMax - Constants.YearDataMin + 1).Where(i => years.Contains(i)).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> KazHydrometSoilPostDatasReport(string[][] Report)
        {
            string sContentRootPath = _hostingEnvironment.WebRootPath;
            sContentRootPath = Path.Combine(sContentRootPath, "Download");
            sContentRootPath = Path.Combine(sContentRootPath, "Report");
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
            string sFileName = $"{_sharedLocalizer["KazHydrometSoilPostDatasReport"]} {DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sContentRootPath, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sContentRootPath, sFileName));
            }
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(_sharedLocalizer["KazHydrometSoilPostDatasReport"]);
                for (int i = 0; i < Report.Length; i++)
                {
                    for (int j = 0; j < Report[i].Length; j++)
                    {
                        decimal value;
                        bool number = decimal.TryParse(Report[i][j], out value);
                        if (number)
                        {
                            worksheet.Cells[i + 1, j + 1].Value = value;
                        }
                        else
                        {
                            worksheet.Cells[i + 1, j + 1].Value = Report[i][j];
                        }
                    }
                }
                for (int j = 0; j < Report[0].Length; j++)
                {
                    worksheet.Cells[1, j + 1].Style.Font.Bold = true;
                    worksheet.Column(j + 1).AutoFit();
                }

                package.Save();
            }
            var mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(sContentRootPath, file.Name));
            return File(fileBytes, mimeType, sFileName);
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult SoilPostDatasReport()
        {
            ViewData["SoilContaminantId"] = new SelectList(_context.SoilContaminant.Where(a => _context.SoilPostData.Include(k => k.SoilContaminant).Select(k => k.SoilContaminantId).Contains(a.Id)).OrderBy(a => a.Name), "Id", "Name");
            ViewData["SoilPostId"] = new SelectList(_context.SoilPost.OrderBy(a => a.Name), "Id", "Name");
            var years = _context.SoilPostData.Select(k => k.DateOfSampling.Year).Distinct().ToList();
            ViewBag.Year = new SelectList(Enumerable.Range(Constants.YearMin, Constants.YearMax - Constants.YearMin + 1).Where(i => years.Contains(i)).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");
            ViewBag.Month = new SelectList(Enumerable.Range(1, 12).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> SoilPostDatasReport(string[][] Report)
        {
            string sContentRootPath = _hostingEnvironment.WebRootPath;
            sContentRootPath = Path.Combine(sContentRootPath, "Download");
            sContentRootPath = Path.Combine(sContentRootPath, "Report");
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
            string sFileName = $"{_sharedLocalizer["SoilPostDatas"]} {DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sContentRootPath, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sContentRootPath, sFileName));
            }
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(_sharedLocalizer["SoilPostDatas"]);
                for (int i = 0; i < Report.Length; i++)
                {
                    for (int j = 0; j < Report[i].Length; j++)
                    {
                        decimal value;
                        bool number = decimal.TryParse(Report[i][j], out value);
                        if (number)
                        {
                            worksheet.Cells[i + 1, j + 1].Value = value;
                        }
                        else
                        {
                            worksheet.Cells[i + 1, j + 1].Value = Report[i][j];
                        }
                    }
                }
                for (int j = 0; j < Report[0].Length; j++)
                {
                    worksheet.Cells[1, j + 1].Style.Font.Bold = true;
                    worksheet.Column(j + 1).AutoFit();
                }

                package.Save();
            }
            var mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(sContentRootPath, file.Name));
            return File(fileBytes, mimeType, sFileName);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public JsonResult GetTargetsByTypeOfTargetId(int[] TypeOfTargetId)
        {
            var targets = _context.Target
                .Where(a => TypeOfTargetId.Contains(a.TypeOfTargetId)).ToArray().OrderBy(a => a.Name);
            JsonResult result = new JsonResult(targets);
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult GetTargetsReport(int[] TargetId, int[] Year)
        {
            List<List<string>> report = new List<List<string>>();
            List<string> line1 = new List<string>();
            line1.Add(_sharedLocalizer["TypeOfTarget"]);
            line1.Add(_sharedLocalizer["Target"]);
            line1.Add(_sharedLocalizer["MeasurementUnit"]);
            line1.Add(_sharedLocalizer["TerritoryType"]);
            line1.Add(_sharedLocalizer["TargetTerritory"]);
            foreach (int year in Year)
            {
                line1.Add($"{year.ToString()} {_sharedLocalizer["Planned"]}");
                line1.Add($"{year.ToString()} {_sharedLocalizer["Actual"]}");
            }
            report.Add(line1);
            foreach (Target target in _context.Target.Where(t => TargetId.Contains(t.Id)).OrderBy(t => t.TypeOfTarget.Name).ThenBy(t => t.Name).Include(t => t.TypeOfTarget).Include(t => t.MeasurementUnit))
            {
                foreach (TargetTerritory targetTerritory in _context.TargetTerritory.OrderBy(t => t.TerritoryType.Name).ThenBy(t => t.TerritoryName).Include(t => t.TerritoryType))
                {
                    var targetValues = _context.TargetValue.Where(t => t.TargetId == target.Id && t.TargetTerritoryId == targetTerritory.Id);
                    if (targetValues.Count() > 0)
                    {
                        List<string> line = new List<string>();
                        line.Add(target.TypeOfTarget.Name);
                        line.Add(target.Name);
                        line.Add(target.MeasurementUnit.Name);
                        line.Add(targetTerritory.TerritoryType.Name);
                        line.Add(targetTerritory.TerritoryName);
                        foreach (int year in Year)
                        {
                            line.Add((targetValues.FirstOrDefault(t => t.Year == year && !t.TargetValueType)?.Value).ToString());
                            line.Add((targetValues.FirstOrDefault(t => t.Year == year && t.TargetValueType)?.Value).ToString());
                        }
                        report.Add(line);
                    }
                }
            }
            string[,] reportarray = new string[report.Count, report[0].Count];
            for (int i = 0; i < report.Count; i++)
            {
                for (int j = 0; j < report[i].Count; j++)
                {
                    reportarray[i, j] = report[i][j];
                }
            }
            return Json(new
            {
                report = reportarray
            });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult GetAirPostDatasReport(int[] AirContaminantId, int[] AirPostId, int[] Year, int[] Month)
        {
            List<List<string>> report = new List<List<string>>();
            List<string> line1 = new List<string>();
            line1.Add(_sharedLocalizer["AirContaminant"]);
            line1.Add(_sharedLocalizer["AirPost"]);
            line1.Add(_sharedLocalizer["DateTime"]);
            line1.Add(_sharedLocalizer["TemperatureC"]);
            line1.Add(_sharedLocalizer["AtmosphericPressurekPa"]);
            line1.Add(_sharedLocalizer["Humidity"]);
            line1.Add(_sharedLocalizer["WindSpeedms"]);
            line1.Add(_sharedLocalizer["WindDirection"]);
            line1.Add(_sharedLocalizer["GeneralWeatherCondition"]);
            line1.Add(_sharedLocalizer["Value"]);
            report.Add(line1);
            var airPostDatas = _context.AirPostData
                .Where(a => AirContaminantId.Contains(a.AirContaminantId) && AirPostId.Contains(a.AirPostId) && Year.Contains(a.DateTime.Year) && Month.Contains(a.DateTime.Month))
                .Include(a => a.AirContaminant)
                .Include(a => a.AirPost)
                .Include(a => a.WindDirection)
                .Include(a => a.GeneralWeatherCondition)
                .OrderBy(a => a.AirContaminant.Name)
                .ThenBy(a => a.AirPost.Name)
                .ThenBy(a => a.DateTime);
            foreach(AirPostData airPostData in airPostDatas)
            {
                List<string> line = new List<string>();
                line.Add(airPostData.AirContaminant.Name);
                line.Add(airPostData.AirPost.Name);
                line.Add(airPostData.DateTime.ToString());
                line.Add(airPostData.TemperatureC.ToString());
                line.Add(airPostData.AtmosphericPressurekPa.ToString());
                line.Add(airPostData.Humidity.ToString());
                line.Add(airPostData.WindSpeedms.ToString());
                line.Add(airPostData.WindDirection.Name);
                line.Add(airPostData.GeneralWeatherCondition.Name);
                line.Add(airPostData.Value.ToString());
                report.Add(line);
            }
            string[,] reportarray = new string[report.Count, report[0].Count];
            for (int i = 0; i < report.Count; i++)
            {
                for (int j = 0; j < report[i].Count; j++)
                {
                    reportarray[i, j] = report[i][j];
                }
            }
            return Json(new
            {
                report = reportarray
            });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult GetKazHydrometAirPostDatasReport(int[] AirContaminantId, int[] KazHydrometAirPostId, int[] Year, int[] Month)
        {
            List<List<string>> report = new List<List<string>>();
            List<string> line1 = new List<string>();
            line1.Add(_sharedLocalizer["AirContaminant"]);
            line1.Add(_sharedLocalizer["KazHydrometAirPost"]);
            line1.Add(_sharedLocalizer["Year"]);
            line1.Add(_sharedLocalizer["Month"]);
            line1.Add(_sharedLocalizer["PollutantConcentrationMonthlyAverage"]);
            line1.Add(_sharedLocalizer["PollutantConcentrationMaximumOneTimePerMonth"]);
            line1.Add(_sharedLocalizer["PollutantConcentrationMaximumOneTimePerMonthDay"]);
            line1.Add(_sharedLocalizer["MaximumPermissibleConcentrationExcessMultiplicityDailyAverage"]);
            line1.Add(_sharedLocalizer["MaximumPermissibleConcentrationExcessMultiplicityMaximumOneTime"]);
            line1.Add(_sharedLocalizer["PollutantConcentrationMaximumOneTimeMonth"]);
            line1.Add(_sharedLocalizer["PollutantConcentrationMaximumOneTimePerYear"]);
            line1.Add(_sharedLocalizer["PollutantConcentrationYearlyAverage"]);
            report.Add(line1);
            var kazHydrometAirPostDatas = _context.KazHydrometAirPostData
                .Where(a => AirContaminantId.Contains(a.AirContaminantId) && KazHydrometAirPostId.Contains(a.KazHydrometAirPostId) && Year.Contains(a.Year)
                    && (Month.Contains((int)a.Month) || (Month.Length == 0 && a.Month == null)))
                .Include(a => a.AirContaminant)
                .Include(a => a.KazHydrometAirPost)
                .OrderBy(a => a.AirContaminant.Name)
                .ThenBy(a => a.KazHydrometAirPost.Name)
                .ThenBy(a => a.Year)
                .ThenBy(a => a.Month);
            foreach(KazHydrometAirPostData kazHydrometAirPostData in kazHydrometAirPostDatas)
            {
                List<string> line = new List<string>();
                line.Add(kazHydrometAirPostData.AirContaminant.Name);
                line.Add(kazHydrometAirPostData.KazHydrometAirPost.Number.ToString());
                line.Add(kazHydrometAirPostData.Year.ToString());
                line.Add(kazHydrometAirPostData.Month.ToString());
                line.Add(kazHydrometAirPostData.PollutantConcentrationMonthlyAverage.ToString());
                line.Add(kazHydrometAirPostData.PollutantConcentrationMaximumOneTimePerMonth.ToString());
                line.Add(kazHydrometAirPostData.PollutantConcentrationMaximumOneTimePerMonthDay.ToString());
                line.Add(kazHydrometAirPostData.MaximumPermissibleConcentrationExcessMultiplicityDailyAverage.ToString());
                line.Add(kazHydrometAirPostData.MaximumPermissibleConcentrationExcessMultiplicityMaximumOneTime.ToString());
                line.Add(kazHydrometAirPostData.PollutantConcentrationMaximumOneTimeMonth.ToString());
                line.Add(kazHydrometAirPostData.PollutantConcentrationMaximumOneTimePerYear.ToString());
                line.Add(kazHydrometAirPostData.PollutantConcentrationYearlyAverage.ToString());
                report.Add(line);
            }
            string[,] reportarray = new string[report.Count, report[0].Count];
            for (int i = 0; i < report.Count; i++)
            {
                for (int j = 0; j < report[i].Count; j++)
                {
                    reportarray[i, j] = report[i][j];
                }
            }
            return Json(new
            {
                report = reportarray
            });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult GetTransportPostDatasReport(int[] TransportPostId, int[] Year, int[] Month)
        {
            List<List<string>> report = new List<List<string>>();
            List<string> line1 = new List<string>();
            line1.Add(_sharedLocalizer["TransportPost"]);
            line1.Add(_sharedLocalizer["DateTime"]);
            line1.Add(_sharedLocalizer["TheLengthOfTheInhibitorySignalSec"]);
            line1.Add(_sharedLocalizer["TotalNumberOfVehiclesIn20Minutes"]);
            line1.Add(_sharedLocalizer["RunningLengthm"]);
            line1.Add(_sharedLocalizer["AverageSpeedkmh"]);
            line1.Add(_sharedLocalizer["CarsNumber"]);
            line1.Add(_sharedLocalizer["TrucksNumber"]);
            line1.Add(_sharedLocalizer["BusesDieselNumber"]);
            line1.Add(_sharedLocalizer["CarsGasolineNumber"]);
            line1.Add(_sharedLocalizer["CarsDieselNumber"]);
            line1.Add(_sharedLocalizer["TrucksGasolineNumber"]);
            line1.Add(_sharedLocalizer["TrucksDieselNumber"]);
            line1.Add(_sharedLocalizer["TrucksGasNumber"]);
            report.Add(line1);
            var transportPostDatas = _context.TransportPostData
                .Where(a => TransportPostId.Contains(a.TransportPostId) && Year.Contains(a.DateTime.Year) && Month.Contains(a.DateTime.Month))
                .Include(a => a.TransportPost)
                .OrderBy(a => a.TransportPost.Name)
                .ThenBy(a => a.DateTime);
            foreach(TransportPostData transportPostData in transportPostDatas)
            {
                List<string> line = new List<string>();
                line.Add(transportPostData.TransportPost.Name);
                line.Add(transportPostData.DateTime.ToString());
                line.Add(transportPostData.TheLengthOfTheInhibitorySignalSec.ToString());
                line.Add(transportPostData.TotalNumberOfVehiclesIn20Minutes.ToString());
                line.Add(transportPostData.RunningLengthm.ToString());
                line.Add(transportPostData.AverageSpeedkmh.ToString());
                line.Add(transportPostData.CarsNumber.ToString());
                line.Add(transportPostData.TrucksNumber.ToString());
                line.Add(transportPostData.BusesDieselNumber.ToString());
                line.Add(transportPostData.CarsGasolineNumber.ToString());
                line.Add(transportPostData.CarsDieselNumber.ToString());
                line.Add(transportPostData.TrucksGasolineNumber.ToString());
                line.Add(transportPostData.TrucksDieselNumber.ToString());
                line.Add(transportPostData.TrucksGasNumber.ToString());
                report.Add(line);
            }
            string[,] reportarray = new string[report.Count, report[0].Count];
            for (int i = 0; i < report.Count; i++)
            {
                for (int j = 0; j < report[i].Count; j++)
                {
                    reportarray[i, j] = report[i][j];
                }
            }
            return Json(new
            {
                report = reportarray
            });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult GetWaterSurfacePostDatasReport(int[] WaterContaminantId, int[] WaterObjectId, int[] Year, int[] Month)
        {
            List<List<string>> report = new List<List<string>>();
            List<string> line1 = new List<string>();
            line1.Add(_sharedLocalizer["WaterContaminant"]);
            line1.Add(_sharedLocalizer["WaterObject"]);
            line1.Add(_sharedLocalizer["WaterSurfacePost"]);
            line1.Add(_sharedLocalizer["DateOfSampling"]);
            line1.Add(_sharedLocalizer["DateOfAnalysis"]);
            line1.Add(_sharedLocalizer["Value"]);
            line1.Add(_sharedLocalizer["Class"]);
            report.Add(line1);
            var waterSurfacePostDatas = _context.WaterSurfacePostData
                .Include(a => a.WaterSurfacePost)
                .Where(a => WaterContaminantId.Contains(a.WaterContaminantId) && WaterObjectId.Contains(a.WaterSurfacePost.WaterObjectId) && Year.Contains(a.DateOfSampling.Year) && Month.Contains(a.DateOfSampling.Month))
                .Include(a => a.WaterSurfacePost.WaterObject)
                .Include(a => a.WaterContaminant)
                .OrderBy(a => a.WaterContaminant.Name)
                .OrderBy(a => a.WaterSurfacePost.WaterObject.Name)
                .ThenBy(a => a.DateOfSampling);
            foreach(WaterSurfacePostData waterSurfacePostData in waterSurfacePostDatas)
            {
                List<string> line = new List<string>();
                line.Add(waterSurfacePostData.WaterContaminant.Name);
                line.Add(waterSurfacePostData.WaterSurfacePost.WaterObject.Name);
                line.Add(waterSurfacePostData.WaterSurfacePost.Number.ToString());
                line.Add(waterSurfacePostData.DateOfSampling.ToString());
                line.Add(waterSurfacePostData.DateOfAnalysis.ToString());
                line.Add(waterSurfacePostData.Value.ToString());
                line.Add(waterSurfacePostData.Class.ToString());
                report.Add(line);
            }
            string[,] reportarray = new string[report.Count, report[0].Count];
            for (int i = 0; i < report.Count; i++)
            {
                for (int j = 0; j < report[i].Count; j++)
                {
                    reportarray[i, j] = report[i][j];
                }
            }
            return Json(new
            {
                report = reportarray
            });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult GetKazHydrometWaterPostDatasReport(int[] WaterContaminantId, int[] KazHydrometWaterPostId, int[] Year, int[] Month)
        {
            List<List<string>> report = new List<List<string>>();
            List<string> line1 = new List<string>();
            line1.Add(_sharedLocalizer["WaterContaminant"]);
            line1.Add(_sharedLocalizer["KazHydrometWaterPost"]);
            line1.Add(_sharedLocalizer["Year"]);
            line1.Add(_sharedLocalizer["Month"]);
            line1.Add(_sharedLocalizer["PollutantConcentrationmgl"]);
            report.Add(line1);
            var kazHydrometWaterPostDatas = _context.KazHydrometWaterPostData
                .Where(a => WaterContaminantId.Contains(a.WaterContaminantId) && KazHydrometWaterPostId.Contains(a.KazHydrometWaterPostId) && Year.Contains(a.Year) && Month.Contains(a.Month))
                .Include(a => a.WaterContaminant)
                .Include(a => a.KazHydrometWaterPost)
                .OrderBy(a => a.WaterContaminant.Name)
                .ThenBy(a => a.KazHydrometWaterPost.Name)
                .ThenBy(a => a.Year)
                .ThenBy(a => a.Month);
            foreach(KazHydrometWaterPostData kazHydrometWaterPostData in kazHydrometWaterPostDatas)
            {
                List<string> line = new List<string>();
                line.Add(kazHydrometWaterPostData.WaterContaminant.Name);
                line.Add(kazHydrometWaterPostData.KazHydrometWaterPost.Name);
                line.Add(kazHydrometWaterPostData.Year.ToString());
                line.Add(kazHydrometWaterPostData.Month.ToString());
                line.Add(kazHydrometWaterPostData.PollutantConcentrationmgl.ToString());
                report.Add(line);
            }
            string[,] reportarray = new string[report.Count, report[0].Count];
            for (int i = 0; i < report.Count; i++)
            {
                for (int j = 0; j < report[i].Count; j++)
                {
                    reportarray[i, j] = report[i][j];
                }
            }
            return Json(new
            {
                report = reportarray
            });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult GetKazHydrometSoilPostDatasReport(int[] SoilContaminantId, int[] KazHydrometSoilPostId, int[] Year)
        {
            List<List<string>> report = new List<List<string>>();
            List<string> line1 = new List<string>();
            line1.Add(_sharedLocalizer["SoilContaminant"]);
            line1.Add(_sharedLocalizer["KazHydrometSoilPost"]);
            line1.Add(_sharedLocalizer["Year"]);
            line1.Add(_sharedLocalizer["Season"]);
            line1.Add(_sharedLocalizer["ConcentrationValuemgkg"]);
            line1.Add(_sharedLocalizer["MultiplicityOfExcessOfTheMaximumPermissibleConcentration"]);
            report.Add(line1);
            var kazHydrometSoilPostDatas = _context.KazHydrometSoilPostData
                .Where(a => SoilContaminantId.Contains(a.SoilContaminantId) && KazHydrometSoilPostId.Contains(a.KazHydrometSoilPostId) && Year.Contains(a.Year))
                .Include(a => a.SoilContaminant)
                .Include(a => a.KazHydrometSoilPost)
                .OrderBy(a => a.SoilContaminant.Name)
                .ThenBy(a => a.KazHydrometSoilPost.Name)
                .ThenBy(a => a.Year);
            foreach(KazHydrometSoilPostData kazHydrometSoilPostData in kazHydrometSoilPostDatas)
            {
                List<string> line = new List<string>();
                line.Add(kazHydrometSoilPostData.SoilContaminant.Name);
                line.Add(kazHydrometSoilPostData.KazHydrometSoilPost.Name);
                line.Add(kazHydrometSoilPostData.Year.ToString());
                line.Add(kazHydrometSoilPostData.SeasonName);
                line.Add(kazHydrometSoilPostData.ConcentrationValuemgkg.ToString());
                line.Add(kazHydrometSoilPostData.MultiplicityOfExcessOfTheMaximumPermissibleConcentration.ToString());
                report.Add(line);
            }
            string[,] reportarray = new string[report.Count, report[0].Count];
            for (int i = 0; i < report.Count; i++)
            {
                for (int j = 0; j < report[i].Count; j++)
                {
                    reportarray[i, j] = report[i][j];
                }
            }
            return Json(new
            {
                report = reportarray
            });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult GetSoilPostDatasReport(int[] SoilContaminantId, int[] SoilPostId, int[] Year, int[] Month)
        {
            List<List<string>> report = new List<List<string>>();
            List<string> line1 = new List<string>();
            line1.Add(_sharedLocalizer["SoilContaminant"]);
            line1.Add(_sharedLocalizer["SoilPost"]);
            line1.Add(_sharedLocalizer["DateOfSampling"]);
            line1.Add(_sharedLocalizer["GammaBackgroundOfTheSoil"]);
            line1.Add(_sharedLocalizer["ConcentrationValuemgkg"]);
            line1.Add(_sharedLocalizer["MultiplicityOfExcessOfNormative"]);
            report.Add(line1);
            var soilPostDatas = _context.SoilPostData
                .Where(a => SoilContaminantId.Contains(a.SoilContaminantId) && SoilPostId.Contains(a.SoilPostId) && Year.Contains(a.DateOfSampling.Year) && Month.Contains(a.DateOfSampling.Month))
                .Include(a => a.SoilContaminant)
                .Include(a => a.SoilPost)
                .OrderBy(a => a.SoilContaminant.Name)
                .ThenBy(a => a.SoilPost.Name)
                .ThenBy(a => a.DateOfSampling);
            foreach(SoilPostData soilPostData in soilPostDatas)
            {
                List<string> line = new List<string>();
                line.Add(soilPostData.SoilContaminant.Name);
                line.Add(soilPostData.SoilPost.Name);
                line.Add(soilPostData.DateOfSampling.ToShortDateString());
                line.Add(soilPostData.GammaBackgroundOfTheSoil.ToString());
                line.Add(soilPostData.ConcentrationValuemgkg.ToString());
                line.Add(soilPostData.MultiplicityOfExcessOfNormative.ToString());
                report.Add(line);
            }
            string[,] reportarray = new string[report.Count, report[0].Count];
            for (int i = 0; i < report.Count; i++)
            {
                for (int j = 0; j < report[i].Count; j++)
                {
                    reportarray[i, j] = report[i][j];
                }
            }
            return Json(new
            {
                report = reportarray
            });
        }
    }
}