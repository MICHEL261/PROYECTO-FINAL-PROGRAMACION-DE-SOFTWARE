using lib_dominio.Entidades;

namespace lib_aplicaciones.Interfaces
{
    public interface iFormatosAplicacion
    {
        void Configurar(string StringConexion);
        List<Formatos> PorTipo(Formatos? entidad);
        List<Formatos> Listar();
        Formatos? Guardar(Formatos? entidad);
        Formatos? Modificar(Formatos? entidad);
        Formatos? Borrar(Formatos? entidad);
    }
}