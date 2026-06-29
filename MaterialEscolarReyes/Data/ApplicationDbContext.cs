using DocumentFormat.OpenXml.Drawing.Charts;
using MaterialEscolarReyes.Models;
using Microsoft.EntityFrameworkCore;

namespace MaterialEscolarReyes.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }
        public DbSet<MovimientoInventario> MovimientoInventarios { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }

        public DbSet<DetallePedido> DetallePedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>().ToTable("usuarios");
            modelBuilder.Entity<Cliente>().ToTable("clientes");
            modelBuilder.Entity<Categoria>().ToTable("categorias");
            modelBuilder.Entity<Marca>().ToTable("marcas");
            modelBuilder.Entity<Proveedor>().ToTable("proveedores");
            modelBuilder.Entity<Producto>().ToTable("productos");
            modelBuilder.Entity<Venta>().ToTable("ventas");
            modelBuilder.Entity<DetalleVenta>().ToTable("detalle_ventas");
            modelBuilder.Entity<MovimientoInventario>().ToTable("movimientos");
            modelBuilder.Entity<Usuario>()

                .HasIndex(u => u.Correo)
                .IsUnique();

            modelBuilder.Entity<Producto>()
                .HasIndex(p => p.Codigo)
                .IsUnique();


            modelBuilder.Entity<Carrito>()
               .ToTable("carrito");

            modelBuilder.Entity<Pedido>().ToTable("pedidos");

            modelBuilder.Entity<DetallePedido>().ToTable("detalle_pedidos");

        }
    }
}