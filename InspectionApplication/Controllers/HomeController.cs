using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace InspectionApplication.Controllers
{
    public class HomeController : Controller
    {
        private Models.DbContext db = new Models.DbContext();

        //登录页面
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        //用户登录、加载用户权限
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string userID, string pwd, string returnUrl)
        {
            var userInfo = db.UserInfo.Where(w => w.UserNum == userID & w.UserState == 0).FirstOrDefault();
            if (userInfo != null)
            {
                System.Web.HttpContext.Current.Session["user"] = userInfo;
                string strConnection = "user id=KqLogin;password=rjkf3877;initial catalog = GM_MT; Server = 10.126.10.54";
                using (SqlConnection conn = new SqlConnection(strConnection))
                {
                    try
                    {
                        var isRight = string.Empty;
                        #region 考勤登录代码
                        conn.Open();
                        using (SqlCommand sqlComm = conn.CreateCommand())
                        {
                            sqlComm.CommandText = "CheckId";
                            sqlComm.CommandType = CommandType.StoredProcedure;

                            //工号
                            SqlParameter employeeId = sqlComm.Parameters.Add(new

        SqlParameter("@employeeId", SqlDbType.NVarChar, 20));
                            employeeId.Direction = ParameterDirection.Input;
                            employeeId.Value = userID;

                            //密码
                            SqlParameter sqlPwd = sqlComm.Parameters.Add(new

        SqlParameter("@pwd", SqlDbType.NVarChar, 20));
                            sqlPwd.Direction = ParameterDirection.Input;
                            sqlPwd.Value = pwd;

                            //返回行数
                            SqlParameter result = sqlComm.Parameters.Add(new

        SqlParameter("@result", SqlDbType.NVarChar, 20));
                            result.Direction = ParameterDirection.Output;
                            sqlComm.ExecuteNonQuery();
                            isRight = sqlComm.Parameters["@result"].Value.ToString();
                            conn.Close();
                        }
                        #endregion
                        //加true，进行测试用，上线后删除true
                        if (isRight == "1"||true)
                        {
                            #region 写入权限，登录转跳
                            FormsAuthenticationTicket authTicket =
                                new FormsAuthenticationTicket(1,userID,DateTime.Now,DateTime.Now.AddMinutes(20),false,userInfo.UserRole);

                            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                            //设置用户身份验证的cookie
                            System.Web.HttpCookie authCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                            System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);

                            //设置用户姓名的cookie
                            var cUserName = System.Web.HttpContext.Current.Server.UrlEncode(userInfo.UserName);
                            System.Web.HttpCookie userNameCookie = new System.Web.HttpCookie("cUserName", cUserName);
                            System.Web.HttpContext.Current.Response.Cookies.Add(userNameCookie);

                            var action = string.Empty;
                            var controller = string.Empty;

                            switch (userInfo.UserRole)
                            {
                                case "报检单申请":
                                    action = "MaterialList";
                                    controller = "Add";
                                    break;
                                case "报检单审批":
                                    action = "IndexList";
                                    controller = "Check";
                                    break;
                                case "系统管理员":
                                    action = "IndexList";
                                    controller = "Check";
                                    break;
                            }

                            return Redirect(returnUrl ?? Url.Action(action, controller));
                            #endregion
                        }
                        else
                        {
                            ModelState.AddModelError("", "用户名或密码错误！");
                            return View();
                        }
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e.Message);
                        return View();
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "您还不是此系统用户，如有疑问请联系管理员！");
                return View();
            }
        }

        //注销
        public ActionResult LoginOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("/Home/Login");
        }

        //加载菜单
        public PartialViewResult Menu()
        {
            var userInfo = Session["user"] as Models.UserInfo;
            List<Models.ViewMenu> userMenu = new List<Models.ViewMenu>();
            switch (userInfo.UserRole)
            {
                case "报检单申请":
                    userMenu.Add(new Models.ViewMenu
                    {
                        MenuName = "添加报检单",
                        MenuIcon = "glyphicon glyphicon-plus",
                        MenuController = "Add",
                        MenuAction = "MaterialList"
                    });
                    userMenu.Add(new Models.ViewMenu
                    {
                        MenuName = "我的报检单",
                        MenuIcon = "glyphicon glyphicon-list-alt",
                        MenuController = "Add",
                        MenuAction = "ListAdded"
                    });
                    break;
                case "报检单审批":
                    userMenu.Add(new Models.ViewMenu
                    {
                        MenuName = "报检单审批",
                        MenuIcon = "glyphicon glyphicon-check",
                        MenuController = "Check",
                        MenuAction = "IndexList"
                    });
                    userMenu.Add(new Models.ViewMenu
                    {
                        MenuName = "报检单统计",
                        MenuIcon = "glyphicon glyphicon-list-alt",
                        MenuController = "Check",
                        MenuAction = "IndexStatistic"
                    });
                    break;
                case "系统管理员":
                    userMenu.Add(new Models.ViewMenu
                    {
                        MenuName = "报检单审批",
                        MenuIcon = "glyphicon glyphicon-check",
                        MenuController = "Check",
                        MenuAction = "IndexList"
                    });
                    userMenu.Add(new Models.ViewMenu
                    {
                        MenuName = "报检单统计",
                        MenuIcon = "glyphicon glyphicon-list-alt",
                        MenuController = "Check",
                        MenuAction = "IndexStatistic"
                    });
                    userMenu.Add(new Models.ViewMenu
                    {
                        MenuName = "用户管理",
                        MenuIcon = "glyphicon glyphicon-user",
                        MenuController = "User",
                        MenuAction = "List"
                    });
                    break;
            }
            return PartialView(userMenu);
        }

        //加载二级单位
        public JsonResult GetFatherDept()
        {
            var list = db.DeptInfo
                .Where(w => w.DeptFatherID == 1)
                .OrderBy(o=>o.DeptOrder)
                .Select(s=>new {s.DeptID,s.DeptName})
                .ToList();
            return Json(list);
        }
    }
}