using System.Collections.Generic;

namespace Contexto
{
    public class Pedido
    {
        public int pedidoId { get; set; }
        public string numeroPedido { get; set; }
        public string fechaPedido { get; set; }
        public string fechaEntrega { get; set; }
        public string codigoUsuario { get; set; }
        public string codigoCliente { get; set; }
        public string nombreCliente { get; set; }
        public string codformaPago { get; set; }
        public string descripcionformaPago { get; set; }
        public decimal montoTotal { get; set; }
        public string codigoMoneda { get; set; }
        public string descripcionMoneda { get; set; }
        public string codigoEmpTransporte { get; set; }
        public string descripcionEmpTransporte { get; set; }
        public string estado { get; set; }
        public string descripcionEstado { get; set; }
        public string estadoFacturacion { get; set; }
        public string observacion { get; set; }
        public int identity { get; set; }
        public int clienteId { get; set; }
        public int tipoDb { get; set; }
        public int tipoPrecio { get; set; }
        public int precioCambiado { get; set; }
        public string puntoEntrega { get; set; }
        public List<PedidoDetalle> detalles { get; set; }
    }

    public class PedidoDetalle
    {
        public int pedidoDetalleId { get; set; }
        public int pedidoId { get; set; }
        public int item { get; set; }
        public string codigoProducto { get; set; }
        public string descripcionProducto { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio { get; set; }
        public int identity { get; set; }
        public int identityDetalle { get; set; }
        public string unidad { get; set; }
        public int tipoDb { get; set; }
        public int active { get; set; }
        public int estado { get; set; }
        public decimal subTotal { get; set; }
        public string tipoMoneda { get; set; }
        public int precioCambiado { get; set; }
        public int unidadPaquete { get; set; }
        public decimal porDescuento { get; set; }
    }

    public class PedidoFacturacion
    {
        public string td { get; set; }
        public string numeroDoc { get; set; }
        public string fechaDoc { get; set; }
        public int item { get; set; }
        public string codigo { get; set; }
        public string producto { get; set; }
        public string lote { get; set; }
        public decimal cantidad { get; set; }
    }

}
