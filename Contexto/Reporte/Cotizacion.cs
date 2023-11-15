using System;
using System.Collections.Generic;

namespace Contexto.Reporte
{
    public class Cotizacion
    {
        public int NroCotizacion { get; set; }

        public string DireccionLinea1 { get; set; }
        public string DireccionLinea2 { get; set; }
        public string DireccionLinea3 { get; set; }
        public string DireccionLinea4 { get; set; }

        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public string Ruc { get; set; }
        public string Direccion { get; set; }
        public string Distrito { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }

        public int Peso { get; set; }
        public string LugarEntrega { get; set; }
        public string TiempoEntrega { get; set; }

        public string FormaPago { get; set; }
        public string Moneda { get; set; }
        public string ValidezOferta { get; set; }


        public string CuentaBcpSoles { get; set; }
        public string CuentaInterbancariaBcpSoles { get; set; }
        public string CuentaBcpDolares { get; set; }
        public string CuentaInterbancariaBcpDolares { get; set; }
        public string CodigoAgenteBcp { get; set; }
        public string CuentaBancoNacionSoles { get; set; }
        public string CuentaBbvaSoles { get; set; }
        public string CuentaInterbancariaBbvaDolares { get; set; }

        public List<CotizacionDetalle> Detalles { get; set; }
    }
}
