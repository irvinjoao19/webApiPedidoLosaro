using Contexto;
using Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Negocio
{
    class porusar
    {
        private static readonly string dbIndus = ConfigurationManager.ConnectionStrings["conexionIndus"].ConnectionString;
        private static readonly string dbFox = ConfigurationManager.ConnectionStrings["conexionFox"].ConnectionString;
         
        public static Mensaje UpdateReparto(Reparto r, int tipo)
        {
            try
            {
                Mensaje m = null;
                var db = tipo == 1 ? dbIndus : dbFox;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Movil_UpdateReparto";
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = r.repartoId;
                    cmd.Parameters.Add("@estado", SqlDbType.Int).Value = r.estado;
                    cmd.Parameters.Add("@id_MotivoDevolucion", SqlDbType.Int).Value = r.motivoId;
                    cmd.Parameters.Add("@cantidad", SqlDbType.Decimal).Value = r.subTotal;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            m = new Mensaje
                            {
                                codigoBase = r.repartoId,
                                codigoRetorno = dr.GetInt32(0),
                                mensaje = "Actualizado"
                            };


                            foreach (var d in r.detalle)
                            {
                                SqlCommand cmdD = cn.CreateCommand();
                                cmdD.CommandType = CommandType.StoredProcedure;
                                cmdD.CommandText = "Movil_Actualizar_cantidad";
                                cmdD.Parameters.Add("@id_pedido", SqlDbType.Int).Value = d.repartoId;
                                cmdD.Parameters.Add("@producto", SqlDbType.Int).Value = d.productoId;
                                cmdD.Parameters.Add("@cant", SqlDbType.Decimal).Value = d.cantidad;
                                cmdD.Parameters.Add("@accion", SqlDbType.Int).Value = d.estado;
                                cmdD.ExecuteNonQuery();
                            }
                        }
                    }
                    cn.Close();
                }
                return m;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static List<Personal> GetPersonal(string fecha, int tipo)
        {
            try
            {
                List<Personal> p = null;
                var db = tipo == 1 ? dbIndus : dbFox;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Movil_Get_ResumenDia";
                    cmd.Parameters.Add("@Fecha", SqlDbType.VarChar).Value = fecha;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        p = new List<Personal>();
                        while (dr.Read())
                        {
                            p.Add(new Personal
                            {
                                personalId = dr.GetInt32(0),
                                nombrePersonal = dr.GetString(1),
                                countPedidos = dr.GetInt32(2),
                                countClientes = dr.GetInt32(3),
                                countProductos = dr.GetInt32(4),
                                total = dr.GetDecimal(5),
                                latitud = dr.GetString(6),
                                longitud = dr.GetString(7)
                            });
                        }
                    }
                    cn.Close();
                }
                return p;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public static Resumen GetResumenes(string fecha, int tipo)
        {
            try
            {
                Resumen r = null;
                var db = tipo == 1 ? dbIndus : dbFox;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Movil_Get_TotalGeneral";
                    cmd.Parameters.Add("@Fecha", SqlDbType.VarChar).Value = fecha;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            r = new Resumen
                            {
                                totalVenta = dr.GetDecimal(0),
                                countPedidoVenta = dr.GetInt32(1),
                                countClientes = dr.GetInt32(2),
                                vendedorId = dr.GetInt32(3),
                                mejorVendedor = dr.GetString(4),
                                mejorVendedorSoles = dr.GetDecimal(5),
                                productoId = dr.GetInt32(6),
                                mejorProducto = dr.GetString(7),
                                mejorProductoSoles = dr.GetDecimal(8),
                                totalDevolucion = dr.GetDecimal(9),
                                peorVendedorId = dr.GetInt32(10),
                                peorVendedor = dr.GetString(11),
                                peorVendedorSoles = dr.GetDecimal(12)
                            };
                        }
                    }
                    cn.Close();
                }
                return r;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        //[HttpPost]
        //[Route("SaveGps")]
        //public IHttpActionResult SaveOperarioGps(EstadoOperario estadoOperario)
        //{
        //    Mensaje mensaje = MigrationDao.SaveGps(estadoOperario);
        //    if (mensaje != null)
        //    {
        //        return Ok(mensaje);
        //    }
        //    else
        //        return BadRequest("Error de Envio");

        //}

        //[HttpPost]
        //[Route("SaveMovil")]
        //public IHttpActionResult SaveMovil(EstadoMovil e)
        //{
        //    Mensaje mensaje = MigrationDao.SaveMovil(e);
        //    if (mensaje != null)
        //    {
        //        return Ok(mensaje);
        //    }
        //    else
        //        return BadRequest("Error de Envio");
        //}


        //[HttpPost]
        //[Route("UpdateReparto")]
        //public IHttpActionResult UpdateReparto(Reparto r, int tipo)
        //{
        //    Mensaje m = MigrationDao.UpdateReparto(r, tipo);
        //    if (m != null)
        //        return Ok(m);
        //    else return BadRequest("Error de Envio");

        //}

        //[HttpGet]
        //[Route("GetPersonal")]
        //public IHttpActionResult GetPersonal(string fecha, int tipo)
        //{
        //    List<Personal> p = MigrationDao.GetPersonal(fecha, tipo);
        //    if (p != null)
        //        return Ok(p);
        //    else return BadRequest("No hay datos");
        //}

        //[HttpGet]
        //[Route("GetResumen")]
        //public IHttpActionResult GetResumen(string fecha, int tipo)
        //{
        //    Resumen r = MigrationDao.GetResumenes(fecha, tipo);
        //    if (r != null)
        //        return Ok(r);
        //    else return BadRequest("No hay datos");

        //}


        public static Mensaje DeleteDetallePedido(PedidoDetalle d)
        {
            try
            {
                Mensaje m = null;
                var db = d.tipoDb == 1 ? dbIndus : dbFox;
                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();

                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_PROY_M_ELIMINA_ITEM_PEDIDO";
                    cmd.Parameters.Add("@pedidoDetalleId", SqlDbType.Int).Value = d.identityDetalle;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            m = new Mensaje
                            {
                                codigoBase = d.pedidoDetalleId,
                                codigoRetorno = dr.GetInt32(0),
                                mensaje = "Eliminado"
                            };
                        }
                    }

                    con.Close();
                }

                return m;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Mensaje DeletePedido(Pedido d)
        {
            try
            {
                Mensaje m = null;
                var db = d.tipoDb == 1 ? dbIndus : dbFox;
                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();

                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_PROY_M_ELIMINA_PEDIDO";
                    cmd.Parameters.Add("@ID_CAB", SqlDbType.Int).Value = d.identity;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            m = new Mensaje
                            {
                                codigoBase = d.pedidoId,
                                codigoRetorno = dr.GetInt32(0),
                                mensaje = "Eliminado"
                            };
                        }
                    }

                    con.Close();
                }

                return m;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //[HttpPost]
        //[Route("DeletePedidoDetalle")]
        //public IHttpActionResult DeletePedidoDetalle(PedidoDetalle p)
        //{
        //    Mensaje m = MigrationDao.DeleteDetallePedido(p);
        //    if (m != null)
        //        return Ok(m);
        //    else return BadRequest("Error de Envio");
        //}

        //[HttpPost]
        //[Route("DeletePedido")]
        //public IHttpActionResult DeletePedido(Pedido p)
        //{
        //    Mensaje m = MigrationDao.DeletePedido(p);
        //    if (m != null)
        //        return Ok(m);
        //    else return BadRequest("Error de Envio");
        //}
    }
}
