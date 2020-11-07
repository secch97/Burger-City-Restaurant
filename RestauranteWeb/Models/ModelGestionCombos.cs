using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestauranteWeb.Models
{
    public class ModelGestionCombos
    {
        public Combos Combos { get; set; }
        public IEnumerable<ProductosRestaurante> Productos { get; set; }
        public IEnumerable<CombosDetalle> CombosDetalles { get; set; }

    }
}