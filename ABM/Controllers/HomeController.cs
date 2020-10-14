using ABM.Filters;
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
            var images = from i in _homeRepository.GetHomeSliderImages()
                         select new HomePageImageViewModel()
                         {
                             Id = i.id,
                             ImageData = i.imageData,
                             EditDate = i.editDate,
                             UserId = i.UserId
                         };
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
        [AuthorizeUser(new int[] { administrador })]
        public ActionResult UploadImage()
        {
            return View();
        }

        //POST: Home/UploadImage
        [AuthorizeUser(new int[] { administrador })]
        [HttpPost]
        [Route("~/Home/UploadImage")]
        public ActionResult UploadImage(UploadImageViewModel model)
        {
            HttpPostedFileBase file = Request.Files["ImageData"];
            var user = (Models.User)Session["User"];
            model.UserId = user.id;
            bool isUploaded = _homeRepository.UploadImageInDataBase(file, model.ToEntity());
            if (isUploaded)
            {
                TempData["ImageSuccess"] = "La imagen se ha guardado correctamente!";
                return RedirectToAction("Edit", "Home");
            }
            return View(model.ToEntity());
        }

        // This view shows a list of images
        [AuthorizeUser(new int[] { administrador })]
        public ActionResult ImageGallery()
        {
            try
            {
                var imageList = from i in _homeRepository.GetHomeSliderImages() 
                                orderby i.editDate descending
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

        [AuthorizeUser(new int[] { administrador })]
        public ActionResult Edit()
        {
            try
            {
                int idHomepage = _homeRepository.GetFirstHomePageDataID();
                var homePageData = _homeRepository.GetById(idHomepage);
                if (homePageData != null)
                {
                    HomeViewModel viewModel = new HomeViewModel()
                    {
                        Id = homePageData.id,
                        WelcomeText = homePageData.WelcomeText
                    };

                    return View(viewModel);
                }
                TempData["Error"] = "No existe datos para la pantalla de inicio.";
                return RedirectToAction("Admin", "User");
            }
            catch (Exception)
            {
                TempData["Error"] = "Hubo un error al ingresar al panel de administracion de pantalla de inicio.";
                return RedirectToAction("Admin", "User");
            }
        }
        [AuthorizeUser(new int[] { administrador })]
        [HttpPost]
        public ActionResult Edit(HomeViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = (Models.User)Session["User"];
                    viewModel.UserId = user.id;
                    _homeRepository.UpdateHome(viewModel.ToEntity());
                }
                TempData["Success"] = "El cambio se ha guardado correctamente!";
                return RedirectToAction("Edit", "Home");
            }
            catch
            {
                ViewData["Error"] = "Error al guardar los cambios";
                return View();
            }
        }

        [AuthorizeUser(new int[] { administrador })]
        public ActionResult DeleteImage(int id)
        {
            try
            {
                _homeRepository.DeleteImage(id);
                TempData["SucessMessage"] = "Se elimino la imagen correctamente";
                return RedirectToAction("Edit", "Home");
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Converts byte array to image
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
    }
}