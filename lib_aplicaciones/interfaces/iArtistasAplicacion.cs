using lib_dominio.Entidades;

namespace lib_aplicaciones.Interfaces
{
    public interface iArtistasAplicacion
    {
        void Configurar(string StringConexion);
        List<Artistas> PorNombre(Artistas? entidad);
        List<Artistas> Listar();
        Artistas? Guardar(Artistas? entidad);
        Artistas? Modificar(Artistas? entidad);
        Artistas? Borrar(Artistas? entidad);
    }
}