using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class PresupuestoController : Controller
    {
        private readonly MisComprasContext _context;

        public PresupuestoController(MisComprasContext context)
        {
            _context = context;
        }

        // GET: Pedido
        public async Task<IActionResult> Index()
        {
            var misComprasContext = _context.Presupuesto.Include(p => p.Estado).Include(p => p.Usuario);
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
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion");
            ViewData["ProveedorId"] = new SelectList(_context.Proveedor, "Id", "Descripcion");
            
            List<AuxListBox> listAnio = new List<AuxListBox>();
            List<AuxListBox> listMes = new List<AuxListBox>();
            int mesActual = DateTime.Now.Month;

            AuxListBox itemAnio = new AuxListBox();
            itemAnio.id = DateTime.Now.Year;
            itemAnio.descripcion = "Año " + DateTime.Now.Year;
            listAnio.Add(itemAnio);

            if (mesActual == 12) {
                itemAnio = new AuxListBox();
                itemAnio.id = DateTime.Now.Year + 1;
                itemAnio.descripcion = "Año " + DateTime.Now.AddYears(1).Year;
                listAnio.Add(itemAnio);
            }
            DateTime fechaActual = new DateTime(DateTime.Now.Year, mesActual-1, 01);
            for (int i = 1; i <= 2; i++)
            {
                AuxListBox itemMes = new AuxListBox();
                fechaActual = fechaActual.AddMonths(1);
                itemMes.id = fechaActual.Month;
                string descripcion= fechaActual.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                string letraInicial = descripcion.Substring(0, 1).ToUpper();
                itemMes.descripcion =letraInicial+descripcion.Substring(1,descripcion.Length-1);
                listMes.Add(itemMes);
            }


            ViewData["AnioId"] = new SelectList(listAnio,"id", "descripcion");
            ViewData["MesId"] = new SelectList(listMes, "id", "descripcion");

            return View();
        }

        // POST: Pedido/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Anio,Mes")] Presupuesto pedido)
        {
            if (ModelState.IsValid)
            {
                pedido.FechaCreacion = DateTime.Now;
                pedido.UsuarioId = Convert.ToInt32(HttpContext.Session.GetString("UsuarioId"));
                pedido.EstadoId = 1;
                _context.Add(pedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", pedido.EstadoId);
            if (pedido.Id > 0)
            {
                Redirect("~/PresupuestoDetalle/Index/" + pedido.Id);
            }
            List<AuxListBox> listAnio = new List<AuxListBox>();
            List<AuxListBox> listMes = new List<AuxListBox>();
            int mesActual = DateTime.Now.Month;

            AuxListBox itemAnio = new AuxListBox();
            itemAnio.id = DateTime.Now.Year;
            itemAnio.descripcion = "Año " + DateTime.Now.Year;
            listAnio.Add(itemAnio);

            if (mesActual == 12)
            {
                itemAnio = new AuxListBox();
                itemAnio.id = DateTime.Now.Year + 1;
                itemAnio.descripcion = "Año " + DateTime.Now.AddYears(1).Year;
                listAnio.Add(itemAnio);
            }
            DateTime fechaActual = new DateTime(DateTime.Now.Year, mesActual - 1, 01);
            for (int i = 1; i <= 2; i++)
            {
                AuxListBox itemMes = new AuxListBox();
                fechaActual = fechaActual.AddMonths(1);
                itemMes.id = fechaActual.Month;
                string descripcion = fechaActual.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                string letraInicial = descripcion.Substring(0, 1).ToUpper();
                itemMes.descripcion = letraInicial + descripcion.Substring(1, descripcion.Length - 1);
                listMes.Add(itemMes);
            }


            ViewData["AnioId"] = new SelectList(listAnio, "id", "descripcion");
            ViewData["MesId"] = new SelectList(listMes, "id", "descripcion");


            return View(pedido);
        }

        // GET: Pedido/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Presupuesto.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", pedido.EstadoId);

            List<AuxListBox> listAnio = new List<AuxListBox>();
            List<AuxListBox> listMes = new List<AuxListBox>();
            int mesActual = DateTime.Now.Month;

            AuxListBox itemAnio = new AuxListBox();
            itemAnio.id = DateTime.Now.Year;
            itemAnio.descripcion = "Año " + DateTime.Now.Year;
            listAnio.Add(itemAnio);

            if (mesActual == 12)
            {
                itemAnio = new AuxListBox();
                itemAnio.id = DateTime.Now.Year + 1;
                itemAnio.descripcion = "Año " + DateTime.Now.AddYears(1).Year;
                listAnio.Add(itemAnio);
            }
            DateTime fechaActual = new DateTime(DateTime.Now.Year, mesActual - 1, 01);
            for (int i = 1; i <= 2; i++)
            {
                AuxListBox itemMes = new AuxListBox();
                fechaActual = fechaActual.AddMonths(1);
                itemMes.id = fechaActual.Month;
                string descripcion = fechaActual.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                string letraInicial = descripcion.Substring(0, 1).ToUpper();
                itemMes.descripcion = letraInicial + descripcion.Substring(1, descripcion.Length - 1);
                listMes.Add(itemMes);
            }


            ViewData["AnioId"] = new SelectList(listAnio, "id", "descripcion");
            ViewData["MesId"] = new SelectList(listMes, "id", "descripcion");


            return View(pedido);
        }

        // POST: Pedido/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Anio,Mes,EstadoId")] Presupuesto pedido)
        {
            if (id != pedido.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var infoPedido = await _context.Presupuesto.FindAsync(id);
                    infoPedido.Nombre = pedido.Nombre;
                    infoPedido.Anio = pedido.Anio;
                    infoPedido.Mes = pedido.Mes;
                    infoPedido.EstadoId = pedido.EstadoId;
                    _context.Update(infoPedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PresupuestoExists(pedido.Id))
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
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", pedido.EstadoId);
            List<AuxListBox> listAnio = new List<AuxListBox>();
            List<AuxListBox> listMes = new List<AuxListBox>();
            int mesActual = DateTime.Now.Month;

            AuxListBox itemAnio = new AuxListBox();
            itemAnio.id = DateTime.Now.Year;
            itemAnio.descripcion = "Año " + DateTime.Now.Year;
            listAnio.Add(itemAnio);

            if (mesActual == 12)
            {
                itemAnio = new AuxListBox();
                itemAnio.id = DateTime.Now.Year + 1;
                itemAnio.descripcion = "Año " + DateTime.Now.AddYears(1).Year;
                listAnio.Add(itemAnio);
            }
            DateTime fechaActual = new DateTime(DateTime.Now.Year, mesActual - 1, 01);
            for (int i = 1; i <= 2; i++)
            {
                AuxListBox itemMes = new AuxListBox();
                fechaActual = fechaActual.AddMonths(1);
                itemMes.id = fechaActual.Month;
                string descripcion = fechaActual.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                string letraInicial = descripcion.Substring(0, 1).ToUpper();
                itemMes.descripcion = letraInicial + descripcion.Substring(1, descripcion.Length - 1);
                listMes.Add(itemMes);
            }


            ViewData["AnioId"] = new SelectList(listAnio, "id", "descripcion");
            ViewData["MesId"] = new SelectList(listMes, "id", "descripcion");


            return View(pedido);
        }

        // GET: Pedido/Delete/5
      
        private bool PresupuestoExists(int id)
        {
            return _context.Presupuesto.Any(e => e.Id == id);
        }
    }
}
