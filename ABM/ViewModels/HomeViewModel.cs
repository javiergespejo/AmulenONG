using ABM.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ABM.ViewModels
{
    public class HomeViewModel
    {
        public int Id { get; set; }
        public IEnumerable<HomePageImageViewModel> SliderImages { get; set; }
        public string WelcomeText { get; set; }
        public List<Proyect> Projects { get; set; }

        public HomePageData ToEntity()
        {
            HomePageData homePageData = new HomePageData()
            {
                id = Id,
                WelcomeText = WelcomeText,
                editDate = DateTime.Now,
                UserId = 1
            };
            return homePageData;
        }
    }
}