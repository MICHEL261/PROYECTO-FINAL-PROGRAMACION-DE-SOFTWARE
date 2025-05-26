
using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_aplicaciones.Implementaciones
{
    public class RolesAplicacion : iRolesAplicacion
    {
        private IConexion? IConexion = null;

        public RolesAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Roles? Borrar(Roles? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");
            var datos = "Nombre Rol: " + entidad.NombreRol + ", " + "descripcion: " + entidad.Descripcion;

            // Calculos

            this.IConexion!.Roles!.Remove(entidad);
            this.IConexion.SaveChanges();
            GuardarAuditoria("borrar", datos);

            return entidad;
        }

        public Roles? Guardar(Roles? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");
            var datos = "Nombre Rol: " + entidad.NombreRol + ", " + "descripcion: " + entidad.Descripcion;

            // Calculos

            this.IConexion!.Roles!.Add(entidad);
            this.IConexion.SaveChanges();
            GuardarAuditoria("guardar", datos);

            return entidad;
        }

        public List<Roles> Listar()
        {
            return this.IConexion!.Roles!.Take(20).ToList();
        }

        public List<Roles> PorNombre(Roles? entidad)
        {
            return this.IConexion!.Roles!
                .Where(x => x.NombreRol!.Contains(entidad!.NombreRol!))
                .ToList();
        }
        public Roles? Modificar(Roles? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");
            var datos ="Nombre Rol: "+ entidad.NombreRol + ", "+"descripcion: " + entidad.Descripcion;

            // Calculos

            var entry = this.IConexion!.Entry<Roles>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            GuardarAuditoria("modificar", datos);

            return entidad;
        }

        public void GuardarAuditoria(String operacion, String datos)
        {
            var Auditorias = new Auditorias();
            {
                Auditorias.Entidad = "Roles";
                Auditorias.Operacion = operacion;
                Auditorias.Fecha = DateTime.Now;
                Auditorias.Datos = datos;
            }

            IConexion!.Auditorias!.Add(Auditorias);
            IConexion.SaveChanges();
        }
    }
}
