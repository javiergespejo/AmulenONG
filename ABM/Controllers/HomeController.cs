using ABM.Business_Logic;
using ABM.Filters;
using ABM.Repository;
using ABM.ViewModels;
using Microsoft.Ajax.Utilities;
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
        private readonly FileRepository _fileRepository;

        public HomeController()
        {
            _homeRepository = new HomeRepository();
            _projectRepository = new ProjectRepository();
            _fileRepository = new FileRepository();
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
            try
            {
                var welcomeText = _homeRepository.GetWelcomeText();
                var importantDoc = _fileRepository.GetAll();
                var images = from i in _homeRepository.GetHomeSliderImages()
                             select new HomePageImageViewModel()
                             {
                                 Id = i.id,
                                 ImageData = i.imageData,
                                 EditDate = i.editDate,
                                 UserId = i.UserId
                             };
                var projects = _projectRepository.GetActiveProjects();
                if (welcomeText.IsNullOrWhiteSpace())
                {
                    welcomeText = "Bienvenido a Amulen";
                }
                HomeViewModel homeViewModel = new HomeViewModel
                {
                    Files = importantDoc.ToList(),
                    SliderImages = images,
                    WelcomeText = welcomeText,
                    Projects = projects.ToList()
                };

                return View(homeViewModel);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
            
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
            try
            {
                HttpPostedFileBase file = Request.Files["ImageData"];
                if (ValidateFile.ValidFileExtension(file))
                {
                    if (ValidateFile.ValidateFileSize(file))
                    {
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
                    throw new Exception("El archivo supera el tamaño maximos permitido de 5 MB.");
                }
                throw new Exception("El formato del archivo debe ser jpg, jpeg o png.");
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction("Edit", "Home");
            }
            
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
                TempData["Error"] = "Hubo un problema al eliminar la imagen.";
                return RedirectToAction("Edit", "Home");
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