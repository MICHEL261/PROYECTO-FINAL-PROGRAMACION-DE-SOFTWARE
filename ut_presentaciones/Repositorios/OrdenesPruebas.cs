﻿using lib_dominio.Entidades;
using lib_dominio.Nucleo;
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


            Assert.AreEqual(true, Guardar());
            Assert.AreEqual(true, Modificar());
            Assert.AreEqual(true, Listar());
            Assert.AreEqual(true, Borrar());

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

            var datos = JsonConversor.ConvertirAString(entidad!);
            String operacion = "Guardar";

           
            this.entidad = EntidadesNucleo.Ordenes(cliente!, pago!);


            this.iConexion!.Ordenes!.Add(this.entidad!);
            this.iConexion.SaveChanges();
            return true;
        }


        public bool Modificar()
        {
            var orden = this.iConexion!.Ordenes!.FirstOrDefault(x => x.Id == 1);

            var datos = JsonConversor.ConvertirAString(entidad!);
            String operacion = "Modificar";



            if (orden != null)
            {
                var ordenesDiscosPruebas = new OrdenesDiscosPruebas();

                orden.MontoTotal = ordenesDiscosPruebas.CalcularMontoTotal(orden);


            }



            var entry = this.iConexion!.Entry<Ordenes>(orden!);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {

            var datos = JsonConversor.ConvertirAString(entidad!);
            String operacion = "Borrar";

         
            this.iConexion!.Ordenes!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }

      
    }
}