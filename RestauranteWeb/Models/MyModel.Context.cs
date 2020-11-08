﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RestauranteWeb.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using System.Collections.Generic;

    public partial class ProyectoASP_RestauranteEntities : DbContext
    {
        public ProyectoASP_RestauranteEntities()
            : base("name=ProyectoASP_RestauranteEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CategoriasProductos> CategoriasProductos { get; set; }
        public virtual DbSet<CuentasClientes> CuentasClientes { get; set; }
        public virtual DbSet<CuentasEmpleados> CuentasEmpleados { get; set; }
        public virtual DbSet<EstadosProductos> EstadosProductos { get; set; }
        public virtual DbSet<EtapasPedidos> EtapasPedidos { get; set; }
        public virtual DbSet<OfertasProductos> OfertasProductos { get; set; }
        public virtual DbSet<PedidosClientes> PedidosClientes { get; set; }
        public virtual DbSet<ProductosRestaurante> ProductosRestaurante { get; set; }
        public virtual DbSet<RolesEmpleados> RolesEmpleados { get; set; }
        public virtual DbSet<Combos> Combos { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<CombosDetalle> CombosDetalle { get; set; }
        public virtual DbSet<PedidosClientesDetalles> PedidosClientesDetalles { get; set; }
        public virtual DbSet<TrackeoPedidosClientes> TrackeoPedidosClientes { get; set; }
        public virtual DbSet<HistorialPedidos> HistorialPedidos { get; set; }
    
        public virtual ObjectResult<ObtenerDetallePedido_Result> ObtenerDetallePedido(string idPedido)
        {
            var idPedidoParameter = idPedido != null ?
                new ObjectParameter("IdPedido", idPedido) :
                new ObjectParameter("IdPedido", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ObtenerDetallePedido_Result>("ObtenerDetallePedido", idPedidoParameter);
        }

        
    }

    public partial class ProyectoASP_RestauranteEntities
    {
        [DbFunction("ProyectoASP_RestauranteModel.Store", "GeneradorIdObjetos")]
        public string GeneradorIdOjetos(string prefijo)
        {
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;

            var parameters = new List<ObjectParameter>();
            parameters.Add(new ObjectParameter("Prefijo", prefijo));

            return objectContext.CreateQuery<string>("ProyectoASP_RestauranteModel.Store.GeneradorIdObjetos(@Prefijo)", parameters.ToArray())
                 .Execute(MergeOption.NoTracking)
                 .FirstOrDefault();
        }

        [DbFunction("ProyectoASP_RestauranteModel.Store", "ObtenerMontoPedido")]
        public decimal ObtenerMontoPedido(string idPedido)
        {
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;

            var parameters = new List<ObjectParameter>();
            parameters.Add(new ObjectParameter("Idpedido", idPedido));

            return objectContext.CreateQuery<decimal>("ProyectoASP_RestauranteModel.Store.ObtenerMontoPedido(@IdPedido)", parameters.ToArray())
                 .Execute(MergeOption.NoTracking)
                 .FirstOrDefault();
        }
    }
}
