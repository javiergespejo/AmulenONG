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

        public HomeController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            //var projectList = _unitOfWork.PostRepository.Get().Where(p => p.is_active == true).ToList();
            //var images = _unitOfWork.HomeRepository.GetImages();
            //var welcomeText = _unitOfWork.HomeRepository.GetWelcomeText();

            //HomeViewModel homeViewModel = new HomeViewModel
            //{
            //    SliderImage = images,
            //    WelcomeText = welcomeText
            //};

            return View();
            //return View(projectList);
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