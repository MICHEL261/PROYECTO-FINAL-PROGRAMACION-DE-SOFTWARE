
using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace lib_aplicaciones.Implementaciones
{
    public class PreciosDiscosAplicacion : iPreciosDiscosAplicacion
    {
        private IConexion? IConexion = null;

        public PreciosDiscosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public PreciosDiscos? Borrar(PreciosDiscos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            if (entidad._Disco == null && entidad.Disco != 0)
            {
                entidad._Disco = IConexion!.Discos!.FirstOrDefault(m => m.Id == entidad.Disco);
                if (entidad._Disco != null)
                    IConexion.Entry(entidad._Disco).State = EntityState.Unchanged;
            }

            if (entidad._Formato == null && entidad.Formato != 0)
            {
                entidad._Formato = IConexion!.Formatos!.FirstOrDefault(a => a.Id == entidad.Formato);
                if (entidad._Formato != null)
                    IConexion.Entry(entidad._Formato).State = EntityState.Unchanged;
            }

            var datos = "Nombre: " + entidad._Disco!.NombreDisco + ", " + "Formato: " + entidad._Formato?.TipoFormato + ", " +  "Precio: " + entidad.Precio;



            this.IConexion!.PreciosDiscos!.Remove(entidad);
            this.IConexion.SaveChanges();
            GuardarAuditoria("Borrar", datos);

            return entidad;
        }

        public PreciosDiscos? Guardar(PreciosDiscos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");

            if (entidad._Disco == null && entidad.Disco != 0)
            {
                entidad._Disco = IConexion!.Discos!.FirstOrDefault(m => m.Id == entidad.Disco);
                if (entidad._Disco != null)
                    IConexion.Entry(entidad._Disco).State = EntityState.Unchanged;
            }

            if (entidad._Formato == null && entidad.Formato != 0)
            {
                entidad._Formato = IConexion!.Formatos!.FirstOrDefault(a => a.Id == entidad.Formato);
                if (entidad._Formato != null)
                    IConexion.Entry(entidad._Formato).State = EntityState.Unchanged;
            }

            var datos = "Nombre: " + entidad._Disco!.NombreDisco + ", " + "Formato: " + entidad._Formato?.TipoFormato + ", " + "Precio: " + entidad.Precio;

            // Calculos

            this.IConexion!.PreciosDiscos!.Add(entidad);
            this.IConexion.SaveChanges();
            GuardarAuditoria("Guardar", datos);

            return entidad;
        }

        public List<PreciosDiscos> Listar()
        {
            return this.IConexion!.PreciosDiscos!
                .Take(20)
                .Include(x => x._Disco)
                .Include(x => x._Formato)
                .ToList();
        }

        public PreciosDiscos? Modificar(PreciosDiscos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");


            if (entidad._Disco == null && entidad.Disco != 0)
            {
                entidad._Disco = IConexion!.Discos!.FirstOrDefault(m => m.Id == entidad.Disco);
                if (entidad._Disco != null)
                    IConexion.Entry(entidad._Disco).State = EntityState.Unchanged;
            }

            if (entidad._Formato == null && entidad.Formato != 0)
            {
                entidad._Formato = IConexion!.Formatos!.FirstOrDefault(a => a.Id == entidad.Formato);
                if (entidad._Formato != null)
                    IConexion.Entry(entidad._Formato).State = EntityState.Unchanged;
            }

            var datos = "Nombre: " + entidad._Disco!.NombreDisco + ", " + "Formato: " + entidad._Formato?.TipoFormato + ", " + "Precio: " + entidad.Precio;

            // Calculos

            this.IConexion!.PreciosDiscos!.Add(entidad);
            this.IConexion.SaveChanges();
            GuardarAuditoria("Modificar", datos);

            return entidad;
        }
        public void GuardarAuditoria(String operacion, String datos)
        {
            var Auditorias = new Auditorias();
            {
                Auditorias.Entidad = "PreciosDiscos";
                Auditorias.Operacion = operacion;
                Auditorias.Fecha = DateTime.Now;
                Auditorias.Datos = datos;
            }

            IConexion!.Auditorias!.Add(Auditorias);
            IConexion.SaveChanges();
        }

        public void ObtenerDatos(String operacion, String datos)
        {
            var Auditorias = new Auditorias();
            {
                Auditorias.Entidad = "PreciosDiscos";
                Auditorias.Operacion = operacion;
                Auditorias.Fecha = DateTime.Now;
                Auditorias.Datos = datos;
            }

            IConexion!.Auditorias!.Add(Auditorias);
            IConexion.SaveChanges();
        }
    }
}