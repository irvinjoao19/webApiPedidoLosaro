using Contexto;
using Entidades;
using Negocio;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace WebApiDsigeVentas.Controllers
{
    [RoutePrefix("api/Ventas")]
    public class MigrationController : ApiController
    {
        [HttpPost]
        [Route("Login")]
        public IHttpActionResult GetLogin(Filtro f)
        {
            try
            {
                Usuario u = MigrationDao.GetLogin(f);
                if (u != null)
                {
                    if (u.pass == "Error")
                        return BadRequest("Contraseña Incorrecta");
                    else
                        return Ok(u);
                }
                else return BadRequest("Usuario no existe");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        [Route("Logout")]
        public IHttpActionResult Logout(Filtro f)
        {
            try
            {
                Mensaje m = MigrationDao.GetLogout(f);
                if (m != null)
                    return Ok(m);
                else return BadRequest("Error");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        [Route("Sync")]
        public IHttpActionResult Sync(Filtro f)
        {
            try
            {
                Sync m = MigrationDao.GetSync(f);
                if (m.mensaje != "Update")
                    return Ok(m);
                else return BadRequest("Actualizar Versión del Aplicativo.");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        [Route("SaveCliente")]
        public IHttpActionResult SaveCliente(Cliente c)
        {
            Mensaje m = MigrationDao.SaveCliente(c);
            if (m != null)
                return Ok(m);
            else return BadRequest("Error de Envio");
        }

        [HttpPost]
        [Route("UpdateCoordenadas")]
        public IHttpActionResult UpdateCoordenadas(Filtro f)
        {
            try
            {
                Mensaje m = MigrationDao.UpdateCoordenadas(f);
                if (m != null)
                    return Ok(m);
                else return BadRequest("Error..");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        [Route("GetProductos")]
        public IHttpActionResult GetProductos(int tipo, int tipoPrecio)
        {
            List<Producto> p = MigrationDao.GetProductos(tipo, tipoPrecio);
            if (p != null)
                return Ok(p);
            else return BadRequest("No hay datos");
        }

        [HttpGet]
        [Route("GetPedidos")]
        public IHttpActionResult GetPedidos(int tipo, string codUsuario, string fechaInicio, string fechaFinal, int precioCambiado)
        {
            List<Pedido> p = MigrationDao.GetPedidos(tipo, codUsuario, fechaInicio, fechaFinal, precioCambiado);
            if (p != null)
                return Ok(p);
            else return BadRequest("No hay datos");
        }

        [HttpPost]
        [Route("SaveCabeceraPedido")]
        public IHttpActionResult SaveCabeceraPedido(Pedido p)
        {
            Mensaje m = MigrationDao.SaveCabeceraPedido(p);
            if (m != null)
                return Ok(m);
            else return BadRequest("Error de Envio");
        }

        [HttpPost]
        [Route("ClienteCredito")]
        public IHttpActionResult ClienteCredito(Filtro f)
        {
            ClienteCredito c = MigrationDao.GetClienteCredito(f);
            if (c != null)
                return Ok(c);
            else return BadRequest("No hay data de credito");
        }

        [HttpPost]
        [Route("ChangePass")]
        public IHttpActionResult ChangePass(Filtro f)
        {
            Mensaje m = MigrationDao.ChangePass(f);
            if (m != null)
                return Ok(m);
            else return BadRequest("Error");
        }

        [HttpPost]
        [Route("PedidoFacturacion")]
        public IHttpActionResult PedidoFacturacion(Filtro f)
        {
            List<PedidoFacturacion> p = MigrationDao.GetPedidoFacturacion(f);
            if (p != null)
                return Ok(p);
            else return BadRequest("No hay datos de facturación");
        }

        [HttpPost]
        [Route("EstadoCuenta")]
        public IHttpActionResult EstadoCuenta(Filtro f)
        {
            List<EstadoCuenta> c = MigrationDao.GetEstadoCuenta(f);
            if (c != null)
                return Ok(c);
            else return BadRequest("No hay datos de cuenta");
        }

        [HttpPost]
        [Route("SaveClientVisit")]
        public IHttpActionResult SaveClientVisit(ClientVisit c)
        {
            Mensaje m = MigrationDao.SaveClientVisit(c);
            if (m != null)
                return Ok(m);
            else return BadRequest("Error");
        }

        [HttpGet]
        [Route("GetPuntoContacto")]
        public IHttpActionResult GetPuntoContacto(string code)
        {
            List<PuntoContacto> m = MigrationDao.GetPuntoContacto(code);
            if (m != null)
                return Ok(m);
            else return BadRequest("No hay datos");
        }

        [HttpPost]
        [Route("SaveGps")]
        public IHttpActionResult SaveOperarioGps(Gps gps)
        {
            Mensaje m = MigrationDao.SaveGps(gps);
            if (m != null)
                return Ok(m);
            else return BadRequest("Error al enviar");
        }

    }
}
