using lib_dominio.Entidades;

namespace ut_presentacion.Nucleo
{
    
    public class EntidadesNucleo
    {
        public static Clientes? Clientes()
        {
            var entidad = new Clientes();
            entidad.NombreCliente = "Maria";
            entidad.ApellidoCliente = "Cardona";
            entidad.DireccionCliente = "Calle 12";
            entidad.TelefonoCliente = "3022016491";
            return entidad;
        }
        public static Artistas? Artistas()
        {
            var entidad = new Artistas();
            entidad.NombreArtista = "Frank Sinatra";
            entidad.GeneroMusical = "Jazz";
            return entidad;
        }
        public static Marcas? Marcas()
        {
            var entidad = new Marcas();
            entidad.NombreMarca = "Warner";
            entidad.PaginaWeb = "www.warner.com";
            return entidad;
        }
        public static Discos? Discos(Artistas artista, Marcas marca)
        {
            var entidad = new Discos();
            entidad.Artista = artista.Id;
            entidad.Marca = marca.Id;
            entidad.NombreDisco = "Touch Me";
            entidad.DuracionDisco = "2:40";
            entidad.FechaLanzamiento = new DateTime(1999, 6, 20);
            return entidad;
        }
        public static Formatos? Formatos()
        {
            var entidad = new Formatos();
            entidad.TipoFormato= "CD";
            entidad.Material = "Plastico";
            return entidad;
        }
        public static Pagos? Pagos()
        {
            var entidad = new Pagos();
            entidad.TipoPago = "Transferencia";
            entidad.Pais_Disponibilidad = "Todos los paises";
            return entidad;
        }
        public static Ordenes? Ordenes(Clientes cliente, Pagos pago)
        {
            var entidad = new Ordenes();
            entidad.Fecha = new DateTime(2024, 9, 11);
            entidad.Cliente = cliente.Id;
            entidad.Pago = pago.Id;
            entidad.MontoTotal = 1500;
            return entidad;
        }

        public static OrdenesDiscos? OrdenesDiscos(Ordenes orden, Discos disco, Formatos formato)
        {
            var entidad = new OrdenesDiscos();
            entidad.Orden = orden.Id;
            entidad.Disco = disco.Id;
            entidad.Formato = formato.Id;
            entidad.Cantidad = 2;
            entidad.ValorUnitario = 150;
            return entidad;

        }

    }
}
