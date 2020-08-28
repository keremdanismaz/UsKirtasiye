using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UsKirtasiye.DB;

namespace UsKirtasiye.Controllers
{
    public class MemberController : Controller
    {
        private UsKirtasiyeEntities context;

        public MemberController()
        {
            context = new UsKirtasiyeEntities();
        }

        [HttpGet]
        public ActionResult Member()
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
                var member = context.Members.Where(x => x.UserGroup != 10).ToList();
                return View(member.OrderByDescending(x => x.AddedDate).ToList());
            }
        }
    }
}