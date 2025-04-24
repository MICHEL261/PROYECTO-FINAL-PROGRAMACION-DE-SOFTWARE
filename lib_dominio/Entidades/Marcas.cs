namespace lib_dominio.Entidades
{
    public class Marcas 
    {
        public int Id { get; set; }
        public string? NombreMarca { get; set; }
        public string? PaginaWeb { get; set; }
        public List<Discos>? Discos { get; set; }
    }
}
