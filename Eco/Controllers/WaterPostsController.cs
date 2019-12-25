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
    public class WaterPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public WaterPostsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: WaterPosts
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string Number, string NameKK, string NameRU, int? Page)
        {
            var waterPosts = _context.WaterPost
                .Where(w => true);

            ViewBag.NumberFilter = Number;
            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NumberSort = SortOrder == "Number" ? "NumberDesc" : "Number";
            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(Number))
            {
                waterPosts = waterPosts.Where(w => w.Number.ToString().ToLower().Contains(Number.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameKK))
            {
                waterPosts = waterPosts.Where(w => w.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                waterPosts = waterPosts.Where(w => w.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "Number":
                    waterPosts = waterPosts.OrderBy(w => w.Number);
                    break;
                case "NumberDesc":
                    waterPosts = waterPosts.OrderByDescending(w => w.Number);
                    break;
                case "NameKK":
                    waterPosts = waterPosts.OrderBy(w => w.NameKK);
                    break;
                case "NameKKDesc":
                    waterPosts = waterPosts.OrderByDescending(w => w.NameKK);
                    break;
                case "NameRU":
                    waterPosts = waterPosts.OrderBy(w => w.NameRU);
                    break;
                case "NameRUDesc":
                    waterPosts = waterPosts.OrderByDescending(w => w.NameRU);
                    break;
                default:
                    waterPosts = waterPosts.OrderBy(w => w.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(waterPosts.Count(), Page);

            var viewModel = new WaterPostIndexPageViewModel
            {
                Items = waterPosts.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: WaterPosts/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterPost = await _context.WaterPost
                .SingleOrDefaultAsync(m => m.Id == id);
            if (waterPost == null)
            {
                return NotFound();
            }

            return View(waterPost);
        }

        // GET: WaterPosts/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: WaterPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Number,NameKK,NameRU,AdditionalInformationKK,AdditionalInformationRU,NorthLatitude,EastLongitude")] WaterPost waterPost)
        {
            var waterPosts = _context.WaterPost.AsNoTracking().ToList();
            if (waterPosts.Select(w => w.Number).Contains(waterPost.Number))
            {
                ModelState.AddModelError("Number", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (waterPosts.Select(w => w.NameKK).Contains(waterPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (waterPosts.Select(w => w.NameRU).Contains(waterPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(waterPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(waterPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(waterPost);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "WaterPost",
                    New = waterPost.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(waterPost);
        }

        // GET: WaterPosts/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterPost = await _context.WaterPost.SingleOrDefaultAsync(m => m.Id == id);
            if (waterPost == null)
            {
                return NotFound();
            }
            return View(waterPost);
        }

        // POST: WaterPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,NameKK,NameRU,AdditionalInformationKK,AdditionalInformationRU,NorthLatitude,EastLongitude")] WaterPost waterPost)
        {
            if (id != waterPost.Id)
            {
                return NotFound();
            }
            var waterPosts = _context.WaterPost.AsNoTracking().Where(w => w.Id != waterPost.Id).ToList();
            if (waterPosts.Select(w => w.Number).Contains(waterPost.Number))
            {
                ModelState.AddModelError("Number", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (waterPosts.Select(w => w.NameKK).Contains(waterPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (waterPosts.Select(w => w.NameRU).Contains(waterPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(waterPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(waterPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var waterPost_old = _context.LimitingIndicator.AsNoTracking().FirstOrDefault(w => w.Id == waterPost.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "WaterPost",
                        Operation = "Edit",
                        New = waterPost.ToString(),
                        Old = waterPost_old.ToString()
                    });
                    _context.Update(waterPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WaterPostExists(waterPost.Id))
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
            return View(waterPost);
        }

        // GET: WaterPosts/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterPost = await _context.WaterPost
                .SingleOrDefaultAsync(m => m.Id == id);
            if (waterPost == null)
            {
                return NotFound();
            }

            return View(waterPost);
        }

        // POST: WaterPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var waterPost = await _context.WaterPost.SingleOrDefaultAsync(m => m.Id == id);
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Class = "WaterPost",
                Operation = "Delete",
                New = "",
                Old = waterPost.ToString()
            });
            _context.WaterPost.Remove(waterPost);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WaterPostExists(int id)
        {
            return _context.WaterPost.Any(e => e.Id == id);
        }
    }
}
