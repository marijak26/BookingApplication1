using Booking.Domain.DomainModels;
using Booking.Domain.Enum;
using Booking.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Web.Controllers
{
    public class AccommodationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccommodationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Accommodations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Accommodations.Include(a => a.Host);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Accommodations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accommodation = await _context.Accommodations
                .Include(a => a.Host)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accommodation == null)
            {
                return NotFound();
            }

            return View(accommodation);
        }

        // GET: Accommodations/Create
        public IActionResult Create()
        {
            var hosts = _context.Hosts.ToList();
            ViewData["HostId"] = new SelectList(hosts, "Id", "FullName");

            var categories = Enum.GetValues(typeof(AccommodationCategory))
                                 .Cast<AccommodationCategory>()
                                 .Select(c => new SelectListItem
                                 {
                                     Value = ((int)c).ToString(),
                                     Text = c.ToString()
                                 }).ToList();
            ViewData["Category"] = categories;

            return View();
        }



        // POST: Accommodations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,PricePerNight,IsRented,Category,HostId,Id")] Accommodation accommodation)
        {
            if (ModelState.IsValid)
            {
                accommodation.Id = Guid.NewGuid();
                _context.Add(accommodation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var hosts = _context.Hosts.ToList();
            ViewData["HostId"] = new SelectList(hosts, "Id", "FullName", accommodation.HostId);

            var categories = Enum.GetValues(typeof(AccommodationCategory))
                                 .Cast<AccommodationCategory>()
                                 .Select(c => new SelectListItem
                                 {
                                     Value = ((int)c).ToString(),
                                     Text = c.ToString()
                                 }).ToList();
            ViewData["Category"] = categories;

            return View(accommodation);
        }



        // GET: Accommodations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accommodation = await _context.Accommodations
                                      .Include(a => a.Host) 
                                      .FirstOrDefaultAsync(a => a.Id == id);

            if (accommodation == null)
                return NotFound();

            ViewData["HostId"] = new SelectList(
                _context.Hosts,
                "Id",                 
                "FullName",           
                accommodation.HostId  
            );
            var categories = Enum.GetValues(typeof(AccommodationCategory))
                         .Cast<AccommodationCategory>()
                         .Select(c => new SelectListItem
                         {
                             Value = ((int)c).ToString(),
                             Text = c.ToString()
                         }).ToList();
            ViewData["Category"] = categories;
            return View(accommodation);
        }

        // POST: Accommodations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Description,PricePerNight,IsRented,Category,HostId,Id")] Accommodation accommodation)
        {
            if (id != accommodation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accommodation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccommodationExists(accommodation.Id))
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
            ViewData["HostId"] = new SelectList(_context.Hosts, "Id", "FullName", accommodation.HostId);
            return View(accommodation);
        }

        // GET: Accommodations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accommodation = await _context.Accommodations
                .Include(a => a.Host)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accommodation == null)
            {
                return NotFound();
            }

            return View(accommodation);
        }

        // POST: Accommodations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var accommodation = await _context.Accommodations.FindAsync(id);
            if (accommodation != null)
            {
                _context.Accommodations.Remove(accommodation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccommodationExists(Guid id)
        {
            return _context.Accommodations.Any(e => e.Id == id);
        }
    }
}
