using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contexto
{
    public class Usuario
    {
        public string codUsuario { get; set; }
        public string pass { get; set; }
        public string nombreUsuario { get; set; }
        public string nivel { get; set; }
        public string estado { get; set; }
        public string codigoVendedor { get; set; }
        public string nombreVendedor { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public int itemPedido { get; set; }

        public string horaInicio { get; set; }
        public string horaFin { get; set; }
        public int intervalos { get; set; }
        public int tipo { get; set; }
    }
}
