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
        [StringLength(30, ErrorMessage = "El campo nombre no puede tener mas de 30 caracteres! ")]
        public string ProjectName { get; set; }
        [Display(Name = "Detalles del proyecto")]
        [Required(ErrorMessage = "Este campo es obligatorio!")]
        [StringLength(150, ErrorMessage = "El campo detalles no puede tener mas de 150 caracteres! ")]
        public string ProjectDetail { get; set; }
        public int UserId { get; set; }

        public Proyect ToEntity()
        {
            Proyect p = new Proyect
            {
                id = Id,
                proyectName = ProjectName,
                proyectDetail = ProjectDetail,
                editDate = DateTime.Now,
                StateId = 1,
                UserStateId = UserId
            };
            return p;
        }

        public void ToProyectViewModel(Proyect model)
        {
            this.Id = model.id;
            this.ProjectName = model.proyectName;
            this.ProjectDetail = model.proyectDetail;
        }

    }
}
