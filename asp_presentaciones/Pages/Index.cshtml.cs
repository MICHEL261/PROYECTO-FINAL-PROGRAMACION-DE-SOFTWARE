using System;
using System.Linq;
using System.Threading.Tasks;
using lib_presentaciones.Implementaciones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_presentaciones.Pages
{
    public class IndexModel : PageModel
    {
        public bool EstaLogueado { get; set; }

        [BindProperty] public string? Email { get; set; }
        [BindProperty] public string? Contrasena { get; set; }
        public bool UsuarioRegistrado { get; set; }
        public void OnGet()
        {
            OnPostBtClose();
        }


        public void OnPostBtClean()
        {
            Email = string.Empty;
            Contrasena = string.Empty;
        }

        public async Task<IActionResult> OnPostBtEnter()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Contrasena))
            {
                OnPostBtClean();
                return Page(); 
            }

            var usuarios = await new UsuariosPresentacion().Listar();
            var usuarioLogueado = usuarios.FirstOrDefault(u =>
                u.NombreUsuario == Email && u.Contraseña == Contrasena);

            if (usuarioLogueado != null)
            {
                HttpContext.Session.SetString("NombreUsuario", usuarioLogueado.NombreUsuario!);
                HttpContext.Session.SetString("Rol", usuarioLogueado._Rol?.NombreRol ?? "");

                return RedirectToPage("/Ventanas/Inicio");
            }
            else
            {
                ViewData["Error"] = "Usuario o contraseña incorrectos.";
            }

            OnPostBtClean();
            return Page(); 
        }


        public IActionResult OnPostBtClose()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }

        
    }
}
