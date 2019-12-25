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
    public class IssuingPermitsStateAuthoritiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public IssuingPermitsStateAuthoritiesController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: IssuingPermitsStateAuthorities
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string NameKK, string NameRU, int? Page)
        {
            var issuingPermitsStateAuthorities = _context.IssuingPermitsStateAuthority
                .Where(s => true);

            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(NameKK))
            {
                issuingPermitsStateAuthorities = issuingPermitsStateAuthorities.Where(s => s.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                issuingPermitsStateAuthorities = issuingPermitsStateAuthorities.Where(s => s.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameKK":
                    issuingPermitsStateAuthorities = issuingPermitsStateAuthorities.OrderBy(s => s.NameKK);
                    break;
                case "NameKKDesc":
                    issuingPermitsStateAuthorities = issuingPermitsStateAuthorities.OrderByDescending(s => s.NameKK);
                    break;
                case "NameRU":
                    issuingPermitsStateAuthorities = issuingPermitsStateAuthorities.OrderBy(s => s.NameRU);
                    break;
                case "NameRUDesc":
                    issuingPermitsStateAuthorities = issuingPermitsStateAuthorities.OrderByDescending(s => s.NameRU);
                    break;
                default:
                    issuingPermitsStateAuthorities = issuingPermitsStateAuthorities.OrderBy(s => s.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(issuingPermitsStateAuthorities.Count(), Page);

            var viewModel = new IssuingPermitsStateAuthorityIndexPageViewModel
            {
                Items = issuingPermitsStateAuthorities.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: IssuingPermitsStateAuthorities/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issuingPermitsStateAuthority = await _context.IssuingPermitsStateAuthority
                .SingleOrDefaultAsync(m => m.Id == id);
            if (issuingPermitsStateAuthority == null)
            {
                return NotFound();
            }

            return View(issuingPermitsStateAuthority);
        }

        // GET: IssuingPermitsStateAuthorities/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: IssuingPermitsStateAuthorities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,NameKK,NameRU")] IssuingPermitsStateAuthority issuingPermitsStateAuthority)
        {
            var issuingPermitsStateAuthorities = _context.IssuingPermitsStateAuthority.AsNoTracking().ToList();
            if (issuingPermitsStateAuthorities.Select(s => s.NameKK).Contains(issuingPermitsStateAuthority.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (issuingPermitsStateAuthorities.Select(s => s.NameRU).Contains(issuingPermitsStateAuthority.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(issuingPermitsStateAuthority.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(issuingPermitsStateAuthority.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(issuingPermitsStateAuthority);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "IssuingPermitsStateAuthority",
                    New = issuingPermitsStateAuthority.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(issuingPermitsStateAuthority);
        }

        // GET: IssuingPermitsStateAuthorities/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issuingPermitsStateAuthority = await _context.IssuingPermitsStateAuthority.SingleOrDefaultAsync(m => m.Id == id);
            if (issuingPermitsStateAuthority == null)
            {
                return NotFound();
            }
            return View(issuingPermitsStateAuthority);
        }

        // POST: IssuingPermitsStateAuthorities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameKK,NameRU")] IssuingPermitsStateAuthority issuingPermitsStateAuthority)
        {
            if (id != issuingPermitsStateAuthority.Id)
            {
                return NotFound();
            }
            var issuingPermitsStateAuthorities = _context.IssuingPermitsStateAuthority.AsNoTracking().ToList();
            if (issuingPermitsStateAuthorities.Where(s => s.Id != issuingPermitsStateAuthority.Id).Select(s => s.NameKK).Contains(issuingPermitsStateAuthority.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (issuingPermitsStateAuthorities.Where(s => s.Id != issuingPermitsStateAuthority.Id).Select(s => s.NameRU).Contains(issuingPermitsStateAuthority.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(issuingPermitsStateAuthority.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(issuingPermitsStateAuthority.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var issuingPermitsStateAuthority_old = _context.IssuingPermitsStateAuthority.AsNoTracking().FirstOrDefault(s => s.Id == issuingPermitsStateAuthority.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "IssuingPermitsStateAuthority",
                        Operation = "Edit",
                        New = issuingPermitsStateAuthority.ToString(),
                        Old = issuingPermitsStateAuthority_old.ToString()
                    });
                    _context.Update(issuingPermitsStateAuthority);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IssuingPermitsStateAuthorityExists(issuingPermitsStateAuthority.Id))
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
            return View(issuingPermitsStateAuthority);
        }

        // GET: IssuingPermitsStateAuthorities/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issuingPermitsStateAuthority = await _context.IssuingPermitsStateAuthority
                .SingleOrDefaultAsync(m => m.Id == id);
            if (issuingPermitsStateAuthority == null)
            {
                return NotFound();
            }
            ViewBag.AnnualMaximumPermissibleEmissionsVolumes = _context.AnnualMaximumPermissibleEmissionsVolume
                .AsNoTracking()
                .Where(k => k.IssuingPermitsStateAuthorityId == id)
                .Include(a => a.Company)
                .Include(a => a.SubsidiaryCompany)
                .OrderBy(k => k.CompanyOrSubsidiaryCompanyAbbreviatedName);
            return View(issuingPermitsStateAuthority);
        }

        // POST: IssuingPermitsStateAuthorities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var issuingPermitsStateAuthority = await _context.IssuingPermitsStateAuthority.SingleOrDefaultAsync(m => m.Id == id);
            if (_context.AnnualMaximumPermissibleEmissionsVolume
                .AsNoTracking()
                .FirstOrDefault(k => k.IssuingPermitsStateAuthorityId == id) == null)
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "IssuingPermitsStateAuthority",
                    Operation = "Delete",
                    New = "",
                    Old = issuingPermitsStateAuthority.ToString()
                });
                _context.IssuingPermitsStateAuthority.Remove(issuingPermitsStateAuthority);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool IssuingPermitsStateAuthorityExists(int id)
        {
            return _context.IssuingPermitsStateAuthority.Any(e => e.Id == id);
        }
    }
}
