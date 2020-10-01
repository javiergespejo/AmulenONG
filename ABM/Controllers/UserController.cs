using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography.Xml;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ABM.Filters;
using ABM.Models;
using ABM.Repository;
using ABM.ViewModels;
using static ABM.Repository.UserRepository;

namespace ABM.Controllers
{
    public class UserController : Controller
    {
        private UnitOfWork unit = new UnitOfWork();
        private readonly UserRepository _userRepository;
        public UserController()
        {
            _userRepository = new UserRepository(new AmulenEntities());
        }
        const int administrador = 1;
        const int suscriptor = 2;

        // GET: Users
        [AllowAnonymous]
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

        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [AllowAnonymous]
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
        [AllowAnonymous]
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
        [AllowAnonymous]
        public ActionResult Delete(int id)
        {
            _userRepository.DeleteUser(id);
            return RedirectToAction("Index", "User");
        }

        // FALTA IMPLEMENTAR AUTENTICACION
        [AuthorizeUser(new int[]{ administrador })]
        public ActionResult Details(int id)
        {
            var user = unit.UserRepository.GetByID(id);
            UserViewModel userDetails = new UserViewModel();
            userDetails.ToViewModel(user);

            return View(userDetails);
        }
        [HttpGet]
        [AuthorizeUser(new int[] { administrador, suscriptor })]
        public ActionResult UpdatePassword(int id)
        {
            var user = unit.UserRepository.GetByID(id);
            UserViewModel userUpdate = new UserViewModel();
            userUpdate.ToViewModel(user);
            return View(userUpdate);
        }

        [HttpPost]
        [AuthorizeUser(new int[] { administrador, suscriptor })]
        public ActionResult UpdatePassword(UserViewModel userUpdated)
        {

            var user = unit.UserRepository.GetByID(userUpdated.Id);
            var encryptedPass = Encrypt.GetSHA256(userUpdated.Pass);
            user.pass = encryptedPass;
            unit.UserRepository.Update(user);
            unit.UserRepository.Save();
            return RedirectToAction("Index", "User");
        }

        //GET LOGIN
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(FormCollection collection)

        {
            UserViewModel usm = new UserViewModel

            {
                Email = collection["Email"].ToString(),
                Pass = Encrypt.GetSHA256(collection["Pass"].ToString())
            };

            var getUser = _userRepository.GetUserByUserMail(usm.Email);

            try
            {
                if (usm.Pass.Equals(getUser.pass))
                {
                    
                    Session["User"] = getUser;
                    if (getUser.id == 1)
                    {
                        Session["isAdmin"] = true;
                    }
                    else
                    {
                        Session["isAdmin"] = null;
                    }
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
