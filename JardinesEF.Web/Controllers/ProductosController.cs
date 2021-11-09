using JardinesEf.Entidades.Entidades;
using JardinesEF.Servicios.Facades;
using JardinesEF.Web.Clases;
using JardinesEF.Web.Models.Producto;
using PagedList;
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
        private readonly int cantidadPorPagina = 12;
        public ProductosController(IProductosServicios servicio, ICategoriasServicios servicioCategorias,
            IProveedoresServicios servicioProveedores)
        {
            _servicio = servicio;
            _servicioCategorias = servicioCategorias;
            _servicioProveedores = servicioProveedores;
        }
        public ActionResult Index(int? categoriaSeleccionadaId=null,int? page=null)
        {
            page=(page??1);
            if (categoriaSeleccionadaId != null)
            {
                Session["categoriaSeleccionadaId"] = categoriaSeleccionadaId;
            }
            else
            {
                if (Session["categoriaSeleccionadaId"] != null)
                {
                    categoriaSeleccionadaId = (int)Session["categoriaSeleccionadaId"];
                }
            }

            List<Producto> lista;
            if (categoriaSeleccionadaId!=null)
            {
                if (categoriaSeleccionadaId.Value>0)
                {
                     lista = _servicio.GetLista(categoriaSeleccionadaId.Value);
                }
                else
                {
                    lista = _servicio.GetLista();
                }
            }
            else
            {
                lista = _servicio.GetLista();
            }
            var listaVm = Mapeador.ConstruirListaProductosVm(lista);
            var listaCategorias = _servicioCategorias.GetLista();
            listaCategorias.Insert(0, new Categoria() { CategoriaId = 0, NombreCategoria = "[Seleccione un Categoria]" });
            listaCategorias.Insert(1, new Categoria() { CategoriaId = -1, NombreCategoria = "[Ver Todos]" });

            ViewBag.Categorias = new SelectList(listaCategorias, "CategoriaId", "NombreCategoria", categoriaSeleccionadaId);

            return View(listaVm.ToPagedList((int)page,cantidadPorPagina));
        }
    }
}