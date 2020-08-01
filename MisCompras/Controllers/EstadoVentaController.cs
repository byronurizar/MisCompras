using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MisCrompras.Models;

namespace MisCrompras.Controllers
{
    public class EstadoVentaController : Controller
    {
        private readonly MisComprasContext _context;

        public EstadoVentaController(MisComprasContext context)
        {
            _context = context;
        }

        // GET: EstadoVenta
        public async Task<IActionResult> Index()
        {
            var misComprasContext = _context.EstadoVentas.Include(e => e.Estado).Include(e => e.Usuario).Where(a=>a.EstadoId!=3);
            return View(await misComprasContext.ToListAsync());
        }

        // GET: EstadoVenta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoVenta = await _context.EstadoVentas
                .Include(e => e.Estado)
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estadoVenta == null)
            {
                return NotFound();
            }

            return View(estadoVenta);
        }

        // GET: EstadoVenta/Create
        public IActionResult Create()
        {
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion");
            return View();
        }

        // POST: EstadoVenta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion,EstadoId")] EstadoVenta estadoVenta)
        {
            if (ModelState.IsValid)
            {
                estadoVenta.FechaCreacion = DateTime.Now;
                estadoVenta.UsuarioId = Convert.ToInt32(HttpContext.Session.GetString("UsuarioId"));
                estadoVenta.Descripcion = estadoVenta.Descripcion.Trim().ToUpper();
                _context.Add(estadoVenta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", estadoVenta.EstadoId);
            return View(estadoVenta);
        }

        // GET: EstadoVenta/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoVenta = await _context.EstadoVentas.FindAsync(id);
            if (estadoVenta == null)
            {
                return NotFound();
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", estadoVenta.EstadoId);
            return View(estadoVenta);
        }

        // POST: EstadoVenta/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion,EstadoId,FechaCreacion,UsuarioId")] EstadoVenta estadoVenta)
        {
            if (id != estadoVenta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var InfoEstadoVenta = await _context.EstadoVentas.FindAsync(id);
                    InfoEstadoVenta.Descripcion = estadoVenta.Descripcion.Trim().ToUpper();
                    InfoEstadoVenta.EstadoId = estadoVenta.EstadoId;
                    _context.Update(InfoEstadoVenta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadoVentaExists(estadoVenta.Id))
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
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", estadoVenta.EstadoId);
            return View(estadoVenta);
        }

        // GET: EstadoVenta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoVenta = await _context.EstadoVentas
                .Include(e => e.Estado)
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estadoVenta == null)
            {
                return NotFound();
            }

            return View(estadoVenta);
        }

        // POST: EstadoVenta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estadoVenta = await _context.EstadoVentas.FindAsync(id);
            estadoVenta.EstadoId = 3;
            _context.Update(estadoVenta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstadoVentaExists(int id)
        {
            return _context.EstadoVentas.Any(e => e.Id == id);
        }
    }
}
