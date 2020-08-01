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
    public class GastoController : Controller
    {
        private readonly MisComprasContext _context;

        public GastoController(MisComprasContext context)
        {
            _context = context;
        }

        // GET: Gasto
        public async Task<IActionResult> Index()
        {
            DateTime fechaIncial = new DateTime(DateTime.Now.Year,DateTime.Now.Month,1);
            DateTime fechaFinal = fechaIncial.AddMonths(1).AddDays(-1);
            decimal totalGastos = 0;
            List<Gasto> listaGastos = new List<Gasto>();
            List<Categoria> listaCategoria = new List<Categoria>();
            List<GastosPorCategoria> listaGastosPorCategoria = new List<GastosPorCategoria>();

            var categorias = _context.Categorias.Where(c => c.EstadoId == 1);
            listaCategoria = categorias.ToList();
            var misComprasContext = _context.Gastos.Include(g => g.Categoria).Include(g => g.Estado).Include(g => g.Usuario)
                .Where(g=>g.EstadoId!=3 && (g.FechaCreacion>=fechaIncial && g.FechaCreacion<=fechaFinal));

            listaGastos = misComprasContext.ToList();

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
            ViewBag.TotalGastos = totalGastos;
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre");
            return View();
        }

        // POST: Gasto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoriaId,Monto,Descripción,EstadoId,FechaCreacion,UsuarioId")] Gasto gasto)
        {
            if (ModelState.IsValid)
            {
                if (gasto.Monto > 0)
                {
                    gasto.FechaCreacion = DateTime.Now;
                    gasto.UsuarioId = Convert.ToInt32(HttpContext.Session.GetString("UsuarioId"));
                    gasto.Descripción = gasto.Descripción.Trim().ToUpper();
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", gasto.CategoriaId);
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", gasto.EstadoId);
            return View(gasto);
        }

        // POST: Gasto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoriaId,Monto,Descripción,EstadoId,FechaCreacion,UsuarioId")] Gasto gasto)
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
                    Infogasto.Descripción = gasto.Descripción.Trim().ToUpper();
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
