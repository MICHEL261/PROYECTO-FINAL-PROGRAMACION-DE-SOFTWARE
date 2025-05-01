using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_aplicaciones.Implementaciones
{
    public class OrdenesAplicacion : IOrdenesAplicacion
    {
        private IConexion? IConexion = null;

        public OrdenesAplicacion(IConexion iConexion)


        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Ordenes? Borrar(Ordenes? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.Ordenes!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Ordenes? Guardar(Ordenes? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");

            this.IConexion!.Ordenes!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Ordenes> Listar()
        {
            return this.IConexion!.Ordenes!.Take(20).ToList();
        }

        public List<Ordenes> PorNombre(Ordenes? entidad)
        {
            return this.IConexion!.Ordenes!
                //.Where(x => x.Id!.Contains(entidad!.Id!))
                .ToList();
        }

        public Ordenes? Modificar(Ordenes? entidad)


        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            var entry = this.IConexion!.Entry<Ordenes>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}



