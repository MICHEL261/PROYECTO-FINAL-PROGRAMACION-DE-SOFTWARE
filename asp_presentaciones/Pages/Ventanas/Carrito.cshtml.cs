using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Interfaces;
using lib_presentaciones.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace asp_presentaciones.Pages.Ventanas
{
    public class CarritoModel : PageModel
    {
        private const string SessionKeyCarrito = "_Carrito";
        public decimal Total => Lista?.Sum(i => i.Cantidad * i.ValorUnitario) ?? 0;

        private readonly ICarritoPresentacion? _carritoService;
        private readonly IDiscosPresentacion? IDiscosPresentacion;


        [BindProperty] public Enumerables.Ventanas Accion { get; set; }
        [BindProperty] public OrdenesDiscos? Actual { get; set; }
        [BindProperty] public Carrito? Filtro { get; set; }
        [BindProperty] public List<Carrito> Lista { get; set; } = new List<Carrito>();
        [BindProperty] public List<Discos> Discos { get; set; } 


        [BindProperty]
        public Carrito NuevoItem { get; set; } = new Carrito();

        public CarritoModel(ICarritoPresentacion carritoService, IDiscosPresentacion IDiscosPresentacion)
        {
            this._carritoService = carritoService;
            this.IDiscosPresentacion = IDiscosPresentacion;
        }

        public void OnGet(string? disco)
        {
            Lista = ObtenerCarritoSesion();

            var task = this.IDiscosPresentacion!.Listar();
            task.Wait();
            Discos = task.Result;

            if (!string.IsNullOrEmpty(disco))
            {
                var discoEncontrado = Discos.FirstOrDefault(x => x.NombreDisco == disco);

                if (discoEncontrado != null)
                {
                    NuevoItem = new Carrito
                    {
                        Disco = discoEncontrado.NombreDisco,
                        Formato = 0,
                        Cantidad = 1,
                        ValorUnitario = 0
                    };
                }
            }
        }

        public IActionResult OnPostAgregar()
        {
            Lista = ObtenerCarritoSesion();

            if (ModelState.IsValid)
            {
                AgregarAlCarrito(NuevoItem);
                GuardarCarritoSesion(Lista);
                NuevoItem = new Carrito(); 
            }

            return RedirectToPage();
        }

        public IActionResult OnPostEliminar(string discoNom, int formatoId)
        {
            Lista = ObtenerCarritoSesion();
            Lista.RemoveAll(i => i.Disco == discoNom && i.Formato == formatoId);
            GuardarCarritoSesion(Lista);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostFinalizar(string accion, int clienteId, List<string> discosNom, int pagoId)
        {
            Lista = ObtenerCarritoSesion();
            if (accion == "Cancelar")
            {
                Lista.Clear();
                GuardarCarritoSesion(Lista);
                return RedirectToPage("/Ventanas/Inicio");
            }

            if (accion == "Confirmar")
            {
                try
                {
                    await _carritoService!.FinalizarCompra(clienteId, Lista, pagoId);


                    Lista.Clear();
                    GuardarCarritoSesion(Lista);
                    TempData["MensajeExito"] = "Compra realizada exitosamente.";
                }
                catch (Exception ex)
                {
                    TempData["MensajeError"] = ex.Message;
                }

                return RedirectToPage();
            }

            return Page();
        }



        private List<Carrito> ObtenerCarritoSesion()
        {
            var json = HttpContext.Session.GetString(SessionKeyCarrito);
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<List<Carrito>>(json) ?? new List<Carrito>()
                : new List<Carrito>();
        }

        private void GuardarCarritoSesion(List<Carrito> carrito)
        {
            var json = JsonConvert.SerializeObject(carrito);
            HttpContext.Session.SetString(SessionKeyCarrito, json);
        }

        private void AgregarAlCarrito(Carrito nuevo)
        {
            var existente = Lista.FirstOrDefault(i => i.Disco == nuevo.Disco && i.Formato == nuevo.Formato);
            if (existente != null)
            {
                existente.Cantidad += nuevo.Cantidad;
            }
            else
            {
                Lista.Add(nuevo);
            }
        }
    }
}