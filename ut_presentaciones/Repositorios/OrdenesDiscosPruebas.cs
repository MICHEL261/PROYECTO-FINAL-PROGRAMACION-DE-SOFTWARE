using lib_dominio.Entidades;
using lib_dominio.Nucleo;
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
            var datos = JsonConversor.ConvertirAString(entidad!);
            String operacion = "Guardar";

            GuardarAuditoria(operacion, datos);
            this.entidad = EntidadesNucleo.OrdenesDiscos(orden!, disco!, formato!)!;
            this.iConexion!.OrdenesDiscos!.Add(this.entidad);
            this.iConexion.SaveChanges();
            return true;
        }

        public bool Modificar()
        {
            this.entidad!.Cantidad = 4;

            var entry = this.iConexion!.Entry<OrdenesDiscos>(this.entidad);
            var datos = JsonConversor.ConvertirAString(entidad);
            String operacion = "Modificar";

            GuardarAuditoria(operacion, datos);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            var datos = JsonConversor.ConvertirAString(entidad!);
            String operacion = "Borrar";

            GuardarAuditoria(operacion, datos);
            this.iConexion!.OrdenesDiscos!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }

        public decimal CalcularMontoTotal(Ordenes? orden)
        {

            var respuesta = 0.0m;

            var entidades = this.iConexion!.OrdenesDiscos!.Where(p => p.Orden == orden!.Id).ToList();
            if (orden == null)
            {
                throw new Exception("La orden no existe.");
            }


            foreach (var elemento in entidades)
                respuesta += elemento.Cantidad * elemento.ValorUnitario;



            return respuesta;
        }

        public void GuardarAuditoria(String operacion, String datos)
        {
            var Auditorias = new Auditorias();
            {
                Auditorias.Entidad = "OrdenesDiscos";
                Auditorias.Operacion = operacion;
                Auditorias.Fecha = DateTime.Now;
                Auditorias.Datos = datos;


            }

            iConexion!.Auditorias!.Add(Auditorias);
            iConexion.SaveChanges();
        }
    }
}
