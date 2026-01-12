using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class ReporteCompras
    {
        public string FechaRegistro { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string MontoTotal { get; set; }
        public string UsuarioRegistro { get; set; }
        public string DocumentoProveedor { get; set; }
        public string RazonSocial { get; set; }
        public string CodigoProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Categoria { get; set; }
        public string PrecioCompra { get; set; }
        public string PrecioVenta { get; set; }
        public string Cantidad { get; set; }
        public string SubTotal { get; set; }

        // --- NUEVAS PROPIEDADES ---
        public string TasaCambio { get; set; }
        public string EsCompraEnBs { get; set; } // Lo manejamos como string para reporte
        public string PrecioCompraBs { get; set; }
        public string PrecioVentaBs { get; set; }
        public string SubTotalBs { get; set; }
        public string MontoTotalBs { get; set; }
    }
}
