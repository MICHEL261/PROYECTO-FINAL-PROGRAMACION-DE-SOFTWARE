using lib_dominio.Entidades;

namespace lib_presentaciones.Interfaces
{
    public interface IOrdenesDiscosPresentacion
    {
        Task<List<OrdenesDiscos>> Listar();
        Task<List<OrdenesDiscos>> PorId(OrdenesDiscos? entidad);
        Task<OrdenesDiscos?> Guardar(OrdenesDiscos? entidad);
        Task<OrdenesDiscos?> Modificar(OrdenesDiscos? entidad);
        Task<OrdenesDiscos?> Borrar(OrdenesDiscos? entidad);
    }
}