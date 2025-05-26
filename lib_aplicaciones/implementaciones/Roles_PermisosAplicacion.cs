using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_aplicaciones.Implementaciones
{
    public class Roles_PermisosAplicacion : IRoles_PermisosAplicacion
    {
        private IConexion? IConexion = null;

        public Roles_PermisosAplicacion(IConexion iConexion)


        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Roles_Permisos? Borrar(Roles_Permisos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");
            if (entidad._Permiso == null && entidad.Permiso != 0)
            {
                entidad._Permiso = IConexion!.Permisos!.FirstOrDefault(m => m.Id == entidad.Permiso);
                if (entidad._Permiso != null)
                    IConexion.Entry(entidad._Permiso).State = EntityState.Unchanged;
            }

            if (entidad._Rol == null && entidad.Rol != 0)
            {
                entidad._Rol = IConexion!.Roles!.FirstOrDefault(a => a.Id == entidad.Rol);
                if (entidad._Rol != null)
                    IConexion.Entry(entidad._Rol).State = EntityState.Unchanged;
            }
            var datos = "Permiso: " + entidad._Permiso!.Nombre + ", " + "Rol: " + entidad._Rol!.NombreRol;



            this.IConexion!.Roles_Permisos!.Remove(entidad);
            this.IConexion.SaveChanges();
            GuardarAuditoria("Borrar", datos);

            return entidad;
        }

        public Roles_Permisos? Guardar(Roles_Permisos? entidad)
        {
            var contextoReal = (DbContext)IConexion!;
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");
            if (entidad._Permiso == null && entidad.Permiso != 0)
            {
                entidad._Permiso = IConexion!.Permisos!.FirstOrDefault(m => m.Id == entidad.Permiso);
                if (entidad._Permiso != null)
                    IConexion.Entry(entidad._Permiso).State = EntityState.Unchanged;
            }

            if (entidad._Rol == null && entidad.Rol != 0)
            {
                entidad._Rol = IConexion!.Roles!.FirstOrDefault(a => a.Id == entidad.Rol);
                if (entidad._Rol != null)
                    IConexion.Entry(entidad._Rol).State = EntityState.Unchanged;
            }
            var datos = "Permiso: " + entidad._Permiso!.Nombre + ", " + "Rol: " + entidad._Rol!.NombreRol;




            this.IConexion!.Roles_Permisos!.Add(entidad);
            this.IConexion.SaveChanges();
            GuardarAuditoria("Guardar", datos);

            return entidad;
        }

        public List<Roles_Permisos> Listar()
        {
            return this.IConexion!.Roles_Permisos!.Take(20)
                .Include(x => x._Rol)
                .Include(x => x._Permiso)

                .ToList();
        }

        public List<Roles_Permisos> PorId(Roles_Permisos? entidad)
        {
            return this.IConexion!.Roles_Permisos!
                .Where(x => x.Id! == (entidad!.Id!))
                
                .ToList();
        }

        public Roles_Permisos? Modificar(Roles_Permisos? entidad)


        {

            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");
            if (entidad._Permiso == null && entidad.Permiso != 0)
            {
                entidad._Permiso = IConexion!.Permisos!.FirstOrDefault(m => m.Id == entidad.Permiso);
                if (entidad._Permiso != null)
                    IConexion.Entry(entidad._Permiso).State = EntityState.Unchanged;
            }

            if (entidad._Rol == null && entidad.Rol != 0)
            {
                entidad._Rol = IConexion!.Roles!.FirstOrDefault(a => a.Id == entidad.Rol);
                if (entidad._Rol != null)
                    IConexion.Entry(entidad._Rol).State = EntityState.Unchanged;
            }
            var datos = "Permiso: " + entidad._Permiso!.Nombre + ", " + "Rol: " + entidad._Rol!.NombreRol;

            var entry = this.IConexion!.Entry<Roles_Permisos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            GuardarAuditoria("Modificar", datos);

            return entidad;
        }
        
        public void GuardarAuditoria(String operacion, String datos)
        {
            var Auditorias = new Auditorias();
            {
                Auditorias.Entidad = "Roles_Permisos";
                Auditorias.Operacion = operacion;
                Auditorias.Fecha = DateTime.Now;
                Auditorias.Datos = datos;
            }

            IConexion!.Auditorias!.Add(Auditorias);
            IConexion.SaveChanges();
        }

    }

}




