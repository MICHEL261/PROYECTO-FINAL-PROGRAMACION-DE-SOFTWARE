using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class Clientes
    {
        public int Id { get; set; }
        public string? NombreCliente { get; set; }
        public string? ApellidoCliente { get; set; }
        public string? DireccionCliente { get; set; }
        public string? TelefonoCliente { get; set; }
        [JsonIgnore]
        public int Usuario { get; set; }
        [ForeignKey("Usuario")] public Usuarios? _Usuario { get; set; }
        [JsonIgnore]
        public List<Ordenes>? Ordenes { get; set; }

    }
}
