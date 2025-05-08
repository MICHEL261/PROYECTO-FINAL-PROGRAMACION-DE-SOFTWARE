using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public partial class Conexion : DbContext, IConexion
    {
        public string? StringConexion { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.StringConexion!, p => { });
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        public DbSet<Clientes>? Clientes { get; set; }
        public DbSet<Artistas>? Artistas { get; set; }
        public DbSet<Marcas>? Marcas { get; set; }
        public DbSet<Discos>? Discos { get; set; }
        public DbSet<Formatos>? Formatos { get; set; }
        public DbSet<Pagos>? Pagos { get; set; }
        public DbSet<Ordenes>? Ordenes { get; set; }
        public DbSet<OrdenesDiscos>? OrdenesDiscos { get; set; }
        public DbSet<Auditorias>? Auditorias { get; set; }
        public DbSet<Roles>? Roles { get; set; }
        public DbSet<Usuarios>? Usuarios { get; set; }



    }
}

