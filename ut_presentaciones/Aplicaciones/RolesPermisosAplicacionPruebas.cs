using lib_aplicaciones.Implementaciones;
using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Aplicaciones
{
    [TestClass]
    public class RolesPermisosAplicacionPrueba
    {
        private readonly IRoles_PermisosAplicacion? iAplicacion;
        private readonly IConexion? iConexion;
        private List<Roles_Permisos>? lista;
        private Roles_Permisos? entidad;

        public RolesPermisosAplicacionPrueba()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            iAplicacion = new Roles_PermisosAplicacion(iConexion);
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
            this.lista = this.iAplicacion!.Listar();
            return lista.Count > 0;
        }

        public bool Guardar()
        {

            var rol = this.iConexion!.Roles!.FirstOrDefault(x => x.NombreRol == "Administrador");
            var permiso = this.iConexion!.Permisos!.FirstOrDefault(x => x.Nombre == "Nuevo");

            this.entidad = EntidadesNucleo.RolesPermisos(permiso, rol)!;

            this.iAplicacion!.Guardar(this.entidad);
            return true;
        }

        public bool Modificar()
        {
            this.iAplicacion!.Modificar(this.entidad);
            return true;
        }

        public bool Borrar()
        {
            this.iAplicacion!.Borrar(this.entidad);
            return true;
        }
    }
}

