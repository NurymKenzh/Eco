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
    public class WaterSurfacePostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public WaterSurfacePostsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: WaterSurfacePosts
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder,
            string WaterObjectName,
            string Number,
            int? Page)
        {
            var waterSurfacePosts = _context.WaterSurfacePost
                .Include(w => w.WaterObject)
                .Where(w => true);


            ViewBag.WaterObjectNameFilter = WaterObjectName;
            ViewBag.NumberFilter = Number;

            ViewBag.WaterObjectNameSort = SortOrder == "WaterObjectName" ? "WaterObjectNameDesc" : "WaterObjectName";
            ViewBag.NumberSort = SortOrder == "Number" ? "NumberDesc" : "Number";

            if (!string.IsNullOrEmpty(WaterObjectName))
            {
                waterSurfacePosts = waterSurfacePosts.Where(k => k.WaterObject.Name.ToLower().Contains(WaterObjectName.ToLower()));
            }
            if (!string.IsNullOrEmpty(Number))
            {
                waterSurfacePosts = waterSurfacePosts.Where(k => k.Number.ToString().ToLower().Contains(Number.ToLower()));
            }

            switch (SortOrder)
            {
                case "WaterObjectName":
                    waterSurfacePosts = waterSurfacePosts.OrderBy(k => k.WaterObject.Name);
                    break;
                case "WaterObjectNameDesc":
                    waterSurfacePosts = waterSurfacePosts.OrderByDescending(k => k.WaterObject.Name);
                    break;
                case "Number":
                    waterSurfacePosts = waterSurfacePosts.OrderBy(k => k.Number);
                    break;
                case "NumberDesc":
                    waterSurfacePosts = waterSurfacePosts.OrderByDescending(k => k.Number);
                    break;
                default:
                    waterSurfacePosts = waterSurfacePosts.OrderBy(k => k.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(waterSurfacePosts.Count(), Page);

            var viewModel = new WaterSurfacePostIndexPageViewModel
            {
                Items = waterSurfacePosts.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: WaterSurfacePosts/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterSurfacePost = await _context.WaterSurfacePost
                .Include(w => w.WaterObject)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (waterSurfacePost == null)
            {
                return NotFound();
            }

            return View(waterSurfacePost);
        }

        // GET: WaterSurfacePosts/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["WaterObjectId"] = new SelectList(_context.WaterObject.OrderBy(w => w.Name), "Id", "Name");
            return View();
        }

        // POST: WaterSurfacePosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,WaterObjectId,Number,AdditionalInformationKK,AdditionalInformationRU,NorthLatitude,EastLongitude")] WaterSurfacePost waterSurfacePost)
        {
            if (_context.WaterSurfacePost.AsNoTracking().FirstOrDefault(a => a.WaterObjectId == waterSurfacePost.WaterObjectId
                && a.Number == waterSurfacePost.Number) != null)
            {
                ModelState.AddModelError("", $"{_sharedLocalizer["ErrorDublicateValue"]} " +
                    $"({_sharedLocalizer["WaterObject"]}, " +
                    $"{_sharedLocalizer["Number"]})");
            }
            if (ModelState.IsValid)
            {
                _context.Add(waterSurfacePost);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "WaterSurfacePost",
                    New = waterSurfacePost.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["WaterObjectId"] = new SelectList(_context.WaterObject.OrderBy(w => w.Name), "Id", "Name", waterSurfacePost.WaterObjectId);
            return View(waterSurfacePost);
        }

        // GET: WaterSurfacePosts/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var waterSurfacePost = await _context.WaterSurfacePost.SingleOrDefaultAsync(m => m.Id == id);
            if (waterSurfacePost == null)
            {
                return NotFound();
            }
            ViewData["WaterObjectId"] = new SelectList(_context.WaterObject.OrderBy(w => w.Name), "Id", "Name", waterSurfacePost.WaterObjectId);
            return View(waterSurfacePost);
        }

        // POST: WaterSurfacePosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WaterObjectId,Number,AdditionalInformationKK,AdditionalInformationRU,NorthLatitude,EastLongitude")] WaterSurfacePost waterSurfacePost)
        {
            if (id != waterSurfacePost.Id)
            {
                return NotFound();
            }
            if (_context.WaterSurfacePost.AsNoTracking().FirstOrDefault(a => a.Id != id
                && a.WaterObjectId == waterSurfacePost.WaterObjectId
                && a.Number == waterSurfacePost.Number) != null)
            {
                ModelState.AddModelError("", $"{_sharedLocalizer["ErrorDublicateValue"]} " +
                    $"({_sharedLocalizer["WaterObject"]}, " +
                    $"{_sharedLocalizer["Number"]})");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var waterSurfacePost_old = _context.WaterSurfacePost.AsNoTracking().FirstOrDefault(k => k.Id == waterSurfacePost.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "WaterSurfacePost",
                        Operation = "Edit",
                        New = waterSurfacePost.ToString(),
                        Old = waterSurfacePost_old.ToString()
                    });
                    _context.Update(waterSurfacePost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WaterSurfacePostExists(waterSurfacePost.Id))
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
            ViewData["WaterObjectId"] = new SelectList(_context.WaterObject.OrderBy(w => w.Name), "Id", "Name", waterSurfacePost.WaterObjectId);
            return View(waterSurfacePost);
        }

        // GET: WaterSurfacePosts/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterSurfacePost = await _context.WaterSurfacePost
                .Include(w => w.WaterObject)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (waterSurfacePost == null)
            {
                return NotFound();
            }
            ViewBag.WaterSurfacePostDatas = _context.WaterSurfacePostData
                .AsNoTracking()
                .Where(a => a.WaterSurfacePostId == id)
                .Take(50)
                .OrderBy(a => a.DateOfSampling);
            ViewBag.TargetTerritories = _context.TargetTerritory
                .AsNoTracking()
                .Where(k => k.WaterSurfacePostId == id)
                .OrderBy(k => k.TerritoryName);
            return View(waterSurfacePost);
        }

        // POST: WaterSurfacePosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var waterSurfacePost = await _context.WaterSurfacePost.SingleOrDefaultAsync(m => m.Id == id);
            if ((_context.WaterSurfacePostData
                .AsNoTracking()
                .FirstOrDefault(a => a.WaterSurfacePostId == id) == null)
                &&(_context.TargetTerritory
                .AsNoTracking()
                .FirstOrDefault(k => k.WaterSurfacePostId == id) == null))
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "WaterSurfacePost",
                    Operation = "Delete",
                    New = "",
                    Old = waterSurfacePost.ToString()
                });
                _context.WaterSurfacePost.Remove(waterSurfacePost);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool WaterSurfacePostExists(int id)
        {
            return _context.WaterSurfacePost.Any(e => e.Id == id);
        }
    }
}
