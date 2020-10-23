using ABM.Controllers;
using ABM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABM.Filters
{
    public class VerifySession : ActionFilterAttribute
    {
        private User _user;
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                base.OnActionExecuted(filterContext);
                //GETS USER FROM SESSION AND CONVERTS
                _user = (User)HttpContext.Current.Session["User"];

                if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), false)
                && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), false)
                && (_user == null))
                {
                    if (filterContext.Controller is UserController == false)
                    {
                        filterContext.HttpContext.Response.Redirect("/User/Login", false);
                    }
                }
            }
            catch(Exception)
            {
                filterContext.Result = new RedirectResult("~/User/Login");
            }
            
        }
    }
}