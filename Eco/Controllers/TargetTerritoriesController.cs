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
    public class TargetTerritoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public TargetTerritoriesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: TargetTerritories
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            int? TerritoryTypeId,
            string TerritoryNameKK,
            string TerritoryNameRU,
            string GISConnectionCode,
            int? Page)
        {
            var targetTerritories = _context.TargetTerritory
                .Include(t => t.CityDistrict)
                .Include(t => t.TerritoryType)
                .Where(t => true);

            ViewBag.TerritoryTypeIdFilter = TerritoryTypeId;
            ViewBag.TerritoryNameKKFilter = TerritoryNameKK;
            ViewBag.TerritoryNameRUFilter = TerritoryNameRU;
            ViewBag.GISConnectionCodeFilter = GISConnectionCode;

            ViewBag.TerritoryTypeIdSort = SortOrder == "TerritoryTypeId" ? "TerritoryTypeIdDesc" : "TerritoryTypeId";
            ViewBag.TerritoryNameKKSort = SortOrder == "TerritoryNameKK" ? "TerritoryNameKKDesc" : "TerritoryNameKK";
            ViewBag.TerritoryNameRUSort = SortOrder == "TerritoryNameRU" ? "TerritoryNameRUDesc" : "TerritoryNameRU";
            ViewBag.GISConnectionCodeSort = SortOrder == "GISConnectionCode" ? "GISConnectionCodeDesc" : "GISConnectionCode";

            if (TerritoryTypeId!=null)
            {
                targetTerritories = targetTerritories.Where(t => t.TerritoryTypeId == TerritoryTypeId);
            }
            if (!string.IsNullOrEmpty(TerritoryNameKK))
            {
                targetTerritories = targetTerritories.Where(t => t.TerritoryNameKK.ToLower().Contains(TerritoryNameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(TerritoryNameRU))
            {
                targetTerritories = targetTerritories.Where(t => t.TerritoryNameRU.ToLower().Contains(TerritoryNameRU.ToLower()));
            }
            if (!string.IsNullOrEmpty(GISConnectionCode))
            {
                targetTerritories = targetTerritories.Where(t => t.GISConnectionCode.ToLower().Contains(GISConnectionCode.ToLower()));
            }

            switch (SortOrder)
            {
                case "TerritoryTypeId":
                    targetTerritories = targetTerritories.OrderBy(t => t.TerritoryType.Name);
                    break;
                case "TerritoryTypeIdDesc":
                    targetTerritories = targetTerritories.OrderByDescending(t => t.TerritoryType.Name);
                    break;
                case "TerritoryNameKK":
                    targetTerritories = targetTerritories.OrderBy(t => t.TerritoryNameKK);
                    break;
                case "TerritoryNameKKDesc":
                    targetTerritories = targetTerritories.OrderByDescending(t => t.TerritoryNameKK);
                    break;
                case "TerritoryNameRU":
                    targetTerritories = targetTerritories.OrderBy(t => t.TerritoryNameRU);
                    break;
                case "TerritoryNameRUDesc":
                    targetTerritories = targetTerritories.OrderByDescending(t => t.TerritoryNameRU);
                    break;
                case "GISConnectionCode":
                    targetTerritories = targetTerritories.OrderBy(t => t.GISConnectionCode);
                    break;
                case "GISConnectionCodeDesc":
                    targetTerritories = targetTerritories.OrderByDescending(t => t.GISConnectionCode);
                    break;
                default:
                    targetTerritories = targetTerritories.OrderBy(t => t.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(targetTerritories.Count(), Page);

            var viewModel = new TargetTerritoryIndexPageViewModel
            {
                Items = targetTerritories.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewBag.TerritoryTypeId = new SelectList(_context.TerritoryType.OrderBy(t => t.Name), "Id", "Name");
            
            return View(viewModel);
        }

        // GET: TargetTerritories/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var targetTerritory = await _context.TargetTerritory
                .Include(t => t.CityDistrict)
                .Include(t => t.TerritoryType)
                .Include(t => t.KazHydrometAirPost)
                .Include(t => t.AirPost)
                .Include(t => t.TransportPost)
                .Include(t => t.WaterSurfacePost)
                .Include(t => t.WaterSurfacePost.WaterObject)
                .Include(t => t.KazHydrometWaterPost)
                .Include(t => t.KazHydrometSoilPost)
                .Include(t => t.SoilPost)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (targetTerritory == null)
            {
                return NotFound();
            }

            return View(targetTerritory);
        }

        // GET: TargetTerritories/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name");
            ViewData["TerritoryTypeId"] = new SelectList(_context.TerritoryType.OrderBy(c => c.Name), "Id", "Name");
            ViewData["CityDistrictCATO"] = new SelectList(_context.CityDistrict, "Id", "CATO");
            ViewData["CityDistrictNameKK"] = new SelectList(_context.CityDistrict, "Id", "NameKK");
            ViewData["CityDistrictNameRU"] = new SelectList(_context.CityDistrict, "Id", "NameRU");

            ViewData["City"] = _context.TerritoryType.FirstOrDefault(t => t.NameRU == Startup.Configuration["City"]).Id;
            ViewData["CityDistrict"] = _context.TerritoryType.FirstOrDefault(t => t.NameRU == Startup.Configuration["CityDistrict"]).Id;
            ViewData["OtherTerritoryType"] = _context.TerritoryType.FirstOrDefault(t => t.NameRU == Startup.Configuration["OtherTerritoryType"]).Id;
            ViewData["AlmatyCATO"] = Startup.Configuration["AlmatyCATO"];
            ViewData["AlmatyKK"] = Startup.Configuration["AlmatyKK"];
            ViewData["AlmatyRU"] = Startup.Configuration["AlmatyRU"];

            ViewData["KazHydrometAirPostId"] = new SelectList(_context.KazHydrometAirPost.OrderBy(k => k.Number), "Id", "Number");
            ViewData["AirPostId"] = new SelectList(_context.AirPost.OrderBy(k => k.Name), "Id", "Name");
            ViewData["TransportPostId"] = new SelectList(_context.TransportPost.OrderBy(k => k.Name), "Id", "Name");
            ViewData["WaterSurfacePostId"] = new SelectList(_context.WaterSurfacePost.Include(w => w.WaterObject).OrderBy(k => k.WaterObjectName), "Id", "WaterObjectName");
            ViewData["KazHydrometWaterPostId"] = new SelectList(_context.KazHydrometWaterPost.OrderBy(k => k.Name), "Id", "Name");
            ViewData["KazHydrometSoilPostId"] = new SelectList(_context.KazHydrometSoilPost.OrderBy(k => k.Name), "Id", "Name");
            ViewData["SoilPostId"] = new SelectList(_context.SoilPost.OrderBy(k => k.Name), "Id", "Name");

            TargetTerritory model = new TargetTerritory()
            {
                KazHydrometAirPostId = _context.KazHydrometAirPost.FirstOrDefault()?.Id
            };

            return View(model);
        }

        // POST: TargetTerritories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,TerritoryTypeId,CityDistrictId,TerritoryNameKK,TerritoryNameRU,GISConnectionCode,AdditionalInformationKK,AdditionalInformationRU,KazHydrometAirPostId,AirPostId,TransportPostId,WaterSurfacePostId,KazHydrometWaterPostId,KazHydrometSoilPostId,SoilPostId")] TargetTerritory targetTerritory)
        {
            TerritoryType targetTerritoryType = _context.TerritoryType.AsNoTracking().FirstOrDefault(t => t.Id == targetTerritory.TerritoryTypeId);
            var targetTerritories = _context.TargetTerritory.AsNoTracking().Include(t => t.TerritoryType).Include(t => t.CityDistrict).ToList();
            if (targetTerritoryType.NameRU == Startup.Configuration["OtherTerritoryType"])
            {
                if (string.IsNullOrWhiteSpace(targetTerritory.TerritoryNameKK))
                {
                    ModelState.AddModelError("TerritoryNameKK", _sharedLocalizer["ErrorNeedToInput"]);
                }
                if (string.IsNullOrWhiteSpace(targetTerritory.TerritoryNameRU))
                {
                    ModelState.AddModelError("TerritoryNameRU", _sharedLocalizer["ErrorNeedToInput"]);
                }
                if (targetTerritories.Select(t => t.TerritoryNameKK).Contains(targetTerritory.TerritoryNameKK))
                {
                    ModelState.AddModelError("TerritoryNameKK", _sharedLocalizer["ErrorDublicateValue"]);
                }
                if (targetTerritories.Select(t => t.TerritoryNameRU).Contains(targetTerritory.TerritoryNameRU))
                {
                    ModelState.AddModelError("TerritoryNameRU", _sharedLocalizer["ErrorDublicateValue"]);
                }
                if (targetTerritories.Select(t => t.GISConnectionCode).Contains(targetTerritory.GISConnectionCode))
                {
                    ModelState.AddModelError("GISConnectionCode", _sharedLocalizer["ErrorDublicateValue"]);
                }
                targetTerritory.CityDistrictId = null;
            }
            if (targetTerritoryType.NameRU == Startup.Configuration["City"])
            {
                targetTerritory.CityDistrictId = null;
                if (targetTerritories.Select(t => t.TerritoryType.NameRU).Contains(Startup.Configuration["City"]))
                {
                    ModelState.AddModelError("TerritoryTypeId", _sharedLocalizer["ErrorDublicateValue"]);
                }
            }
            if (targetTerritoryType.NameRU == Startup.Configuration["CityDistrict"])
            {
                if (targetTerritories.Select(t => t.CityDistrict != null ? t.CityDistrict.Id : 0).Contains((int)targetTerritory.CityDistrictId))
                {
                    ModelState.AddModelError("CityDistrictId", _sharedLocalizer["ErrorDublicateValue"]);
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(targetTerritory);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "TargetTerritory",
                    New = targetTerritory.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name", targetTerritory.CityDistrictId);
            ViewData["TerritoryTypeId"] = new SelectList(_context.TerritoryType.OrderBy(c => c.Name), "Id", "Name", targetTerritory.TerritoryTypeId);
            ViewData["CityDistrictCATO"] = new SelectList(_context.CityDistrict, "Id", "CATO");
            ViewData["CityDistrictNameKK"] = new SelectList(_context.CityDistrict, "Id", "NameKK");
            ViewData["CityDistrictNameRU"] = new SelectList(_context.CityDistrict, "Id", "NameRU");

            ViewData["City"] = _context.TerritoryType.FirstOrDefault(t => t.NameRU == Startup.Configuration["City"]).Id;
            ViewData["CityDistrict"] = _context.TerritoryType.FirstOrDefault(t => t.NameRU == Startup.Configuration["CityDistrict"]).Id;
            ViewData["OtherTerritoryType"] = _context.TerritoryType.FirstOrDefault(t => t.NameRU == Startup.Configuration["OtherTerritoryType"]).Id;
            ViewData["AlmatyCATO"] = Startup.Configuration["AlmatyCATO"];
            ViewData["AlmatyKK"] = Startup.Configuration["AlmatyKK"];
            ViewData["AlmatyRU"] = Startup.Configuration["AlmatyRU"];

            ViewData["KazHydrometAirPostId"] = new SelectList(_context.KazHydrometAirPost.OrderBy(k => k.Number), "Id", "Number", targetTerritory.KazHydrometAirPostId);
            ViewData["AirPostId"] = new SelectList(_context.AirPost.OrderBy(k => k.Name), "Id", "Name", targetTerritory.AirPostId);
            ViewData["TransportPostId"] = new SelectList(_context.TransportPost.OrderBy(k => k.Name), "Id", "Name", targetTerritory.TransportPostId);
            ViewData["WaterSurfacePostId"] = new SelectList(_context.WaterSurfacePost.Include(w => w.WaterObject).OrderBy(k => k.WaterObjectName), "Id", "WaterObjectName", targetTerritory.WaterSurfacePostId);
            ViewData["KazHydrometWaterPostId"] = new SelectList(_context.KazHydrometWaterPost.OrderBy(k => k.Name), "Id", "Name", targetTerritory.KazHydrometWaterPostId);
            ViewData["KazHydrometSoilPostId"] = new SelectList(_context.KazHydrometSoilPost.OrderBy(k => k.Name), "Id", "Name", targetTerritory.KazHydrometSoilPostId);
            ViewData["SoilPostId"] = new SelectList(_context.SoilPost.OrderBy(k => k.Name), "Id", "Name", targetTerritory.SoilPostId);

            return View(targetTerritory);
        }

        // GET: TargetTerritories/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var targetTerritory = await _context.TargetTerritory.SingleOrDefaultAsync(m => m.Id == id);
            if (targetTerritory == null)
            {
                return NotFound();
            }
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name", targetTerritory.CityDistrictId);
            ViewData["TerritoryTypeId"] = new SelectList(_context.TerritoryType.OrderBy(c => c.Name), "Id", "Name", targetTerritory.TerritoryTypeId);
            ViewData["CityDistrictCATO"] = new SelectList(_context.CityDistrict, "Id", "CATO");
            ViewData["CityDistrictNameKK"] = new SelectList(_context.CityDistrict, "Id", "NameKK");
            ViewData["CityDistrictNameRU"] = new SelectList(_context.CityDistrict, "Id", "NameRU");

            ViewData["City"] = _context.TerritoryType.FirstOrDefault(t => t.NameRU == Startup.Configuration["City"]).Id;
            ViewData["CityDistrict"] = _context.TerritoryType.FirstOrDefault(t => t.NameRU == Startup.Configuration["CityDistrict"]).Id;
            ViewData["OtherTerritoryType"] = _context.TerritoryType.FirstOrDefault(t => t.NameRU == Startup.Configuration["OtherTerritoryType"]).Id;
            ViewData["AlmatyCATO"] = Startup.Configuration["AlmatyCATO"];
            ViewData["AlmatyKK"] = Startup.Configuration["AlmatyKK"];
            ViewData["AlmatyRU"] = Startup.Configuration["AlmatyRU"];

            ViewData["KazHydrometAirPostId"] = new SelectList(_context.KazHydrometAirPost.OrderBy(k => k.Number), "Id", "Number", targetTerritory.KazHydrometAirPostId);
            ViewData["AirPostId"] = new SelectList(_context.AirPost.OrderBy(k => k.Name), "Id", "Name", targetTerritory.AirPostId);
            ViewData["TransportPostId"] = new SelectList(_context.TransportPost.OrderBy(k => k.Name), "Id", "Name", targetTerritory.TransportPostId);
            ViewData["WaterSurfacePostId"] = new SelectList(_context.WaterSurfacePost.Include(w => w.WaterObject).OrderBy(k => k.WaterObjectName), "Id", "WaterObjectName", targetTerritory.WaterSurfacePostId);
            ViewData["KazHydrometWaterPostId"] = new SelectList(_context.KazHydrometWaterPost.OrderBy(k => k.Name), "Id", "Name", targetTerritory.KazHydrometWaterPostId);
            ViewData["KazHydrometSoilPostId"] = new SelectList(_context.KazHydrometSoilPost.OrderBy(k => k.Name), "Id", "Name", targetTerritory.KazHydrometSoilPostId);
            ViewData["SoilPostId"] = new SelectList(_context.SoilPost.OrderBy(k => k.Name), "Id", "Name", targetTerritory.SoilPostId);

            return View(targetTerritory);
        }

        // POST: TargetTerritories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TerritoryTypeId,CityDistrictId,TerritoryNameKK,TerritoryNameRU,GISConnectionCode,AdditionalInformationKK,AdditionalInformationRU,KazHydrometAirPostId,AirPostId,TransportPostId,WaterSurfacePostId,KazHydrometWaterPostId,KazHydrometSoilPostId,SoilPostId")] TargetTerritory targetTerritory)
        {
            if (id != targetTerritory.Id)
            {
                return NotFound();
            }
            TerritoryType targetTerritoryType = _context.TerritoryType.AsNoTracking().FirstOrDefault(t => t.Id == targetTerritory.TerritoryTypeId);
            var targetTerritories = _context.TargetTerritory.AsNoTracking().Include(t => t.TerritoryType).Include(t => t.CityDistrict).Where(t => t.Id != targetTerritory.Id).ToList();
            if (targetTerritoryType.NameRU == Startup.Configuration["OtherTerritoryType"])
            {
                if (string.IsNullOrWhiteSpace(targetTerritory.TerritoryNameKK))
                {
                    ModelState.AddModelError("TerritoryNameKK", _sharedLocalizer["ErrorNeedToInput"]);
                }
                if (string.IsNullOrWhiteSpace(targetTerritory.TerritoryNameRU))
                {
                    ModelState.AddModelError("TerritoryNameRU", _sharedLocalizer["ErrorNeedToInput"]);
                }
                if (targetTerritories.Select(t => t.TerritoryNameKK).Contains(targetTerritory.TerritoryNameKK))
                {
                    ModelState.AddModelError("TerritoryNameKK", _sharedLocalizer["ErrorDublicateValue"]);
                }
                if (targetTerritories.Select(t => t.TerritoryNameRU).Contains(targetTerritory.TerritoryNameRU))
                {
                    ModelState.AddModelError("TerritoryNameRU", _sharedLocalizer["ErrorDublicateValue"]);
                }
                if (targetTerritories.Select(t => t.GISConnectionCode).Contains(targetTerritory.GISConnectionCode))
                {
                    ModelState.AddModelError("GISConnectionCode", _sharedLocalizer["ErrorDublicateValue"]);
                }
                targetTerritory.CityDistrictId = null;
            }
            if (targetTerritoryType.NameRU == Startup.Configuration["City"])
            {
                targetTerritory.CityDistrictId = null;
                if (targetTerritories.Select(t => t.TerritoryType.NameRU).Contains(Startup.Configuration["City"]))
                {
                    ModelState.AddModelError("TerritoryTypeId", _sharedLocalizer["ErrorDublicateValue"]);
                }
            }
            if (targetTerritoryType.NameRU == Startup.Configuration["CityDistrict"])
            {
                if (targetTerritories.Select(t => t.CityDistrict != null ? t.CityDistrict.Id : 0).Contains((int)targetTerritory.CityDistrictId))
                {
                    ModelState.AddModelError("CityDistrictId", _sharedLocalizer["ErrorDublicateValue"]);
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var targetTerritory_old = _context.TargetTerritory.AsNoTracking().FirstOrDefault(t => t.Id == targetTerritory.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "TargetTerritory",
                        Operation = "Edit",
                        New = targetTerritory.ToString(),
                        Old = targetTerritory_old.ToString()
                    });
                    _context.Update(targetTerritory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TargetTerritoryExists(targetTerritory.Id))
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
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name", targetTerritory.CityDistrictId);
            ViewData["TerritoryTypeId"] = new SelectList(_context.TerritoryType.OrderBy(c => c.Name), "Id", "Name", targetTerritory.TerritoryTypeId);
            ViewData["CityDistrictCATO"] = new SelectList(_context.CityDistrict, "Id", "CATO");
            ViewData["CityDistrictNameKK"] = new SelectList(_context.CityDistrict, "Id", "NameKK");
            ViewData["CityDistrictNameRU"] = new SelectList(_context.CityDistrict, "Id", "NameRU");

            ViewData["City"] = _context.TerritoryType.FirstOrDefault(t => t.NameRU == Startup.Configuration["City"]).Id;
            ViewData["CityDistrict"] = _context.TerritoryType.FirstOrDefault(t => t.NameRU == Startup.Configuration["CityDistrict"]).Id;
            ViewData["OtherTerritoryType"] = _context.TerritoryType.FirstOrDefault(t => t.NameRU == Startup.Configuration["OtherTerritoryType"]).Id;
            ViewData["AlmatyCATO"] = Startup.Configuration["AlmatyCATO"];
            ViewData["AlmatyKK"] = Startup.Configuration["AlmatyKK"];
            ViewData["AlmatyRU"] = Startup.Configuration["AlmatyRU"];

            ViewData["KazHydrometAirPostId"] = new SelectList(_context.KazHydrometAirPost.OrderBy(k => k.Number), "Id", "Number", targetTerritory.KazHydrometAirPostId);
            ViewData["AirPostId"] = new SelectList(_context.AirPost.OrderBy(k => k.Name), "Id", "Name", targetTerritory.AirPostId);
            ViewData["TransportPostId"] = new SelectList(_context.TransportPost.OrderBy(k => k.Name), "Id", "Name", targetTerritory.TransportPostId);
            ViewData["WaterSurfacePostId"] = new SelectList(_context.WaterSurfacePost.Include(w => w.WaterObject).OrderBy(k => k.WaterObjectName), "Id", "WaterObjectName", targetTerritory.WaterSurfacePostId);
            ViewData["KazHydrometWaterPostId"] = new SelectList(_context.KazHydrometWaterPost.OrderBy(k => k.Name), "Id", "Name", targetTerritory.KazHydrometWaterPostId);
            ViewData["KazHydrometSoilPostId"] = new SelectList(_context.KazHydrometSoilPost.OrderBy(k => k.Name), "Id", "Name", targetTerritory.KazHydrometSoilPostId);
            ViewData["SoilPostId"] = new SelectList(_context.SoilPost.OrderBy(k => k.Name), "Id", "Name", targetTerritory.SoilPostId);

            return View(targetTerritory);
        }

        // GET: TargetTerritories/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var targetTerritory = await _context.TargetTerritory
                .Include(t => t.CityDistrict)
                .Include(t => t.TerritoryType)
                .Include(t => t.KazHydrometAirPost)
                .Include(t => t.AirPost)
                .Include(t => t.TransportPost)
                .Include(t => t.WaterSurfacePost)
                .Include(t => t.WaterSurfacePost.WaterObject)
                .Include(t => t.KazHydrometWaterPost)
                .Include(t => t.KazHydrometSoilPost)
                .Include(t => t.SoilPost)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (targetTerritory == null)
            {
                return NotFound();
            }
            ViewBag.TargetValues = _context.TargetValue
                .AsNoTracking()
                .Where(t => t.TargetTerritoryId == id);
            ViewBag.AActivities = _context.AActivity
                .AsNoTracking()
                .Where(a => a.TargetTerritoryId == id);
            return View(targetTerritory);
        }

        // POST: TargetTerritories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var targetTerritory = await _context.TargetTerritory.SingleOrDefaultAsync(m => m.Id == id);
            if ((_context.TargetValue
                .AsNoTracking()
                .FirstOrDefault(t => t.TargetTerritoryId == id) == null)
                && (_context.AActivity
                .AsNoTracking()
                .FirstOrDefault(a => a.TargetTerritoryId == id) == null))
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "TargetTerritory",
                    Operation = "Delete",
                    New = "",
                    Old = targetTerritory.ToString()
                });
                _context.TargetTerritory.Remove(targetTerritory);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TargetTerritoryExists(int id)
        {
            return _context.TargetTerritory.Any(e => e.Id == id);
        }
    }
}
