using Mvc6.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Mvc6.Controllers
{
    public class ProductosController : Controller
    {
        BDTestEntities1 storeDB = new BDTestEntities1();
        //
        // GET: /Productos/

        public ActionResult Index()
        {
            if (Session["userID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Products()
        {
            /*using(BDTestEntities1 db = new BDTestEntities1())
            {
                List<Producto> prodList = db.Producto.ToList<Producto>();
                List<Producto> prodListFinal = new List<Producto>();

                foreach(Producto prod in prodList)
                {
                    Producto prodFinal = new Producto();
                    prodFinal.ProductID = prod.ProductID;
                    prodFinal.ProductImg = prod.ProductImg;
                    prodFinal.ProductName = prod.ProductName;
                    prodFinal.ProductDescription = prod.ProductDescription;
                    prodFinal.Stock = prod.Stock;
                    prodFinal.Precio = prod.Precio;
                    prodFinal.Categoria = prod.Categoria;

                    prodListFinal.Add(prodFinal);
                }

                return View(prodListFinal);
            }*/
            
                var productos = storeDB.Producto.ToList();
                return View(productos);
            
        }

        public ActionResult GetData()
        {
            using (BDTestEntities1 db = new BDTestEntities1())
            {
                List<Producto> prodList = db.Producto.ToList<Producto>();
                List<Producto> prodListFinal = new List<Producto>();

                foreach (Producto prod in prodList)
                {
                    Producto prodFinal = new Producto();
                    prodFinal.ProductID = prod.ProductID;
                    prodFinal.ProductImg = prod.ProductImg;
                    prodFinal.ProductName = prod.ProductName;
                    prodFinal.ProductDescription = prod.ProductDescription;
                    prodFinal.Stock = prod.Stock;
                    prodFinal.Precio = prod.Precio;

                    prodListFinal.Add(prodFinal);
                }

                log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                logger.Error("Error de sistema");

                return Json(new { data = prodListFinal }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Producto());
            else
            {
                using (BDTestEntities1 db = new BDTestEntities1())
                {
                    return View(db.Producto.Where(x => x.ProductID == id).FirstOrDefault<Producto>());
                }
            }
        }


        [HttpPost]
        public ActionResult AddOrEdit(Producto prod, HttpPostedFileBase postedFile)
        {
            //byte[] bytes;

            /*using(BinaryReader br = new BinaryReader(postedFile.InputStream))
            {
                bytes = br.ReadBytes(postedFile.ContentLength);
            }*/
            string path = "";
            var fileName = "";
            using (BDTestEntities1 db = new BDTestEntities1())
            {
                        if (postedFile != null && postedFile.ContentLength > 0)
                        {
                            fileName = Path.GetFileName(postedFile.FileName);
                            path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                            postedFile.SaveAs(path);
                        }

                
                        if (prod.ProductID == 0)
                        {
                            prod.ProductImg = fileName;

                            db.Producto.Add(prod);
                            db.SaveChanges();
                            return RedirectToAction("Index", "Productos");
                        }
                        else
                        {
                            prod.ProductImg = fileName;

                            db.Entry(prod).State = System.Data.EntityState.Modified;
                            db.SaveChanges();
                            return RedirectToAction("Index", "Productos");
                        }
                }  
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (BDTestEntities1 db = new BDTestEntities1())
            {
                Producto prod = db.Producto.Where(x => x.ProductID == id).FirstOrDefault<Producto>();
                db.Producto.Remove(prod);
                db.SaveChanges();
                return Json(new { success = true, message = "Eliminado con éxito" }, JsonRequestBehavior.AllowGet);
            }
        }

        /*public IList<Producto> getAllProducts()
        {
            using (BDTestEntities db = new BDTestEntities())
            {
                IList<Producto> productos = new List<Producto>();
                productos = db.Producto.ToList();
                return productos;
            }
        }

        [HttpPost]
        public ActionResult AddCartItem(int prodId)
        {
            using (BDTestEntities1 db = new BDTestEntities1())
            {
                int user = Convert.ToInt32(Session["userIDNumb"]);
                Producto prod = db.Producto.Where(i => i.ProductID == prodId).FirstOrDefault<Producto>();
                int cant = 1;

                if(prod.Stock > 0)
                {
                    CartItem objcart = new CartItem()
                    {
                        userID = user,
                        productoID = prodId,
                        precioProd = prod.ProductID,
                        productName = prod.ProductName,
                        cantidad = cant
                    };

                    prod.Stock = prod.Stock - 1;
                    db.Entry(prod).State = System.Data.EntityState.Modified;
                    db.CartItem.Add(objcart);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Producto agregado con éxito." }, JsonRequestBehavior.AllowGet);
                } else {
                    return Json(new { error = true, message = "Producto no se encuentra en stock." }, JsonRequestBehavior.AllowGet);
                }
                
            }
        }

        public ActionResult GetCartItems()
        {
            using (BDTestEntities db = new BDTestEntities())
            {
                string username = Session["userID"].ToString();
                var sum = 0;
                var GetProductsId = db.Cart.Where(s => s.Username == username).Select(u => u.ProductId).ToList();
                var GetCartItem = from productList in db.Producto where GetProductsId.Contains(productList.ProductID) select productList;

                foreach (var totalsum in GetCartItem)
                {
                    sum = sum + Convert.ToInt32(totalsum.Precio);
                }

                ViewBag.Total = sum;
                return View("cartItem", GetCartItem);
            }
        }

        public ActionResult DeleteCart(int productId)
        {
            using (BDTestEntities db = new BDTestEntities())
            {
                string getName = Session["userID"].ToString();
                Cart removeCart = db.Cart.FirstOrDefault(s => s.Username == getName && s.ProductId == productId);
                db.Cart.Remove(removeCart);
                db.SaveChanges();
                return GetCartItems();
            }

        }*/

    }
}
