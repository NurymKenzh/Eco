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

namespace Eco.Controllers
{
    public class TreesByFacilityManagementMeasuresListsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TreesByFacilityManagementMeasuresListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TreesByFacilityManagementMeasuresLists
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            int? GreemPlantsPassportId,
            int? PlantationsTypeId,
            string Quantity,
            int? Page)
        {
            var treesByFacilityManagementMeasuresLists = _context.TreesByFacilityManagementMeasuresList
                .Include(t => t.GreemPlantsPassport)
                .Include(t => t.PlantationsType)
                .Where(t => true);

            ViewBag.GreemPlantsPassportIdFilter = GreemPlantsPassportId;
            ViewBag.PlantationsTypeIdFilter = PlantationsTypeId;

            ViewBag.GreemPlantsPassportIdSort = SortOrder == "GreemPlantsPassportId" ? "GreemPlantsPassportIdDesc" : "GreemPlantsPassportId";
            ViewBag.PlantationsTypeIdSort = SortOrder == "PlantationsTypeId" ? "PlantationsTypeIdDesc" : "PlantationsTypeId";
            ViewBag.QuantitySort = SortOrder == "Quantity" ? "QuantityDesc" : "Quantity";

            if (GreemPlantsPassportId != null)
            {
                treesByFacilityManagementMeasuresLists = treesByFacilityManagementMeasuresLists.Where(a => a.GreemPlantsPassportId == GreemPlantsPassportId);
            }
            if (PlantationsTypeId != null)
            {
                treesByFacilityManagementMeasuresLists = treesByFacilityManagementMeasuresLists.Where(a => a.PlantationsTypeId == PlantationsTypeId);
            }

            switch (SortOrder)
            {
                case "GreemPlantsPassportId":
                    treesByFacilityManagementMeasuresLists = treesByFacilityManagementMeasuresLists.OrderBy(a => a.GreemPlantsPassport.GreenObject);
                    break;
                case "GreemPlantsPassportIdDesc":
                    treesByFacilityManagementMeasuresLists = treesByFacilityManagementMeasuresLists.OrderByDescending(a => a.GreemPlantsPassport.GreenObject);
                    break;
                case "PlantationsTypeId":
                    treesByFacilityManagementMeasuresLists = treesByFacilityManagementMeasuresLists.OrderBy(a => a.PlantationsType.Name);
                    break;
                case "PlantationsTypeIdDesc":
                    treesByFacilityManagementMeasuresLists = treesByFacilityManagementMeasuresLists.OrderByDescending(a => a.PlantationsType.Name);
                    break;
                case "Quantity":
                    treesByFacilityManagementMeasuresLists = treesByFacilityManagementMeasuresLists.OrderBy(a => a.Quantity);
                    break;
                case "QuantityDesc":
                    treesByFacilityManagementMeasuresLists = treesByFacilityManagementMeasuresLists.OrderByDescending(a => a.Quantity);
                    break;
                default:
                    treesByFacilityManagementMeasuresLists = treesByFacilityManagementMeasuresLists.OrderBy(a => a.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(treesByFacilityManagementMeasuresLists.Count(), Page);

            var viewModel = new TreesByFacilityManagementMeasuresListIndexPageViewModel
            {
                Items = treesByFacilityManagementMeasuresLists.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewBag.GreemPlantsPassportId = new SelectList(_context.GreemPlantsPassport.OrderBy(a => a.GreenObject), "Id", "GreenObject");
            ViewBag.PlantationsTypeId = new SelectList(_context.PlantationsType.OrderBy(a => a.Name), "Id", "Name");

            return View(viewModel);
        }

        // GET: TreesByFacilityManagementMeasuresLists/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treesByFacilityManagementMeasuresList = await _context.TreesByFacilityManagementMeasuresList
                .Include(t => t.BusinessEventsPlantationsType)
                .Include(t => t.GreemPlantsPassport)
                .Include(t => t.PlantationsType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (treesByFacilityManagementMeasuresList == null)
            {
                return NotFound();
            }

            return View(treesByFacilityManagementMeasuresList);
        }

        // GET: TreesByFacilityManagementMeasuresLists/Create
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public IActionResult Create()
        {
            TreesByFacilityManagementMeasuresList model = new TreesByFacilityManagementMeasuresList();
            ViewData["BusinessEventsPlantationsTypeId"] = new SelectList(_context.PlantationsType.OrderBy(b => b.Name), "Id", "Name");
            ViewData["GreemPlantsPassportId"] = new SelectList(_context.GreemPlantsPassport.OrderBy(g => g.GreenObject), "Id", "GreenObject");
            ViewData["PlantationsTypeId"] = new SelectList(_context.PlantationsType.OrderBy(b => b.Name), "Id", "Name");
            return View(model);
        }

        // POST: TreesByFacilityManagementMeasuresLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Create([Bind("Id,GreemPlantsPassportId,PlantationsTypeId,BusinessEventsPlantationsTypeId,SanitaryPruning,CrownFormation,SanitaryFelling,MaintenanceWork,Quantity,BusinessEvents")] TreesByFacilityManagementMeasuresList treesByFacilityManagementMeasuresList)
        {
            if (ModelState.IsValid)
            {
                if (treesByFacilityManagementMeasuresList.BusinessEvents)
                {
                    treesByFacilityManagementMeasuresList.BusinessEventsPlantationsTypeId = null;
                }
                else
                {
                    treesByFacilityManagementMeasuresList.SanitaryPruning = null;
                    treesByFacilityManagementMeasuresList.CrownFormation = null;
                    treesByFacilityManagementMeasuresList.SanitaryFelling = null;
                }
                _context.Add(treesByFacilityManagementMeasuresList);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "TreesByFacilityManagementMeasuresList",
                    New = treesByFacilityManagementMeasuresList.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BusinessEventsPlantationsTypeId"] = new SelectList(_context.PlantationsType.OrderBy(b => b.Name), "Id", "Name", treesByFacilityManagementMeasuresList.BusinessEventsPlantationsTypeId);
            ViewData["GreemPlantsPassportId"] = new SelectList(_context.GreemPlantsPassport.OrderBy(g => g.GreenObject), "Id", "GreenObject", treesByFacilityManagementMeasuresList.GreemPlantsPassportId);
            ViewData["PlantationsTypeId"] = new SelectList(_context.PlantationsType.OrderBy(b => b.Name), "Id", "Name", treesByFacilityManagementMeasuresList.PlantationsTypeId);
            return View(treesByFacilityManagementMeasuresList);
        }

        // GET: TreesByFacilityManagementMeasuresLists/Edit/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treesByFacilityManagementMeasuresList = await _context.TreesByFacilityManagementMeasuresList.SingleOrDefaultAsync(m => m.Id == id);
            if (treesByFacilityManagementMeasuresList == null)
            {
                return NotFound();
            }
            
            if (treesByFacilityManagementMeasuresList.BusinessEventsPlantationsTypeId == null)
            {
                treesByFacilityManagementMeasuresList.BusinessEvents = true;
            }
            else
            {
                treesByFacilityManagementMeasuresList.BusinessEvents = false;
            }

            ViewData["BusinessEventsPlantationsTypeId"] = new SelectList(_context.PlantationsType.OrderBy(b => b.Name), "Id", "Name", treesByFacilityManagementMeasuresList.BusinessEventsPlantationsTypeId);
            ViewData["GreemPlantsPassportId"] = new SelectList(_context.GreemPlantsPassport.OrderBy(g => g.GreenObject), "Id", "GreenObject", treesByFacilityManagementMeasuresList.GreemPlantsPassportId);
            ViewData["PlantationsTypeId"] = new SelectList(_context.PlantationsType.OrderBy(b => b.Name), "Id", "Name", treesByFacilityManagementMeasuresList.PlantationsTypeId);
            return View(treesByFacilityManagementMeasuresList);
        }

        // POST: TreesByFacilityManagementMeasuresLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GreemPlantsPassportId,PlantationsTypeId,BusinessEventsPlantationsTypeId,SanitaryPruning,CrownFormation,SanitaryFelling,MaintenanceWork,Quantity,BusinessEvents")] TreesByFacilityManagementMeasuresList treesByFacilityManagementMeasuresList)
        {
            if (id != treesByFacilityManagementMeasuresList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (treesByFacilityManagementMeasuresList.BusinessEvents)
                {
                    treesByFacilityManagementMeasuresList.BusinessEventsPlantationsTypeId = null;
                }
                else
                {
                    treesByFacilityManagementMeasuresList.SanitaryPruning = null;
                    treesByFacilityManagementMeasuresList.CrownFormation = null;
                    treesByFacilityManagementMeasuresList.SanitaryFelling = null;
                }
                try
                {
                    var treesByFacilityManagementMeasuresList_old = _context.TreesByFacilityManagementMeasuresList.AsNoTracking().FirstOrDefault(a => a.Id == treesByFacilityManagementMeasuresList.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "TreesByFacilityManagementMeasuresList",
                        Operation = "Edit",
                        New = treesByFacilityManagementMeasuresList.ToString(),
                        Old = treesByFacilityManagementMeasuresList_old.ToString()
                    });
                    _context.Update(treesByFacilityManagementMeasuresList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TreesByFacilityManagementMeasuresListExists(treesByFacilityManagementMeasuresList.Id))
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
            ViewData["BusinessEventsPlantationsTypeId"] = new SelectList(_context.PlantationsType, "Id", "Id", treesByFacilityManagementMeasuresList.BusinessEventsPlantationsTypeId);
            ViewData["GreemPlantsPassportId"] = new SelectList(_context.GreemPlantsPassport.OrderBy(g => g.GreenObject), "Id", "GreenObject", treesByFacilityManagementMeasuresList.GreemPlantsPassportId);
            ViewData["PlantationsTypeId"] = new SelectList(_context.PlantationsType.OrderBy(b => b.Name), "Id", "Name", treesByFacilityManagementMeasuresList.PlantationsTypeId);
            return View(treesByFacilityManagementMeasuresList);
        }

        // GET: TreesByFacilityManagementMeasuresLists/Delete/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treesByFacilityManagementMeasuresList = await _context.TreesByFacilityManagementMeasuresList
                .Include(t => t.BusinessEventsPlantationsType)
                .Include(t => t.GreemPlantsPassport)
                .Include(t => t.PlantationsType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (treesByFacilityManagementMeasuresList == null)
            {
                return NotFound();
            }

            return View(treesByFacilityManagementMeasuresList);
        }

        // POST: TreesByFacilityManagementMeasuresLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var treesByFacilityManagementMeasuresList = await _context.TreesByFacilityManagementMeasuresList.SingleOrDefaultAsync(m => m.Id == id);
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Class = "TreesByFacilityManagementMeasuresList",
                Operation = "Delete",
                New = "",
                Old = treesByFacilityManagementMeasuresList.ToString()
            });
            _context.TreesByFacilityManagementMeasuresList.Remove(treesByFacilityManagementMeasuresList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TreesByFacilityManagementMeasuresListExists(int id)
        {
            return _context.TreesByFacilityManagementMeasuresList.Any(e => e.Id == id);
        }
    }
}
