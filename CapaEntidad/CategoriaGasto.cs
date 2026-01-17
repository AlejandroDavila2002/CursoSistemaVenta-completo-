namespace CapaEntidad
{
    // Debe ser pública para que las otras capas la vean
    public class CategoriaGasto
    {
        public int IdCategoriaGasto { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public string FechaRegistro { get; set; }
    }
}