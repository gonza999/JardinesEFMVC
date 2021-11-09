using JardinesEF.Servicios.Facades;
using JardinesEF.Web.Clases;
using JardinesEF.Web.Models.Cliente;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace JardinesEF.Web.Controllers
{
    public class ClientesController : Controller
    {
        // GET: Clientes
        private readonly IClientesServicios _servicio;
        private readonly IPaisesServicios _servicioPaises;
        private readonly ICiudadesServicios _servicioCiudades;


        private readonly int cantidadPorPagina = 12;

        public ClientesController(IClientesServicios servicio,ICiudadesServicios servicioCiudades,
            IPaisesServicios servicioPaises)
        {
            _servicio=servicio;
            _servicioCiudades = servicioCiudades;
            _servicioPaises = servicioPaises;
        }
        public ActionResult Index(int? page=null)
        {
            page = (page ?? 1);

            var lista = _servicio.GetLista();
            var listaVm = Mapeador.ConstruirListaClienteVm(lista);
            return View(listaVm.ToPagedList((int)page, cantidadPorPagina));
        }

        [HttpGet]
        public ActionResult Create()
        {
            ClienteEditVm clienteEditVm = new ClienteEditVm()
            {
                Paises = Mapeador.ConstruirListaPaisesVm(_servicioPaises.GetLista()),
                Ciudades=Mapeador.ConstruirListaCiudadVm(_servicioCiudades.GetLista())
            };
            return View(clienteEditVm);
        }

        [HttpPost]

        public ActionResult Create(ClienteEditVm clienteEditVm)
        {
            if (!ModelState.IsValid)
            {
                clienteEditVm.Paises = Mapeador.ConstruirListaPaisesVm(_servicioPaises.GetLista());
                clienteEditVm.Ciudades = Mapeador.ConstruirListaCiudadVm(_servicioCiudades.GetLista(clienteEditVm.PaisId));
                return View(clienteEditVm);
            }
            var cliente = Mapeador.ConstruirCliente(clienteEditVm);
            try
            {
                if (_servicio.Existe(cliente))
                {
                    ModelState.AddModelError(string.Empty,"Cliente repetido");
                    clienteEditVm.Paises = Mapeador.ConstruirListaPaisesVm(_servicioPaises.GetLista());
                    clienteEditVm.Ciudades = Mapeador.ConstruirListaCiudadVm(_servicioCiudades.GetLista(clienteEditVm.PaisId));
                    return View(clienteEditVm);
                }
                _servicio.Guardar(cliente);
                TempData["Operacion"]=Operacion.Agregar;
                TempData["Msg"]="Cliente Agregado";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                clienteEditVm.Paises = Mapeador.ConstruirListaPaisesVm(_servicioPaises.GetLista());
                clienteEditVm.Ciudades = Mapeador.ConstruirListaCiudadVm(_servicioCiudades.GetLista(clienteEditVm.PaisId));
                return View(clienteEditVm);
            }
        }

        public JsonResult GetCities(int paisId)
        {
            //Database.Configuration.ProxyCreationEnabled = false;
            var ciudadesVm = Mapeador.ConstruirListaCiudadVm(_servicioCiudades.GetLista(paisId));
            return Json(ciudadesVm);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cliente = _servicio.GetEntityPorId(id.Value);
            if (cliente == null)
            {
                return new HttpNotFoundResult("Codigo de cliente inexistente");
            }
            var clienteEditVm = Mapeador.ConstruirClienteEditVm(cliente);
            clienteEditVm.Paises = Mapeador.ConstruirListaPaisesVm(_servicioPaises.GetLista());
            clienteEditVm.Ciudades = Mapeador.ConstruirListaCiudadVm(_servicioCiudades.GetLista(clienteEditVm.PaisId));

            return View(clienteEditVm);
        }

        [HttpPost]

        public ActionResult Edit(ClienteEditVm clienteEditVm)
        {
            if (!ModelState.IsValid)
            {
                clienteEditVm.Paises = Mapeador.ConstruirListaPaisesVm(_servicioPaises.GetLista());
                clienteEditVm.Ciudades = Mapeador.ConstruirListaCiudadVm(_servicioCiudades.GetLista(clienteEditVm.PaisId));
                return View(clienteEditVm);
            }
            var cliente = Mapeador.ConstruirCliente(clienteEditVm);
            try
            {
                if (_servicio.Existe(cliente))
                {
                    ModelState.AddModelError(string.Empty, "Cliente repetido");
                    clienteEditVm.Paises = Mapeador.ConstruirListaPaisesVm(_servicioPaises.GetLista());
                    clienteEditVm.Ciudades = Mapeador.ConstruirListaCiudadVm(_servicioCiudades.GetLista(clienteEditVm.PaisId));
                    return View(clienteEditVm);
                }
                _servicio.Guardar(cliente);
                TempData["Operacion"] = Operacion.Editar;
                TempData["Msg"] = "Cliente Editado";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                clienteEditVm.Paises = Mapeador.ConstruirListaPaisesVm(_servicioPaises.GetLista());
                clienteEditVm.Ciudades = Mapeador.ConstruirListaCiudadVm(_servicioCiudades.GetLista(clienteEditVm.PaisId));
                return View(clienteEditVm);
            }
        }

        [HttpGet]

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cliente = _servicio.GetEntityPorId(id.Value);
            if (cliente == null)
            {
                return new HttpNotFoundResult("Codigo de cliente inexistente");
            }
            var clienteVm = Mapeador.ConstruirClienteVm(cliente);
            return View(clienteVm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var cliente = _servicio.GetEntityPorId(id);
            if (_servicio.EstaRelacionado(cliente))
            {
                var clienteVm = Mapeador.ConstruirClienteVm(cliente);
                ModelState.AddModelError(string.Empty, "Cliente relacionado... baja denegada");
                return View(clienteVm);
            }

            try
            {
                _servicio.Borrar(id);
                TempData["Operacion"] = Operacion.Borrar;
                TempData["Msg"] = "Cliente Borrado";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var clienteVm = Mapeador.ConstruirClienteVm(cliente);
                ModelState.AddModelError(string.Empty, e.Message);
                return View(clienteVm);
            }
        }
    }
}