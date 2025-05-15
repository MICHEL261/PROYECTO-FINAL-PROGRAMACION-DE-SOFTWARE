using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class FormatosPruebas
    {
        private readonly IConexion? iConexion;
        private List<Formatos>? lista;
        private Formatos? entidad;

        public FormatosPruebas()
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
            this.lista = this.iConexion!.Formatos!.ToList();
            return lista.Count > 0;
        }

        public bool Guardar()
        {

            var datos = JsonConversor.ConvertirAString(entidad);
            String operacion = "Guardar";

            GuardarAuditoria(operacion, datos);

            this.entidad = EntidadesNucleo.Formatos()!;
            this.iConexion!.Formatos!.Add(this.entidad);
            this.iConexion.SaveChanges();
            return true;
        }

        public bool Modificar()
        {
            this.entidad!.Material = "Policarbonato";
            var datos = JsonConversor.ConvertirAString(entidad);
            String operacion = "Modificar";

            GuardarAuditoria(operacion, datos);

            var entry = this.iConexion!.Entry<Formatos>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            var datos = JsonConversor.ConvertirAString(entidad);
            String operacion = "Borrar";

            GuardarAuditoria(operacion, datos);
            this.iConexion!.Formatos!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }

        public void GuardarAuditoria(String operacion, String datos)
        {
            var Auditorias = new Auditorias();
            {
                Auditorias.Entidad = "Formatos";
                Auditorias.Operacion = operacion;
                Auditorias.Fecha = DateTime.Now;
                Auditorias.Datos = datos;


            }

            iConexion!.Auditorias!.Add(Auditorias);
            iConexion.SaveChanges();
        }
    }
}