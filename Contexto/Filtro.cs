using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contexto
{
    public class Filtro
    {
        public int usuarioId { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public string search { get; set; }
        public string pass { get; set; }
        public string newPass { get; set; }
        public string confirmPass { get; set; }
        public string imei { get; set; }
        public string version { get; set; }      
        public string codUsuario { get; set; } 
        public int tipo { get; set; } 
        public int clienteId { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public string codPedido { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFinal { get; set; }
        public int precioCambiado { get; set; } 
        public int syncPedidos { get; set; } 
    }
}
