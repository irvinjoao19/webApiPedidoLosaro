using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contexto
{
    public class Reparto
    {
        public int repartoId { get; set; }
        public string numeroPedido { get; set; }
        public int almacenId { get; set; }
        public string descripcion { get; set; }
        public int personalVendedorId { get; set; }
        public string apellidoPersonal { get; set; }
        public int clienteId { get; set; }
        public string apellidoNombreCliente { get; set; }
        public string direccion { get; set; }
        public string fechaEntrega { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public string numeroDocumento { get; set; }
        public decimal subTotal { get; set; }
        public int estado { get; set; }
        public int motivoId { get; set; }
        public string docVTA { get; set; }
        public int localId { get; set; }
        public int distritoId { get; set; }
        public string nombreDistrito { get; set; }
        public List<RepartoDetalle> detalle { get; set; }
    }

    public class RepartoDetalle
    {
        public int detalleId { get; set; }
        public int repartoId { get; set; }
        public int pedidoItem { get; set; }
        public int productoId { get; set; }
        public decimal precioVenta { get; set; }
        public decimal porcentajeDescuento { get; set; }
        public decimal descuento { get; set; }
        public decimal cantidad { get; set; }
        public decimal cantidadExacta { get; set; }
        public decimal porcentajeIGV { get; set; }
        public decimal total { get; set; }
        public string numeroPedido { get; set; }
        public string nombreProducto { get; set; }
        public string codigoProducto { get; set; }
        public int estado { get; set; }
    }
}
