using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UsKirtasiye.DB;

namespace UsKirtasiye.Controllers
{
    public class AdminController : Controller
    {
        private UsKirtasiyeEntities context;

        public AdminController()
        {
            context = new UsKirtasiyeEntities(); // entitiy frame bağlandık.
        }

        [HttpGet]  //  sayfanın gösterimi için gerekli olan kodlar
        public ActionResult Admin()
        {
            var currentuser = (UsKirtasiye.DB.Members)Session["LogonUser"];
            if (Session["LogonUser"] == null)
            {
                return RedirectToAction("Index", "Index");
            }
            else if (currentuser.UserGroup != 10)
            {
                return RedirectToAction("Index", "Index");
            }
            else
            {
                var products = context.Products.Where(x => x.isDeleted == false || x.isDeleted == null).ToList();
                return View(products.OrderByDescending(x => x.AddedDate).ToList());
            }
        }

        public ActionResult Edit(int id = 0)
        {
            var currentuser = (UsKirtasiye.DB.Members)Session["LogonUser"];
            if (Session["LogonUser"] == null)
            {
                return RedirectToAction("Index", "Index");
            }
            else if (currentuser.UserGroup != 10)
            {
                return RedirectToAction("Index", "Index");
            }
            var product = context.Products.FirstOrDefault(x => x.Id == id);
            var categories = context.Categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            }).ToList();
            ViewBag.Categories = categories;

            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(DB.Products product)
        {
            var productImagePath = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file.ContentLength > 0)
                {
                    var folder = Server.MapPath("~/Images/UserImages/Product");
                    var fileName = Guid.NewGuid() + ".jpg";
                    file.SaveAs(Path.Combine(folder, fileName));

                    var filePath = "/Images/UserImages/Product/" + fileName;
                    productImagePath = filePath;
                }
            }
            if (product.Id > 0)
            {
                var dbProduct = context.Products.FirstOrDefault(x => x.Id == product.Id);

                dbProduct.Category_Id = product.Category_Id;
                dbProduct.ModifiedDate = DateTime.Now;
                dbProduct.Description = product.Description;
                dbProduct.IsContinued = product.IsContinued;
                dbProduct.Name = product.Name;
                dbProduct.Price = product.Price;
                dbProduct.UnitsInStock = product.UnitsInStock;
                dbProduct.isDeleted = false;
                dbProduct.isMain = product.isMain;
                if (string.IsNullOrEmpty(productImagePath) == false)
                {
                    dbProduct.Photo = productImagePath;
                }
            }
            else
            {
                product.AddedDate = DateTime.Now;
                product.Photo = productImagePath;
                product.isDeleted = false;

                context.Entry(product).State = System.Data.Entity.EntityState.Added;
            }
            context.SaveChanges();
            return RedirectToAction("Admin", "Admin");
        }

        public ActionResult Delete(int id)
        {
            var currentuser = (UsKirtasiye.DB.Members)Session["LogonUser"];
            if (Session["LogonUser"] == null)
            {
                return RedirectToAction("Index", "Index");
            }
            else if (currentuser.UserGroup != 10)
            {
                return RedirectToAction("Index", "Index");
            }

            var pro = context.Products.FirstOrDefault(x => x.Id == id);
            pro.isDeleted = true;
            context.SaveChanges();
            return RedirectToAction("Admin", "Admin");
        }

        [HttpGet]
        public ActionResult AdminProfile()
        {
            var currentuser = (UsKirtasiye.DB.Members)Session["LogonUser"];
            if (Session["LogonUser"] == null)
            {
                return RedirectToAction("Index", "Index");
            }
            else if (currentuser.UserGroup != 10)
            {
                return RedirectToAction("Index", "Index");
            }
            DB.Members user = context.Members.FirstOrDefault(x => x.Id == currentuser.Id);
            return View(user);
        }

        [HttpGet]
        public ActionResult AdminFirstPge()
        {
            var currentuser = (UsKirtasiye.DB.Members)Session["LogonUser"];
            if (Session["LogonUser"] == null)
            {
                return RedirectToAction("Index", "Index");
            }
            else if (currentuser.UserGroup != 10)
            {
                return RedirectToAction("Index", "Index");
            }
            else
            {
                return View();
            }
        }
    }
}