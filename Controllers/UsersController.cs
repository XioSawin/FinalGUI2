using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc6.Models;
using log4net;

namespace Mvc6.Controllers
{
    public class UsersController : Controller
    {
        //
        // GET: /Users/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            using(BDTestEntities1 db = new BDTestEntities1())
            {
                List<Users> userList = db.Users.ToList<Users>();
				List<Users> userListFinal = new List<Users>();

				foreach (Users user in userList)
				{
					Users userFinal = new Users();
					userFinal.UserID = user.UserID;
					userFinal.Username = user.Username;
					userFinal.Password = user.Password;
                    userFinal.Email = user.Email;
                    userFinal.Perfil = user.Perfil;
					userListFinal.Add(userFinal);	
			    }

				log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
				logger.Error("Error de sistema");
				
				return Json(new { data = userListFinal }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Users());
            else
            {
                using (BDTestEntities1 db = new BDTestEntities1())
                {
                    return View(db.Users.Where(x => x.UserID == id).FirstOrDefault<Users>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Users user)
        {
            using (BDTestEntities1 db = new BDTestEntities1())
            {
                if (user.UserID == 0)
                {
                    user.Perfil = "Comprador";
                    db.Users.Add(user);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Guardado con éxito" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(user).State = System.Data.EntityState.Modified;
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
                Users user = db.Users.Where(x => x.UserID == id).FirstOrDefault<Users>();
                db.Users.Remove(user);
                db.SaveChanges();
                return Json(new { success = true, message = "Eliminado con éxito" }, JsonRequestBehavior.AllowGet);

            }
        }

        // GET: /Users/Register

        
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Users/Register

        [HttpPost]
        public ActionResult Register(Users user)
        {
            using (BDTestEntities1 db = new BDTestEntities1())
            {
                if (user.UserID == 0)
                {
                    user.Perfil = "Comprador";
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Login");
                }

                // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
                return View(user);
            }
        }

        // POST: /Users/LogOff

        [HttpPost]
        public ActionResult LogOff()
        {
            Session.Clear();
            //WebSecurity.Logout();

            return RedirectToAction("Index", "Login");
        }
    }
}
