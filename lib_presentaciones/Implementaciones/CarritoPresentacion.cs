using lib_presentaciones.Interfaces;
using lib_presentaciones.models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lib_presentaciones.Implementaciones
{
    public class CarritoPresentacion : ICarritoPresentacion
    {
        private List<Carrito> Items { get; set; } = new List<Carrito>();

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
                Items = Items.ToList() // Pasar una copia o nueva lista
            };
        }

        public async Task FinalizarCompra(int clienteId, int pagoId)
        {
            // Aquí implementa la lógica real para finalizar compra.
            // Por ejemplo, enviar la orden a tu backend, guardar en base de datos, etc.
            await Task.CompletedTask;
            // Por ahora es un stub
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