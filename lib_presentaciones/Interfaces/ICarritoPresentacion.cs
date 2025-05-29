using lib_presentaciones.models;
using System.Threading.Tasks;
using static lib_presentaciones.Implementaciones.CarritoPresentacion;

namespace lib_presentaciones.Interfaces
{
    public interface ICarritoPresentacion
    {
        void AgregarAlCarrito(Carrito item);
        void EliminarDelCarrito(string discoNom, int formatoId);
        CarritoCompra ObtenerCarrito();
        Task FinalizarCompra(int clienteId, List<Carrito> items, int pagoId);
    }
}