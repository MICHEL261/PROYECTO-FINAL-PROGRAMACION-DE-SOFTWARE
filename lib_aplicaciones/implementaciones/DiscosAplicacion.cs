
using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace lib_aplicaciones.Implementaciones
{
    public class DiscosAplicacion : iDiscosAplicacion
    {
        private IConexion? IConexion = null;

        public DiscosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Discos? Borrar(Discos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            if (entidad._Marca == null && entidad.Marca != 0)
            {
                entidad._Marca = IConexion!.Marcas!.FirstOrDefault(m => m.Id == entidad.Marca);
                if (entidad._Marca != null)
                    IConexion.Entry(entidad._Marca).State = EntityState.Unchanged;
            }

            if (entidad._Artista == null && entidad.Artista != 0)
            {
                entidad._Artista = IConexion!.Artistas!.FirstOrDefault(a => a.Id == entidad.Artista);
                if (entidad._Artista != null)
                    IConexion.Entry(entidad._Artista).State = EntityState.Unchanged;
            }

            var datos = "Nombre: " + entidad.NombreDisco + ", " + "marca: " + entidad._Marca?.NombreMarca + ", " + "Duracion: " + entidad.DuracionDisco + ", " + "Artista: " + entidad._Artista?.NombreArtista;


            // Calculos

            this.IConexion!.Discos!.Remove(entidad);
            this.IConexion.SaveChanges();
            GuardarAuditoria("Borrar", datos);

            return entidad;
        }

        public Discos? Guardar(Discos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");

            if (entidad._Marca == null && entidad.Marca != 0)
            {
                entidad._Marca = IConexion!.Marcas!.FirstOrDefault(m => m.Id == entidad.Marca);
                if (entidad._Marca != null)
                    IConexion.Entry(entidad._Marca).State = EntityState.Unchanged;
            }

            if (entidad._Artista == null && entidad.Artista != 0)
            {
                entidad._Artista = IConexion!.Artistas!.FirstOrDefault(a => a.Id == entidad.Artista);
                if (entidad._Artista != null)
                    IConexion.Entry(entidad._Artista).State = EntityState.Unchanged;
            }

            var datos = "Nombre: " + entidad.NombreDisco + ", " + "marca: " + entidad._Marca?.NombreMarca + ", " + "Duracion: " + entidad.DuracionDisco + ", " + "Artista: " + entidad._Artista?.NombreArtista;

            // Calculos

            this.IConexion!.Discos!.Add(entidad);
            this.IConexion.SaveChanges();
            GuardarAuditoria("Guardar", datos);

            return entidad;
        }

        public List<Discos> Listar()
        {
            return this.IConexion!.Discos!
                .Take(20)
                .Include(x => x._Artista)
                .Include(x => x._Marca)
                .ToList();
        }

        public List<Discos> PorNombre(Discos? entidad)
        {
            return this.IConexion!.Discos!
                .Where(x => x.NombreDisco!.Contains(entidad!.NombreDisco!))
                .Include(x => x._Artista)
                .Include(x => x._Marca)
                .ToList();
        }
        public Discos? Modificar(Discos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");


            if (entidad._Marca == null && entidad.Marca != 0)
            {
                entidad._Marca = IConexion!.Marcas!.FirstOrDefault(m => m.Id == entidad.Marca);
                if (entidad._Marca != null)
                    IConexion.Entry(entidad._Marca).State = EntityState.Unchanged;
            }

            if (entidad._Artista == null && entidad.Artista != 0)
            {
                entidad._Artista = IConexion!.Artistas!.FirstOrDefault(a => a.Id == entidad.Artista);
                if (entidad._Artista != null)
                    IConexion.Entry(entidad._Artista).State = EntityState.Unchanged;
            }

            var datos = "Nombre: " + entidad.NombreDisco + ", " + "marca: " + entidad._Marca?.NombreMarca + ", " + "Duracion: " + entidad.DuracionDisco + ", " + "Artista: " + entidad._Artista?.NombreArtista;

            // Calculos

            this.IConexion!.Discos!.Add(entidad);
            this.IConexion.SaveChanges();
            GuardarAuditoria("Modificar", datos);

            return entidad;
        }
        public void GuardarAuditoria(String operacion, String datos)
        {
            var Auditorias = new Auditorias();
            {
                Auditorias.Entidad = "Discos";
                Auditorias.Operacion = operacion;
                Auditorias.Fecha = DateTime.Now;
                Auditorias.Datos = datos;
            }

            IConexion!.Auditorias!.Add(Auditorias);
            IConexion.SaveChanges();
        }

        public void ObtenerDatos(String operacion, String datos)
        {
            var Auditorias = new Auditorias();
            {
                Auditorias.Entidad = "Discos";
                Auditorias.Operacion = operacion;
                Auditorias.Fecha = DateTime.Now;
                Auditorias.Datos = datos;
            }

            IConexion!.Auditorias!.Add(Auditorias);
            IConexion.SaveChanges();
        }
    }
}