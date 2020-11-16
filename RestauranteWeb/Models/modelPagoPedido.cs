using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestauranteWeb.Models
{
    public class ModelPagoPedido
    {
        public CuentasClientes cliente { get; set; }
        public IEnumerable<modeloCarritoObjetos> objetos { get; set; }

        public decimal total { get; set; }
    }
}