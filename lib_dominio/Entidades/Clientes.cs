﻿namespace lib_dominio.Entidades
{
    public class Clientes
    {
        public int Id { get; set; }
        public string? NombreCliente { get; set; }
        public string? ApellidoCliente { get; set; }
        public string? DireccionCliente { get; set; }
        public string? TelefonoCliente { get; set; }
        public List<Ordenes>? Ordenes { get; set; }

    }
}
