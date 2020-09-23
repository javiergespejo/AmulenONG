using ABM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABM.Repository
{
    public class HomeRepository : IHomeRepository, IDisposable
    {
        private readonly UnitOfWork unitOfWork = new UnitOfWork();
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<HomePageImage> GetHomeSliderImages()
        {
            return unitOfWork.HomePageImageRepository.Get();
        }

        public string GetWelcomeText()
        {
            string welcomeText = unitOfWork.HomePageDataRepository.Get().First().WelcomeText;
            return welcomeText;
        }
    }
}