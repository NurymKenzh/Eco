using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eco.Data;
using Eco.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

namespace Eco.Controllers
{
    public class AirContaminantsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public AirContaminantsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: AirContaminants
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            int? SubstanceHazardClassId,
            string Name,
            string Number168,
            string Number104,
            //string ContaminantCodeERA,
            string NumberCAS,
            int? Page)
        {
            var airContaminants = _context.AirContaminant
                .Include(a => a.SubstanceHazardClass)
                .Where(a => true);

            ViewBag.SubstanceHazardClassIdFilter = SubstanceHazardClassId;
            ViewBag.NameFilter = Name;
            ViewBag.Number168Filter = Number168;
            ViewBag.Number104Filter = Number104;
            //ViewBag.ContaminantCodeERAFilter = ContaminantCodeERA;
            ViewBag.NumberCASFilter = NumberCAS;

            ViewBag.SubstanceHazardClassIdSort = SortOrder == "SubstanceHazardClassId" ? "SubstanceHazardClassIdDesc" : "SubstanceHazardClassId";
            ViewBag.NameSort = SortOrder == "Name" ? "NameDesc" : "Name";
            ViewBag.Number168Sort = SortOrder == "Number168" ? "Number168Desc" : "Number168";
            ViewBag.Number104Sort = SortOrder == "Number104" ? "Number104Desc" : "Number104";
            //ViewBag.ContaminantCodeERASort = SortOrder == "ContaminantCodeERA" ? "ContaminantCodeERADesc" : "ContaminantCodeERA";
            ViewBag.NumberCASSort = SortOrder == "NumberCAS" ? "NumberCASDesc" : "NumberCAS";

            if (SubstanceHazardClassId!=null)
            {
                airContaminants = airContaminants.Where(a => a.SubstanceHazardClassId == SubstanceHazardClassId);
            }
            if (!string.IsNullOrEmpty(Name))
            {
                airContaminants = airContaminants.Where(a => a.Name.ToLower().Contains(Name.ToLower()));
            }
            if (!string.IsNullOrEmpty(Number168))
            {
                airContaminants = airContaminants.Where(a => a.Number168.ToString().ToLower().Contains(Number168.ToLower()));
            }
            if (!string.IsNullOrEmpty(Number104))
            {
                airContaminants = airContaminants.Where(a => a.Number104.ToString().ToLower().Contains(Number104.ToLower()));
            }
            //if (!string.IsNullOrEmpty(ContaminantCodeERA))
            //{
            //    airContaminants = airContaminants.Where(a => a.ContaminantCodeERA.ToLower().Contains(ContaminantCodeERA.ToLower()));
            //}
            if (!string.IsNullOrEmpty(NumberCAS))
            {
                airContaminants = airContaminants.Where(a => a.NumberCAS.ToLower().Contains(NumberCAS.ToLower()));
            }

            switch (SortOrder)
            {
                case "SubstanceHazardClassId":
                    airContaminants = airContaminants.OrderBy(a => a.SubstanceHazardClass.Name);
                    break;
                case "SubstanceHazardClassIdDesc":
                    airContaminants = airContaminants.OrderByDescending(a => a.SubstanceHazardClass.Name);
                    break;
                case "Name":
                    airContaminants = airContaminants.OrderBy(a => a.Name);
                    break;
                case "NameDesc":
                    airContaminants = airContaminants.OrderByDescending(a => a.Name);
                    break;
                case "Number168":
                    airContaminants = airContaminants.OrderBy(a => a.Number168);
                    break;
                case "Number168Desc":
                    airContaminants = airContaminants.OrderByDescending(a => a.Number168);
                    break;
                case "Number104":
                    airContaminants = airContaminants.OrderBy(a => a.Number104);
                    break;
                case "Number104Desc":
                    airContaminants = airContaminants.OrderByDescending(a => a.Number104);
                    break;
                //case "ContaminantCodeERA":
                //    airContaminants = airContaminants.OrderBy(a => a.ContaminantCodeERA);
                //    break;
                //case "ContaminantCodeERADesc":
                //    airContaminants = airContaminants.OrderByDescending(a => a.ContaminantCodeERA);
                //    break;
                case "NumberCAS":
                    airContaminants = airContaminants.OrderBy(a => a.NumberCAS);
                    break;
                case "NumberCASDesc":
                    airContaminants = airContaminants.OrderByDescending(a => a.NumberCAS);
                    break;
                default:
                    airContaminants = airContaminants.OrderBy(a => a.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(airContaminants.Count(), Page);

            var viewModel = new AirContaminantIndexPageViewModel
            {
                Items = airContaminants.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewBag.SubstanceHazardClassId = new SelectList(_context.SubstanceHazardClass.OrderBy(s => s.Name), "Id", "Name");

            return View(viewModel);
        }

        // GET: AirContaminants/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airContaminant = await _context.AirContaminant
                .Include(a => a.SubstanceHazardClass)
                .Include(a => a.LimitingIndicator)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (airContaminant == null)
            {
                return NotFound();
            }

            return View(airContaminant);
        }

        // GET: AirContaminants/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            AirContaminant airContaminant = new AirContaminant()
            {
                //Synonyms = new string[0]
            };
            ViewData["SubstanceHazardClassId"] = new SelectList(_context.SubstanceHazardClass.OrderBy(s => s.Name), "Id", "Name");
            ViewData["LimitingIndicatorId"] = new SelectList(_context.LimitingIndicator.OrderBy(l => l.Name), "Id", "Name");
            return View(airContaminant);
        }

        // POST: AirContaminants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Name,Number168,Number104,NumberCAS,SubstanceHazardClassId,LimitingIndicatorId,PresenceOfTheMaximumPermissibleConcentration,MaximumPermissibleConcentrationOneTimemaximum,MaximumPermissibleConcentrationDailyAverage,ApproximateSafeExposureLevel,CoefficientOfSettlement")] AirContaminant airContaminant)
        {
            //if(airContaminant.Synonyms == null)
            //{
            //    airContaminant.Synonyms = new string[0];
            //}
            //if(airContaminant.Synonyms!=null)
            //{
            //    for (int i = airContaminant.Synonyms.Length - 1; i >= 0; i--)
            //    {
            //        if (string.IsNullOrEmpty(airContaminant.Synonyms[i]))
            //        {
            //            List<string> synonymsList = airContaminant.Synonyms.ToList();
            //            synonymsList.RemoveAt(i);
            //            airContaminant.Synonyms = synonymsList.ToArray();
            //        }
            //    }
            //}
            var airContaminants = _context.AirContaminant.AsNoTracking().ToList();
            if (airContaminants.Select(a => a.Name).Contains(airContaminant.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (airContaminants.Select(a => a.Number168).Contains(airContaminant.Number168))
            {
                ModelState.AddModelError("Number168", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (airContaminants.Select(a => a.Number104).Contains(airContaminant.Number104))
            {
                ModelState.AddModelError("Number104", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(airContaminant.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorNeedToInput"]);
            }
            //if (string.IsNullOrWhiteSpace(airContaminant.ContaminantCodeERA))
            //{
            //    ModelState.AddModelError("ContaminantCodeERA", _sharedLocalizer["ErrorNeedToInput"]);
            //}
            //if (airContaminant.PresenceOfTheMaximumPermissibleConcentration)
            //{
            //    if (airContaminant.MaximumPermissibleConcentrationOneTimemaximum == null)
            //    {
            //        ModelState.AddModelError("MaximumPermissibleConcentrationOneTimemaximum", _sharedLocalizer["ErrorNeedToInput"]);
            //    }
            //    if (airContaminant.MaximumPermissibleConcentrationDailyAverage == null)
            //    {
            //        ModelState.AddModelError("MaximumPermissibleConcentrationDailyAverage", _sharedLocalizer["ErrorNeedToInput"]);
            //    }
            //}
            //else
            //{
            //    if (airContaminant.ApproximateSafeExposureLevel == null)
            //    {
            //        ModelState.AddModelError("ApproximateSafeExposureLevel", _sharedLocalizer["ErrorNeedToInput"]);
            //    }
            //}
            //if (airContaminant.ContaminantCodeERA!=null)
            //{
            //    airContaminant.ContaminantCodeERA = airContaminant.ContaminantCodeERA.PadLeft(4, '0');
            //}
            if (ModelState.IsValid)
            {
                if (!airContaminant.PresenceOfTheMaximumPermissibleConcentration)
                {
                    airContaminant.MaximumPermissibleConcentrationOneTimemaximum = null;
                    airContaminant.MaximumPermissibleConcentrationDailyAverage = null;
                    airContaminant.LimitingIndicatorId = null;
                }
                else
                {
                    airContaminant.ApproximateSafeExposureLevel = null;
                }
                _context.Add(airContaminant);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "AirContaminant",
                    New = airContaminant.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubstanceHazardClassId"] = new SelectList(_context.SubstanceHazardClass.OrderBy(s => s.Name), "Id", "Name", airContaminant.SubstanceHazardClassId);
            ViewData["LimitingIndicatorId"] = new SelectList(_context.LimitingIndicator.OrderBy(l => l.Name), "Id", "Name", airContaminant.LimitingIndicatorId);
            return View(airContaminant);
        }

        // GET: AirContaminants/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airContaminant = await _context.AirContaminant.SingleOrDefaultAsync(m => m.Id == id);
            if (airContaminant == null)
            {
                return NotFound();
            }
            //if (airContaminant.Synonyms == null)
            //{
            //    airContaminant.Synonyms = new string[0];
            //}
            ViewData["SubstanceHazardClassId"] = new SelectList(_context.SubstanceHazardClass.OrderBy(s => s.Name), "Id", "Name", airContaminant.SubstanceHazardClassId);
            ViewData["LimitingIndicatorId"] = new SelectList(_context.LimitingIndicator.OrderBy(l => l.Name), "Id", "Name", airContaminant.LimitingIndicatorId);
            return View(airContaminant);
        }

        // POST: AirContaminants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Number168,Number104,NumberCAS,SubstanceHazardClassId,LimitingIndicatorId,PresenceOfTheMaximumPermissibleConcentration,MaximumPermissibleConcentrationOneTimemaximum,MaximumPermissibleConcentrationDailyAverage,ApproximateSafeExposureLevel,CoefficientOfSettlement")] AirContaminant airContaminant)
        {
            if (id != airContaminant.Id)
            {
                return NotFound();
            }
            //if (airContaminant.Synonyms == null)
            //{
            //    airContaminant.Synonyms = new string[0];
            //}
            //if (airContaminant.Synonyms != null)
            //{
            //    for (int i = airContaminant.Synonyms.Length - 1; i >= 0; i--)
            //    {
            //        if (string.IsNullOrEmpty(airContaminant.Synonyms[i]))
            //        {
            //            List<string> synonymsList = airContaminant.Synonyms.ToList();
            //            synonymsList.RemoveAt(i);
            //            airContaminant.Synonyms = synonymsList.ToArray();
            //        }
            //    }
            //}
            var airContaminants = _context.AirContaminant.AsNoTracking().Where(a => a.Id != airContaminant.Id).ToList();
            if (airContaminants.Select(a => a.Name).Contains(airContaminant.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (airContaminants.Select(a => a.Number168).Contains(airContaminant.Number168))
            {
                ModelState.AddModelError("Number168", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (airContaminants.Select(a => a.Number104).Contains(airContaminant.Number104))
            {
                ModelState.AddModelError("Number104", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(airContaminant.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorNeedToInput"]);
            }
            //if (string.IsNullOrWhiteSpace(airContaminant.ContaminantCodeERA))
            //{
            //    ModelState.AddModelError("ContaminantCodeERA", _sharedLocalizer["ErrorNeedToInput"]);
            //}
            //if (airContaminant.PresenceOfTheMaximumPermissibleConcentration)
            //{
            //    if (airContaminant.MaximumPermissibleConcentrationOneTimemaximum == null)
            //    {
            //        ModelState.AddModelError("MaximumPermissibleConcentrationOneTimemaximum", _sharedLocalizer["ErrorNeedToInput"]);
            //    }
            //    if (airContaminant.MaximumPermissibleConcentrationDailyAverage == null)
            //    {
            //        ModelState.AddModelError("MaximumPermissibleConcentrationDailyAverage", _sharedLocalizer["ErrorNeedToInput"]);
            //    }
            //}
            //else
            //{
            //    if (airContaminant.ApproximateSafeExposureLevel == null)
            //    {
            //        ModelState.AddModelError("ApproximateSafeExposureLevel", _sharedLocalizer["ErrorNeedToInput"]);
            //    }
            //}
            //if (airContaminant.ContaminantCodeERA != null)
            //{
            //    airContaminant.ContaminantCodeERA = airContaminant.ContaminantCodeERA.PadLeft(4, '0');
            //}
            if (ModelState.IsValid)
            {
                if (!airContaminant.PresenceOfTheMaximumPermissibleConcentration)
                {
                    airContaminant.MaximumPermissibleConcentrationOneTimemaximum = null;
                    airContaminant.MaximumPermissibleConcentrationDailyAverage = null;
                    airContaminant.LimitingIndicatorId = null;
                }
                else
                {
                    airContaminant.ApproximateSafeExposureLevel = null;
                }
                try
                {
                    var airContaminant_old = _context.AirContaminant.AsNoTracking().FirstOrDefault(a => a.Id == airContaminant.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "AirContaminant",
                        Operation = "Edit",
                        New = airContaminant.ToString(),
                        Old = airContaminant_old.ToString()
                    });
                    _context.Update(airContaminant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AirContaminantExists(airContaminant.Id))
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
            ViewData["SubstanceHazardClassId"] = new SelectList(_context.SubstanceHazardClass.OrderBy(s => s.Name), "Id", "Name", airContaminant.SubstanceHazardClassId);
            ViewData["LimitingIndicatorId"] = new SelectList(_context.LimitingIndicator.OrderBy(l => l.Name), "Id", "Name", airContaminant.LimitingIndicatorId);
            return View(airContaminant);
        }

        // GET: AirContaminants/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airContaminant = await _context.AirContaminant
                .Include(a => a.SubstanceHazardClass)
                .Include(a => a.LimitingIndicator)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (airContaminant == null)
            {
                return NotFound();
            }
            ViewBag.SummationAirContaminantsGroups = _context.SummationAirContaminantsGroup
                .AsNoTracking()
                .Where(s => s.AirContaminants.Contains(airContaminant.Id))
                .OrderBy(s => s.SummationGroupCodeERA);
            ViewBag.KazHydrometAirPostDatas = _context.KazHydrometAirPostData
                .AsNoTracking()
                .Where(k => k.KazHydrometAirPostId == id)
                .Take(50)
                .OrderBy(k => k.Year);
            ViewBag.AirPostDatas = _context.AirPostData
                .AsNoTracking()
                .Where(a => a.AirPostId == id)
                .Take(50)
                .OrderBy(a => a.DateTime.Year);
            return View(airContaminant);
        }

        // POST: AirContaminants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var airContaminant = await _context.AirContaminant.SingleOrDefaultAsync(m => m.Id == id);
            if((_context.SummationAirContaminantsGroup
                .AsNoTracking()
                .FirstOrDefault(s => s.AirContaminants.Contains(airContaminant.Id)) == null)
                && (_context.KazHydrometAirPostData
                .AsNoTracking()
                .FirstOrDefault(k => k.KazHydrometAirPostId == id) == null)
                && (_context.AirPostData
                .AsNoTracking()
                .FirstOrDefault(a => a.AirPostId == id) == null))
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "AirContaminant",
                    Operation = "Delete",
                    New = "",
                    Old = airContaminant.ToString()
                });
                _context.AirContaminant.Remove(airContaminant);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AirContaminantExists(int id)
        {
            return _context.AirContaminant.Any(e => e.Id == id);
        }
    }
}
