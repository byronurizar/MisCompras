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
    public class EstadoPedidoController : Controller
    {
        private readonly MisComprasContext _context;

        public EstadoPedidoController(MisComprasContext context)
        {
            _context = context;
        }

        // GET: EstadoPedido
        public async Task<IActionResult> Index()
        {
            var misComprasContext = _context.EstadoPedidos.Include(e => e.Estado).Include(e => e.Usuario).Where(a=>a.EstadoId!=3);
            return View(await misComprasContext.ToListAsync());
        }

        // GET: EstadoPedido/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoPedido = await _context.EstadoPedidos
                .Include(e => e.Estado)
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estadoPedido == null)
            {
                return NotFound();
            }

            return View(estadoPedido);
        }

        // GET: EstadoPedido/Create
        public IActionResult Create()
        {
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion");
            return View();
        }

        // POST: EstadoPedido/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion,EstadoId")] EstadoPedido estadoPedido)
        {
            if (ModelState.IsValid)
            {
                estadoPedido.FechaCreacion = DateTime.Now;
                estadoPedido.UsuarioId=Convert.ToInt32(HttpContext.Session.GetString("UsuarioId"));
                estadoPedido.Descripcion = estadoPedido.Descripcion.Trim().ToUpper();
                _context.Add(estadoPedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", estadoPedido.EstadoId);
            return View(estadoPedido);
        }

        // GET: EstadoPedido/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoPedido = await _context.EstadoPedidos.FindAsync(id);
            if (estadoPedido == null)
            {
                return NotFound();
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", estadoPedido.EstadoId);
            return View(estadoPedido);
        }

        // POST: EstadoPedido/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion,EstadoId")] EstadoPedido estadoPedido)
        {
            if (id != estadoPedido.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var infoEstadoPedido = await _context.EstadoPedidos.FindAsync(id);
                    infoEstadoPedido.Descripcion = estadoPedido.Descripcion.Trim().ToUpper();
                    infoEstadoPedido.EstadoId = estadoPedido.EstadoId;
                    _context.Update(infoEstadoPedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadoPedidoExists(estadoPedido.Id))
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
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", estadoPedido.EstadoId);
            return View(estadoPedido);
        }

        // GET: EstadoPedido/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoPedido = await _context.EstadoPedidos
                .Include(e => e.Estado)
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estadoPedido == null)
            {
                return NotFound();
            }

            return View(estadoPedido);
        }

        // POST: EstadoPedido/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estadoPedido = await _context.EstadoPedidos.FindAsync(id);
            estadoPedido.EstadoId = 3;
            _context.Update(estadoPedido);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstadoPedidoExists(int id)
        {
            return _context.EstadoPedidos.Any(e => e.Id == id);
        }
    }
}
