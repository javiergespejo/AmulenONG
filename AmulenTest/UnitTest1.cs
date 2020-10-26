using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using ABM.Controllers;
using ABM.Models;
using ABM.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AmulenTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestUploadImageViewData_ShouldThrowException()
        {
            //Arrange
            var controller = new HomeController();
            UploadImageViewModel viewModel = new UploadImageViewModel()
            {
                Id = 1,
                UserId = 1
            };
            var result = controller.UploadImage(viewModel) as ViewResult;
            var expected = new byte[1];
            expected[0] = 0;
            //Act
            var homePageImageViewModel = (HomePageImage)result.ViewData.Model;
            var actual = homePageImageViewModel.imageData;
            //Assert
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void TestImageGalleryViewData()
        {
            //Arrange
            var controller = new HomeController();
            var result = controller.ImageGallery() as PartialViewResult;
            var expected = new List<HomePageImageViewModel>();
            //Act
            var homePageImageList = (List<HomePageImageViewModel>)result.ViewData.Model;
            var actual = homePageImageList;
            //Assert
            Assert.IsInstanceOfType(actual, typeof(List<HomePageImageViewModel>));
        }

        [TestMethod]
        public void TestIndexViewData()
        {
            //Arrange
            var controller = new HomeController();
            var result = controller.Index() as ViewResult;
            var expected = "Texto de bienvenida 7";
            //Act
            var homeViewModel = (HomeViewModel)result.ViewData.Model;
            var actual = homeViewModel.WelcomeText;
            //Assert
            Assert.IsNotNull(homeViewModel);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestDeleteImageRedirect()
        {
            //Arrange
            var controller = new HomeController();
            var imageId = 41;
            //Act
            var result = (RedirectToRouteResult)controller.DeleteImage(imageId);
            //Assert
            Assert.AreEqual("Edit", result.RouteValues["action"]);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestDeleteImage_ShouldThrowException()
        {
            //Arrange
            var controller = new HomeController();
            var imageId = 50;
            //Act
            controller.DeleteImage(imageId);
            //Assert
        }
        [TestMethod]
        public void TestRetrieveImage()
        {
            //Arrange
            var controller = new HomeController();
            var imageId = 42;
            var result = controller.RetrieveImage(imageId) as FileContentResult;
            var expected = "image/jpg";
            //Act
            var actual = result.ContentType;
            //Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestRetrieveImage_ShouldThrowException()
        {
            //Arrange
            var controller = new HomeController();
            var imageId = 1;
            var result = controller.RetrieveImage(imageId) as FileContentResult;
            var expected = "image/jpg";
            //Act
            var actual = result.ContentType;
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestEdit()
        {
            //Arrange
            var controller = new HomeController();
            var result = controller.Edit() as ViewResult;
            var editViewData = (HomeViewModel)result.ViewData.Model;
            var expected = "Texto de bienvenida 7";
            //Act
            var actual = editViewData.WelcomeText;
            //Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestEdit_ShouldThrowException()
        {
            //Arrange
            var controller = new HomeController();
            HomeViewModel homeViewModel = new HomeViewModel();
            //Act
            var result = controller.Edit(homeViewModel) as RedirectToRouteResult;
            //Assert
            Assert.AreEqual("Edit", result.RouteValues["action"]);
        }
    }
}
