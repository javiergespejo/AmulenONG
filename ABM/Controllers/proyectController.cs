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
        // GET: Proyect
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
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Proyect/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proyect/Create
        [HttpPost]
        public ActionResult Create(ProyectViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
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
        public ActionResult Edit(int id)
        {
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
        public ActionResult Edit(ProyectViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _projectRepository.UpdateProyect(model.ToEntity());
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Proyect/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Proyect/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
