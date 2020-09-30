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
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<HomePageImage> GetHomeSliderImages()
        {
            try
            {
                return unitOfWork.HomePageImageRepository.Get();
            }
            catch(Exception)
            {
                return  Enumerable.Empty<HomePageImage>();
            }
            
        }

        public string GetWelcomeText()
        {
            string welcomeText;
            try
            {
                welcomeText = unitOfWork.HomePageDataRepository.Get().First().WelcomeText;
            }
            catch (Exception )
            {
                return e.Message;
            }
            return welcomeText;
        }
    }
}