using lib_dominio.Entidades;

namespace lib_presentaciones.Interfaces
{
    public interface IMarcasPresentacion
    {
        Task<List<Marcas>> Listar();
        Task<List<Marcas>> PorNombre(Marcas? entidad);
        Task<Marcas?> Guardar(Marcas? entidad);
        Task<Marcas?> Modificar(Marcas? entidad);
        Task<Marcas?> Borrar(Marcas? entidad);
    }
}