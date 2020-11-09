using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestauranteWeb.Models
{
    public class ModelHistorialOrdenes
    {
        
        public IEnumerable<PedidosClientes> pedidosClientes { get; set; }

        public decimal getMonto(string idPedido)
        {
            ProyectoASP_RestauranteEntities db = new ProyectoASP_RestauranteEntities();
            return Convert.ToDecimal(db.ObtenerMontoPedido(idPedido));

        }


    }
}