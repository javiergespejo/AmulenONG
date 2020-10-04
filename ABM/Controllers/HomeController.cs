using ABM.Filters;
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
        private readonly HomeRepository _homeRepository;
        private readonly ProjectRepository _projectRepository;

        public HomeController()
        {
            _homeRepository = new HomeRepository();
            _projectRepository = new ProjectRepository();
        }

        const int administrador = 1;

        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            var welcomeText = _homeRepository.GetWelcomeText();
            var images = _homeRepository.GetHomeSliderImages();
            var projects = _projectRepository.GetActiveProjects();

            HomeViewModel homeViewModel = new HomeViewModel
            {
                SliderImages = new List<byte[]>(),
                WelcomeText = welcomeText,
                Projects = projects.ToList()
            };

            foreach (var image in images)
            {
                homeViewModel.SliderImages.Add(image.imageData);
            }

            return View(homeViewModel);
        }

        // GET: Home/UploadImage
        [AllowAnonymous]
        public ActionResult UploadImage()
        {
            return View();
        }

        //POST: Home/UploadImage
        [AllowAnonymous]
        [HttpPost]
        public ActionResult UploadImage(HomePageImageViewModel model)
        {
            HttpPostedFileBase file = Request.Files["ImageData"];
            int i = _homeRepository.UploadImageInDataBase(file, model.ToEntity());
            if (i == 1)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("ImageData", "No ha seleccionado ningun archivo!");
            return View(model);
        }
    }
}