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
    public class MobileAirPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public MobileAirPostsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: MobileAirPosts
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string Number, string NameKK, string NameRU, int? Page)
        {
            var mobileAirPosts = _context.MobileAirPost
                .Where(m => true);

            ViewBag.NumberFilter = Number;
            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NumberSort = SortOrder == "Number" ? "NumberDesc" : "Number";
            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(Number))
            {
                mobileAirPosts = mobileAirPosts.Where(m => m.Number.ToString().ToLower().Contains(Number.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameKK))
            {
                mobileAirPosts = mobileAirPosts.Where(m => m.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                mobileAirPosts = mobileAirPosts.Where(m => m.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "Number":
                    mobileAirPosts = mobileAirPosts.OrderBy(m => m.Number);
                    break;
                case "NumberDesc":
                    mobileAirPosts = mobileAirPosts.OrderByDescending(m => m.Number);
                    break;
                case "NameKK":
                    mobileAirPosts = mobileAirPosts.OrderBy(m => m.NameKK);
                    break;
                case "NameKKDesc":
                    mobileAirPosts = mobileAirPosts.OrderByDescending(m => m.NameKK);
                    break;
                case "NameRU":
                    mobileAirPosts = mobileAirPosts.OrderBy(m => m.NameRU);
                    break;
                case "NameRUDesc":
                    mobileAirPosts = mobileAirPosts.OrderByDescending(m => m.NameRU);
                    break;
                default:
                    mobileAirPosts = mobileAirPosts.OrderBy(m => m.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(mobileAirPosts.Count(), Page);

            var viewModel = new MobileAirPostIndexPageViewModel
            {
                Items = mobileAirPosts.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: MobileAirPosts/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mobileAirPost = await _context.MobileAirPost
                .SingleOrDefaultAsync(m => m.Id == id);
            if (mobileAirPost == null)
            {
                return NotFound();
            }

            return View(mobileAirPost);
        }

        // GET: MobileAirPosts/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: MobileAirPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Number,NameKK,NameRU,AdditionalInformationKK,AdditionalInformationRU,NorthLatitude,EastLongitude")] MobileAirPost mobileAirPost)
        {
            var mobileAirPosts = _context.MobileAirPost.AsNoTracking().ToList();
            if (mobileAirPosts.Select(m => m.Number).Contains(mobileAirPost.Number))
            {
                ModelState.AddModelError("Number", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (mobileAirPosts.Select(m => m.NameKK).Contains(mobileAirPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (mobileAirPosts.Select(m => m.NameRU).Contains(mobileAirPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(mobileAirPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(mobileAirPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(mobileAirPost);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "MobileAirPost",
                    New = mobileAirPost.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mobileAirPost);
        }

        // GET: MobileAirPosts/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mobileAirPost = await _context.MobileAirPost.SingleOrDefaultAsync(m => m.Id == id);
            if (mobileAirPost == null)
            {
                return NotFound();
            }
            return View(mobileAirPost);
        }

        // POST: MobileAirPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,NameKK,NameRU,AdditionalInformationKK,AdditionalInformationRU,NorthLatitude,EastLongitude")] MobileAirPost mobileAirPost)
        {
            if (id != mobileAirPost.Id)
            {
                return NotFound();
            }
            var mobileAirPosts = _context.MobileAirPost.AsNoTracking().Where(m => m.Id != mobileAirPost.Id).ToList();
            if (mobileAirPosts.Select(m => m.Number).Contains(mobileAirPost.Number))
            {
                ModelState.AddModelError("Number", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (mobileAirPosts.Select(m => m.NameKK).Contains(mobileAirPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (mobileAirPosts.Select(m => m.NameRU).Contains(mobileAirPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(mobileAirPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(mobileAirPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var mobileAirPost_old = _context.LimitingIndicator.AsNoTracking().FirstOrDefault(m => m.Id == mobileAirPost.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "MobileAirPost",
                        Operation = "Edit",
                        New = mobileAirPost.ToString(),
                        Old = mobileAirPost_old.ToString()
                    });
                    _context.Update(mobileAirPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MobileAirPostExists(mobileAirPost.Id))
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
            return View(mobileAirPost);
        }

        // GET: MobileAirPosts/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mobileAirPost = await _context.MobileAirPost
                .SingleOrDefaultAsync(m => m.Id == id);
            if (mobileAirPost == null)
            {
                return NotFound();
            }

            return View(mobileAirPost);
        }

        // POST: MobileAirPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mobileAirPost = await _context.MobileAirPost.SingleOrDefaultAsync(m => m.Id == id);
            _context.Log.Add(new Log()
            {
                DateTime = DateTime.Now,
                Email = User.Identity.Name,
                Class = "MobileAirPost",
                Operation = "Delete",
                New = "",
                Old = mobileAirPost.ToString()
            });
            _context.MobileAirPost.Remove(mobileAirPost);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MobileAirPostExists(int id)
        {
            return _context.MobileAirPost.Any(e => e.Id == id);
        }
    }
}
