using Mvc6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Mvc6.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(Users user)
        {
            using(BDTestEntities1 db = new BDTestEntities1())
            {
                var userDetails = db.Users.Where(x => x.Username == user.Username && x.Password == user.Password).FirstOrDefault();
                if(userDetails == null)
                {
                    //user.LoginErrorMessage = "Usuario o contraseña incorrectos";
                    return View("Index", user);
                } else
                {
                    Session["userID"] = userDetails.Username;
                    Session["userIDNumb"] = userDetails.UserID;
                    Session["userRole"] = userDetails.Perfil;
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        // POST: /Users/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Clear();
            WebSecurity.Logout();

            return RedirectToAction("Index", "Login");
        }

    }
}
