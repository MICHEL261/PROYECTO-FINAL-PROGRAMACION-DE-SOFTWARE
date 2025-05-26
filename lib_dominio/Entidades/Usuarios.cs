using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class Usuarios
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Contraseña { get; set; }
        public int Rol { get; set; }

        [ForeignKey("Rol")] public Roles? _Rol { get; set; }
    }
}
