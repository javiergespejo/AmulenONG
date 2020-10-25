using ABM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABM.ViewModels
{
    public class SubViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Nombre completo")]
        [Required(ErrorMessage = "Este campo es obligatorio!")]
        [StringLength(30, ErrorMessage = "El campo nombre no puede tener mas de 30 caracteres! ")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Este campo es obligatorio!")]
        [EmailAddress(ErrorMessage = "Dirección invalida")]
        public string Email { get; set; }

        public Suscriptor ToEntity()
        {
            Suscriptor s = new Suscriptor
            {
                id = Id,
                name = Name,
                email = Email
            };
            return s;
        }

        public void ToViewModel(Suscriptor sub)
        {
            this.Id = sub.id;
            this.Name = sub.name;
            this.Email = sub.email;
        }
    }
}