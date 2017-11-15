using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace InspectionApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        public MvcApplication()
        {
            AuthorizeRequest += new EventHandler(Application_AuthenticateRequest);
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            var isAjax = Context.Request.Headers.Get("x-requested-with");
            if (isAjax == "XMLHttpRequest")
            {
                if (authCookie == null || authCookie.Value == "")
                {
                    Context.Response.StatusCode = 499;
                }
            }

            if (authCookie == null || authCookie.Value == "")
            {
                return;
            }
            else
            {
                FormsAuthenticationTicket authTicket = null;
                try
                {
                    authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                }
                catch
                {
                    return;
                }
                string[] roles = authTicket.UserData.Split(new char[] { ',' });
                if (Context.User != null)
                {
                    Context.User = new System.Security.Principal.GenericPrincipal(Context.User.Identity, roles);
                }
            }
        }
    }
}
