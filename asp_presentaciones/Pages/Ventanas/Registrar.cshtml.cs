using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Implementaciones;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace asp_presentaciones.Pages.Ventanas
{
    public class RegistrarModel : PageModel
    {
        private IRolesPresentacion? iPresentacion = null;
        private IUsuariosPresentacion? IUsuariosPresentacion = null;

        public RegistrarModel(IRolesPresentacion iPresentacion, IUsuariosPresentacion IUsuariosPresentacion)
        {
            try
            {
                this.iPresentacion = iPresentacion;
                this.IUsuariosPresentacion = IUsuariosPresentacion;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }
        [BindProperty] public Usuarios? Usuario { get; set; }

        [BindProperty] public List<Roles> Roles { get; set; }

        public async Task OnGetAsync()
        {
            var todosLosRoles = await this.iPresentacion!.Listar();
            Roles = todosLosRoles
                .Where(r => r.NombreRol == "Cliente") // Solo el rol Cliente
                .ToList();
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();
            this.IUsuariosPresentacion!.Guardar(Usuario!);

            return RedirectToPage("/Index");
        }

    }
}