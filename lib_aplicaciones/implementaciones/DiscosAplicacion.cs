
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
            var datos = entidad.NombreDisco + ", " + entidad.Marca.ToString() + ", " + entidad.NombreDisco + ", " + entidad.Artista; 
            GuardarAuditoria("Borrar", datos);


            // Calculos

            this.IConexion!.Discos!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Discos? Guardar(Discos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");

            var datos = entidad.NombreDisco + ", " + entidad.Marca.ToString() + ", " + entidad.NombreDisco + ", " + entidad.Artista;
            GuardarAuditoria("Guardar", datos);

            // Calculos

            this.IConexion!.Discos!.Add(entidad);
            this.IConexion.SaveChanges();
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



            var datos = entidad.NombreDisco + ", " + entidad.Marca.ToString() + ", " + entidad.FechaLanzamiento + ", " + entidad.Artista;
            GuardarAuditoria("Modificar", datos);


            // Calculos

            var entry = this.IConexion!.Entry<Discos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
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