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
    public class MunicipioController : Controller
    {
        private readonly MisComprasContext _context;

        public MunicipioController(MisComprasContext context)
        {
            _context = context;
        }

        // GET: Municipio
        public async Task<IActionResult> Index()
        {
            var misComprasContext = _context.Municipios.Include(m => m.Departamento).Include(m => m.Estado).Include(m => m.Usuario).Where(a=>a.EstadoId!=3);
            return View(await misComprasContext.ToListAsync());
        }

        // GET: Municipio/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var municipio = await _context.Municipios
                .Include(m => m.Departamento)
                .Include(m => m.Estado)
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (municipio == null)
            {
                return NotFound();
            }

            return View(municipio);
        }

        // GET: Municipio/Create
        public IActionResult Create()
        {
            ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "Id", "Descripcion");
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion");
            return View();
        }

        // POST: Municipio/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion,DepartamentoId,EstadoId")] Municipio municipio)
        {
            if (ModelState.IsValid)
            {
                municipio.FechaCreacion = DateTime.Now;
                municipio.UsuarioId = Convert.ToInt32(HttpContext.Session.GetString("UsuarioId"));
                municipio.Descripcion = municipio.Descripcion.Trim().ToUpper();

                _context.Add(municipio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "Id", "Descripcion", municipio.DepartamentoId);
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", municipio.EstadoId);
            return View(municipio);
        }

        // GET: Municipio/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var municipio = await _context.Municipios.FindAsync(id);
            if (municipio == null)
            {
                return NotFound();
            }
            ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "Id", "Descripcion", municipio.DepartamentoId);
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", municipio.EstadoId);
            return View(municipio);
        }

        // POST: Municipio/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Descripcion,DepartamentoId,EstadoId")] Municipio municipio)
        {
            if (id != municipio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var infoMunicipio = await _context.Municipios.FindAsync(id);
                    infoMunicipio.Descripcion = municipio.Descripcion.Trim().ToUpper();
                    infoMunicipio.DepartamentoId = municipio.DepartamentoId;
                    infoMunicipio.EstadoId = municipio.EstadoId;
                    _context.Update(infoMunicipio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MunicipioExists(municipio.Id))
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
            ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "Id", "Descripcion", municipio.DepartamentoId);
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", municipio.EstadoId);
            return View(municipio);
        }

        // GET: Municipio/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var municipio = await _context.Municipios
                .Include(m => m.Departamento)
                .Include(m => m.Estado)
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (municipio == null)
            {
                return NotFound();
            }

            return View(municipio);
        }

        // POST: Municipio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var municipio = await _context.Municipios.FindAsync(id);
            municipio.EstadoId = 3;
            _context.Update(municipio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MunicipioExists(string id)
        {
            return _context.Municipios.Any(e => e.Id == id);
        }
    }
}
