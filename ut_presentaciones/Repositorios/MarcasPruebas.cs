using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class MarcasPruebas
    {
        private readonly IConexion? iConexion;
        private List<Marcas>? lista;
        private Marcas? entidad;

        public MarcasPruebas()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
        }

        [TestMethod]
        public void Ejecutar()
        {
            var contextoReal = (DbContext)iConexion!;
            contextoReal.Database.ExecuteSqlRaw("DISABLE TRIGGER tr_Auditoria_Marcas ON Marcas");
            contextoReal.Database.ExecuteSqlRaw("DISABLE TRIGGER tr_Update_Marcas ON Marcas");

            Assert.AreEqual(true, Guardar());
            Assert.AreEqual(true, Modificar());
            Assert.AreEqual(true, Listar());
            Assert.AreEqual(true, Borrar());

            contextoReal.Database.ExecuteSqlRaw("ENABLE TRIGGER tr_Auditoria_Marcas ON Marcas");
            contextoReal.Database.ExecuteSqlRaw("ENABLE TRIGGER tr_Update_Marcas ON Marcas");
        }

        public bool Listar()
        {
            this.lista = this.iConexion!.Marcas!.ToList();
            return lista.Count > 0;
        }

        public bool Guardar()
        {
            this.entidad = EntidadesNucleo.Marcas()!;
            this.iConexion!.Marcas!.Add(this.entidad);
            this.iConexion.SaveChanges();
            var contextoReal = (DbContext)iConexion!;

            contextoReal.Database.ExecuteSqlRaw("DISABLE TRIGGER tr_Auditoria_Marcas ON Marcas");
            return true;
        }

        public bool Modificar()
        {
            this.entidad!.NombreMarca = "WarnerMusic";

            var entry = this.iConexion!.Entry<Marcas>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            this.iConexion!.Marcas!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}
