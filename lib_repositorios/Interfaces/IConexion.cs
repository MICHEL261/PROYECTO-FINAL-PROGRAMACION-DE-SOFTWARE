using lib_dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace lib_repositorios.Interfaces
{
    public interface IConexion
    {
        string? StringConexion { get; set; }

        DbSet<Clientes>? Clientes { get; set; }
        DbSet<Artistas>? Artistas { get; set; }
        DbSet<Marcas>? Marcas { get; set; }
        DbSet<Discos>? Discos { get; set; }
        DbSet<Formatos>? Formatos { get; set; }
        DbSet<Pagos>? Pagos { get; set; }
        DbSet<Ordenes>? Ordenes { get; set; }
        DbSet<OrdenesDiscos>? OrdenesDiscos { get; set; }


        EntityEntry<T> Entry<T>(T entity) where T : class;
        int SaveChanges();
    }
}
