using JardinesEf.Entidades.Entidades;
using JardinesEF.Servicios.Facades;
using JardinesEF.Web.Clases;
using JardinesEF.Web.Models.Ciudad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace JardinesEF.Web.Controllers
{
    public class CiudadesController : Controller
    {
        private readonly ICiudadesServicios _servicio;
        private readonly IPaisesServicios _serviciosPaises;
        public CiudadesController(ICiudadesServicios servicio,IPaisesServicios servicioPais)
        {
            _servicio=servicio;
            _serviciosPaises = servicioPais;
        }
        // GET: Ciudades
        public ActionResult Index()
        {
            List<Ciudad> listaCiudades = _servicio.GetLista();
            var listaVm = Mapeador.ConstruirListaCiudadVm(listaCiudades);
            return View(listaVm);
        }
        public ActionResult Index2(int pagina = 1)
        {
            var cantidadDeRegistrosPorPagina = 10;
            var cantidadDeRegistros = _servicio.GetCantidad();
            var totalPaginas = (int)Math.Ceiling((double)cantidadDeRegistros / cantidadDeRegistrosPorPagina);
            var ciudades = _servicio.GetLista(cantidadDeRegistrosPorPagina,pagina);
            var ciudadesVm = Mapeador.ConstruirListaCiudadVm(ciudades);

            var paginador = new Listador<CiudadListVm>()
            {
                RegistrosPorPagina = cantidadDeRegistrosPorPagina,
                TotalPaginas = totalPaginas,
                PaginaActual = pagina,
                TotalRegistros = cantidadDeRegistros,
                Registros = ciudadesVm

            };

            return View(paginador);
            //return View("Index3",paginador);

        }
        public ActionResult Index3(int pagina = 1)
        {
            var cantidadDeRegistrosPorPagina = 10;
            var cantidadDeRegistros = _servicio.GetCantidad();
            var totalPaginas = (int)Math.Ceiling((double)cantidadDeRegistros / cantidadDeRegistrosPorPagina);
            var ciudades = _servicio.GetLista(cantidadDeRegistrosPorPagina, pagina);
            var ciudadesVm = Mapeador.ConstruirListaCiudadVm(ciudades);

            var paginador = new Listador<CiudadListVm>()
            {
                RegistrosPorPagina = cantidadDeRegistrosPorPagina,
                TotalPaginas = totalPaginas,
                PaginaActual = pagina,
                TotalRegistros = cantidadDeRegistros,
                Registros = ciudadesVm

            };

            return View(paginador);

        }

        [HttpGet]
        public ActionResult Create()
        {
            var ciudadEditVm = new CiudadEditVm()
            {
                Paises=Mapeador.ConstruirListaPaisesVm(_serviciosPaises.GetLista())
            };
            return View(ciudadEditVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CiudadEditVm ciudadEditVm)
        {
            if (!ModelState.IsValid)
            {
                ciudadEditVm.Paises = Mapeador.ConstruirListaPaisesVm(_serviciosPaises.GetLista());
                return View(ciudadEditVm);
            }
            var ciudad = Mapeador.ConstruirCiudad(ciudadEditVm);
            try
            {
                if (_servicio.Existe(ciudad))
                {
                    ModelState.AddModelError(string.Empty,"Ciudad Existente");
                    ciudadEditVm.Paises = Mapeador.ConstruirListaPaisesVm(_serviciosPaises.GetLista());
                    return View(ciudadEditVm);
                }
                _servicio.Guardar(ciudad);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty,e.Message);
                ciudadEditVm.Paises = Mapeador.ConstruirListaPaisesVm(_serviciosPaises.GetLista());
                return View(ciudadEditVm);
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ciudad = _servicio.GetEntityPorId(id.Value);
            if (ciudad==null)
            {
                return HttpNotFound("Codigo de ciudad inexistente");
            }
            var ciudadEditVm = Mapeador.ConstruirCiudadEditVm(ciudad);
            ciudadEditVm.Paises = Mapeador.ConstruirListaPaisesVm(_serviciosPaises.GetLista());
            return View(ciudadEditVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(CiudadEditVm ciudadEditVm)
        {
            if (!ModelState.IsValid)
            {
                ciudadEditVm.Paises = Mapeador.ConstruirListaPaisesVm(_serviciosPaises.GetLista());
                return View(ciudadEditVm);
            }
            var ciudad = Mapeador.ConstruirCiudad(ciudadEditVm);
            try
            {
                if (_servicio.Existe(ciudad))
                {
                    ModelState.AddModelError(string.Empty,"Ciudad Existente");
                    ciudadEditVm.Paises = Mapeador.ConstruirListaPaisesVm(_serviciosPaises.GetLista());
                    return View(ciudadEditVm);
                }
                _servicio.Guardar(ciudad);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                ciudadEditVm.Paises = Mapeador.ConstruirListaPaisesVm(_serviciosPaises.GetLista());
                return View(ciudadEditVm);
            }
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ciudad = _servicio.GetEntityPorId(id.Value);
            if (ciudad == null)
            {
                return HttpNotFound("Codigo de ciudad inexistente");
            }
            var ciudadVm = Mapeador.ConstruirCiudadVm(ciudad);
            return View(ciudadVm);
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var ciudad = _servicio.GetEntityPorId(id);
            try
            {
                if (_servicio.EstaRelacionado(ciudad))
                {
                    var ciudadVm = Mapeador.ConstruirCiudadVm(ciudad);
                    ModelState.AddModelError(string.Empty, "Ciudad Relacionada");
                    return View(ciudadVm);
                }
                _servicio.Borrar(id);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var ciudadVm = Mapeador.ConstruirCiudadVm(ciudad);
                ModelState.AddModelError(string.Empty, e.Message);
                return View(ciudadVm);
            }
        }
    }
}