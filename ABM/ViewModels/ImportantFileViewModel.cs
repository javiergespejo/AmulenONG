using ABM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABM.ViewModels
{
    public class ImportantFileViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio!")]
        public string Description { get; set; }
        public DateTime EditDate { get; set; }
        public int UserId { get; set; }
        public ImportantFile ToEntity()
        {
            ImportantFile importantFile = new ImportantFile()
            {
                id = Id,
                description = Description.ToUpper(),
                editDate = DateTime.Now,
                UserId = UserId
            };
            return importantFile;
        }
    }
}