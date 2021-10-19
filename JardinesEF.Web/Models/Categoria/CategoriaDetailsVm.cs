using JardinesEF.Web.Models.Producto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JardinesEF.Web.Models.Categoria
{
    public class CategoriaDetailsVm
    {
        public int CategoriaId { get; set; }

        [Display(Name ="Categoria")]
        public string NombreCategoria { get; set; }

        [Display(Name = "Cant. Productos")]

        public int CantidadProductos { get; set; }

        public List<ProductoListVm> Productos { get; set; }
    }
}