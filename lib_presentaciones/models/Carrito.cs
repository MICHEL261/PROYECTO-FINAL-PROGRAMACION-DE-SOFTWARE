using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_presentaciones.models
{
    public class Carrito
    {

        public int Disco { get; set; }
        public int Formato { get; set; }
        public int Cantidad { get; set; }
        public decimal ValorUnitario { get; set; }
    }
}
