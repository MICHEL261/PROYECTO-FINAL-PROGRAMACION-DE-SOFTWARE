using lib_aplicaciones.Implementaciones;
using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Aplicaciones
{
    [TestClass]
    public class OrdenesDiscosAplicacionPrueba
    {
        private readonly IOrdenesDiscosAplicacion? iAplicacion;
        private readonly IConexion? iConexion;
        private List<OrdenesDiscos>? lista;
        private OrdenesDiscos? entidad;

        public OrdenesDiscosAplicacionPrueba()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            iAplicacion = new OrdenesDiscosAplicacion(iConexion);
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


            var orden = this.iConexion!.Ordenes!.FirstOrDefault(x => x._Cliente!.NombreCliente == "Juan");
            var disco = this.iConexion.Discos!.FirstOrDefault(x => x.NombreDisco == "Love Street");
            var formato = this.iConexion.Formatos!.FirstOrDefault(x => x.TipoFormato == "Vinilo");

            this.entidad = EntidadesNucleo.OrdenesDiscos(orden, disco,formato)!;

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

