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

        
        [Display(Name = "Correo electrónico")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese su correo electrónico")]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }



        [Display(Name = "Teléfono fijo")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingresa tu telefono fijo")]
        [RegularExpression("^[0-9]{4}[-][0-9]{4}", ErrorMessage = "El formato tiene que ser 9999-9999")]
        public string TelefonoFijo { get; set; }


        [Display(Name = "Teléfono celular")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese tu telefono celular")]
        [RegularExpression("^[0-9]{4}[-][0-9]{4}",ErrorMessage ="El formato tiene que ser 9999-9999")]
        public string TelefonoCelular { get; set; }


        [Display(Name = "Dirección")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese su dirección de domicilio")]
        public string Direccion { get; set; }

        
        [Display(Name = "Dirección de entrega")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingrese su dirección actual de entrega de pedidos ")]
        public string DireccionEntrega { get; set; }

        [Display(Name = "Contraseña")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Escriba su contraseña")]
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage ="Minimo de 6 caracteres")]
        public string Clave { get; set; }

       
        [Display(Name = "Confirmar contraseña")]
        [DataType(DataType.Password)]
        [Compare("Clave", ErrorMessage = "Las contraseñas deben ser iguales")]
        public string ConfimarClave { get; set; }
    }

}