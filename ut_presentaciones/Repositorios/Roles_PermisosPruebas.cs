using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class Roles_PermisosPruebas
    {
        private readonly IConexion? iConexion;
        private List<Roles_Permisos>? lista;
        private Roles_Permisos? entidad;

        public Roles_PermisosPruebas()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
        }

        [TestMethod]
        public void Ejecutar()
        {


            Assert.AreEqual(true, Guardar());
            Assert.AreEqual(true, Modificar());
            Assert.AreEqual(true, Listar());
            Assert.AreEqual(true, Borrar());


        }

        public bool Listar()
        {
            this.lista = this.iConexion!.Roles_Permisos!.ToList();
            return lista.Count > 0;
        }

        public bool Guardar()
        {


            var datos = JsonConversor.ConvertirAString(entidad!);
            String operacion = "Guardar";
            var rol = this.iConexion!.Roles!.FirstOrDefault(x => x.NombreRol == "Administrador");
            var permiso = this.iConexion!.Permisos!.FirstOrDefault(x => x.Nombre == "Nuevo");


            GuardarAuditoria(operacion, datos);
            this.entidad = EntidadesNucleo.RolesPermisos(permiso, rol!)!;
            this.iConexion!.Roles_Permisos!.Add(this.entidad);
            this.iConexion.SaveChanges();
            return true;
        }

        public bool Modificar()
        {
            this.entidad!.Rol = 2;
            var datos = JsonConversor.ConvertirAString(entidad);
            String operacion = "Modificar";

            GuardarAuditoria(operacion, datos);

            var entry = this.iConexion!.Entry<Roles_Permisos>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            var datos = JsonConversor.ConvertirAString(entidad!);
            String operacion = "Borrar";

            GuardarAuditoria(operacion, datos);
            this.iConexion!.Roles_Permisos!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
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

            iConexion!.Auditorias!.Add(Auditorias);
            iConexion.SaveChanges();
        }
    }
}
