using ABM.Models;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABM.ViewModels
{
    public class UserUpdatePasswordModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public void ParseToUser(User user)
        {
            user.pass = this.Password;
        }
        public void SetProperties(User user)
        {
            this.Id = user.id;
            this.Username = user.username;
            this.Password = user.pass;
        }
    }
}