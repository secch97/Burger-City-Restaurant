using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RestauranteWeb.Models
{
    public class UserLogin
    {
        [Display(Name = "Correo Electronico")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="Escriba un correo ")]
        public string Correo { get; set; }

        [Display(Name = "Contraseña")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Escriba una contraseña ")]
        [DataType(DataType.Password)]
        public string Clave { get; set; }

        [Display(Name = "Recordarme")]
        public bool recordarme { get; set; }
    }   


}