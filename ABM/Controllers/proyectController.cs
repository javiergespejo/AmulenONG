using ABM.Repository;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ABM.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectRepository _projectRepository;


        public ProjectController()
        {
            _projectRepository = new projectRepository;
        }


        //public ProjectController(IProjectRepository projectRepository)
        //{
        //  _projectRepository = projectRepository
        //}

        public ActionResult Home ()
        {
            var project = _projectRepository.GetActiveProjects;
            return View(project);

        }


        
    }
}
