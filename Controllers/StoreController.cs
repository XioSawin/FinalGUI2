using Mvc6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc6.Controllers
{
    public class StoreController : Controller
    {
        BDTestEntities1 storeDB = new BDTestEntities1();
        //
        // GET: /Store/

        public ActionResult Index()
        {
            var categorias = storeDB.Categoria.ToList();
            return View(categorias);
        }
        //
        // GET: /Store/Browse?genre=?Disco
        public ActionResult Browse(string categoria)
        {
            var categModel = storeDB.Categoria.Include("Producto").Single(c => c.Nombre == categoria);

            return View(categModel);
        }
        //
        // GET: /Store/Details/?
        public ActionResult Details(int id)
        {
            var producto = storeDB.Producto.Find(id);
            return View(producto);
        }
        //
        // GET: /Store/CategMenu
        [ChildActionOnly]
        public ActionResult CategMenu()
        {
            var categorias = storeDB.Categoria.ToList();
            return PartialView(categorias);
        }
            
    }
}
