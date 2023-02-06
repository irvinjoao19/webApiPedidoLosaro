using Contexto;
using Entidades;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Negocio
{
    public class MigrationDao
    {
        private static readonly string dbIndus = ConfigurationManager.ConnectionStrings["conexionIndus"].ConnectionString;
        private static readonly string dbFox = ConfigurationManager.ConnectionStrings["conexionFox"].ConnectionString;

        public static Usuario GetLogin(Filtro f)
        {
            try
            {
                Usuario u = null;
                var db = f.tipo == 1 ? dbIndus : dbFox;
                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();

                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_PROY_M_LOGIN";
                    cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = f.codUsuario;
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        u = new Usuario();

                        if (f.pass == dr.GetString(1))
                        {
                            u.codUsuario = dr.GetString(0);
                            u.pass = dr.GetString(1);
                            u.nombreUsuario = dr.GetString(2);
                            u.nivel = dr.GetString(3);
                            u.estado = dr.GetString(4);
                            u.codigoVendedor = dr.GetString(5);
                            u.nombreVendedor = dr.GetString(6);
                            u.telefono = dr.GetString(7);
                            u.email = dr.GetString(8);
                            u.itemPedido = dr.GetInt32(9);
                            u.horaInicio = dr.GetString(10);
                            u.horaFin = dr.GetString(11);
                            u.intervalos = dr.GetInt32(12);
                            u.tipo = f.tipo;
                        }
                        else
                        {
                            u.pass = "Error";
                        }
                    }

                    dr.Close();

                    con.Close();
                }

                return u;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static ClienteCredito GetClienteCredito(Filtro p)
        {
            try
            {
                ClienteCredito m = null;
                var db = p.tipo == 1 ? dbIndus : dbFox;
                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();

                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_PROY_M_LISTA_CLIENTES_LINEA_CREDITO";
                    cmd.Parameters.Add("@CODIGO_CLIENTE", SqlDbType.VarChar).Value = p.codUsuario;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            m = new ClienteCredito()
                            {
                                lineacd = dr.GetDecimal(0),
                                lineacs = dr.GetDecimal(1),
                                lineadd = dr.GetDecimal(2),
                                lineads = dr.GetDecimal(3)
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
        public static Mensaje GetLogout(Filtro f)
        {
            try
            {
                Mensaje m = null;
                var db = f.tipo == 1 ? dbIndus : dbFox;
                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();

                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Movil_Logout";
                    cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = f.codUsuario;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            m = new Mensaje
                            {
                                mensaje = dr.GetString(0)
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
        public static Sync GetSync(Filtro f)
        {
            try
            {
                Sync sync = new Sync();
                var db = f.tipo == 1 ? dbIndus : dbFox;
                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();
                    //// Version
                    //SqlCommand cmdVersion = con.CreateCommand();
                    //cmdVersion.CommandTimeout = 0;
                    //cmdVersion.CommandType = CommandType.StoredProcedure;
                    //cmdVersion.CommandText = "Movil_GetVersion";
                    //cmdVersion.Parameters.Add("@version", SqlDbType.VarChar).Value = version;

                    //SqlDataReader drVersion = cmdVersion.ExecuteReader();
                    //if (!drVersion.HasRows)
                    //{
                    //    sync.mensaje = "Update";
                    //}
                    //else
                    //{
                    // Identidad
                    SqlCommand cmdIdentidad = con.CreateCommand();
                    cmdIdentidad.CommandTimeout = 0;
                    cmdIdentidad.CommandType = CommandType.StoredProcedure;
                    cmdIdentidad.CommandText = "SP_PROY_M_LISTA_TIPO_CLIENTE";
                    SqlDataReader drI = cmdIdentidad.ExecuteReader();
                    if (drI.HasRows)
                    {
                        List<Identidad> i = new List<Identidad>();
                        while (drI.Read())
                        {
                            i.Add(new Identidad()
                            {
                                id = drI.GetString(0),
                                descripcion = drI.GetString(1),
                                grupo = 0
                            });
                        }
                        sync.identidades = i;
                    }

                    // Negocio
                    SqlCommand cmdNegocio = con.CreateCommand();
                    cmdNegocio.CommandTimeout = 0;
                    cmdNegocio.CommandType = CommandType.StoredProcedure;
                    cmdNegocio.CommandText = "SP_PROY_M_LISTA_GIRO_NEGOCIO";
                    SqlDataReader drN = cmdNegocio.ExecuteReader();
                    if (drN.HasRows)
                    {
                        List<GiroNegocio> n = new List<GiroNegocio>();
                        while (drN.Read())
                        {
                            n.Add(new GiroNegocio()
                            {
                                negocioId = drN.GetString(0),
                                nombre = drN.GetString(1)

                            });
                        }
                        sync.negocios = n;
                    }

                    SqlCommand cmdDe = con.CreateCommand();
                    cmdDe.CommandTimeout = 0;
                    cmdDe.CommandType = CommandType.StoredProcedure;
                    cmdDe.CommandText = "SP_PROY_M_UBIGEO";
                    SqlDataReader drDe = cmdDe.ExecuteReader();
                    if (drDe.HasRows)
                    {
                        List<Ubigeo> e = new List<Ubigeo>();
                        while (drDe.Read())
                        {
                            e.Add(new Ubigeo()
                            {
                                id = drDe.GetInt32(0),
                                codDepartamento = drDe.GetString(1),
                                nombreDepartamento = drDe.GetString(2),
                                codProvincia = drDe.GetString(3),
                                provincia = drDe.GetString(4),
                                codDistrito = drDe.GetString(5),
                                nombreDistrito = drDe.GetString(6)
                            });
                        }
                        sync.ubigeos = e;
                    }

                    SqlCommand cmdC = con.CreateCommand();
                    cmdC.CommandTimeout = 0;
                    cmdC.CommandType = CommandType.StoredProcedure;
                    cmdC.CommandText = "SP_PROY_M_LISTA_CLIENTES";
                    cmdC.Parameters.Add("@codUsuario", SqlDbType.VarChar).Value = f.codUsuario;
                    SqlDataReader drC = cmdC.ExecuteReader();
                    if (drC.HasRows)
                    {
                        List<Cliente> d = new List<Cliente>();
                        while (drC.Read())
                        {
                            d.Add(new Cliente()
                            {
                                identity = drC.GetInt32(0),
                                clienteId = drC.GetInt32(0),
                                tipoDocumento = drC.GetString(1),
                                descripcionDocumento = drC.GetString(2),
                                codigoRuc = drC.GetString(3),
                                razonSocial = drC.GetString(4),
                                codigoGiroNegocio = drC.GetString(5),
                                descripcionGiroNegocio = drC.GetString(6),
                                codigoCondicionPago = drC.GetString(7),
                                descripcionCondicionPago = drC.GetString(8),
                                codigoDepartamento = drC.GetString(9),
                                codigoProvincia = drC.GetString(10),
                                codigoDistrito = drC.GetString(11),
                                nombreDepartamento = drC.GetString(12),
                                nombreProvincia = drC.GetString(13),
                                nombreDistrito = drC.GetString(14),
                                direccion = drC.GetString(15),
                                telefono = drC.GetString(16),
                                email = drC.GetString(17),
                                diaVisita = drC.GetString(18),
                                motivonoCompra = drC.GetString(19),
                                productoInteres = drC.GetString(20),
                                latitud = drC.GetString(21),
                                longitud = drC.GetString(22),
                                usuarioId = drC.GetString(23),
                                tipoPrecio = drC.GetInt32(24),
                                tipodb = f.tipo
                            });
                        }
                        sync.clientes = d;
                    }

                    SqlCommand cmdF = con.CreateCommand();
                    cmdF.CommandTimeout = 0;
                    cmdF.CommandType = CommandType.StoredProcedure;
                    cmdF.CommandText = "SP_PROY_M_LISTA_CONDICION_PAGO";
                    SqlDataReader drF = cmdF.ExecuteReader();
                    if (drF.HasRows)
                    {
                        List<FormaPago> fo = new List<FormaPago>();
                        while (drF.Read())
                        {
                            fo.Add(new FormaPago()
                            {
                                formaPagoId = drF.GetString(0),
                                descripcion = drF.GetString(1),
                            });
                        }
                        sync.formaPagos = fo;
                    }

                    SqlCommand cmdG = con.CreateCommand();
                    cmdG.CommandTimeout = 0;
                    cmdG.CommandType = CommandType.StoredProcedure;
                    cmdG.CommandText = "SP_PROY_M_LISTA_MONEDA";
                    SqlDataReader drG = cmdG.ExecuteReader();
                    if (drG.HasRows)
                    {
                        List<Moneda> g = new List<Moneda>();
                        while (drG.Read())
                        {
                            g.Add(new Moneda()
                            {
                                codigo = drG.GetString(0),
                                descripcion = drG.GetString(1)
                            });
                        }
                        sync.moneda = g;
                    }

                    SqlCommand cmdL = con.CreateCommand();
                    cmdL.CommandTimeout = 0;
                    cmdL.CommandType = CommandType.StoredProcedure;
                    cmdL.CommandText = "SP_PROY_M_LISTA_EMP_TRANSPORTE";
                    SqlDataReader drL = cmdL.ExecuteReader();
                    if (drL.HasRows)
                    {
                        List<Transporte> l = new List<Transporte>();
                        while (drL.Read())
                        {
                            l.Add(new Transporte()
                            {
                                codigo = drL.GetString(0),
                                descripcion = drL.GetString(1)
                            });
                        }
                        sync.transporte = l;
                    }

                    SqlCommand cmdM = con.CreateCommand();
                    cmdM.CommandTimeout = 0;
                    cmdM.CommandType = CommandType.StoredProcedure;
                    cmdM.CommandText = "SP_PROY_M_LISTA_MOTIVOS_VISITA";
                    SqlDataReader drM = cmdM.ExecuteReader();
                    if (drM.HasRows)
                    {
                        List<Motivo> m = new List<Motivo>();
                        while (drM.Read())
                        {
                            m.Add(new Motivo()
                            {
                                codigo = drM.GetInt32(0),
                                descripcion = drM.GetString(1)
                            });
                        }
                        sync.motivos = m;
                    }

                    SqlCommand cmdP = con.CreateCommand();
                    cmdP.CommandType = CommandType.StoredProcedure;
                    cmdP.CommandTimeout = 0;
                    cmdP.CommandText = "SP_PROY_M_LISTA_PRODUCTOS_PRECIOS";
                    SqlDataReader drP = cmdP.ExecuteReader();
                    if (drP.HasRows)
                    {
                        List<Producto> p = new List<Producto>();
                        while (drP.Read())
                        {
                            p.Add(new Producto()
                            {
                                tipoPrecio = drP.GetInt32(0),
                                codigoProducto = drP.GetString(1),
                                nombreProducto = drP.GetString(2),
                                presentacionProducto = drP.GetString(3),
                                unidadMedida = drP.GetString(4),
                                stock = drP.GetDecimal(5),
                                precio = drP.GetDecimal(6),
                                precioContado = drP.GetDecimal(7),
                                precioCredito = drP.GetDecimal(8),
                                unidadPaquete = drP.GetInt32(9),
                                porDescuento = drP.GetDecimal(10),
                            });
                        }
                        sync.products = p;
                    }


                    if (f.syncPedidos == 1)
                    {
                        SqlCommand cmd1 = con.CreateCommand();
                        cmd1.CommandTimeout = 0;
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.CommandText = "SP_PROY_M_LISTA_PEDIDOS";
                        cmd1.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = f.codUsuario;
                        cmd1.Parameters.Add("@FECHA_INICIAL", SqlDbType.VarChar).Value = f.fechaInicio;
                        cmd1.Parameters.Add("@FECHA_FINAL", SqlDbType.VarChar).Value = f.fechaFinal;
                        cmd1.Parameters.Add("@PED_PRECIOS_CAMBIADOS", SqlDbType.Int).Value = f.precioCambiado;

                        SqlDataReader dr1 = cmd1.ExecuteReader();
                        if (dr1.HasRows)
                        {
                            List<Pedido> p = new List<Pedido>();
                            while (dr1.Read())
                            {
                                var r = new Pedido
                                {
                                    pedidoId = dr1.GetInt32(0),
                                    identity = dr1.GetInt32(0),
                                    numeroPedido = dr1.GetString(1),
                                    codigoCliente = dr1.GetString(2),
                                    nombreCliente = dr1.GetString(3),
                                    fechaPedido = dr1.GetString(4),
                                    fechaEntrega = dr1.GetString(5),
                                    codformaPago = dr1.GetString(6),
                                    descripcionformaPago = dr1.GetString(7),
                                    codigoMoneda = dr1.GetString(8),
                                    descripcionMoneda = dr1.GetString(9),
                                    estado = dr1.GetString(10),
                                    descripcionEstado = dr1.GetString(11),
                                    codigoUsuario = dr1.GetString(12),
                                    codigoEmpTransporte = dr1.GetString(13),
                                    descripcionEmpTransporte = dr1.GetString(14),
                                    montoTotal = dr1.GetDecimal(15),
                                    estadoFacturacion = dr1.GetString(16),
                                    clienteId = dr1.GetInt32(17),
                                    observacion = dr1.GetString(18),
                                    tipoPrecio = dr1.GetInt32(19),
                                    precioCambiado = dr1.GetInt32(20),
                                    puntoEntrega = dr1.GetString(21),
                                    tipoDb = f.tipo
                                };

                                SqlCommand cmd2 = con.CreateCommand();
                                cmd2.CommandTimeout = 0;
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.CommandText = "SP_PROY_M_LISTA_PEDIDOS_DETALLE";
                                cmd2.Parameters.Add("@ID_CAB", SqlDbType.Int).Value = r.pedidoId;
                                SqlDataReader dr2 = cmd2.ExecuteReader();
                                if (dr2.HasRows)
                                {
                                    List<PedidoDetalle> d = new List<PedidoDetalle>();
                                    while (dr2.Read())
                                    {
                                        d.Add(new PedidoDetalle()
                                        {
                                            pedidoDetalleId = dr2.GetInt32(0),
                                            identityDetalle = dr2.GetInt32(0),
                                            pedidoId = dr2.GetInt32(1),
                                            identity = dr2.GetInt32(1),
                                            codigoProducto = dr2.GetString(2),
                                            descripcionProducto = dr2.GetString(3),
                                            unidad = dr2.GetString(4),
                                            cantidad = dr2.GetDecimal(5),
                                            precio = dr2.GetDecimal(6),
                                            subTotal = dr2.GetDecimal(7),
                                            precioCambiado = dr2.GetInt32(8),
                                            unidadPaquete = dr2.GetInt32(9),
                                            porDescuento = dr2.GetDecimal(10),
                                            item = 0,
                                            active = 1,
                                            tipoDb = f.tipo,
                                            tipoMoneda = dr1.GetString(9),
                                            estado = Convert.ToInt32(dr1.GetString(10))
                                        });
                                    }
                                    r.detalles = d;
                                }
                                p.Add(r);
                            }

                            sync.pedidos = p;
                        }
                    }


                  

                    SqlCommand cmd3 = con.CreateCommand();
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.CommandTimeout = 0;
                    cmd3.CommandText = "SP_PROY_M_LISTA_USUARIOS";
                    SqlDataReader dr3 = cmd3.ExecuteReader();
                    if (dr3.HasRows)
                    {
                        List<Vendedor> v = new List<Vendedor>();
                        while (dr3.Read())
                        {
                            v.Add(new Vendedor()
                            {
                                codUsuario = dr3.GetString(0),
                                codVendedor = dr3.GetString(1),
                                estado = dr3.GetString(2),
                                nivel = dr3.GetString(3),
                                nombreVendedor = dr3.GetString(4),
                                nomUsuario = dr3.GetString(5),
                                pass = dr3.GetString(6),
                                telefono = dr3.GetString(7),
                            });
                        }
                        sync.vendedores = v;
                    }

                    SqlCommand cmd4 = con.CreateCommand();
                    cmd4.CommandType = CommandType.StoredProcedure;
                    cmd4.CommandTimeout = 0;
                    cmd4.CommandText = "SP_PROY_M_LISTA_CLIENTES_LINEA_CREDITO";
                    cmd4.Parameters.Add("@codUsuario", SqlDbType.VarChar).Value = f.codUsuario;

                    SqlDataReader dr4 = cmd4.ExecuteReader();
                    if (dr4.HasRows)
                    {
                        List<Credito> c = new List<Credito>();
                        while (dr4.Read())
                        {
                            c.Add(new Credito()
                            {
                                clienteId = dr4.GetInt32(0),
                                lineaCreditoDolares = dr4.GetDecimal(1),
                                lineaCreditoSoles = dr4.GetDecimal(2),
                                lineaDisponibleDolares = dr4.GetDecimal(3),
                                lineaDisponibleSoles = dr4.GetDecimal(4),
                            });
                        }
                        sync.lineaCreditos = c;
                    }

                    SqlCommand cmd5 = con.CreateCommand();
                    cmd5.CommandType = CommandType.StoredProcedure;
                    cmd5.CommandTimeout = 0;
                    cmd5.CommandText = "SP_PROY_M_LISTA_CLIENTES_PUNTOS_DESPACHO";
                    cmd5.Parameters.Add("@codUsuario", SqlDbType.VarChar).Value = f.codUsuario;
                    SqlDataReader dr5 = cmd5.ExecuteReader();
                    if (dr5.HasRows)
                    {
                        List<Despacho> d = new List<Despacho>();
                        while (dr5.Read())
                        {
                            d.Add(new Despacho()
                            {
                                despachoId = dr5.GetInt32(0),
                                clienteId = dr5.GetInt32(1),
                                item = dr5.GetInt32(2),
                                addr = dr5.GetString(3),

                            });
                        }
                        sync.puntoDespachos = d;
                    }



                    sync.mensaje = "Sincronización Completada.";

                    con.Close();
                }
                return sync;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static Mensaje UpdateCoordenadas(Filtro f)
        {
            try
            {
                Mensaje m = null;
                var db = f.tipo == 1 ? dbIndus : dbFox;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_PROY_M_GUARDA_GEOLOCALIZACION_CLIENTES";
                    cmd.Parameters.Add("@clienteId", SqlDbType.Int).Value = f.clienteId;
                    cmd.Parameters.Add("@LATITUD", SqlDbType.VarChar).Value = f.latitud;
                    cmd.Parameters.Add("@LONGITUD", SqlDbType.VarChar).Value = f.longitud;
                    int a = cmd.ExecuteNonQuery();
                    if (a == 1)
                    {
                        m = new Mensaje
                        {
                            codigoBase = f.clienteId,
                            mensaje = "Enviado"
                        };
                    }
                    cn.Close();
                }
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Mensaje SaveCliente(Cliente c)
        {
            try
            {
                Mensaje m = null;
                var db = c.tipodb == 1 ? dbIndus : dbFox;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "SP_PROY_M_GUARDA_CLIENTES";
                    cmd.Parameters.Add("@clienteId", SqlDbType.Int).Value = c.identity;

                    cmd.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = c.tipoDocumento;
                    cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = c.codigoRuc;
                    cmd.Parameters.Add("@NAME", SqlDbType.VarChar).Value = c.razonSocial;
                    cmd.Parameters.Add("@BUSINESS_TYPE", SqlDbType.VarChar).Value = c.codigoGiroNegocio;
                    cmd.Parameters.Add("@TERMS_TYPE", SqlDbType.VarChar).Value = c.codigoCondicionPago;
                    cmd.Parameters.Add("@DEPARTAMENTO_ID", SqlDbType.VarChar).Value = c.codigoDepartamento;
                    cmd.Parameters.Add("@PROVINCIA_ID", SqlDbType.VarChar).Value = c.codigoProvincia;
                    cmd.Parameters.Add("@DISTRITO_ID", SqlDbType.VarChar).Value = c.codigoDistrito;
                    cmd.Parameters.Add("@ADDR", SqlDbType.VarChar).Value = c.direccion;
                    cmd.Parameters.Add("@PHONE", SqlDbType.VarChar).Value = c.telefono;
                    cmd.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = c.email;
                    cmd.Parameters.Add("@DIA_VISITA", SqlDbType.VarChar).Value = c.diaVisita;
                    cmd.Parameters.Add("@MOTIVO_NO_COMPRA", SqlDbType.VarChar).Value = c.motivonoCompra;
                    cmd.Parameters.Add("@PRODUCTO_INTERES", SqlDbType.VarChar).Value = c.productoInteres;
                    cmd.Parameters.Add("@usuario_creacion", SqlDbType.VarChar).Value = c.usuarioId;


                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            m = new Mensaje
                            {
                                codigoBase = c.clienteId,
                                codigoRetorno = dr.GetInt32(0),
                                mensaje = "Enviado"
                            };
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
        public static List<Producto> GetProductos(int tipo, int tipoPrecio)
        {
            try
            {
                List<Producto> p = null;
                var db = tipo == 1 ? dbIndus : dbFox;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "SP_PROY_M_LISTA_PRODUCTOS";
                    cmd.Parameters.Add("@TIPO_PRECIO", SqlDbType.Int).Value = tipoPrecio;
                    SqlDataReader drPr = cmd.ExecuteReader();
                    if (drPr.HasRows)
                    {
                        p = new List<Producto>();
                        while (drPr.Read())
                        {
                            p.Add(new Producto()
                            {
                                codigoProducto = drPr.GetString(0),
                                nombreProducto = drPr.GetString(1),
                                presentacionProducto = drPr.GetString(2),
                                unidadMedida = drPr.GetString(3),
                                stock = drPr.GetDecimal(4),
                                precio = drPr.GetDecimal(5),
                                precioContado = drPr.GetDecimal(6),
                                precioCredito = drPr.GetDecimal(7),
                                unidadPaquete = drPr.GetInt32(8),
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
        public static List<Pedido> GetPedidos(int tipo, string codUsuario, string fechaInicio, string fechaFinal, int precioCambiado)
        {
            try
            {
                List<Pedido> p = null;
                var db = tipo == 1 ? dbIndus : dbFox;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();

                    SqlCommand cmd1 = cn.CreateCommand();
                    cmd1.CommandTimeout = 0;
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.CommandText = "SP_PROY_M_LISTA_PEDIDOS";
                    cmd1.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = codUsuario;
                    cmd1.Parameters.Add("@FECHA_INICIAL", SqlDbType.VarChar).Value = fechaInicio;
                    cmd1.Parameters.Add("@FECHA_FINAL", SqlDbType.VarChar).Value = fechaFinal;
                    cmd1.Parameters.Add("@PED_PRECIOS_CAMBIADOS", SqlDbType.Int).Value = precioCambiado;

                    SqlDataReader dr1 = cmd1.ExecuteReader();
                    if (dr1.HasRows)
                    {
                        p = new List<Pedido>();
                        while (dr1.Read())
                        {
                            var r = new Pedido
                            {
                                pedidoId = dr1.GetInt32(0),
                                identity = dr1.GetInt32(0),
                                numeroPedido = dr1.GetString(1),
                                codigoCliente = dr1.GetString(2),
                                nombreCliente = dr1.GetString(3),
                                fechaPedido = dr1.GetString(4),
                                fechaEntrega = dr1.GetString(5),
                                codformaPago = dr1.GetString(6),
                                descripcionformaPago = dr1.GetString(7),
                                codigoMoneda = dr1.GetString(8),
                                descripcionMoneda = dr1.GetString(9),
                                estado = dr1.GetString(10),
                                descripcionEstado = dr1.GetString(11),
                                codigoUsuario = dr1.GetString(12),
                                codigoEmpTransporte = dr1.GetString(13),
                                descripcionEmpTransporte = dr1.GetString(14),
                                montoTotal = dr1.GetDecimal(15),
                                estadoFacturacion = dr1.GetString(16),
                                clienteId = dr1.GetInt32(17),
                                observacion = dr1.GetString(18),
                                tipoPrecio = dr1.GetInt32(19),
                                precioCambiado = dr1.GetInt32(20),
                                puntoEntrega = dr1.GetString(21),
                                tipoDb = tipo
                            };

                            SqlCommand cmd2 = cn.CreateCommand();
                            cmd2.CommandTimeout = 0;
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.CommandText = "SP_PROY_M_LISTA_PEDIDOS_DETALLE";
                            cmd2.Parameters.Add("@ID_CAB", SqlDbType.Int).Value = r.pedidoId;
                            SqlDataReader dr2 = cmd2.ExecuteReader();
                            if (dr2.HasRows)
                            {
                                List<PedidoDetalle> d = new List<PedidoDetalle>();
                                while (dr2.Read())
                                {
                                    d.Add(new PedidoDetalle()
                                    {
                                        pedidoDetalleId = dr2.GetInt32(0),
                                        identityDetalle = dr2.GetInt32(0),
                                        pedidoId = dr2.GetInt32(1),
                                        identity = dr2.GetInt32(1),
                                        codigoProducto = dr2.GetString(2),
                                        descripcionProducto = dr2.GetString(3),
                                        unidad = dr2.GetString(4),
                                        cantidad = dr2.GetDecimal(5),
                                        precio = dr2.GetDecimal(6),
                                        subTotal = dr2.GetDecimal(7),
                                        precioCambiado = dr2.GetInt32(8),
                                        unidadPaquete = dr2.GetInt32(9),
                                        item = 0,
                                        active = 1,
                                        tipoDb = tipo,
                                        tipoMoneda = dr1.GetString(9),
                                        estado = Convert.ToInt32(dr1.GetString(10))
                                    });
                                }
                                r.detalles = d;
                            }
                            p.Add(r);
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
            
        public static Mensaje ChangePass(Filtro e)
        {
            try
            {
                Mensaje m = null;
                var db = e.tipo == 1 ? dbIndus : dbFox;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_PROY_M_UPDATE_PASS";
                    cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = e.codUsuario;
                    cmd.Parameters.Add("@pass", SqlDbType.VarChar).Value = e.newPass;
                    int a = cmd.ExecuteNonQuery();
                    if (a == 1)
                    {
                        m = new Mensaje
                        {
                            codigo = 1,
                            mensaje = "Enviado"
                        };
                    }
                    cn.Close();
                }
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<PedidoFacturacion> GetPedidoFacturacion(Filtro f)
        {
            try
            {
                List<PedidoFacturacion> p = null;
                var db = f.tipo == 1 ? dbIndus : dbFox;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "SP_PROY_M_LISTA_FACTURACION_PEDIDO";
                    cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = f.codPedido;
                    SqlDataReader drPr = cmd.ExecuteReader();
                    if (drPr.HasRows)
                    {
                        p = new List<PedidoFacturacion>();
                        while (drPr.Read())
                        {
                            p.Add(new PedidoFacturacion()
                            {
                                td = drPr.GetString(0),
                                numeroDoc = drPr.GetString(1),
                                fechaDoc = drPr.GetString(2),
                                item = drPr.GetInt32(3),
                                codigo = drPr.GetString(4),
                                producto = drPr.GetString(5),
                                lote = drPr.GetString(6),
                                cantidad = drPr.GetDecimal(7)
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

        public static List<EstadoCuenta> GetEstadoCuenta(Filtro f)
        {
            try
            {
                List<EstadoCuenta> p = null;
                var db = f.tipo == 1 ? dbIndus : dbFox;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "SP_PROY_M_ESTADO_CUENTA_CLIENTE";
                    cmd.Parameters.Add("@codigo", SqlDbType.VarChar).Value = f.codUsuario;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        p = new List<EstadoCuenta>();
                        while (dr.Read())
                        {
                            p.Add(new EstadoCuenta()
                            {
                                cliente = dr.GetString(0),
                                td = dr.GetString(1),
                                numeroDoc = dr.GetString(2),
                                fechaEmision = dr.GetString(3),
                                fechaVcto = dr.GetString(4),
                                moneda = dr.GetString(5),
                                tc = dr.GetDecimal(6),
                                montoOriginal = dr.GetDecimal(7),
                                saldo = dr.GetDecimal(8),
                                estado = dr.GetString(9),
                                banco = dr.GetString(10),
                                condicionVenta = dr.GetString(11),
                                totalSoles = dr.GetDecimal(12),
                                totalDolares = dr.GetDecimal(13),
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

        public static Mensaje SaveClientVisit(ClientVisit c)
        {
            try
            {
                Mensaje m = null;
                var db = c.tipo == 1 ? dbIndus : dbFox;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_PROY_M_GUARDA_VISITAS_CLIENTES";
                    cmd.Parameters.Add("@id_Usuario", SqlDbType.VarChar).Value = c.usuarioId;
                    cmd.Parameters.Add("@latitud_punto_contacto", SqlDbType.VarChar).Value = c.latitud;
                    cmd.Parameters.Add("@longitud_punto_contacto", SqlDbType.VarChar).Value = c.longitud;
                    cmd.Parameters.Add("@descripcion_visita", SqlDbType.VarChar).Value = c.descripcion;
                    cmd.Parameters.Add("@id_cliente", SqlDbType.Int).Value = c.clienteId;
                    cmd.Parameters.Add("@id_motivo_visita", SqlDbType.Int).Value = c.motivoVisitaId;
                    int a = cmd.ExecuteNonQuery();
                    if (a == 1)
                    {
                        m = new Mensaje
                        {
                            codigo = 1,
                            mensaje = "Enviado"
                        };
                    }
                    cn.Close();
                }
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<PuntoContacto> GetPuntoContacto(string code)
        {
            try
            {
                List<PuntoContacto> p = null;
                var db = dbFox;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "SP_PROY_M_S_ULTIMA_GEOLOCALIZACION";
                    cmd.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = code;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        p = new List<PuntoContacto>();
                        while (dr.Read())
                        {
                            p.Add(new PuntoContacto()
                            {
                                puntoId = dr.GetInt32(0),
                                usuario = dr.GetString(1),
                                latitud = dr.GetString(2),
                                longitud = dr.GetString(3),
                                fecha = dr.GetDateTime(4).ToString("dd/MM/yyyy")
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

        public static Mensaje SaveCabeceraPedido(Pedido p)
        {
            try
            {
                Mensaje m = null;
                var db = dbFox;
                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();

                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_PROY_M_GUARDA_PEDIDOS_CAB";
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@pedidoId", SqlDbType.Int).Value = p.identity;
                    cmd.Parameters.Add("@fecha_pedido", SqlDbType.VarChar).Value = p.fechaPedido;
                    cmd.Parameters.Add("@fecha_entrega", SqlDbType.VarChar).Value = p.fechaEntrega;
                    cmd.Parameters.Add("@codigo_usuario", SqlDbType.VarChar).Value = p.codigoUsuario;
                    cmd.Parameters.Add("@codigo_cliente", SqlDbType.VarChar).Value = p.codigoCliente;
                    cmd.Parameters.Add("@nombre_cliente", SqlDbType.VarChar).Value = p.nombreCliente;
                    cmd.Parameters.Add("@forma_pago", SqlDbType.VarChar).Value = p.codformaPago;
                    cmd.Parameters.Add("@monto_total", SqlDbType.Decimal).Value = p.montoTotal;
                    cmd.Parameters.Add("@codigo_moneda", SqlDbType.VarChar).Value = p.codigoMoneda;
                    cmd.Parameters.Add("@codigo_emp_transporte", SqlDbType.VarChar).Value = p.codigoEmpTransporte;
                    cmd.Parameters.Add("@estado", SqlDbType.VarChar).Value = p.estado;
                    cmd.Parameters.Add("@observaciones", SqlDbType.VarChar).Value = p.observacion;
                    cmd.Parameters.Add("@tipo_precio", SqlDbType.Int).Value = p.tipoPrecio;
                    cmd.Parameters.Add("@punto_entrega", SqlDbType.VarChar).Value = p.puntoEntrega;
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {

                            m = new Mensaje
                            {
                                codigoBase = p.pedidoId,
                                codigoRetorno = dr.GetInt32(0),
                                codigoPedido = dr.GetString(1),
                                mensaje = "Guardado"
                            };
                             

                            foreach (var d in p.detalles)
                            {
                                SqlCommand cmdD = con.CreateCommand();
                                cmdD.CommandType = CommandType.StoredProcedure;
                                cmdD.CommandText = "SP_PROY_M_GUARDA_PEDIDOS_DET";
                                cmdD.Parameters.Add("@pedidoDetalleId", SqlDbType.Int).Value = d.identityDetalle;
                                cmdD.Parameters.Add("@id_cab", SqlDbType.Int).Value = m.codigoRetorno;
                                cmdD.Parameters.Add("@item", SqlDbType.Int).Value = d.item;
                                cmdD.Parameters.Add("@cod_producto", SqlDbType.VarChar).Value = d.codigoProducto;
                                cmdD.Parameters.Add("@descripcion_producto", SqlDbType.VarChar).Value = d.descripcionProducto;
                                cmdD.Parameters.Add("@cantidad", SqlDbType.Decimal).Value = d.cantidad;
                                cmdD.Parameters.Add("@precio", SqlDbType.Decimal).Value = d.precio;
                                cmdD.Parameters.Add("@unidad", SqlDbType.VarChar).Value = d.unidad;
                                cmdD.Parameters.Add("@por_descuento", SqlDbType.Decimal).Value = d.porDescuento;


                                SqlDataReader drR = cmdD.ExecuteReader();
                                if (drR.HasRows)
                                {
                                    List<MensajeDetalle> list = new List<MensajeDetalle>();
                                    while (drR.Read())
                                    {
                                        list.Add(new MensajeDetalle()
                                        {
                                            detalleId = d.pedidoDetalleId,
                                            detalleRetornoId = drR.GetInt32(0),
                                        });
                                    } 
                                    m.detalle = list;
                                }
                            }
                        }
                    }

                    con.Close();
                }
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Mensaje SaveGps(Gps e)
        {
            try
            {
                Mensaje m = null;
                var db =  dbFox;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_PROY_M_GUARDA_PUNTOS_CONTACTO";
                    cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar).Value = e.usuarioId;
                    cmd.Parameters.Add("@FECHA", SqlDbType.VarChar).Value = e.fecha;
                    cmd.Parameters.Add("@LATITUD", SqlDbType.VarChar).Value = e.latitud;
                    cmd.Parameters.Add("@LONGITUD", SqlDbType.VarChar).Value = e.longitud;
                    int a = cmd.ExecuteNonQuery();
                    if (a == 1)
                    {
                        m = new Mensaje
                        {
                            codigoBase = e.id,
                            mensaje = "Enviado"
                        };
                    }
                    cn.Close();
                }
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}