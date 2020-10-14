using ABM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABM.ViewModels
{
    public class UploadImageViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public HomePageImage ToEntity()
        {
            HomePageImage homePageImage = new HomePageImage()
            {
                id = Id,
                editDate = DateTime.Now,
                UserId = UserId
            };
            return homePageImage;
        }
    }
}