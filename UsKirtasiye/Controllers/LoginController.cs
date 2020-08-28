using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UsKirtasiye.DB;

namespace UsKirtasiye.Controllers
{
    public class LoginController : Controller
    {
        private UsKirtasiyeEntities context;

        public LoginController()
        {
            context = new UsKirtasiyeEntities();
        }

        // GET: Login
        public ActionResult Login()
        {
            List<Products> lastPro = context.Products.Where(x => x.isDeleted == false || x.isDeleted == null).OrderByDescending(t => t.Id).Take(9).ToList();
            ViewBag.lastPro = lastPro;
            return View();
        }

        [HttpPost]
        public ActionResult GirisYap(DB.Members members)
        {
            if (context.Members.Any(x => x.Email == members.Email && x.Password == members.Password))
            {
                var sesion = context.Members.FirstOrDefault(x => x.Email == members.Email && x.Password == members.Password);
                Session["LogonUser"] = sesion;
                var currentuser = (UsKirtasiye.DB.Members)Session["LogonUser"];
                if (currentuser.UserGroup == 10) //  Bu bir admin girişi olmuştur.
                {
                    return RedirectToAction("AdminFirstPge", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Index");
                }
            }
            else
            {
                TempData["Hata"] = "Bu email yada şifreye ait bir kullanıcı bulunamadı";
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Index");
        }
    }
}