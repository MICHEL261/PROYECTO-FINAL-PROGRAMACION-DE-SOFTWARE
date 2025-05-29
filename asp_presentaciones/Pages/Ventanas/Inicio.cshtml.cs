using System;
using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_presentaciones.Pages.Ventanas
{
    public class InicioModel : PageModel
    {
        private IDiscosPresentacion? iPresentacion = null;
        [BindProperty] public List<Discos>? Lista { get; set; }
        [BindProperty] public Enumerables.Ventanas Accion { get; set; }

        public InicioModel(IDiscosPresentacion iPresentacion)
        {
            try
            {
                this.iPresentacion = iPresentacion;



            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }
        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("NombreUsuario") == null)
                return RedirectToPage("Login");

            Accion = Enumerables.Ventanas.Listas;
            var task =  this.iPresentacion!.Listar();
            task.Wait();
            Lista = task.Result;
            return Page();
        }


        public IActionResult OnPostBtClose()
        {
        
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }
    }
}
