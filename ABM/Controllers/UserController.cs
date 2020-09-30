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
            var getUsers = from u in _userRepository.GetActiveUsers()
                           select new UserViewModel()
                           {
                               Id = u.id,
                               Email = u.email,
                               Name = u.name
                           };
            return View(getUsers.ToList());
        }


        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        public ActionResult Create(UserViewModel model)
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

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UserEditViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                _userRepository.UpdateUser(userViewModel);
                _userRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(userViewModel);

        }
        

        public ActionResult Delete(int id)
        {
            _userRepository.DeleteUser(id);
            return RedirectToAction("Index", "User");
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
            UserViewModel usm = new UserViewModel

            {
                Email = collection["Email"].ToString(),
                Pass = collection["Pass"].ToString()
            };

            var getUser = _userRepository.GetUserByUserMail(usm.Email);
            // var dbPass = usm.Pass;

            try
            {
                if (usm.Pass.Equals(getUser.pass))
                {
                    return RedirectToAction("index", "Home");
                }
                return View();
            }
            catch (Exception)
            {
                ViewBag.Message = "No se pudo loguear";
                return View();


            }




        }
    }

}
