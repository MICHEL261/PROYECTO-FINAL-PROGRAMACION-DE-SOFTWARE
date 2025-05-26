
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using lib_aplicaciones.Interfaces;
using lib_dominio.Nucleo;

namespace lib_aplicaciones.Implementaciones
{
    public class UsuariosAplicacion : IUsuariosAplicacion
    {
        private IConexion? IConexion = null;

        public UsuariosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Usuarios? Borrar(Usuarios? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");
            if (entidad._Rol == null && entidad.Rol != 0)
            {
                entidad._Rol = IConexion!.Roles!.FirstOrDefault(a => a.Id == entidad.Rol);
                if (entidad._Rol != null)
                    IConexion.Entry(entidad._Rol).State = EntityState.Unchanged;
            }
            var datos = "Nombre: " + entidad.Nombre + ", " + "Apellido: " + entidad.Apellido + ", " + "Usuario: " + entidad.NombreUsuario + ", " + "contraseña: " + entidad.Contraseña + ", " + "Rol: " + entidad._Rol!.NombreRol;

            // Calculos

            this.IConexion!.Usuarios!.Remove(entidad);
            this.IConexion.SaveChanges();
            GuardarAuditoria("borrar", datos);

            return entidad;
        }

        public Usuarios? Guardar(Usuarios? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");


            if (entidad._Rol == null && entidad.Rol != 0)
            {
                entidad._Rol = IConexion!.Roles!.FirstOrDefault(a => a.Id == entidad.Rol);
                if (entidad._Rol != null)
                    IConexion.Entry(entidad._Rol).State = EntityState.Unchanged;
            }
            var datos = "Nombre: " + entidad.Nombre + ", " + "Apellido: " + entidad.Apellido + ", " + "Usuario: " + entidad.NombreUsuario + ", " + "contraseña: " + entidad.Contraseña + ", " + "Rol: " + entidad._Rol!.NombreRol;

            // Calculos

            this.IConexion!.Usuarios!.Add(entidad);
            this.IConexion.SaveChanges();
            GuardarAuditoria("Guardar", datos);

            return entidad;
        }

        public List<Usuarios> Listar()
        {
            return this.IConexion!.Usuarios!.Take(20)
                .Include(x=> x._Rol)
                .ToList();
        }

        public List<Usuarios> PorNombre(Usuarios? entidad)
        {
            return this.IConexion!.Usuarios!
                .Where(x => x.NombreUsuario!.Contains(entidad!.NombreUsuario!))
                
                .ToList();
        }
        public Usuarios? Modificar(Usuarios? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");


            if (entidad._Rol == null && entidad.Rol != 0)
            {
                entidad._Rol = IConexion!.Roles!.FirstOrDefault(a => a.Id == entidad.Rol);
                if (entidad._Rol != null)
                    IConexion.Entry(entidad._Rol).State = EntityState.Unchanged;
            }
            var datos = "Nombre: " + entidad.Nombre + ", " + "Apellido: " + entidad.Apellido + ", " + "Usuario: " + entidad.NombreUsuario + ", " + "contraseña: " + entidad.Contraseña + ", " + "Rol: " + entidad._Rol!.NombreRol;
            // Calculos

            var entry = this.IConexion!.Entry<Usuarios>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            GuardarAuditoria("Modificar", datos);

            return entidad;
        }

        public void GuardarAuditoria(String operacion, String datos)
        {
            var Auditorias = new Auditorias();
            {
                Auditorias.Entidad = "Usuarios";
                Auditorias.Operacion = operacion;
                Auditorias.Fecha = DateTime.Now;
                Auditorias.Datos = datos;
            }

            IConexion!.Auditorias!.Add(Auditorias);
            IConexion.SaveChanges();
        }
    }
}
