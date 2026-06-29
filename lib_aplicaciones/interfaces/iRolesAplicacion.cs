using lib_dominio.Entidades;

namespace lib_aplicaciones.Interfaces
{
    public interface iRolesAplicacion
    {
        void Configurar(string StringConexion);
        List<Roles> PorNombre(Roles? entidad);
        List<Roles> Listar();
        Roles? Guardar(Roles? entidad);
        Roles? Modificar(Roles? entidad);
        Roles? Borrar(Roles? entidad);
    }
}