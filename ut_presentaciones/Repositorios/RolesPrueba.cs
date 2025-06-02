using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class RolesPruebas
    {
        private readonly IConexion? iConexion;
        private List<Roles>? lista;
        private Roles? entidad;

        public RolesPruebas()
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
            this.lista = this.iConexion!.Roles!.ToList();
            return lista.Count > 0;
        }

        public bool Guardar()
        {
    

            var datos = JsonConversor.ConvertirAString(entidad!);
            String operacion = "Guardar";

          
            this.entidad = EntidadesNucleo.Roles()!;
            this.iConexion!.Roles!.Add(this.entidad);
            this.iConexion.SaveChanges();
            return true;
        }

        public bool Modificar()
        {
            this.entidad!.NombreRol = "Cliente";
            var datos = JsonConversor.ConvertirAString(entidad);
            String operacion = "Modificar";


            var entry = this.iConexion!.Entry<Roles>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            var datos = JsonConversor.ConvertirAString(entidad!);
            String operacion = "Borrar";

            this.iConexion!.Roles!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }

      
    }
}
