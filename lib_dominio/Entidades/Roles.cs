namespace lib_dominio.Entidades
{
    public class Roles
    {
        public int Id { get; set; }
        public string? NombreRol { get; set; }
        public string? Descripcion { get; set; }
        public List<Usuarios>? Usuarios { get; set; }

    }
}
