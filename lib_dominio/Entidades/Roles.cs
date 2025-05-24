using Newtonsoft.Json;

namespace lib_dominio.Entidades
{
    public class Roles
    {
        public int Id { get; set; }
        public string? NombreRol { get; set; }
        public string? Descripcion { get; set; }
        [JsonIgnore]
        public List<Usuarios>? Usuarios { get; set; }
        

        public List<Roles_Permisos>? Roles_Permisos { get; set; }
    }
}
