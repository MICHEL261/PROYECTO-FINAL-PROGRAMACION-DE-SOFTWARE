using lib_dominio.Entidades;

namespace lib_aplicaciones.Interfaces
{
    public interface IMarcasAplicacion
    {
        void Configurar(string StringConexion);
        List<Marcas> PorNombre(Marcas? entidad);
        List<Marcas> Listar();
        Marcas? Guardar(Marcas? entidad);
        Marcas? Modificar(Marcas? entidad);
        Marcas? Borrar(Marcas? entidad);
    }
}
