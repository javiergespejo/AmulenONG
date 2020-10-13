using ABM.Filters;
using ABM.Repository;
using ABM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.Mvc;

namespace ABM.Controllers
{
    public class ProyectController : Controller
    {
        private readonly ProjectRepository _projectRepository;

        public ProyectController()
        {
            _projectRepository = new ProjectRepository();
        }

        const int administrador = 1;

        // GET: Proyect
        [AuthorizeUser(new int[] { administrador })]
        public ActionResult Index()
        {
            var projects = from p in _projectRepository.GetActiveProjects()
                           select new ProyectViewModel()
                           {
                               Id = p.id,
                               ProjectName = p.proyectName,
                               ProjectDetail = p.proyectDetail
                           };

            return View(projects);
        }

        // GET: Proyect/Details/5
        [AuthorizeUser(new int[] { administrador })]
        public ActionResult Details(int id)
        {
            var model = _projectRepository.GetById(id);
            ProyectViewModel project = new ProyectViewModel();
            project.ToProyectViewModel(model);
            return View(project);
        }

        // GET: Proyect/Create
        [AuthorizeUser(new int[] { administrador })]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proyect/Create
        [HttpPost]
        [AuthorizeUser(new int[] { administrador })]
        public ActionResult Create(ProyectViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = (Models.User)Session["User"];
                    model.UserId = user.id;
                    _projectRepository.InsertProject(model.ToEntity());
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Proyect/Edit/5
        [AuthorizeUser(new int[] { administrador })]
        public ActionResult Edit(int id)
        {
            if (Session["isAdmin"] == null)
            {
                return View("Error");
            }

            var p = _projectRepository.GetById(id);
            ProyectViewModel viewModel = new ProyectViewModel()
            {
                Id = p.id,
                ProjectName = p.proyectName,
                ProjectDetail = p.proyectDetail
            };

            return View(viewModel);
        }

        // POST: Proyect/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(new int[] { administrador })]
        public ActionResult Edit(ProyectViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = (Models.User)Session["User"];
                    model.UserId = user.id;
                    _projectRepository.UpdateProject(model.ToEntity());
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Proyect/Delete/5
        [AuthorizeUser(new int[] { administrador })]
        public ActionResult Delete(int id)
        {
            _projectRepository.DeleteProject(id);
            ModelState.Clear();
            return RedirectToAction("Index", "Proyect");
        }
    }
}
