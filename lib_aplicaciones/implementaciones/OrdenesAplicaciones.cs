using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_dominio.Nucleo;
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

            if (entidad._Pago == null && entidad.Pago != 0)
            {
                entidad._Pago = IConexion!.Pagos!.FirstOrDefault(m => m.Id == entidad.Pago);
                if (entidad._Pago != null)
                    IConexion.Entry(entidad._Pago).State = EntityState.Unchanged;
            }

            if (entidad._Cliente == null && entidad.Cliente != 0)
            {
                entidad._Cliente = IConexion!.Clientes!.FirstOrDefault(a => a.Id == entidad.Cliente);
                if (entidad._Cliente != null)
                    IConexion.Entry(entidad._Cliente).State = EntityState.Unchanged;
            }

            var datos = "Pago: " + entidad._Pago!.TipoPago + ", " + "Fecha: " + entidad.Fecha + ", " + "Monto Total: " + entidad.MontoTotal + ", " + "cliente: " + entidad._Cliente!.NombreCliente;

            this.IConexion!.Ordenes!.Remove(entidad);
            this.IConexion.SaveChanges();
            GuardarAuditoria("borrar", datos);
            return entidad;
        }

        public Ordenes? Guardar(Ordenes? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");
            if (entidad._Pago == null && entidad.Pago != 0)
            {
                entidad._Pago = IConexion!.Pagos!.FirstOrDefault(m => m.Id == entidad.Pago);
                if (entidad._Pago != null)
                    IConexion.Entry(entidad._Pago).State = EntityState.Unchanged;
            }

            if (entidad._Cliente == null && entidad.Cliente != 0)
            {
                entidad._Cliente = IConexion!.Clientes!.FirstOrDefault(a => a.Id == entidad.Cliente);
                if (entidad._Cliente != null)
                    IConexion.Entry(entidad._Cliente).State = EntityState.Unchanged;
            }

            var datos = "Pago: " + entidad._Pago!.TipoPago + ", " + "Fecha: " + entidad.Fecha + ", " + "Monto Total: " + entidad.MontoTotal + ", " + "cliente: " + entidad._Cliente!.NombreCliente;



            this.IConexion!.Ordenes!.Add(entidad);
            this.IConexion.SaveChanges();
            GuardarAuditoria("guardar", datos);

            return entidad;
        }

        public List<Ordenes> Listar()
        {
            return this.IConexion!.Ordenes!
                .Include(x => x._Cliente)
                .Include(x => x._Pago)
                .Take(20)
                .ToList();
        }

        public List<Ordenes> PorId(Ordenes? entidad)
        {
            return this.IConexion!.Ordenes!
                .Where(x => x.Id! == (entidad!.Id!))
                .Include(x => x._Cliente)
                .Include(x => x._Pago)
                .ToList();
        }

        public Ordenes? Modificar(Ordenes? entidad)

        {


            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");
            if (entidad._Pago == null && entidad.Pago != 0)
            {
                entidad._Pago = IConexion!.Pagos!.FirstOrDefault(m => m.Id == entidad.Pago);
                if (entidad._Pago != null)
                    IConexion.Entry(entidad._Pago).State = EntityState.Unchanged;
            }

            if (entidad._Cliente == null && entidad.Cliente != 0)
            {
                entidad._Cliente = IConexion!.Clientes!.FirstOrDefault(a => a.Id == entidad.Cliente);
                if (entidad._Cliente != null)
                    IConexion.Entry(entidad._Cliente).State = EntityState.Unchanged;
            }

            var datos = "Pago: " + entidad._Pago!.TipoPago + ", " + "Fecha: " + entidad.Fecha + ", " + "Monto Total: " + entidad.MontoTotal + ", " + "cliente: " + entidad._Cliente!.NombreCliente;


            var entry = this.IConexion!.Entry<Ordenes>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            GuardarAuditoria("modificar", datos);

            return entidad;
        }

        public void GuardarAuditoria(String operacion, String datos)
        {
            var Auditorias = new Auditorias();
            {
                Auditorias.Entidad = "Ordenes";
                Auditorias.Operacion = operacion;
                Auditorias.Fecha = DateTime.Now;
                Auditorias.Datos = datos;
            }

            IConexion!.Auditorias!.Add(Auditorias);
            IConexion.SaveChanges();
        }
    }
}



