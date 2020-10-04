using ABM.Interfaces;
using ABM.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
            catch (Exception e)
            {
                return e.Message;
            }
            return welcomeText;
        }

        public int UploadImageInDataBase(HttpPostedFileBase file, HomePageImage homePageImage)
        {
            homePageImage.imageData = ConvertToBytes(file);
            int fileNotSelected = homePageImage.imageData.Length;
            if (fileNotSelected >= 1)
            {
                unitOfWork.HomePageImageRepository.Insert(homePageImage);
                unitOfWork.Save();
                return 1;
            }
            return 0;
        }
        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
    }
}