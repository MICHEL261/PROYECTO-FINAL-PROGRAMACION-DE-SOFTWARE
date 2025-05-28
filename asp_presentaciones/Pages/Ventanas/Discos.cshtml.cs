using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Implementaciones;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_presentaciones.Pages.Ventanas
{
    public class DiscosModel : PageModel
    {
        private IDiscosPresentacion? iPresentacion = null;
        private IArtistasPresentacion? IArtistasPresentacion = null;
        private IMarcasPresentacion? IMarcasPresentacion = null;
        


        public DiscosModel(IDiscosPresentacion iPresentacion, IArtistasPresentacion IArtistasPresentacion, IMarcasPresentacion IMarcasPresentacion)
        {
            try
            {
                this.iPresentacion = iPresentacion;
                this.IArtistasPresentacion = IArtistasPresentacion;
                this.IMarcasPresentacion = IMarcasPresentacion;
                Filtro = new Discos();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public IFormFile? FormFile { get; set; }
        [BindProperty] public Enumerables.Ventanas Accion { get; set; }
        [BindProperty] public Discos? Actual { get; set; }
        [BindProperty] public Discos? Filtro { get; set; }
        [BindProperty] public List<Discos>? Lista { get; set; }
        [BindProperty] public List<Artistas>? Artistas { get; set; }
        [BindProperty] public List<Marcas>? Marcas { get; set; }


        public virtual void OnGet() { OnPostBtRefrescar(); }

        public void OnPostBtRefrescar()
        {
            try
            {
                var variable_session = HttpContext.Session.GetString("NombreUsuario");
                
                if (String.IsNullOrEmpty(variable_session))
                {
                    HttpContext.Response.Redirect("/");
                    return;
                }

                Filtro!.NombreDisco = Filtro!.NombreDisco ?? "";

                Accion = Enumerables.Ventanas.Listas;
                var task = this.iPresentacion!.PorNombre(Filtro!);
                task.Wait();
                Lista = task.Result;
                Actual = null;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }
        private void CargarCombox()
        {
            try
            {
                var task = this.IArtistasPresentacion!.Listar();
                var task2 = this.IMarcasPresentacion!.Listar();
                task.Wait();
                task2.Wait();
                Artistas = task.Result;
                Marcas = task2.Result;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }



        public virtual void OnPostBtNuevo()
        {
            try
            {
                Accion = Enumerables.Ventanas.Editar;
                Actual = new Discos();
                CargarCombox();

            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public virtual void OnPostBtModificar(string data)
        {
            try
            {
                OnPostBtRefrescar();

                CargarCombox();

                Accion = Enumerables.Ventanas.Editar;
                Actual = Lista!.FirstOrDefault(x => x.Id.ToString() == data);

            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public virtual void OnPostBtGuardar()
        {
            try
            {
                Accion = Enumerables.Ventanas.Editar;

                Task<Discos>? task = null;
                if (Actual!.Id == 0)
                    task = this.iPresentacion!.Guardar(Actual!)!;
                else
                    task = this.iPresentacion!.Modificar(Actual!)!;
                task.Wait();
                Actual = task.Result;
                Accion = Enumerables.Ventanas.Listas;
                OnPostBtRefrescar();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public virtual void OnPostBtBorrarVal(string data)
        {
            try
            {
                OnPostBtRefrescar();
                Accion = Enumerables.Ventanas.Borrar;
                Actual = Lista!.FirstOrDefault(x => x.Id.ToString() == data);
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public virtual void OnPostBtBorrar()
        {
            try
            {
                var task = this.iPresentacion!.Borrar(Actual!);
                Actual = task.Result;
                OnPostBtRefrescar();
            }
            catch (Exception ex)
            {

                ViewData["MensajeError"] = ex.Message.ToString() + "Debe borrar primero las relaciones que tiene Discos con otras entidades";

                OnPostBtRefrescar();
            }
        }

        public void OnPostBtCancelar()
        {
            try
            {
                Accion = Enumerables.Ventanas.Listas;
                OnPostBtRefrescar();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public void OnPostBtCerrar()
        {
            try
            {
                if (Accion == Enumerables.Ventanas.Listas)
                    OnPostBtRefrescar();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }
        public bool ValidarPermiso()
        {
            var variable_session = HttpContext.Session.GetString("NombreUsuario");

            if (string.IsNullOrEmpty(variable_session))
                return false;

            var UsuarioPresentacion = new UsuariosPresentacion();
            var usuarios = UsuarioPresentacion.Listar().Result;

            if (usuarios == null)
                return false;

            var usuario = usuarios.FirstOrDefault(x => x.NombreUsuario!.ToLower() == variable_session.ToLower());

            if (usuario == null)
                return false;


            bool Edita = false;
            bool Borra = false;
            bool Nuevo = false;
            bool Listar = false;

            switch (usuario.Rol)
            {
                case 1:
                    Edita = Borra = Nuevo = Listar = true;
                    break;
                case 2:
                    Listar = true;
                    break;
                default:
                    return false;
            }


            return Edita || Borra || Nuevo || Listar;
        }
    }
}