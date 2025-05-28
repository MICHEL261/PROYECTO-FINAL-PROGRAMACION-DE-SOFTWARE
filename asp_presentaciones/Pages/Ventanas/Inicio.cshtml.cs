using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_presentaciones.Pages.Ventanas
{
    public class InicioModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("NombreUsuario") == null)
            {
                return RedirectToPage("/Index"); 
            }

            return Page();
        }

        public IActionResult OnPostBtClose()
        {
        
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }
    }
}
