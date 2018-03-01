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
            var listFatherList = db.DeptInfo.Where(w => w.DeptFatherID == 1).OrderBy(o => o.DeptOrder).ToList();
            return Json(listFatherList);
        }

        public JsonResult GetChildDept()
        {
            var postList =
            JsonConvert.DeserializeObject<Dictionary<String, Object>>(HttpUtility.UrlDecode(Request.Form.ToString()));
            var deptFatherID = 0;
            int.TryParse(postList["deptFatherID"].ToString(), out deptFatherID);
            var deptChildList = db.DeptInfo.Where(w => w.DeptFatherID == deptFatherID).OrderBy(o => o.DeptOrder).ToList();
            return Json(deptChildList);
        }

        //标记删除部门，但不能删除二级单位、大庆炼化公司
        public string Del()
        {
            try
            {
                var infoList =
   JsonConvert.DeserializeObject<Dictionary<String, Object>>(HttpUtility.UrlDecode(Request.Form.ToString()));

                var id = 0;
                int.TryParse(infoList["id"].ToString(), out id);

                var deptInfo = db.DeptInfo.Find(id);
                deptInfo.DeptState = 1;//将部门状态信息标记为【1】删除状态

                var userInfo = Session["user"] as Models.UserInfo;

                Models.Log log = new Models.Log();
                log.LogInfo = "执行删除单位操作,部门编号：" + deptInfo.DeptID + ",单位名称：" + deptInfo.DeptName + "";
                log.LogInputDate = DateTime.Now;
                log.LogInputPerson = userInfo.UserID;
                log.LogType = "删除用户信息";
                log.InspectionID = deptInfo.DeptID;
                db.Log.Add(log);
                db.SaveChanges();
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //获取部门信息json列表，去掉标记为1删除的单位
        public JsonResult List()
        {
            return Json("");
        }

        //添加单位
        public string Add()
        {
            return "";
        }

        //修改单位名称、上级部门
        public string Edit()
        {
            return "";
        }

        //调整单位顺序
        public string ChangeOrder()
        {
            return "";
        }
    }
}