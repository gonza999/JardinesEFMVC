using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JardinesEF.Web.Models.Producto
{
    public class ProductoListVm
    {
        public int ProductoId { get; set; }

        [Display(Name = "Producto")]
        public string NombreProducto { get; set; }

        [Display(Name = "Latín")]
        public string NombreLatin { get; set; }
        [Display(Name = "Proveedor")]

        public string NombreProveedor { get; set; }
        [Display(Name = "Categoria")]

        public string NombreCategoria { get; set; }
        [Display(Name = "Precio")]

        public decimal PrecioUnitario { get; set; }
        [Display(Name = "Stock")]

        public int UnidadesEnStock { get; set; }

    }
}