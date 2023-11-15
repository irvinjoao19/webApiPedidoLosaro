using Contexto.Reporte;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Negocio.Reporte
{
    public class CotizacionDao
    {
        private static readonly string _dbIndus = ConfigurationManager.ConnectionStrings["conexionIndus"].ConnectionString;
        //private static readonly string _dbFox = ConfigurationManager.ConnectionStrings["conexionFox"].ConnectionString;

        public static Cotizacion BuscarPorId(int id)
        {
            Cotizacion cotizacion = null;

            using (SqlConnection cn = new SqlConnection(_dbIndus))
            {
                cn.Open();
                using (SqlCommand cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_PROY_M_GENERA_COTIZACION";
                    cmd.Parameters.AddWithValue("@ID_CAB", id);

                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            if (cotizacion == null)
                            {
                                cotizacion = new Cotizacion
                                {
                                    NroCotizacion = rd.GetInt32(0),
                                    DireccionLinea1 = rd.GetString(1),
                                    DireccionLinea2 = rd.GetString(2),
                                    DireccionLinea3 = rd.GetString(3),
                                    DireccionLinea4 = rd.GetString(4),
                                    Fecha = rd.GetDateTime(5),
                                    Cliente = rd.GetString(6),
                                    Ruc = rd.GetString(7),
                                    Direccion = rd.GetString(8),
                                    Distrito = rd.GetString(9),
                                    Telefono = rd.GetString(10),
                                    Email = rd.GetString(11),
                                    Peso = rd.GetInt32(21),
                                    LugarEntrega = rd.GetString(22),
                                    TiempoEntrega = rd.GetString(23),
                                    FormaPago = rd.GetString(24),
                                    Moneda = rd.GetString(25),
                                    ValidezOferta = rd.GetString(26),
                                    CuentaBcpSoles = rd.GetString(27),
                                    CuentaInterbancariaBcpSoles = rd.GetString(28),
                                    CuentaBcpDolares = rd.GetString(29),
                                    CuentaInterbancariaBcpDolares = rd.GetString(30),
                                    CodigoAgenteBcp = rd.GetString(31),
                                    CuentaBancoNacionSoles = rd.GetString(32),
                                    CuentaBbvaSoles = rd.GetString(33),
                                    CuentaInterbancariaBbvaDolares = rd.GetString(34),
                                    Detalles = new List<CotizacionDetalle>()
                                };
                            }

                            cotizacion.Detalles.Add(new CotizacionDetalle()
                            {
                                Nro = rd.GetString(12),
                                Codigo = rd.GetString(13),
                                Producto = rd.GetString(14),
                                Cantidad = rd.GetDecimal(15),
                                Precio = rd.GetDecimal(16),
                                Descuento = rd.GetDecimal(17),
                                Igv = rd.GetDecimal(18),
                                SubTotal = rd.GetDecimal(19),
                                Total = rd.GetDecimal(20),
                            });
                        }

                        rd.Close();
                    }
                }
                cn.Close();
            }
            return cotizacion;



        }
    }
}
