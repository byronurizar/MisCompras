using Microsoft.EntityFrameworkCore;
using MisCompras.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisCrompras.Models
{
    public class MisComprasContext:DbContext
    {
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Perfil> Perfil { get; set; }
        //public DbSet<Menu> Menus { get; set; }
        //public DbSet<PerfilMenu> PerfilMenus { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<CreditoAbono> CreditoAbonos { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<EstadoPedido> EstadoPedidos { get; set; }
        public DbSet<Proveedor> Proveedor { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoDetalle> PedidoDetalles { get; set; }
        public DbSet<EstadoVenta> EstadoVentas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<SubCategoria> SubCategoria { get; set; }
        public DbSet<Gasto> Gastos { get; set; }
        public DbSet<Presupuesto> Presupuesto { get; set; }
        public DbSet<PresupuestoDetalle> PresupuestoDetalle { get; set; }
        //public DbSet<VentaDetalle> VentaDetalles { get; set; }
        //public DbSet<GastoPedido> GastoPedidos { get; set; }
        public MisComprasContext(DbContextOptions<MisComprasContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);
            List<Estado> listaEstado = getListaEstado();

            Perfil perfil = new Perfil();
            perfil.Id = 1;
            perfil.Nombre = "Administrador";
            perfil.Descripcion = "Administrador del sistema";
            perfil.EstadoId = 1;

            model.Entity<Estado>().HasData(listaEstado.ToArray());
            model.Entity<Perfil>().HasData(perfil);
        }

        private static List<Estado> getListaEstado()
        {
            List<Estado> listaEstado = new List<Estado>();
            Estado estado1 = new Estado();
            estado1.Id = 1;
            estado1.Descripcion = "Activo";
            estado1.Activo = true;
            listaEstado.Add(estado1);

            Estado estado2 = new Estado();
            estado2.Id = 2;
            estado2.Descripcion = "Inactivo";
            estado2.Activo = true;
            listaEstado.Add(estado2);

            Estado estado3 = new Estado();
            estado3.Id = 3;
            estado3.Descripcion = "Eliminado";
            estado3.Activo = true;
            listaEstado.Add(estado3);
            return listaEstado;
        }

    }
}
