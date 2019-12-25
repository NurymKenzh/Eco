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
    public class MovementDirectionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public MovementDirectionsController(ApplicationDbContext context,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: MovementDirections
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Index(string SortOrder, string NameKK, string NameRU, int? Page)
        {
            var movementDirections = _context.MovementDirection
                .Where(m => true);

            ViewBag.NameKKFilter = NameKK;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NameKKSort = SortOrder == "NameKK" ? "NameKKDesc" : "NameKK";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(NameKK))
            {
                movementDirections = movementDirections.Where(m => m.NameKK.ToLower().Contains(NameKK.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                movementDirections = movementDirections.Where(m => m.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameKK":
                    movementDirections = movementDirections.OrderBy(m => m.NameKK);
                    break;
                case "NameKKDesc":
                    movementDirections = movementDirections.OrderByDescending(m => m.NameKK);
                    break;
                case "NameRU":
                    movementDirections = movementDirections.OrderBy(m => m.NameRU);
                    break;
                case "NameRUDesc":
                    movementDirections = movementDirections.OrderByDescending(m => m.NameRU);
                    break;
                default:
                    movementDirections = movementDirections.OrderBy(m => m.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(movementDirections.Count(), Page);

            var viewModel = new MovementDirectionIndexPageViewModel
            {
                Items = movementDirections.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: MovementDirections/Details/5
        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movementDirection = await _context.MovementDirection
                .SingleOrDefaultAsync(m => m.Id == id);
            if (movementDirection == null)
            {
                return NotFound();
            }

            return View(movementDirection);
        }

        // GET: MovementDirections/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: MovementDirections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Create([Bind("Id,NameKK,NameRU")] MovementDirection movementDirection)
        {
            var movementDirections = _context.MovementDirection.AsNoTracking().ToList();
            if (movementDirections.Select(m => m.NameKK).Contains(movementDirection.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (movementDirections.Select(m => m.NameRU).Contains(movementDirection.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(movementDirection.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(movementDirection.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                _context.Add(movementDirection);
                await _context.SaveChangesAsync();
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Operation = "Create",
                    Class = "MovementDirection",
                    New = movementDirection.ToString(),
                    Old = ""
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movementDirection);
        }

        // GET: MovementDirections/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movementDirection = await _context.MovementDirection.SingleOrDefaultAsync(m => m.Id == id);
            if (movementDirection == null)
            {
                return NotFound();
            }
            return View(movementDirection);
        }

        // POST: MovementDirections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameKK,NameRU")] MovementDirection movementDirection)
        {
            if (id != movementDirection.Id)
            {
                return NotFound();
            }
            var movementDirections = _context.MovementDirection.AsNoTracking().ToList();
            if (movementDirections.Where(m => m.Id != movementDirection.Id).Select(m => m.NameKK).Contains(movementDirection.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (movementDirections.Where(m => m.Id != movementDirection.Id).Select(m => m.NameRU).Contains(movementDirection.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorDublicateValue"]);
            }
            if (string.IsNullOrWhiteSpace(movementDirection.NameKK))
            {
                ModelState.AddModelError("NameKK", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (string.IsNullOrWhiteSpace(movementDirection.NameRU))
            {
                ModelState.AddModelError("NameRU", _sharedLocalizer["ErrorNeedToInput"]);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var movementDirection_old = _context.MovementDirection.AsNoTracking().FirstOrDefault(m => m.Id == movementDirection.Id);
                    _context.Log.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Email = User.Identity.Name,
                        Class = "MovementDirection",
                        Operation = "Edit",
                        New = movementDirection.ToString(),
                        Old = movementDirection_old.ToString()
                    });
                    _context.Update(movementDirection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovementDirectionExists(movementDirection.Id))
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
            return View(movementDirection);
        }

        // GET: MovementDirections/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movementDirection = await _context.MovementDirection
                .SingleOrDefaultAsync(m => m.Id == id);
            if (movementDirection == null)
            {
                return NotFound();
            }
            ViewBag.TransportPosts = _context.TransportPost
                .AsNoTracking()
                .Where(t => t.MovementDirectionId == id)
                .OrderBy(t => t.Name);
            return View(movementDirection);
        }

        // POST: MovementDirections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movementDirection = await _context.MovementDirection.SingleOrDefaultAsync(m => m.Id == id);
            if (_context.TransportPost
                .AsNoTracking()
                .FirstOrDefault(t => t.MovementDirectionId == id) == null)
            {
                _context.Log.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Email = User.Identity.Name,
                    Class = "MovementDirection",
                    Operation = "Delete",
                    New = "",
                    Old = movementDirection.ToString()
                });
                _context.MovementDirection.Remove(movementDirection);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MovementDirectionExists(int id)
        {
            return _context.MovementDirection.Any(e => e.Id == id);
        }
    }
}
