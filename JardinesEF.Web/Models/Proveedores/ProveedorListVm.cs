using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JardinesEF.Web.Models.Proveedores
{
    public class ProveedorListVm
    {
        public int ProveedorId { get; set; }

        [Display(Name = "Proveedor")]
        public string NombreProveedor { get; set; }
        public string Direccion { get; set; }
        public string CodigoPostal { get; set; }

        [Display(Name = "Ciudad")]
        public string NombreCiudad { get; set; }

        [Display(Name = "Cant. Productos")]
        public int CantidadProductos { get; set; }
    }
}