using Mvc6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc6.Controllers
{
    public class OrdenController : Controller
    {
        //
        // GET: /Orden/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            using (BDTestEntities1 db = new BDTestEntities1())
            {
                List<Orden> orderList = db.Orden.ToList<Orden>();
                List<Orden> orderListFinal = new List<Orden>();

                foreach (Orden orden in orderList)
                {
                    Orden orderFinal = new Orden();

                    orderFinal.Ciudad = orden.Ciudad;
                    orderFinal.Direccion = orden.Direccion;
                    orderFinal.Email = orden.Email;
                    orderFinal.FirstName = orden.FirstName;
                    orderFinal.LastName = orden.LastName;
                    orderFinal.OrderId = orden.OrderId;
                    orderFinal.Phone = orden.Phone;
                    orderFinal.Total = orden.Total;
                    orderFinal.Username = orden.Username;

                    orderListFinal.Add(orderFinal);
                }

                log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                logger.Error("Error de sistema");

                return Json(new { data = orderListFinal }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Orden());
            else
            {
                using (BDTestEntities1 db = new BDTestEntities1())
                {
                    return View(db.Orden.Where(x => x.OrderId == id).FirstOrDefault<Orden>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Orden order)
        {
            using (BDTestEntities1 db = new BDTestEntities1())
            {
                if (order.OrderId == 0)
                {
                    db.Orden.Add(order);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Guardado con éxito" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(order).State = System.Data.EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Actualizado con éxito" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (BDTestEntities1 db = new BDTestEntities1())
            {
                Orden order = db.Orden.Where(x => x.OrderId == id).FirstOrDefault<Orden>();
                db.Orden.Remove(order);
                db.SaveChanges();
                return Json(new { success = true, message = "Eliminado con éxito" }, JsonRequestBehavior.AllowGet);

            }
        }
    }
}
