using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Eco.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Eco.Models;

namespace Eco.Controllers
{
    public class ChartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public IActionResult Index()
        {
            ViewBag.TypeOfTargetId = new SelectList(_context.TypeOfTarget.OrderBy(a => a.Name), "Id", "Name");
            ViewBag.TargetId = new SelectList(_context.Target.Where(a => a.TypeOfTargetId == _context.TypeOfTarget.OrderBy(tt => tt.Name).Select(tt => tt.Id).FirstOrDefault()).OrderBy(a => a.Name), "Id", "Name");
            ViewBag.MeasurementUnitId = new SelectList(_context.MeasurementUnit.OrderBy(m => m.Name), "Id", "Name", _context.Target.Where(a => a.TypeOfTargetId == _context.TypeOfTarget.OrderBy(tt => tt.Name).Select(tt => tt.Id).FirstOrDefault()).OrderBy(a => a.Name).FirstOrDefault().MeasurementUnitId);
            ViewBag.TerritoryTypeId = new SelectList(_context.TerritoryType.OrderBy(a => a.Name), "Id", "Name");
            ViewBag.TargetTerritoryId = new SelectList(_context.TargetTerritory.Where(a => a.TerritoryTypeId == _context.TerritoryType.OrderBy(tt => tt.Name).Select(tt => tt.Id).FirstOrDefault()).OrderBy(a => a.TerritoryName), "Id", "TerritoryName");
            //ViewData["EventId"] = new SelectList(_context.Event, "Id", "Name");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public ActionResult GetTargetValuesAndAActivities(int TargetId, int TargetTerritoryId/*, int EventId*/)
        {
            var years = Enumerable.Range(Constants.YearMin, Constants.YearMax - Constants.YearMin + 1);
            var targetValuesPlanList = _context.TargetValue.Where(t => t.TargetId == TargetId && t.TargetTerritoryId == TargetTerritoryId && !t.TargetValueType).ToList();
            var targetValuesFactList = _context.TargetValue.Where(t => t.TargetId == TargetId && t.TargetTerritoryId == TargetTerritoryId && t.TargetValueType).ToList();
            //var aActivitiesList = _context.AActivity.Where(a => a.TargetId == TargetId && a.TargetTerritoryId == TargetTerritoryId && a.EventId == EventId && a.ActivityType).ToList();
            var targetValuesPlan = new List<decimal?>();
            var targetValuesFact = new List<decimal?>();
            //var aActivities = new List<decimal?>();
            foreach (int year in years)
            {
                TargetValue targetValuePlan = targetValuesPlanList.FirstOrDefault(t => t.Year == year);
                targetValuesPlan.Add(targetValuePlan?.Value);
                TargetValue targetValueFact = targetValuesFactList.FirstOrDefault(t => t.Year == year);
                targetValuesFact.Add(targetValueFact?.Value);
                //AActivity aActivity = aActivitiesList.FirstOrDefault(a => a.Year == year);
                //aActivities.Add(aActivity?.ImplementationPercentage);
            }
            return Json(new
            {
                years = years,
                targetValuesPlan = targetValuesPlan,
                targetValuesFact = targetValuesFact,
                //aActivities = aActivities
            });
        }
    }
}