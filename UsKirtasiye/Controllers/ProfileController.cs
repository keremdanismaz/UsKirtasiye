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

            var addresses = context.Addresses.Where(x => x.Member_Id == currentUser.Id && x.IsActive == true).ToList();
            if (addresses.Any())
            {
                return View(addresses);
            }
            else
            {
                return RedirectToAction("AdresEkle", "Profile");
            }
        }

        [HttpPost]
        public ActionResult Adresler(DB.Addresses adres)
        {
            var currentUser = (DB.Members)Session["LogonUser"];
            var dbadress = context.Addresses.FirstOrDefault(x => x.Id == adres.Id);
            dbadress.Member_Id = currentUser.Id;
            dbadress.Name = adres.Name;
            dbadress.AdresDescription = adres.AdresDescription;
            dbadress.ModifiedDate = DateTime.Now;
            TempData["AdresDoğrulama"] = "✓  Adres Başarı ile güncellenmiştir.";
            context.SaveChanges();
            return RedirectToAction("Adresler", "Profile");
        }

        [HttpGet]
        public ActionResult AdresEkle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdresEkle(DB.Addresses adres)
        {
            DB.Addresses adresler = context.Addresses.FirstOrDefault(x => x.AdresDescription == adres.AdresDescription);
            if (adresler != null)
            {
                TempData["AdresHata"] = "Böyle bir adres zaten kayıtlıdır.";
            }
            else
            {
                var currentuser = (UsKirtasiye.DB.Members)Session["LogonUser"];
                adres.AddedDate = DateTime.Now;
                adres.Member_Id = currentuser.Id;
                adres.IsActive = true;
                context.Addresses.Add(adres);
                context.SaveChanges();
            }
            return View();
        }

        [HttpGet]
        public ActionResult DeleteAdress(int adressId)
        {
            var dbadress = context.Addresses.FirstOrDefault(x => x.Id == adressId);
            dbadress.IsActive = false;
            dbadress.ModifiedDate = DateTime.Now;
            context.SaveChanges();
            return RedirectToAction("Adresler", "Profile");
        }
    }
}