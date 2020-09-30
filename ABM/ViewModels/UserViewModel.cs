using ABM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABM.ViewModels
{
    public class UserViewModel
    {
        public IEnumerable<User> users { get; set; }

        [Display(Name = "Nombre completo")]
        [Required(ErrorMessage = "Este campo es obligatorio!")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Este campo es obligatorio!")]
        [EmailAddress(ErrorMessage = "Dirección invalida")]
        public string Email { get; set; }

        [Display(Name = "Nombre de usuario")]
        [Required(ErrorMessage = "Este campo es obligatorio!")]
        public string UserName { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Este campo es obligatorio!")]
        public string Pass { get; set; }

        public int Id { get; set; }
    }
}