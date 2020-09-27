using ABM.Models;
using ABM.ViewModels;
using ABM.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABM.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserRepository _userRepository;

        public LoginController()
        {
            _userRepository = new UserRepository(new AmulenEntities());
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)

        { 
            User us = new User

            {
                username = collection["username"].ToString(),
                pass = collection["pass"].ToString()
            }; 

                var getUser = _userRepository.GetUserByUserName(us.username);
                var dbPass = getUser.pass;

                if (us.pass.Equals(dbPass))
                {
                return RedirectToAction("Home/Index");
                }

                return View();
            
           
        }

    }
}