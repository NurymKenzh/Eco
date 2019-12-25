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
    public class TransportPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public TransportPostsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: TransportPosts
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string NameKK, string NameRU, int? Page)
        {
            var transportPosts = _context.TransportPost
                .Where(t => true);

            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(NameKK))
            {
                transportPosts = transportPosts.Where(t => t.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                transportPosts = transportPosts.Where(t => t.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameKK":
                    transportPosts = transportPosts.OrderBy(t => t.NameKK);
                    break;
                case "NameKKDesc":
                    transportPosts = transportPosts.OrderByDescending(t => t.NameKK);
                    break;
                case "NameRU":
                    transportPosts = transportPosts.OrderBy(t => t.NameRU);
                    break;
                case "NameRUDesc":
                    transportPosts = transportPosts.OrderByDescending(t => t.NameRU);
                    break;
                default:
                    transportPosts = transportPosts.OrderBy(t => t.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(transportPosts.Count(), Page);

            var viewModel = new TransportPostIndexPageViewModel
            {
                Items = transportPosts.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: TransportPosts/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transportPost = await _context.TransportPost
                .Include(t => t.MovementDirection)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (transportPost == null)
            {
                return NotFound();
            }

            return View(transportPost);
        }

        // GET: TransportPosts/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["MovementDirectionId"] = new SelectList(_context.MovementDirection.OrderBy(m => m.Name), "Id", "Name");
            TransportPost model = new TransportPost();
            return View(model);
        }

        // POST: TransportPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,NameKK,NameRU,Type,MovementDirectionId,NumberOfBands,AdditionalInformationKK,AdditionalInformationRU,NorthLatitude,EastLongitude")] TransportPost transportPost)
        {
            var transportPosts = _context.TransportPost.AsNoTracking().ToList();
            if (transportPosts.FirstOrDefault(t => t.Type == transportPost.Type && t.NameKK == transportPost.NameKK) != null)
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (transportPosts.FirstOrDefault(t => t.Type == transportPost.Type && t.NameRU == transportPost.NameRU) != null)
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(transportPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(transportPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(transportPost);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "TransportPost",
                    New = transportPost.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovementDirectionId"] = new SelectList(_context.MovementDirection.OrderBy(m => m.Name), "Id", "Name", transportPost.MovementDirectionId);
            return View(transportPost);
        }

        // GET: TransportPosts/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transportPost = await _context.TransportPost.SingleOrDefaultAsync(m => m.Id == id);
            if (transportPost == null)
            {
                return NotFound();
            }
            ViewData["MovementDirectionId"] = new SelectList(_context.MovementDirection.OrderBy(m => m.Name), "Id", "Name", transportPost.MovementDirectionId);
            return View(transportPost);
        }

        // POST: TransportPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameKK,NameRU,Type,MovementDirectionId,NumberOfBands,AdditionalInformationKK,AdditionalInformationRU,NorthLatitude,EastLongitude")] TransportPost transportPost)
        {
            if (id != transportPost.Id)
            {
                return NotFound();
            }
            var transportPosts = _context.TransportPost.AsNoTracking().Where(k => k.Id != transportPost.Id).ToList();
            if (transportPosts.FirstOrDefault(t => t.Type == transportPost.Type && t.NameKK == transportPost.NameKK) != null)
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (transportPosts.FirstOrDefault(t => t.Type == transportPost.Type && t.NameRU == transportPost.NameRU) != null)
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(transportPost.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(transportPost.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var transportPost_old = _context.LimitingIndicator.AsNoTracking().FirstOrDefault(k => k.Id == transportPost.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "TransportPost",
                        Operation = "Edit",
                        New = transportPost.ToString(),
                        Old = transportPost_old.ToString()
                    });
                    _context.Update(transportPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransportPostExists(transportPost.Id))
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
            ViewData["MovementDirectionId"] = new SelectList(_context.MovementDirection.OrderBy(m => m.Name), "Id", "Name", transportPost.MovementDirectionId);
            return View(transportPost);
        }

        // GET: TransportPosts/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transportPost = await _context.TransportPost
                .Include(t => t.MovementDirection)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (transportPost == null)
            {
                return NotFound();
            }
            ViewBag.TransportPostDatas = _context.TransportPostData
                .AsNoTracking()
                .Where(a => a.TransportPostId == id)
                .Take(50)
                .OrderBy(a => a.DateTime.Year);
            ViewBag.TargetTerritories = _context.TargetTerritory
                .AsNoTracking()
                .Where(k => k.TransportPostId == id)
                .OrderBy(k => k.TerritoryName);
            return View(transportPost);
        }

        // POST: TransportPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transportPost = await _context.TransportPost.SingleOrDefaultAsync(m => m.Id == id);
            if ((_context.TransportPostData
                .AsNoTracking()
                .FirstOrDefault(a => a.TransportPostId == id) == null)
                &&(_context.TargetTerritory
                .AsNoTracking()
                .FirstOrDefault(k => k.TransportPostId == id) == null))
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "TransportPost",
                    Operation = "Delete",
                    New = "",
                    Old = transportPost.ToString()
                });
                _context.TransportPost.Remove(transportPost);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TransportPostExists(int id)
        {
            return _context.TransportPost.Any(e => e.Id == id);
        }
    }
}
