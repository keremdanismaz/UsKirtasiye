using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UsKirtasiye.DB;

namespace UsKirtasiye.Controllers
{
    public class ProductController : Controller
    {
        private UsKirtasiyeEntities context;

        public ProductController()
        {
            context = new UsKirtasiyeEntities();
            ViewBag.MenuCat = context.Categories.Where(x => x.Parent_Id == null).ToList();
        }

        // GET: Product
        public ActionResult Product(int id = 0)
        {
            IQueryable<DB.Products> products = context.Products.Where(x => x.isDeleted == false || x.isDeleted == null);
            DB.Categories category = null;
            if (id > 0)
            {
                products = products.Where(x => x.Category_Id == id);
                category = context.Categories.FirstOrDefault(x => x.Id == id);
            }

            var viewmodel = new UsKirtasiye.Models.ProductModel()
            {
                Products = products.ToList(),
                Categories = category
            };

            return View(viewmodel);
        }
    }
}