using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_aplicaciones.Implementaciones
{
    public class OrdenesDiscosAplicacion : IOrdenesDiscosAplicacion
    {
        private IConexion? IConexion = null;

        public OrdenesDiscosAplicacion(IConexion iConexion)


        {
            this.IConexion = iConexion;
        }


        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public OrdenesDiscos? Borrar(OrdenesDiscos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");
            if (entidad._Formato == null && entidad.Formato != 0)
            {
                entidad._Formato = IConexion!.Formatos!.FirstOrDefault(m => m.Id == entidad.Formato);
                if (entidad._Formato != null)
                    IConexion.Entry(entidad._Formato).State = EntityState.Unchanged;
            }

            if (entidad._Disco == null && entidad.Disco != 0)
            {
                entidad._Disco = IConexion!.Discos!.FirstOrDefault(a => a.Id == entidad.Disco);
                if (entidad._Disco != null)
                    IConexion.Entry(entidad._Disco).State = EntityState.Unchanged;
            }


            var datos = "Orden: " + entidad._Orden + ", " + "Formato: " + entidad._Formato!.TipoFormato + ", " + "Cantidad: " + entidad.Cantidad + ", "  + "Disco: " + entidad._Disco!.NombreDisco;


            this.IConexion!.OrdenesDiscos!.Remove(entidad);
            this.IConexion.SaveChanges();
            GuardarAuditoria("borrar", datos);

            return entidad;
        }

        public OrdenesDiscos? Guardar(OrdenesDiscos? entidad)
        {
            var contextoReal = (DbContext)IConexion!;
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");
            if (entidad._Formato == null && entidad.Formato != 0)
            {
                entidad._Formato = IConexion!.Formatos!.FirstOrDefault(m => m.Id == entidad.Formato);
                if (entidad._Formato != null)
                    IConexion.Entry(entidad._Formato).State = EntityState.Unchanged;
            }

            if (entidad._Disco == null && entidad.Disco != 0)
            {
                entidad._Disco = IConexion!.Discos!.FirstOrDefault(a => a.Id == entidad.Disco);
                if (entidad._Disco != null)
                    IConexion.Entry(entidad._Disco).State = EntityState.Unchanged;
            }
            

            var datos = "Orden: " + entidad._Orden + ", " + "Formato: " + entidad._Formato!.TipoFormato + ", " + "Cantidad: " + entidad.Cantidad + ", " + "Disco: " + entidad._Disco!.NombreDisco;

            this.IConexion!.OrdenesDiscos!.Add(entidad);
            this.IConexion.SaveChanges();
            GuardarAuditoria("guardar", datos); 

            return entidad;
        }

        public List<OrdenesDiscos> Listar()
        {
            return this.IConexion!.OrdenesDiscos!.Take(20)
                .Include(x => x._Orden)
                .Include(x => x._Disco)
                .Include(x => x._Formato)
                .ToList();
        }

        public List<OrdenesDiscos> PorId(OrdenesDiscos? entidad)
        {
            return this.IConexion!.OrdenesDiscos!
                .Where(x => x.Id! == (entidad!.Id!))
                .Include(x => x._Orden)
                .Include(x => x._Disco)
                .Include(x => x._Formato)
                .ToList();
        }

        public OrdenesDiscos? Modificar(OrdenesDiscos? entidad)


        {

            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            if (entidad._Formato == null && entidad.Formato != 0)
            {
                entidad._Formato = IConexion!.Formatos!.FirstOrDefault(m => m.Id == entidad.Formato);
                if (entidad._Formato != null)
                    IConexion.Entry(entidad._Formato).State = EntityState.Unchanged;
            }

            if (entidad._Disco == null && entidad.Disco != 0)
            {
                entidad._Disco = IConexion!.Discos!.FirstOrDefault(a => a.Id == entidad.Disco);
                if (entidad._Disco != null)
                    IConexion.Entry(entidad._Disco).State = EntityState.Unchanged;
            }


            var datos = "Orden: " + entidad._Orden + ", " + "Formato: " + entidad._Formato!.TipoFormato + ", " + "Cantidad: " + entidad.Cantidad + ", " + "Disco: " + entidad._Disco!.NombreDisco;

            var entry = this.IConexion!.Entry<OrdenesDiscos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            GuardarAuditoria("modificar", datos);

            return entidad;
        }
        public decimal CalcularMontoTotal(Ordenes? orden)
        {
            var respuesta = 0.0m;

            var entidades = this.IConexion!.OrdenesDiscos!.Where(p => p.Orden == orden!.Id).ToList();
            if (entidades == null)
            {
                throw new Exception("La orden no existe.");
            }

            foreach (var elemento in entidades)
            {
                var precio = this.IConexion.PreciosDiscos!
                    .FirstOrDefault(p => p.Disco == elemento.Disco && p.Formato == elemento.Formato);

                if (precio == null)
                {
                    throw new Exception($"No se encontró el precio para el disco {elemento.Disco} en el formato {elemento.Formato}.");
                }

                respuesta += elemento.Cantidad * precio.Precio;
            }

            return respuesta;
        }
        public void ActualizarMonto(OrdenesDiscos? entidad)
        {

            var orden = this.IConexion!.Ordenes!.FirstOrDefault(p => p.Id == entidad!.Orden);

            orden!.MontoTotal = CalcularMontoTotal(orden);

            var entry = this.IConexion.Entry<Ordenes>(orden!);
            entry.State = EntityState.Modified;
            this.IConexion!.SaveChanges();

        }

        public void GuardarAuditoria(String operacion, String datos)
        {
            var Auditorias = new Auditorias();
            {
                Auditorias.Entidad = "OrdenesDiscos";
                Auditorias.Operacion = operacion;
                Auditorias.Fecha = DateTime.Now;
                Auditorias.Datos = datos;
            }

            IConexion!.Auditorias!.Add(Auditorias);
            IConexion.SaveChanges();
        }

    }

}




