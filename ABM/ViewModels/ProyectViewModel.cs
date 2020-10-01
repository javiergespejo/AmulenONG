using ABM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABM.ViewModels
{
    public class ProyectViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Titulo del proyecto")]
        [Required(ErrorMessage = "Este campo es obligatorio!")]
        public string ProjectName { get; set; }
        [Display(Name = "Detalles del proyecto")]
        [Required(ErrorMessage = "Este campo es obligatorio!")]
        public string ProjectDetail { get; set; }

        public Proyect ToEntity()
        {
            Proyect p = new Proyect
            {
                id = Id,
                proyectName = ProjectName,
                proyectDetail = ProjectDetail,
                editDate = DateTime.Now,
                StateId = 1,
                UserStateId = 1
            };

            return p;
        }

    }
}
