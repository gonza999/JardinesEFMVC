using JardinesEf.Entidades.Entidades;
using JardinesEF.Servicios.Facades;
using JardinesEF.Web.Clases;
using JardinesEF.Web.Models.Categoria;
using JardinesEF.Web.Models.Ciudad;
using JardinesEF.Web.Models.Producto;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace JardinesEF.Web.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly ICategoriasServicios _servicio;
        private readonly IProductosServicios _servicioProductos;
        private readonly IProveedoresServicios _servicioProveedores;

        private readonly int cantidadPorPaginas=12;
        public CategoriaController(ICategoriasServicios servicio,
            IProductosServicios servicioProductos,IProveedoresServicios servicioProveedores)
        {
            _servicio = servicio;
            _servicioProductos = servicioProductos;
            _servicioProveedores=servicioProveedores;
        }
        // GET: Categoria
        public ActionResult Index(int? page=null)
        {
            page = (page ?? 1);
            var lista = _servicio.GetLista();
            var listaVm = Mapeador.ConstruirListaCategoriaVm(lista);
            foreach (var categoriaVm in listaVm)
            {
                categoriaVm.CantidadProductos = _servicioProductos.GetCantidad(p=>p.CategoriaId==categoriaVm.CategoriaId);
            }
            return View(listaVm.ToPagedList((int)page,cantidadPorPaginas));
        }

        [HttpGet]

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoriaEditVm categoriaEditVm)
        {
            if (!ModelState.IsValid)
            {
                return View(categoriaEditVm);
            }
            var categoria = Mapeador.ContruirCategoria(categoriaEditVm);
            try
            {
                if (_servicio.Existe(categoria))
                {
                    ModelState.AddModelError(string.Empty, "Categoria existente");
                    return View(categoriaEditVm);
                }
                _servicio.Guardar(categoria);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View(categoriaEditVm);
            }
        }


        [HttpGet]

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var categoria = _servicio.GetEntityPorId(id.Value);
            if (categoria == null)
            {
                return new HttpNotFoundResult("Codigo de Categoria inexistente");
            }
            var categoriaEditVm = Mapeador.ConstruirCategoriaEditVm(categoria);
            return View(categoriaEditVm);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoriaEditVm categoriaEditVm)
        {
            if (!ModelState.IsValid)
            {
                return View(categoriaEditVm);
            }
            var categoria = Mapeador.ContruirCategoria(categoriaEditVm);
            try
            {
                if (_servicio.Existe(categoria))
                {
                    ModelState.AddModelError(string.Empty, "Categoria existente");
                    return View(categoriaEditVm);
                }
                _servicio.Guardar(categoria);
                return RedirectToAction("Index");

            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View(categoriaEditVm);
            }
        }
        [HttpGet]

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var categoria = _servicio.GetEntityPorId(id.Value);
            if (categoria == null)
            {
                return new HttpNotFoundResult("Codigo de Categoria inexistente");
            }
            var categoriaVm = Mapeador.ConstruirCategoriaVm(categoria);
            return View(categoriaVm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var categoria = _servicio.GetEntityPorId(id);
            var categoriaVm = Mapeador.ConstruirCategoriaVm(categoria);
            if (_servicio.EstaRelacionado(categoria))
            {

                ModelState.AddModelError(string.Empty, "Categoria relacionada");
                return View(categoriaVm);
            }
            try
            {
                _servicio.Borrar(id);
                return RedirectToAction("Index");

            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View(categoriaVm);
            }
        }

        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var categoria = _servicio.GetEntityPorId(id.Value);
            if (categoria==null)
            {
                return HttpNotFound("Codigo de Categoria Inexistente");
            }
            var categoriaDetailsVm = Mapeador.ConstruirCategoriaDetailsVm(categoria);
            categoriaDetailsVm.CantidadProductos = _servicioProductos.GetCantidad(p=>p.CategoriaId==categoria.CategoriaId);
            categoriaDetailsVm.Productos =Mapeador.ConstruirListaProductosVm( _servicioProductos.Find(p=>p.CategoriaId==categoria.CategoriaId,null,null));
            return View(categoriaDetailsVm);
        }

        public ActionResult AddProducto(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var categoria = _servicio.GetEntityPorId(id.Value);
            if (categoria == null)
            {
                return HttpNotFound("Codigo de Categoria Inexistente");
            }
            var productoEditVm = new ProductoEditVm()
            {
                CategoriaId = categoria.CategoriaId,
                Categoria = Mapeador.ConstruirCategoriaVm(categoria)
            };
            productoEditVm.Proveedores = Mapeador.ConstruirListaProveedoresVm(_servicioProveedores.GetLista());
            return View(productoEditVm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProducto(ProductoEditVm productoEditVm)
        {
            if (!ModelState.IsValid)
            {
                var categoria = _servicio.GetEntityPorId(productoEditVm.CategoriaId);
                productoEditVm.Categoria = Mapeador.ConstruirCategoriaVm(categoria);
                productoEditVm.Proveedores = Mapeador.ConstruirListaProveedoresVm(_servicioProveedores.GetLista());
                return View(productoEditVm);
            }
            var producto = Mapeador.ConstruirProducto(productoEditVm);
            try
            {
                if (_servicioProductos.Existe(producto))
                {
                    var categoria = _servicio.GetEntityPorId(productoEditVm.CategoriaId);
                    productoEditVm.Categoria = Mapeador.ConstruirCategoriaVm(categoria);
                    productoEditVm.Proveedores = Mapeador.ConstruirListaProveedoresVm(_servicioProveedores.GetLista());
                    ModelState.AddModelError(string.Empty, "Producto existente");
                    return View(productoEditVm);
                }
                _servicioProductos.Guardar(producto);
                return RedirectToAction($"Details/{producto.CategoriaId}");
            }
            catch (Exception e)
            {
                var categoria = _servicio.GetEntityPorId(productoEditVm.CategoriaId);
                productoEditVm.Categoria = Mapeador.ConstruirCategoriaVm(categoria);
                productoEditVm.Proveedores = Mapeador.ConstruirListaProveedoresVm(_servicioProveedores.GetLista());
                ModelState.AddModelError(string.Empty, e.Message);
                return View(productoEditVm);
            }
        }

        public ActionResult DeleteProducto(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var producto = _servicioProductos.GetEntityPorId(id.Value);
            if (producto == null)
            {
                return HttpNotFound("Codigo de Producto Inexistente");
            }
            var productoEditVm = Mapeador.ConstruirProductoEditVm(producto);
            var categoriaVm = Mapeador.ConstruirCategoriaVm(producto.Categoria);
            productoEditVm.Categoria = categoriaVm;
            return View(productoEditVm);
        }

        [HttpPost, ActionName("DeleteProducto")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProducto(int id)
        {
            var producto = _servicioProductos.GetEntityPorId(id);

            try
            {
                if (_servicioProductos.EstaRelacionado(producto))
                {
                    var categoriaVm = Mapeador.ConstruirCategoriaVm(_servicio.GetEntityPorId(producto.CategoriaId));
                    var productoEditVm = Mapeador.ConstruirProductoEditVm(producto);
                    productoEditVm.Categoria = categoriaVm;
                    ModelState.AddModelError(string.Empty, "Producto Relacionado");
                    return View(productoEditVm);
                }
                _servicioProductos.Borrar(producto.ProductoId);
                return RedirectToAction($"Details/{producto.CategoriaId}");
            }
            catch (Exception e)
            {
                var categoriaVm = Mapeador.ConstruirCategoriaVm(_servicio.GetEntityPorId(producto.CategoriaId));
                var productoEditVm = Mapeador.ConstruirProductoEditVm(producto);
                productoEditVm.Categoria = categoriaVm;
                ModelState.AddModelError(string.Empty, e.Message);
                return View(productoEditVm);
            }
        }

        public ActionResult EditProducto(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var producto = _servicioProductos.GetEntityPorId(id.Value);
            if (producto == null)
            {
                return new HttpNotFoundResult("Codigo de Producto inexistente");
            }
            var productoEditVm = Mapeador.ConstruirProductoEditVm(producto);
            productoEditVm.Proveedores = Mapeador.ConstruirListaProveedoresVm(_servicioProveedores.GetLista());
            return View(productoEditVm);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProducto(ProductoEditVm productoEditVm)
        {
            if (!ModelState.IsValid)
            {
                productoEditVm.Proveedores = Mapeador.ConstruirListaProveedoresVm(_servicioProveedores.GetLista());
                return View(productoEditVm);
            }
            var producto = Mapeador.ConstruirProducto(productoEditVm);
            try
            {
                if (_servicioProductos.Existe(producto))
                {
                    ModelState.AddModelError(string.Empty, "Producto existente");
                    productoEditVm.Proveedores = Mapeador.ConstruirListaProveedoresVm(_servicioProveedores.GetLista());
                    return View(productoEditVm);
                }
                _servicioProductos.Guardar(producto);
                return RedirectToAction("Index");

            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                productoEditVm.Proveedores = Mapeador.ConstruirListaProveedoresVm(_servicioProveedores.GetLista());
                return View(productoEditVm);
            }
        }
    }
}