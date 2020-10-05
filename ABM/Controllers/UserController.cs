using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

        // GET: Users
        [AuthorizeUser(new int[] { administrador })]
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
        [AllowAnonymous]
        public ActionResult Delete(int id)
        {
            _userRepository.DeleteUser(id);
            return RedirectToAction("Index", "User");
        }

        // FALTA IMPLEMENTAR AUTENTICACION
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
            if (usm.Email == string.Empty)
            {
                ViewBag.message = "No se pudo loguear";
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
                    }
                    else
                    {
                        Session["isAdmin"] = null;
                    }
                    return RedirectToAction("Index", "Home");
                }
                return View();
            }
            catch (Exception)
            {
                ViewBag.Message = "No se pudo loguear";
                return View();
            }
        }

        public ActionResult LogOff()
        {
            Session["User"] = null;
            return RedirectToAction("Index", "Home");
        }

        [NonAction]
        public void SendRecoveryPasswordLinkMail(string email, string activationCode)
        {
            var verifyUrl = "/User/RecoveryPassowrd/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("testmail@gmail.com", "MailAdress");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "**********"; // Replace with actual password

            string subject = "Proceso de recuperación de contraseña";
            string body = "Hola!<br/><br/>Hemos recibido una solicitud para recuperar la contraseña de su cuenta de Amulen. " +
                $"Para recuperar su contraseña, por favor, ingrese al siguiente link <br/><br/><a href='{link}'>RECUPERAR CONTRASEÑA</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
    }


}
