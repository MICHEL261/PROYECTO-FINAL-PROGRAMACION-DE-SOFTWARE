using lib_aplicaciones.Interfaces;
using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Implementaciones;
using lib_presentaciones.Interfaces;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_presentaciones.Pages
{
    public class IndexModel : PageModel
    {
        

       
        

        public bool EstaLogueado { get; set; }


        [BindProperty] public string? Email { get; set; }
        [BindProperty] public string? Contrasena { get; set; }

        public bool Edita { get; set; } = false;
        public bool Nuevo { get; set; } = false;
        public bool Borra { get; set; } = false;
        public async Task OnGet()
        {
            var variable_session = HttpContext.Session.GetString("NombreUsuario");
            if (!string.IsNullOrEmpty(variable_session))
            {
                var presentacion = new UsuariosPresentacion();
                var resultado = await presentacion.PorNombre(new Usuarios { NombreUsuario = variable_session });

                var UsuarioCompleto = resultado.FirstOrDefault();
                EstaLogueado = true;
                if (UsuarioCompleto.Rol == 1) 
                {
                    Edita = true;
                    Nuevo = true;
                    Borra = true;
                    HttpContext.Session.SetString("PermisoEdita", "true");
                    HttpContext.Session.SetString("PermisoNuevo", "true");
                    HttpContext.Session.SetString("PermisoBorra", "true");
                }
                else
                {
                   
                    if (UsuarioCompleto.Rol == 2)
                    {
                        Edita = true;
                        Nuevo = false;
                        Borra = false;
                    }
                    
                }
                return;
            }
        }

        public void OnPostBtClean()
        {
            try
            {
                Email = string.Empty;
                Contrasena = string.Empty;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public async Task OnPostBtEnter()
        {
            try
            {
                if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Contrasena))
                {
                    OnPostBtClean();
                    return;
                }

                var usuariosPresentacion = new UsuariosPresentacion();
                var usuarios = await usuariosPresentacion.Listar(); 

                bool loginExitoso = usuarios.Any(usuario =>
                    usuario.NombreUsuario == Email && usuario.Contrase�a == Contrasena);

                if (loginExitoso)
                {
                    ViewData["Logged"] = true;
                    HttpContext.Session.SetString("Usuario", Email!);
                    EstaLogueado = true;
                }
                else
                {
                    ViewData["Error"] = "Usuario o contrase�a incorrectos.";
                }

                OnPostBtClean();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }
        public void OnPostBtClose()
        {
            try
            {
                HttpContext.Session.Clear();
                HttpContext.Response.Redirect("/");
                EstaLogueado = false;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }
    }
}

