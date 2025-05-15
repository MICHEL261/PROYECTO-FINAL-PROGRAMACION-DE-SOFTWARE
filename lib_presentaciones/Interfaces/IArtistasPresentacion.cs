﻿using lib_dominio.Entidades;

namespace lib_presentaciones.Interfaces
{
    public interface IArtistasPresentacion
    {
        Task<List<Artistas>> Listar();
        Task<List<Artistas>> PorNombre(Artistas? entidad);
        Task<Artistas?> Guardar(Artistas? entidad);
        Task<Artistas?> Modificar(Artistas? entidad);
        Task<Artistas?> Borrar(Artistas? entidad);
    }
}