using Contexto.Reporte;
using Negocio.Reporte;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkiaSharp;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using Microsoft.Ajax.Utilities;

namespace WebApiDsigeVentas.Controllers
{
    [RoutePrefix("api/Cotizacion")]
    public class CotizacionController : ApiController
    {

        [HttpPost]
        [Route("Reporte/{id}")]
        public HttpResponseMessage Post(int id)
        {
            Cotizacion _ = CotizacionDao.BuscarPorId(id);

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(GenerarPdf(_))
            };

            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = $"{_.NroCotizacion}.pdf" };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }

        #region Metodos

        [NonAction]
        public byte[] GenerarPdf(Cotizacion entidad)
        {
            string urlLogo = HttpContext.Current.Server.MapPath("~/img/logo.jpeg"),
                urlFooter = HttpContext.Current.Server.MapPath("~/img/footer.png");

            const float BORDER = 0.5f;

            IContainer DefaultCellHeaderStyle(IContainer container)
            {
                return container
                    .Border(BORDER)
                    .PaddingHorizontal(5)
                    .AlignMiddle();
            }

            IContainer DefaultCellStyle(IContainer container)
            {
                return container
                    .BorderLeft(BORDER)
                    .BorderRight(BORDER)
                    .PaddingHorizontal(5)
                    .AlignMiddle();
            }

            decimal subTotal = 0, igv = 0, descuento = 0, total = 0, totalCantidad = 0;

            var data = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.MarginHorizontal(0);
                    page.MarginVertical(0);

                    page.DefaultTextStyle(x => x.FontSize(9).FontFamily(Fonts.Arial));

                    page.Header()
                        .PaddingHorizontal(0.7f, Unit.Centimetre)
                        .PaddingVertical(0.3f, Unit.Centimetre)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(6);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(6);
                            });

                            table.Cell().Width(6f, Unit.Centimetre).Image(urlLogo);
                            table.Cell().AlignCenter().Column(x =>
                            {
                                x.Item().AlignCenter().Text("COTIZACIÓN").FontFamily(Fonts.TimesNewRoman).FontSize(18).Bold().Italic();
                                x.Item().AlignCenter().Text(entidad.NroCotizacion.ToString("0000000")).FontFamily(Fonts.TimesNewRoman).FontSize(20).Bold().Italic();
                            });

                            table.Cell().PaddingLeft(40).Column(x =>
                            {
                                x.Item().Text(text =>
                                {
                                    text.Line(entidad.DireccionLinea1);
                                    text.Line(entidad.DireccionLinea2);
                                    text.Line(entidad.DireccionLinea3);
                                    text.Line(entidad.DireccionLinea4);
                                });

                            });
                        });

                    IContainer CellHeaderStyle(IContainer _) => DefaultCellHeaderStyle(_);
                    IContainer CellStyle(IContainer _) => DefaultCellStyle(_);

                    page.Content()
                        .PaddingHorizontal(0.7f, Unit.Centimetre)
                        .PaddingVertical(0.2f, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Item().Layers(layers =>
                            {
                                layers.Layer().Canvas((canvas, size) =>
                                {
                                    DrawRoundedRectangle(Colors.White, false);
                                    DrawRoundedRectangle(Colors.Black, true);

                                    void DrawRoundedRectangle(string color, bool isStroke)
                                    {
                                        using (var paint = new SKPaint
                                        {
                                            Color = SKColor.Parse(color),
                                            IsStroke = isStroke,
                                            StrokeWidth = 1,
                                            IsAntialias = true
                                        })
                                        {
                                            canvas.DrawRoundRect(0, 0, size.Width, size.Height, 10, 10, paint);
                                        };

                                    }
                                });

                                layers
                                    .PrimaryLayer()
                                    .PaddingVertical(5)
                                    .PaddingHorizontal(20)
                                    .Table(table =>
                                    {
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn();
                                            columns.RelativeColumn(11);
                                        });

                                        table.Cell().Text("Cliente:");
                                        table.Cell().Text(entidad.Cliente).Bold();

                                        table.Cell().Text("RUC:");
                                        table.Cell().Text(entidad.Ruc);

                                        table.Cell().Text("Fecha:");
                                        table.Cell().Text(entidad.Fecha.ToString("dd/MM/yyyy"));

                                        table.Cell().Text("Dirección:");
                                        table.Cell().Text(entidad.Direccion);

                                        table.Cell().Text("Distrito:");
                                        table.Cell().Text(entidad.Distrito);

                                        table.Cell().Text("Teléfono:");
                                        table.Cell().Text(entidad.Telefono);

                                        table.Cell().Text("E-mail:");
                                        table.Cell().Text(entidad.Email);
                                    });
                            });

                            x.Item().PaddingVertical(4).Text(text =>
                            {
                                text.Line("De nuestra consideración:");
                                text.Span("Por intermedio de la presente nos es grato hacerles llegar nuestra cotización");
                            });

                            x.Item().BorderBottom(BORDER).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(10);
                                    columns.RelativeColumn(2.2f);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellHeaderStyle).AlignCenter().Text("CANTIDAD").SemiBold();
                                    header.Cell().Element(CellHeaderStyle).AlignCenter().Text("DESCRIPCIÓN").SemiBold();
                                    header.Cell().Element(CellHeaderStyle).AlignCenter().Text("P. UNIT.").SemiBold();
                                    header.Cell().Element(CellHeaderStyle).AlignCenter().Text("DESC.").SemiBold();
                                    header.Cell().Element(CellHeaderStyle).AlignCenter().Text("TOTAL").SemiBold();
                                });

                                foreach (var item in entidad.Detalles)
                                {
                                    subTotal += item.SubTotal;
                                    igv += item.Igv;
                                    descuento += item.Descuento;
                                    total += item.Total;

                                    totalCantidad += item.Cantidad;

                                    table.Cell().Element(CellStyle).AlignCenter().Text(item.Cantidad.ToString("#########0"));
                                    table.Cell().Element(CellStyle).Text(item.Producto);
                                    table.Cell().Element(CellStyle).AlignRight().Text(item.Precio.ToString("#########0.000"));
                                    table.Cell().Element(CellStyle).AlignRight().Text(item.Descuento.ToString("#########0.00"));
                                    table.Cell().Element(CellStyle).AlignRight().Text(item.Total.ToString("#########0.00"));
                                }
                            });

                            x.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(10);
                                    columns.RelativeColumn(2.2f);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                });

                                table.Cell().ColumnSpan(2).Text(text =>
                                {
                                    text.Span("Total: ");
                                    text.Span(totalCantidad.ToString("#########0"));
                                });
                                table.Cell().Element(CellHeaderStyle).Text("SUB TOTAL").SemiBold();
                                table.Cell().ColumnSpan(2).Element(CellHeaderStyle).AlignRight().Text(subTotal.ToString("#########0.00"));

                                table.Cell().ColumnSpan(2).Text(text =>
                                {
                                    text.Span("Peso: ");
                                    text.Span(entidad.Peso.ToString()).Bold();
                                    text.Span("   Kg").Bold();
                                });
                                table.Cell().Element(CellHeaderStyle).Text("IGV 18%").SemiBold();
                                table.Cell().ColumnSpan(2).Element(CellHeaderStyle).AlignRight().Text(igv.ToString("#########0.00"));

                                table.Cell().ColumnSpan(2).Text(text =>
                                {
                                    text.Span("Lugar de entrega: ");
                                    text.Span(entidad.LugarEntrega);
                                });
                                table.Cell().Element(CellHeaderStyle).Text("DESCUENTO").SemiBold();
                                table.Cell().ColumnSpan(2).Element(CellHeaderStyle).AlignRight().Text(descuento.ToString("#########0.00"));

                                table.Cell().ColumnSpan(2);
                                table.Cell().Element(CellHeaderStyle).Text("TOTAL").SemiBold();
                                table.Cell().ColumnSpan(2).Element(CellHeaderStyle).AlignRight().Text(text =>
                                {
                                    text.Span("S/.   ");
                                    text.Span(total.ToString("#########0.00"));
                                });
                            });

                            x.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(0.2f);
                                    columns.RelativeColumn(15);
                                });

                                table.Cell().ColumnSpan(2).PaddingLeft(20).Text("CONDICIONES").Bold().Underline();
                                table.Cell();

                                table.Cell().Text("Tiempo de entrega");
                                table.Cell().Text(":");
                                table.Cell().Text(entidad.TiempoEntrega);

                                table.Cell().Text("Forma de pago");
                                table.Cell().Text(":");
                                table.Cell().Text(entidad.FormaPago);

                                table.Cell().Text("Moneda");
                                table.Cell().Text(":");
                                //table.Cell().Text(entidad.Moneda).BackgroundColor(Colors.Grey.Lighten1);
                                table.Cell().Text(entidad.Moneda);

                                table.Cell().Text("Validez de la oferta");
                                table.Cell().Text(":");
                                table.Cell().Text(entidad.ValidezOferta);
                            });


                            x.Item().PaddingTop(10).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });


                                table.Cell().Element(CellHeaderStyle).Text("CTA BCP SOLES :").FontFamily(Fonts.TimesNewRoman);
                                table.Cell().Element(CellHeaderStyle).Text(entidad.CuentaBcpSoles).Bold().FontFamily(Fonts.TimesNewRoman);
                                table.Cell();

                                table.Cell().Element(CellStyle).Text("CTA INTER BANCARIA BCP SOLES :").FontFamily(Fonts.TimesNewRoman);
                                table.Cell().Element(CellHeaderStyle).Text(entidad.CuentaInterbancariaBcpSoles).Bold().FontFamily(Fonts.TimesNewRoman);
                                table.Cell();

                                table.Cell().Element(CellHeaderStyle).Text("CTA BCP DOLARES:").FontFamily(Fonts.TimesNewRoman);
                                table.Cell().Element(CellHeaderStyle).Text(entidad.CuentaBcpDolares).Bold().FontFamily(Fonts.TimesNewRoman);
                                table.Cell();

                                table.Cell().Element(CellHeaderStyle).Text("CTA INTER BANCARIA BCP DOLARES :").FontFamily(Fonts.TimesNewRoman);
                                table.Cell().Element(CellHeaderStyle).Text(entidad.CuentaInterbancariaBcpDolares).Bold().FontFamily(Fonts.TimesNewRoman);
                                table.Cell();

                                table.Cell().Element(CellHeaderStyle).Text("CÓDIGO AGENTE BCP :").FontFamily(Fonts.TimesNewRoman);
                                table.Cell().Element(CellHeaderStyle).Text(entidad.CodigoAgenteBcp).Bold().FontFamily(Fonts.TimesNewRoman);
                                table.Cell();

                                table.Cell().Element(CellHeaderStyle).Text("CTA BANCO DE LA NACIÓN SOLES :").FontFamily(Fonts.TimesNewRoman);
                                table.Cell().Element(CellHeaderStyle).Text(entidad.CuentaBancoNacionSoles).Bold().FontFamily(Fonts.TimesNewRoman);
                                table.Cell();

                                table.Cell().Element(CellHeaderStyle).Text("CTA CONTINENTAL BBVA SOLES :").FontFamily(Fonts.TimesNewRoman);
                                table.Cell().Element(CellHeaderStyle).Text(entidad.CuentaBbvaSoles).Bold().FontFamily(Fonts.TimesNewRoman);
                                table.Cell();

                                table.Cell().Element(CellHeaderStyle).Text("CTA INTER BANCARIA BBVA DOLARES").FontFamily(Fonts.TimesNewRoman);
                                table.Cell().Element(CellHeaderStyle).Text(entidad.CuentaInterbancariaBbvaDolares).Bold().FontFamily(Fonts.TimesNewRoman);
                                table.Cell();
                            });

                        });

                    page.Footer()
                        .Column(x =>
                        {
                            x.Item().Image(urlFooter);
                        });
                });
            });


            return data.GeneratePdf();
        }

        

        #endregion
    }
}
