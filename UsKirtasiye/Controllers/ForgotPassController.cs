using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UsKirtasiye.DB;

namespace UsKirtasiye.Controllers
{
    public class ForgotPassController : Controller
    {
        private UsKirtasiyeEntities context;

        public ForgotPassController()
        {
            context = new UsKirtasiyeEntities();
        }    // context  bağlanma

        [HttpGet]
        public ActionResult ForgotPass()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPass(string email)
        {
            var member = context.Members.FirstOrDefault(x => x.Email == email);
            if (member == null)
            {
                TempData["err"] = "Böyle bir emaile ait hesap bulunamadı";
                return RedirectToAction("ForgotPass", "ForgotPass");
            }
            else
            {
                //gelen mail doğru ise
                var body = "Şifreniz : " + member.Password;
                MyMail mail = new MyMail(member.Email, body, "Şifremi  Unuttum");
                mail.SendMail();
                TempData["info"] = email + "mail adresinize yeni şifre gönderildi. ";
                return RedirectToAction("Login", "Login");
            }
        }
    }
}