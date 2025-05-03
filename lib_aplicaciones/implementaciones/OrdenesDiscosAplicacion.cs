using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_aplicaciones.Implementaciones
{
    public class OrdenesDiscosAplicacion : IOrdenesDiscosAplicacion
    {
        private IConexion? IConexion = null;

        public OrdenesDiscosAplicacion(IConexion iConexion)


        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public OrdenesDiscos? Borrar(OrdenesDiscos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.OrdenesDiscos!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public OrdenesDiscos? Guardar(OrdenesDiscos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo"); 

            this.IConexion!.OrdenesDiscos!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<OrdenesDiscos> Listar()
        {
            return this.IConexion!.OrdenesDiscos!.Take(20).ToList();
        }

        public List<OrdenesDiscos> PorNombre(OrdenesDiscos? entidad)
        {
            return this.IConexion!.OrdenesDiscos!
                //.Where(x => x.Id!.Contains(entidad!.Id!))
                .ToList();
        }

        public OrdenesDiscos? Modificar(OrdenesDiscos? entidad)


        {
           
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            var entry = this.IConexion!.Entry<OrdenesDiscos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
        public decimal CalcularMontoTotal(OrdenesDiscos? entidad)
        {
            var respuesta = 0.0m;

            var entidades = this.IConexion!.OrdenesDiscos!.Where(p => p.Orden == entidad!.Orden).ToList();
            if (entidades == null) {
                throw new Exception("La orden no existe.");
            }

            foreach (var elemento in entidades)
                respuesta += elemento.Cantidad * elemento.ValorUnitario;

            return respuesta;            
        }
    }
}



