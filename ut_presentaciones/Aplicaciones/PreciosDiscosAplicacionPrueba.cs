using lib_aplicaciones.Implementaciones;
using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Aplicaciones
{
    [TestClass]
    public class PreciosDiscosAplicacionPrueba
    {
        private readonly iPreciosDiscosAplicacion? iAplicacion;
        private readonly IConexion? iConexion;
        private List<PreciosDiscos>? lista;
        private PreciosDiscos? entidad;

        public PreciosDiscosAplicacionPrueba()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            iAplicacion = new PreciosDiscosAplicacion(iConexion);
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
            var disco = this.iConexion!.Discos!.FirstOrDefault(x => x.NombreDisco == "I Wonder");
            var formato = this.iConexion.Formatos!.FirstOrDefault(x => x.TipoFormato == "Vinilo");
            this.entidad = EntidadesNucleo.PreciosDiscos(disco!, formato!);

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

