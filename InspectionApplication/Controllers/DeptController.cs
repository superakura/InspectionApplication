using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspectionApplication.Controllers
{
    public class DeptController : Controller
    {
        private Models.DbContext db = new Models.DbContext();

        public JsonResult GetFatherDept()
        {
            var listFatherList = db.DeptInfo.Where(w => w.DeptFatherID == 1).OrderBy(o=>o.DeptOrder).ToList();
            return Json(listFatherList);
        }

        public JsonResult GetChildDept()
        {
            var postList =
            JsonConvert.DeserializeObject<Dictionary<String, Object>>(HttpUtility.UrlDecode(Request.Form.ToString()));
            var deptFatherID = 0;
            int.TryParse(postList["deptFatherID"].ToString(), out deptFatherID);
            var deptChildList = db.DeptInfo.Where(w => w.DeptFatherID == deptFatherID).OrderBy(o=>o.DeptOrder).ToList();
            return Json(deptChildList);
        }
    }
}