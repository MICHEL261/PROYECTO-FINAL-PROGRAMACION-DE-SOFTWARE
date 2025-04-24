namespace lib_dominio.Entidades
{
    public class Artistas
    {
        public int Id { get; set; }
        public string? NombreArtista { get; set; }
        public string? GeneroMusical { get; set; }
        public List<Discos>? Discos { get; set; }

    }
}
