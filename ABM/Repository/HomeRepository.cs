using ABM.Interfaces;
using ABM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABM.Repository
{
    public class HomeRepository : IHomeRepository, IDisposable
    {

        private readonly UnitOfWork unitOfWork = new UnitOfWork();

        public IEnumerable<HomePageImage> GetHomeSliderImages()
        {
            return unitOfWork.HomePageImageRepository.Get();
        }

        public string GetWelcomeText()
        {
            string welcomeText = unitOfWork.HomePageDataRepository.Get().First().WelcomeText;
            return welcomeText;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}