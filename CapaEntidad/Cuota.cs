


namespace CapaEntidad
{
    public class Cuota
    {
        public int IdCuota { get; set; }
        public int IdVenta { get; set; }
        public int NumeroCuota { get; set; }
        public string FechaProgramada { get; set; } // Fecha Vencimiento
        public decimal MontoCuota { get; set; }
        public string Estado { get; set; }
        public string FechaPago { get; set; }       // Fecha Real de Pago
    }
}