using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Implementaciones;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_presentaciones.Pages.Ventanas
{
    public class ArtistasModel : PageModel
    {
        private IArtistasPresentacion? iPresentacion = null;

        public ArtistasModel(IArtistasPresentacion iPresentacion)
        {
            try
            {
                this.iPresentacion = iPresentacion;
                Filtro = new Artistas();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public IFormFile? FormFile { get; set; }
        [BindProperty] public Enumerables.Ventanas Accion { get; set; }
        [BindProperty] public Artistas? Actual { get; set; }
        [BindProperty] public Artistas? Filtro { get; set; }
        [BindProperty] public List<Artistas>? Lista { get; set; }

        public bool Edita { get; set; } = false;
        public bool Nuevo { get; set; } = false;
        public bool Borra { get; set; } = false;

        public virtual void OnGet()
        {


            OnPostBtRefrescar();
          


        }

        public void OnPostBtRefrescar()
        {
            try
            {
                var variable_session = HttpContext.Session.GetString("NombreUsuario");
                if (!ValidarPermiso())
                {
                    // Puedes redirigir, lanzar excepción, o establecer un mensaje de error en ViewData
                    TempData["MensajeError"] = "No tienes permisos para Listar.";
                    return;
                }
                if (String.IsNullOrEmpty(variable_session))
                {
                    HttpContext.Response.Redirect("/");
                    return;
                }
                

                Filtro!.NombreArtista = Filtro!.NombreArtista ?? "";

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

        public virtual void OnPostBtNuevo()
        {
            try
            {
                Accion = Enumerables.Ventanas.Editar;
                Actual = new Artistas();
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
                if (!ValidarPermiso())
                {
                    // Puedes redirigir, lanzar excepción, o establecer un mensaje de error en ViewData
                    TempData["MensajeError"] = "No tienes permisos para modificar.";
                    return;
                }
                OnPostBtRefrescar();
                OnGet();


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
                if (!ValidarPermiso())
                {
                    TempData["MensajeError"] = "No tienes permisos para Guardar.";
                    return;
                }
                Accion = Enumerables.Ventanas.Editar;

                Task<Artistas>? task = null;
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

                if (!ValidarPermiso())
                {
                    TempData["MensajeError"] = "No tienes permisos para Borrar.";
                    return;
                }
                var task = this.iPresentacion!.Borrar(Actual!);
                Actual = task.Result;
                OnPostBtRefrescar();
            }

            catch (Exception ex)
            {



                ViewData["MensajeError"] = ex.Message.ToString() + "Debe borrar primero las relaciones que tiene el Artista con otras entidades";

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
            var variable_session = HttpContext.Session.GetString("Usuario");

            if (string.IsNullOrEmpty(variable_session))
                return false;

            var UsuarioPresentacion = new UsuariosPresentacion();
            var usuarios = UsuarioPresentacion.Listar().Result;

            if (usuarios == null)
                return false;

            var usuario = usuarios.FirstOrDefault(x => x.NombreUsuario.ToLower() == variable_session.ToLower());

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
                    
                    break;
                default:
                    return false;
            }

           
            return Edita || Borra || Nuevo || Listar;
        }
    }
}