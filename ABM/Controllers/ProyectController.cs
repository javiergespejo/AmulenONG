using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ABM.Repository;

namespace ABM.Controllers
{
    public class ProyectController
    {
        private readonly ProjectRepository _projectRepository;


        public ProyectController()
        {
            this._projectRepository= new ProjectRepository();
        }


        //public ProjectController(IProjectRepository projectRepository)
        //{
        //  _projectRepository = projectRepository
        //}

        //public ActionResult Home()
        //{
        //  var project = _projectRepository.GetActiveProjects();
            //return View(project);

        //}



    }
}

