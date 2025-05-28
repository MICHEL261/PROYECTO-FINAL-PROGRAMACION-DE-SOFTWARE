using lib_presentaciones.models;
using System.Threading.Tasks;
using static lib_presentaciones.Implementaciones.CarritoPresentacion;

namespace lib_presentaciones.Interfaces
{
    public interface ICarritoPresentacion
    {
        void AgregarAlCarrito(Carrito item);
        void EliminarDelCarrito(int discoId, int formatoId);
        CarritoCompra ObtenerCarrito(); 
        Task FinalizarCompra(int clienteId, int pagoId);
    }
}