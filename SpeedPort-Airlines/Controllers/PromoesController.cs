using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpeedPort_Airlines.Data;
using SpeedPort_Airlines.Models;

namespace SpeedPort_Airlines.Views
{
    public class PromoesController : Controller
    {
        private readonly SpeedPort_AirlinesNewContext _context;

        public PromoesController(SpeedPort_AirlinesNewContext context)
        {
            _context = context;
        }

        // GET: Promoes //Search Promo by Destination Country & Travel agency
        public async Task<IActionResult> Index(string DestinationCountry, string TravelAgency)
        {
            var Promo = from m in _context.Promo
                        select m;
            if (!String.IsNullOrEmpty(DestinationCountry))
            {
                Promo = Promo.Where(s => s.DestinationCountry.Contains(DestinationCountry));
            }
            //attach the values to the dropdown list
            IQueryable<string> TypeQuery = from m in _context.Promo
                                           orderby m.TravelAgency
                                           select m.TravelAgency;
            IEnumerable<SelectListItem> items =
                new SelectList(await TypeQuery.Distinct().ToListAsync());
            ViewBag.TravelAgency = items;

            //Filtering based on Travel Agency
            if (!String.IsNullOrEmpty(TravelAgency))
            {
                Promo = Promo.Where(s => s.TravelAgency.Contains(TravelAgency));
            }
            return View(await Promo.ToListAsync());
        }

        // GET: Promoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promo = await _context.Promo
                .FirstOrDefaultAsync(m => m.ID == id);
            if (promo == null)
            {
                return NotFound();
            }

            return View(promo);
        }

        // GET: Promoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Promoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,DestinationCountry,PromoValidity,TravelAgency,PromoPrice")] Promo promo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(promo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(promo);
        }

        // GET: Promoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promo = await _context.Promo.FindAsync(id);
            if (promo == null)
            {
                return NotFound();
            }
            return View(promo);
        }

        // POST: Promoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,DestinationCountry,PromoValidity,TravelAgency,PromoPrice")] Promo promo)
        {
            if (id != promo.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(promo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PromoExists(promo.ID))
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
            return View(promo);
        }

        // GET: Promoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promo = await _context.Promo
                .FirstOrDefaultAsync(m => m.ID == id);
            if (promo == null)
            {
                return NotFound();
            }

            return View(promo);
        }

        // POST: Promoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var promo = await _context.Promo.FindAsync(id);
            _context.Promo.Remove(promo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PromoExists(int id)
        {
            return _context.Promo.Any(e => e.ID == id);
        }
    }
}
