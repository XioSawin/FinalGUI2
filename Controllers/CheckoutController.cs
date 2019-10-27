using Mvc6.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc6.Controllers
{
    public class CheckoutController : Controller
    {
        BDTestEntities1 storeDB = new BDTestEntities1();
        const string PromoCode = "100";

        //
        // GET: /Checkout/AddressAndPayment

        public ActionResult AddressAndPayment()
        {
            return View();
        }
        //
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult AddressAndPayment(Orden order)
        {
            var cart = Carrito.GetCart(this.HttpContext);
            //var orderTotal = cart.GetTotal();

            using (BDTestEntities1 db = new BDTestEntities1())
            {
                order.Total = cart.GetTotal();

                db.Orden.Add(order);
                db.SaveChanges();

                //cart.CreateOrder(order);
                cart.EmptyCart();

                return RedirectToAction("Complete", new { id = order.OrderId });
            }
            /*var order = new Orden();
            TryUpdateModel(order);

            //try
            //{
                
            try
            {
                storeDB.Orden.Add(order);
                storeDB.SaveChanges();
            } catch(DbEntityValidationException dbEx)
            {
                foreach(var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
                

                var cart = Carrito.GetCart(this.HttpContext);
                cart.CreateOrder(order);

                //storeDB.SaveChanges();

                return RedirectToAction("Complete", new { id = order.OrderId });
            }
            catch
            {
                //Invalid
                return View(order);
            }*/

           
        }
        //
        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            Orden order = storeDB.Orden.Where(o => o.OrderId == id).SingleOrDefault();
            return View(order);
        }
    }
}
