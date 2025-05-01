using lib_dominio.Entidades;
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
            var contextoReal = (DbContext)iConexion!;
            contextoReal.Database.ExecuteSqlRaw("DISABLE TRIGGER tr_Auditoria_Clientes ON clientes");
            contextoReal.Database.ExecuteSqlRaw("DISABLE TRIGGER tr_update_Clientes ON Clientes");

            Assert.AreEqual(true, Guardar());
            Assert.AreEqual(true, Modificar());
            Assert.AreEqual(true, Listar());
            Assert.AreEqual(true, Borrar());

            contextoReal.Database.ExecuteSqlRaw("ENABLE TRIGGER tr_Auditoria_Clientes ON Clientes");
            contextoReal.Database.ExecuteSqlRaw("ENABLE TRIGGER tr_update_Clientes ON Clientes");

        }

        public bool Listar()
        {
            this.lista = this.iConexion!.Clientes!.ToList();
            return lista.Count > 0;
        }

        public bool Guardar()
        {
            this.entidad = EntidadesNucleo.Clientes()!;
            this.iConexion!.Clientes!.Add(this.entidad);
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
            this.iConexion!.Clientes!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}
