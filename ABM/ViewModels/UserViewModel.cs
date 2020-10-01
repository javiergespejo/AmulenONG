﻿using ABM.Models;
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

        public int Id { get; set; }
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

        public User ToEntity()
        {
            User u = new User();
            u.id = Id;
            u.name = Name;
            u.pass = Pass;
            u.username = UserName;
            u.email = Email;

            return u;
        }

        /// <summary>
        /// Assigns the properties of the ViewModel based on the object user
        /// </summary>
        /// <param name="user">user with the data</param>
        public void ToViewModel(User user)
        {
            this.Id = user.id;
            this.Name = user.name;
            this.Pass = user.pass;
            this.UserName = user.username;
            this.Email = user.email;
        }
    }
}