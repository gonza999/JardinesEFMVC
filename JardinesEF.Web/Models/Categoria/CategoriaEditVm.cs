using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JardinesEF.Web.Models.Categoria
{
    public class CategoriaEditVm
    {
        public int CategoriaId { get; set; }

        [Display(Name ="Categoria")]
        [Required(ErrorMessage ="El campo {0} es requerido")]
        [StringLength(100,ErrorMessage ="El campo {0} debe contener entre {2} y {1} caracteres",MinimumLength =3)]
        public string NombreCategoria { get; set; }

        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(255, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres", MinimumLength = 5)]
        public string Descripcion { get; set; }
    }
}