
using lib_dominio.Entidades;
using lib_presentaciones.Interfaces;
using lib_presentaciones.models;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lib_presentaciones.Implementaciones
{
    public class CarritoPresentacion : ICarritoPresentacion
    {
        private readonly IOrdenesPresentacion _ordenesPresentacion;
        private readonly IOrdenesDiscosPresentacion _ordenesDiscosPresentacion;
        private readonly IDiscosPresentacion IDiscosPresentacion;


        private List<Carrito> Items { get; set; } = new List<Carrito>();
        private List<Discos> Discos { get; set; }
        private List<Ordenes> Ordenes { get; set; }



        public CarritoPresentacion(IOrdenesPresentacion ordenesPresentacion, IOrdenesDiscosPresentacion ordenesDiscosPresentacion, IDiscosPresentacion IDiscosPresentacion)
        {
            this._ordenesPresentacion = ordenesPresentacion;
            this._ordenesDiscosPresentacion = ordenesDiscosPresentacion;
            this.IDiscosPresentacion = IDiscosPresentacion;
        }

        public void AgregarAlCarrito(Carrito item)
        {
            var existente = Items.FirstOrDefault(i => i.Disco == item.Disco && i.Formato == item.Formato);
            if (existente != null)
            {
                existente.Cantidad += item.Cantidad;
            }
            else
            {
                this.Items.Add(item);
            }
        }

        public void EliminarDelCarrito(string discoNom, int formatoId)
        {
            this.Items.RemoveAll(i => i.Disco == discoNom && i.Formato == formatoId);
        }

        public CarritoCompra ObtenerCarrito()
        {
            return new CarritoCompra
            {
               Items = Items.ToList()
            };
        }

        public async Task FinalizarCompra(int clienteId, List<Carrito> items, int pagoId)
        {
            var nuevaOrden = new Ordenes
            {
                Fecha = DateTime.Now,
                Cliente = clienteId,
                Pago = pagoId,
                MontoTotal = 0
            };

            nuevaOrden = await _ordenesPresentacion.Guardar(nuevaOrden);

            var discosDisponibles = await IDiscosPresentacion.Listar();

            foreach (var item in items)
            {
                var disco = discosDisponibles.FirstOrDefault(x => x.NombreDisco == item.Disco);
                if (disco == null)
                    throw new Exception($"No se encontró el disco '{item.Disco}'.");

                var ordenDisco = new OrdenesDiscos
                {
                    Orden = nuevaOrden!.Id,
                    Disco = disco.Id,
                    Formato = item.Formato,
                    Cantidad = item.Cantidad
                };

                await _ordenesDiscosPresentacion.Guardar(ordenDisco);
            }

            Items.Clear();
        }

        public class CarritoCompra
        {
            public List<Carrito> Items { get; set; } = new List<Carrito>();

            public decimal Total => Items.Sum(i => i.Cantidad * i.ValorUnitario);

            public void Limpiar()
            {
                Items.Clear();
            }
        }
    }
}