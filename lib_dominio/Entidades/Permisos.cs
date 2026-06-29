using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace lib_dominio.Entidades
{
    public class Permisos
    {

        public int Id { get; set; }
        public string? Nombre { get; set; }
        [JsonProperty]
        public List<Roles_Permisos>? Roles_Permisos { get; set; }

    }
}
