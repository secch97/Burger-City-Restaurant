using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestauranteWeb.Models
{
    public class ModelInicioCliente
    {
        public IEnumerable<Combos> combos { get; set; }
        public IEnumerable<CategoriasProductos> categorias { get; set; }
        public IEnumerable<ProductosRestaurante> productos { get; set; }
    }
}