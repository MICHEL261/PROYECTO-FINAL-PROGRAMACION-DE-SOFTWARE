
using lib_dominio.Entidades;
using lib_presentaciones.Interfaces;
using lib_presentaciones.models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lib_presentaciones.Implementaciones
{
    public class CarritoPresentacion : ICarritoPresentacion
    {
        private readonly IOrdenesPresentacion _ordenesPresentacion;
        private readonly IOrdenesDiscosPresentacion _ordenesDiscosPresentacion;

        private List<Carrito> Items { get; set; } = new List<Carrito>();

        
        public CarritoPresentacion(IOrdenesPresentacion ordenesPresentacion, IOrdenesDiscosPresentacion ordenesDiscosPresentacion)
        {
            _ordenesPresentacion = ordenesPresentacion;
            _ordenesDiscosPresentacion = ordenesDiscosPresentacion;
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
                Items.Add(item);
            }
        }

        public void EliminarDelCarrito(int discoId, int formatoId)
        {
            Items.RemoveAll(i => i.Disco == discoId && i.Formato == formatoId);
        }

        public CarritoCompra ObtenerCarrito()
        {
            return new CarritoCompra
            {
                Items = Items.ToList()
            };
        }

        public async Task FinalizarCompra(int clienteId, int pagoId)
        {
            decimal monto = Items.Sum(i => i.Cantidad * i.ValorUnitario);

          
            var nuevaOrden = new Ordenes
            {
                Fecha = DateTime.Now,
                Cliente = clienteId,
                Pago = pagoId,
                MontoTotal = monto
            };

            
            nuevaOrden = await _ordenesPresentacion.Guardar(nuevaOrden);

            
                foreach (var item in Items)
                {
                    var ordenDisco = new OrdenesDiscos
                    {
                        Orden = 1,  
                        Disco = item.Disco,
                        Formato = item.Formato,
                        Cantidad = item.Cantidad,
                        ValorUnitario = item.ValorUnitario
                    };

                   
                    await _ordenesDiscosPresentacion.Guardar(ordenDisco);
                }
           

           
            Items.Clear();

            
            await Task.CompletedTask;
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