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

        // FALTA IMPLEMENTAR AUTENTICACION
        //[Authorize]
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
        [HttpGet]
        public ActionResult UpdatePassword(int id)
        {
            var user = unit.UserRepository.GetByID(id);
            UserUpdatePasswordModel userUpdate = new UserUpdatePasswordModel
            {
                Id = id,
                Username = user.username,
                Password = user.pass
            };

            return View(userUpdate);
        }

        /// FALTA IMPLEMENTAR AUTENTICACION
        [HttpPost]
        public ActionResult UpdatePassword(UserUpdatePasswordModel userUpdated)
        {

            var user = unit.UserRepository.GetByID(userUpdated.Id);
            user.pass = userUpdated.Password;
            unit.UserRepository.Update(user);
            return RedirectToAction("Index", "User");
        }



    }
}
