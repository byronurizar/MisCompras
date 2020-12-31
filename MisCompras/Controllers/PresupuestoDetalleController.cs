using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MisCompras.Models;
using MisCrompras.Models;

namespace MisCrompras.Controllers
{
    public class PresupuestoDetalleController : Controller
    {
        private readonly MisComprasContext _context;

        public PresupuestoDetalleController(MisComprasContext context)
        {
            _context = context;
        }

        // GET: PedidoDetalle
        public async Task<IActionResult> Index(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var pedido = await _context.Presupuesto.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            ViewData["PresupuestoId"] = id;

            var misComprasContext = _context.PresupuestoDetalle.Include(p => p.Categoria).Include(p => p.Estado).Include(p => p.Presupuesto).Include(p => p.Usuario).Where(p=>p.PresupuestoId==id && p.EstadoId!=3);
            return View(await misComprasContext.ToListAsync());
        }
        public async Task<IActionResult> generarPedido(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            ViewData["PedidoId"] = id;

            var misComprasContext = _context.PedidoDetalles.Include(p => p.Cliente).Include(p => p.Estado).Include(p => p.Marca).Include(p => p.Pedido).Include(p => p.Usuario).Where(p => p.PedidoId == id && p.EstadoId != 3);
            return View(await misComprasContext.ToListAsync());
        }

        // GET: PedidoDetalle/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoDetalle = await _context.PresupuestoDetalle
                .Include(p => p.Categoria)
                .Include(p => p.Estado)
                .Include(p => p.Presupuesto)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedidoDetalle == null)
            {
                return NotFound();
            }

            return View(pedidoDetalle);
        }

        // GET: PedidoDetalle/Create
        public async Task<IActionResult> Create(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            ViewData["PresupuestoId"]= id;
            var pedido = await _context.Presupuesto.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            ViewData["CategoriaId"] = new SelectList(_context.Categorias.Where(item => item.EstadoId == 1), "Id", "Nombre");
            return View();
        }

        // POST: PedidoDetalle/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("PresupuestoDetalle/Create/{PresupuestoId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int PresupuestoId, [Bind("Id,CategoriaId,Monto,PresupuestoId")] PresupuestoDetalle pedidoDetalle)
        {
            if (PresupuestoId <= 0)
            {
                return NotFound();
            }
            var pedido = _context.Presupuesto.Where(p=>p.Id== PresupuestoId);
            if (pedido == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                pedidoDetalle.PresupuestoId = PresupuestoId;
                pedidoDetalle.EstadoId =1;
                pedidoDetalle.FechaCreacion = DateTime.Now;
                pedidoDetalle.UsuarioId = Convert.ToInt32(HttpContext.Session.GetString("UsuarioId"));
                pedidoDetalle.Monto = pedidoDetalle.Monto;
                pedidoDetalle.CategoriaId = pedidoDetalle.CategoriaId;
               
                _context.Add(pedidoDetalle);
                await _context.SaveChangesAsync();
                return Redirect("~/PresupuestoDetalle/Index/" + PresupuestoId);
            }
            
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre");
            return View(pedidoDetalle);
        }

        // GET: PedidoDetalle/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoDetalle = await _context.PresupuestoDetalle.FindAsync(id);
            if (pedidoDetalle == null)
            {
                return NotFound();
            }
            ViewBag.EstadoId = new SelectList(_context.Estados, "Id", "Descripcion");
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre");
            return View(pedidoDetalle);
        }
        // POST: PedidoDetalle/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoriaId,Monto,EstadoId")] PresupuestoDetalle pedidoDetalle)
        {
            var pedidoId=0;
            if (id != pedidoDetalle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var infoPedidoDetalle = await _context.PresupuestoDetalle.FindAsync(id);
                pedidoId = infoPedidoDetalle.PresupuestoId;
                try
                {
                    
                   
                    infoPedidoDetalle.Monto = pedidoDetalle.Monto;
                    infoPedidoDetalle.CategoriaId = pedidoDetalle.CategoriaId;
                    infoPedidoDetalle.EstadoId = pedidoDetalle.EstadoId;
                    
                    _context.Update(infoPedidoDetalle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoDetalleExists(pedidoDetalle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("~/PresupuestoDetalle/Index/"+ pedidoId);
            }
            ViewBag.EstadoId = new SelectList(_context.Estados, "Id", "Descripcion");
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre");
            return View(pedidoDetalle);
        }

        // GET: PedidoDetalle/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoDetalle = await _context.PresupuestoDetalle
                .Include(p => p.Categoria)
                .Include(p => p.Estado)
                .Include(p => p.Presupuesto)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedidoDetalle == null)
            {
                return NotFound();
            }

            return View(pedidoDetalle);
        }

        // POST: PedidoDetalle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var idPedido = 0;
            var pedidoDetalle = await _context.PresupuestoDetalle.FindAsync(id);
            idPedido = pedidoDetalle.PresupuestoId;
            pedidoDetalle.EstadoId = 3;
            _context.PresupuestoDetalle.Update(pedidoDetalle);
            await _context.SaveChangesAsync();
            return Redirect("~/PresupuestoDetalle/Index/" + idPedido);
        }

        private bool PedidoDetalleExists(int id)
        {
            return _context.PresupuestoDetalle.Any(e => e.Id == id);
        }
    }
}
