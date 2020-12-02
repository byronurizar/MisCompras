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
    public class SubCategoriaController : Controller
    {
        private readonly MisComprasContext _context;

        public SubCategoriaController(MisComprasContext context)
        {
            _context = context;
        }

        // GET: Gasto
        public async Task<IActionResult> Index()
        {
            var misComprasContext = _context.SubCategoria.Include(c => c.Estado).Include(c => c.Usuario).Include(c=>c.Categoria).Where(c => c.EstadoId != 3);
            return View(await misComprasContext.ToListAsync());
        }

        // GET: Gasto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gasto = await _context.SubCategoria
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias.Where(item=>item.EstadoId==1), "Id", "Nombre");
            return View();
        }

        // POST: Gasto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoriaId,Nombre")] SubCategoria subcategoria)
        {
            if (ModelState.IsValid)
            {
                subcategoria.FechaCreacion = DateTime.Now;
                subcategoria.UsuarioId = Convert.ToInt32(HttpContext.Session.GetString("UsuarioId"));
                subcategoria.Nombre = subcategoria.Nombre.Trim().ToUpper();
                subcategoria.EstadoId = 1;
                _context.Add(subcategoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", subcategoria.CategoriaId);
            return View(subcategoria);
        }

        // GET: Gasto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gasto = await _context.SubCategoria.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoriaId,Nombre,EstadoId")] SubCategoria subCategoria)
        {
            if (id != subCategoria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var infoSubCategoria = await _context.SubCategoria.FindAsync(id);
                    infoSubCategoria.Nombre = subCategoria.Nombre.Trim().ToUpper();
                    infoSubCategoria.EstadoId = subCategoria.EstadoId;
                    infoSubCategoria.CategoriaId = subCategoria.CategoriaId;
                    _context.Update(infoSubCategoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GastoExists(subCategoria.Id))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", subCategoria.CategoriaId);
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", subCategoria.EstadoId);
            return View(subCategoria);
        }

        // GET: Gasto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gasto = await _context.SubCategoria
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
            var subcategoria = await _context.SubCategoria.FindAsync(id);
            subcategoria.EstadoId = 3;
            _context.Update(subcategoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GastoExists(int id)
        {
            return _context.SubCategoria.Any(e => e.Id == id);
        }
    }
}
