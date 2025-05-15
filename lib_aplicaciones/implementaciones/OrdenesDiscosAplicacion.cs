using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_dominio.Nucleo;
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
            var datos = JsonConversor.ConvertirAString(entidad);
            String operacion = "Borrar";

            GuardarAuditoria(operacion, datos);

            this.IConexion!.OrdenesDiscos!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public OrdenesDiscos? Guardar(OrdenesDiscos? entidad)
        {
            var contextoReal = (DbContext)IConexion!;
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");

            var datos = JsonConversor.ConvertirAString(entidad);
            String operacion = "Guardar";

            GuardarAuditoria(operacion, datos);

            this.IConexion!.OrdenesDiscos!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<OrdenesDiscos> Listar()
        {
            return this.IConexion!.OrdenesDiscos!.Take(20).ToList();
        }

        public List<OrdenesDiscos> PorId(OrdenesDiscos? entidad)
        {
            return this.IConexion!.OrdenesDiscos!
                .Where(x => x.Id! == (entidad!.Id!))
                .ToList();
        }

        public OrdenesDiscos? Modificar(OrdenesDiscos? entidad)


        {

            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            var datos = JsonConversor.ConvertirAString(entidad);
            String operacion = "Modificar";

            GuardarAuditoria(operacion, datos);

            var entry = this.IConexion!.Entry<OrdenesDiscos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
        public decimal CalcularMontoTotal(Ordenes? orden)
        {
            var respuesta = 0.0m;

            var entidades = this.IConexion!.OrdenesDiscos!.Where(p => p.Orden == orden!.Id).ToList();
            if (entidades == null)
            {
                throw new Exception("La orden no existe.");
            }

            foreach (var elemento in entidades)
                respuesta += elemento.Cantidad * elemento.ValorUnitario;

            return respuesta;
        }
        public void ActualizarMonto(OrdenesDiscos? entidad)
        {

            var orden = this.IConexion!.Ordenes!.FirstOrDefault(p => p.Id == entidad!.Orden);

            orden!.MontoTotal = CalcularMontoTotal(orden);

            var entry = this.IConexion.Entry<Ordenes>(orden!);
            entry.State = EntityState.Modified;
            this.IConexion!.SaveChanges();

        }

        public void GuardarAuditoria(String operacion, String datos)
        {
            var Auditorias = new Auditorias();
            {
                Auditorias.Entidad = "OrdenesDiscos";
                Auditorias.Operacion = operacion;
                Auditorias.Fecha = DateTime.Now;
                Auditorias.Datos = datos;
            }

            IConexion!.Auditorias!.Add(Auditorias);
            IConexion.SaveChanges();
        }

    }

}




