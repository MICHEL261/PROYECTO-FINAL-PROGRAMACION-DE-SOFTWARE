using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class OrdenesDiscosPruebas
    {
        private readonly IConexion? iConexion;
        private List<OrdenesDiscos>? lista;
        private OrdenesDiscos? entidad;

        public OrdenesDiscosPruebas()
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
            this.lista = this.iConexion!.OrdenesDiscos!.ToList();
            return lista.Count > 0;
        }

        public bool Guardar()
        {
            var orden = this.iConexion!.Ordenes!.FirstOrDefault(x => x._Cliente!.NombreCliente == "Juan");
            var disco = this.iConexion.Discos!.FirstOrDefault(x => x.NombreDisco == "Love Street");
            var formato = this.iConexion.Formatos!.FirstOrDefault(x => x.TipoFormato == "Vinilo");
            this.entidad = EntidadesNucleo.OrdenesDiscos(orden!, disco!, formato!)!;
            this.iConexion!.OrdenesDiscos!.Add(this.entidad);
            this.iConexion.SaveChanges();
            return true;
        }

        public bool Modificar()
        {
            this.entidad!.Cantidad = 4;

            var entry = this.iConexion!.Entry<OrdenesDiscos>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            this.iConexion!.OrdenesDiscos!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}
