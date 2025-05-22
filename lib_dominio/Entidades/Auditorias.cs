using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class Auditorias
    {
        public int Id { get; set; }
        public string? Entidad { get; set; }
        public string? Operacion { get; set; }
        public DateTime Fecha { get; set; }
        public string? Datos { get; set; }

        [NotMapped]
        public object? DatosDeserializados { get; set; }
    }
}
