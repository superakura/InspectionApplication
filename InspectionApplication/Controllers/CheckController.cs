using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspectionApplication.Controllers
{
    public class CheckController : Controller
    {
        private Models.DbContext db = new Models.DbContext();

        //加载报检单审批视图
        public ViewResult IndexList()
        {
            ViewBag.Title = "报检单审批";
            return View();
        }

        //加载报检单统计视图
        public ViewResult IndexStatistic()
        {
            ViewBag.Title = "报检单统计";
            return View();
        }

        //获取未处理的报检单列表
        public JsonResult GetNoCheckList()
        {
            try
            {
                var result = from i in db.InspectionApplications
                             where i.InspectionApplicationState== "待审核"
                             orderby i.InputDate descending
                             select new {
                                 i.ProductName,
                                 i.InspectionFatherDeptName,
                                 i.InspectionPersonName,
                                 i.InspectionDate,
                                 i.ProductBatchNum,
                                 i.ProductType,
                                 i.InspectionApplicationID
                             };
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        //获取报检单详细信息
        public JsonResult GetNoCheckDetail()
        {
            try
            {
                return Json("");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        //对报检单编号进行检查，不能有重复的报检单编号
        public string CheckInspectionNum()
        {
            try
            {
                var postList =
   JsonConvert.DeserializeObject<Dictionary<String, Object>>(HttpUtility.UrlDecode(Request.Form.ToString()));

                var inspectionNum = postList["num"].ToString();

                var isOk = db.InspectionApplications.Where(w => w.InspectionApplicationNum == inspectionNum).FirstOrDefault();
                if (isOk==null)
                {
                    return "ok";
                }
                else
                {
                    return "error";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //报检单处理，接收或回退
        public string InspectionCheck()
        {
            try
            {
                var postList =
   JsonConvert.DeserializeObject<Dictionary<String, Object>>(HttpUtility.UrlDecode(Request.Form.ToString()));

                var inspectionID = 0;
                int.TryParse(postList["id"].ToString(), out inspectionID);

                var inspectionNum = postList["inspectionNum"].ToString();
                var inspectionRemark = postList["inspectionRemark"].ToString();
                var submitType = postList["submitType"].ToString();

                var person = Session["user"] as Models.UserInfo;

                var info = db.InspectionApplications.Find(inspectionID);

                if (submitType=="access")
                {
                    info.InspectionApplicationNum = inspectionNum;
                    info.InspectionApplicationState = "审核通过";
                }
                else
                {
                    info.InspectionApplicationState = "审核回退";
                }
                info.DisposeRemark = inspectionRemark;
                info.DisposeDate = DateTime.Now;
                info.DisposePersonName = person.UserName;
                info.DisposePersonID = person.UserID;

                db.SaveChanges();
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //获取已处理的报检单列表
        public JsonResult GetCheckList()
        {
            try
            {
                var postList =
       JsonConvert.DeserializeObject<Dictionary<String, Object>>(HttpUtility.UrlDecode(Request.Form.ToString()));

                var curPage = 0;
                int.TryParse(postList["curPage"].ToString(), out curPage);

                var pageSize = 0;
                int.TryParse(postList["pageSize"].ToString(), out pageSize);

                var productName = postList["productName"].ToString();//产品名称
                var inspectionDate = postList["inspectionDate"].ToString();//报检时间

                var result = from i in db.InspectionApplications
                             where i.InspectionApplicationState == "审核通过"
                             select i;

                if (!string.IsNullOrEmpty(productName))
                {
                    result = result.Where(w => w.ProductName.Contains(productName));
                }
                if (!string.IsNullOrEmpty(inspectionDate))
                {
                    result = result.Where(w => SqlFunctions.DateDiff("day", w.InspectionDate, inspectionDate) == 0);
                }
                Dictionary<string, object> infoList = new Dictionary<string, object>();
                infoList.Add("count", result.Count());
                infoList.Add("infoList", result.OrderByDescending(o => o.InspectionDate).Take(pageSize * curPage).Skip(pageSize * (curPage - 1)).ToList());
                return Json(infoList);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}