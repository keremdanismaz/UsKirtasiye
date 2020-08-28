using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UsKirtasiye.DB;

namespace UsKirtasiye.Controllers
{
    public class RegisterController : Controller
    {
        private UsKirtasiyeEntities context;

        public RegisterController()
        {
            context = new UsKirtasiyeEntities();
        }

        [HttpGet]
        public ActionResult Register()
        {
            List<Products> lastPro = context.Products.Where(x => x.isDeleted == false || x.isDeleted == null).OrderByDescending(t => t.Id).Take(9).ToList();
            ViewBag.lastPro = lastPro;
            return View();
        }

        [HttpPost]
        public ActionResult KayıtAl(DB.Members members)
        {
            DB.Members userlar = context.Members.FirstOrDefault(x => x.Email == members.Email);

            if (userlar != null)
            {
                // Bu Kullanıcı daha önce eklenmiş yani aynı email dan bulunmaktadır.
                TempData["Kullanıcı"] = "X Bu email adresde bir kullanıcı bulunmaktadır.";
                return RedirectToAction("Register", "Register");
            }
            else if (members.Password.Length < 6)
            {
                TempData["Kullanıcı"] = "Şifre en az 6 karakter olmalıdır.";
                return RedirectToAction("Register", "Register");
            }
            else if (members.PhoneNumber.Length != 10)
            {
                TempData["Kullanıcı"] = "X  Telefon Numarası 10 karakterden oluşmalıdır.";
                return RedirectToAction("Register", "Register");
            }
            else
            {
                members.AddedDate = DateTime.Now;
                // Böyle bir kullanıcı yok ise  durumu
                context.Members.Add(members);
                context.SaveChanges();
                TempData["Logine"] = "✓  Kullanıcı başarı ile Kaydedildi";
                return RedirectToAction("Register", "Register");
            }
        }
    }
}