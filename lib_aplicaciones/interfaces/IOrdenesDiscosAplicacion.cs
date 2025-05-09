using lib_dominio.Entidades;

namespace lib_aplicaciones.Interfaces
{
    public interface IOrdenesDiscosAplicacion
    {
        void Configurar(string StringConexion);
        List<OrdenesDiscos> PorNombre(OrdenesDiscos? entidad);
        List<OrdenesDiscos> Listar();
        OrdenesDiscos? Guardar(OrdenesDiscos? entidad);
        OrdenesDiscos? Modificar(OrdenesDiscos? entidad);
        OrdenesDiscos? Borrar(OrdenesDiscos? entidad);
        void ActualizarMonto(OrdenesDiscos? entidad);
    }
}
