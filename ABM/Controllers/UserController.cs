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
            // TODO: Hay que resolver qué se va a mostrar en este método.

            //var getUsers = _userRepository.GetUsers();

            //UserViewModel userViewModel = new UserViewModel
            //{
            //    users = getUsers.ToList()
            //};

            return View();//userViewModel);
        }

        // Este metodo lo hizo Fede con el ViewModel.
        public ActionResult Details(int id)
        {
            User user = _userRepository.GetByID(id);

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            User user = _userRepository.GetById(id);
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
    }
}
