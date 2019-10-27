using Mvc6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc6.Controllers
{
    public class ProveedoresController : Controller
    {
        //
        // GET: /Proveedores/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            using (BDTestEntities1 db = new BDTestEntities1())
            {
                List<Proveedores> provList = db.Proveedores.ToList<Proveedores>();
                List<Proveedores> provListFinal = new List<Proveedores>();

                foreach (Proveedores prov in provList)
                {
                    Proveedores provFinal = new Proveedores();
                    provFinal.ProviderID = prov.ProviderID;
                    provFinal.ProviderName = prov.ProviderName;
                    provFinal.Telefono = prov.Telefono;
                    provFinal.Email = prov.Email;
                    provFinal.Direccion = prov.Direccion;

                    provListFinal.Add(provFinal);
                }

                log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                logger.Error("Error de sistema");

                return Json(new { data = provListFinal }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddOrEdit (int id = 0)
        {
            if (id == 0)
                return View(new Proveedores());
            else
            {
                using (BDTestEntities1 db = new BDTestEntities1())
                {
                    return View(db.Proveedores.Where(x => x.ProviderID == id).FirstOrDefault<Proveedores>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit (Proveedores prov)
        {
            using (BDTestEntities1 db = new BDTestEntities1())
            {
                if (prov.ProviderID == 0)
                {
                    db.Proveedores.Add(prov);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Guardado con éxito" }, JsonRequestBehavior.AllowGet);
                } else
                {
                    db.Entry(prov).State = System.Data.EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Actualizado con éxito" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using(BDTestEntities1 db = new BDTestEntities1())
            {
                Proveedores prov = db.Proveedores.Where(x => x.ProviderID == id).FirstOrDefault<Proveedores>();
                db.Proveedores.Remove(prov);
                db.SaveChanges();
                return Json(new { success = true, message = "Eliminado con éxito" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
