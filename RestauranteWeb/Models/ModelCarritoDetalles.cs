using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestauranteWeb.Models
{
    public class ModelCarritoDetalles
    {
        public IEnumerable<modeloCarritoObjetos> carrito { get; set; }
        public decimal total { get; set; }
    }
}