using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
                var productCount = postList["productCount"].ToString();
                var inspectionDate =Convert.ToDateTime(postList["inspectionDate"].ToString());

                Models.InspectionApplications inspectionApplications = new Models.InspectionApplications();
                inspectionApplications.ProductName = productName;
                inspectionApplications.ProductDealer = productDealer;
                inspectionApplications.ProductPackingType=productPackingType;
                inspectionApplications.ArrivalDate = arrivalDate;
                inspectionApplications.ProductFactory = productFactory;
                inspectionApplications.ProductType = productType;
                inspectionApplications.ProductCount = productCount;

                inspectionApplications.InspectionPersonID = personInfo.UserID;
                inspectionApplications.InspectionPersonName = personInfo.UserName;
                inspectionApplications.InspectionDeptID = personInfo.UserDeptID;
                inspectionApplications.InspectionDeptName =db.DeptInfo.Find(personInfo.UserDeptID).DeptName;
                inspectionApplications.InspectionDate = DateTime.Now;

                inspectionApplications.InspectionApplicationState = "待审核";

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

        public string Del()
        {
            return "";
        }
    }
}