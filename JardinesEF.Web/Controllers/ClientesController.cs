using JardinesEF.Servicios.Facades;
using JardinesEF.Web.Clases;
using JardinesEF.Web.Models.Cliente;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}