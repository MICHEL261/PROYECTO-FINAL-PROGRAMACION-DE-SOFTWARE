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
            var datos = "Permiso: " + entidad.Permiso + ", " + "Rol: " + entidad.Rol;
            GuardarAuditoria("Borrar", datos);



            this.IConexion!.Roles_Permisos!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Roles_Permisos? Guardar(Roles_Permisos? entidad)
        {
            var contextoReal = (DbContext)IConexion!;
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");
            var datos ="Permiso: "+ entidad.Permiso + ", "+ "Rol: " + entidad.Rol;
            GuardarAuditoria("Guardar", datos);




            this.IConexion!.Roles_Permisos!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Roles_Permisos> Listar()
        {
            return this.IConexion!.Roles_Permisos!.Take(20)
                
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
            var datos = "Permiso: " + entidad.Permiso + ", " + "Rol: " + entidad.Rol;
            GuardarAuditoria("Modificar", datos);




            var entry = this.IConexion!.Entry<Roles_Permisos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
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




