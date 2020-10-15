using System;
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
using Newtonsoft.Json;
using static ABM.Repository.UserRepository;

namespace ABM.Controllers
{
    public class UserController : Controller
    {
        #region Controller Startup
        private readonly UnitOfWork unit = new UnitOfWork();
        private readonly UserRepository _userRepository;
        public UserController()
        {
            _userRepository = new UserRepository(new AmulenEntities());
        }
        const int administrador = 1;
        const int suscriptor = 2;
        const string keyEncriptacion = "1234567891234567";

        #endregion

        // GET: Users
        [AuthorizeUser(new int[] { administrador })]
        public ActionResult Admin()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }
            return View();
        }
        [HttpGet]
        [AuthorizeUser(new int[] { administrador })]
        public ActionResult Perfil()
        {
            try
            {
                if (Session["User"] != null)
                {
                    UserViewModel userView = new UserViewModel();
                    userView.ToViewModel((User)Session["User"]);
                    return View(userView);

                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
                throw new Exception();
            }
            catch (Exception)
            {
                TempData["Error"] = "No se puede mostrar la informacion de perfil del usuario.";
                return RedirectToAction("Admin", "User");
            }
        }


        [AuthorizeUser(new int[] { administrador })]
        public ActionResult Index()
        {
            try
            {
                if (Session["User"] != null)
                {
                    var getUsers = from u in _userRepository.GetActiveUsers()
                                   where u.typeUserId == 1
                                   select new UserViewModel()
                                   {
                                       Id = u.id,
                                       Email = u.email,
                                       Name = u.name
                                   };
                    if (TempData["Error"] != null)
                    {
                        ViewBag.Error = TempData["Error"];
                    }
                    else if (TempData["SuccessMessage"] != null)
                    {
                        ViewBag.Message = TempData["SuccessMessage"];
                    }

                    return View(getUsers.ToList());
                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrio un error al obtener la lista de usuarios.";
                return RedirectToAction("Admin", "User");
            }

        }

        #region ABM Usuarios


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
                    TempData["SuccessMessage"] = "Usuario agregado con exito";
                    return RedirectToAction("Index", "User");
                }
                throw new Exception();
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrio un error al agregar el usuario, puede que sea invalido.";
                return RedirectToAction("Index", "User");
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                User user = _userRepository.GetByID(id);
                if (user != null)
                {
                    UserEditViewModel userEditViewModel = new UserEditViewModel(user);
                    return View(userEditViewModel);
                }
                throw new Exception();
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrio un error al mostrar el usuario, puede que sea invalido.";
                return RedirectToAction("Index", "User");
            }

        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserEditViewModel userViewModel)
        {
            try
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
                    TempData["SuccessMessage"] = "El usuario fue editado con exito.s";
                    return RedirectToAction("Index", "User");
                }
                throw new Exception();
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrio un error al editar el usuario, puede que sea invalido.";
                return View(userViewModel);
            }



        }
        [AuthorizeUser(new int[] { administrador })]
        public ActionResult Delete(int id)
        {
            try
            {
                if (_userRepository.GetByID(id) != null)
                {
                    _userRepository.DeleteUser(id);
                    TempData["SuccessMessage"] = "El usuario fue eliminado con exito.";
                    return RedirectToAction("Index", "User");
                }
                throw new Exception();
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrio un error al eliminar el usuario, puede que sea invalido.";
                return RedirectToAction("Index", "User");
            }

        }

        [AuthorizeUser(new int[] { administrador })]
        public ActionResult Details(int id)
        {
            try
            {
                var user = unit.UserRepository.GetByID(id);
                if (user != null)
                {
                    UserViewModel userDetails = new UserViewModel();
                    userDetails.ToViewModel(user);
                    return View(userDetails);
                }
                throw new Exception();
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrio un error al mostrar el usuario, puede que sea invalido.";
                return RedirectToAction("Index", "User");
            }

        }

        //UPDATE PASSWORD  DESDE PANEL ADMINISTRACION
        [HttpGet]
        [AuthorizeUser(new int[] { administrador, suscriptor })]
        public ActionResult UpdatePassword(int id)
        {
            try
            {
                var user = unit.UserRepository.GetByID(id);
                if (user != null)
                {
                    UserViewModel userUpdate = new UserViewModel();
                    userUpdate.ToViewModel(user);
                    return View(userUpdate);
                }
                throw new Exception();

            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrio un error al mostrar el usuario, puede que sea invalido.";
                return RedirectToAction("Index", "User");
            }

        }

        [HttpPost]
        [AuthorizeUser(new int[] { administrador, suscriptor })]
        public ActionResult UpdatePassword(UserViewModel userUpdated)
        {
            try
            {
                var user = unit.UserRepository.GetByID(userUpdated.Id);
                if (user != null)
                {
                    var encryptedPass = Encrypt.GetSHA256(userUpdated.Pass);
                    user.pass = encryptedPass;
                    unit.UserRepository.Update(user);
                    unit.UserRepository.Save();
                    TempData["SuccessMessage"] = "Contraseña actualizada con exito";
                    return RedirectToAction("Index", "User");
                }
                throw new Exception();
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrio un error al actualizar la contraseña, puede que el usuario sea invalido.";
                return View();
            }
            
        }
        #endregion

        #region Login / LogOff

        //GET LOGIN
        
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]

        //login with valid mail and password
        public ActionResult Login(UserViewModel userViewModel)

        {
            
    
            ModelState["UserName"].Errors.Clear();
            ModelState["Name"].Errors.Clear();

            if (ModelState.IsValid)
            {
                bool MailExists = _userRepository.CheckMail(userViewModel.ToEntity());
                var getUser = _userRepository.GetUserByUserMail(userViewModel.Email);
                var getPass = _userRepository.CheckPassword(userViewModel.ToEntity());
                if (!MailExists || !getPass)
                {
                    if (!MailExists)
                    {
                        ModelState.AddModelError("email", "El email es incorrecto!");
                    }
                    if (!getPass)
                    {
                        ModelState.AddModelError("pass", "La contraseña es incorrecta!");
                    }
                    return View();
                }


                if (getPass && getUser.email == userViewModel.Email)
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

                    return RedirectToAction("Index", "Home");
                }
                return View();

            }
            else
            {
                ViewBag.Message = "El email es incorrecto";
                return View();
            }
            return View();
        }

        //LogOff
        public ActionResult LogOff()
        {
            Session["User"] = null;
            return RedirectToAction("Index", "Home");
        }
        #endregion
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
            try
            {
                var idDecrypted = Business_Logic.EncryptionManager.Decrypt(resetCode, keyEncriptacion);
                var user = unit.UserRepository.GetByID(Int32.Parse(idDecrypted));
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = resetCode;
                    return View(model);
                }
                throw new Exception();
            }
            catch (Exception)
            {
                TempData["Error"] = "El usuario es invalido o no existe.";
                return RedirectToAction("Login", "User");
            }
            

        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var decryptedId = Business_Logic.EncryptionManager.Decrypt(model.ResetCode, keyEncriptacion);
                    var user = unit.UserRepository.GetByID(Int32.Parse(decryptedId));
                    if (user != null)
                    {
                        user.pass = Encrypt.GetSHA256(model.NewPassword);
                        unit.UserRepository.context.Configuration.ValidateOnSaveEnabled = false;
                        unit.UserRepository.Save();
                        TempData["SuccessMessage"] = "Contraseña actualizada con exito";
                        return RedirectToAction("Login", "User");
                    }
                    
                }
                throw new Exception();
            }
            catch (Exception)
            {
                TempData["Error"] = "Hubo un problema con la verificacion!";
                return View(model);
            }
            
        }
        #endregion

        public ActionResult _mostrarPerfil()
        {
            try
            {
                if(Session["User"] != null)
                {
                    UserViewModel user = new UserViewModel();
                    User usuario = (User)Session["User"];
                    user.ToViewModel(usuario);
                    return PartialView(user);
                }
                throw new Exception();
            }
            catch (Exception)
            {
                return PartialView(new UserViewModel() { Name = "No disponible", Email = "No disponible" });
            }
            
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
                    TempData["SucessMessage"] = "Usuario creado con exito";

                    return RedirectToAction("Index", "Home");
                }
                throw new Exception();
            }
            catch (Exception)
            {
                ViewBag.Error = "Hubo un error al crear el usuario";
                return View();
            }
        }
    }
}
