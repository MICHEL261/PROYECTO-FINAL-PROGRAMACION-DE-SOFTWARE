using ClosedXML.Excel;
using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Implementaciones;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;

namespace asp_presentaciones.Pages.Ventanas
{
    public class DetallesDiscosModel : PageModel
    {
        private IDiscosPresentacion? iPresentacion = null;
        private IArtistasPresentacion? IArtistasPresentacion = null;
        private IMarcasPresentacion? IMarcasPresentacion = null;



        public DetallesDiscosModel(IDiscosPresentacion iPresentacion, IArtistasPresentacion IArtistasPresentacion, IMarcasPresentacion IMarcasPresentacion)
        {
            try
            {
                this.iPresentacion = iPresentacion;
                this.IArtistasPresentacion = IArtistasPresentacion;
                this.IMarcasPresentacion = IMarcasPresentacion;
                Filtro = new Discos();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public IFormFile? FormFile { get; set; }
        [BindProperty] public Enumerables.Ventanas Accion { get; set; }
        [BindProperty] public Discos? Actual { get; set; }
        [BindProperty] public Discos? Filtro { get; set; }
        [BindProperty] public List<Discos>? Lista { get; set; }
        [BindProperty] public List<Artistas>? Artistas { get; set; }
        [BindProperty] public List<Marcas>? Marcas { get; set; }


        public virtual void OnGet() { OnPostBtRefrescar(); }

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

                Filtro!.NombreDisco = Filtro!.NombreDisco ?? "";

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


  

        public async Task<IActionResult> OnPostBtExportarPDFAsync()
        {
            QuestPDF.Settings.License = LicenseType.Community;
            OnPostBtRefrescar();
            var listaDiscos = Lista ?? new List<Discos>();
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
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(3);
                        });

                        // Encabezados
                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Artista");
                            header.Cell().Element(CellStyle).Text("Marca");
                            header.Cell().Element(CellStyle).Text("Nombre");
                            header.Cell().Element(CellStyle).Text("Duración");
                            header.Cell().Element(CellStyle).Text("Fecha");

                            static IContainer CellStyle(IContainer container) =>
                                container.DefaultTextStyle(x => x.SemiBold()).Padding(5).Background("#eee");
                        });

                        // Filas
                        foreach (var d in listaDiscos)
                        {
                            table.Cell().Element(CellStyle).Text(d._Artista?.NombreArtista ?? "");
                            table.Cell().Element(CellStyle).Text(d._Marca?.NombreMarca ?? "");
                            table.Cell().Element(CellStyle).Text(d.NombreDisco ?? "");
                            table.Cell().Element(CellStyle).Text(d.DuracionDisco ?? "");
                            table.Cell().Element(CellStyle).Text(d.FechaLanzamiento.ToString("yyyy-MM-dd") ?? "");

                            static IContainer CellStyle(IContainer container) =>
                                container.Padding(5);
                        }
                    });
                });
            });

            using var stream = new MemoryStream();
            document.GeneratePdf(stream);
            stream.Position = 0;

            return File(stream.ToArray(), "application/pdf", "Listado_Discos.pdf");
        }

        public async Task<IActionResult> OnPostBtExportarExcelAsync()
        {

            OnPostBtRefrescar();
            var listaDiscos = Lista ?? new List<Discos>();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Discos");


            worksheet.Cell(1, 1).Value = "Artista";
            worksheet.Cell(1, 2).Value = "Marca";
            worksheet.Cell(1, 3).Value = "Nombre";
            worksheet.Cell(1, 4).Value = "Duración";
            worksheet.Cell(1, 5).Value = "Fecha de Lanzamiento";


            int row = 2;
            foreach (var disco in listaDiscos)
            {
                worksheet.Cell(row, 1).Value = disco._Artista?.NombreArtista ?? "";
                worksheet.Cell(row, 2).Value = disco._Marca?.NombreMarca ?? "";
                worksheet.Cell(row, 3).Value = disco.NombreDisco ?? "";
                worksheet.Cell(row, 4).Value = disco.DuracionDisco ?? "";
                worksheet.Cell(row, 5).Value = disco.FechaLanzamiento.ToString("yyyy-MM-dd") ?? "";
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Listado_Discos.xlsx");
        }
    }
}