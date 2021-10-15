using JardinesEf.Entidades.Entidades;
using JardinesEF.Servicios.Facades;
using JardinesEF.Web.Clases;
using JardinesEF.Web.Models.Producto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JardinesEF.Web.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductosServicios _servicio;
        private readonly ICategoriasServicios _servicioCategorias;
        private readonly IProveedoresServicios _servicioProveedores;
        // GET: Productos

        public ProductosController(IProductosServicios servicio, ICategoriasServicios servicioCategorias,
            IProveedoresServicios servicioProveedores)
        {
            _servicio = servicio;
            _servicioCategorias = servicioCategorias;
            _servicioProveedores = servicioProveedores;
        }
        public ActionResult Index()
        {
            List<Producto> listaProductos = _servicio.GetLista();
            var listaVm = Mapeador.ConstruirListaProductosVm(listaProductos);
            return View(listaVm);
        }
    }
}