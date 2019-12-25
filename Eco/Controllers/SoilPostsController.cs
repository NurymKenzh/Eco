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
    public class SoilPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public SoilPostsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: SoilPosts
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string NameKK, string NameRU, int? Page)
        {
            var soilPosts = _context.SoilPost
                .Where(s => true);
            
            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;
            
            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";
            
            if (!string.IsNullOrEmpty(NameKK))
            {
                soilPosts = soilPosts.Where(s => s.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                soilPosts = soilPosts.Where(s => s.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameKK":
                    soilPosts = soilPosts.OrderBy(s => s.NameKK);
                    break;
                case "NameKKDesc":
                    soilPosts = soilPosts.OrderByDescending(s => s.NameKK);
                    break;
                case "NameRU":
                    soilPosts = soilPosts.OrderBy(s => s.NameRU);
                    break;
                case "NameRUDesc":
                    soilPosts = soilPosts.OrderByDescending(s => s.NameRU);
                    break;
                default:
                    soilPosts = soilPosts.OrderBy(s => s.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(soilPosts.Count(), Page);

            var viewModel = new SoilPostIndexPageViewModel
            {
                Items = soilPosts.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: SoilPosts/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soilPost = await _context.SoilPost
                .SingleOrDefaultAsync(m => m.Id == id);
            if (soilPost == null)
            {
                return NotFound();
            }

            return View(soilPost);
        }

        // GET: SoilPosts/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: SoilPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,NameKK,NameRU,AdditionalInformationKK,AdditionalInformationRU,NorthLatitude,EastLongitude")] SoilPost soilPost)
        {
            var soilPosts = _context.SoilPost.AsNoTracking().ToList();
            if (soilPosts.Select(s => s.NameKK).Contains(soilPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (soilPosts.Select(s => s.NameRU).Contains(soilPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(soilPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(soilPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(soilPost);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "SoilPost",
                    New = soilPost.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(soilPost);
        }

        // GET: SoilPosts/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soilPost = await _context.SoilPost.SingleOrDefaultAsync(m => m.Id == id);
            if (soilPost == null)
            {
                return NotFound();
            }
            return View(soilPost);
        }

        // POST: SoilPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameKK,NameRU,AdditionalInformationKK,AdditionalInformationRU,NorthLatitude,EastLongitude")] SoilPost soilPost)
        {
            if (id != soilPost.Id)
            {
                return NotFound();
            }
            var soilPosts = _context.SoilPost.AsNoTracking().Where(s => s.Id != soilPost.Id).ToList();
            if (soilPosts.Select(s => s.NameKK).Contains(soilPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (soilPosts.Select(s => s.NameRU).Contains(soilPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(soilPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(soilPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var soilPost_old = _context.LimitingIndicator.AsNoTracking().FirstOrDefault(s => s.Id == soilPost.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "SoilPost",
                        Operation = "Edit",
                        New = soilPost.ToString(),
                        Old = soilPost_old.ToString()
                    });
                    _context.Update(soilPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SoilPostExists(soilPost.Id))
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
            return View(soilPost);
        }

        // GET: SoilPosts/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soilPost = await _context.SoilPost
                .SingleOrDefaultAsync(m => m.Id == id);
            if (soilPost == null)
            {
                return NotFound();
            }
            ViewBag.SoilPostDatas = _context.SoilPostData
                .AsNoTracking()
                .Where(a => a.SoilPostId == id)
                .Take(50)
                .OrderBy(a => a.DateOfSampling);
            return View(soilPost);
        }

        // POST: SoilPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var soilPost = await _context.SoilPost.SingleOrDefaultAsync(m => m.Id == id);
            if (_context.SoilPostData
                .AsNoTracking()
                .FirstOrDefault(a => a.SoilPostId == id) == null)
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "SoilPost",
                    Operation = "Delete",
                    New = "",
                    Old = soilPost.ToString()
                });
                _context.SoilPost.Remove(soilPost);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SoilPostExists(int id)
        {
            return _context.SoilPost.Any(e => e.Id == id);
        }
    }
}
