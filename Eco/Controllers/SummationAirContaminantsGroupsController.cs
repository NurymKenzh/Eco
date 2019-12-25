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
    public class SummationAirContaminantsGroupsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public SummationAirContaminantsGroupsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: SummationAirContaminantsGroups
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            string SummationGroupCodeERA,
            string Number25012012,
            int? Page)
        {
            var summationAirContaminantsGroups = _context.SummationAirContaminantsGroup
                .Where(s => true);

            ViewBag.SummationGroupCodeERAFilter = SummationGroupCodeERA;
            ViewBag.Number25012012Filter = Number25012012;

            ViewBag.SummationGroupCodeERASort = SortOrder == "SummationGroupCodeERA" ? "SummationGroupCodeERADesc" : "SummationGroupCodeERA";
            ViewBag.Number25012012Sort = SortOrder == "Number25012012" ? "Number25012012Desc" : "Number25012012";
            ViewBag.CoefficientOfPotentiationSort = SortOrder == "CoefficientOfPotentiation" ? "CoefficientOfPotentiationDesc" : "CoefficientOfPotentiation";

            if (!string.IsNullOrEmpty(SummationGroupCodeERA))
            {
                summationAirContaminantsGroups = summationAirContaminantsGroups.Where(s => s.SummationGroupCodeERA.ToLower().Contains(SummationGroupCodeERA.ToLower()));
            }
            if (!string.IsNullOrEmpty(Number25012012))
            {
                summationAirContaminantsGroups = summationAirContaminantsGroups.Where(s => s.Number25012012.ToString().ToLower().Contains(Number25012012.ToLower()));
            }

            switch (SortOrder)
            {
                case "SummationGroupCodeERA":
                    summationAirContaminantsGroups = summationAirContaminantsGroups.OrderBy(s => s.SummationGroupCodeERA);
                    break;
                case "SummationGroupCodeERADesc":
                    summationAirContaminantsGroups = summationAirContaminantsGroups.OrderByDescending(s => s.SummationGroupCodeERA);
                    break;
                case "Number25012012":
                    summationAirContaminantsGroups = summationAirContaminantsGroups.OrderBy(s => s.Number25012012);
                    break;
                case "Number25012012Desc":
                    summationAirContaminantsGroups = summationAirContaminantsGroups.OrderByDescending(s => s.Number25012012);
                    break;
                case "CoefficientOfPotentiation":
                    summationAirContaminantsGroups = summationAirContaminantsGroups.OrderBy(s => s.CoefficientOfPotentiation);
                    break;
                case "CoefficientOfPotentiationDesc":
                    summationAirContaminantsGroups = summationAirContaminantsGroups.OrderByDescending(s => s.CoefficientOfPotentiation);
                    break;
                default:
                    summationAirContaminantsGroups = summationAirContaminantsGroups.OrderBy(s => s.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(summationAirContaminantsGroups.Count(), Page);

            var viewModel = new SummationAirContaminantsGroupIndexPageViewModel
            {
                Items = summationAirContaminantsGroups.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: SummationAirContaminantsGroups/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var summationAirContaminantsGroup = await _context.SummationAirContaminantsGroup
                .Include(a => a.SubstanceHazardClass)
                .Include(a => a.LimitingIndicator)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (summationAirContaminantsGroup == null)
            {
                return NotFound();
            }
            if (summationAirContaminantsGroup.AirContaminantsList == null)
            {
                summationAirContaminantsGroup.AirContaminantsList = new List<AirContaminant>();
            }
            foreach (int airContaminantId in summationAirContaminantsGroup.AirContaminants)
            {
                summationAirContaminantsGroup.AirContaminantsList.Add(_context.AirContaminant.FirstOrDefault(a => a.Id == airContaminantId));
            }
            return View(summationAirContaminantsGroup);
        }

        // GET: SummationAirContaminantsGroups/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            SummationAirContaminantsGroup summationAirContaminantsGroup = new SummationAirContaminantsGroup()
            {
                AirContaminantsList = new List<AirContaminant>()
            };
            ViewBag.AirContaminantId = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name");
            ViewData["SubstanceHazardClassId"] = new SelectList(_context.SubstanceHazardClass.OrderBy(s => s.Name), "Id", "Name");
            ViewData["LimitingIndicatorId"] = new SelectList(_context.LimitingIndicator.OrderBy(l => l.Name), "Id", "Name");
            return View(summationAirContaminantsGroup);
        }

        // POST: SummationAirContaminantsGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,SummationGroupCodeERA,Number25012012,CoefficientOfPotentiation,AirContaminants,AirContaminantsList,SubstanceHazardClassId,LimitingIndicatorId,PresenceOfTheMaximumPermissibleConcentration,MaximumPermissibleConcentrationOneTimemaximum,MaximumPermissibleConcentrationDailyAverage,ApproximateSafeExposureLevel")] SummationAirContaminantsGroup summationAirContaminantsGroup)
        {
            if(summationAirContaminantsGroup.AirContaminantsList==null)
            {
                summationAirContaminantsGroup.AirContaminantsList = new List<AirContaminant>();
            }
            if (summationAirContaminantsGroup.AirContaminantsList != null)
            {
                for (int i = summationAirContaminantsGroup.AirContaminantsList.Count - 1; i >= 0; i--)
                {
                    if (summationAirContaminantsGroup.AirContaminantsList[i].Id == 0)
                    {
                        summationAirContaminantsGroup.AirContaminantsList.RemoveAt(i);
                    }
                    else
                    {
                        summationAirContaminantsGroup.AirContaminantsList[i] = _context.AirContaminant.FirstOrDefault(a => a.Id == summationAirContaminantsGroup.AirContaminantsList[i].Id);
                    }
                }
            }
            var summationAirContaminantsGroups = _context.SummationAirContaminantsGroup.AsNoTracking().ToList();
            if (summationAirContaminantsGroups.Select(s => s.SummationGroupCodeERA).Contains(summationAirContaminantsGroup.SummationGroupCodeERA))
            {
                ModelState.AddModelError("SummationGroupCodeERA", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(summationAirContaminantsGroup.SummationGroupCodeERA))
            {
                ModelState.AddModelError("SummationGroupCodeERA", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (summationAirContaminantsGroup.SummationGroupCodeERA != null)
            {
                summationAirContaminantsGroup.SummationGroupCodeERA = summationAirContaminantsGroup.SummationGroupCodeERA.PadLeft(2, '0');
            }
            if (summationAirContaminantsGroup.PresenceOfTheMaximumPermissibleConcentration)
            {
                if (summationAirContaminantsGroup.MaximumPermissibleConcentrationOneTimemaximum == null)
                {
                    ModelState.AddModelError("MaximumPermissibleConcentrationOneTimemaximum", _sharedLocalizer["ErrorNeedToInput"]);
                }
                if (summationAirContaminantsGroup.MaximumPermissibleConcentrationDailyAverage == null)
                {
                    ModelState.AddModelError("MaximumPermissibleConcentrationDailyAverage", _sharedLocalizer["ErrorNeedToInput"]);
                }
            }
            else
            {
                if (summationAirContaminantsGroup.ApproximateSafeExposureLevel == null)
                {
                    ModelState.AddModelError("ApproximateSafeExposureLevel", _sharedLocalizer["ErrorNeedToInput"]);
                }
            }
            if (ModelState.IsValid)
            {
                if (!summationAirContaminantsGroup.PresenceOfTheMaximumPermissibleConcentration)
                {
                    summationAirContaminantsGroup.MaximumPermissibleConcentrationOneTimemaximum = null;
                    summationAirContaminantsGroup.MaximumPermissibleConcentrationDailyAverage = null;
                    summationAirContaminantsGroup.LimitingIndicatorId = null;
                }
                else
                {
                    summationAirContaminantsGroup.ApproximateSafeExposureLevel = null;
                }
                summationAirContaminantsGroup.AirContaminants = summationAirContaminantsGroup.AirContaminantsList.Select(a => a.Id).ToArray();
                _context.Add(summationAirContaminantsGroup);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "SummationAirContaminantsGroup",
                    New = summationAirContaminantsGroup.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.AirContaminantId = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name");
            return View(summationAirContaminantsGroup);
        }

        // GET: SummationAirContaminantsGroups/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var summationAirContaminantsGroup = await _context.SummationAirContaminantsGroup.SingleOrDefaultAsync(m => m.Id == id);
            if (summationAirContaminantsGroup == null)
            {
                return NotFound();
            }
            ViewBag.AirContaminantId = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name");
            if (summationAirContaminantsGroup.AirContaminantsList == null)
            {
                summationAirContaminantsGroup.AirContaminantsList = new List<AirContaminant>();
            }
            foreach (int airContaminantId in summationAirContaminantsGroup.AirContaminants)
            {
                summationAirContaminantsGroup.AirContaminantsList.Add(_context.AirContaminant.FirstOrDefault(a => a.Id == airContaminantId));
            }
            ViewBag.AirContaminantId = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name");
            ViewData["SubstanceHazardClassId"] = new SelectList(_context.SubstanceHazardClass.OrderBy(s => s.Name), "Id", "Name", summationAirContaminantsGroup.SubstanceHazardClassId);
            ViewData["LimitingIndicatorId"] = new SelectList(_context.LimitingIndicator.OrderBy(l => l.Name), "Id", "Name", summationAirContaminantsGroup.LimitingIndicatorId);
            return View(summationAirContaminantsGroup);
        }

        // POST: SummationAirContaminantsGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SummationGroupCodeERA,Number25012012,CoefficientOfPotentiation,AirContaminants,AirContaminantsList,SubstanceHazardClassId,LimitingIndicatorId,PresenceOfTheMaximumPermissibleConcentration,MaximumPermissibleConcentrationOneTimemaximum,MaximumPermissibleConcentrationDailyAverage,ApproximateSafeExposureLevel")] SummationAirContaminantsGroup summationAirContaminantsGroup)
        {
            if (id != summationAirContaminantsGroup.Id)
            {
                return NotFound();
            }
            if (summationAirContaminantsGroup.AirContaminantsList == null)
            {
                summationAirContaminantsGroup.AirContaminantsList = new List<AirContaminant>();
            }
            if (summationAirContaminantsGroup.AirContaminantsList != null)
            {
                for (int i = summationAirContaminantsGroup.AirContaminantsList.Count - 1; i >= 0; i--)
                {
                    if (summationAirContaminantsGroup.AirContaminantsList[i].Id == 0)
                    {
                        summationAirContaminantsGroup.AirContaminantsList.RemoveAt(i);
                    }
                    else
                    {
                        summationAirContaminantsGroup.AirContaminantsList[i] = _context.AirContaminant.FirstOrDefault(a => a.Id == summationAirContaminantsGroup.AirContaminantsList[i].Id);
                    }
                }
            }
            var summationAirContaminantsGroups = _context.SummationAirContaminantsGroup.AsNoTracking().Where(s => s.Id != summationAirContaminantsGroup.Id).ToList();
            if (summationAirContaminantsGroups.Select(s => s.SummationGroupCodeERA).Contains(summationAirContaminantsGroup.SummationGroupCodeERA))
            {
                ModelState.AddModelError("SummationGroupCodeERA", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(summationAirContaminantsGroup.SummationGroupCodeERA))
            {
                ModelState.AddModelError("SummationGroupCodeERA", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (summationAirContaminantsGroup.SummationGroupCodeERA != null)
            {
                summationAirContaminantsGroup.SummationGroupCodeERA = summationAirContaminantsGroup.SummationGroupCodeERA.PadLeft(2, '0');
            }
            if (summationAirContaminantsGroup.PresenceOfTheMaximumPermissibleConcentration)
            {
                if (summationAirContaminantsGroup.MaximumPermissibleConcentrationOneTimemaximum == null)
                {
                    ModelState.AddModelError("MaximumPermissibleConcentrationOneTimemaximum", _sharedLocalizer["ErrorNeedToInput"]);
                }
                if (summationAirContaminantsGroup.MaximumPermissibleConcentrationDailyAverage == null)
                {
                    ModelState.AddModelError("MaximumPermissibleConcentrationDailyAverage", _sharedLocalizer["ErrorNeedToInput"]);
                }
            }
            else
            {
                if (summationAirContaminantsGroup.ApproximateSafeExposureLevel == null)
                {
                    ModelState.AddModelError("ApproximateSafeExposureLevel", _sharedLocalizer["ErrorNeedToInput"]);
                }
            }
            if (ModelState.IsValid)
            {
                if (!summationAirContaminantsGroup.PresenceOfTheMaximumPermissibleConcentration)
                {
                    summationAirContaminantsGroup.MaximumPermissibleConcentrationOneTimemaximum = null;
                    summationAirContaminantsGroup.MaximumPermissibleConcentrationDailyAverage = null;
                    summationAirContaminantsGroup.LimitingIndicatorId = null;
                }
                else
                {
                    summationAirContaminantsGroup.ApproximateSafeExposureLevel = null;
                }
                summationAirContaminantsGroup.AirContaminants = summationAirContaminantsGroup.AirContaminantsList.Select(a => a.Id).ToArray();
                try
                {
                    var summationAirContaminantsGroup_old = _context.SummationAirContaminantsGroup.AsNoTracking().FirstOrDefault(s => s.Id == summationAirContaminantsGroup.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "SummationAirContaminantsGroup",
                        Operation = "Edit",
                        New = summationAirContaminantsGroup.ToString(),
                        Old = summationAirContaminantsGroup_old.ToString()
                    });
                    _context.Update(summationAirContaminantsGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SummationAirContaminantsGroupExists(summationAirContaminantsGroup.Id))
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
            ViewBag.AirContaminantId = new SelectList(_context.AirContaminant.OrderBy(a => a.Name), "Id", "Name");
            ViewData["SubstanceHazardClassId"] = new SelectList(_context.SubstanceHazardClass.OrderBy(s => s.Name), "Id", "Name", summationAirContaminantsGroup.SubstanceHazardClassId);
            ViewData["LimitingIndicatorId"] = new SelectList(_context.LimitingIndicator.OrderBy(l => l.Name), "Id", "Name", summationAirContaminantsGroup.LimitingIndicatorId);
            return View(summationAirContaminantsGroup);
        }

        // GET: SummationAirContaminantsGroups/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var summationAirContaminantsGroup = await _context.SummationAirContaminantsGroup
                .Include(a => a.SubstanceHazardClass)
                .Include(a => a.LimitingIndicator)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (summationAirContaminantsGroup == null)
            {
                return NotFound();
            }
            if (summationAirContaminantsGroup.AirContaminantsList == null)
            {
                summationAirContaminantsGroup.AirContaminantsList = new List<AirContaminant>();
            }
            foreach (int airContaminantId in summationAirContaminantsGroup.AirContaminants)
            {
                summationAirContaminantsGroup.AirContaminantsList.Add(_context.AirContaminant.FirstOrDefault(a => a.Id == airContaminantId));
            }
            return View(summationAirContaminantsGroup);
        }

        // POST: SummationAirContaminantsGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var summationAirContaminantsGroup = await _context.SummationAirContaminantsGroup.SingleOrDefaultAsync(m => m.Id == id);
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Class = "SummationAirContaminantsGroup",
                Operation = "Delete",
                New = "",
                Old = summationAirContaminantsGroup.ToString()
            });
            _context.SummationAirContaminantsGroup.Remove(summationAirContaminantsGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SummationAirContaminantsGroupExists(int id)
        {
            return _context.SummationAirContaminantsGroup.Any(e => e.Id == id);
        }
    }
}
