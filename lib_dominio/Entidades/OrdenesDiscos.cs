using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class OrdenesDiscos 
    {
        public int Id { get; set; }
        public int Orden { get; set; } 
        public int Disco { get; set; } 
        public int Formato { get; set; }
        public int Cantidad { get; set; }
        public decimal ValorUnitario { get; set; }
        [ForeignKey("Orden")] public Ordenes? _Orden { get; set; }
        [ForeignKey("Disco")] public Discos? _Disco { get; set; }
        [ForeignKey("Formato")] public Formatos? _Formato { get; set; } 



    }
}
