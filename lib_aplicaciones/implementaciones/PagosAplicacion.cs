
using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_aplicaciones.Implementaciones
{
    public class PagosAplicacion : iPagosAplicacion
    {
        private IConexion? IConexion = null;

        public PagosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Pagos? Borrar(Pagos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            var datos = JsonConversor.ConvertirAString(entidad);
            String operacion = "Borrar";

            GuardarAuditoria(operacion, datos);

            // Calculos

            this.IConexion!.Pagos!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Pagos? Guardar(Pagos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");

            var datos = JsonConversor.ConvertirAString(entidad);
            String operacion = "Guardar";

            GuardarAuditoria(operacion, datos);

            // Calculos

            this.IConexion!.Pagos!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Pagos> Listar()
        {
            return this.IConexion!.Pagos!.Take(20).ToList();
        }

        public List<Pagos> PorTipo(Pagos? entidad)
        {
            return this.IConexion!.Pagos!
                .Where(x => x.TipoPago!.Contains(entidad!.TipoPago!))
                .ToList();
        }
        public Pagos? Modificar(Pagos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            var datos = JsonConversor.ConvertirAString(entidad);
            String operacion = "Modificar";

            GuardarAuditoria(operacion, datos);

            // Calculos

            var entry = this.IConexion!.Entry<Pagos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public void GuardarAuditoria(String operacion, String datos)
        {
            var Auditorias = new Auditorias();
            {
                Auditorias.Entidad = "Pagos";
                Auditorias.Operacion = operacion;
                Auditorias.Fecha = DateTime.Now;
                Auditorias.Datos = datos;
            }

            IConexion!.Auditorias!.Add(Auditorias);
            IConexion.SaveChanges();
        }
    }
}
