using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc6.Models
{
    public class Carrito
    {
        BDTestEntities1 storeDB = new BDTestEntities1();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";
        public static Carrito GetCart(HttpContextBase context)
        {
            var cart = new Carrito();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        //simplificar llamar al carrito
        public static Carrito GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddToCart(Producto product)
        {
            //Get matching cart and album instances
            var cartItem = storeDB.Cart.SingleOrDefault(c => c.CartID == ShoppingCartId && c.ProductID == product.ProductID);

            if (product.Stock > 0)
            {
                if (cartItem == null)
                {
                    //si item no está en carrito, lo añado
                    cartItem = new Cart
                    {
                        ProductID = product.ProductID,
                        CartID = ShoppingCartId,
                        Cantidad = 1,
                        Product = product,
                        ProductPrice = product.Precio,
                        ProductName = product.ProductName
                    };
                    storeDB.Cart.Add(cartItem);
                    product.Stock--;
                }
                else
                {
                    //si item existe. sumo 1 más
                    cartItem.Cantidad++;
                    product.Stock--;
                }
                //Guardar cambios

                try
                {
                    storeDB.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                        }
                    }
                }
            }
        }

        public int RemoveFromCart(int id)
        {
            //obtener carrito
            var cartItem = storeDB.Cart.Single(cart => cart.CartID == ShoppingCartId && cart.RecordID == id);

            int itemCount = 0;

            if(cartItem != null)
            {
                if (cartItem.Cantidad > 1)
                {
                    cartItem.Cantidad--;
                    itemCount = cartItem.Cantidad;
                } else
                {
                    storeDB.Cart.Remove(cartItem);
                }
                storeDB.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = storeDB.Cart.Where(cart => cart.CartID == ShoppingCartId);

            foreach(var cartItem in cartItems)
            {
                storeDB.Cart.Remove(cartItem);
            }
            storeDB.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
            return storeDB.Cart.Where(cart => cart.CartID == ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            //toma el valor de cada item y sumar el total.
            int? count = (from cartItems in storeDB.Cart
                          where cartItems.CartID == ShoppingCartId
                          select (int?)cartItems.Cantidad).Sum();

            //0 if all entries null
            return count ?? 0;
        }

        public int GetTotal()
        {
            //Obtener total multiplicando el precio de cada producto por la cantidad
            //luego sumando las cantidades totales.

            int? total = (from cartItems in storeDB.Cart
                            where cartItems.CartID == ShoppingCartId
                            select cartItems.Cantidad * cartItems.ProductPrice).Sum();
            
            /*int? totaA = (from cartItems in storeDB.Cart
                              where cartItems.CartID == ShoppingCartId
                              select cartItems.Cantidad * cartItems.Producto.Precio).Sum();*/

            return total ?? 0;
        }

        public int CreateOrder(Orden order)
        {
            int? orderTotal = 0;

            var cartItems = GetCartItems();

            //iterar sobre los items del carrito e ir agregándolos al total de la orden
            using(BDTestEntities1 storeDB = new BDTestEntities1())
            {
                foreach (var item in cartItems)
                {
                    var orderDetail = new OrdenDetail
                    {
                        ProductoId = item.ProductID,
                        OrderId = order.OrderId,
                        UnitPrice = item.ProductPrice,
                        Cantidad = item.Cantidad
                    };

                    //set order total of the shopping cart
                    orderTotal += (item.Cantidad * item.ProductPrice);

                    storeDB.OrdenDetail.Add(orderDetail);
                }

                //Set the order's total to the orderTotal count
                order.Total = orderTotal;
                storeDB.SaveChanges();
                EmptyCart();

                return order.OrderId;
            }

            
        }

        //HttpContextBase to allow access to cookies

        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    //Generar random GUID usando System.Guid
                    Guid tempCartId = Guid.NewGuid();
                    //send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        //cuando user inicia sesión, migrar el contenido del carro de compras
        //if comenzó compra como anónimo.

        public void MigrateCart(string userName)
        {
            var shoppingCart = storeDB.Cart.Where(c => c.CartID == ShoppingCartId);

            foreach(Cart item in shoppingCart)
            {
                item.CartID = userName;
            }
            storeDB.SaveChanges();
        }
    }
}