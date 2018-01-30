using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KnjiznicaProjekt.Data;
using KnjiznicaProjekt.Models;
using Microsoft.AspNetCore.Authorization;

namespace KnjiznicaProjekt.Controllers
{
    public class KnjigaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KnjigaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Knjiga

        public async Task<IActionResult> Index(string trazi)
        {
            /*
            var applicationDbContext = _context.Knjiga.Include(k => k.Autor);
            return View(await applicationDbContext.ToListAsync());
            */
            var knjige = from k in _context.Knjiga
                          select k; //LINQ izvlačimo knjige na searchu
            if (!String.IsNullOrEmpty(trazi))//provjera jeli trazi postavljen
            {
                knjige = knjige.Where(s => s.Naziv.Contains(trazi));
            }
            var autor = from a in _context.Autor
                        select a;
            foreach (var item in knjige)
            {
                foreach(var item2 in autor)
                {
                    if (item.AutorId == item2.Id) item.Autor = item2;   
                }
            }
            return View(knjige.ToList());
        }

        // GET: Knjiga/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var knjiga = await _context.Knjiga
                .Include(k => k.Autor)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (knjiga == null)
            {
                return NotFound();
            }

            return View(knjiga);
        }

        // GET: Knjiga/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["AutorId"] = new SelectList(_context.Set<Autor>(), "Id", "Id");
            return View();
        }

        // POST: Knjiga/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naziv,Godina,AutorId")] Knjiga knjiga)
        {
            if (ModelState.IsValid)
            {
                _context.Add(knjiga);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AutorId"] = new SelectList(_context.Set<Autor>(), "Id", "Id", knjiga.AutorId);
            return View(knjiga);
        }

        // GET: Knjiga/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var knjiga = await _context.Knjiga.SingleOrDefaultAsync(m => m.Id == id);
            if (knjiga == null)
            {
                return NotFound();
            }
            ViewData["AutorId"] = new SelectList(_context.Set<Autor>(), "Id", "Id", knjiga.AutorId);
            return View(knjiga);
        }

        // POST: Knjiga/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Godina,AutorId")] Knjiga knjiga)
        {
            if (id != knjiga.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(knjiga);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KnjigaExists(knjiga.Id))
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
            ViewData["AutorId"] = new SelectList(_context.Set<Autor>(), "Id", "Id", knjiga.AutorId);
           // ViewData["Autor"] = new SelectList(_context.Set<Autor>(), "Autor", "Autor", knjiga.Autor);
            return View(knjiga);
        }

        // GET: Knjiga/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var knjiga = await _context.Knjiga
                .Include(k => k.Autor)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (knjiga == null)
            {
                return NotFound();
            }

            return View(knjiga);
        }

        // POST: Knjiga/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var knjiga = await _context.Knjiga.SingleOrDefaultAsync(m => m.Id == id);
            _context.Knjiga.Remove(knjiga);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KnjigaExists(int id)
        {
            return _context.Knjiga.Any(e => e.Id == id);
        }
    }
}
