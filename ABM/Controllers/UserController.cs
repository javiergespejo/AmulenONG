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
                    bool mailAlreadyExists = _userRepository.CheckMail(model.ToEntity());
                    bool nameAlreadyExists = _userRepository.CheckUserName(model.ToEntity());

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
                    _userRepository.InsertUser(model.ToEntity());
                }
                return RedirectToAction("Index", "User");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public ActionResult Edit(int id)
        {
            User user = _userRepository.GetByID(id);
            UserEditViewModel userEditViewModel = new UserEditViewModel(user);

            if (userEditViewModel == null)
            {
                return View("Error");
            }

            return View(userEditViewModel);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserEditViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                bool mailAlreadyExists = _userRepository.CheckMail(userViewModel.ToUserEntity());
                bool nameAlreadyExists = _userRepository.CheckUserName(userViewModel.ToUserEntity());

                if (mailAlreadyExists || nameAlreadyExists)
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
                _userRepository.UpdateUser(userViewModel.ToUserEntity());
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

        //TODO: traer el edit del view


    }

}
