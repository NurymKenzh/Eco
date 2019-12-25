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
    public class AuthorizedAuthoritiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public AuthorizedAuthoritiesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: AuthorizedAuthorities
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string Name, string NameRU, int? Page)
        {
            var authorizedAuthorities = _context.AuthorizedAuthority
                .Where(h => true);

            ViewBag.NameFilter = Name;

            ViewBag.NameSort = SortOrder == "Name" ? "NameDesc" : "Name";

            if (!string.IsNullOrEmpty(Name))
            {
                authorizedAuthorities = authorizedAuthorities.Where(h => h.Name.ToLower().Contains(Name.ToLower()));
            }

            switch (SortOrder)
            {
                case "Name":
                    authorizedAuthorities = authorizedAuthorities.OrderBy(h => h.Name);
                    break;
                case "NameDesc":
                    authorizedAuthorities = authorizedAuthorities.OrderByDescending(h => h.Name);
                    break;
                default:
                    authorizedAuthorities = authorizedAuthorities.OrderBy(h => h.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(authorizedAuthorities.Count(), Page);

            var viewModel = new AuthorizedAuthorityIndexPageViewModel
            {
                Items = authorizedAuthorities.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: AuthorizedAuthorities/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorizedAuthority = await _context.AuthorizedAuthority
                .SingleOrDefaultAsync(m => m.Id == id);
            if (authorizedAuthority == null)
            {
                return NotFound();
            }

            return View(authorizedAuthority);
        }

        // GET: AuthorizedAuthorities/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: AuthorizedAuthorities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Name")] AuthorizedAuthority authorizedAuthority)
        {
            var authorizedAuthorities = _context.AuthorizedAuthority.AsNoTracking().ToList();
            if (authorizedAuthorities.Select(h => h.Name).Contains(authorizedAuthority.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(authorizedAuthority.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(authorizedAuthority);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "AuthorizedAuthority",
                    New = authorizedAuthority.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(authorizedAuthority);
        }

        // GET: AuthorizedAuthorities/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorizedAuthority = await _context.AuthorizedAuthority.SingleOrDefaultAsync(m => m.Id == id);
            if (authorizedAuthority == null)
            {
                return NotFound();
            }
            return View(authorizedAuthority);
        }

        // POST: AuthorizedAuthorities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] AuthorizedAuthority authorizedAuthority)
        {
            if (id != authorizedAuthority.Id)
            {
                return NotFound();
            }
            var authorizedAuthorities = _context.AuthorizedAuthority.AsNoTracking().ToList();
            if (authorizedAuthorities.Where(h => h.Id != authorizedAuthority.Id).Select(h => h.Name).Contains(authorizedAuthority.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(authorizedAuthority.Name))
            {
                ModelState.AddModelError("Name", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var authorizedAuthority_old = _context.AuthorizedAuthority.AsNoTracking().FirstOrDefault(h => h.Id == authorizedAuthority.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "AuthorizedAuthority",
                        Operation = "Edit",
                        New = authorizedAuthority.ToString(),
                        Old = authorizedAuthority_old.ToString()
                    });
                    _context.Update(authorizedAuthority);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorizedAuthorityExists(authorizedAuthority.Id))
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
            return View(authorizedAuthority);
        }

        // GET: AuthorizedAuthorities/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorizedAuthority = await _context.AuthorizedAuthority
                .SingleOrDefaultAsync(m => m.Id == id);
            if (authorizedAuthority == null)
            {
                return NotFound();
            }
            ViewBag.SpeciallyProtectedNaturalTerritories = _context.SpeciallyProtectedNaturalTerritory
                .AsNoTracking()
                .Where(a => a.AuthorizedAuthorityId == id)
                .OrderBy(a => a.Name);
            return View(authorizedAuthority);
        }

        // POST: AuthorizedAuthorities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var authorizedAuthority = await _context.AuthorizedAuthority.SingleOrDefaultAsync(m => m.Id == id);
            if (_context.SpeciallyProtectedNaturalTerritory
                .AsNoTracking()
                .FirstOrDefault(a => a.AuthorizedAuthorityId == id) == null)
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "AuthorizedAuthority",
                    Operation = "Delete",
                    New = "",
                    Old = authorizedAuthority.ToString()
                });
                _context.AuthorizedAuthority.Remove(authorizedAuthority);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorizedAuthorityExists(int id)
        {
            return _context.AuthorizedAuthority.Any(e => e.Id == id);
        }
    }
}
