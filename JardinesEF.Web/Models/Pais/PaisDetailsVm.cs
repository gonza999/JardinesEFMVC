using JardinesEF.Web.Models.Ciudad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JardinesEF.Web.Models.Pais
{
    public class PaisDetailsVm
    {
        public int PaisId { get; set; }

        [Display(Name ="Pais")]
        public string NombrePais { get; set; }

        [Display(Name = "Cant. Ciudades")]

        public int CantidadCiudades { get; set; }

        public List<CiudadListVm> Ciudades { get; set; }
    }
}