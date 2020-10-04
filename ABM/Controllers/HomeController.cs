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
                SliderImages = images,
                WelcomeText = welcomeText,
                Projects = projects.ToList()
            };

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
        public ActionResult UploadImage(UploadImageViewModel model)
        {
            HttpPostedFileBase file = Request.Files["ImageData"];
            HomePageImage homePageImage = model.ToEntity();
            int i = _homeRepository.UploadImageInDataBase(file, homePageImage);
            if (i == 1)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("ImageData", "No ha seleccionado ningun archivo!");
            return View(model.ToEntity());
        }
        [AllowAnonymous]
        public ActionResult ImageGallery()
        {
            try
            {
                var imageList = from i in _homeRepository.GetHomeSliderImages()
                                select new HomePageImageViewModel()
                                {
                                    Id = i.id,
                                    ImageData = i.imageData,
                                    EditDate = i.editDate,
                                    UserId = i.UserId
                                };
                return PartialView(imageList.ToList());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [AllowAnonymous]
        public ActionResult RetrieveImage(int id)
        {
            byte[] cover = _homeRepository.GetImageById(id);

            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }
        [AllowAnonymous]
        public ActionResult Edit(int id)
        {
            var homePageData = _homeRepository.GetById(id);
            HomeViewModel viewModel = new HomeViewModel()
            {
                Id = homePageData.id,
                WelcomeText = homePageData.WelcomeText
            };

            return View(viewModel);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Edit(HomeViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _homeRepository.UpdateHome(model.ToEntity());
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}