using lib_dominio.Nucleo;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_presentaciones.Pages
{
    public class LogoutModel : PageModel
    {
        public void OnPostBtRefrescar()
        {
            try
            {
                var variable_session = HttpContext.Session.GetString("NombreUsuario");
                
                if (String.IsNullOrEmpty(variable_session))
                {
                    HttpContext.Response.Redirect("/Index");
                    return;
                }

              
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }
        public IActionResult OnGet()
        {
            HttpContext.Session.Clear();
            OnPostBtRefrescar();
            return RedirectToPage("/Index");
        }
    }
}
