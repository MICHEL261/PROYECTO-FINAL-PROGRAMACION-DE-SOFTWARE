
using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_aplicaciones.Implementaciones
{
    public class ClientesAplicacion : iClientesAplicacion
    {
        private IConexion? IConexion = null;

        public ClientesAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Clientes? Borrar(Clientes? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");


            var datos = "Nombre Cliente: " + entidad.NombreCliente + ", " + "apellido: " + entidad.ApellidoCliente + ", " + "telefono: " + entidad.TelefonoCliente + ", " + "direccion: " + entidad.DireccionCliente;
            GuardarAuditoria("borrar", datos);


            // Calculos

            this.IConexion!.Clientes!.Remove(entidad);

                this.IConexion.SaveChanges();

            


            return entidad;
        }

        public Clientes? Guardar(Clientes? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");

            // Calculos


            var datos = "Nombre Cliente: " + entidad.NombreCliente + ", " + "apellido: " + entidad.ApellidoCliente + ", " + "telefono: " + entidad.TelefonoCliente + ", " + "direccion: " + entidad.DireccionCliente;
            GuardarAuditoria("guardar", datos);

            

            this.IConexion!.Clientes!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Clientes> Listar()
        {
            return this.IConexion!.Clientes!.Take(20).ToList();
        }

        public List<Clientes> PorNombre(Clientes? entidad)
        {
            return this.IConexion!.Clientes!
                .Where(x => x.NombreCliente!.Contains(entidad!.NombreCliente!))
                .ToList();
        }
        public Clientes? Modificar(Clientes? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");


            var datos ="Nombre Cliente: "+ entidad.NombreCliente + ", " + "apellido: " + entidad.ApellidoCliente + ", "+"telefono: " + entidad.TelefonoCliente + ", "+"direccion: " + entidad.DireccionCliente;
            GuardarAuditoria("modificar", datos);

            // Calculos

            var entry = this.IConexion!.Entry<Clientes>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
        public void GuardarAuditoria(String operacion, String datos)
        {
            var Auditorias = new Auditorias();
            {
                Auditorias.Entidad = "Clientes";
                Auditorias.Operacion = operacion;
                Auditorias.Fecha = DateTime.Now;
                Auditorias.Datos = datos;
            }

            IConexion!.Auditorias!.Add(Auditorias);
            IConexion.SaveChanges();
        }
    }
}
