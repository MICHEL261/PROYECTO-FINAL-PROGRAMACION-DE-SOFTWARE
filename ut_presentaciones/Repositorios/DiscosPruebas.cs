﻿using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class DiscosPruebas
    {
        private readonly IConexion? iConexion;
        private List<Discos>? lista;
        private Discos? entidad;

        public DiscosPruebas()
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
            this.lista = this.iConexion!.Discos!.ToList();
            return lista.Count > 0;
        }

        public bool Guardar()
        {
            var artista = this.iConexion!.Artistas!.FirstOrDefault(x => x.NombreArtista == "The Doors");
            var marca = this.iConexion.Marcas!.FirstOrDefault(x => x.NombreMarca == "Sony");

            this.entidad = EntidadesNucleo.Discos(artista!, marca!)!;
            this.iConexion!.Discos!.Add(this.entidad);
            this.iConexion.SaveChanges();
            return true;
        }

        public bool Modificar()
        {
            this.entidad!.DuracionDisco = "3:20";


            

            var entry = this.iConexion!.Entry<Discos>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {


            
            this.iConexion!.Discos!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }

       
    }
}
