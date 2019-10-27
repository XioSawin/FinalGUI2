using Mvc6.Models;
using Mvc6.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc6.Controllers
{
    public class CarritoController : Controller
    {
        BDTestEntities1 storeDB = new BDTestEntities1();
        //
        // GET: /Carrito/

        public ActionResult Index()
        {
            var cart = Carrito.GetCart(this.HttpContext);

            //set up viewmodel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            return View(viewModel);
        }

        //
        // GET: /Store/AddToCart/id
        public ActionResult AddToCart(int id)
        {
            //retrieve producto from database
            var addedProd = storeDB.Producto.Single(producto => producto.ProductID == id);

            //añadir al carrito
            var cart = Carrito.GetCart(this.HttpContext);

            cart.AddToCart(addedProd);
            //back to main store
            return RedirectToAction("Index");
        }
        //
        // AJAX: /Carrito/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            //eliminar art del carrito
            var cart = Carrito.GetCart(this.HttpContext);

            //para la confirmación de que hay que eliminar el artículo
            string prodName = storeDB.Cart.Single(item => item.RecordID == id).ProductName;

            //remove from cart
            int itemCount = cart.RemoveFromCart(id);

            //display confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Mensaje = Server.HtmlEncode(prodName) + " ha sido eliminado de su carro de compras.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

            return Json(results);
        }
        //
        // GET: /Carrito/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = Carrito.GetCart(this.HttpContext);
            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}
