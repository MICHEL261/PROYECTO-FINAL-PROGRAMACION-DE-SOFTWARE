
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using lib_aplicaciones.Interfaces;

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

            // Calculos

            this.IConexion!.Discos!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Discos> Listar()
        {
            return this.IConexion!.Discos!.Take(20).ToList();
        }

        public List<Discos> PorNombre(Discos? entidad)
        {
            return this.IConexion!.Discos!
                .Where(x => x.NombreDisco!.Contains(entidad!.NombreDisco!))
                .ToList();
        }
        public Discos? Modificar(Discos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            // Calculos

            var entry = this.IConexion!.Entry<Discos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}