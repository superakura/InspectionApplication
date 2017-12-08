using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspectionApplication.Controllers
{
    public class AddController : Controller
    {
        private Models.DbContext db = new Models.DbContext();

        public ActionResult ListAdded()
        {
            ViewBag.Title = "我的报检单";
            return View();
        }

        public ViewResult MaterialList()
        {
            ViewBag.Title = "添加报检单";
            return View();
        }

        //获取生产辅料的全部信息
        public JsonResult GetMaterialInfo()
        {
            string sql = @"select  Material_Name,ExecutionStandard_Name,Material_ID,ExecutionStandard_Type,
[MaterialType_Name]+'&' as  MaterialType,
Execution_Date
from [View_MaterialInfo] order by Material_Type";

            System.Collections.Generic.List<Material> list_mt = new System.Collections.Generic.List<Material>();
            System.Data.DataSet ds = DB2.InitDs(sql, "t");

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Material mt = new Material();
                mt.Material_Name = ds.Tables[0].Rows[i][0].ToString();
                mt.ExecutionStandard_Name = ds.Tables[0].Rows[i][1].ToString();
                mt.Material_ID = Convert.ToInt32(ds.Tables[0].Rows[i][2]);
                mt.MaterialType_Name = ds.Tables[0].Rows[i][4].ToString();
                mt.ExecutionStandard_Type = ds.Tables[0].Rows[i][3].ToString();
                string sql_Dept = "select * from View_MaterialDept where Material_ID=" + mt.Material_ID + "";
                System.Data.DataSet ds_MaterialDept = DB2.InitDs(sql_Dept, "t2");
                for (int x = 0; x < ds_MaterialDept.Tables[0].Rows.Count; x++)
                {
                    mt.dept += ds_MaterialDept.Tables[0].Rows[x][0].ToString() + ";";
                }
                list_mt.Add(mt);
            }
            return Json(list_mt, JsonRequestBehavior.AllowGet);
        }

        //获取生产辅料的全部信息
        public JsonResult GetMaterialInfoByID()
        {
            var postList =
   JsonConvert.DeserializeObject<Dictionary<String, Object>>(HttpUtility.UrlDecode(Request.Form.ToString()));

            var id = 0;
            int.TryParse(postList["id"].ToString(), out id);

            string sql = @"select * from [View_MaterialInfo] where Material_ID="+id+"";
            System.Data.DataSet ds = DB2.InitDs(sql, "t");

            Material mt = new Material();
            mt.Material_Name= ds.Tables[0].Rows[0]["Material_Name"].ToString();
            mt.Material_Request = ds.Tables[0].Rows[0]["Material_Request"].ToString();
            mt.ExecutionStandard_Name= ds.Tables[0].Rows[0]["ExecutionStandard_Name"].ToString();
            mt.ExecutionStandard_Type= ds.Tables[0].Rows[0]["ExecutionStandard_Type"].ToString();
            mt.ExecutionStandard_File = ds.Tables[0].Rows[0]["ExecutionStandard_File"].ToString();
            mt.TechnicalProtocol_Name= ds.Tables[0].Rows[0]["TechnicalProtocol_Name"].ToString();
            mt.TechnicalProtocol_File = ds.Tables[0].Rows[0]["TechnicalProtocol_File"].ToString();

            string sql_Dept = "select * from View_MaterialDept where Material_ID=" + id + "";
            System.Data.DataSet ds_MaterialDept = DB2.InitDs(sql_Dept, "t2");
            mt.dept += "<ul class='list-unstyled'>";
            for (int x = 0; x < ds_MaterialDept.Tables[0].Rows.Count; x++)
            {
                mt.dept +="<li>" +ds_MaterialDept.Tables[0].Rows[x][0].ToString() + ";</li>";
            }
            mt.dept += "</ul>";
            return Json(mt);
        }

        //获取生产辅料的类型
        public JsonResult GetMaterialType()
        {
            string sql = "select * from Material_Type order by MaterialTypeOrder";
            System.Collections.Generic.List<Material_Type> list_mt = new System.Collections.Generic.List<Material_Type>();
            System.Data.DataSet ds = DB2.InitDs(sql, "t");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Material_Type mt = new Material_Type();
                mt.MaterialType_ID = Convert.ToInt32(ds.Tables[0].Rows[i][0]);
                mt.MaterialType_Name = ds.Tables[0].Rows[i][1].ToString();
                list_mt.Add(mt);
            }
            return Json(list_mt, JsonRequestBehavior.AllowGet);
        }

        //获取生产辅料的使用部门
        public JsonResult GetMaterialDept()
        {
            string sql = @"select Dept_Name from Dept_Info 
where Dept_ParentID=1 and Dept_Order is not null order by Dept_Order";
            System.Collections.Generic.List<MaterialDept> list_mt = new System.Collections.Generic.List<MaterialDept>();
            System.Data.DataSet ds = DB2.InitDs(sql, "t");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                MaterialDept mt = new MaterialDept();
                mt.Dept_Name = ds.Tables[0].Rows[i][0].ToString();
                list_mt.Add(mt);
            }
            return Json(list_mt, JsonRequestBehavior.AllowGet);
        }

        //提交报检单
        public string Submit()
        {
            try
            {
                var personInfo = Session["user"] as Models.UserInfo;
                var postList =
   JsonConvert.DeserializeObject<Dictionary<String, Object>>(HttpUtility.UrlDecode(Request.Form.ToString()));

                var productName = postList["productName"].ToString();
                var productDealer = postList["productDealer"].ToString();
                var productPackingType = postList["productPackingType"].ToString();
                var arrivalDate =Convert.ToDateTime(postList["arrivalDate"].ToString());
                var productFactory = postList["productFactory"].ToString();
                var productType = postList["productType"].ToString();
                var productBatchNum = postList["productBatchNum"].ToString();
                var samplePlace= postList["samplePlace"].ToString();
                var productCount = postList["productCount"].ToString();
                var inspectionDate =Convert.ToDateTime(postList["inspectionDate"].ToString());

                Models.InspectionApplications inspectionApplications = new Models.InspectionApplications();
                inspectionApplications.ProductName = productName;
                inspectionApplications.ProductDealer = productDealer;
                inspectionApplications.ProductPackingType=productPackingType;
                inspectionApplications.ArrivalDate = arrivalDate;
                inspectionApplications.ProductFactory = productFactory;
                inspectionApplications.ProductType = productType;
                inspectionApplications.ProductBatchNum = productBatchNum;
                inspectionApplications.ProductCount = productCount;
                inspectionApplications.SamplePlace = samplePlace;

                inspectionApplications.InspectionPersonID = personInfo.UserID;
                inspectionApplications.InspectionPersonName = personInfo.UserName;

                var deptInfo = db.DeptInfo.Find(personInfo.UserDeptID);
                var deptFatherInfo = db.DeptInfo.Find(deptInfo.DeptFatherID);
                inspectionApplications.InspectionDeptID = personInfo.UserDeptID;
                inspectionApplications.InspectionDeptName =deptInfo.DeptName;
                inspectionApplications.InspectionFartherDeptID = deptFatherInfo.DeptID;
                inspectionApplications.InspectionFatherDeptName = deptFatherInfo.DeptName;
                inspectionApplications.InspectionDate = inspectionDate;
                inspectionApplications.InspectionApplicationState = "待审核";

                inspectionApplications.InputDate = DateTime.Now;

                db.InspectionApplications.Add(inspectionApplications);
                db.SaveChanges();

                Dictionary<string, object> deptList =
                               JsonConvert.DeserializeObject<Dictionary<String, Object>>(postList["deptList"].ToString());
                foreach (var item in deptList)
                {
                    Dictionary<string, object> list=
                        JsonConvert.DeserializeObject<Dictionary<String, Object>>(item.Value.ToString());
                    var fatherID = 0;
                    var childID = 0;
                    int.TryParse(list["deptFatherID"].ToString(), out fatherID);
                    int.TryParse(list["deptChildID"].ToString(), out childID);

                    Models.ProductUseDept productUseDept = new Models.ProductUseDept();
                    productUseDept.InspectionApplicationID = inspectionApplications.InspectionApplicationID;
                    productUseDept.ProductUseDeptID = childID;
                    productUseDept.ProductUseFatherDeptID = fatherID;
                    db.ProductUseDept.Add(productUseDept);
                    db.SaveChanges();
                }
                return "ok";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        //删除质检回退的报检单
        public string Del()
        {
            try
            {
                var infoList =
   JsonConvert.DeserializeObject<Dictionary<String, Object>>(HttpUtility.UrlDecode(Request.Form.ToString()));

                var id = 0;
                int.TryParse(infoList["id"].ToString(), out id);

                var inspectionApplicationInfo = db.InspectionApplications.Find(id);

                if (inspectionApplicationInfo.InspectionApplicationState== "审核回退")
                {
                    IEnumerable<Models.ProductUseDept> delDeptList = db.ProductUseDept
                    .Where(w => w.InspectionApplicationID == id)
                    .ToList();
                    db.ProductUseDept.RemoveRange(delDeptList);
                    db.InspectionApplications.Remove(inspectionApplicationInfo);
                    db.SaveChanges();
                    return "ok";
                }
                else
                {
                    return "noBack";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //获取我的报检单列表
        public JsonResult GetInspectionList()
        {
            try
            {
                var userInfo = Session["user"] as Models.UserInfo;

                var postList =
       JsonConvert.DeserializeObject<Dictionary<String, Object>>(HttpUtility.UrlDecode(Request.Form.ToString()));

                var curPage = 0;
                int.TryParse(postList["curPage"].ToString(), out curPage);

                var pageSize = 0;
                int.TryParse(postList["pageSize"].ToString(), out pageSize);

                var productName = postList["productName"].ToString();//产品名称
                var productBatch = postList["productBatchNum"].ToString();//产品批号
                var inspectionDate = postList["inspectionDate"].ToString();//报检时间
                var inspectionState = postList["inspectionState"].ToString();//报检单状态

                var result = from i in db.InspectionApplications
                             where i.InspectionPersonID == userInfo.UserID
                             select i;

                if (!string.IsNullOrEmpty(productName))
                {
                    result = result.Where(w => w.ProductName.Contains(productName));
                }
                if (!string.IsNullOrEmpty(productBatch))
                {
                    result = result.Where(w => w.ProductBatchNum.Contains(productBatch));
                }
                if (!string.IsNullOrEmpty(inspectionState))
                {
                    result = result.Where(w => w.InspectionApplicationState == inspectionState);
                }
                if (!string.IsNullOrEmpty(inspectionDate))
                {
                    result = result.Where(w => SqlFunctions.DateDiff("day", w.InspectionDate, inspectionDate) == 0);
                }

                Dictionary<string, object> infoList = new Dictionary<string, object>();
                infoList.Add("count", result.Count());
                infoList.Add("infoList", result.OrderByDescending(o => o.InputDate).Take(pageSize * curPage).Skip(pageSize * (curPage - 1)).ToList());
                return Json(infoList);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        //根据报检单ID获取报检单详细信息
        public JsonResult GetInspectionDetail()
        {
            try
            {
                var postList =
   JsonConvert.DeserializeObject<Dictionary<String, Object>>(HttpUtility.UrlDecode(Request.Form.ToString()));

                var id = 0;
                int.TryParse(postList["id"].ToString(), out id);
                var inspectionInfo = db.InspectionApplications.Find(id);

                var inspectionDeptList = from d in db.ProductUseDept
                                         join p in db.DeptInfo on d.ProductUseFatherDeptID equals p.DeptID
                                         join c in db.DeptInfo on d.ProductUseDeptID equals c.DeptID
                                         where d.InspectionApplicationID == id
                                         select new
                                         {
                                             d.ProductUseDeptInfoID,
                                             d.ProductUseFatherDeptID,
                                             d.ProductUseDeptID,
                                             fatherName =p.DeptName,
                                             childName=c.DeptName
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
    }
}