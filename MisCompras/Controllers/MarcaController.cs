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
    public class MarcaController : Controller
    {
        private readonly MisComprasContext _context;

        public MarcaController(MisComprasContext context)
        {
            _context = context;
        }

        // GET: Marca
        public async Task<IActionResult> Index()
        {
            var misComprasContext = _context.Marcas.Include(m => m.Estado).Include(m=>m.Usuario).Where(m=>m.EstadoId!=3);
            return View(await misComprasContext.ToListAsync());
        }

        // GET: Marca/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marca = await _context.Marcas
                .Include(m => m.Estado)
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (marca == null)
            {
                return NotFound();
            }

            return View(marca);
        }

        // GET: Marca/Create
        public IActionResult Create()
        {
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion");
            return View();
        }

        // POST: Marca/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion,PorcentajeDescuento,Dividendo,Moneda,EstadoId")] Marca marca)
        {
            if (ModelState.IsValid)
            {
                marca.FechaCreacion = DateTime.Now;
                marca.UsuarioId = Convert.ToInt32(HttpContext.Session.GetString("UsuarioId"));
                marca.Descripcion = marca.Descripcion.Trim().ToUpper();
                if (marca.PorcentajeDescuento <= 0)
                {
                    marca.PorcentajeDescuento = 0;
                }
                if (marca.Dividendo <= 0)
                {
                    marca.Dividendo = 0;
                }
                _context.Add(marca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", marca.EstadoId);
            return View(marca);
        }

        // GET: Marca/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marca = await _context.Marcas.FindAsync(id);
            if (marca == null)
            {
                return NotFound();
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", marca.EstadoId);
            return View(marca);
        }

        // POST: Marca/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion,PorcentajeDescuento,Dividendo,Moneda,EstadoId")] Marca marca)
        {
            if (id != marca.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var infoMarca = await _context.Marcas.FindAsync(id);
                    infoMarca.Descripcion = marca.Descripcion.Trim().ToUpper();
                    infoMarca.Dividendo = marca.Dividendo;
                    infoMarca.Moneda = marca.Moneda.Trim().ToUpper();
                    infoMarca.PorcentajeDescuento= marca.PorcentajeDescuento;
                    if (marca.PorcentajeDescuento <= 0)
                    {
                        infoMarca.PorcentajeDescuento = 0;
                    }
                    if (marca.Dividendo <= 0)
                    {
                        infoMarca.Dividendo = 0;
                    }
                    infoMarca.EstadoId = marca.EstadoId;
                    _context.Update(infoMarca);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarcaExists(marca.Id))
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
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", marca.EstadoId);
            return View(marca);
        }

        // GET: Marca/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marca = await _context.Marcas
                .Include(m => m.Estado)
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (marca == null)
            {
                return NotFound();
            }

            return View(marca);
        }

        // POST: Marca/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var marca = await _context.Marcas.FindAsync(id);
            marca.EstadoId = 3;
            _context.Update(marca);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarcaExists(int id)
        {
            return _context.Marcas.Any(e => e.Id == id);
        }
    }
}
