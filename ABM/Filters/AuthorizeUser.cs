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
                var userId = (User)HttpContext.Current.Session["userId"];
                _user = (User)db.User.Where(x => x.id == _user.id);
                var userTypeList = db.User.Select(x => x.TypeUser);
                foreach (var item in userTypeList)
                {
                    if(_authorizedTypes.Contains(item.id) != true)
                    {
                        filterContext.Result = new RedirectResult("~/Home/Index");
                    }
                }
                //if (_user.id )
                //{
                //    filterContext.Result = new RedirectResult("~/Home/Index");
                //}

                //if (userTypeList.ToList().Count() == 0)
                //{
                //    SE PODRIA AGREGAR UNA VIEW PARA ERRORES DE AUTORIZACION Y REDIRIGIR ALLi
                //    filterContext.Result = new RedirectResult("~/Home/Index");
                //}
            }
            catch (Exception)
            {
                filterContext.Result = new RedirectResult("~/Home/Index"); 
            }
        }
    }

}