using Newtonsoft.Json;

namespace lib_dominio.Entidades
{
    public class Formatos
    {
        public int Id { get; set; }
        public string? TipoFormato { get; set; }
        public string? Material { get; set; }
        [JsonIgnore]
        public List<OrdenesDiscos>? OrdenesDiscos { get; set; }

    }
}
