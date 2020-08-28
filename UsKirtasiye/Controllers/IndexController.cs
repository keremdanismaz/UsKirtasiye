using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UsKirtasiye.DB;

namespace UsKirtasiye.Controllers
{
    public class IndexController : Controller
    {
        private UsKirtasiyeEntities context;

        public IndexController()
        {
            context = new UsKirtasiyeEntities();
        }

        // GET: Index
        public ActionResult Index()
        {
            var products = context.Products.Where(x => x.isMain == true && x.isDeleted == false).Take(50).ToList();
            return View(products.OrderByDescending(x => x.AddedDate).ToList());
        }
    }
}