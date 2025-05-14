using lib_dominio.Entidades;

namespace lib_presentaciones.Interfaces
{
    public interface IDiscosPresentacion
    {
        Task<List<Discos>> Listar();
        Task<List<Discos>> PorNombre(Discos? entidad);
        Task<Discos?> Guardar(Discos? entidad);
        Task<Discos?> Modificar(Discos? entidad);
        Task<Discos?> Borrar(Discos? entidad);
    }
}