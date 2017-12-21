using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspectionApplication.Controllers
{
    public class UserController : Controller
    {
        private Models.DbContext db = new Models.DbContext();

        //加载用户信息视图
        public ViewResult List()
        {
            ViewBag.Title = "用户管理";
            return View();
        }

        //加载用户下拉列表信息
        public JsonResult GetUserDropDownListInfo()
        {
            try
            {
                return Json(db.UserInfo.ToList());
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        //用户修改电话、备注信息
        public string EditByUser()
        {
            try
            {
                var infoList =
   JsonConvert.DeserializeObject<Dictionary<String, Object>>(HttpUtility.UrlDecode(Request.Form.ToString()));

                var id = 0;
                int.TryParse(infoList["id"].ToString(), out id);

                var remark = infoList["remark"].ToString();
                var phone = infoList["phone"].ToString();
                var userInfo = db.UserInfo.Find(id);
                userInfo.UserPhone = phone;
                userInfo.UserRemark = remark;
                db.SaveChanges();
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //根据ID获取用户信息
        public JsonResult GetUserInfoByID()
        {
            try
            {
                var postList =
       JsonConvert.DeserializeObject<Dictionary<String, Object>>(HttpUtility.UrlDecode(Request.Form.ToString()));

                var userID = 0;
                int.TryParse(postList["id"].ToString(), out userID);//用户ID

                var result = from u in db.UserInfo
                             join d in db.DeptInfo on u.UserDeptID equals d.DeptID
                             where u.UserID==userID&u.UserState==0
                             select new
                             {
                                 u.UserID,
                                 u.UserName,
                                 u.UserNum,
                                 u.UserPhone,
                                 u.UserRemark,
                                 u.UserRole,
                                 u.UserState,
                                 u.UserDeptID,
                                 d.DeptFatherID
                             };
                return Json(result.ToList());
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        //管理员删除用户信息，写入操作日志
        public string Del()
        {
            try
            {
                var infoList =
   JsonConvert.DeserializeObject<Dictionary<String, Object>>(HttpUtility.UrlDecode(Request.Form.ToString()));

                var id = 0;
                int.TryParse(infoList["id"].ToString(), out id);

                var userInfo = db.UserInfo.Find(id);
                userInfo.UserState = 1;//将用户状态信息标记为【1】删除状态

                Models.Log log = new Models.Log();
                log.LogInfo = "执行删除用户操作,员工编号：" + userInfo.UserNum + ",姓名：" + userInfo.UserName + "";
                log.LogInputDate = DateTime.Now;
                log.LogInputPerson = userInfo.UserID;
                log.LogType = "删除用户信息";
                log.InspectionID = userInfo.UserID;
                db.Log.Add(log);
                db.SaveChanges();
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //管理员添加、修改用户的权限、单位、姓名信息
        public string EditByAdmin()
        {
            try
            {
                var infoList =
   JsonConvert.DeserializeObject<Dictionary<String, Object>>(HttpUtility.UrlDecode(Request.Form.ToString()));

                var userID = 0;
                int.TryParse(infoList["useID"].ToString(), out userID);

                var userdeptID = 0;
                int.TryParse(infoList["userDeptID"].ToString(), out userdeptID);

                var userName = infoList["userName"].ToString();
                var userNum = infoList["userNum"].ToString();
                var userRemark = infoList["userRemark"].ToString();
                var userPhone = infoList["userPhone"].ToString();
                var userRole = infoList["userRole"].ToString();

                if (userID==0)
                {
                    var userInfo = new Models.UserInfo();
                    userInfo.UserName = userName;
                    userInfo.UserRole = userRole;
                    userInfo.UserPhone = userPhone;
                    userInfo.UserNum = userNum;
                    userInfo.UserRemark = userRemark;
                    userInfo.UserDeptID = userdeptID;
                    userInfo.UserState = 0;
                    db.UserInfo.Add(userInfo);
                    db.SaveChanges();
                    return "ok";
                }
                else
                {
                    var userInfo = db.UserInfo.Find(userID);
                    userInfo.UserName =userName;
                    userInfo.UserRole = userRole;
                    userInfo.UserPhone = userPhone;
                    userInfo.UserNum = userNum;
                    userInfo.UserRemark = userRemark;
                    userInfo.UserDeptID = userdeptID;
                    db.SaveChanges();
                    return "ok";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //获取用户信息列表--分页
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

                var userName = postList["userName"].ToString();//员工姓名
                var userNum = postList["userNum"].ToString();//员工编号
                var userRole= postList["userRole"].ToString();//员工权限

                var userDeptFatherID = 0;
                int.TryParse(postList["userDeptFatherID"].ToString(), out userDeptFatherID);//用户二级单位ID

                var result = from u in db.UserInfo
                             join d in db.DeptInfo on u.UserDeptID equals d.DeptID
                             join f in db.DeptInfo on d.DeptFatherID equals f.DeptID
                             where u.UserState==0
                             select new
                             {
                                 u.UserID,
                                 u.UserName,
                                 u.UserNum,
                                 u.UserPhone,
                                 u.UserRemark,
                                 u.UserRole,
                                 u.UserState,
                                 u.UserDeptID,
                                 d.DeptFatherID,
                                 d.DeptName,
                                 fatherDeptName=f.DeptName
                             };
                if (!string.IsNullOrEmpty(userName))
                {
                    result = result.Where(w => w.UserName.Contains(userName));
                }
                if (!string.IsNullOrEmpty(userNum))
                {
                    result = result.Where(w => w.UserNum.Contains(userNum));
                }
                if (!string.IsNullOrEmpty(userRole))
                {
                    result = result.Where(w => w.UserRole.Contains(userRole));
                }
                if (userDeptFatherID != 0)
                {
                    result = result.Where(w => w.DeptFatherID == userDeptFatherID);
                }
                Dictionary<string, object> infoList = new Dictionary<string, object>();
                infoList.Add("count", result.Count());
                infoList.Add("infoList", result.OrderBy(o => o.UserNum).Take(pageSize * curPage).Skip(pageSize * (curPage - 1)).ToList());
                return Json(infoList);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}