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
    public class UsuarioController : Controller
    {
        private readonly MisComprasContext _context;


        public UsuarioController(MisComprasContext context)
        {
            _context = context;
        }
        // GET: Usuario
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("User,Password")] Usuario usuario)
        {

            var infoUsuario = await _context.Usuarios
                              .FirstOrDefaultAsync(m => m.User == usuario.User.Trim().ToUpper() && m.Password == usuario.Password.Trim() && m.EstadoId == 1);

            if (infoUsuario == null)
            {
                TempData["Codigo"] = -1;
                TempData["Mensaje"] = "Usuario y Contraseña Incorrectos, por favor verifique";
            }
            else
            {
                int UsuarioId = infoUsuario.Id;
                int PerfilId = infoUsuario.PerfilId;
                HttpContext.Session.SetString("UsuarioId", UsuarioId.ToString());
                HttpContext.Session.SetString("PerfilId", PerfilId.ToString());
                return Redirect("~/Cliente/Index");
            }

            return View(usuario);
        }

        // GET: Usuario
        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetString("UsuarioId", "1");
            var misComprasContext = _context.Usuarios.Include(u => u.Estado).Include(u => u.Perfil).Include(u => u.Usuarios).Where(a => a.EstadoId != 3);
            return View(await misComprasContext.ToListAsync());
        }

        // GET: Usuario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Estado)
                .Include(u => u.Perfil)
                .Include(u => u.Usuarios)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuario/Create
        public IActionResult Create()
        {
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion");
            ViewData["PerfilId"] = new SelectList(_context.Perfil, "Id", "Nombre");
            return View();
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,User,Password,PerfilId,EstadoId")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.FechaCreacion = DateTime.Now;
                usuario.UsuarioId = Convert.ToInt32(HttpContext.Session.GetString("UsuarioId"));
                _context.Add(usuario);
                await _context.SaveChangesAsync(true);
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", usuario.EstadoId);
            ViewData["PerfilId"] = new SelectList(_context.Perfil, "Id", "Descripcion", usuario.PerfilId);
            ViewBag.Codigo = "Soy un código de prueba";
            return View(usuario);
        }

        // GET: Usuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", usuario.EstadoId);
            ViewData["PerfilId"] = new SelectList(_context.Perfil, "Id", "Nombre", usuario.PerfilId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Password", usuario.UsuarioId);
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,User,Password,PerfilId,EstadoId")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var infoUsuario = await _context.Usuarios.FindAsync(id);
                    infoUsuario.User = usuario.User;
                    infoUsuario.Password = usuario.Password;
                    infoUsuario.PerfilId = usuario.PerfilId;
                    infoUsuario.EstadoId = usuario.EstadoId;
                    _context.Update(infoUsuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", usuario.EstadoId);
            ViewData["PerfilId"] = new SelectList(_context.Perfil, "Id", "Nombre", usuario.PerfilId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Password", usuario.UsuarioId);
            return View(usuario);
        }

        // GET: Usuario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Estado)
                .Include(u => u.Perfil)
                .Include(u => u.Usuarios)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            usuario.EstadoId = 3;
            _context.Update(usuario);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
