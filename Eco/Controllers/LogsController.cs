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
    public class LogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Logs
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index(string SortOrder, string Email, string Class, string Operation, int? Page)
        {
            var logs = _context.Log
                .Where(b => true);

            ViewBag.CodeFilter = Email;
            ViewBag.ClassFilter = Class;
            ViewBag.OperationFilter = Operation;

            ViewBag.EmailSort = SortOrder == "Email" ? "EmailDesc" : "Email";
            ViewBag.ClassSort = SortOrder == "Class" ? "ClassDesc" : "Class";
            ViewBag.OperationSort = SortOrder == "Operation" ? "OperationDesc" : "Operation";
            ViewBag.DateTimeSort = SortOrder == "DateTime" ? "DateTimeDesc" : "DateTime";

            if (!string.IsNullOrEmpty(Email))
            {
                logs = logs.Where(l => l.Email.ToLower().Contains(Email.ToLower()));
            }
            if (!string.IsNullOrEmpty(Class))
            {
                logs = logs.Where(l => l.Class.ToLower().Contains(Class.ToLower()));
            }
            if (!string.IsNullOrEmpty(Operation))
            {
                logs = logs.Where(l => l.Operation.ToLower().Contains(Operation.ToLower()));
            }

            switch (SortOrder)
            {
                case "Email":
                    logs = logs.OrderBy(l => l.Email);
                    break;
                case "EmailDesc":
                    logs = logs.OrderByDescending(l => l.Email);
                    break;
                case "Class":
                    logs = logs.OrderBy(l => l.Class);
                    break;
                case "ClassDesc":
                    logs = logs.OrderByDescending(l => l.Class);
                    break;
                case "Operation":
                    logs = logs.OrderBy(l => l.Operation);
                    break;
                case "OperationDesc":
                    logs = logs.OrderByDescending(l => l.Operation);
                    break;
                case "DateTime":
                    logs = logs.OrderBy(l => l.DateTime);
                    break;
                case "DateTimeDesc":
                    logs = logs.OrderByDescending(l => l.DateTime);
                    break;
                default:
                    logs = logs.OrderBy(l => l.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(logs.Count(), Page);

            var viewModel = new LogIndexPageViewModel
            {
                Items = logs.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: Logs/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var log = await _context.Log
                .SingleOrDefaultAsync(m => m.Id == id);
            if (log == null)
            {
                return NotFound();
            }

            return View(log);
        }

        //// GET: Logs/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Logs/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Email,DateTime,Class,Operation,Old,New")] Log log)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(log);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(log);
        //}

        //// GET: Logs/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var log = await _context.Log.SingleOrDefaultAsync(m => m.Id == id);
        //    if (log == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(log);
        //}

        //// POST: Logs/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Email,DateTime,Class,Operation,Old,New")] Log log)
        //{
        //    if (id != log.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(log);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!LogExists(log.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(log);
        //}

        //// GET: Logs/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var log = await _context.Log
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (log == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(log);
        //}

        //// POST: Logs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var log = await _context.Log.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.Log.Remove(log);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool LogExists(int id)
        {
            return _context.Log.Any(e => e.Id == id);
        }
    }
}
