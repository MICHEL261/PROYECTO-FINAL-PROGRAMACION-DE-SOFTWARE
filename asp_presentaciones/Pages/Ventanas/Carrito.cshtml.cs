using ClosedXML.Excel;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Implementaciones;
using lib_presentaciones.Interfaces;
using lib_presentaciones.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace asp_presentaciones.Pages.Ventanas
{
    public class CarritoModel : PageModel
    {
        private const string SessionKeyCarrito = "_Carrito";
        public decimal Total => Lista?.Sum(i => i.Cantidad * i.ValorUnitario) ?? 0;

        private readonly ICarritoPresentacion? _carritoService;
        private readonly IDiscosPresentacion? IDiscosPresentacion;
        private readonly IPagosPresentacion? IPagosPresentacion;
        private readonly IFormatosPresentacion? IFormatosPresentacion;
        private readonly IPreciosDiscosPresentacion? IPreciosDiscosPresentacion;



        [BindProperty] public Enumerables.Ventanas Accion { get; set; }
        [BindProperty] public OrdenesDiscos? Actual { get; set; }
        [BindProperty] public Carrito? Filtro { get; set; }
        [BindProperty] public List<Carrito> Lista { get; set; } = new List<Carrito>();
        [BindProperty] public List<Discos> Discos { get; set; }

        [BindProperty] public List<Formatos> Formatos { get; set; }
        [BindProperty] public List<PreciosDiscos> PreciosDiscos { get; set; }
        [BindProperty] public List<Pagos> Pagos { get; set; } = new List<Pagos>();


        [BindProperty]
        public Carrito NuevoItem { get; set; } = new Carrito();
        public bool ItemAgregado { get; set; } = false;


        public CarritoModel(ICarritoPresentacion carritoService, IDiscosPresentacion IDiscosPresentacion, IFormatosPresentacion IFormatosPresentacion, IPreciosDiscosPresentacion IPreciosDiscosPresentacion, IPagosPresentacion IPagosPresentacion)
        {
            this._carritoService = carritoService;
            this.IDiscosPresentacion = IDiscosPresentacion;
            this.IFormatosPresentacion = IFormatosPresentacion;
            this.IPreciosDiscosPresentacion = IPreciosDiscosPresentacion;
            this.IPagosPresentacion = IPagosPresentacion;
        }

        public async Task OnGet(string? disco)
        {
            Lista = ObtenerCarritoSesion();

            Discos = await IDiscosPresentacion!.Listar() ?? new List<Discos>();
            Formatos = await IFormatosPresentacion!.Listar() ?? new List<Formatos>();
            PreciosDiscos = await IPreciosDiscosPresentacion!.Listar() ?? new List<PreciosDiscos>();
            Pagos = await IPagosPresentacion!.Listar() ?? new List<Pagos>();
            if (!string.IsNullOrEmpty(disco))
            {
                NuevoItem = new Carrito();
                var discoEncontrado = Discos.FirstOrDefault(x => x.NombreDisco == disco);
                var discoId = Discos.FirstOrDefault(x => x.Id == discoEncontrado!.Id);

                if (discoEncontrado != null)
                {

                    NuevoItem.Disco = discoEncontrado.NombreDisco;
                    NuevoItem.Formato = 1;
                    NuevoItem.Cantidad = 1;
                    var precioDisco = PreciosDiscos.FirstOrDefault(p => p.Disco == discoId!.Id && p.Formato == NuevoItem.Formato)?.Precio ?? 0;
                    NuevoItem.ValorUnitario = precioDisco;
                }
            }
            else
            {
                ItemAgregado = true;
            }
        }

        public IActionResult OnPostAgregar()
        {
            Lista = ObtenerCarritoSesion();

            if (!ModelState.IsValid)
            {
                Discos = IDiscosPresentacion!.Listar().Result ?? new List<Discos>();
                Formatos = IFormatosPresentacion!.Listar().Result ?? new List<Formatos>();
                Pagos = IPagosPresentacion!.Listar().Result ?? new List<Pagos>();
                ItemAgregado = false;
                return Page();
            }

            AgregarAlCarrito(NuevoItem);
            GuardarCarritoSesion(Lista);

            ItemAgregado = true;

            Discos = IDiscosPresentacion!.Listar().Result ?? new List<Discos>();
            Formatos = IFormatosPresentacion!.Listar().Result ?? new List<Formatos>();
            Pagos = IPagosPresentacion!.Listar().Result ?? new List<Pagos>();

            NuevoItem = new Carrito();

            return Page();
        }
        public IActionResult OnGetIrAlCatalogo()
        {
            return RedirectToPage("/Ventanas/Inicio");

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

                return RedirectToPage("/Ventanas/Inicio");
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

        public async Task<IActionResult> OnPostBtExportarPDFAsync()
        {
            QuestPDF.Settings.License = LicenseType.Community;


            Lista = ObtenerCarritoSesion();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);

                    page.Header().Text("Listado de Discos").FontSize(20).Bold().AlignCenter();

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                        });


                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Disco");
                            header.Cell().Element(CellStyle).Text("Formato");
                            header.Cell().Element(CellStyle).Text("Cantidad");
                            header.Cell().Element(CellStyle).Text("Valor unitario");
                            header.Cell().Element(CellStyle).Text("Total");

                            static IContainer CellStyle(IContainer container) =>
                                container.DefaultTextStyle(x => x.SemiBold()).Padding(5).Background("#eee");
                        });


                        foreach (var d in Lista)
                        {
                            table.Cell().Element(CellStyle).Text(d.Disco ?? "");
                            table.Cell().Element(CellStyle).Text(d.Formato.ToString());
                            table.Cell().Element(CellStyle).Text(d.Cantidad.ToString());
                            table.Cell().Element(CellStyle).Text(d.ValorUnitario.ToString("C"));
                            table.Cell().Element(CellStyle).Text((d.Cantidad * d.ValorUnitario).ToString("C"));

                            static IContainer CellStyle(IContainer container) =>
                                container.Padding(5);
                        }
                    });
                });
            });

            using var stream = new MemoryStream();
            document.GeneratePdf(stream);
            stream.Position = 0;

            return File(stream.ToArray(), "application/pdf", "Carrito.pdf");
        }
        public async Task<IActionResult> OnPostBtExportarExcelAsync()
        {
            Lista = ObtenerCarritoSesion();
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Carrito");


            worksheet.Cell(1, 1).Value = "Disco";
            worksheet.Cell(1, 2).Value = "Formato";
            worksheet.Cell(1, 3).Value = "Cantidad";
            worksheet.Cell(1, 4).Value = "Valor Unitario";
            worksheet.Cell(1, 5).Value = "Subtotal";

            int row = 2;
            foreach (var item in Lista)
            {
                worksheet.Cell(row, 1).Value = item.Disco ?? "";
                worksheet.Cell(row, 2).Value = item.Formato.ToString();
                worksheet.Cell(row, 3).Value = item.Cantidad;
                worksheet.Cell(row, 4).Value = item.ValorUnitario;
                worksheet.Cell(row, 5).Value = item.Cantidad * item.ValorUnitario;

                row++;
            }



            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Listado_Carrito.xlsx");
        }
    }
}