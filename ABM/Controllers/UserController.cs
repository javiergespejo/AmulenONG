﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
        private readonly UnitOfWork unit = new UnitOfWork();
        private readonly UserRepository _userRepository;
        public UserController()
        {
            _userRepository = new UserRepository(new AmulenEntities());
        }
        const int administrador = 1;
        const int suscriptor = 2;
        const string keyEncriptacion = "1234567891234567";

        // GET: Users
        [AuthorizeUser(new int[] { administrador })]
        public ActionResult Admin()
        {
            if(Session["User"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }
        [HttpGet]
        [AuthorizeUser(new int[] { administrador })]
        public ActionResult Perfil()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            UserViewModel userView = new UserViewModel();
            userView.ToViewModel((User)Session["User"]);

            return View(userView);
        }


        [AuthorizeUser(new int[] { administrador })]
        public ActionResult Index()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            var getUsers = from u in _userRepository.GetActiveUsers()
                           where u.typeUserId == 1
                           select new UserViewModel()
                           {
                               Id = u.id,
                               Email = u.email,
                               Name = u.name
                           };
            return View(getUsers.ToList());
        }

        // This is the method that creates an admin user
        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        // This is the method that creates an admin user
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
                    model.UserType = 1;
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
                return RedirectToAction(nameof(Admin));
            }
            return View(userViewModel);

        }
        [AllowAnonymous]
        public ActionResult Delete(int id)
        {
            _userRepository.DeleteUser(id);
            return RedirectToAction("Index", "User");
        }

        [AuthorizeUser(new int[] { administrador })]
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


            if (usm.Email == string.Empty || usm.Pass == string.Empty)
            {
                if (usm.Email == string.Empty)
                    ViewBag.message = "Los datos que ingresaste no son válidos";
                return View();
            }
            var getUser = _userRepository.GetUserByUserMail(usm.Email);


            try
            {

                if (usm.Pass.Equals(getUser.pass))
                {

                    Session["User"] = getUser;
                    if (getUser.typeUserId == 1)
                    {
                        Session["isAdmin"] = true;
                        return RedirectToAction("Admin", "User");
                    }

                    else
                    {
                        Session["isAdmin"] = null;
                    }
                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    ViewBag.message = "La contraseña es incorrecta";

                    return View();
                }

            }
            catch (Exception)
            {
                ViewBag.Message = "El email es incorrecto";
                return View();
            }

        }

        public ActionResult LogOff()
        {
            Session["User"] = null;
            return RedirectToAction("Index", "Home");
        }

        #region Recuperacion de password

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ForgotPassword(string providedEmail)
        {
            string message = "";

            var account = unit.UserRepository.GetUserByUserMail(providedEmail);
            if (account != null)
            {
                // Encrypts the 
                var resetCode = Business_Logic.EncryptionManager.Encrypt(account.id.ToString(), keyEncriptacion);
                SendVerificationLinkEmail(account.email, resetCode);

                unit.UserRepository.context.Configuration.ValidateOnSaveEnabled = false;
                unit.UserRepository.Save();
                message = "Se envio un link de recuperacion a tu mail.";
            }
            else
            {
                message = "Cuenta no encontrada";
            }

            ViewBag.Message = message;
            return View();
        }

        [NonAction]
        public void SendVerificationLinkEmail(string userEmail, string resetCode)
        {
            var verifyUrl = "/User/ResetPassword/?id=" + System.Web.HttpUtility.UrlEncode(resetCode);
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("jlfamt14@gmail.com", "Administrador prueba"); // PONER MAIL VALIDO DEL SENDER
            var toEmail = new MailAddress(userEmail);
            var fromEmailPassword = "JWbfHnnAAH2EVzG"; // USAR CONSTRASEÑA VALIDA PARA EL SENDER

            // MENSAJE DEL MAIL //
            string subject = "Recuperar contraseña AMULEN";
            string body = "Hola<br/>Recibimos una solicitud para recuperar su contraseña de Amulen. Por favor haga click en el link para hacerlo." +
                "<br/><br/><a href=" + link + ">Restaurar contraseña</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com", // LUEGO CAMBIAR EL PROVEEDOR
                Port = 587, // TAMBIEN CAMBIAR EL PUERTO
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword),
                Timeout = 20000
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
        
        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            string resetCode = Request.QueryString["id"];
            if (string.IsNullOrWhiteSpace(resetCode))
            {
                return HttpNotFound();
            }
            // DECRIPTA EL RESET CODE CON UNA KEY
            var idDecrypted = Business_Logic.EncryptionManager.Decrypt(resetCode, keyEncriptacion);
            var user = unit.UserRepository.GetByID(Int32.Parse(idDecrypted));
            if (user != null)
            {
                ResetPasswordModel model = new ResetPasswordModel();
                model.ResetCode = resetCode;
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }

        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                var decryptedId = Business_Logic.EncryptionManager.Decrypt(model.ResetCode, keyEncriptacion);
                var user = unit.UserRepository.GetByID(Int32.Parse(decryptedId));
                if (user != null)
                {
                    user.pass = Encrypt.GetSHA256(model.NewPassword);
                    unit.UserRepository.context.Configuration.ValidateOnSaveEnabled = false;
                    unit.UserRepository.Save();
                    message = "Nueva contraseña actualizada correctamente";
                }

            }
            else
            {
                message = "Hubo un problema con la verificacion!";
            }
            ViewBag.Message = message;
            return View(model);
        }
        #endregion

        public ActionResult _mostrarPerfil()
        {
            UserViewModel user = new UserViewModel();
            User usuario = (User)Session["User"];
            user.ToViewModel(usuario);
            return PartialView(user);
        }

        // This is the create method for suscriptor user type
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // This is the create method for suscriptor user type
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(UserViewModel model)
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
                    model.UserType = 2;
                    _userRepository.InsertUser(model.ToEntity());
                    TempData["RegisterSuccess"] = "El usuario se ha registrado correctamente!";
                }
                return View();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
