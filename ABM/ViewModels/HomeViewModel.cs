using ABM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ABM.ViewModels
{
    public class HomeViewModel
    {
        public int Id { get; set; }
        public IEnumerable<HomePageImageViewModel> SliderImages { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio!")]
        public string WelcomeText { get; set; }
        public List<Proyect> Projects { get; set; }
        public List<ImportantFile> Files { get; set; }
        public int UserId { get; set; }

        public HomePageData ToEntity()
        {
            HomePageData homePageData = new HomePageData()
            {
                id = 1,
                WelcomeText = WelcomeText,
                editDate = DateTime.Now,
                UserId = UserId
            };
            return homePageData;
        }
    }
}