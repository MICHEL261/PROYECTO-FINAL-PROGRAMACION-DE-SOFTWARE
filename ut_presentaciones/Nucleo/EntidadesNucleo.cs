using lib_dominio.Entidades;

namespace ut_presentacion.Nucleo
{

    public class EntidadesNucleo
    {
        public static Clientes? Clientes(Usuarios usuarios)
        {
            var entidad = new Clientes();
            entidad.NombreCliente = "Maria";
            entidad.ApellidoCliente = "Cardona";
            entidad.DireccionCliente = "Calle 12";
            entidad.TelefonoCliente = "3022016491";
            entidad.Usuario = usuarios.Id; 
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
            entidad.Imagen="https://example.com/imagen.jpg";
            return entidad;
        }
        public static Formatos? Formatos()
        {
            var entidad = new Formatos();
            entidad.TipoFormato = "CD";
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
            return entidad;

        }
        public static Roles? Roles()
        {
            var entidad = new Roles();
            entidad.NombreRol = "administrador";
            entidad.Descripcion = "crear, guardar, editar";

            return entidad;

        }

        public static Usuarios? Usuarios(Roles Rol)
        {
            var entidad = new Usuarios();
            entidad.Nombre = "Maria";
            entidad.Apellido = "ramirez";
            entidad.Email = "maria@gmail.com";
            entidad.NombreUsuario = "mariaG";
            entidad.Contraseña = "contraseña";
            entidad.Rol = Rol.Id;

            return entidad;

        }
        public static Roles_Permisos? RolesPermisos(Permisos? permiso, Roles rol)
        {
            var entidad = new Roles_Permisos();
            entidad.Permiso = permiso!.Id;
            entidad.Rol = rol.Id;

            return entidad;

        }
        public static Permisos? Permisos()
        {
            var entidad = new Permisos();
            entidad.Nombre = "Editar";

            return entidad;

        }


    }
}
