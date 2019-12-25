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
    public class KazHydrometSoilPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public KazHydrometSoilPostsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: KazHydrometSoilPosts
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string Number, string NameKK, string NameRU, int? Page)
        {
            var kazHydrometSoilPosts = _context.KazHydrometSoilPost
                .Where(k => true);

            ViewBag.NumberFilter = Number;
            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NumberSort = SortOrder == "Number" ? "NumberDesc" : "Number";
            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(Number))
            {
                kazHydrometSoilPosts = kazHydrometSoilPosts.Where(k => k.Number.ToString()==Number);
            }
            if (!string.IsNullOrEmpty(NameKK))
            {
                kazHydrometSoilPosts = kazHydrometSoilPosts.Where(k => k.NameKK==NameKK);
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                kazHydrometSoilPosts = kazHydrometSoilPosts.Where(k => k.NameRU==NameRU);
            }

            switch (SortOrder)
            {
                case "Number":
                    kazHydrometSoilPosts = kazHydrometSoilPosts.OrderBy(k => k.Number);
                    break;
                case "NumberDesc":
                    kazHydrometSoilPosts = kazHydrometSoilPosts.OrderByDescending(k => k.Number);
                    break;
                case "NameKK":
                    kazHydrometSoilPosts = kazHydrometSoilPosts.OrderBy(k => k.NameKK);
                    break;
                case "NameKKDesc":
                    kazHydrometSoilPosts = kazHydrometSoilPosts.OrderByDescending(k => k.NameKK);
                    break;
                case "NameRU":
                    kazHydrometSoilPosts = kazHydrometSoilPosts.OrderBy(k => k.NameRU);
                    break;
                case "NameRUDesc":
                    kazHydrometSoilPosts = kazHydrometSoilPosts.OrderByDescending(k => k.NameRU);
                    break;
                default:
                    kazHydrometSoilPosts = kazHydrometSoilPosts.OrderBy(k => k.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(kazHydrometSoilPosts.Count(), Page);

            var viewModel = new KazHydrometSoilPostIndexPageViewModel
            {
                Items = kazHydrometSoilPosts.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewData["Number"] = new SelectList(_context.KazHydrometSoilPost.OrderBy(a => a.Number).GroupBy(k => k.Number).Select(g => g.First()), "Number", "Number");
            ViewData["NameKK"] = new SelectList(_context.KazHydrometSoilPost.OrderBy(a => a.NameKK).GroupBy(k => k.NameKK).Select(g => g.First()), "NameKK", "NameKK");
            ViewData["NameRU"] = new SelectList(_context.KazHydrometSoilPost.OrderBy(a => a.NameRU).GroupBy(k => k.NameRU).Select(g => g.First()), "NameRU", "NameRU");

            return View(viewModel);
        }

        // GET: KazHydrometSoilPosts/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kazHydrometSoilPost = await _context.KazHydrometSoilPost
                .SingleOrDefaultAsync(m => m.Id == id);
            if (kazHydrometSoilPost == null)
            {
                return NotFound();
            }

            return View(kazHydrometSoilPost);
        }

        // GET: KazHydrometSoilPosts/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: KazHydrometSoilPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Number,NameKK,NameRU,AdditionalInformationKK,AdditionalInformationRU,NorthLatitude,EastLongitude")] KazHydrometSoilPost kazHydrometSoilPost)
        {
            var kazHydrometSoilPosts = _context.KazHydrometSoilPost.AsNoTracking().ToList();
            if (kazHydrometSoilPosts.Select(k => k.Number).Contains(kazHydrometSoilPost.Number))
            {
                ModelState.AddModelError("Number", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (kazHydrometSoilPosts.Select(k => k.NameKK).Contains(kazHydrometSoilPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (kazHydrometSoilPosts.Select(k => k.NameRU).Contains(kazHydrometSoilPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(kazHydrometSoilPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(kazHydrometSoilPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(kazHydrometSoilPost);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "KazHydrometSoilPost",
                    New = kazHydrometSoilPost.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kazHydrometSoilPost);
        }

        // GET: KazHydrometSoilPosts/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kazHydrometSoilPost = await _context.KazHydrometSoilPost.SingleOrDefaultAsync(m => m.Id == id);
            if (kazHydrometSoilPost == null)
            {
                return NotFound();
            }
            return View(kazHydrometSoilPost);
        }

        // POST: KazHydrometSoilPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,NameKK,NameRU,AdditionalInformationKK,AdditionalInformationRU,NorthLatitude,EastLongitude")] KazHydrometSoilPost kazHydrometSoilPost)
        {
            if (id != kazHydrometSoilPost.Id)
            {
                return NotFound();
            }
            var kazHydrometSoilPosts = _context.KazHydrometSoilPost.AsNoTracking().Where(k => k.Id != kazHydrometSoilPost.Id).ToList();
            if (kazHydrometSoilPosts.Select(k => k.Number).Contains(kazHydrometSoilPost.Number))
            {
                ModelState.AddModelError("Number", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (kazHydrometSoilPosts.Select(k => k.NameKK).Contains(kazHydrometSoilPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (kazHydrometSoilPosts.Select(k => k.NameRU).Contains(kazHydrometSoilPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(kazHydrometSoilPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(kazHydrometSoilPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var kazHydrometSoilPost_old = _context.KazHydrometSoilPost.AsNoTracking().FirstOrDefault(k => k.Id == kazHydrometSoilPost.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "KazHydrometSoilPost",
                        Operation = "Edit",
                        New = kazHydrometSoilPost.ToString(),
                        Old = kazHydrometSoilPost_old.ToString()
                    });
                    _context.Update(kazHydrometSoilPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KazHydrometSoilPostExists(kazHydrometSoilPost.Id))
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
            return View(kazHydrometSoilPost);
        }

        // GET: KazHydrometSoilPosts/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kazHydrometSoilPost = await _context.KazHydrometSoilPost
                .SingleOrDefaultAsync(m => m.Id == id);
            if (kazHydrometSoilPost == null)
            {
                return NotFound();
            }
            ViewBag.KazHydrometSoilPostDatas = _context.KazHydrometSoilPostData
                .AsNoTracking()
                .Where(k => k.KazHydrometSoilPostId == id)
                .Take(50)
                .OrderBy(k => k.Year);
            ViewBag.TargetTerritories = _context.TargetTerritory
                .AsNoTracking()
                .Where(k => k.KazHydrometSoilPostId == id)
                .OrderBy(k => k.TerritoryName);
            return View(kazHydrometSoilPost);
        }

        // POST: KazHydrometSoilPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kazHydrometSoilPost = await _context.KazHydrometSoilPost.SingleOrDefaultAsync(m => m.Id == id);
            if ((_context.KazHydrometSoilPostData
                .AsNoTracking()
                .FirstOrDefault(k => k.KazHydrometSoilPostId == id) == null)
                &&(_context.TargetTerritory
                .AsNoTracking()
                .FirstOrDefault(k => k.KazHydrometSoilPostId == id) == null))
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "KazHydrometSoilPost",
                    Operation = "Delete",
                    New = "",
                    Old = kazHydrometSoilPost.ToString()
                });
                _context.KazHydrometSoilPost.Remove(kazHydrometSoilPost);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool KazHydrometSoilPostExists(int id)
        {
            return _context.KazHydrometSoilPost.Any(e => e.Id == id);
        }
    }
}
