using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class PreciosDiscosPruebas
    {
        private readonly IConexion? iConexion;
        private List<PreciosDiscos>? lista;
        private PreciosDiscos? entidad;

        public PreciosDiscosPruebas()
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
            this.lista = this.iConexion!.PreciosDiscos!.ToList();
            return lista.Count > 0;
        }

        public bool Guardar()
        {
            var disco = this.iConexion!.Discos!.FirstOrDefault(x => x.NombreDisco == "I Wonder");
            var formato = this.iConexion.Formatos!.FirstOrDefault(x => x.TipoFormato == "Vinilo");

            var datos = JsonConversor.ConvertirAString(entidad!);
            String operacion = "Guardar";


            this.entidad = EntidadesNucleo.PreciosDiscos(disco!, formato!)!;
            this.iConexion!.PreciosDiscos!.Add(this.entidad);
            this.iConexion.SaveChanges();
            return true;
        }

        public bool Modificar()
        {
            this.entidad!.Precio = 300;
            var datos = JsonConversor.ConvertirAString(entidad);
            String operacion = "Modificar";



            var entry = this.iConexion!.Entry<PreciosDiscos>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            var datos = JsonConversor.ConvertirAString(entidad!);
            String operacion = "Borrar";


            this.iConexion!.PreciosDiscos!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }


    }
}
