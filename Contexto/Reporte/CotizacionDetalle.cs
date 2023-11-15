namespace Contexto.Reporte
{
    public class CotizacionDetalle
    {
        public string Nro { get; set; }
        public string Codigo { get; set; }
        public string Producto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Descuento { get; set; }
        public decimal Igv { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
    }
}
