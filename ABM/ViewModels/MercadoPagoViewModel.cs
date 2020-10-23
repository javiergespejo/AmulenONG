using ABM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABM.ViewModels
{
    public class MercadoPagoViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Monto")]
        [Required(ErrorMessage = "Este campo es obligatorio!")]
        [DataType(DataType.Currency, ErrorMessage = "Debe ser de tipo numerico!")]
        [Range(1, Int32.MaxValue, ErrorMessage = "El monto tiene que ser mayor a cero")]
        public decimal Amount { get; set; }

        [Display(Name = "Link de MercadoPago")]
        [Required(ErrorMessage = "Este campo es obligatorio!")]
        [DataType(DataType.Url, ErrorMessage = "Debe ser un direccion web valida!")]
        public string Link { get; set; }

        public MercadoPagoButton ToEntity()
        {
            MercadoPagoButton mpButton = new MercadoPagoButton
            {
                id = this.Id,
                amount = this.Amount,
                link = this.Link
            };
            return mpButton;
        }


    }
}
