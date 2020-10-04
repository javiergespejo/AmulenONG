using ABM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABM.ViewModels
{
    public class HomePageImageViewModel
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; }
        public HomePageImage ToEntity()
        {
            HomePageImage homePageImage = new HomePageImage()
            {
                id = Id,
                imageData = ImageData,
                editDate = DateTime.Now,
                UserId = 1
            };
            return homePageImage;
        }
    }
}