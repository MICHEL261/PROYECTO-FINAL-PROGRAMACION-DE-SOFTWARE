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
         
            var nuevoUsuario = new Usuarios()
            {
                Nombre = "ANDRES",
                Apellido = "gomes",
                Email = "andres000@gmail",
                NombreUsuario = "andrew",
                Contraseña = "1234",
                Rol = 2
            };

          
            this.iConexion!.Usuarios!.Add(nuevoUsuario);
            this.iConexion.SaveChanges(); 

            this.entidad = EntidadesNucleo.Clientes(nuevoUsuario)!;

           
            var datos = JsonConversor.ConvertirAString(entidad);
            string operacion = "Guardar";
           

           
            this.iConexion.Clientes!.Add(this.entidad);
            this.iConexion.SaveChanges();

            return true;
        }

        public bool Modificar()
        {
            this.entidad!.TelefonoCliente = "3008543470";

            var entry = this.iConexion!.Entry<Clientes>(this.entidad);

            var datos = JsonConversor.ConvertirAString(entidad);
            String operacion = "modificar";

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
