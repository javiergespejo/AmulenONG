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
        public DateTime EditDate { get; set; }
        public int UserId { get; set; }

        public void ToViewModel(HomePageImage model)
        {
            this.Id = model.id;
            this.ImageData = model.imageData;
            this.EditDate = model.editDate;
            this.UserId = model.UserId;
        }
    }
}