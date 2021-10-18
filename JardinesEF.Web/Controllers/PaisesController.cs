using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JardinesEf.Entidades.Entidades;
using JardinesEF.Servicios.Facades;
using JardinesEF.Web.Clases;
using JardinesEF.Web.Models.Ciudad;
using JardinesEF.Web.Models.Pais;
using Microsoft.Ajax.Utilities;

namespace JardinesEF.Web.Controllers
{
    public class PaisesController : Controller
    {
        private readonly IPaisesServicios _servicio;
        private readonly ICiudadesServicios _servicioCiudades;

        public PaisesController(IPaisesServicios servicio,ICiudadesServicios servicioCiudades)
        {
            _servicio = servicio;
            _servicioCiudades=servicioCiudades;
        }
        // GET: Paises
        public ActionResult Index()
        {
            var listaPaises = _servicio.GetLista();
            var listaPaisVm = Mapeador.ConstruirListaPaisesVm(listaPaises);
            foreach (var paisVm in listaPaisVm)
            {
                paisVm.CantidadCiudades = _servicioCiudades.GetCantidad(c=>c.PaisId==paisVm.PaisId);
            }
            return View(listaPaisVm);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PaisEditVm paisVm)
        {
            if (!ModelState.IsValid)
            {
                return View(paisVm);
            }

            var pais = Mapeador.ConstruirPais(paisVm);

            try
            {
                if (_servicio.Existe(pais))
                {
                    ModelState.AddModelError(string.Empty,"País existente!!!");
                    return View(paisVm);
                }
                _servicio.Guardar(pais);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty,e.Message);
                return View(paisVm);
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            var pais = _servicio.GetEntityPorId(id.Value);
            if (pais==null)
            {
                return new HttpNotFoundResult("Código de País inexistente!!!");
            }

            var paisVm = Mapeador.ConstruirPaisEditVm(pais);
            return View(paisVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PaisEditVm paisVm)
        {
            if (!ModelState.IsValid)
            {
                return View(paisVm);
            }

            var pais = Mapeador.ConstruirPais(paisVm);
            try
            {
                if (_servicio.Existe(pais))
                {
                    ModelState.AddModelError(string.Empty,"País existente!!!");
                    return View(paisVm);
                }
                _servicio.Guardar(pais);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View(paisVm);
            }
        }


        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var pais = _servicio.GetEntityPorId(id.Value);
            if (pais == null)
            {
                return new HttpNotFoundResult("Código de País inexistente!!!");
            }

            var paisVm = Mapeador.ConstruirPaisVm(pais);
            return View(paisVm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var pais = _servicio.GetEntityPorId(id);
            if (_servicio.EstaRelacionado(pais))
            {
                var paisVm = Mapeador.ConstruirPaisVm(pais);
                ModelState.AddModelError(string.Empty, "País relacionado... baja denegada");
                return View(paisVm);
            }

            try
            {
                _servicio.Borrar(id);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var paisVm = Mapeador.ConstruirPaisVm(pais);
                ModelState.AddModelError(string.Empty, e.Message);
                return View(paisVm);
            }
        }

        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pais = _servicio.GetEntityPorId(id.Value);
            if (pais==null)
            {
                return HttpNotFound("Codigo de Pais Inexistente");
            }
            var paisDetailsVm = Mapeador.ConstruirPaisDetailsVm(pais);
            paisDetailsVm.CantidadCiudades = _servicioCiudades.GetCantidad(c=>c.PaisId==paisDetailsVm.PaisId);
            paisDetailsVm.Ciudades =Mapeador.ConstruirListaCiudadVm( _servicioCiudades.Find(c=>c.PaisId==paisDetailsVm.PaisId,null,null));
            return View(paisDetailsVm);

        }

        public ActionResult AddCity(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pais = _servicio.GetEntityPorId(id.Value);
            if (pais == null)
            {
                return HttpNotFound("Codigo de Pais Inexistente");
            }
            var ciudadEditVm = new CiudadEditVm()
            {
                PaisId = pais.PaisId,
                Pais=Mapeador.ConstruirPaisVm(pais)
            };
            return View(ciudadEditVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCity(CiudadEditVm ciudadEditVm)
        {
            if (!ModelState.IsValid)
            {
                var pais = _servicio.GetEntityPorId(ciudadEditVm.PaisId);
                ciudadEditVm.Pais = Mapeador.ConstruirPaisVm(pais);
                return View(ciudadEditVm);
            }
            var ciudad = Mapeador.ConstruirCiudad(ciudadEditVm);
            try
            {
                if (_servicioCiudades.Existe(ciudad))
                {
                    var pais = _servicio.GetEntityPorId(ciudadEditVm.PaisId);
                    ciudadEditVm.Pais = Mapeador.ConstruirPaisVm(pais);
                    ModelState.AddModelError(string.Empty,"Ciudad existente");
                    return View(ciudadEditVm);
                }
                _servicioCiudades.Guardar(ciudad);
                return RedirectToAction($"Details/{ciudad.PaisId}");
            }
            catch (Exception e)
            {
                var pais = _servicio.GetEntityPorId(ciudadEditVm.PaisId);
                ciudadEditVm.Pais = Mapeador.ConstruirPaisVm(pais);
                ModelState.AddModelError(string.Empty, e.Message);
                return View(ciudadEditVm);
            }
        }

        public ActionResult DeleteCity(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ciudad = _servicioCiudades.GetEntityPorId(id.Value);
            if (ciudad == null)
            {
                return HttpNotFound("Codigo de Ciudad Inexistente");
            }
            var ciudadEditVm = Mapeador.ConstruirCiudadEditVm(ciudad);
            var paisVm = Mapeador.ConstruirPaisVm(ciudad.Pais);
            ciudadEditVm.Pais = paisVm;
            return View(ciudadEditVm);
        }

        [HttpPost,ActionName("DeleteCity")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCity(int id)
        {
            var ciudad = _servicioCiudades.GetEntityPorId(id);

            try
            {
                if (_servicioCiudades.EstaRelacionado(ciudad))
                {
                    var paisVm = Mapeador.ConstruirPaisVm(_servicio.GetEntityPorId(ciudad.PaisId));
                    var ciudadEditVm = Mapeador.ConstruirCiudadEditVm(ciudad);
                    ciudadEditVm.Pais = paisVm;
                    ModelState.AddModelError(string.Empty, "Ciudad Relacionada");
                    return View(ciudadEditVm);
                }
                _servicioCiudades.Borrar(ciudad.CiudadId);
                return RedirectToAction($"Details/{ciudad.PaisId}");
            }
            catch (Exception e)
            {
                var paisVm = Mapeador.ConstruirPaisVm(_servicio.GetEntityPorId(ciudad.PaisId));
                var ciudadEditVm = Mapeador.ConstruirCiudadEditVm(ciudad);
                ciudadEditVm.Pais = paisVm;
                ModelState.AddModelError(string.Empty, e.Message);
                return View(ciudadEditVm);
            }
        }
    }
}