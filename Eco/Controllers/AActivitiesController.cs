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
    public class AActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public AActivitiesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: AActivities
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            int? TypeOfTargetId,
            int? TargetId,
            int? MeasurementUnitId,
            int? TerritoryTypeId,
            int? TargetTerritoryId,
            int? EventId,
            int? Year,
            bool? ActivityType,
            int? Page)
        {
            var aActivities = _context.AActivity
                .Include(a => a.Target)
                .Include(a => a.Target.MeasurementUnit)
                .Include(a => a.Target.TypeOfTarget)
                .Include(a => a.TargetTerritory)
                .Include(a => a.TargetTerritory.TerritoryType)
                .Where(a => true);

            ViewBag.TypeOfTargetIdFilter = TypeOfTargetId;
            ViewBag.TargetIdFilter = TargetId;
            ViewBag.MeasurementUnitIdFilter = MeasurementUnitId;
            ViewBag.TerritoryTypeIdFilter = TerritoryTypeId;
            ViewBag.TargetTerritoryIdFilter = TargetTerritoryId;
            ViewBag.EventIdFilter = EventId;
            ViewBag.YearFilter = Year;
            ViewBag.ActivityTypeFilter = ActivityType;

            ViewBag.TypeOfTargetIdSort = SortOrder == "TypeOfTargetId" ? "TypeOfTargetIdDesc" : "TypeOfTargetId";
            ViewBag.TargetIdSort = SortOrder == "TargetId" ? "TargetIdDesc" : "TargetId";
            ViewBag.MeasurementUnitIdSort = SortOrder == "MeasurementUnitId" ? "MeasurementUnitIdDesc" : "MeasurementUnitId";
            ViewBag.TerritoryTypeIdSort = SortOrder == "TerritoryTypeId" ? "TerritoryTypeIdDesc" : "TerritoryTypeId";
            ViewBag.TargetTerritoryIdSort = SortOrder == "TargetTerritoryId" ? "TargetTerritoryIdDesc" : "TargetTerritoryId";
            ViewBag.EventIdSort = SortOrder == "EventId" ? "EventIdDesc" : "EventId";
            ViewBag.YearSort = SortOrder == "Year" ? "YearDesc" : "Year";
            ViewBag.ActivityTypeSort = SortOrder == "ActivityType" ? "ActivityTypeDesc" : "ActivityType";

            if (TypeOfTargetId != null)
            {
                aActivities = aActivities.Where(a => a.Target.TypeOfTargetId == TypeOfTargetId);
            }
            if (TargetId != null)
            {
                aActivities = aActivities.Where(a => a.TargetId == TargetId);
            }
            if (MeasurementUnitId != null)
            {
                aActivities = aActivities.Where(a => a.Target.MeasurementUnitId == MeasurementUnitId);
            }
            if (TerritoryTypeId != null)
            {
                aActivities = aActivities.Where(a => a.TargetTerritory.TerritoryTypeId == TerritoryTypeId);
            }
            if (TargetTerritoryId != null)
            {
                aActivities = aActivities.Where(a => a.TargetTerritoryId == TargetTerritoryId);
            }
            if (EventId != null)
            {
                aActivities = aActivities.Where(a => a.EventId == EventId);
            }
            if (Year != null)
            {
                aActivities = aActivities.Where(a => a.Year == Year);
            }
            if (ActivityType != null)
            {
                aActivities = aActivities.Where(a => a.ActivityType == ActivityType);
            }

            switch (SortOrder)
            {
                case "TypeOfTargetId":
                    aActivities = aActivities.OrderBy(a => a.Target.TypeOfTarget.Name);
                    break;
                case "TypeOfTargetIdDesc":
                    aActivities = aActivities.OrderByDescending(a => a.Target.TypeOfTarget.Name);
                    break;
                case "TargetId":
                    aActivities = aActivities.OrderBy(a => a.Target.Name);
                    break;
                case "TargetIdDesc":
                    aActivities = aActivities.OrderByDescending(a => a.Target.Name);
                    break;
                case "MeasurementUnitId":
                    aActivities = aActivities.OrderBy(a => a.Target.MeasurementUnit.Name);
                    break;
                case "MeasurementUnitIdDesc":
                    aActivities = aActivities.OrderByDescending(a => a.Target.MeasurementUnit.Name);
                    break;
                case "TerritoryTypeId":
                    aActivities = aActivities.OrderBy(a => a.TargetTerritory.TerritoryType.Name);
                    break;
                case "TerritoryTypeIdDesc":
                    aActivities = aActivities.OrderByDescending(a => a.TargetTerritory.TerritoryType.Name);
                    break;
                case "TargetTerritoryId":
                    aActivities = aActivities.OrderBy(a => a.TargetTerritory.TerritoryName);
                    break;
                case "TargetTerritoryIdDesc":
                    aActivities = aActivities.OrderByDescending(a => a.TargetTerritory.TerritoryName);
                    break;
                case "EventId":
                    aActivities = aActivities.OrderBy(a => a.Event.Name);
                    break;
                case "EventIdDesc":
                    aActivities = aActivities.OrderByDescending(a => a.Event.Name);
                    break;
                case "Year":
                    aActivities = aActivities.OrderBy(a => a.Year);
                    break;
                case "YearDesc":
                    aActivities = aActivities.OrderByDescending(a => a.Year);
                    break;
                case "ActivityType":
                    aActivities = aActivities.OrderBy(a => a.ActivityTypeName);
                    break;
                case "ActivityTypeDesc":
                    aActivities = aActivities.OrderByDescending(a => a.ActivityTypeName);
                    break;
                default:
                    aActivities = aActivities.OrderBy(a => a.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(aActivities.Count(), Page);

            var viewModel = new AActivityIndexPageViewModel
            {
                Items = aActivities.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewBag.TypeOfTargetId = new SelectList(_context.TypeOfTarget.OrderBy(a => a.Name), "Id", "Name");
            ViewBag.TargetId = new SelectList(_context.Target.OrderBy(a => a.Name), "Id", "Name");
            ViewBag.MeasurementUnitId = new SelectList(_context.MeasurementUnit.OrderBy(m => m.Name), "Id", "Name");
            ViewBag.TerritoryTypeId = new SelectList(_context.TerritoryType.OrderBy(a => a.Name), "Id", "Name");
            ViewBag.TargetTerritoryId = new SelectList(_context.TargetTerritory.OrderBy(a => a.TerritoryName), "Id", "TerritoryName");
            ViewBag.TypeOfTargetId = new SelectList(_context.TypeOfTarget.OrderBy(a => a.Name), "Id", "Name");
            ViewBag.EventId = new SelectList(_context.Event.OrderBy(a => a.Name), "Id", "Name");
            ViewBag.Year = new SelectList(Enumerable.Range(Constants.YearMin, Constants.YearMax - Constants.YearMin + 1).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");
            ViewBag.ActivityType = new List<SelectListItem>() {
                new SelectListItem() { Text=_sharedLocalizer["Actual"], Value="true"},
                new SelectListItem() { Text=_sharedLocalizer["Planned"], Value="false"}
            }.OrderBy(s => s.Text);

            return View(viewModel);
        }

        // GET: AActivities/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aActivity = await _context.AActivity
                .Include(a => a.Event)
                .Include(a => a.Target)
                .Include(a => a.Target.MeasurementUnit)
                .Include(a => a.Target.TypeOfTarget)
                .Include(a => a.TargetTerritory)
                .Include(a => a.TargetTerritory.TerritoryType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (aActivity == null)
            {
                return NotFound();
            }

            return View(aActivity);
        }

        // GET: AActivities/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewBag.TypeOfTargetId = new SelectList(_context.TypeOfTarget.OrderBy(a => a.Name), "Id", "Name");
            ViewBag.TargetId = new SelectList(_context.Target.Where(a => a.TypeOfTargetId == _context.TypeOfTarget.OrderBy(tt => tt.Name).Select(tt => tt.Id).FirstOrDefault()).OrderBy(a => a.Name), "Id", "Name");
            ViewBag.MeasurementUnitId = new SelectList(_context.MeasurementUnit.OrderBy(m => m.Name), "Id", "Name", _context.Target.Where(a => a.TypeOfTargetId == _context.TypeOfTarget.OrderBy(tt => tt.Name).Select(tt => tt.Id).FirstOrDefault()).OrderBy(a => a.Name).FirstOrDefault().MeasurementUnitId);
            ViewBag.TerritoryTypeId = new SelectList(_context.TerritoryType.OrderBy(a => a.Name), "Id", "Name");
            ViewBag.TargetTerritoryId = new SelectList(_context.TargetTerritory.Where(a => a.TerritoryTypeId == _context.TerritoryType.OrderBy(tt => tt.Name).Select(tt => tt.Id).FirstOrDefault()).OrderBy(a => a.TerritoryName), "Id", "TerritoryName");
            ViewData["EventId"] = new SelectList(_context.Event.OrderBy(a => a.Name), "Id", "Name");
            ViewBag.Year = new SelectList(Enumerable.Range(Constants.YearMin, Constants.YearMax - Constants.YearMin + 1).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");
            AActivity model = new AActivity
            {
                ActivityType = true
            };
            return View(model);
        }

        // POST: AActivities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,TargetId,TargetTerritoryId,EventId,Year,ActivityType,ImplementationPercentage,AdditionalInformationKK,AdditionalInformationRU")] AActivity aActivity)
        {
            aActivity.Target = _context.Target.FirstOrDefault(a => a.Id == aActivity.TargetId);
            aActivity.TargetTerritory = _context.TargetTerritory.FirstOrDefault(a => a.Id == aActivity.TargetTerritoryId);
            if (_context.AActivity.AsNoTracking().FirstOrDefault(a => a.TargetId == aActivity.TargetId
                && a.TargetTerritoryId == aActivity.TargetTerritoryId
                && a.Year == aActivity.Year
                && a.EventId == aActivity.EventId
                && a.ActivityType == aActivity.ActivityType) != null)
            {
                ModelState.AddModelError("", $"{_sharedLocalizer["ErrorDublicateValue"]} " +
                    $"({_sharedLocalizer["Target"]}, " +
                    $"{_sharedLocalizer["TargetTerritory"]}, " +
                    $"{_sharedLocalizer["Year"]}, " +
                    $"{_sharedLocalizer["ActivityType"]})");
            }
            if(aActivity.ActivityType && aActivity.ImplementationPercentage == null)
            {
                ModelState.AddModelError("ImplementationPercentage", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(aActivity);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "AActivity",
                    New = aActivity.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TypeOfTargetId = new SelectList(_context.TypeOfTarget.OrderBy(a => a.Name), "Id", "Name", aActivity.Target.TypeOfTargetId);
            ViewBag.TargetId = new SelectList(_context.Target.Where(a => a.TypeOfTargetId == aActivity.Target.TypeOfTargetId).OrderBy(a => a.Name), "Id", "Name", aActivity.TargetId);
            ViewBag.MeasurementUnitId = new SelectList(_context.MeasurementUnit.OrderBy(m => m.Name), "Id", "Name", _context.Target.FirstOrDefault(a => a.Id == aActivity.TargetId).MeasurementUnitId);
            ViewBag.TerritoryTypeId = new SelectList(_context.TerritoryType.OrderBy(a => a.Name), "Id", "Name", aActivity.TargetTerritory.TerritoryTypeId);
            ViewBag.TargetTerritoryId = new SelectList(_context.TargetTerritory.Where(a => a.TerritoryTypeId == aActivity.TargetTerritory.TerritoryTypeId).OrderBy(a => a.TerritoryName), "Id", "TerritoryName");
            ViewBag.Year = new SelectList(Enumerable.Range(Constants.YearMin, Constants.YearMax - Constants.YearMin + 1).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text", aActivity.Year);
            ViewData["EventId"] = new SelectList(_context.Event.OrderBy(e => e.Name), "Id", "Name", aActivity.EventId);
            return View(aActivity);
        }

        // GET: AActivities/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aActivity = await _context.AActivity.SingleOrDefaultAsync(m => m.Id == id);
            if (aActivity == null)
            {
                return NotFound();
            }
            aActivity.Target = _context.Target.FirstOrDefault(a => a.Id == aActivity.TargetId);
            aActivity.TargetTerritory = _context.TargetTerritory.FirstOrDefault(a => a.Id == aActivity.TargetTerritoryId);
            ViewBag.TypeOfTargetId = new SelectList(_context.TypeOfTarget.OrderBy(a => a.Name), "Id", "Name", aActivity.Target.TypeOfTargetId);
            ViewBag.TargetId = new SelectList(_context.Target.Where(a => a.TypeOfTargetId == aActivity.Target.TypeOfTargetId).OrderBy(a => a.Name), "Id", "Name", aActivity.TargetId);
            ViewBag.MeasurementUnitId = new SelectList(_context.MeasurementUnit.OrderBy(m => m.Name), "Id", "Name", _context.Target.FirstOrDefault(a => a.Id == aActivity.TargetId).MeasurementUnitId);
            ViewBag.TerritoryTypeId = new SelectList(_context.TerritoryType.OrderBy(a => a.Name), "Id", "Name", aActivity.TargetTerritory.TerritoryTypeId);
            ViewBag.TargetTerritoryId = new SelectList(_context.TargetTerritory.Where(a => a.TerritoryTypeId == aActivity.TargetTerritory.TerritoryTypeId).OrderBy(a => a.TerritoryName), "Id", "TerritoryName");
            ViewBag.Year = new SelectList(Enumerable.Range(Constants.YearMin, Constants.YearMax - Constants.YearMin + 1).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text", aActivity.Year);
            ViewData["EventId"] = new SelectList(_context.Event.OrderBy(e => e.Name), "Id", "Name", aActivity.EventId);
            return View(aActivity);
        }

        // POST: AActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TargetId,TargetTerritoryId,EventId,Year,ActivityType,ImplementationPercentage,AdditionalInformationKK,AdditionalInformationRU")] AActivity aActivity)
        {
            if (id != aActivity.Id)
            {
                return NotFound();
            }
            aActivity.Target = _context.Target.FirstOrDefault(a => a.Id == aActivity.TargetId);
            aActivity.TargetTerritory = _context.TargetTerritory.FirstOrDefault(a => a.Id == aActivity.TargetTerritoryId);
            if (_context.AActivity.AsNoTracking().FirstOrDefault(a => a.Id != aActivity.Id
                && a.TargetId == aActivity.TargetId
                && a.TargetTerritoryId == aActivity.TargetTerritoryId
                && a.Year == aActivity.Year
                && a.EventId == aActivity.EventId
                && a.ActivityType == aActivity.ActivityType) != null)
            {
                ModelState.AddModelError("", $"{_sharedLocalizer["ErrorDublicateValue"]} " +
                    $"({_sharedLocalizer["Target"]}, " +
                    $"{_sharedLocalizer["TargetTerritory"]}, " +
                    $"{_sharedLocalizer["Year"]}, " +
                    $"{_sharedLocalizer["ActivityType"]})");
            }
            if (aActivity.ActivityType && aActivity.ImplementationPercentage == null)
            {
                ModelState.AddModelError("ImplementationPercentage", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var aActivity_old = _context.AActivity.AsNoTracking().FirstOrDefault(a => a.Id == aActivity.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "AActivity",
                        Operation = "Edit",
                        New = aActivity.ToString(),
                        Old = aActivity_old.ToString()
                    });
                    _context.Update(aActivity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AActivityExists(aActivity.Id))
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
            aActivity.Target = _context.Target.FirstOrDefault(a => a.Id == aActivity.TargetId);
            aActivity.TargetTerritory = _context.TargetTerritory.FirstOrDefault(a => a.Id == aActivity.TargetTerritoryId);
            ViewBag.TypeOfTargetId = new SelectList(_context.TypeOfTarget.OrderBy(a => a.Name), "Id", "Name", aActivity.Target.TypeOfTargetId);
            ViewBag.TargetId = new SelectList(_context.Target.Where(a => a.TypeOfTargetId == aActivity.Target.TypeOfTargetId).OrderBy(a => a.Name), "Id", "Name", aActivity.TargetId);
            ViewBag.MeasurementUnitId = new SelectList(_context.MeasurementUnit.OrderBy(m => m.Name), "Id", "Name", _context.Target.FirstOrDefault(a => a.Id == aActivity.TargetId).MeasurementUnitId);
            ViewBag.TerritoryTypeId = new SelectList(_context.TerritoryType.OrderBy(a => a.Name), "Id", "Name", aActivity.TargetTerritory.TerritoryTypeId);
            ViewBag.TargetTerritoryId = new SelectList(_context.TargetTerritory.Where(a => a.TerritoryTypeId == aActivity.TargetTerritory.TerritoryTypeId).OrderBy(a => a.TerritoryName), "Id", "TerritoryName");
            ViewBag.Year = new SelectList(Enumerable.Range(Constants.YearMin, Constants.YearMax - Constants.YearMin + 1).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text", aActivity.Year);
            ViewData["EventId"] = new SelectList(_context.Event.OrderBy(e => e.Name), "Id", "Name", aActivity.EventId);
            return View(aActivity);
        }

        // GET: AActivities/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aActivity = await _context.AActivity
                .Include(a => a.Event)
                .Include(a => a.Target.MeasurementUnit)
                .Include(a => a.Target.TypeOfTarget)
                .Include(a => a.TargetTerritory)
                .Include(a => a.TargetTerritory.TerritoryType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (aActivity == null)
            {
                return NotFound();
            }

            return View(aActivity);
        }

        // POST: AActivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aActivity = await _context.AActivity.SingleOrDefaultAsync(m => m.Id == id);
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Class = "AActivity",
                Operation = "Delete",
                New = "",
                Old = aActivity.ToString()
            });
            _context.AActivity.Remove(aActivity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AActivityExists(int id)
        {
            return _context.AActivity.Any(e => e.Id == id);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public JsonResult GetTargetsByTypeOfTargetId(int TypeOfTargetId)
        {
            var targets = _context.Target
                .Where(a => a.TypeOfTargetId == TypeOfTargetId).ToArray().OrderBy(a => a.Name);
            JsonResult result = new JsonResult(targets);
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public JsonResult GetTargetTerritoriesByTerritoryTypeId(int TerritoryTypeId)
        {
            var targetTerritories = _context.TargetTerritory
                .Where(a => a.TerritoryTypeId == TerritoryTypeId).ToArray().OrderBy(a => a.TerritoryName);
            JsonResult result = new JsonResult(targetTerritories);
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public JsonResult MeasurementUnitIdByTargetId(int TargetId)
        {
            var target = _context.Target
                .FirstOrDefault(a => a.Id == TargetId);
            JsonResult result = new JsonResult(target == null ? 0 : target.MeasurementUnitId);
            return result;
        }
    }
}
