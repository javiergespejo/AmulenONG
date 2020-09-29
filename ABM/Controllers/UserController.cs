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
            return View(getUsers.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        public ActionResult Create(User model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool mailAlreadyExists = _userRepository.CheckMail(model);
                    bool nameAlreadyExists = _userRepository.CheckUserName(model);
                    if (nameAlreadyExists || mailAlreadyExists)
                    {
                        if (mailAlreadyExists)
                        {
                            ModelState.AddModelError("email", "Email no disponible!");
                        }
                        if (nameAlreadyExists)
                        {
                            ModelState.AddModelError("username", "Nombre de usuario no disponible!");
                        }
                        return View();
                    }

                    _userRepository.InsertUser(model);
                }
                return RedirectToAction("Index", "User");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public ActionResult Delete(int id)
        {
            _userRepository.DeleteUser(id);
            return RedirectToAction("Index", "User");
        }
    }
}
