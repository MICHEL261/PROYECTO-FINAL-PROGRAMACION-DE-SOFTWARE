using lib_dominio.Entidades;

namespace lib_aplicaciones.Interfaces
{
    public interface IOrdenesAplicacion
    {
        void Configurar(string StringConexion);
        List<Ordenes> PorId(Ordenes? entidad);
        List<Ordenes> Listar();
        Ordenes? Guardar(Ordenes? entidad);
        Ordenes? Modificar(Ordenes? entidad);
        Ordenes? Borrar(Ordenes? entidad);
    }
}
