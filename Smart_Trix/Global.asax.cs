using Smart_Trix.Models.StData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;//IIdentity
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Smart_Trix
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        //******************For Role  of user and admin****************//
        protected void Appliction_Authentication()
        {
            //check if the user is logged in
            if (User==null)
            {
                return;
            }

            //Get username
            string username = Context.User.Identity.Name;

            //Declare the array of roles
            string[] roles = null;


            using (SmartDB db=new SmartDB())
            {
                //populates the roles
                StUserDT dt = db.Users.FirstOrDefault(a => a.UserName == username);

                roles = db.UserRoles.Where(a => a.UserId == dt.Id).Select(a => a.Roles.Name).ToArray();
            }

            //Build the principle object
            IIdentity userIdentity = new GenericIdentity(username);
            IPrincipal newUserObj = new GenericPrincipal(userIdentity,roles);

            //Update the context the user
            Context.User = newUserObj;

        }

     



   

    }
}
