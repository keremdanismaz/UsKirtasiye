using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UsKirtasiye.DB;
using UsKirtasiye.Models;

namespace UsKirtasiye.Controllers
{
    public class ProfileController : Controller
    {
        private UsKirtasiyeEntities context;

        public ProfileController()
        {
            context = new UsKirtasiyeEntities();
        }

        [HttpGet]
        public ActionResult Profile()
        {
            if (Session["LogonUser"] == null)
            {
                return RedirectToAction("Index", "Index");
            }
            var currentUser = (DB.Members)Session["LogonUser"];
            DB.Members user = context.Members.FirstOrDefault(x => x.Id == currentUser.Id);
            return View(user);
        }

        [HttpPost]
        public ActionResult ProfileEdit(DB.Members üyekayıt)
        {
            if (üyekayıt.Password.Length < 6)
            {
                TempData["YenilemeHatası"] = "X Şifre 6 karakterden az olamaz";
            }
            else if (üyekayıt.PhoneNumber.Length != 10)
            {
                TempData["YenilemeHatası"] = "X  Telefon numarası 10 karakterden oluşmalıdır. ";
            }
            else
            {
                üyekayıt.ModifiedDate = DateTime.Now;
                context.Entry<DB.Members>(üyekayıt).State = System.Data.Entity.EntityState.Modified;
                TempData["Doğrulama"] = "✓  Kullanıcı başarı ile güncellenmiştir";
                context.SaveChanges();
            }
            return RedirectToAction("Profile", "Profile");
        }

        [HttpPost]
        public ActionResult AdminProfile(DB.Members üyekayıt)
        {
            if (üyekayıt.Password.Length < 6)
            {
                TempData["YenilemeHatası"] = "X Şifre 6 karakterden az olamaz";
            }
            else if (üyekayıt.PhoneNumber.Length != 10)
            {
                TempData["YenilemeHatası"] = "X  Telefon numarası 10 karakterden oluşmalıdır. ";
            }
            else
            {
                context.Entry<DB.Members>(üyekayıt).State = System.Data.Entity.EntityState.Modified;
                TempData["Doğrulama"] = "✓  Kullanıcı başarı ile güncellenmiştir";
                context.SaveChanges();
            }
            return RedirectToAction("AdminProfile", "Admin");
        }

        [HttpGet]
        public ActionResult Adresler()
        {
            if (Session["LogonUser"] == null)
            {
                return RedirectToAction("Index", "Index");
            }
            var currentUser = (DB.Members)Session["LogonUser"];
            DB.Addresses user = context.Addresses.FirstOrDefault(x => x.Member_Id == currentUser.Id);
            return View(user);
        }

        [HttpPost]
        public ActionResult Adresler(DB.Addresses adresler)
        {
            var currentUser = (DB.Members)Session["LogonUser"];
            var dbadress = context.Addresses.FirstOrDefault(x => x.Id == adresler.Id);
            dbadress.Member_Id = currentUser.Id;
            dbadress.Name = adresler.Name;
            dbadress.AdresDescription = adresler.AdresDescription;
            dbadress.AdresDescription = adresler.AdresDescription;
            dbadress.ModifiedDate = DateTime.Now;
            TempData["AdresDoğrulama"] = "✓  Adres Başarı ile güncellenmiştir.";
            context.SaveChanges();
            return RedirectToAction("Adresler", "Profile");
        }
    }
}