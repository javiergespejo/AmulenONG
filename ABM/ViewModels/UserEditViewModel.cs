using ABM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABM.ViewModels
{
    public class UserEditViewModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName(displayName: "Nombre")]
        public string Name { get; set; }
        [Required]
        [DisplayName(displayName: "Correo electrónico")]
        [EmailAddress(ErrorMessage = "Dirección invalida")]
        public string Email { get; set; }
        [Required]
        [DisplayName(displayName: "Usuario")]
        public string Username { get; set; }

        // Parameterless constructor
        public UserEditViewModel()
        {
        }

        public UserEditViewModel(User user)
        {
            this.Id = user.id;
            this.Name = user.name;
            this.Email = user.email;
            this.Username = user.username;
        }

        public User ToUserEntity()
        {
            return new User()
            {
                id = this.Id,
                name = this.Name,
                email = this.Email,
                username = this.Username
            };
        }
    }
}