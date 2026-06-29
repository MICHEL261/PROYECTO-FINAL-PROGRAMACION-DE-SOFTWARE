using lib_dominio.Entidades;

namespace lib_aplicaciones.Interfaces
{
    public interface iPagosAplicacion
    {
        void Configurar(string StringConexion);
        List<Pagos> PorTipo(Pagos? entidad);
        List<Pagos> Listar();
        Pagos? Guardar(Pagos? entidad);
        Pagos? Modificar(Pagos? entidad);
        Pagos? Borrar(Pagos? entidad);
    }
}