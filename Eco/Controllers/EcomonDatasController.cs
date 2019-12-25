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
using System.IO;

namespace Eco.Controllers
{
    public class EcomonDatasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;
        public string FileName = @"C:\Ecomon\Data.txt";
        public string LastDateTimeFileName = @"C:\Ecomon\DateTimeLast.txt";
        public string BlockFileName = @"C:\Ecomon\Block";

        public EcomonDatasController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: EcomonDatas
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(
            string SortOrder,
            string EcomonNumber,
            int? EcomonType,
            int? Page)
        {
            //new List<EcomonData>();

            using (System.IO.File.Create(BlockFileName)) { }
            string lines = System.IO.File.ReadAllText(FileName);
            foreach(string line in lines.Split(Environment.NewLine))
            {
                string[] lined = line.Split('\t');
                try
                {
                    if (_context.EcomonData.Count(e => e.timestamp_ms == Convert.ToInt64(lined[1]) && e.EcomonType == (EcomonType)Convert.ToInt32(lined[0])) == 0)
                    {
                        try
                        {
                            DateTime DateTime = new DateTime(1970, 1, 1, 6, 0, 0, 0);
                            _context.EcomonData.Add(new EcomonData()
                            {
                                EcomonType = (EcomonType)Convert.ToInt32(lined[0]),
                                timestamp_ms = Convert.ToInt64(lined[1]),
                                value = Convert.ToDecimal(lined[2].Replace('.', ',')),
                                DateTime = DateTime.AddSeconds(Convert.ToInt64(lined[1]) / 1000),
                                EcomonNumber = "11"
                            });
                            _context.SaveChanges();
                        }
                        catch
                        {
                            try
                            {
                                DateTime DateTime = new DateTime(1970, 1, 1, 6, 0, 0, 0);
                                _context.EcomonData.Add(new EcomonData()
                                {
                                    EcomonType = (EcomonType)Convert.ToInt32(lined[0]),
                                    timestamp_ms = Convert.ToInt64(lined[1]),
                                    value = Convert.ToDecimal(lined[2].Replace(',', '.')),
                                    DateTime = DateTime.AddSeconds(Convert.ToInt64(lined[1]) / 1000),
                                    EcomonNumber = "11"
                                });
                                _context.SaveChanges();
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                catch { }
            }
            using (System.IO.File.Create(FileName)) { }

            using (System.IO.File.Create(LastDateTimeFileName)) { }
            System.IO.File.AppendAllText(LastDateTimeFileName, (_context.EcomonData.Max(e => e.timestamp_ms) / 1000).ToString());

            System.IO.File.Delete(BlockFileName);
            List<EcomonData> ecomonDatas = _context.EcomonData.Where(e => true).ToList();

            ViewBag.EcomonNumberFilter = EcomonNumber;
            ViewBag.EcomonTypeFilter = EcomonType;

            ViewBag.EcomonNumberSort = SortOrder == "EcomonNumber" ? "EcomonNumberDesc" : "EcomonNumber";
            ViewBag.EcomonTypeSort = SortOrder == "EcomonType" ? "EcomonTypeDesc" : "EcomonType";
            ViewBag.DateTimeSort = SortOrder == "DateTime" ? "DateTimeDesc" : "DateTime";

            if (!string.IsNullOrEmpty(EcomonNumber))
            {
                ecomonDatas = ecomonDatas.Where(a => a.EcomonNumber.ToString() == EcomonNumber).ToList();
            }
            if (EcomonType!=null)
            {
                ecomonDatas = ecomonDatas.Where(a => a.EcomonType == (EcomonType)EcomonType).ToList();
            }

            switch (SortOrder)
            {
                case "EcomonNumber":
                    ecomonDatas = ecomonDatas.OrderBy(a => a.EcomonNumber).ToList();
                    break;
                case "EcomonNumberDesc":
                    ecomonDatas = ecomonDatas.OrderByDescending(a => a.EcomonNumber).ToList();
                    break;
                case "EcomonType":
                    ecomonDatas = ecomonDatas.OrderBy(a => a.EcomonType).ToList();
                    break;
                case "EcomonTypeDesc":
                    ecomonDatas = ecomonDatas.OrderByDescending(a => a.EcomonType).ToList();
                    break;
                case "DateTime":
                    ecomonDatas = ecomonDatas.OrderBy(a => a.DateTime).ToList();
                    break;
                case "DateTimeDesc":
                    ecomonDatas = ecomonDatas.OrderByDescending(a => a.DateTime).ToList();
                    break;
                default:
                    ecomonDatas = ecomonDatas.OrderByDescending(a => a.timestamp_ms).ToList();
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(ecomonDatas.Count(), Page);

            var viewModel = new EcomonDataIndexPageViewModel
            {
                Items = ecomonDatas.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewData["EcomonNumber"] = new List<SelectListItem>
            {
                new SelectListItem { Value = "11", Text = "11" },
            };

            var ecomonType = new List<SelectListItem>();
            ecomonType.Add(new SelectListItem() { Text = _sharedLocalizer["AtmosphericPressure"], Value = "116" });
            ecomonType.Add(new SelectListItem() { Text = _sharedLocalizer["PM10"], Value = "117" });
            ecomonType.Add(new SelectListItem() { Text = _sharedLocalizer["PM25"], Value = "118" });
            ecomonType.Add(new SelectListItem() { Text = _sharedLocalizer["AirTemperature"],Value = "120" });
            ecomonType.Add(new SelectListItem() { Text = _sharedLocalizer["WindSpeed"],Value = "121" });
            ecomonType.Add(new SelectListItem() { Text = _sharedLocalizer["WindDirection"],Value = "122" });
            ecomonType.Add(new SelectListItem() { Text = _sharedLocalizer["CO"],Value = "123 " });
            ecomonType.Add(new SelectListItem() { Text = _sharedLocalizer["NO2"],Value = "124" });
            ecomonType.Add(new SelectListItem() { Text = _sharedLocalizer["SO2"],Value = "125" });
            ecomonType.Add(new SelectListItem() { Text = _sharedLocalizer["Ozone"],Value = "126" });
            ViewData["EcomonType"] = ecomonType;

            return View(viewModel);
        }

        //// GET: EcomonDatas/Details/5
        //[Authorize(Roles = "Administrator, Moderator, Analyst")]
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var ecomonData = await _context.EcomonData
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (ecomonData == null)
        //    {
        //        return NotFound();
        //    }

        //    ecomonData.EcomonDataReport = new EcomonDataReport()
        //    {
        //        AirContaminantReports = new List<AirContaminantReport>()
        //    };
        //    foreach (var airContaminant in _context.AirContaminant.OrderBy(a => a.Name))
        //    {
        //        AirContaminantReport airContaminantReport = new AirContaminantReport()
        //        {
        //            Name = airContaminant.Name
        //        };
        //        var ecomonDataDatas = _context.EcomonDataData.Where(k => k.AirContaminantId == airContaminant.Id && k.EcomonDataId == id).Include(k => k.AirContaminant).ToList();
        //        if (ecomonDataDatas.Count() > 0)
        //        {
        //            EcomonDataData MaximumOneTimeValue = ecomonDataDatas.OrderByDescending(k => k.Value).FirstOrDefault();
        //            airContaminantReport.MaximumOneTimeValue = MaximumOneTimeValue.Value;
        //            airContaminantReport.MaximumOneTimeValueDate = MaximumOneTimeValue.DateTime;
        //            airContaminantReport.TheMaximumFrequencyOfExceedingTheMaximumPermissibleConcentrationIsMaximumOneTime = MaximumOneTimeValue.Value / airContaminant.MaximumPermissibleConcentrationOneTimemaximum;
        //            ecomonData.EcomonDataReport.AirContaminantReports.Add(airContaminantReport);
        //        }
        //    }

        //    return View(ecomonData);
        //}

        //// GET: EcomonDatas/Create
        //[Authorize(Roles = "Administrator, Moderator")]
        //public IActionResult Create()
        //{
        //    EcomonData model = new EcomonData();
        //    return View(model);
        //}

        //// POST: EcomonDatas/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Administrator, Moderator")]
        //public async Task<IActionResult> Create([Bind("Id,Number,NameKK,NameRU,PollutionSource,AdditionalInformationKK,AdditionalInformationRU,NorthLatitude,EastLongitude")] EcomonData ecomonData)
        //{
        //    var ecomonDatas = _context.EcomonData.AsNoTracking().ToList();
        //    if (ecomonDatas.Select(a => a.Number).Contains(ecomonData.Number))
        //    {
        //        ModelState.AddModelError("Number", _sharedLocalizer["ErrorDublicateValue"]);
        //    }
        //    if (ecomonDatas.Select(a => a.NameKK).Contains(ecomonData.NameKK))
        //    {
        //        ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
        //    }
        //    if (ecomonDatas.Select(a => a.NameRU).Contains(ecomonData.NameRU))
        //    {
        //        ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
        //    }
        //    if (string.IsNullOrWhiteSpace(ecomonData.NameKK))
        //    {
        //        ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
        //    }
        //    if (string.IsNullOrWhiteSpace(ecomonData.NameRU))
        //    {
        //        ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(ecomonData);
        //        await _context.SaveChangesAsync();
        //        _context.Log.Add(new Log()
        //        {
        //            DateTime = DateTime.Now,
        //            Email = User.Identity.Name,
        //            Operation = "Create",
        //            Class = "EcomonData",
        //            New = ecomonData.ToString(),
        //            Old = ""
        //        });
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(ecomonData);
        //}

        //// GET: EcomonDatas/Edit/5
        //[Authorize(Roles = "Administrator, Moderator")]
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var ecomonData = await _context.EcomonData.SingleOrDefaultAsync(m => m.Id == id);
        //    if (ecomonData == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(ecomonData);
        //}

        //// POST: EcomonDatas/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Administrator, Moderator")]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Number,NameKK,NameRU,PollutionSource,AdditionalInformationKK,AdditionalInformationRU,NorthLatitude,EastLongitude")] EcomonData ecomonData)
        //{
        //    if (id != ecomonData.Id)
        //    {
        //        return NotFound();
        //    }
        //    var ecomonDatas = _context.EcomonData.AsNoTracking().Where(a => a.Id != ecomonData.Id).ToList();
        //    if (ecomonDatas.Select(a => a.Number).Contains(ecomonData.Number))
        //    {
        //        ModelState.AddModelError("Number", _sharedLocalizer["ErrorDublicateValue"]);
        //    }
        //    if (ecomonDatas.Select(a => a.NameKK).Contains(ecomonData.NameKK))
        //    {
        //        ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
        //    }
        //    if (ecomonDatas.Select(a => a.NameRU).Contains(ecomonData.NameRU))
        //    {
        //        ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
        //    }
        //    if (string.IsNullOrWhiteSpace(ecomonData.NameKK))
        //    {
        //        ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
        //    }
        //    if (string.IsNullOrWhiteSpace(ecomonData.NameRU))
        //    {
        //        ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var ecomonData_old = _context.EcomonData.AsNoTracking().FirstOrDefault(a => a.Id == ecomonData.Id);
        //            _context.Log.Add(new Log()
        //            {
        //                DateTime = DateTime.Now,
        //                Email = User.Identity.Name,
        //                Class = "EcomonData",
        //                Operation = "Edit",
        //                New = ecomonData.ToString(),
        //                Old = ecomonData_old.ToString()
        //            });
        //            _context.Update(ecomonData);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!EcomonDataExists(ecomonData.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(ecomonData);
        //}

        //// GET: EcomonDatas/Delete/5
        //[Authorize(Roles = "Administrator, Moderator")]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var ecomonData = await _context.EcomonData
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (ecomonData == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewBag.EcomonDataDatas = _context.EcomonDataData
        //        .AsNoTracking()
        //        .Where(a => a.EcomonDataId == id)
        //        .Take(50)
        //        .OrderBy(a => a.DateTime);
        //    ViewBag.TargetTerritories = _context.TargetTerritory
        //        .AsNoTracking()
        //        .Where(k => k.EcomonDataId == id)
        //        .OrderBy(k => k.TerritoryName);
        //    return View(ecomonData);
        //}

        //// POST: EcomonDatas/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Administrator, Moderator")]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var ecomonData = await _context.EcomonData.SingleOrDefaultAsync(m => m.Id == id);
        //    if ((_context.EcomonDataData
        //        .AsNoTracking()
        //        .FirstOrDefault(a => a.EcomonDataId == id) == null)
        //        && (_context.TargetTerritory
        //        .AsNoTracking()
        //        .FirstOrDefault(k => k.EcomonDataId == id) == null))
        //    {
        //        _context.Log.Add(new Log()
        //        {
        //            DateTime = DateTime.Now,
        //            Email = User.Identity.Name,
        //            Class = "EcomonData",
        //            Operation = "Delete",
        //            New = "",
        //            Old = ecomonData.ToString()
        //        });
        //        _context.EcomonData.Remove(ecomonData);
        //        await _context.SaveChangesAsync();
        //    }
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool EcomonDataExists(int id)
        //{
        //    return _context.EcomonData.Any(e => e.Id == id);
        //}
    }
}
