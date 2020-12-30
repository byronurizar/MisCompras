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
    public class PedidoDetalleController : Controller
    {
        private readonly MisComprasContext _context;

        public PedidoDetalleController(MisComprasContext context)
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

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            ViewData["PedidoId"] = id;

            var misComprasContext = _context.PedidoDetalles.Include(p => p.Cliente).Include(p => p.Estado).Include(p => p.Marca).Include(p => p.Pedido).Include(p => p.Usuario).Where(p=>p.PedidoId==id && p.EstadoId!=3);
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

            var pedidoDetalle = await _context.PedidoDetalles
                .Include(p => p.Cliente)
                .Include(p => p.Estado)
                .Include(p => p.Marca)
                .Include(p => p.Pedido)
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
            ViewData["PedidoId"]= id;
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            ViewData["Clientes"] = _context.Clientes;
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion");
            ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Descripcion");
            return View();
        }

        // POST: PedidoDetalle/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("PedidoDetalle/Create/{PedidoId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int PedidoId, [Bind("Id,Pagina,Talla,Color,PrecioCompra,Cantidad,PedidoId,ClienteId,MarcaId,EstadoId,Descripcion")] PedidoDetalle pedidoDetalle)
        {
            if (PedidoId <= 0)
            {
                return NotFound();
            }
            var pedido = _context.Pedidos.Where(p=>p.Id==PedidoId);
            if (pedido == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                pedidoDetalle.PedidoId = PedidoId;
                pedidoDetalle.EstadoId =1;
                pedidoDetalle.FechaCreacion = DateTime.Now;
                pedidoDetalle.UsuarioId = Convert.ToInt32(HttpContext.Session.GetString("UsuarioId"));
                pedidoDetalle.Descripcion = pedidoDetalle.Descripcion.Trim().ToUpper();
                if (pedidoDetalle.Pagina <= 0)
                {
                    pedidoDetalle.Pagina = 0;
                }
                if (pedidoDetalle.Talla==null)
                {
                    pedidoDetalle.Talla = "N/A";
                }
                if (pedidoDetalle.Color == null)
                {
                    pedidoDetalle.Color = "N/A";
                }
                _context.Add(pedidoDetalle);
                await _context.SaveChangesAsync();
                return Redirect("~/PedidoDetalle/Index/" +PedidoId);
            }
            ViewData["Clientes"] = _context.Clientes;
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", pedidoDetalle.EstadoId);
            ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Descripcion", pedidoDetalle.MarcaId);
            return View(pedidoDetalle);
        }

        // GET: PedidoDetalle/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoDetalle = await _context.PedidoDetalles.FindAsync(id);
            if (pedidoDetalle == null)
            {
                return NotFound();
            }
            ViewData["Clientes"] = _context.Clientes;
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", pedidoDetalle.EstadoId);
            ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Descripcion", pedidoDetalle.MarcaId);
            return View(pedidoDetalle);
        }
        [HttpGet]
        public RespuestaModel InfoMarca(int MarcaId)
        {
            RespuestaModel rsp = new RespuestaModel();
            try
            {
                rsp.codigo = 0;
                rsp.valor = _context.Marcas.Where(m=>m.Id== MarcaId);
            }catch(Exception ex)
            {
                rsp.codigo = -1;
                rsp.error = ex.ToString();
                rsp.valor = null;
            }
            return rsp;
        }
        // POST: PedidoDetalle/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Pagina,Talla,Color,PrecioCompra,Cantidad,ClienteId,MarcaId,Descripcion")] PedidoDetalle pedidoDetalle)
        {
            var pedidoId=0;
            if (id != pedidoDetalle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var infoPedidoDetalle = await _context.PedidoDetalles.FindAsync(id);
                pedidoId = infoPedidoDetalle.PedidoId;
                try
                {
                    
                    infoPedidoDetalle.Pagina = pedidoDetalle.Pagina;
                    infoPedidoDetalle.Talla = pedidoDetalle.Talla;
                    infoPedidoDetalle.Color = pedidoDetalle.Color;
                    if (pedidoDetalle.Pagina <= 0)
                    {
                        infoPedidoDetalle.Pagina = 0;
                    }
                    
                    if (pedidoDetalle.Talla==null)
                    {
                        infoPedidoDetalle.Talla = "N/A";
                    }
                    if (pedidoDetalle.Color == null)
                    {
                        infoPedidoDetalle.Color = "N/A";
                    }
                    infoPedidoDetalle.PrecioCompra = pedidoDetalle.PrecioCompra;
                    infoPedidoDetalle.Cantidad = pedidoDetalle.Cantidad;
                    infoPedidoDetalle.ClienteId = pedidoDetalle.ClienteId;
                    infoPedidoDetalle.Descripcion = pedidoDetalle.Descripcion.ToUpper().Trim();
                    infoPedidoDetalle.MarcaId = pedidoDetalle.MarcaId;
                    
                    
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
                return Redirect("~/PedidoDetalle/Index/"+ pedidoId);
            }
            ViewData["Clientes"] = _context.Clientes;
            ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Descripcion", pedidoDetalle.MarcaId);
            return View(pedidoDetalle);
        }

        // GET: PedidoDetalle/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoDetalle = await _context.PedidoDetalles
                .Include(p => p.Cliente)
                .Include(p => p.Estado)
                .Include(p => p.Marca)
                .Include(p => p.Pedido)
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
            var pedidoDetalle = await _context.PedidoDetalles.FindAsync(id);
            idPedido = pedidoDetalle.PedidoId;
            pedidoDetalle.EstadoId = 3;
            _context.PedidoDetalles.Update(pedidoDetalle);
            await _context.SaveChangesAsync();
            return Redirect("~/PedidoDetalle/Index/" + idPedido);
        }

        private bool PedidoDetalleExists(int id)
        {
            return _context.PedidoDetalles.Any(e => e.Id == id);
        }
    }
}
