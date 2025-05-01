using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class ArtistasPruebas
    {
        private readonly IConexion? iConexion;
        private List<Artistas>? lista;
        private Artistas? entidad;

        public ArtistasPruebas()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
        }

        [TestMethod]
        public void Ejecutar()
        {
            var contextoReal = (DbContext)iConexion!;
            contextoReal.Database.ExecuteSqlRaw("DISABLE TRIGGER tr_Auditoria_Artistas ON Artistas");
            contextoReal.Database.ExecuteSqlRaw("DISABLE TRIGGER tr_update_Artistas ON Artistas");

            Assert.AreEqual(true, Guardar());
            Assert.AreEqual(true, Modificar());
            Assert.AreEqual(true, Listar());
            Assert.AreEqual(true, Borrar());

            contextoReal.Database.ExecuteSqlRaw("ENABLE TRIGGER tr_Auditoria_Artistas ON Artistas");
            contextoReal.Database.ExecuteSqlRaw("ENABLE TRIGGER tr_Update_Artistas ON Artistas");

        }

        public bool Listar()
        {
            this.lista = this.iConexion!.Artistas!.ToList();
            return lista.Count > 0;
        }

        public bool Guardar()
        {
            this.entidad = EntidadesNucleo.Artistas()!;
            this.iConexion!.Artistas!.Add(this.entidad);
            this.iConexion.SaveChanges();
            return true;
        }

        public bool Modificar()
        {
            this.entidad!.GeneroMusical = "Rock";

            var entry = this.iConexion!.Entry<Artistas>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            this.iConexion!.Artistas!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}

