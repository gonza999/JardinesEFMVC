using JardinesEf.Entidades.Entidades;
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
        private readonly IOrdenesServicios _servicioOrdenes;

        public CarritoController(IProductosServicios servicio, IPaisesServicios servicioPaises,
            IClientesServicios servicioClientes, ICiudadesServicios servicioCiudades,
            IOrdenesServicios servicioOrdenes)
        {
            _servicio = servicio;
            _servicioPaises = servicioPaises;
            _servicioCiudades = servicioCiudades;
            _servicioClientes = servicioClientes;
            _servicioOrdenes = servicioOrdenes;
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
                _servicio.SetearReservarProducto(productoVm.ProductoId, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(int productoId, int cantidad, string returnUrl)
        {
            GetCart().RemoveItem(productoId);
            _servicio.SetearReservarProducto(productoId, -cantidad);
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
        public PartialViewResult ResumenCarrito()
        {
            var carrito = GetCart();
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
                try
                {
                    Orden orden = new Orden()
                    {
                        ClienteId = carrito.FacturacionInfo.ClienteId,
                        FechaCompra = DateTime.Now,
                        FechaEntrega = DateTime.Now.AddDays(5),
                        DireccionEnvio=carrito.DireccionEnvio.Direccion,
                        CodigoPostalEnvio=carrito.DireccionEnvio.CodigoPostal,
                        PaisEnvioId=carrito.DireccionEnvio.PaisId,
                        CiudadEnvioId=carrito.DireccionEnvio.CiudadId,
                        DetalleOrdenes=ObtenerDetalleOrdenes(carrito)
                    };
                    _servicioOrdenes.Guardar(orden);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty,ex.Message);
                    info.Clientes = Mapeador.ConstruirListaClienteVm(_servicioClientes.GetLista());
                    return View(info);
                }
                carrito.Clear();
                return View("OrdenCompleta");
            }
            else
            {
                info.Clientes = Mapeador.ConstruirListaClienteVm(_servicioClientes.GetLista());

                return View(info);
            }
        }

        private ICollection<DetalleOrden> ObtenerDetalleOrdenes(CarritoModel carrito)
        {
            var lista =new List<DetalleOrden>();
            foreach (var c in carrito.Items)
            {
                DetalleOrden detalleOrden = new DetalleOrden()
                {
                    ProductoId=c.Producto.ProductoId,
                    PrecioUnitario=c.Producto.PrecioUnitario,
                    Cantidad=c.Cantidad
                };
                lista.Add(detalleOrden);
            }
            return lista;
        }

        public JsonResult GetCities(int paisId)
        {
            //Database.Configuration.ProxyCreationEnabled = false;
            var ciudadesVm = Mapeador.ConstruirListaCiudadVm(_servicioCiudades.GetLista(paisId));
            return Json(ciudadesVm);
        }

        public ActionResult CancelOrder()
        {
            var cart = GetCart();
            foreach (var c in cart.Items)
            {
                _servicio.SetearReservarProducto(c.Producto.ProductoId,-c.Cantidad);
            }
            cart.Clear();
            return RedirectToAction("Index","Home");
        }

    }
}

