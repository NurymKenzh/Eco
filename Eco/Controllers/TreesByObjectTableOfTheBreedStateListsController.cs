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
    public class TreesByObjectTableOfTheBreedStateListsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TreesByObjectTableOfTheBreedStateListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TreesByObjectTableOfTheBreedStateLists
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            int? GreemPlantsPassportId,
            int? PlantationsTypeId,
            string Quantity,
            int? Page)
        {
            var treesByObjectTableOfTheBreedStateLists = _context.TreesByObjectTableOfTheBreedStateList
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
                treesByObjectTableOfTheBreedStateLists = treesByObjectTableOfTheBreedStateLists.Where(a => a.GreemPlantsPassportId == GreemPlantsPassportId);
            }
            if (PlantationsTypeId != null)
            {
                treesByObjectTableOfTheBreedStateLists = treesByObjectTableOfTheBreedStateLists.Where(a => a.PlantationsTypeId == PlantationsTypeId);
            }

            switch(SortOrder)
            {
                case "GreemPlantsPassportId":
                    treesByObjectTableOfTheBreedStateLists = treesByObjectTableOfTheBreedStateLists.OrderBy(a => a.GreemPlantsPassport.GreenObject);
                    break;
                case "GreemPlantsPassportIdDesc":
                    treesByObjectTableOfTheBreedStateLists = treesByObjectTableOfTheBreedStateLists.OrderByDescending(a => a.GreemPlantsPassport.GreenObject);
                    break;
                case "PlantationsTypeId":
                    treesByObjectTableOfTheBreedStateLists = treesByObjectTableOfTheBreedStateLists.OrderBy(a => a.PlantationsType.Name);
                    break;
                case "PlantationsTypeIdDesc":
                    treesByObjectTableOfTheBreedStateLists = treesByObjectTableOfTheBreedStateLists.OrderByDescending(a => a.PlantationsType.Name);
                    break;
                case "Quantity":
                    treesByObjectTableOfTheBreedStateLists = treesByObjectTableOfTheBreedStateLists.OrderBy(a => a.Quantity);
                    break;
                case "QuantityDesc":
                    treesByObjectTableOfTheBreedStateLists = treesByObjectTableOfTheBreedStateLists.OrderByDescending(a => a.Quantity);
                    break;
                default:
                    treesByObjectTableOfTheBreedStateLists = treesByObjectTableOfTheBreedStateLists.OrderBy(a => a.Id);
                    break;
            }

            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(treesByObjectTableOfTheBreedStateLists.Count(), Page);

            var viewModel = new TreesByObjectTableOfTheBreedStateListIndexPageViewModel
            {
                Items = treesByObjectTableOfTheBreedStateLists.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewBag.GreemPlantsPassportId = new SelectList(_context.GreemPlantsPassport.OrderBy(a => a.GreenObject), "Id", "GreenObject");
            ViewBag.PlantationsTypeId = new SelectList(_context.PlantationsType.OrderBy(a => a.Name), "Id", "Name");

            return View(viewModel);
        }

        // GET: TreesByObjectTableOfTheBreedStateLists/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treesByObjectTableOfTheBreedStateList = await _context.TreesByObjectTableOfTheBreedStateList
                .Include(t => t.GreemPlantsPassport)
                .Include(t => t.PlantationsType)
                .Include(t => t.StateOfCSR15PlantationsType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (treesByObjectTableOfTheBreedStateList == null)
            {
                return NotFound();
            }

            return View(treesByObjectTableOfTheBreedStateList);
        }

        // GET: TreesByObjectTableOfTheBreedStateLists/Create
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public IActionResult Create()
        {
            TreesByObjectTableOfTheBreedStateList model = new TreesByObjectTableOfTheBreedStateList();
            ViewData["GreemPlantsPassportId"] = new SelectList(_context.GreemPlantsPassport.OrderBy(g => g.GreenObject), "Id", "GreenObject");
            ViewData["PlantationsTypeId"] = new SelectList(_context.PlantationsType.OrderBy(p => p.Name), "Id", "Name");
            ViewData["StateOfCSR15PlantationsTypeId"] = new SelectList(_context.PlantationsType.OrderBy(p => p.Name), "Id", "Name");
            return View(model);
        }

        // POST: TreesByObjectTableOfTheBreedStateLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Create([Bind("Id,GreemPlantsPassportId,PlantationsTypeId,StateOfCSR15PlantationsTypeId,StateOfCSR15_1,StateOfCSR15_2,StateOfCSR15_3,StateOfCSR15_4,StateOfCSR15_5,Quantity,StateOfCSR15Type")] TreesByObjectTableOfTheBreedStateList treesByObjectTableOfTheBreedStateList)
        {            
            if (ModelState.IsValid)
            {
                if(treesByObjectTableOfTheBreedStateList.StateOfCSR15Type)
                {
                    treesByObjectTableOfTheBreedStateList.StateOfCSR15PlantationsTypeId = null;
                }
                else
                {
                    treesByObjectTableOfTheBreedStateList.StateOfCSR15_1 = null;
                    treesByObjectTableOfTheBreedStateList.StateOfCSR15_2 = null;
                    treesByObjectTableOfTheBreedStateList.StateOfCSR15_3 = null;
                    treesByObjectTableOfTheBreedStateList.StateOfCSR15_4 = null;
                    treesByObjectTableOfTheBreedStateList.StateOfCSR15_5 = null;
                }
                _context.Add(treesByObjectTableOfTheBreedStateList);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "TreesByObjectTableOfTheBreedStateList",
                    New = treesByObjectTableOfTheBreedStateList.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GreemPlantsPassportId"] = new SelectList(_context.GreemPlantsPassport.OrderBy(g => g.GreenObject), "Id", "GreenObject", treesByObjectTableOfTheBreedStateList.GreemPlantsPassportId);
            ViewData["PlantationsTypeId"] = new SelectList(_context.PlantationsType.OrderBy(p => p.Name), "Id", "Name", treesByObjectTableOfTheBreedStateList.PlantationsTypeId);
            ViewData["StateOfCSR15PlantationsTypeId"] = new SelectList(_context.PlantationsType.OrderBy(p => p.Name), "Id", "Name", treesByObjectTableOfTheBreedStateList.StateOfCSR15PlantationsTypeId);
            return View(treesByObjectTableOfTheBreedStateList);
        }

        // GET: TreesByObjectTableOfTheBreedStateLists/Edit/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treesByObjectTableOfTheBreedStateList = await _context.TreesByObjectTableOfTheBreedStateList.SingleOrDefaultAsync(m => m.Id == id);
            if (treesByObjectTableOfTheBreedStateList == null)
            {
                return NotFound();
            }

            if (treesByObjectTableOfTheBreedStateList.StateOfCSR15PlantationsTypeId == null)
            {
                treesByObjectTableOfTheBreedStateList.StateOfCSR15Type = true;
            }
            else
            {
                treesByObjectTableOfTheBreedStateList.StateOfCSR15Type = false;
            }

            ViewData["GreemPlantsPassportId"] = new SelectList(_context.GreemPlantsPassport.OrderBy(g => g.GreenObject), "Id", "GreenObject", treesByObjectTableOfTheBreedStateList.GreemPlantsPassportId);
            ViewData["PlantationsTypeId"] = new SelectList(_context.PlantationsType.OrderBy(p => p.Name), "Id", "Name", treesByObjectTableOfTheBreedStateList.PlantationsTypeId);
            ViewData["StateOfCSR15PlantationsTypeId"] = new SelectList(_context.PlantationsType.OrderBy(p => p.Name), "Id", "Name", treesByObjectTableOfTheBreedStateList.StateOfCSR15PlantationsTypeId);
            return View(treesByObjectTableOfTheBreedStateList);
        }

        // POST: TreesByObjectTableOfTheBreedStateLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GreemPlantsPassportId,PlantationsTypeId,StateOfCSR15PlantationsTypeId,StateOfCSR15_1,StateOfCSR15_2,StateOfCSR15_3,StateOfCSR15_4,StateOfCSR15_5,Quantity,StateOfCSR15Type")] TreesByObjectTableOfTheBreedStateList treesByObjectTableOfTheBreedStateList)
        {
            if (id != treesByObjectTableOfTheBreedStateList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (treesByObjectTableOfTheBreedStateList.StateOfCSR15Type)
                {
                    treesByObjectTableOfTheBreedStateList.StateOfCSR15PlantationsTypeId = null;
                }
                else
                {
                    treesByObjectTableOfTheBreedStateList.StateOfCSR15_1 = null;
                    treesByObjectTableOfTheBreedStateList.StateOfCSR15_2 = null;
                    treesByObjectTableOfTheBreedStateList.StateOfCSR15_3 = null;
                    treesByObjectTableOfTheBreedStateList.StateOfCSR15_4 = null;
                    treesByObjectTableOfTheBreedStateList.StateOfCSR15_5 = null;
                }
                try
                {
                    var treesByObjectTableOfTheBreedStateList_old = _context.TreesByObjectTableOfTheBreedStateList.AsNoTracking().FirstOrDefault(a => a.Id == treesByObjectTableOfTheBreedStateList.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "TreesByObjectTableOfTheBreedStateList",
                        Operation = "Edit",
                        New = treesByObjectTableOfTheBreedStateList.ToString(),
                        Old = treesByObjectTableOfTheBreedStateList_old.ToString()
                    });
                    _context.Update(treesByObjectTableOfTheBreedStateList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TreesByObjectTableOfTheBreedStateListExists(treesByObjectTableOfTheBreedStateList.Id))
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
            ViewData["GreemPlantsPassportId"] = new SelectList(_context.GreemPlantsPassport.OrderBy(g => g.GreenObject), "Id", "GreenObject", treesByObjectTableOfTheBreedStateList.GreemPlantsPassportId);
            ViewData["PlantationsTypeId"] = new SelectList(_context.PlantationsType.OrderBy(p => p.Name), "Id", "Name", treesByObjectTableOfTheBreedStateList.PlantationsTypeId);
            ViewData["StateOfCSR15PlantationsTypeId"] = new SelectList(_context.PlantationsType.OrderBy(p => p.Name), "Id", "Name", treesByObjectTableOfTheBreedStateList.StateOfCSR15PlantationsTypeId);
            return View(treesByObjectTableOfTheBreedStateList);
        }

        // GET: TreesByObjectTableOfTheBreedStateLists/Delete/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treesByObjectTableOfTheBreedStateList = await _context.TreesByObjectTableOfTheBreedStateList
                .Include(t => t.GreemPlantsPassport)
                .Include(t => t.PlantationsType)
                .Include(t => t.StateOfCSR15PlantationsType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (treesByObjectTableOfTheBreedStateList == null)
            {
                return NotFound();
            }

            return View(treesByObjectTableOfTheBreedStateList);
        }

        // POST: TreesByObjectTableOfTheBreedStateLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var treesByObjectTableOfTheBreedStateList = await _context.TreesByObjectTableOfTheBreedStateList.SingleOrDefaultAsync(m => m.Id == id);
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Class = "TreesByObjectTableOfTheBreedStateList",
                Operation = "Delete",
                New = "",
                Old = treesByObjectTableOfTheBreedStateList.ToString()
            });
            _context.TreesByObjectTableOfTheBreedStateList.Remove(treesByObjectTableOfTheBreedStateList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TreesByObjectTableOfTheBreedStateListExists(int id)
        {
            return _context.TreesByObjectTableOfTheBreedStateList.Any(e => e.Id == id);
        }
    }
}
