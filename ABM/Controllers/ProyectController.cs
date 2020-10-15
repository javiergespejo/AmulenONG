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
            try
            {
                var projects = from p in _projectRepository.GetActiveProjects()
                               select new ProyectViewModel()
                               {
                                   Id = p.id,
                                   ProjectName = p.proyectName,
                                   ProjectDetail = p.proyectDetail
                               };
                if (TempData["Error"] != null)
                {
                    ViewBag.Error = TempData["Error"];
                }
                else if (TempData["SucessMessage"] != null)
                {
                    ViewBag.Message = TempData["SucessMessage"];
                }
                return View(projects);
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrio un error al obtener la lista de proyectos.";
                return RedirectToAction("Admin", "User");
            }
               
        }

        // GET: Proyect/Details/5
        [AuthorizeUser(new int[] { administrador })]
        public ActionResult Details(int id)
        {
            try
            {
                var model = _projectRepository.GetById(id);
                if (model != null)
                {
                    ProyectViewModel project = new ProyectViewModel();
                    project.ToProyectViewModel(model);
                    return View(project);
                }
                throw new Exception();
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrio un error al mostrar el proyecto, puede que sea invalido.";
                return RedirectToAction("Index");
            }

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
                    TempData["SucessMessage"] = "El proyecto fue creado con exito.";
                    return RedirectToAction("Index");
                }
                throw new Exception();

            }
            catch
            {
                TempData["Error"] = "Operacion invalida, se redirigio a la pantalla principal.";
                return RedirectToAction("Index");
            }
        }

        // GET: Proyect/Edit/5
        [AuthorizeUser(new int[] { administrador })]
        public ActionResult Edit(int id)
        {
            try
            {
                var p = _projectRepository.GetById(id);
                if (p != null)
                {
                    ProyectViewModel viewModel = new ProyectViewModel()
                    {
                        Id = p.id,
                        ProjectName = p.proyectName,
                        ProjectDetail = p.proyectDetail
                    };

                    return View(viewModel);
                }
                throw new Exception();
            }
            catch (Exception)
            {
                TempData["Error"] = "Proyecto invalido, se redirigio a la pantalla principal.";
                return RedirectToAction("Index");
            }
            
            
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
                    TempData["SucessMessage"] = "El proyecto fue editado con exito.";
                    return RedirectToAction("Index");

                }
                throw new Exception();
            }
            catch (Exception)
            {
                TempData["Error"] = "Hubo un error al editar el proyecto, puede que sea invalido.";
                return View();
            }
        }

        // GET: Proyect/Delete/5
        [AuthorizeUser(new int[] { administrador })]
        public ActionResult Delete(int id)
        {
            try
            {
                if (_projectRepository.GetById(id) != null)
                {
                    _projectRepository.DeleteProject(id);
                    ModelState.Clear();
                    TempData["SucessMessage"] = "El proyecto fue eliminado con exito.";
                    return RedirectToAction("Index", "Proyect");
                }
                throw new Exception();
            }
            catch (Exception)
            {
                TempData["Error"] = "Hubo un error al eliminar el proyecto.";
                return View();
            }
        }
    }
}
