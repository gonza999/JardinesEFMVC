﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JardinesEf.Entidades.Entidades;
using JardinesEF.Servicios.Facades;
using JardinesEF.Web.Clases;
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

    }
}