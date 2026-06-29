using lib_aplicaciones.Implementaciones;
using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Aplicaciones
{
    [TestClass]
    public class ClientesAplicacionPrueba
    {
        private readonly iClientesAplicacion? iAplicacion;
        private readonly IConexion? iConexion;
        private List<Clientes>? lista;
        private Clientes? entidad;

        public ClientesAplicacionPrueba()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            iAplicacion = new ClientesAplicacion(iConexion);
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
            // 1. Crear nuevo usuario
            var nuevoUsuario = new Usuarios
            {
                Nombre = "Juan",
                Apellido = "Pérez",
                Email = "juan.perez" + Guid.NewGuid().ToString("N").Substring(0, 5) + "@outlook.com",
                NombreUsuario = "juanp" + Guid.NewGuid().ToString("N").Substring(0, 5),
                Contraseña = "1234",
                Rol = 2
            };

           
            this.iConexion!.Usuarios!.Add(nuevoUsuario);
            this.iConexion.SaveChanges(); 

       
            this.entidad = new Clientes
            {
                NombreCliente = nuevoUsuario.Nombre,
                ApellidoCliente = nuevoUsuario.Apellido,
                DireccionCliente = "Calle falsa 123",
                TelefonoCliente = "123456789",
                Usuario = nuevoUsuario.Id 
            };

            
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

