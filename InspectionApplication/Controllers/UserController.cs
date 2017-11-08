using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspectionApplication.Controllers
{
    public class UserController : Controller
    {
        private Models.DbContext db = new Models.DbContext();

        public ActionResult List()
        {
            ViewBag.Title = "用户管理";
            return View();
        }
    }
}