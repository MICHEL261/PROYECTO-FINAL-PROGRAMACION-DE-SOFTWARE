
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using lib_aplicaciones.Interfaces;

namespace lib_aplicaciones.Implementaciones
{
    public class FormatosAplicacion : iFormatosAplicacion
    {
        private IConexion? IConexion = null;

        public FormatosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Formatos? Borrar(Formatos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            // Calculos

            this.IConexion!.Formatos!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Formatos? Guardar(Formatos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");

            // Calculos

            this.IConexion!.Formatos!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Formatos> Listar()
        {
            return this.IConexion!.Formatos!.Take(20).ToList();
        }

        public List<Formatos> PorTipo(Formatos? entidad)
        {
            return this.IConexion!.Formatos!
                .Where(x => x.TipoFormato!.Contains(entidad!.TipoFormato!))
                .ToList();
        }
        public Formatos? Modificar(Formatos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            // Calculos

            var entry = this.IConexion!.Entry<Formatos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}