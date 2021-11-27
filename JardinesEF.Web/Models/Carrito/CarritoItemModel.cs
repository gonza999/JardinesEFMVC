using JardinesEF.Web.Models.Producto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JardinesEF.Web.Models.Carrito
{
    public class CarritoItemModel
    {
        public ProductoListVm Producto { get; set; }
        public int Cantidad { get; set; }

    }

}