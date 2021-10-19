using JardinesEF.Web.Models.Categoria;
using JardinesEF.Web.Models.Pais;
using JardinesEF.Web.Models.Proveedores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JardinesEF.Web.Models.Producto
{
    public class ProductoEditVm
    {
        public int ProductoId { get; set; }

        [Display(Name = "Producto")]
        public string NombreProducto { get; set; }

        [Display(Name = "Latín")]
        public string NombreLatin { get; set; }

        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoria")]
        public int CategoriaId { get; set; }

        public CategoriaListVm Categoria { get; set; }
        public List<CategoriaListVm> Categorias { get; set; }

        [Display(Name = "Proveedor")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una proveedor")]
        public int ProveedorId { get; set; }

        public ProveedorListVm Proveedor { get; set; }
        public List<ProveedorListVm> Proveedores { get; set; }

        [Display(Name = "Precio")]
        [Range(0,100000,ErrorMessage ="El precio debe estar entre el rango de 0 y 100000")]
        public decimal PrecioUnitario { get; set; }
        [Display(Name = "Stock")]

        [Range(0, 100000, ErrorMessage = "El stock debe estar entre el rango de 0 y 100000")]
        public int UnidadesEnStock { get; set; }
    }
}