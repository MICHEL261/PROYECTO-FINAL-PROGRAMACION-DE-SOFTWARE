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
        

        // Inyección de la dependencia
        

        public bool EstaLogueado { get; set; }


        [BindProperty] public string? Email { get; set; }
        [BindProperty] public string? Contrasena { get; set; }


        public void OnGet()
        {
            var variable_session = HttpContext.Session.GetString("Usuario");
            if (!string.IsNullOrEmpty(variable_session))
            {
                EstaLogueado = true;
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

        public void OnPostBtEnter()
        {

            try
            {
                if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Contrasena))
                {
                    OnPostBtClean();
                    return;
                }

               
                var usuariosPresentacion = new UsuariosPresentacion();
                var usuarios = usuariosPresentacion.Listar().Result; 

                bool loginExitoso = false;

                foreach (var usuario in usuarios)
                {
                    if (usuario.NombreUsuario == Email && usuario.Contraseña == Contrasena)
                    {
                        loginExitoso = true;
                        break;
                    }
                }

                if (loginExitoso)
                {
                    ViewData["Logged"] = true;
                    HttpContext.Session.SetString("Usuario", Email!);
                    EstaLogueado = true;
                }
                else
                {
                    ViewData["Error"] = "Usuario o contraseña incorrectos.";
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

