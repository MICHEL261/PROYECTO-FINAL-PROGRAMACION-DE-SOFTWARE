using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_dominio.Entidades
{
    public class Auditorias
    {
        public int Id { get; set; } 
        public string Entidad {  get; set; }    
        public string Operacion { get; set; }
        public DateTime Fecha { get; set; } 
        public string Datos {  get; set; }  
    }
}
