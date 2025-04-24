using lib_dominio.Entidades;

namespace lib_aplicaciones.Interfaces
{
    public interface iClientesAplicacion
    {
        void Configurar(string StringConexion);
        List<Clientes> PorNombre(Clientes? entidad);
        List<Clientes> Listar();
        Clientes? Guardar(Clientes? entidad);
        Clientes? Modificar(Clientes? entidad);
        Clientes? Borrar(Clientes? entidad);
    }
}