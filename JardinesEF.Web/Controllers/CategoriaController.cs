using JardinesEf.Entidades.Entidades;
using JardinesEF.Servicios.Facades;
using JardinesEF.Web.Models.Categoria;
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

        public CategoriaController(ICategoriasServicios servicio)
        {
            _servicio = servicio;
        }
        // GET: Categoria
        public ActionResult Index()
        {
            var lista = _servicio.GetLista();
            var listaVm = ConstruirListaCategoriaVm(lista);
            return View(listaVm);
        }

        private List<CategoriaListVm> ConstruirListaCategoriaVm(List<Categoria> lista)
        {
            var listaVm = new List<CategoriaListVm>();
            foreach (var c in lista)
            {
                var categoriaVm = ConstruirCategoriaVm(c);
                listaVm.Add(categoriaVm);
            }
            return listaVm;
        }

        private CategoriaListVm ConstruirCategoriaVm(Categoria c)
        {
            return new CategoriaListVm()
            {
                CategoriaId = c.CategoriaId,
                NombreCategoria = c.NombreCategoria
            };
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
            var categoria = ContruirCategoria(categoriaEditVm);
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

        private Categoria ContruirCategoria(CategoriaEditVm categoriaEditVm)
        {
            return new Categoria()
            {
                CategoriaId = categoriaEditVm.CategoriaId,
                NombreCategoria = categoriaEditVm.NombreCategoria,
                Descripcion = categoriaEditVm.Descripcion
            };
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
            var categoriaEditVm = ConstruirCategoriaEditVm(categoria);
            return View(categoriaEditVm);
        }

        private CategoriaEditVm ConstruirCategoriaEditVm(Categoria categoria)
        {
            return new CategoriaEditVm()
            {
                CategoriaId = categoria.CategoriaId,
                NombreCategoria = categoria.NombreCategoria,
                Descripcion = categoria.Descripcion
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoriaEditVm categoriaEditVm)
        {
            if (!ModelState.IsValid)
            {
                return View(categoriaEditVm);
            }
            var categoria = ContruirCategoria(categoriaEditVm);
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
            var categoriaVm = ConstruirCategoriaVm(categoria);
            return View(categoriaVm);
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var categoria = _servicio.GetEntityPorId(id);
            var categoriaVm = ConstruirCategoriaVm(categoria);
            if (_servicio.EstaRelacionado(categoria))
            {
               
                ModelState.AddModelError(string.Empty,"Categoria relacionada");
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

    }
}