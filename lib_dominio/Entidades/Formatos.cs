namespace lib_dominio.Entidades
{
    public class Formatos 
    {
        public int Id { get; set; }
        public string? TipoFormato { get; set; }
        public string? Material { get; set; }
        public List<OrdenesDiscos>? OrdenesDiscos { get; set; } 

    }
}
