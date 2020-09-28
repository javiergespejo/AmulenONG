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
        UnitOfWork unit = new UnitOfWork();


        // GET: Users
        public ActionResult Index()
        {
            var getUsers = unit.UserRepository.GetActiveUsers();

            UserViewModel userViewModel = new UserViewModel
            {
                users = getUsers.ToList()
            };

            return View(userViewModel);
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var user = unit.UserRepository.GetByID(id);
            UserDetailsViewModel userDetails = new UserDetailsViewModel
            {
                Name = user.name,
                Username = user.username,
                Email = user.email
            };

            return View(userDetails);
        }
    }
}
