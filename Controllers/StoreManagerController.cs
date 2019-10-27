using Mvc6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Mvc6.Controllers
{
    public class StoreManagerController : Controller
    {
        private BDTestEntities1 storeDB = new BDTestEntities1(); 
        //
        // GET: /StoreManager/

        public ViewResult Index()
        {
            var productos = storeDB.Producto.Include(p => p.Category);
            return View(productos.ToList());
        }

    }
}
