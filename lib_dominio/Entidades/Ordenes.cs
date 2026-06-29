using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class Ordenes
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int Cliente { get; set; }
        public int Pago { get; set; }
        public decimal MontoTotal { get; set; }

        [ForeignKey("Cliente")] public Clientes? _Cliente { get; set; }
        [ForeignKey("Pago")] public Pagos? _Pago { get; set; }
        [JsonIgnore]
        public List<OrdenesDiscos>? OrdenesDiscos { get; set; }

        [NotMapped]
        public string Descripcion => $"Orden #{Id} - {Fecha.ToShortDateString()} - Total: {MontoTotal:C}";
    }
}

