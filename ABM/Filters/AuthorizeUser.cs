using ABM.Models;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ABM.Repository;

namespace ABM.Filters
{
    
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorizeUser : AuthorizeAttribute
    {
        private User _user;
        AmulenEntities db = new AmulenEntities();
        private int[] _authorizedTypes;

        public AuthorizeUser(int[] authorizedTypes)
        {
            this._authorizedTypes = authorizedTypes;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                // SE PODRIA CAMBIAR PARA QUE SE GUARDE UNA ID EN VES DE USER COMPLETO Y BUSCARLO EN REPO
                _user = (User)HttpContext.Current.Session["User"];
                var userTypeList = db.User.Select(x => x.TypeUser);
                if (_authorizedTypes.Count() == 1)
                {
                    if (!(_authorizedTypes.Contains(userTypeList.First().id) && _authorizedTypes.Contains(_user.typeUserId)))
                    {
                        filterContext.Result = new RedirectResult("~/Home/Index");
                    }
                }else
                {
                    foreach (var item in userTypeList)
                    {
                        if ((_authorizedTypes.Contains(item.id) && _authorizedTypes.Contains(_user.typeUserId)) != true)
                        {
                            filterContext.Result = new RedirectResult("~/Home/Index");
                        }
                    }
                }   
            }
            catch (NullReferenceException)
            {
                filterContext.Result = new RedirectResult("~/Home/Index"); 
            }
        }
    }

}