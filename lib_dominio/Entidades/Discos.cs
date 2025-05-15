
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class Discos
    {
        public int Id { get; set; }
        public int Artista { get; set; }
        public int Marca { get; set; }
        public string? NombreDisco { get; set; }
        public string? DuracionDisco { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        [ForeignKey("Artista")] public Artistas? _Artista { get; set; }
        [ForeignKey("Marca")] public Marcas? _Marca { get; set; }
        public List<OrdenesDiscos>? OrdenesDiscos { get; set; }

    }
}
