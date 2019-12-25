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
    public class GreemPlantsPassportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public GreemPlantsPassportsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: GreemPlantsPassports
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            string GreenObject,
            int? CityDistrictId,
            int? Page)
        {
            var greemPlantsPassports = _context.GreemPlantsPassport
                .Include(g => g.CityDistrict)
                .Where(g => true);

            ViewBag.GreenObjectFilter = GreenObject;
            ViewBag.CityDistrictIdFilter = CityDistrictId;

            ViewBag.GreenObjectSort = SortOrder == "GreenObject" ? "GreenObjectDesc" : "GreenObject";
            ViewBag.CityDistrictIdSort = SortOrder == "CityDistrictId" ? "CityDistrictIdDesc" : "CityDistrictId";

            if(!string.IsNullOrEmpty(GreenObject))
            {
                greemPlantsPassports = greemPlantsPassports.Where(g => g.GreenObject.ToLower().Contains(GreenObject.ToLower()));
            }
            if(CityDistrictId!=null)
            {
                greemPlantsPassports = greemPlantsPassports.Where(g => g.CityDistrictId == CityDistrictId);
            }

            switch (SortOrder)
            {
                case "GreenObject":
                    greemPlantsPassports = greemPlantsPassports.OrderBy(g => g.GreenObject);
                    break;
                case "GreenObjectDesc":
                    greemPlantsPassports = greemPlantsPassports.OrderByDescending(g => g.GreenObject);
                    break;
                case "CityDistrictId":
                    greemPlantsPassports = greemPlantsPassports.OrderBy(g => g.CityDistrict.Name);
                    break;
                case "CityDistrictIdDesc":
                    greemPlantsPassports = greemPlantsPassports.OrderByDescending(g => g.CityDistrict.Id);
                    break;
                default:
                    greemPlantsPassports = greemPlantsPassports.OrderBy(g => g.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(greemPlantsPassports.Count(), Page);

            var viewModel = new GreemPlantsPassportIndexPageViewModel
            {
                Items = greemPlantsPassports.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name");

            return View(viewModel);
        }

        // GET: GreemPlantsPassports/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var greemPlantsPassport = await _context.GreemPlantsPassport
                .Include(g => g.CityDistrict)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (greemPlantsPassport == null)
            {
                return NotFound();
            }

            return View(greemPlantsPassport);
        }

        // GET: GreemPlantsPassports/Create
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public IActionResult Create()
        {
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name");
            return View();
        }

        // POST: GreemPlantsPassports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Create([Bind("Id,GreenObject,CityDistrictId,NameOfPowersAttributed,NameOfRegistrationObject,LegalEntityUse,AccountNumber,NameAndLocation,PresenceOfHistoricalObject,GreenTotalAreaGa,Lawns,Flowerbeds,TracksAndPlatforms,Tree,Shrubs,SofasAndBenches,Urns,EquippedPlaygrounds,EquippedSportsgrounds,Monument,Toilets,OutdoorLighting,Billboards,OtherCapitalStructures,GreenTotalArea,AreaUnderGreenery,AreaUnderLawn,AreaUnderGroundlawn,AreaUnderOrdinarylawn,AreaUnderMeadowlawn,AreaUnderTrees,AreaUnderShrubs,AreaUndeFlowerbeds,AreaUndeTracksAndPlatforms,Asphalted,PavingBlocks,LengthOfTrays,AmountConiferousTrees,ListOfTreesConiferous,Upto10yearsConiferous,Betwen10_20yearsConiferous,Over10yearsConiferous,AmountDeciduousTrees,ListOfTreesDeciduous,Upto10yearsDeciduous,Betwen10_20yearsDeciduous,Over10yearsDeciduous,AmountFormedTrees,TotallAmountShrubs,AmountShrubs,LengthOfHedges,AmountEquippedPlaygrounds,AmountEquippedSportsgrounds,AmountSofasAndBenches,AmountBenches,AmountSofas,AmountArbours,AmountOutdoorLighting,AmountToilets,AmountMonument,AmountBillboards,ListOfTreesByObjectBreedsCondition,ListOfTreesByObjectEconomicMeasures,PassportGeneralInformation,NorthLatitude,EastLongitude")] GreemPlantsPassport greemPlantsPassport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(greemPlantsPassport);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "GreemPlantsPassport",
                    New = greemPlantsPassport.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name", greemPlantsPassport.CityDistrictId);
            return View(greemPlantsPassport);
        }

        // GET: GreemPlantsPassports/Edit/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var greemPlantsPassport = await _context.GreemPlantsPassport.SingleOrDefaultAsync(m => m.Id == id);
            if (greemPlantsPassport == null)
            {
                return NotFound();
            }
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name", greemPlantsPassport.CityDistrictId);
            return View(greemPlantsPassport);
        }

        // POST: GreemPlantsPassports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GreenObject,CityDistrictId,NameOfPowersAttributed,NameOfRegistrationObject,LegalEntityUse,AccountNumber,NameAndLocation,PresenceOfHistoricalObject,GreenTotalAreaGa,Lawns,Flowerbeds,TracksAndPlatforms,Tree,Shrubs,SofasAndBenches,Urns,EquippedPlaygrounds,EquippedSportsgrounds,Monument,Toilets,OutdoorLighting,Billboards,OtherCapitalStructures,GreenTotalArea,AreaUnderGreenery,AreaUnderLawn,AreaUnderGroundlawn,AreaUnderOrdinarylawn,AreaUnderMeadowlawn,AreaUnderTrees,AreaUnderShrubs,AreaUndeFlowerbeds,AreaUndeTracksAndPlatforms,Asphalted,PavingBlocks,LengthOfTrays,AmountConiferousTrees,ListOfTreesConiferous,Upto10yearsConiferous,Betwen10_20yearsConiferous,Over10yearsConiferous,AmountDeciduousTrees,ListOfTreesDeciduous,Upto10yearsDeciduous,Betwen10_20yearsDeciduous,Over10yearsDeciduous,AmountFormedTrees,TotallAmountShrubs,AmountShrubs,LengthOfHedges,AmountEquippedPlaygrounds,AmountEquippedSportsgrounds,AmountSofasAndBenches,AmountBenches,AmountSofas,AmountArbours,AmountOutdoorLighting,AmountToilets,AmountMonument,AmountBillboards,ListOfTreesByObjectBreedsCondition,ListOfTreesByObjectEconomicMeasures,PassportGeneralInformation,NorthLatitude,EastLongitude")] GreemPlantsPassport greemPlantsPassport)
        {
            if (id != greemPlantsPassport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var greemPlantsPassport_old = _context.GreemPlantsPassport.AsNoTracking().FirstOrDefault(a => a.Id == greemPlantsPassport.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "GreemPlantsPassport",
                        Operation = "Edit",
                        New = greemPlantsPassport.ToString(),
                        Old = greemPlantsPassport_old.ToString()
                    });
                    _context.Update(greemPlantsPassport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GreemPlantsPassportExists(greemPlantsPassport.Id))
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
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict.OrderBy(c => c.Name), "Id", "Name", greemPlantsPassport.CityDistrictId);
            return View(greemPlantsPassport);
        }

        // GET: GreemPlantsPassports/Delete/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var greemPlantsPassport = await _context.GreemPlantsPassport
                .Include(g => g.CityDistrict)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (greemPlantsPassport == null)
            {
                return NotFound();
            }

            return View(greemPlantsPassport);
        }

        // POST: GreemPlantsPassports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var greemPlantsPassport = await _context.GreemPlantsPassport.SingleOrDefaultAsync(m => m.Id == id);
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Class = "GreemPlantsPassport",
                Operation = "Delete",
                New = "",
                Old = greemPlantsPassport.ToString()
            });
            _context.GreemPlantsPassport.Remove(greemPlantsPassport);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GreemPlantsPassportExists(int id)
        {
            return _context.GreemPlantsPassport.Any(e => e.Id == id);
        }
    }
}
