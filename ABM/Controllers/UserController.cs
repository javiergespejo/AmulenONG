using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ABM.Filters;
using ABM.Models;
using ABM.Repository;
using ABM.ViewModels;

namespace ABM.Controllers
{
    public class UserController : Controller
    {
        UnitOfWork unit = new UnitOfWork();
        const int administrador = 1;
        const int suscriptor = 2;
        
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

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UserEditViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
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

        // FALTA IMPLEMENTAR AUTENTICACION
        //[Authorize]
        public ActionResult Details(int id)
        {
            var user = unit.UserRepository.GetByID(id);
            UserDetailsViewModel userDetails = new UserDetailsViewModel();
            userDetails.SetProperties(user);

            return View(userDetails);
        }
        [HttpGet]
        public ActionResult UpdatePassword(int id)
        {
            var user = unit.UserRepository.GetByID(id);
            UserUpdatePasswordModel userUpdate = new UserUpdatePasswordModel();
            userUpdate.SetProperties(user);

            return View(userUpdate);
        }

        /// FALTA IMPLEMENTAR AUTENTICACION
        /// FALTA ENCRIPTAR LA CONTRASEÑA, USAR METODO DE ENCRIPTACION DE JUAN
        [HttpPost]
        //[AuthorizeUser(idTipo: administrador)]
        public ActionResult UpdatePassword(UserUpdatePasswordModel userUpdated)
        {

            var user = unit.UserRepository.GetByID(userUpdated.Id);
            userUpdated.ParseToUser(user);
            unit.UserRepository.Update(user);
            unit.UserRepository.Save();
            return RedirectToAction("Index", "User");
        }



    }

}
