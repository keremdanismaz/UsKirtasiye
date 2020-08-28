using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UsKirtasiye.Controllers
{
    public class AboutMeController : Controller
    {
        // GET: AboutMe
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Hakkımızda()
        {
            return View();
        }
    }
}