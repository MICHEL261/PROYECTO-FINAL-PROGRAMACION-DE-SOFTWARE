using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Implementaciones;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_presentaciones.Pages.Ventanas
{
    public class ClientesModel : PageModel
    {
        private IClientesPresentacion? iPresentacion = null;
        private IUsuariosPresentacion? IUsuarioPresentacion = null;

        public ClientesModel(IClientesPresentacion iPresentacion, IUsuariosPresentacion IUsuarioPresentacion)
        {
            try
            {
                this.iPresentacion = iPresentacion;
                this.IUsuarioPresentacion = IUsuarioPresentacion;
                Filtro = new Clientes();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public IFormFile? FormFile { get; set; }
        [BindProperty] public Enumerables.Ventanas Accion { get; set; }
        [BindProperty] public Clientes? Actual { get; set; }
        [BindProperty] public Clientes? Filtro { get; set; }
        [BindProperty] public List<Clientes>? Lista { get; set; }
        [BindProperty] public List<Usuarios>? Usuarios { get; set; }


        public virtual IActionResult OnGet()
        {
            Usuarios = new UsuariosPresentacion().Listar().Result;

            if (ValidarPermisoRol())
            {
                Accion = lib_dominio.Nucleo.Enumerables.Ventanas.Editar;
                Actual = new Clientes();
            }
            else
            {
                Accion = lib_dominio.Nucleo.Enumerables.Ventanas.Listas;
                Lista = new ClientesPresentacion().Listar().Result;
            }

            return Page();
        }

        public void OnPostBtRefrescar()
        {
            try
            {
                var variable_session = HttpContext.Session.GetString("NombreUsuario");
                if (!ValidarPermiso())
                {
                   
                    TempData["MensajeError"] = "No tienes permisos para Listar.";
                    return;
                }
                if (String.IsNullOrEmpty(variable_session))
                {
                    HttpContext.Response.Redirect("/");
                    return;
                }

                Filtro!.NombreCliente = Filtro!.NombreCliente ?? "";

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
                var task = this.IUsuarioPresentacion!.Listar();
                task.Wait();
                Usuarios = task.Result;
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
                Actual = new Clientes();
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
                if (!ValidarPermiso())
                {
                    
                    TempData["MensajeError"] = "No tienes permisos para Modificar.";
                    return;
                }
                Accion = Enumerables.Ventanas.Editar;
                Actual = Lista!.FirstOrDefault(x => x.Id.ToString() == data);
                CargarCombox();
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
                

                Task<Clientes>? task = null;
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



                ViewData["MensajeError"] = ex.Message.ToString() + "Debe borrar primero las relaciones que tiene el Cliente con otras entidades";

                OnPostBtRefrescar();
            }
        }

        public IActionResult OnPostBtCancelar()
        {
            if (ValidarPermisoRol())
            {
                Accion = lib_dominio.Nucleo.Enumerables.Ventanas.Editar;
                Actual = new Clientes(); 
            }
            else
            {
                Accion = lib_dominio.Nucleo.Enumerables.Ventanas.Listas;
                Lista = new ClientesPresentacion().Listar().Result;
            }

            Usuarios = new UsuariosPresentacion().Listar().Result;

            return Page();
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
                   Nuevo= true;
                    break;
                default:
                    return false;
            }


            return Listar;
        }

        public bool ValidarPermisoRol()
        {
            var variable_session = HttpContext.Session.GetString("NombreUsuario");

            if (string.IsNullOrEmpty(variable_session))
                return false;

            var UsuarioPresentacion = new UsuariosPresentacion();
            var usuarios = UsuarioPresentacion.Listar().Result;

            if (usuarios == null)
                return false;

            var usuario = usuarios.FirstOrDefault(x => x.NombreUsuario.ToLower() == variable_session.ToLower());

            if (usuario == null)
                return false;


            bool Nuevo = false;
           

            switch (usuario.Rol)
            {
                case 1:
                   
                    break;
                case 2:
                    Nuevo = true;
                    break;
                default:
                    return false;
            }


            return Nuevo;
        }
    }
}