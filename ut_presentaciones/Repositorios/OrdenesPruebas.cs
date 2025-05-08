using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class OrdenesPruebas
    {
        private readonly IConexion? iConexion;
        private List<Ordenes>? lista;
        private Ordenes? entidad;

        public OrdenesPruebas()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
        }

        [TestMethod]
        public void Ejecutar()
        {
            var contextoReal = (DbContext)iConexion!;
            contextoReal.Database.ExecuteSqlRaw("DISABLE TRIGGER tr_Auditoria_Ordenes ON Ordenes");
            contextoReal.Database.ExecuteSqlRaw("DISABLE TRIGGER tr_update_Ordenes ON Ordenes");
            contextoReal.Database.ExecuteSqlRaw("DISABLE TRIGGER tr_Delete_Ordenes ON Ordenes");

            Assert.AreEqual(true, Guardar());
            Assert.AreEqual(true, Modificar());
            Assert.AreEqual(true, Listar());
            Assert.AreEqual(true, Borrar());

            contextoReal.Database.ExecuteSqlRaw("ENABLE TRIGGER tr_Auditoria_Ordenes ON Ordenes");
            contextoReal.Database.ExecuteSqlRaw("ENABLE TRIGGER tr_update_Ordenes ON Ordenes");
            contextoReal.Database.ExecuteSqlRaw("ENABLE TRIGGER tr_Delete_Ordenes ON Ordenes");
        }

        public bool Listar()
        {
            this.lista = this.iConexion!.Ordenes!.ToList();
            return lista.Count > 0;
        }

        public bool Guardar()
        {
            var cliente = this.iConexion!.Clientes!.FirstOrDefault(x => x.NombreCliente == "Juan");
            var pago = this.iConexion.Pagos!.FirstOrDefault(x => x.Id == 1);
            this.entidad = EntidadesNucleo.Ordenes(cliente!, pago!);


            this.iConexion!.Ordenes!.Add(this.entidad!);
            this.iConexion.SaveChanges();
            return true;
        }
      

        public bool Modificar()
        {
            var orden = this.iConexion.Ordenes!.FirstOrDefault(x => x.Id == 1);

           
                if (orden != null)
            {
                var ordenesDiscosPruebas = new OrdenesDiscosPruebas();

                this.entidad!.MontoTotal = ordenesDiscosPruebas.CalcularMontoTotal(orden);
               

            }

            

            var entry = this.iConexion!.Entry<Ordenes>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            this.iConexion!.Ordenes!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}