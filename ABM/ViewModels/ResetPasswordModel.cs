using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABM.ViewModels
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Se requiere el campo nueva constraseña", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva constraseña")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden!")]
        [Display(Name = "Confirme nueva constraseña")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string ResetCode { get; set; }
    }
}