using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Mensaje
    {
        public int codigo { get; set; }
     

        public int codigoBase { get; set; }
        public int codigoRetorno { get; set; }
        public string codigoPedido { get; set; }
        public string mensaje { get; set; }
        public List<MensajeDetalle> detalle { get; set; }
    }

    public class MensajeDetalle
    {
        public int detalleId { get; set; }
        public int detalleRetornoId { get; set; }
        public string tipo { get; set; }
    }
}
