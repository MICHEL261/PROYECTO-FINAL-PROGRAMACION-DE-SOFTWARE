
using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_aplicaciones.Implementaciones
{
    public class ArtistasAplicacion : iArtistasAplicacion
    {
        private IConexion? IConexion = null;

        public ArtistasAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Artistas? Borrar(Artistas? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");
            var datos = "Nombre Artista: "+entidad.NombreArtista +", "+"Genero Musical: " +entidad.GeneroMusical; 
            GuardarAuditoria("Borrar", datos);


            // Calculos

            this.IConexion!.Artistas!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Artistas? Guardar(Artistas? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");
            var datos = "Nombre Artista: " + entidad.NombreArtista + ", " + "Genero Musical: " + entidad.GeneroMusical;


            // Calculos

            this.IConexion!.Artistas!.Add(entidad);
            this.IConexion.SaveChanges();
            GuardarAuditoria("guardar", datos);

            return entidad;
        }

        public List<Artistas> Listar()
        {
            return this.IConexion!.Artistas!.Take(20).ToList();
        }

        public List<Artistas> PorNombre(Artistas? entidad)
        {
            return this.IConexion!.Artistas!
                .Where(x => x.NombreArtista!.Contains(entidad!.NombreArtista!))
                .ToList();
        }
        public Artistas? Modificar(Artistas? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");
            var datos = "Nombre Artista: " + entidad.NombreArtista + ", " + "Genero Musical: " + entidad.GeneroMusical;
            


            // Calculos

            var entry = this.IConexion!.Entry<Artistas>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            GuardarAuditoria("Modificar", datos);
            return entidad;
        }

        public void GuardarAuditoria(String operacion, String datos)
        {
            var Auditorias = new Auditorias();
            {
                Auditorias.Entidad = "Artistas";
                Auditorias.Operacion = operacion;
                Auditorias.Fecha = DateTime.Now;
                Auditorias.Datos = datos;
            }

            IConexion!.Auditorias!.Add(Auditorias);
            IConexion.SaveChanges();
        }
    }
}
