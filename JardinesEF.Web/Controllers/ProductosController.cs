using JardinesEf.Entidades.Entidades;
using JardinesEF.Servicios.Facades;
using JardinesEF.Web.Clases;
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
    public class ProductosController : Controller
    {
        private readonly IProductosServicios _servicio;
        private readonly ICategoriasServicios _servicioCategorias;
        private readonly IProveedoresServicios _servicioProveedores;
        private readonly string folder = "~/Content/Imagenes/Productos/";
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

        public ActionResult Create()
        {
            var productoEditVm = new ProductoEditVm()
            {
                Proveedores = Mapeador.ConstruirListaProveedoresVm(_servicioProveedores.GetLista()),
                Categorias = Mapeador.ConstruirListaCategoriaVm(_servicioCategorias.GetLista()),
                Imagen= "SinImagenDisponible.png"
            };
            return View(productoEditVm);
        }

        [HttpPost]

        public ActionResult Create(ProductoEditVm productoEditVm)
        {
            if (!ModelState.IsValid)
            {
                productoEditVm.Proveedores = Mapeador.ConstruirListaProveedoresVm(_servicioProveedores.GetLista());
                productoEditVm.Categorias = Mapeador.ConstruirListaCategoriaVm(_servicioCategorias.GetLista());
                productoEditVm.Imagen = "SinImagenDisponible.png";
                return View(productoEditVm);
            }
            var producto = Mapeador.ConstruirProducto(productoEditVm);
            try
            {
                if (_servicio.Existe(producto))
                {
                    ModelState.AddModelError(string.Empty,"Producto existente");
                    productoEditVm.Proveedores = Mapeador.ConstruirListaProveedoresVm(_servicioProveedores.GetLista());
                    productoEditVm.Categorias = Mapeador.ConstruirListaCategoriaVm(_servicioCategorias.GetLista());
                    productoEditVm.Imagen = "SinImagenDisponible.png";
                    return View(productoEditVm);
                }
                if (productoEditVm.ImagenFile != null)
                {
                    producto.Imagen = $"{productoEditVm.ImagenFile.FileName}";
                }
                _servicio.Guardar(producto);
                if (productoEditVm.ImagenFile != null)
                {
                    var file = $"{productoEditVm.ImagenFile.FileName}";
                    var response = FileHelper.UploadPhoto(productoEditVm.ImagenFile, folder, file);
                }
                //_servicio.Guardar(producto);
                TempData["operacion"] = Operacion.Agregar;
                TempData["Msg"] = "Producto agregado!!!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty,ex.Message);
                productoEditVm.Proveedores = Mapeador.ConstruirListaProveedoresVm(_servicioProveedores.GetLista());
                productoEditVm.Categorias = Mapeador.ConstruirListaCategoriaVm(_servicioCategorias.GetLista());
                productoEditVm.Imagen = "SinImagenDisponible.png";
                return View(productoEditVm);
            }
        }

        [HttpGet]

        public ActionResult Edit(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var producto = _servicio.GetEntityPorId(id.Value);
            if (producto==null)
            {
                return HttpNotFound("Codigo de Producto no existente");
            }
            var productoEditVm = Mapeador.ConstruirProductoEditVm(producto);
            productoEditVm.Proveedores = Mapeador.ConstruirListaProveedoresVm(_servicioProveedores.GetLista());
            productoEditVm.Categorias = Mapeador.ConstruirListaCategoriaVm(_servicioCategorias.GetLista());
            if (productoEditVm.Imagen==null)
            {
                productoEditVm.Imagen = "SinImagenDisponible.png";
            }
            return View(productoEditVm);
        }

        [HttpPost]

        public ActionResult Edit(ProductoEditVm productoEditVm)
        {
            if (!ModelState.IsValid)
            {
                productoEditVm.Proveedores = Mapeador.ConstruirListaProveedoresVm(_servicioProveedores.GetLista());
                productoEditVm.Categorias = Mapeador.ConstruirListaCategoriaVm(_servicioCategorias.GetLista());
                if (productoEditVm.Imagen == null)
                {
                    productoEditVm.Imagen = "SinImagenDisponible.png";
                }
                return View(productoEditVm);
            }
            var producto = Mapeador.ConstruirProducto(productoEditVm);
            try
            {
                if (_servicio.Existe(producto))
                {
                    ModelState.AddModelError(string.Empty, "Producto existente");
                    productoEditVm.Proveedores = Mapeador.ConstruirListaProveedoresVm(_servicioProveedores.GetLista());
                    productoEditVm.Categorias = Mapeador.ConstruirListaCategoriaVm(_servicioCategorias.GetLista());
                    if (productoEditVm.Imagen == null)
                    {
                        productoEditVm.Imagen = "SinImagenDisponible.png";
                    }
                    return View(productoEditVm);
                }
                if (productoEditVm.ImagenFile != null)
                {
                    producto.Imagen = $"{productoEditVm.ImagenFile.FileName}";
                }
                _servicio.Guardar(producto);
                if (productoEditVm.ImagenFile != null)
                {
                    var file = $"{productoEditVm.ImagenFile.FileName}";
                    var response = FileHelper.UploadPhoto(productoEditVm.ImagenFile, folder, file);
                }
                TempData["operacion"] = Operacion.Editar;
                TempData["Msg"] = "Producto Editado!!!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty,ex.Message);
                productoEditVm.Proveedores = Mapeador.ConstruirListaProveedoresVm(_servicioProveedores.GetLista());
                productoEditVm.Categorias = Mapeador.ConstruirListaCategoriaVm(_servicioCategorias.GetLista());
                if (productoEditVm.Imagen == null)
                {
                    productoEditVm.Imagen = "SinImagenDisponible.png";
                }
                return View(productoEditVm);
            }
        }
    }
}