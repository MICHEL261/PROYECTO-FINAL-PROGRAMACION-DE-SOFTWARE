using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_dominio.Entidades
{
    public class Roles_Permisos
    {

        public int Id { get; set; }
        public int Permiso { get; set; }
        public int Rol{ get; set; }
        [ForeignKey("Permiso")] public Permisos? _Permiso { get; set; }
        [ForeignKey("Rol")] public Roles? _Rol { get; set; }

    }
}
