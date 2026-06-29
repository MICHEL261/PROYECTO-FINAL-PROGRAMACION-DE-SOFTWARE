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


namespace asp_presentacion.Pages.Ventanas
{
    public class DetallesFormatosModel : PageModel
    {
        private IFormatosPresentacion? iPresentacion = null;

        public DetallesFormatosModel(IFormatosPresentacion iPresentacion)
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

        public IFormFile? FormFile { get; set; }
        [BindProperty] public Enumerables.Ventanas Accion { get; set; }
        [BindProperty] public Formatos? Actual { get; set; }
        [BindProperty] public Formatos? Filtro { get; set; }
        [BindProperty] public List<Formatos>? Lista { get; set; }

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

                Accion = Enumerables.Ventanas.Listas;
                var task = this.iPresentacion!.Listar();
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
            var listaFormatos = Lista ?? new List<Formatos>();
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);

                    page.Header().Text("Listado de Formatos").FontSize(20).Bold().AlignCenter();
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
                            header.Cell().Element(CellStyle).Text("Tipo formato");
                            header.Cell().Element(CellStyle).Text("Material");


                            static IContainer CellStyle(IContainer container) =>
                                container.DefaultTextStyle(x => x.SemiBold()).Padding(5).Background("#eee");
                        });

                        // Filas
                        foreach (var d in listaFormatos)
                        {
                            table.Cell().Element(CellStyle).Text(d.TipoFormato ?? "");
                            table.Cell().Element(CellStyle).Text(d.Material ?? "");


                            static IContainer CellStyle(IContainer container) =>
                                container.Padding(5);
                        }
                    });
                });
            });

            using var stream = new MemoryStream();
            document.GeneratePdf(stream);
            stream.Position = 0;

            return File(stream.ToArray(), "application/pdf", "Listado_Formatos.pdf");
        }

        public async Task<IActionResult> OnPostBtExportarExcelAsync()
        {

            OnPostBtRefrescar();
            var listaFormatos = Lista ?? new List<Formatos>();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Formatos");


            worksheet.Cell(1, 1).Value = "Tipo formato";
            worksheet.Cell(1, 2).Value = "Material";



            int row = 2;
            foreach (var disco in listaFormatos)
            {
                worksheet.Cell(row, 1).Value = disco.TipoFormato ?? "";
                worksheet.Cell(row, 2).Value = disco.Material ?? "";

                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Listado_Formatos.xlsx");
        }



    }
}