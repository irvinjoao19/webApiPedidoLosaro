using System.Collections.Generic;

namespace Contexto
{
    public class Sync
    {
        public List<Identidad> identidades { get; set; }
        public List<GiroNegocio> negocios { get; set; }
        public List<FormaPago> formaPagos { get; set; }
        public List<Ubigeo> ubigeos { get; set; }
        public List<Cliente> clientes { get; set; }
        public List<Moneda> moneda { get; set; }
        public List<Transporte> transporte { get; set; }
        public List<Motivo> motivos { get; set; }
        public List<Producto> products { get; set; }
        public List<Pedido> pedidos { get; set; }
        public List<Vendedor> vendedores { get; set; }
        public List<Credito> lineaCreditos { get; set; }
        public List<Despacho> puntoDespachos { get; set; }
        public string mensaje { get; set; }
    }

    public class Moneda
    {
        public string codigo { get; set; }
        public string descripcion { get; set; }

    }
    public class Transporte
    {
        public string codigo { get; set; }
        public string descripcion { get; set; }

    }

    public class Motivo
    {
        public int codigo { get; set; }
        public string descripcion { get; set; }

    }

    public class Identidad
    {
        public string id { get; set; }
        public string descripcion { get; set; }
        public int grupo { get; set; }
    }

    public class Departamento
    {
        public int departamentoId { get; set; }
        public string codigo { get; set; }
        public string departamento { get; set; }

    }

    public class Provincia
    {
        public int provinciaId { get; set; }
        public string codigo { get; set; }
        public string provincia { get; set; }
        public string codigoDeparmento { get; set; }
    }

    public class Distrito
    {
        public int distritoId { get; set; }
        public string codigoProvincia { get; set; }
        public string codigoDepartamento { get; set; }
        public string codigoDistrito { get; set; }
        public string nombre { get; set; }
    }

    public class GiroNegocio
    {
        public string negocioId { get; set; }
        public string nombre { get; set; }
    }

    public class Producto
    {
        public int tipoPrecio { get; set; }
        public string codigoProducto { get; set; }
        public string nombreProducto { get; set; }
        public string presentacionProducto { get; set; }
        public string unidadMedida { get; set; }
        public decimal stock { get; set; }
        public decimal precio { get; set; }
        public decimal precioContado { get; set; }
        public decimal precioCredito { get; set; }
        public int unidadPaquete { get; set; }
        public decimal porDescuento { get; set; }
    }

    public class Cliente
    {
        public int clienteId { get; set; }
        public string tipoDocumento { get; set; }
        public string descripcionDocumento { get; set; }
        public string codigoRuc { get; set; }
        public string razonSocial { get; set; }
        public string codigoGiroNegocio { get; set; }
        public string descripcionGiroNegocio { get; set; }
        public string codigoCondicionPago { get; set; }
        public string descripcionCondicionPago { get; set; }
        public string codigoDepartamento { get; set; }
        public string codigoProvincia { get; set; }
        public string codigoDistrito { get; set; }
        public string nombreDepartamento { get; set; }
        public string nombreProvincia { get; set; }
        public string nombreDistrito { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public string diaVisita { get; set; }
        public string motivonoCompra { get; set; }
        public string productoInteres { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public int tipodb { get; set; }
        public int identity { get; set; }
        public string usuarioId { get; set; }
        public int tipoPrecio { get; set; }
    }

    public class FormaPago
    {
        public string formaPagoId { get; set; }
        public string descripcion { get; set; }
    }

    public class Estado
    {
        public int estadoId { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string tipoProceso { get; set; }
        public string descripcionTipoProceso { get; set; }
        public int moduloId { get; set; }
        public int backColor { get; set; }
        public string forecolor { get; set; }
        public int estado { get; set; }
    }

    public class Grupo
    {
        public int detalleTablaId { get; set; }
        public int grupoTablaId { get; set; }
        public string codigoDetalle { get; set; }
        public string descripcion { get; set; }
        public int estado { get; set; }
    }

    public class Local
    {
        public int localId { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public int estado { get; set; }
    }

    public class Visita
    {
        public int visitaId { get; set; }
        public string nombreVisita { get; set; }
    }

    public class Ubigeo
    {
        public int id { get; set; }
        public string codDepartamento { get; set; }
        public string nombreDepartamento { get; set; }
        public string codProvincia { get; set; }
        public string provincia { get; set; }
        public string codDistrito { get; set; }
        public string nombreDistrito { get; set; }
    }

    public class ClienteCredito
    {
        public decimal lineacd { get; set; }
        public decimal lineacs { get; set; }
        public decimal lineadd { get; set; }
        public decimal lineads { get; set; }
    }

    public class EstadoCuenta
    {
        public string cliente { get; set; }
        public string td { get; set; }
        public string numeroDoc { get; set; }
        public string fechaEmision { get; set; }
        public string fechaVcto { get; set; }
        public string moneda { get; set; }
        public decimal tc { get; set; }
        public decimal montoOriginal { get; set; }
        public decimal saldo { get; set; }
        public string estado { get; set; }
        public string banco { get; set; }
        public string condicionVenta { get; set; }
        public decimal totalSoles { get; set; }
        public decimal totalDolares { get; set; }
    }

    public class ClientVisit
    {
        public string usuarioId { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public string descripcion { get; set; }
        public int tipo { get; set; }
        public int clienteId { get; set; }
        public int motivoVisitaId { get; set; }
    }
    public class Vendedor
    {
        public string codUsuario { get; set; }
        public string codVendedor { get; set; }
        public string estado { get; set; }
        public string nivel { get; set; }
        public string nombreVendedor { get; set; }
        public string nomUsuario { get; set; }
        public string pass { get; set; }
        public string telefono { get; set; }
    }

    public class Credito
    {
        public int clienteId { get; set; }
        public decimal lineaCreditoDolares { get; set; }
        public decimal lineaCreditoSoles { get; set; }
        public decimal lineaDisponibleDolares { get; set; }
        public decimal lineaDisponibleSoles { get; set; }
    }
    public class Despacho
    {
        public int despachoId { get; set; }
        public int clienteId { get; set; }
        public int item { get; set; }
        public string addr { get; set; }
    }

    public class PuntoContacto
    {
        public int puntoId { get; set; }
        public string usuario { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public string fecha { get; set; }
    }
}
