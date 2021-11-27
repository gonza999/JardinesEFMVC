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
        public CarritoController(IProductosServicios servicio)
        {
            _servicio = servicio;
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

    }
}

