using MaterialEscolarReyes.Data;
using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using MaterialEscolarReyes.Models;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;

namespace MaterialEscolarReyes.Controllers
{
    public class ReportesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportesController(ApplicationDbContext context)
        {
            _context = context;

            QuestPDF.Settings.License = LicenseType.Community;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ProductosExcel()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .Include(p => p.Proveedor)
                .ToListAsync();

            using var libro = new XLWorkbook();

            var hoja = libro.Worksheets.Add("Productos");

            hoja.Cell(1, 1).Value = "Código";
            hoja.Cell(1, 2).Value = "Producto";
            hoja.Cell(1, 3).Value = "Categoría";
            hoja.Cell(1, 4).Value = "Marca";
            hoja.Cell(1, 5).Value = "Proveedor";
            hoja.Cell(1, 6).Value = "Precio Compra";
            hoja.Cell(1, 7).Value = "Precio Venta";
            hoja.Cell(1, 8).Value = "Stock";

            int fila = 2;

            foreach (var p in productos)
            {
                hoja.Cell(fila, 1).Value = p.Codigo;
                hoja.Cell(fila, 2).Value = p.Nombre;
                hoja.Cell(fila, 3).Value = p.Categoria?.Nombre;
                hoja.Cell(fila, 4).Value = p.Marca?.Nombre;
                hoja.Cell(fila, 5).Value = p.Proveedor?.Nombre;
                hoja.Cell(fila, 6).Value = p.PrecioCompra;
                hoja.Cell(fila, 7).Value = p.PrecioVenta;
                hoja.Cell(fila, 8).Value = p.Stock;

                fila++;
            }

            hoja.Columns().AdjustToContents();

            using var stream = new MemoryStream();

            libro.SaveAs(stream);

