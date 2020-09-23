using ABM.Models;
using ABM.Repository;
using ABM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ABM.Controllers
{
    public class HomeController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        //private readonly HomeRepository _homeRepository;
        //private readonly ProjectRepository _projectRepository;

        public HomeController()//HomeRepository homeRepository)//, ProjectRepository projectRepository)
        {
            //_homeRepository = homeRepository;
            //_projectRepository = projectRepository;
        }

        public ActionResult Index()
        {
            //var welcomeText = _homeRepository.GetWelcomeText();
            //var images = _homeRepository.GetHomeSliderImages();
            //var projects = _projectsRepository.GetActiveProjects();

            HomeViewModel homeViewModel = new HomeViewModel
            {
                SliderImage = new List<byte[]>(),
                //WelcomeText = welcomeText
                //Projects = projects
            };

            //foreach (var image in images)
            //{
            //    homeViewModel.SliderImage.Add(image.imageData);
            //}

            return View(homeViewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}