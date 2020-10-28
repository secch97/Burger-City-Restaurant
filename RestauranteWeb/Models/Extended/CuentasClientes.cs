using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RestauranteWeb.Models
{
    [MetadataType(typeof(CuentasClientesMetadata))]
    public partial class CuentasClientes
    {
        public string ConfimarClave { get; set; }
    }

    public class CuentasClientesMetadata
    {
        [Display(Name = "Nombres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese sus nombres")]
        public string Nombres { get; set; }


        [Display(Name = "Apellidos")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese sus apellidos")]
        public string Apellidos { get; set; }

        
        [Display(Name = "Correo Electronico")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese su Correo Electronico")]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }



        [Display(Name = "Telefono Fijo")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingresa tu telefono fijo")]
        public string TelefonoFijo { get; set; }


        [Display(Name = "Telefono Celular")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese tu telefono celular")]
        public string TelefonoCelular { get; set; }


        [Display(Name = "Dirrecion")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese su direccion")]
        public string Direccion { get; set; }

        
        [Display(Name = "Direccion de entrega")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese su direccion de entrega")]
        public string DireccionEntrega { get; set; }

        [Display(Name = "Contraseñas")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Escriba su contraseña")]
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage ="Minimo de 6 caracteres")]
        public string Clave { get; set; }

       
        [Display(Name = "Confirmar Contraseñas")]
        [DataType(DataType.Password)]
        [Compare("Clave", ErrorMessage = "Las contraseñas deben ser iguales")]
        public string ConfimarClave { get; set; }
    }

}