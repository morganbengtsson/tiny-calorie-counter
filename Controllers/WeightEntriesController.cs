using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Diet.Contexts;
using Diet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Diet.Controllers
{
    [Authorize]
    public class WeightEntriesController : Controller
    {
        private readonly MainContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public WeightEntriesController(MainContext context, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _signInManager.UserManager.GetUserAsync(User);
            var entries = _context.WeightEntries.Where(w => w.User == user);
            return View(await entries.ToListAsync());
        }

        // GET: WeightEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weightEntry = await _context.WeightEntries
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weightEntry == null)
            {
                return NotFound();
            }

            return View(weightEntry);
        }

        // GET: WeightEntries/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: WeightEntries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Weight,Date,UserId")] WeightEntry weightEntry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(weightEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", weightEntry.UserId);
            return View(weightEntry);
        }

        // GET: WeightEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weightEntry = await _context.WeightEntries.FindAsync(id);
            if (weightEntry == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", weightEntry.UserId);
            return View(weightEntry);
        }

        // POST: WeightEntries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Weight,Date,UserId")] WeightEntry weightEntry)
        {
            if (id != weightEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(weightEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeightEntryExists(weightEntry.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", weightEntry.UserId);
            return View(weightEntry);
        }

        // GET: WeightEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weightEntry = await _context.WeightEntries
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weightEntry == null)
            {
                return NotFound();
            }

            return View(weightEntry);
        }

        // POST: WeightEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var weightEntry = await _context.WeightEntries.FindAsync(id);
            _context.WeightEntries.Remove(weightEntry);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeightEntryExists(int id)
        {
            return _context.WeightEntries.Any(e => e.Id == id);
        }
    }
}
