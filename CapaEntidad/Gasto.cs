using System;

namespace CapaEntidad
{
    public class Gasto
    {
        public int IdGasto { get; set; }
        public Usuario oUsuario { get; set; }
        public CategoriaGasto oCategoriaGasto { get; set; } 
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public string Referencia { get; set; }
        public string FormaPago { get; set; }
        public string FechaRegistro { get; set; }
    }
}