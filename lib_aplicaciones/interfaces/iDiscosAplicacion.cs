using lib_dominio.Entidades;

namespace lib_aplicaciones.Interfaces
{
    public interface iDiscosAplicacion
    {
        void Configurar(string StringConexion);
        List<Discos> PorNombre(Discos? entidad);
        List<Discos> Listar();
        Discos? Guardar(Discos? entidad);
        Discos? Modificar(Discos? entidad);
        Discos? Borrar(Discos? entidad);
    }
}