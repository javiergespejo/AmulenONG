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
    public class AuthorizeUser: AuthorizeAttribute
    {
        private User _user;
        AmulenEntities db = new AmulenEntities();
        private List<int> arrayTipo = new List<int>();

        public AuthorizeUser(int idTipo = 0, int secondaryTipo = 0)
        {
            arrayTipo.Add(idTipo);
            arrayTipo.Add(secondaryTipo);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                _user = (User)HttpContext.Current.Session["User"];
                var userTypeList = from m in db.TypeUser
                                   where m.id == _user.id
                                   && (m.id == arrayTipo[0] || m.id == arrayTipo[1])
                                   select m;


                if (userTypeList.ToList().Count() == 0)
                {
                    // SE PODRIA AGREGAR UNA VIEW PARA ERRORES DE AUTORIZACION Y REDIRIGIR ALLi
                    filterContext.Result = new RedirectResult("~/Home/Index");
                }
            }
            catch(Exception)
            {
                filterContext.Result = new RedirectResult("~/Home/Index"); 
            }
        }
    }
}