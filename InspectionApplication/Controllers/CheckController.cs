using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspectionApplication.Controllers
{
    public class CheckController : Controller
    {
        private Models.DbContext db = new Models.DbContext();

        public ViewResult IndexList()
        {
            return View();
        }

        public ViewResult IndexStatistic()
        {
            return View();
        }

        public ActionResult ListNoCheck()
        {
            return View();
        }

        public ViewResult ListChecked()
        {
            return View();
        }
    }
}