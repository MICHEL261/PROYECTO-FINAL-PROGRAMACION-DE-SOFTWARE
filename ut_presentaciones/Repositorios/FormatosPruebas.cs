﻿using lib_dominio.Entidades;
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

            var datos = JsonConversor.ConvertirAString(entidad!);
            String operacion = "Guardar";

           

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

        

            var entry = this.iConexion!.Entry<Formatos>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            var datos = JsonConversor.ConvertirAString(entidad!);
            String operacion = "Borrar";

           
            this.iConexion!.Formatos!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }

    }
}