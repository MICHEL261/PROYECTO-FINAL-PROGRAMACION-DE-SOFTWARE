using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class ClientesPruebas
    {
        private readonly IConexion? iConexion;
        private List<Clientes>? lista;
        private Clientes? entidad;
        public ClientesPruebas()
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
            this.lista = this.iConexion!.Clientes!.ToList();
            return lista.Count > 0;
        }

        public bool Guardar()
        {
            var rol = iConexion!.Roles!.FirstOrDefault(x => x.NombreRol == "Administrador");
            Usuarios nuevoUsuario = new Usuarios
            {
                Nombre = "Prueba" + DateTime.Now + ("yyyyMMddhhss"),
                Apellido = "Prueba" + DateTime.Now + ("yyyyMMddhhss"),
                Email = "Prueba" + DateTime.Now + ("yyyyMMddhhss"),
                NombreUsuario = "Prueba" + DateTime.Now + ("yyyyMMddhhss"),
                Contraseña = "Prueba" + DateTime.Now + ("yyyyMMddhhss"),
                Rol = rol!.Id

            };
            this.iConexion.Usuarios!.Add(nuevoUsuario);
            this.iConexion.SaveChanges();
            this.entidad = EntidadesNucleo.Clientes(nuevoUsuario);
            this.iConexion!.Clientes!.Add(this.entidad!);
            this.iConexion.SaveChanges();
            return true;
        }

        public bool Modificar()
        {
            this.entidad!.TelefonoCliente = "3008543470";

            var entry = this.iConexion!.Entry<Clientes>(this.entidad);

            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            var datos = JsonConversor.ConvertirAString(entidad!);
            String operacion = "Borrar";

           
            this.iConexion!.Clientes!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }

        

    }
}
