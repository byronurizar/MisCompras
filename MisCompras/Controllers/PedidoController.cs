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
    public class PedidoController : Controller
    {
        private readonly MisComprasContext _context;

        public PedidoController(MisComprasContext context)
        {
            _context = context;
        }

        // GET: Pedido
        public async Task<IActionResult> Index()
        {
            var misComprasContext = _context.Pedidos.Include(p => p.EstadoPedido).Include(p => p.Proveedor).Include(p => p.Usuario);
            return View(await misComprasContext.ToListAsync());
        }

        // GET: Pedido/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.EstadoPedido)
                .Include(p => p.Proveedor)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // GET: Pedido/Create
        public IActionResult Create()
        {
            ViewData["EstadoPedidoId"] = new SelectList(_context.EstadoPedidos, "Id", "Descripcion");
            ViewData["ProveedorId"] = new SelectList(_context.Proveedor, "Id", "Descripcion");
            return View();
        }

        // POST: Pedido/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProveedorId,EstadoPedidoId")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                pedido.FechaCreacion = DateTime.Now;
                pedido.UsuarioId = Convert.ToInt32(HttpContext.Session.GetString("UsuarioId"));
                _context.Add(pedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoPedidoId"] = new SelectList(_context.EstadoPedidos, "Id", "Descripcion", pedido.EstadoPedidoId);
            ViewData["ProveedorId"] = new SelectList(_context.Proveedor, "Id", "Descripcion", pedido.ProveedorId);
            if (pedido.Id > 0)
            {
                Redirect("~/PedidoDetalle/Index/" + pedido.Id);
            }
            return View(pedido);
        }

        // GET: Pedido/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            ViewData["EstadoPedidoId"] = new SelectList(_context.EstadoPedidos, "Id", "Descripcion", pedido.EstadoPedidoId);
            ViewData["ProveedorId"] = new SelectList(_context.Proveedor, "Id", "Descripcion", pedido.ProveedorId);
            return View(pedido);
        }

        // POST: Pedido/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProveedorId,EstadoPedidoId")] Pedido pedido)
        {
            if (id != pedido.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var infoPedido = await _context.Pedidos.FindAsync(id);
                    infoPedido.ProveedorId = pedido.ProveedorId;
                    infoPedido.EstadoPedidoId = pedido.EstadoPedidoId;
                    _context.Update(infoPedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.Id))
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
            ViewData["EstadoPedidoId"] = new SelectList(_context.EstadoPedidos, "Id", "Descripcion", pedido.EstadoPedidoId);
            ViewData["ProveedorId"] = new SelectList(_context.Proveedor, "Id", "Descripcion", pedido.ProveedorId);
            return View(pedido);
        }

        // GET: Pedido/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.EstadoPedido)
                .Include(p => p.Proveedor)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Pedido/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.Id == id);
        }
    }
}
