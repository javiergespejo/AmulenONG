using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ABM.Models;
using ABM.Repository;
using ABM.ViewModels;

namespace ABM.Controllers
{
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository;

        public UserController()
        {
            _userRepository = new UserRepository(new AmulenEntities());
        }

        // GET: Users
        public ActionResult Index()
        {
            var getUsers = _userRepository.GetUsers();

            UserViewModel userViewModel = new UserViewModel
            {
                users = getUsers.ToList()
            };

            return View(userViewModel);
        }

        //GET LOGIN
        [HttpGet]
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
                return RedirectToAction("index", "Home");
            }

            return View();


        }
    }
}
