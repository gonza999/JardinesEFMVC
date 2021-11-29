using JardinesEF.Servicios.Facades;
using JardinesEF.Web.Clases;
using JardinesEF.Web.Models.Carrito;
using JardinesEF.Web.Models.Producto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JardinesEF.Web.Controllers
{
    public class CarritoController : Controller
    {
        private readonly IProductosServicios _servicio;
        private readonly IPaisesServicios _servicioPaises;
        private readonly IClientesServicios _servicioClientes;
        private readonly ICiudadesServicios _servicioCiudades;

        public CarritoController(IProductosServicios servicio,IPaisesServicios servicioPaises,
            IClientesServicios servicioClientes,ICiudadesServicios servicioCiudades)
        {
            _servicio = servicio;
            _servicioPaises = servicioPaises;
            _servicioCiudades = servicioCiudades;
            _servicioClientes = servicioClientes;
        }
        // GET: Carrito
        public CarritoController()
        {
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CarritoViewModel
            {
                Carrito = GetCart(),
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToCart(int productoId, string returnUrl)
        {
            ProductoListVm productoVm = Mapeador.ConstruirProductoVm(_servicio.GetEntityPorId(productoId));

            if (productoVm != null)
            {
                GetCart().AddItem(productoVm, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(int productoId, string returnUrl)
        {
            GetCart().RemoveItem(productoId);
            return RedirectToAction("Index", new { returnUrl });
        }

        private CarritoModel GetCart()
        {
            CarritoModel carrito = (CarritoModel)Session["Carrito"];
            if (carrito == null)
            {
                carrito = new CarritoModel();
                Session["Carrito"] = carrito;
            }
            return carrito;
        }
        public PartialViewResult ResumenCarrito(CarritoModel carrito)
        {
            return PartialView(carrito);
        }
        public ActionResult DireccionEnvio()
        {
            var direccionEnvio = new DireccionEnvio();
            direccionEnvio.Paises = Mapeador.ConstruirListaPaisesVm(_servicioPaises.GetLista());
            direccionEnvio.Ciudades = Mapeador.ConstruirListaCiudadVm(_servicioCiudades.GetLista(0));
            return View(direccionEnvio);
        }
        [HttpPost]
        public ActionResult DireccionEnvio(DireccionEnvio info)
        {
            if (ModelState.IsValid)
            {
                CarritoModel carrito = GetCart();
                carrito.DireccionEnvio = info;
                return RedirectToAction("FacturacionInfo");
            }
            else
            {
                info.Paises = Mapeador.ConstruirListaPaisesVm(_servicioPaises.GetLista());
                info.Ciudades = Mapeador.ConstruirListaCiudadVm(_servicioCiudades.GetLista(0));
                return View(info);
            }
        }

        public ActionResult FacturacionInfo()
        {
            FacturacionInfo info = new FacturacionInfo()
            {
                Clientes = Mapeador.ConstruirListaClienteVm(_servicioClientes.GetLista())
            };
            return View(info);
        }

        [HttpPost]
        public ActionResult FacturacionInfo(FacturacionInfo info)
        {
            if (ModelState.IsValid)
            {
                CarritoModel carrito = GetCart();
                carrito.FacturacionInfo = info;
                //Ver en servicio
                //ProcessOrder(cart);
                carrito.Clear();
                return View("OrdenCompleta");
            }
            else
            {
                info.Clientes = Mapeador.ConstruirListaClienteVm(_servicioClientes.GetLista());

                return View(info);
            }
        }

        public JsonResult GetCities(int paisId)
        {
            //Database.Configuration.ProxyCreationEnabled = false;
            var ciudadesVm = Mapeador.ConstruirListaCiudadVm(_servicioCiudades.GetLista(paisId));
            return Json(ciudadesVm);
        }

    }
}

