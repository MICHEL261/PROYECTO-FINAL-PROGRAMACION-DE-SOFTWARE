using lib_dominio.Entidades;
using lib_dominio.Nucleo;
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


            Assert.AreEqual(true, Guardar());
            Assert.AreEqual(true, Modificar());
            Assert.AreEqual(true, Listar());
            Assert.AreEqual(true, Borrar());



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
            var datos = JsonConversor.ConvertirAString(entidad);
            String operacion = "Guardar";

          

            this.iConexion.SaveChanges();
            return true;
        }

        public bool Modificar()
        {
            this.entidad!.GeneroMusical = "Rock";
            var datos = JsonConversor.ConvertirAString(entidad);
            String operacion = "Modificar";

           

            var entry = this.iConexion!.Entry<Artistas>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            var datos = JsonConversor.ConvertirAString(entidad!);
            String operacion = "Borrar";

           
            this.iConexion!.Artistas!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }

       


    }
}

