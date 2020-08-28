using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UsKirtasiye.DB;
using UsKirtasiye.Models;

namespace UsKirtasiye.Controllers
{
    public class Product_DetailController : Controller
    {
        private UsKirtasiyeEntities context;

        public Product_DetailController()
        {
            context = new UsKirtasiyeEntities();
        }

        // GET: Product
        public ActionResult Product_Detail(int id)
        {
            List<DB.Products> product = context.Products.Where(x => x.Id == id).ToList();
            ViewBag.product = product;
            return View();
        }

        [HttpGet]
        public ActionResult AddBasket(int id, bool remove = false)
        {
            List<Models.Product_Detail.ProductBasketModels> basket = null;  // Modelin tipinde bir basket (Sepet) Değişkeni
            if (Session["Basket"] == null) // Eğer Null olarak geliyorsa
            {
                basket = new List<Models.Product_Detail.ProductBasketModels>();  //  bu nesneden bir adaet oluştur.
            }
            else
            {
                basket = (List<Models.Product_Detail.ProductBasketModels>)Session["Basket"];  // değilse oluşmuş olan nesneye sesionı ata.
            }

            if (basket.Any(x => x.Products.Id == id))
            {
                var pro = basket.FirstOrDefault(x => x.Products.Id == id);
                if (remove && pro.Count > 0)
                {
                    pro.Count -= 1;
                }
                else
                {
                    if (pro.Products.UnitsInStock > pro.Count)
                    {
                        pro.Count += 1;
                    }
                    else
                    {
                        TempData["myerror"] = "Yeterli Stok Yok";
                    }
                }
            }
            else
            {
                var pro = context.Products.FirstOrDefault(x => x.Id == id);
                if (pro != null && pro.IsContinued && pro.UnitsInStock > 0)
                {
                    basket.Add(new Models.Product_Detail.ProductBasketModels()
                    {
                        Count = 1,
                        Products = pro
                    });
                }
                else if (pro.IsContinued == false && pro != null)
                {
                    TempData["myerror"] = "";
                }
            }
            basket.RemoveAll(x => x.Count < 1);
            Session["Basket"] = basket;
            return RedirectToAction("Basket", "Product_Detail");
        }

        [HttpGet]
        public ActionResult Basket()
        {
            if (Session["Basket"] == null)
            {
                return RedirectToAction("Index", "Index");
            }
            List<Models.Product_Detail.ProductBasketModels> model = (List<Models.Product_Detail.ProductBasketModels>)Session["Basket"];
            if (model == null)
            {
                model = new List<Models.Product_Detail.ProductBasketModels>();
            }
            TempData["TotalPrice"] = model.Select(x => x.Products.Price * x.Count).Sum();
            List<Products> lastPro = context.Products.OrderByDescending(t => t.Id).Take(9).ToList();
            ViewBag.lastPro = lastPro;
            return View(model);
        }

        [HttpGet]
        public ActionResult RemoveBasket(int id)
        {
            List<Models.Product_Detail.ProductBasketModels> basket = (List<Models.Product_Detail.ProductBasketModels>)Session["Basket"];
            if (basket != null)
            {
                if (id > 0)
                {
                    basket.RemoveAll(x => x.Products.Id == id);
                }
                else if (id == 0)
                {
                    basket.Clear();
                }
                Session["Basket"] = basket;
            }
            return RedirectToAction("Basket", "Product_Detail");
        }

        [HttpPost]
        public ActionResult MusteriKaydi(DB.Members members)
        {
            context.Members.Add(members);
            context.SaveChanges();

            return RedirectToAction("Index", "Index");
        }
    }
}