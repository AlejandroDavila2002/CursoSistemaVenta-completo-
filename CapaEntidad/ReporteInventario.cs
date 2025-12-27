using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class ReporteInventario
    {
            // Datos para dgvDataProducto
            public string Codigo { get; set; }
            public string Producto { get; set; }
            public int Stock { get; set; }
            public string Categoria { get; set; } // Útil para el cboBusquedaCategorias

            // Datos para dgvDataCosto
            public decimal CostoUnitario { get; set; } // Equivale a PrecioCompra
            public decimal TotalCosto { get; set; }   // Calculado: Stock * CostoUnitario

            // Datos para dvgDataVenta
            public decimal PrecioVenta { get; set; }
            public decimal TotalVenta { get; set; }   // Calculado: Stock * PrecioVenta

            // Datos para dgvDataAccion y Estado
            public string Estado { get; set; }

    }
}
