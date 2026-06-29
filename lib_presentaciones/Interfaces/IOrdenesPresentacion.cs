using lib_dominio.Entidades;

namespace lib_presentaciones.Interfaces
{
    public interface IOrdenesPresentacion
    {
        Task<List<Ordenes>> Listar();
        Task<List<Ordenes>> PorId(Ordenes? entidad);
        Task<Ordenes?> Guardar(Ordenes? entidad);
        Task<Ordenes?> Modificar(Ordenes? entidad);
        Task<Ordenes?> Borrar(Ordenes? entidad);
    }
}