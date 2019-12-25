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
    public class TargetValuesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public TargetValuesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: TargetValues
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            int? TypeOfTargetId,
            int? TargetId,
            int? MeasurementUnitId,
            int? TerritoryTypeId,
            int? TargetTerritoryId,
            int? Year,
            bool? TargetValueType,
            int? Page)
        {
            var targetValues = _context.TargetValue
                .Include(t => t.Target)
                .Include(t => t.Target.MeasurementUnit)
                .Include(t => t.Target.TypeOfTarget)
                .Include(t => t.TargetTerritory)
                .Include(t => t.TargetTerritory.TerritoryType)
                .Where(t => true);

            ViewBag.TypeOfTargetIdFilter = TypeOfTargetId;
            ViewBag.TargetIdFilter = TargetId;
            ViewBag.MeasurementUnitIdFilter = MeasurementUnitId;
            ViewBag.TerritoryTypeIdFilter = TerritoryTypeId;
            ViewBag.TargetTerritoryIdFilter = TargetTerritoryId;
            ViewBag.YearFilter = Year;
            ViewBag.TargetValueTypeFilter = TargetValueType;

            ViewBag.TypeOfTargetIdSort = SortOrder == "TypeOfTargetId" ? "TypeOfTargetIdDesc" : "TypeOfTargetId";
            ViewBag.TargetIdSort = SortOrder == "TargetId" ? "TargetIdDesc" : "TargetId";
            ViewBag.MeasurementUnitIdSort = SortOrder == "MeasurementUnitId" ? "MeasurementUnitIdDesc" : "MeasurementUnitId";
            ViewBag.TerritoryTypeIdSort = SortOrder == "TerritoryTypeId" ? "TerritoryTypeIdDesc" : "TerritoryTypeId";
            ViewBag.TargetTerritoryIdSort = SortOrder == "TargetTerritoryId" ? "TargetTerritoryIdDesc" : "TargetTerritoryId";
            ViewBag.YearSort = SortOrder == "Year" ? "YearDesc" : "Year";
            ViewBag.TargetValueTypeSort = SortOrder == "TargetValueType" ? "TargetValueTypeDesc" : "TargetValueType";

            if (TypeOfTargetId != null)
            {
                targetValues = targetValues.Where(t => t.Target.TypeOfTargetId == TypeOfTargetId);
            }
            if (TargetId != null)
            {
                targetValues = targetValues.Where(t => t.TargetId == TargetId);
            }
            if (MeasurementUnitId != null)
            {
                targetValues = targetValues.Where(t => t.Target.MeasurementUnitId == MeasurementUnitId);
            }
            if (TerritoryTypeId != null)
            {
                targetValues = targetValues.Where(t => t.TargetTerritory.TerritoryTypeId == TerritoryTypeId);
            }
            if (TargetTerritoryId != null)
            {
                targetValues = targetValues.Where(t => t.TargetTerritoryId == TargetTerritoryId);
            }
            if (Year != null)
            {
                targetValues = targetValues.Where(t => t.Year == Year);
            }
            if (TargetValueType != null)
            {
                targetValues = targetValues.Where(t => t.TargetValueType == TargetValueType);
            }

            switch (SortOrder)
            {
                case "TypeOfTargetId":
                    targetValues = targetValues.OrderBy(t => t.Target.TypeOfTarget.Name);
                    break;
                case "TypeOfTargetIdDesc":
                    targetValues = targetValues.OrderByDescending(t => t.Target.TypeOfTarget.Name);
                    break;
                case "TargetId":
                    targetValues = targetValues.OrderBy(t => t.Target.Name);
                    break;
                case "TargetIdDesc":
                    targetValues = targetValues.OrderByDescending(t => t.Target.Name);
                    break;
                case "MeasurementUnitId":
                    targetValues = targetValues.OrderBy(t => t.Target.MeasurementUnit.Name);
                    break;
                case "MeasurementUnitIdDesc":
                    targetValues = targetValues.OrderByDescending(t => t.Target.MeasurementUnit.Name);
                    break;
                case "TerritoryTypeId":
                    targetValues = targetValues.OrderBy(t => t.TargetTerritory.TerritoryType.Name);
                    break;
                case "TerritoryTypeIdDesc":
                    targetValues = targetValues.OrderByDescending(t => t.TargetTerritory.TerritoryType.Name);
                    break;
                case "TargetTerritoryId":
                    targetValues = targetValues.OrderBy(t => t.TargetTerritory.TerritoryName);
                    break;
                case "TargetTerritoryIdDesc":
                    targetValues = targetValues.OrderByDescending(t => t.TargetTerritory.TerritoryName);
                    break;
                case "Year":
                    targetValues = targetValues.OrderBy(t => t.Year);
                    break;
                case "YearDesc":
                    targetValues = targetValues.OrderByDescending(t => t.Year);
                    break;
                case "TargetValueType":
                    targetValues = targetValues.OrderBy(t => t.TargetValueTypeName);
                    break;
                case "TargetValueTypeDesc":
                    targetValues = targetValues.OrderByDescending(t => t.TargetValueTypeName);
                    break;
                default:
                    targetValues = targetValues.OrderBy(t => t.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(targetValues.Count(), Page);

            var viewModel = new TargetValueIndexPageViewModel
            {
                Items = targetValues.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewBag.TypeOfTargetId = new SelectList(_context.TypeOfTarget.OrderBy(t => t.Name), "Id", "Name");
            ViewBag.TargetId = new SelectList(_context.Target.OrderBy(t => t.Name), "Id", "Name");
            ViewBag.MeasurementUnitId = new SelectList(_context.MeasurementUnit.OrderBy(m => m.Name), "Id", "Name");
            ViewBag.TerritoryTypeId = new SelectList(_context.TerritoryType.OrderBy(t => t.Name), "Id", "Name");
            ViewBag.TargetTerritoryId = new SelectList(_context.TargetTerritory.OrderBy(t => t.TerritoryName), "Id", "TerritoryName");
            ViewBag.TypeOfTargetId = new SelectList(_context.TypeOfTarget.OrderBy(t => t.Name), "Id", "Name");
            ViewBag.Year = new SelectList(Enumerable.Range(Constants.YearMin, Constants.YearMax - Constants.YearMin + 1).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");
            ViewBag.TargetValueType = new List<SelectListItem>() {
                new SelectListItem() { Text=_sharedLocalizer["Actual"], Value="true"},
                new SelectListItem() { Text=_sharedLocalizer["Planned"], Value="false"}
            }.OrderBy(s => s.Text);

            return View(viewModel);
        }

        // GET: TargetValues/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var targetValue = await _context.TargetValue
                .Include(t => t.Target)
                .Include(t => t.Target.MeasurementUnit)
                .Include(t => t.Target.TypeOfTarget)
                .Include(t => t.TargetTerritory)
                .Include(t => t.TargetTerritory.TerritoryType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (targetValue == null)
            {
                return NotFound();
            }

            return View(targetValue);
        }

        // GET: TargetValues/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewBag.TypeOfTargetId = new SelectList(_context.TypeOfTarget.OrderBy(t => t.Name), "Id", "Name");
            ViewBag.TargetId = new SelectList(_context.Target.Where(t => t.TypeOfTargetId == _context.TypeOfTarget.OrderBy(tt => tt.Name).Select(tt => tt.Id).FirstOrDefault()).OrderBy(t => t.Name), "Id", "Name");
            ViewBag.MeasurementUnitId = new SelectList(_context.MeasurementUnit.OrderBy(m => m.Name), "Id", "Name", _context.Target.Where(t => t.TypeOfTargetId == _context.TypeOfTarget.OrderBy(tt => tt.Name).Select(tt => tt.Id).FirstOrDefault()).OrderBy(t => t.Name).FirstOrDefault().MeasurementUnitId);
            ViewBag.TerritoryTypeId = new SelectList(_context.TerritoryType.OrderBy(t => t.Name), "Id", "Name");
            ViewBag.TargetTerritoryId = new SelectList(_context.TargetTerritory.Where(t => t.TerritoryTypeId == _context.TerritoryType.OrderBy(tt => tt.Name).Select(tt => tt.Id).FirstOrDefault()).OrderBy(t => t.TerritoryName), "Id", "TerritoryName");
            ViewBag.Year = new SelectList(Enumerable.Range(Constants.YearMin, Constants.YearMax - Constants.YearMin + 1).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");
            TargetValue model = new TargetValue
            {
                TargetValueType = true
            };
            return View(model);
        }

        // POST: TargetValues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,TargetId,TargetTerritoryId,Year,TargetValueType,AdditionalInformationKK,AdditionalInformationRU,Value")] TargetValue targetValue)
        {
            targetValue.Target = _context.Target.FirstOrDefault(t => t.Id == targetValue.TargetId);
            targetValue.TargetTerritory = _context.TargetTerritory.FirstOrDefault(t => t.Id == targetValue.TargetTerritoryId);
            if (_context.TargetValue.AsNoTracking().FirstOrDefault(t => t.TargetId == targetValue.TargetId
                && t.TargetTerritoryId == targetValue.TargetTerritoryId
                && t.Year == targetValue.Year
                && t.TargetValueType == targetValue.TargetValueType) != null)
            {
                ModelState.AddModelError("", $"{_sharedLocalizer["ErrorDublicateValue"]} " +
                    $"({_sharedLocalizer["Target"]}, " +
                    $"{_sharedLocalizer["TargetTerritory"]}, " +
                    $"{_sharedLocalizer["Year"]}, " +
                    $"{_sharedLocalizer["TargetValueType"]})");
            }
            if (ModelState.IsValid)
            {
                _context.Add(targetValue);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "TargetValue",
                    New = targetValue.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TypeOfTargetId = new SelectList(_context.TypeOfTarget.OrderBy(t => t.Name), "Id", "Name", targetValue.Target.TypeOfTargetId);
            ViewBag.TargetId = new SelectList(_context.Target.Where(t => t.TypeOfTargetId == targetValue.Target.TypeOfTargetId).OrderBy(t => t.Name), "Id", "Name", targetValue.TargetId);
            ViewBag.MeasurementUnitId = new SelectList(_context.MeasurementUnit.OrderBy(m => m.Name), "Id", "Name", _context.Target.FirstOrDefault(t => t.Id == targetValue.TargetId).MeasurementUnitId);
            ViewBag.TerritoryTypeId = new SelectList(_context.TerritoryType.OrderBy(t => t.Name), "Id", "Name", targetValue.TargetTerritory.TerritoryTypeId);
            ViewBag.TargetTerritoryId = new SelectList(_context.TargetTerritory.Where(t => t.TerritoryTypeId == targetValue.TargetTerritory.TerritoryTypeId).OrderBy(t => t.TerritoryName), "Id", "TerritoryName");
            ViewBag.Year = new SelectList(Enumerable.Range(Constants.YearMin, Constants.YearMax - Constants.YearMin + 1).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text", targetValue.Year);
            return View(targetValue);
        }

        // GET: TargetValues/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var targetValue = await _context.TargetValue.SingleOrDefaultAsync(m => m.Id == id);
            if (targetValue == null)
            {
                return NotFound();
            }
            targetValue.Target = _context.Target.FirstOrDefault(t => t.Id == targetValue.TargetId);
            targetValue.TargetTerritory = _context.TargetTerritory.FirstOrDefault(t => t.Id == targetValue.TargetTerritoryId);
            ViewBag.TypeOfTargetId = new SelectList(_context.TypeOfTarget.OrderBy(t => t.Name), "Id", "Name", targetValue.Target.TypeOfTargetId);
            ViewBag.TargetId = new SelectList(_context.Target.Where(t => t.TypeOfTargetId == targetValue.Target.TypeOfTargetId).OrderBy(t => t.Name), "Id", "Name", targetValue.TargetId);
            ViewBag.MeasurementUnitId = new SelectList(_context.MeasurementUnit.OrderBy(m => m.Name), "Id", "Name", _context.Target.FirstOrDefault(t => t.Id == targetValue.TargetId).MeasurementUnitId);
            ViewBag.TerritoryTypeId = new SelectList(_context.TerritoryType.OrderBy(t => t.Name), "Id", "Name", targetValue.TargetTerritory.TerritoryTypeId);
            ViewBag.TargetTerritoryId = new SelectList(_context.TargetTerritory.Where(t => t.TerritoryTypeId == targetValue.TargetTerritory.TerritoryTypeId).OrderBy(t => t.TerritoryName), "Id", "TerritoryName");
            ViewBag.Year = new SelectList(Enumerable.Range(Constants.YearMin, Constants.YearMax - Constants.YearMin + 1).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text", targetValue.Year);
            return View(targetValue);
        }

        // POST: TargetValues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TargetId,TargetTerritoryId,Year,TargetValueType,AdditionalInformationKK,AdditionalInformationRU,Value")] TargetValue targetValue)
        {
            if (id != targetValue.Id)
            {
                return NotFound();
            }
            targetValue.Target = _context.Target.FirstOrDefault(t => t.Id == targetValue.TargetId);
            targetValue.TargetTerritory = _context.TargetTerritory.FirstOrDefault(t => t.Id == targetValue.TargetTerritoryId);
            if (_context.TargetValue.AsNoTracking().FirstOrDefault(t => t.Id != targetValue.Id
                && t.TargetId == targetValue.TargetId
                && t.TargetTerritoryId == targetValue.TargetTerritoryId
                && t.Year == targetValue.Year
                && t.TargetValueType == targetValue.TargetValueType) != null)
            {
                ModelState.AddModelError("", $"{_sharedLocalizer["ErrorDublicateValue"]} " +
                    $"({_sharedLocalizer["Target"]}, " +
                    $"{_sharedLocalizer["TargetTerritory"]}, " +
                    $"{_sharedLocalizer["Year"]}, " +
                    $"{_sharedLocalizer["TargetValueType"]})");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var targetValue_old = _context.TargetValue.AsNoTracking().FirstOrDefault(t => t.Id == targetValue.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "TargetValue",
                        Operation = "Edit",
                        New = targetValue.ToString(),
                        Old = targetValue_old.ToString()
                    });
                    _context.Update(targetValue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TargetValueExists(targetValue.Id))
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
            ViewBag.TypeOfTargetId = new SelectList(_context.TypeOfTarget.OrderBy(t => t.Name), "Id", "Name", targetValue.Target.TypeOfTargetId);
            ViewBag.TargetId = new SelectList(_context.Target.Where(t => t.TypeOfTargetId == targetValue.Target.TypeOfTargetId).OrderBy(t => t.Name), "Id", "Name", targetValue.TargetId);
            ViewBag.MeasurementUnitId = new SelectList(_context.MeasurementUnit.OrderBy(m => m.Name), "Id", "Name", _context.Target.FirstOrDefault(t => t.Id == targetValue.TargetId).MeasurementUnitId);
            ViewBag.TerritoryTypeId = new SelectList(_context.TerritoryType.OrderBy(t => t.Name), "Id", "Name", targetValue.TargetTerritory.TerritoryTypeId);
            ViewBag.TargetTerritoryId = new SelectList(_context.TargetTerritory.Where(t => t.TerritoryTypeId == targetValue.TargetTerritory.TerritoryTypeId).OrderBy(t => t.TerritoryName), "Id", "TerritoryName");
            ViewBag.Year = new SelectList(Enumerable.Range(Constants.YearMin, Constants.YearMax - Constants.YearMin + 1).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text", targetValue.Year);
            return View(targetValue);
        }

        // GET: TargetValues/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var targetValue = await _context.TargetValue
                .Include(t => t.Target)
                .Include(t => t.Target.MeasurementUnit)
                .Include(t => t.Target.TypeOfTarget)
                .Include(t => t.TargetTerritory)
                .Include(t => t.TargetTerritory.TerritoryType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (targetValue == null)
            {
                return NotFound();
            }

            return View(targetValue);
        }

        // POST: TargetValues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var targetValue = await _context.TargetValue.SingleOrDefaultAsync(m => m.Id == id);
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Class = "TargetValue",
                Operation = "Delete",
                New = "",
                Old = targetValue.ToString()
            });
            _context.TargetValue.Remove(targetValue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TargetValueExists(int id)
        {
            return _context.TargetValue.Any(e => e.Id == id);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public JsonResult GetTargetsByTypeOfTargetId(int TypeOfTargetId)
        {
            var targets = _context.Target
                .Where(t => t.TypeOfTargetId == TypeOfTargetId).ToArray().OrderBy(t => t.Name);
            JsonResult result = new JsonResult(targets);
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public JsonResult GetTargetTerritoriesByTerritoryTypeId(int TerritoryTypeId)
        {
            var targetTerritories = _context.TargetTerritory
                .Where(t => t.TerritoryTypeId == TerritoryTypeId).ToArray().OrderBy(t => t.TerritoryName);
            JsonResult result = new JsonResult(targetTerritories);
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public JsonResult MeasurementUnitIdByTargetId(int TargetId)
        {
            var target = _context.Target
                .FirstOrDefault(t => t.Id == TargetId);
            JsonResult result = new JsonResult(target == null ? 0 : target.MeasurementUnitId);
            return result;
        }
    }
}
