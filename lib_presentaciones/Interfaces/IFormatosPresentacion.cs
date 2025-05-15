using lib_dominio.Entidades;

namespace lib_presentaciones.Interfaces
{
    public interface IFormatosPresentacion
    {
        Task<List<Formatos>> Listar();
        Task<List<Formatos>> PorNombre(Formatos? entidad);
        Task<Formatos?> Guardar(Formatos? entidad);
        Task<Formatos?> Modificar(Formatos? entidad);
        Task<Formatos?> Borrar(Formatos? entidad);
    }
}