using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MisCompras.Models;
using MisCrompras.Models;

namespace MisCrompras.Controllers
{
    public class ClienteController : Controller
    {
        private readonly MisComprasContext _context;

        public ClienteController(MisComprasContext context)
        {
            _context = context;
        }

        // GET: Cliente
        public async Task<IActionResult> Index()
        {
            var misComprasContext = _context.Clientes.Include(c => c.Estado).Include(c => c.Municipio).Include(c => c.Usuario).Where(c=>c.EstadoId!=3);
            return View(await misComprasContext.ToListAsync());
        }

        public async Task<IActionResult> Deudores()
        {
            var creditoAbonos = _context.CreditoAbonos;
            var clientes = _context.Clientes;
            decimal totalSaldo = 0;
            List<CreditoAbono> listaCredioAbonos = new List<CreditoAbono>();
            List<Cliente> listaClientes = new List<Cliente>();
            listaCredioAbonos = creditoAbonos.ToList();
            listaClientes = clientes.ToList();
            List<Deudores> listaDeudores = new List<Deudores>();

            foreach (Cliente cliente in listaClientes)
            {
                int codigoCliente = cliente.Id;
                Deudores deudor = new Deudores();
                string nombre = cliente.Nombre + " " + cliente.Apellido;
                decimal totalCredito = 0;
                decimal totalAbonado = 0;
                decimal saldo = 0;

                totalCredito = listaCredioAbonos.Where(i => i.ClienteId == codigoCliente && i.Tipo == 0).Sum(s => s.Monto);
                totalAbonado = listaCredioAbonos.Where(i => i.ClienteId == codigoCliente && i.Tipo == 1).Sum(s => s.Monto);

                saldo = totalCredito - totalAbonado;
                if (saldo != 0)
                {
                    deudor.Id = codigoCliente;
                    deudor.Cliente = nombre;
                    deudor.Monto = saldo;
                    listaDeudores.Add(deudor);
                    totalSaldo += saldo;
                }
            }
            ViewBag.total = totalSaldo;
            return View(listaDeudores);
        }
        // GET: Cliente/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Estado)
                .Include(c => c.Municipio)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Cliente/Create
        public IActionResult Create()
        {
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion");
            ViewData["MunicipioId"] = new SelectList(_context.Municipios, "Id", "Descripcion");
            return View();
        }

        // POST: Cliente/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,MunicipioId,Direccion,Telefono,EstadoId")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.FechaCreacion = DateTime.Now;
                cliente.UsuarioId = Convert.ToInt32(HttpContext.Session.GetString("UsuarioId"));
                cliente.Nombre = cliente.Nombre.Trim().ToUpper();
                cliente.Apellido = cliente.Apellido.Trim().ToUpper();
                cliente.Direccion = cliente.Direccion.Trim().ToUpper();
                cliente.Telefono = cliente.Telefono.Trim();
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", cliente.EstadoId);
            ViewData["MunicipioId"] = new SelectList(_context.Municipios, "Id", "Descripcion", cliente.MunicipioId);
            return View(cliente);
        }

        // GET: Cliente/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", cliente.EstadoId);
            ViewData["MunicipioId"] = new SelectList(_context.Municipios, "Id", "Descripcion", cliente.MunicipioId);
            
            return View(cliente);
        }

        // POST: Cliente/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,MunicipioId,Direccion,Telefono,EstadoId")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var infoClinete = await _context.Clientes.FindAsync(id);
                    infoClinete.Nombre = cliente.Nombre.Trim().ToUpper();
                    infoClinete.Apellido = cliente.Apellido.Trim().ToUpper();
                    infoClinete.MunicipioId = cliente.MunicipioId;
                    infoClinete.EstadoId = cliente.EstadoId;
                    infoClinete.Direccion = cliente.Direccion.Trim().ToUpper();
                    infoClinete.Telefono = cliente.Telefono.Trim();
                    _context.Update(infoClinete);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
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
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", cliente.EstadoId);
            ViewData["MunicipioId"] = new SelectList(_context.Municipios, "Id", "Descripcion", cliente.MunicipioId);
            return View(cliente);
        }

        // GET: Cliente/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Estado)
                .Include(c => c.Municipio)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            cliente.EstadoId = 3;
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
