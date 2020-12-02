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
    public class GastoController : Controller
    {
        private readonly MisComprasContext _context;

        public GastoController(MisComprasContext context)
        {
            _context = context;
        }

        public List<SubCategoria> ListSubCategoria([FromQuery] int idCategoria)
        {
            List<SubCategoria> lista = new List<SubCategoria>();
            var aux = _context.SubCategoria.Where(item => item.EstadoId == 1 && item.CategoriaId == idCategoria);
            lista = aux.ToList();
            return lista;
        }

        // GET: Gasto
        public async Task<IActionResult> Index()
        {

            int anioActual = DateTime.Now.Year;
            int mesActual = DateTime.Now.Month;
            List<Presupuesto> presuestoMesActual = new List<Presupuesto>();

            presuestoMesActual = _context.Presupuesto.Where(item => item.EstadoId == 1 && item.Anio == anioActual && item.Mes == mesActual).ToList();

            if (presuestoMesActual.Count == 0)
            {
                List<Presupuesto> presuestoMesAnterior = new List<Presupuesto>();
                if (mesActual == 1)
                {
                    mesActual = 12;
                    anioActual = anioActual - 1;
                }
                else
                {
                    mesActual = mesActual - 1;
                }
                presuestoMesAnterior = _context.Presupuesto.Where(item => item.EstadoId == 1 && item.Anio == anioActual && item.Mes == mesActual).ToList();
                if (presuestoMesAnterior.Count > 0)
                {
                    foreach (Presupuesto itemPresupuesto in presuestoMesAnterior)
                    {
                        List<PresupuestoDetalle> listDetallePresupuesto = new List<PresupuestoDetalle>();
                        listDetallePresupuesto=_context.PresupuestoDetalle.Where(item => item.EstadoId == 1 && item.PresupuestoId== itemPresupuesto.Id).ToList();
                        if (listDetallePresupuesto.Count > 0)
                        {
                            Presupuesto presuesto = new Presupuesto();
                            presuesto.FechaCreacion = DateTime.Now;
                            presuesto.UsuarioId = Convert.ToInt32(HttpContext.Session.GetString("UsuarioId"));
                            presuesto.Anio = DateTime.Now.Year;
                            presuesto.Mes = DateTime.Now.Month;
                            presuesto.Nombre="PRESUPUESTO AUT. "+ DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                            presuesto.Nombre = presuesto.Nombre.Trim().ToUpper();
                            presuesto.EstadoId = 1;

                            _context.Add(presuesto);
                            await _context.SaveChangesAsync();

                            foreach (PresupuestoDetalle itemDetalle in listDetallePresupuesto)
                            {
                                PresupuestoDetalle DetallePresu = new PresupuestoDetalle();
                                DetallePresu.PresupuestoId = presuesto.Id;
                                DetallePresu.CategoriaId = itemDetalle.CategoriaId;
                                DetallePresu.Monto = itemDetalle.Monto;
                                DetallePresu.EstadoId = itemDetalle.EstadoId;
                                DetallePresu.FechaCreacion = DateTime.Now;
                                DetallePresu.UsuarioId = Convert.ToInt32(HttpContext.Session.GetString("UsuarioId"));
                                _context.Add(DetallePresu);
                                await _context.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            TempData["mensaje"] = "No existe detalle del presupuesto del mes anterior, por favor registrelo manualmente.";
                        }

                        break;
                    }
                   
                }
                else
                {
                    TempData["mensaje"] = "No existe presupuesto para el mes actual, se intento obtener el presupuesto del mes anterior pero tampoco existe, por favor registrelo manualmente.";
                }
            }
            else
            {
                foreach (Presupuesto item in presuestoMesActual)
                {
                    List<PresupuestoDetalle> listDetallePresupuesto = new List<PresupuestoDetalle>();
                    listDetallePresupuesto = _context.PresupuestoDetalle.Where(i => i.EstadoId == 1 && i.PresupuestoId == item.Id).ToList();
                    if (listDetallePresupuesto.Count== 0)
                    {
                        TempData["mensaje"] = "Existe presupuesto para este mes pero no tiene un detalle, por favor verifique";
                    }
                    }
            }


            DateTime fechaIncial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime fechaFinal = fechaIncial.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59);
            decimal totalGastos = 0;
            List<Gasto> listaGastos = new List<Gasto>();
            List<Categoria> listaCategoria = new List<Categoria>();
            List<GastosPorCategoria> listaGastosPorCategoria = new List<GastosPorCategoria>();

            var categorias = _context.Categorias.Where(c => c.EstadoId != 3);
            listaCategoria = categorias.ToList();
            var misComprasContext = _context.Gastos.Include(g => g.Categoria).Include(c => c.SubCategoria).Include(g => g.Estado).Include(g => g.Usuario)
                .Where(g => g.EstadoId != 3 && (g.FechaCreacion >= fechaIncial && g.FechaCreacion <= fechaFinal));

            listaGastos = misComprasContext.ToList();

            decimal totalGastosVentas = listaGastos.Where(g => g.CategoriaId == 28).Sum(s => s.Monto);
            foreach (Categoria categoria in listaCategoria)
            {
                int codigoCategoria = categoria.Id;
                string nombre = categoria.Nombre;
                GastosPorCategoria agruparGasto = new GastosPorCategoria();
                decimal monto = listaGastos.Where(g => g.CategoriaId == codigoCategoria).Sum(s => s.Monto);
                if (monto > 0)
                {
                    agruparGasto.Categoria = nombre;
                    agruparGasto.Monto = monto;
                    totalGastos += monto;
                    listaGastosPorCategoria.Add(agruparGasto);
                }
            }
            List<GastosPorCategoria> listaOrdenada = new List<GastosPorCategoria>();
            listaOrdenada = listaGastosPorCategoria.OrderByDescending(o => o.Monto).ToList();
            ViewBag.Agrupacion = listaOrdenada;
            ViewBag.Total = totalGastos;
            ViewBag.TotalGastos = totalGastos - totalGastosVentas;
            ViewBag.totalGastosVentas = totalGastosVentas;
            listaGastos = listaGastos.OrderBy(item => item.Id).ToList();
            return View(listaGastos);
        }

        // GET: Gasto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gasto = await _context.Gastos
                .Include(g => g.Categoria)
                .Include(g => g.SubCategoria)
                .Include(g => g.Estado)
                .Include(g => g.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gasto == null)
            {
                return NotFound();
            }

            return View(gasto);
        }

        // GET: Gasto/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias.Where(item => item.EstadoId == 1), "Id", "Nombre");
            return View();
        }

        // POST: Gasto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoriaId,SubCategoriaId,Monto,Descripcion,EstadoId")] Gasto gasto)
        {
            if (ModelState.IsValid)
            {
                if (gasto.Monto > 0)
                {
                    gasto.FechaCreacion = DateTime.Now;
                    gasto.UsuarioId = Convert.ToInt32(HttpContext.Session.GetString("UsuarioId"));
                    if (gasto.Descripcion == null)
                    {
                        gasto.Descripcion = "";
                    }
                    else
                    {
                        gasto.Descripcion = gasto.Descripcion.Trim().ToUpper();
                    }
                    if (gasto.SubCategoriaId == 0)
                    {
                        gasto.SubCategoriaId = null;
                    }
                    gasto.EstadoId = 1;
                    _context.Add(gasto);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", gasto.CategoriaId);
            return View(gasto);
        }

        // GET: Gasto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gasto = await _context.Gastos.FindAsync(id);
            if (gasto == null)
            {
                return NotFound();
            }
            TempData["Prueba"] =gasto.SubCategoriaId;
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", gasto.CategoriaId);
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", gasto.EstadoId);
            return View(gasto);
        }

        // POST: Gasto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoriaId,SubCategoriaId,Monto,Descripcion,EstadoId,FechaCreacion,UsuarioId")] Gasto gasto)
        {
            if (id != gasto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var Infogasto = await _context.Gastos.FindAsync(id);
                    Infogasto.Monto = gasto.Monto;
                    if (gasto.Descripcion == null)
                    {
                        Infogasto.Descripcion = "";
                    }
                    else
                    {
                        Infogasto.Descripcion = gasto.Descripcion.Trim().ToUpper();
                    }
                    if (gasto.SubCategoriaId == 0)
                    {
                        Infogasto.SubCategoriaId = null;
                    }
                    else
                    {
                        Infogasto.SubCategoriaId = gasto.SubCategoriaId;
                    }
                    Infogasto.EstadoId = gasto.EstadoId;
                    Infogasto.CategoriaId = gasto.CategoriaId;
                    _context.Update(Infogasto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GastoExists(gasto.Id))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", gasto.CategoriaId);
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", gasto.EstadoId);
            return View(gasto);
        }

        // GET: Gasto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gasto = await _context.Gastos
                .Include(g => g.Categoria)
                .Include(g => g.SubCategoria)
                .Include(g => g.Estado)
                .Include(g => g.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gasto == null)
            {
                return NotFound();
            }

            return View(gasto);
        }

        // POST: Gasto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gasto = await _context.Gastos.FindAsync(id);
            gasto.EstadoId = 3;
            _context.Update(gasto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GastoExists(int id)
        {
            return _context.Gastos.Any(e => e.Id == id);
        }
    }
}
