using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Booking.Domain.DomainModels;
using Booking.Repository;

namespace Booking.Web.Controllers
{
    public class AccommodationHostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccommodationHostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AccommodationHosts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Hosts.Include(a => a.Country);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AccommodationHosts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accommodationHost = await _context.Hosts
                .Include(a => a.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accommodationHost == null)
            {
                return NotFound();
            }

            return View(accommodationHost);
        }

        // GET: AccommodationHosts/Create
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name");
            return View();
        }

        // POST: AccommodationHosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,ContactEmail,CountryId,Id")] AccommodationHost accommodationHost)
        {
            if (ModelState.IsValid)
            {
                accommodationHost.Id = Guid.NewGuid();
                _context.Add(accommodationHost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Id", accommodationHost.CountryId);
            return View(accommodationHost);
        }

        // GET: AccommodationHosts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accommodationHost = await _context.Hosts.FindAsync(id);
            if (accommodationHost == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Id", accommodationHost.CountryId);
            return View(accommodationHost);
        }

        // POST: AccommodationHosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("FullName,ContactEmail,CountryId,Id")] AccommodationHost accommodationHost)
        {
            if (id != accommodationHost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accommodationHost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccommodationHostExists(accommodationHost.Id))
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
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Id", accommodationHost.CountryId);
            return View(accommodationHost);
        }

        // GET: AccommodationHosts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accommodationHost = await _context.Hosts
                .Include(a => a.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accommodationHost == null)
            {
                return NotFound();
            }

            return View(accommodationHost);
        }

        // POST: AccommodationHosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var accommodationHost = await _context.Hosts.FindAsync(id);
            if (accommodationHost != null)
            {
                _context.Hosts.Remove(accommodationHost);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccommodationHostExists(Guid id)
        {
            return _context.Hosts.Any(e => e.Id == id);
        }
    }
}
