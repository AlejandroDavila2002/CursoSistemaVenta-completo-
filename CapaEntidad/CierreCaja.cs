using System;

namespace CapaEntidad
{
    public class CierreCaja
    {
        public int IdCierre { get; set; }
        public Usuario oUsuario { get; set; }
        public decimal MontoTeorico { get; set; }
        public decimal MontoReal { get; set; }
        public string Observacion { get; set; }
        // La diferencia la calcula la BD, pero puedes ponerla aquí si quieres mostrarla
    }
}