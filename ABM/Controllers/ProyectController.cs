using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABM.Controllers
{
    public class ProyectController
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

        public ActionResult Home()
        {
            var project = _projectRepository.GetActiveProjects;
            return View(project);

        }



    }
}

