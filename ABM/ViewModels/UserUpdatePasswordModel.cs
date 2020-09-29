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

    }
}