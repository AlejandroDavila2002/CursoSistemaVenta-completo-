using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class FormaPago
    {
        public int IdFormaPago { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public string FechaRegistro { get; set; }
    }
}

