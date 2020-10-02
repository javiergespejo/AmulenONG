using ABM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABM.ViewModels
{
    public class UserDetailsViewModel
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public void SetProperties(User user)
        {
            this.Name = user.name;
            this.Username = user.username;
            this.Email = user.email;
        }
    }
}