            stream.Position = 0;

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Productos_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
        }
        public async Task<IActionResult> ClientesExcel()
        {
            var clientes = await _context.Clientes.ToListAsync();

            using var libro = new XLWorkbook();

            var hoja = libro.Worksheets.Add("Clientes");

            hoja.Cell(1, 1).Value = "Nombre";
            hoja.Cell(1, 2).Value = "Apellido";
            hoja.Cell(1, 3).Value = "Correo";
            hoja.Cell(1, 4).Value = "Teléfono";
            hoja.Cell(1, 5).Value = "Dirección";

            int fila = 2;

            foreach (var c in clientes)
            {
                hoja.Cell(fila, 1).Value = c.Nombre;
                hoja.Cell(fila, 2).Value = c.Apellido;
                hoja.Cell(fila, 3).Value = c.Correo;
                hoja.Cell(fila, 4).Value = c.Telefono;
                hoja.Cell(fila, 5).Value = c.Direccion;
                fila++;
            }

            hoja.Columns().AdjustToContents();

            using var stream = new MemoryStream();

            libro.SaveAs(stream);

            stream.Position = 0;

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "ReporteClientes.xlsx");
        }
        public async Task<IActionResult> ProveedoresExcel()
        {
            var proveedores = await _context.Proveedores.ToListAsync();

            using var libro = new XLWorkbook();

            var hoja = libro.Worksheets.Add("Proveedores");

            hoja.Cell(1, 1).Value = "Nombre";
            hoja.Cell(1, 2).Value = "Correo";
            hoja.Cell(1, 3).Value = "Teléfono";
            hoja.Cell(1, 4).Value = "Dirección";

            int fila = 2;

            foreach (var p in proveedores)
            {
                hoja.Cell(fila, 1).Value = p.Nombre;
                hoja.Cell(fila, 2).Value = p.Correo;
                hoja.Cell(fila, 3).Value = p.Telefono;
                hoja.Cell(fila, 4).Value = p.Direccion;
                fila++;
            }

            hoja.Columns().AdjustToContents();

            using var stream = new MemoryStream();

            libro.SaveAs(stream);

            stream.Position = 0;

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "ReporteProveedores.xlsx");
        }
        public async Task<IActionResult> VentasExcel()
        {
            var ventas = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .ToListAsync();

            using var libro = new XLWorkbook();

            var hoja = libro.Worksheets.Add("Ventas");

            hoja.Cell(1, 1).Value = "N° Venta";
            hoja.Cell(1, 2).Value = "Fecha";
            hoja.Cell(1, 3).Value = "Cliente";
            hoja.Cell(1, 4).Value = "Usuario";
            hoja.Cell(1, 5).Value = "Total";

            int fila = 2;

            foreach (var v in ventas)
            {
                hoja.Cell(fila, 1).Value = v.NumeroVenta;
                hoja.Cell(fila, 2).Value = v.Fecha.ToString("dd/MM/yyyy");
                hoja.Cell(fila, 3).Value = v.Cliente?.Nombre;
                hoja.Cell(fila, 4).Value = v.Usuario?.Nombre;
                hoja.Cell(fila, 5).Value = v.Total;
                fila++;
            }

            hoja.Columns().AdjustToContents();

            using var stream = new MemoryStream();

            libro.SaveAs(stream);

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "ReporteVentas.xlsx");
        }
        public async Task<IActionResult> StockExcel()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .ToListAsync();

            using var libro = new XLWorkbook();

            var hoja = libro.Worksheets.Add("Stock");

            hoja.Cell(1, 1).Value = "Código";
            hoja.Cell(1, 2).Value = "Producto";
            hoja.Cell(1, 3).Value = "Categoría";
            hoja.Cell(1, 4).Value = "Stock";
            hoja.Cell(1, 5).Value = "Stock Mínimo";

            int fila = 2;

            foreach (var p in productos)
            {
                hoja.Cell(fila, 1).Value = p.Codigo;
                hoja.Cell(fila, 2).Value = p.Nombre;
                hoja.Cell(fila, 3).Value = p.Categoria?.Nombre;
                hoja.Cell(fila, 4).Value = p.Stock;
                hoja.Cell(fila, 5).Value = p.StockMinimo;
                fila++;
            }

            hoja.Columns().AdjustToContents();

            using var stream = new MemoryStream();

            libro.SaveAs(stream);

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "ReporteStock.xlsx");
        }
        public async Task<IActionResult> ProductosPdf()
        {
            var productos = await _context.Productos
                .Include(x => x.Categoria)
                .Include(x => x.Marca)
                .Include(x => x.Proveedor)
                .ToListAsync();

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header().Column(col =>
                    {
                        col.Item()
                       .AlignCenter()
                            .Height(70)
                            .Image(
                                System.IO.File.ReadAllBytes(
                                    Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot",
                                        "images",
                                        "logo.png"
                                    )),
                                ImageScaling.FitHeight);

                        col.Item()
                            .AlignCenter()
                            .Text("MATERIAL ESCOLAR REYES")
                            .FontSize(22)
                            .Bold();

                        col.Item()
                            .AlignCenter()
                            .Text("Sistema de Inventarios y Ventas")
                            .FontSize(12);

                        col.Item()
                            .AlignCenter()
                            .Text("REPORTE DE PRODUCTOS")
                            .FontSize(18)
                            .Bold();

                        col.Item().PaddingBottom(15);
                    });
                    page.Content().Table(tabla =>
                    {
                        tabla.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        tabla.Header(header =>
                        {
                            header.Cell().Background(Colors.Blue.Medium).Padding(5)
                                .Text("Código").FontColor(Colors.White).Bold();

                            header.Cell().Background(Colors.Blue.Medium).Padding(5)
                                .Text("Producto").FontColor(Colors.White).Bold();

                            header.Cell().Background(Colors.Blue.Medium).Padding(5)
                                .Text("Categoría").FontColor(Colors.White).Bold();

                            header.Cell().Background(Colors.Blue.Medium).Padding(5)
                                .Text("Precio").FontColor(Colors.White).Bold();

                            header.Cell().Background(Colors.Blue.Medium).Padding(5)
                                .Text("Stock").FontColor(Colors.White).Bold();
                        });

                        foreach (var p in productos)
                        {
                            tabla.Cell().Padding(4).Text(p.Codigo);

                            tabla.Cell().Padding(4).Text(p.Nombre);

                            tabla.Cell().Padding(4).Text(p.Categoria?.Nombre ?? "");

                            tabla.Cell().Padding(4).Text($"Bs {p.PrecioVenta:N2}");

                            tabla.Cell().Padding(4).Text(p.Stock.ToString());
                        }

                    });

                    page.Footer().Row(row =>
                    {
                        row.RelativeItem()
                            .AlignLeft()
                            .Text("Material Escolar Reyes");

                        row.RelativeItem()
                            .AlignCenter()
                            .Text($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}");

                        row.RelativeItem()
                            .AlignRight()
                            .Text(x =>
                            {
                                x.CurrentPageNumber();
                                x.Span(" / ");
                                x.TotalPages();
                            });
                    });
                });

            });

            var bytes = pdf.GeneratePdf();

            return File(bytes,
                "application/pdf",
                "ReporteProductos.pdf");
        }
        public async Task<IActionResult> ClientesPdf()
        {
            var clientes = await _context.Clientes.ToListAsync();

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);

                    page.Header().Column(header =>
                    {
                        header.Item().Text("📚 MATERIAL ESCOLAR REYES")
                            .FontSize(24)
                            .Bold()
                            .FontColor(Colors.Blue.Darken3);

                        header.Item().Text("Sistema de Inventario y Ventas")
                            .FontSize(12)
                            .FontColor(Colors.Grey.Darken2);

                        header.Item().PaddingVertical(8);

                        header.Item().LineHorizontal(2)
                            .LineColor(Colors.Blue.Darken2);

                        header.Item().PaddingTop(10);

                        header.Item().Text("REPORTE DE CLIENTES")
                            .FontSize(20)
                            .Bold();

                        header.Item().Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken1);

                        header.Item().PaddingBottom(15);
                    });

                    page.Content().Table(tabla =>
                    {
                        tabla.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(2);
                            c.RelativeColumn(2);
                            c.RelativeColumn();
                            c.RelativeColumn(2);
                        });

                        tabla.Header(h =>
                        {
                            h.Cell().Background(Colors.Blue.Darken2).Padding(8)
                                .Text("Cliente").Bold().FontColor(Colors.White);

                            h.Cell().Background(Colors.Blue.Darken2).Padding(8)
                                .Text("Correo").Bold().FontColor(Colors.White);

                            h.Cell().Background(Colors.Blue.Darken2).Padding(8)
                                .Text("Teléfono").Bold().FontColor(Colors.White);

                            h.Cell().Background(Colors.Blue.Darken2).Padding(8)
                                .Text("Dirección").Bold().FontColor(Colors.White);
                        });

                        foreach (var c in clientes)
                        {
                            tabla.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(6)
                                .Text($"{c.Nombre} {c.Apellido}");

                            tabla.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(6)
                                .Text(c.Correo);

                            tabla.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(6)
                                .Text(c.Telefono ?? "-");

                            tabla.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(6)
                                .Text(c.Direccion ?? "-");
                        }
                    });

                    page.Footer().Column(footer =>
                    {
                        footer.Item().LineHorizontal(1);

                        footer.Item().PaddingTop(10);

                        footer.Item().Row(row =>
                        {
                            row.RelativeItem()
                                .Text($"Total de clientes: {clientes.Count}");

                            row.ConstantItem(170)
                                .AlignRight()
                                .Text(x =>
                                {
                                    x.Span("Página ");
                                    x.CurrentPageNumber();
                                    x.Span(" de ");
                                    x.TotalPages();
                                });
                        });
                    });
                });
            });

            return File(pdf.GeneratePdf(),
                "application/pdf",
                "ReporteClientes.pdf");
        }
        public async Task<IActionResult> ProveedoresPdf()
        {
            var proveedores = await _context.Proveedores.ToListAsync();

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);

                    page.Header().Column(header =>
                    {
                        header.Item().Text("📚 MATERIAL ESCOLAR REYES")
                            .FontSize(24).Bold().FontColor(Colors.Blue.Darken3);

                        header.Item().Text("Sistema de Inventario y Ventas")
                            .FontSize(12);

                        header.Item().LineHorizontal(2)
                            .LineColor(Colors.Blue.Darken2);

                        header.Item().PaddingTop(10);

                        header.Item().Text("REPORTE DE PROVEEDORES")
                            .FontSize(20).Bold();

                        header.Item().Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
                    });

                    page.Content().Table(tabla =>
                    {
                        tabla.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(2);
                            c.RelativeColumn(2);
                            c.RelativeColumn();
                            c.RelativeColumn(2);
                        });

                        tabla.Header(h =>
                        {
                            h.Cell().Background(Colors.Blue.Darken2).Padding(7).Text("Proveedor").Bold().FontColor(Colors.White);
                            h.Cell().Background(Colors.Blue.Darken2).Padding(7).Text("Correo").Bold().FontColor(Colors.White);
                            h.Cell().Background(Colors.Blue.Darken2).Padding(7).Text("Teléfono").Bold().FontColor(Colors.White);
                            h.Cell().Background(Colors.Blue.Darken2).Padding(7).Text("Dirección").Bold().FontColor(Colors.White);
                        });

                        foreach (var p in proveedores)
                        {
                            tabla.Cell().Padding(6).BorderBottom(1).Text(p.Nombre);
                            tabla.Cell().Padding(6).BorderBottom(1).Text(p.Correo ?? "-");
                            tabla.Cell().Padding(6).BorderBottom(1).Text(p.Telefono ?? "-");
                            tabla.Cell().Padding(6).BorderBottom(1).Text(p.Direccion ?? "-");
                        }
                    });

                    page.Footer().AlignCenter()
                        .Text($"Total de proveedores: {proveedores.Count}");
                });
            });

            return File(pdf.GeneratePdf(),
                "application/pdf",
                "ReporteProveedores.pdf");
        }
        public async Task<IActionResult> VentasPdf()
        {
            var ventas = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .ToListAsync();

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header().Column(col =>
                    {
                        col.Item()
                            .AlignCenter()
                            .Height(70)
                            .Image(
                                System.IO.File.ReadAllBytes(
                                    Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot",
                                        "images",
                                        "logo.png")),
                                ImageScaling.FitHeight);

                        col.Item()
                            .AlignCenter()
                            .Text("MATERIAL ESCOLAR REYES")
                            .FontSize(22)
                            .Bold();

                        col.Item()
                            .AlignCenter()
                            .Text("Sistema de Inventarios y Ventas")
                            .FontSize(12);

                        col.Item()
                            .AlignCenter()
                            .Text("REPORTE DE VENTAS")
                            .FontSize(18)
                            .Bold();

                        col.Item().PaddingBottom(15);
                    });

                    page.Content().Table(tabla =>
                    {
                        tabla.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn(2);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        tabla.Header(header =>
                        {
                            header.Cell().Background(Colors.Blue.Medium).Padding(5)
                                .Text("Venta").FontColor(Colors.White).Bold();

                            header.Cell().Background(Colors.Blue.Medium).Padding(5)
                                .Text("Cliente").FontColor(Colors.White).Bold();

                            header.Cell().Background(Colors.Blue.Medium).Padding(5)
                                .Text("Fecha").FontColor(Colors.White).Bold();

                            header.Cell().Background(Colors.Blue.Medium).Padding(5)
                                .Text("Total").FontColor(Colors.White).Bold();
                        });

                        foreach (var v in ventas)
                        {
                            tabla.Cell().Padding(4).Text(v.NumeroVenta);

                            tabla.Cell().Padding(4)
                                .Text($"{v.Cliente?.Nombre} {v.Cliente?.Apellido}");

                            tabla.Cell().Padding(4)
                                .Text(v.Fecha.ToString("dd/MM/yyyy"));

                            tabla.Cell().Padding(4)
                                .Text($"Bs {v.Total:N2}");
                        }
                    });

                    page.Footer().Row(row =>
                    {
                        row.RelativeItem()
                            .AlignLeft()
                            .Text($"Total de ventas: {ventas.Count}");

                        row.RelativeItem()
                            .AlignCenter()
                            .Text($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}");

                        row.RelativeItem()
                            .AlignRight()
                            .Text(x =>
                            {
                                x.CurrentPageNumber();
                                x.Span(" / ");
                                x.TotalPages();
                            });
                    });
                });
            });

            return File(
                pdf.GeneratePdf(),
                "application/pdf",
                "ReporteVentas.pdf");
        }
        public async Task<IActionResult> StockPdf()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .ToListAsync();

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);

                    page.Header().Column(header =>
                    {
                        header.Item().Text("📚 MATERIAL ESCOLAR REYES")
                            .FontSize(24).Bold().FontColor(Colors.Blue.Darken3);

                        header.Item().Text("Sistema de Inventario y Ventas");

                        header.Item().LineHorizontal(2);

                        header.Item().PaddingTop(10);

                        header.Item().Text("REPORTE DE STOCK")
                            .FontSize(20).Bold();

                        header.Item().Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
                    });

                    page.Content().Table(tabla =>
                    {
                        tabla.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn();
                            c.RelativeColumn(2);
                            c.RelativeColumn();
                            c.RelativeColumn();
                        });

                        tabla.Header(h =>
                        {
                            h.Cell().Background(Colors.Blue.Darken2).Padding(7).Text("Código").Bold().FontColor(Colors.White);
                            h.Cell().Background(Colors.Blue.Darken2).Padding(7).Text("Producto").Bold().FontColor(Colors.White);
                            h.Cell().Background(Colors.Blue.Darken2).Padding(7).Text("Stock").Bold().FontColor(Colors.White);
                            h.Cell().Background(Colors.Blue.Darken2).Padding(7).Text("Mínimo").Bold().FontColor(Colors.White);
                        });

                        foreach (var p in productos)
                        {
                            tabla.Cell().Padding(6).BorderBottom(1).Text(p.Codigo);
                            tabla.Cell().Padding(6).BorderBottom(1).Text(p.Nombre);
                            tabla.Cell().Padding(6).BorderBottom(1).Text(p.Stock.ToString());
                            tabla.Cell().Padding(6).BorderBottom(1).Text(p.StockMinimo.ToString());
                        }
                    });

                    page.Footer().AlignCenter()
                        .Text($"Productos registrados: {productos.Count}");
                });
            });

            return File(pdf.GeneratePdf(),
                "application/pdf",
                "ReporteStock.pdf");
        }
    }
}