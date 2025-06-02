using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_dominio.Entidades
{
    public class PreciosDiscos
    {
        public int Id { get; set; }

        public int Disco { get; set; }
        public int Formato { get; set; }
        public decimal Precio { get; set; }

        [ForeignKey("Disco")]
        public Discos? _Disco { get; set; }

        [ForeignKey("Formato")]
        public Formatos? _Formato { get; set; }
    }
}
