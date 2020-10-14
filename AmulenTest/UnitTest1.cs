using System;
using System.Web.Mvc;
using ABM.Controllers;
using ABM.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AmulenTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestIndexViewData()
        {
            var controller = new HomeController();
            var result = controller.Index() as ViewResult;
            var homeViewModel = (HomeViewModel)result.ViewData.Model;
            Assert.AreEqual("Texto de bienvenida 6", homeViewModel.WelcomeText);
        }
    }
}
