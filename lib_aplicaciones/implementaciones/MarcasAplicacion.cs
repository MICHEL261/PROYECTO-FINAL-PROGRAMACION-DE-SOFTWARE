﻿using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_aplicaciones.Implementaciones
{
    public class MarcasAplicacion : IMarcasAplicacion
    {
        private IConexion? IConexion = null;

        public MarcasAplicacion(IConexion iConexion)


        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Marcas? Borrar(Marcas? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            var datos ="NombreMarca: "+ entidad.NombreMarca + ", "+"pagina web: " + entidad.PaginaWeb;
            this.IConexion!.Marcas!.Remove(entidad);
            this.IConexion.SaveChanges();
            GuardarAuditoria("Borrar", datos);

            return entidad;
        }

        public Marcas? Guardar(Marcas? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");
            var datos = "NombreMarca: " + entidad.NombreMarca + ", " + "pagina web: " + entidad.PaginaWeb;

            this.IConexion!.Marcas!.Add(entidad);
            this.IConexion.SaveChanges();
            GuardarAuditoria("guardar", datos);

            return entidad;
        }

        public List<Marcas> Listar()
        {
            return this.IConexion!.Marcas!.Take(20).ToList();
        }

        public List<Marcas> PorNombre(Marcas? entidad)
        {
            return this.IConexion!.Marcas!
                .Where(x => x.NombreMarca!.Contains(entidad!.NombreMarca!))
                .ToList();
        }

        public Marcas? Modificar(Marcas? entidad)


        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");
            var datos = "NombreMarca: " + entidad.NombreMarca + ", " + "pagina web: " + entidad.PaginaWeb;

            var entry = this.IConexion!.Entry<Marcas>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            GuardarAuditoria("modificar", datos);

            return entidad;
        }

        public void GuardarAuditoria(String operacion, String datos)
        {
            var Auditorias = new Auditorias();
            {
                Auditorias.Entidad = "Marcas";
                Auditorias.Operacion = operacion;
                Auditorias.Fecha = DateTime.Now;
                Auditorias.Datos = datos;
            }

            IConexion!.Auditorias!.Add(Auditorias);
            IConexion.SaveChanges();
        }
    }
}



