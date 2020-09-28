using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
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

        public ActionResult Details(int id)
        {
            User user = _userRepository.GetUserById(id);

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            User user = _userRepository.GetUserById(id);

            if (user == null)
            {
                return View("Error");
            }

            // VER
            // Lista hardcodeada porque no hay TypeUserRepository ni metodo para traerlo desde UserRepository.
            List<SelectListItem> typeUserList = new List<SelectListItem>();
            typeUserList.Add(new SelectListItem() { Text = "Administrador", Value = "1" });
            typeUserList.Add(new SelectListItem() { Text = "Suscriptor", Value = "2" });

            ViewBag.TypeUser = typeUserList;
            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,[Bind(Include = "id,name,email,username,pass,typeUserId,isActive")] User user)
        {
            if (ModelState.IsValid)
            {
                _userRepository.UpdateUser(user);
                _userRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
    }
}
