using lib_dominio.Entidades;

namespace lib_presentaciones.Interfaces
{
    public interface IPreciosDiscosPresentacion
    {
        Task<List<PreciosDiscos>> Listar();
        Task<PreciosDiscos?> Guardar(PreciosDiscos? entidad);
        Task<PreciosDiscos?> Modificar(PreciosDiscos? entidad);
        Task<PreciosDiscos?> Borrar(PreciosDiscos? entidad);
    }
}