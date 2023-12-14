using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ElabSupport.Controllers
{
    
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login","Account");
            }
            
        }
    }
}
