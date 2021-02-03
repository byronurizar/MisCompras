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
    public class CreditoAbonoController : Controller
    {
        private readonly MisComprasContext _context;

        public CreditoAbonoController(MisComprasContext context)
        {
            _context = context;
        }

        // GET: CreditoAbono
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.clienteId = id;
            var cliente = _context.Clientes.Where(c=>c.Id==id);
            List<Cliente> clientes = new List<Cliente>();
            clientes = cliente.ToList();
            string nombreCliente = string.Empty;
            foreach (Cliente item in clientes)
            {
                nombreCliente = item.Nombre +" "+item.Apellido;
                break;
            }
            ViewBag.nombreCliente = nombreCliente;
            decimal totalAbono = 0;
            decimal totalCredito = 0;
            decimal saldo = 0;

            var misComprasContext = _context.CreditoAbonos.Include(c => c.Cliente).Include(c => c.Estado).Include(c => c.Usuario).Where(c=>c.ClienteId==id).OrderByDescending(f=>f.FechaCreacion);
            foreach (CreditoAbono item in misComprasContext)
            {
                if (item.Tipo == 0)
                {
                    totalCredito += item.Monto;
                }
                else
                {
                    totalAbono += item.Monto;
                }
            }
            saldo = totalCredito - totalAbono;
            ViewBag.saldo = saldo;
            return View(await misComprasContext.ToListAsync());
        }

        // GET: CreditoAbono/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creditoAbono = await _context.CreditoAbonos
                .Include(c => c.Cliente)
                .Include(c => c.Estado)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (creditoAbono == null)
            {
                return NotFound();
            }

            return View(creditoAbono);
        }

        // GET: CreditoAbono/Create

        public IActionResult Create(int id)
        {
            var cliente = _context.Clientes.Where(c => c.Id == id);
            List<Cliente> clientes = new List<Cliente>();
            clientes = cliente.ToList();
            string nombreCliente = string.Empty;
            foreach (Cliente item in clientes)
            {
                nombreCliente = item.Nombre + " " + item.Apellido;
                break;
            }
            ViewBag.nombreCliente = nombreCliente;

            ViewBag.ClienteId = id;
            return View();
        }

        // POST: CreditoAbono/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("CreditoAbono/Create/{clienteId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int clienteId,[Bind("Id,ClienteId,Monto,Tipo,Descripcion,EstadoId,FechaCreacion,UsuarioId")] CreditoAbono creditoAbono)
        {
            if (clienteId <= 0)
            {
                return NotFound();
            }
            var cliente = _context.Clientes.Where(p => p.Id == clienteId);
            if (cliente == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                creditoAbono.FechaCreacion = DateTime.Now;
                creditoAbono.UsuarioId = Convert.ToInt32(HttpContext.Session.GetString("UsuarioId"));
                creditoAbono.Descripcion = creditoAbono.Descripcion.Trim().ToUpper();
                creditoAbono.EstadoId = 1;
                creditoAbono.Tipo = 0;
                creditoAbono.ClienteId = clienteId;
                _context.Add(creditoAbono);
                await _context.SaveChangesAsync();
                return Redirect("~/CreditoAbono/Index/" + clienteId);
            }
            return View(creditoAbono);
        }
        public IActionResult Abono(int id)
        {
            var cliente = _context.Clientes.Where(c => c.Id == id);
            List<Cliente> clientes = new List<Cliente>();
            clientes = cliente.ToList();
            string nombreCliente = string.Empty;
            foreach (Cliente item in clientes)
            {
                nombreCliente = item.Nombre + " " + item.Apellido;
                break;
            }
            ViewBag.nombreCliente = nombreCliente;

            ViewBag.ClienteId = id;
            return View();
        }
        [HttpPost]
        [Route("CreditoAbono/Abono/{clienteId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Abono(int clienteId, [Bind("Id,ClienteId,Monto,Tipo,Descripcion,EstadoId,FechaCreacion,UsuarioId")] CreditoAbono creditoAbono)
        {
            if (clienteId <= 0)
            {
                return NotFound();
            }
            var cliente = _context.Clientes.Where(p => p.Id == clienteId);
            if (cliente == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                creditoAbono.FechaCreacion = DateTime.Now;
                creditoAbono.UsuarioId = Convert.ToInt32(HttpContext.Session.GetString("UsuarioId"));
                creditoAbono.Descripcion = string.Empty;
                creditoAbono.EstadoId = 1;
                creditoAbono.Tipo = 1;
                creditoAbono.ClienteId = clienteId;
                _context.Add(creditoAbono);
                await _context.SaveChangesAsync();
                return Redirect("~/CreditoAbono/Index/" + clienteId);
            }
            return View(creditoAbono);
        }
        // GET: CreditoAbono/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var creditoAbono = await _context.CreditoAbonos.FindAsync(id);
        //    if (creditoAbono == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", creditoAbono.ClienteId);
        //    ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", creditoAbono.EstadoId);
        //    ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Password", creditoAbono.UsuarioId);
        //    return View(creditoAbono);
        //}

        //// POST: CreditoAbono/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,ClienteId,Monto,Tipo,Descripcion,EstadoId,FechaCreacion,UsuarioId")] CreditoAbono creditoAbono)
        //{
        //    if (id != creditoAbono.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(creditoAbono);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CreditoAbonoExists(creditoAbono.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", creditoAbono.ClienteId);
        //    ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", creditoAbono.EstadoId);
        //    ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Password", creditoAbono.UsuarioId);
        //    return View(creditoAbono);
        //}

        //// GET: CreditoAbono/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var creditoAbono = await _context.CreditoAbonos
        //        .Include(c => c.Cliente)
        //        .Include(c => c.Estado)
        //        .Include(c => c.Usuario)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (creditoAbono == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(creditoAbono);
        //}

        //// POST: CreditoAbono/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var creditoAbono = await _context.CreditoAbonos.FindAsync(id);
        //    _context.CreditoAbonos.Remove(creditoAbono);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool CreditoAbonoExists(int id)
        {
            return _context.CreditoAbonos.Any(e => e.Id == id);
        }
    }
}
