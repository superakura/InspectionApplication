using Aspose.Cells;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
                             join u in db.UserInfo on i.InspectionPersonID equals u.UserID
                             where i.InspectionApplicationState== "待审核"
                             orderby i.InputDate descending
                             select new {
                                 i.ProductName,
                                 i.InspectionFatherDeptName,
                                 i.InspectionPersonName,
                                 i.InspectionDate,
                                 i.ProductBatchNum,
                                 i.ProductType,
                                 i.InspectionApplicationID,
                                 u.UserPhone
                             };
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        //根据报检单ID获取已接收报检单详细信息
        public JsonResult GetCheckedInspectionDetail()
        {
            try
            {
                var postList =
   JsonConvert.DeserializeObject<Dictionary<String, Object>>(HttpUtility.UrlDecode(Request.Form.ToString()));

                var id = 0;
                int.TryParse(postList["id"].ToString(), out id);
                var inspectionInfo = from i in db.InspectionApplications
                                     join u in db.UserInfo on i.InspectionPersonID equals u.UserID
                                     join d in db.UserInfo on i.DisposePersonID equals d.UserID
                                     where i.InspectionApplicationID == id&i.InspectionApplicationState== "审核通过"
                                     select new
                                     {
                                         i.ArrivalDate,
                                         i.DisposeDate,
                                         i.DisposePersonID,
                                         i.DisposePersonName,
                                         i.DisposeRemark,
                                         i.InputDate,
                                         i.InspectionApplicationID,
                                         i.InspectionApplicationNum,
                                         i.InspectionApplicationState,
                                         i.InspectionDate,
                                         i.InspectionDeptID,
                                         i.InspectionDeptName,
                                         i.InspectionFartherDeptID,
                                         i.InspectionFatherDeptName,
                                         i.InspectionPersonID,
                                         i.InspectionPersonName,
                                         i.ProductBatchNum,
                                         i.ProductCount,
                                         i.ProductDealer,
                                         i.ProductFactory,
                                         i.ProductName,
                                         i.ProductPackingType,
                                         i.ProductType,
                                         i.SamplePlace,
                                         u.UserPhone,
                                         disposePersonPhone = d.UserPhone
                                     };

                var inspectionDeptList = from d in db.ProductUseDept
                                         join p in db.DeptInfo on d.ProductUseFatherDeptID equals p.DeptID
                                         join c in db.DeptInfo on d.ProductUseDeptID equals c.DeptID
                                         where d.InspectionApplicationID == id
                                         select new
                                         {
                                             d.ProductUseDeptInfoID,
                                             d.ProductUseFatherDeptID,
                                             d.ProductUseDeptID,
                                             fatherName = p.DeptName,
                                             childName = c.DeptName
                                         };
                Dictionary<string, object> infoList = new Dictionary<string, object>();
                infoList.Add("inspectionInfo", inspectionInfo);
                infoList.Add("inspectionDeptList", inspectionDeptList);
                return Json(infoList);
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
                var inspectionNum = postList["inspectionNum"].ToString();//报检单编号
                var productBatchNum = postList["productBatchNum"].ToString();//产品批号
                var inspectionDept = 0;
                int.TryParse(postList["inspectionDept"].ToString(),out inspectionDept);//报检单位

                var disposeDateStart= postList["disposeDateStart"].ToString();//接收日期开始
                var disposeDateEnd = postList["disposeDateEnd"].ToString();//接收日期结束

                var result = from i in db.InspectionApplications
                             join u in db.UserInfo on i.InspectionPersonID equals u.UserID
                             where i.InspectionApplicationState == "审核通过"
                             select new {
                                 i.ArrivalDate,
                                 i.DisposeDate,
                                 i.DisposePersonID,
                                 i.DisposePersonName,
                                 i.DisposeRemark,
                                 i.InputDate,
                                 i.InspectionApplicationID,
                                 i.InspectionApplicationNum,
                                 i.InspectionApplicationState,
                                 i.InspectionDate,
                                 i.InspectionDeptID,
                                 i.InspectionDeptName,
                                 i.InspectionFartherDeptID,
                                 i.InspectionFatherDeptName,
                                 i.InspectionPersonID,
                                 i.InspectionPersonName,
                                 i.ProductBatchNum,
                                 i.ProductCount,
                                 i.ProductDealer,
                                 i.ProductFactory,
                                 i.ProductName,
                                 i.ProductPackingType,
                                 i.ProductType,
                                 i.SamplePlace,
                                 u.UserPhone
                             };

                if (!string.IsNullOrEmpty(productName))
                {
                    result = result.Where(w => w.ProductName.Contains(productName));
                }
                if (!string.IsNullOrEmpty(inspectionNum))
                {
                    result = result.Where(w => w.InspectionApplicationNum.Contains(inspectionNum));
                }
                if (!string.IsNullOrEmpty(productBatchNum))
                {
                    result = result.Where(w => w.ProductBatchNum.Contains(productBatchNum));
                }
                if (inspectionDept!=0)
                {
                    result = result.Where(w => w.InspectionFartherDeptID==inspectionDept);
                }
                if (!string.IsNullOrEmpty(disposeDateStart)&!string.IsNullOrEmpty(disposeDateEnd))
                {
                    var dateStart = Convert.ToDateTime(disposeDateStart);
                    var dateEnd = Convert.ToDateTime(disposeDateEnd);
                    result = result.Where(w=>DbFunctions.DiffDays(w.DisposeDate, dateStart) <= 0 &&DbFunctions.DiffDays(w.DisposeDate, dateEnd) >= 0);
                }
                //if (!string.IsNullOrEmpty(inspectionDate))
                //{
                //    result = result.Where(w => SqlFunctions.DateDiff("day", w.InspectionDate, inspectionDate) == 0);
                //}
                Dictionary<string, object> infoList = new Dictionary<string, object>();
                infoList.Add("count", result.Count());
                infoList.Add("infoList", result.OrderByDescending(o => o.DisposeDate).Take(pageSize * curPage).Skip(pageSize * (curPage - 1)).ToList());
                return Json(infoList);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        //修改报检单编号
        public string EditInspectionNum()
        {
            try
            {
                var postList =
   JsonConvert.DeserializeObject<Dictionary<String, Object>>(HttpUtility.UrlDecode(Request.Form.ToString()));

                var inspectionID = 0;
                int.TryParse(postList["id"].ToString(), out inspectionID);
                var inspectionNum = postList["inspectionNum"].ToString();

                var inspectionInfo = db.InspectionApplications.Find(inspectionID);

                var inspectionNumOld = inspectionInfo.InspectionApplicationNum;
                inspectionInfo.InspectionApplicationNum = inspectionNum;

                var userInfo = Session["user"] as Models.UserInfo;
                Models.Log log = new Models.Log();
                log.LogInfo = "用户【"+userInfo.UserName+"】将ID为【"+inspectionInfo.InspectionApplicationID+"】的报检单编号【"+inspectionNumOld+"】修改为【"+inspectionNum+"】";
                log.LogInputDate = DateTime.Now;
                log.LogInputPerson = userInfo.UserID;
                log.LogType = "修改报检单编号";
                log.InspectionID = inspectionInfo.InspectionApplicationID;
                db.Log.Add(log);

                db.SaveChanges();
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //回退已接收的报检单
        public string BackInspectionChecked()
        {
            try
            {
                var userInfo = Session["user"] as Models.UserInfo;
                var postList =
  JsonConvert.DeserializeObject<Dictionary<String, Object>>(HttpUtility.UrlDecode(Request.Form.ToString()));

                var inspectionID = 0;
                int.TryParse(postList["id"].ToString(), out inspectionID);
                var remark = postList["remark"].ToString();

                var inspectionInfo = db.InspectionApplications.Find(inspectionID);
                var remarkOld = inspectionInfo.DisposeRemark;

                inspectionInfo.DisposeRemark = remarkOld + remark;//回退的原因要加上报检单原来的备注信息。
                inspectionInfo.DisposePersonID = userInfo.UserID;
                inspectionInfo.DisposeDate = DateTime.Now;
                inspectionInfo.DisposePersonName = userInfo.UserName;
                inspectionInfo.InspectionApplicationState = "审核回退";

                
                Models.Log log = new Models.Log();
                log.LogInfo = "用户【" + userInfo.UserName + "】将编号为【" + inspectionInfo.InspectionApplicationNum + "】的报检单进行回退操作";
                log.LogInputDate = DateTime.Now;
                log.LogInputPerson = userInfo.UserID;
                log.LogType = "回退已接收报检单";
                log.InspectionID = inspectionInfo.InspectionApplicationID;
                db.Log.Add(log);

                db.SaveChanges();
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //获取报检单统计表，根据接收日期范围
        public FileResult InspectionToExcel()
        {
            var dateBeginRequest = Request["dateBegin"].ToString();
            var dateEndRequest = Request["dateEnd"].ToString();
            var dateBegin =Convert.ToDateTime(dateBeginRequest);
            var dateEnd =Convert.ToDateTime(dateEndRequest);

            var result = (from i in db.InspectionApplications
                         join u in db.UserInfo on i.InspectionPersonID equals u.UserID
                         where i.InspectionApplicationState == "审核通过"&
                         DbFunctions.DiffDays(i.DisposeDate, dateBegin) <= 0 &
                         DbFunctions.DiffDays(i.DisposeDate, dateEnd) >= 0
                         select new
                         {
                             i.ArrivalDate,
                             i.DisposeDate,
                             i.DisposePersonID,
                             i.DisposePersonName,
                             i.DisposeRemark,
                             i.InputDate,
                             i.InspectionApplicationID,
                             i.InspectionApplicationNum,
                             i.InspectionApplicationState,
                             i.InspectionDate,
                             i.InspectionDeptID,
                             i.InspectionDeptName,
                             i.InspectionFartherDeptID,
                             i.InspectionFatherDeptName,
                             i.InspectionPersonID,
                             i.InspectionPersonName,
                             i.ProductBatchNum,
                             i.ProductCount,
                             i.ProductDealer,
                             i.ProductFactory,
                             i.ProductName,
                             i.ProductPackingType,
                             i.ProductType,
                             i.SamplePlace,
                             u.UserPhone
                         }).ToList();

            var filename = "报检单统计信息"+dateBeginRequest+"--"+dateEndRequest;
            WorkbookDesigner designer = new WorkbookDesigner();
            string path = System.IO.Path.Combine(Server.MapPath("/"), "ExcelTemplate/ExportStatistic.xls");
            designer.Open(path);

            DataTable dt = new DataTable();
            dt.TableName = "P";

            dt.Columns.Add("Index");
            dt.Columns.Add("InspectionApplicationNum");
            dt.Columns.Add("ProductName");
            dt.Columns.Add("ProductFactory");
            dt.Columns.Add("ProductDealer");
            dt.Columns.Add("ProductPackingType");
            dt.Columns.Add("ProductBatchNum");
            dt.Columns.Add("ProductType");
            dt.Columns.Add("ProductCount");
            dt.Columns.Add("SamplePlace");
            dt.Columns.Add("ArrivalDate");
            dt.Columns.Add("InspectionDate");
            dt.Columns.Add("InspectionFatherDeptName");
            dt.Columns.Add("UserPhone");
            dt.Columns.Add("DisposePersonName");
            dt.Columns.Add("DisposeDate");
            dt.Columns.Add("DisposeRemark");

            foreach (var info in result)
            {
                DataRow dr = dt.NewRow();
                var index = result.IndexOf(info);
                dr["Index"] = (index + 1);
                dr["InspectionApplicationNum"] = info.InspectionApplicationNum;
                dr["ProductName"] = info.ProductName;
                dr["ProductFactory"] = info.ProductFactory;
                dr["ProductDealer"] = info.ProductDealer;
                dr["ProductPackingType"] = info.ProductPackingType;
                dr["ProductBatchNum"] = info.ProductBatchNum;
                dr["ProductType"] = info.ProductType;
                dr["ProductCount"] = info.ProductCount;
                dr["SamplePlace"] = info.SamplePlace;
                dr["ArrivalDate"] = info.ArrivalDate.ToShortDateString();
                dr["InspectionDate"] = info.InspectionDate.ToShortDateString();
                dr["InspectionFatherDeptName"] = info.InspectionFatherDeptName;
                dr["UserPhone"] = info.UserPhone;
                dr["DisposePersonName"] = info.DisposePersonName;
                dr["DisposeDate"] = info.DisposeDate.ToString();
                dr["DisposeRemark"] = info.DisposeRemark;
                dt.Rows.Add(dr);
            }
            designer.SetDataSource(dt);

            designer.Process();
            string fileToSave = System.IO.Path.Combine(Server.MapPath("/"), "ExcelOutPut/" + filename + ".xls");
            if (System.IO.File.Exists(fileToSave))
            {
                System.IO.File.Delete(fileToSave);
            }
            designer.Save(fileToSave, FileFormatType.Excel97To2003);
            return File(fileToSave, "application/vnd.ms-excel", filename + ".xls");
        }
    }
}