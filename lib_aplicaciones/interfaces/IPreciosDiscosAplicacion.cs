using lib_dominio.Entidades;

namespace lib_aplicaciones.Interfaces
{
    public interface iPreciosDiscosAplicacion
    {
        void Configurar(string StringConexion);
        List<PreciosDiscos> Listar();
        PreciosDiscos? Guardar(PreciosDiscos? entidad);
        PreciosDiscos? Modificar(PreciosDiscos? entidad);
        PreciosDiscos? Borrar(PreciosDiscos? entidad);
    }
}