USE  ProyectoASP_Restaurante

GO

ALTER FUNCTION dbo.GeneradorIdObjetos
(@Prefijo CHAR(3)) 
RETURNS VARCHAR(20)
AS
BEGIN
/*
* OBJETIVO: Devolver los Id segun sea el objeto
* BITACORA: 4/11/2020 CREADO POR: Javier Melara
* */
	

/*PREFIJOS POSIBLES
* CAT--> CATEGORIAS
* PRO--> PRODUCTOS
* COM--> COMBOS */

--variables
DECLARE @UltimoId VARCHAR(20),
		@UltimoIdDig INT,
		@IdObjeto VARCHAR(20)
		
IF UPPER(@Prefijo) = 'CAT'
BEGIN
	SELECT TOP 1 @UltimoId = cp.IdCategoria
	FROM dbo.CategoriasProductos AS cp
	ORDER BY cp.IdCategoria DESC 
	
	SELECT @UltimoIdDig = ISNULL(CONVERT(INT,SUBSTRING(@UltimoId,4,LEN(@UltimoId))),0)
END

IF UPPER(@Prefijo) = 'PRO'
BEGIN
	SELECT TOP 1 @UltimoId = pr.IdProducto
	FROM dbo.ProductosRestaurante AS pr
	ORDER BY pr.IdProducto DESC 
	
	SELECT @UltimoIdDig = ISNULL(CONVERT(INT,SUBSTRING(@UltimoId,4,LEN(@UltimoId))),0)
END

IF UPPER(@Prefijo) = 'COM'
BEGIN
	SELECT TOP 1 @UltimoId = c.IdCombo
	FROM dbo.Combos AS c 
	ORDER BY c.IdCombo DESC 
	
	SELECT @UltimoIdDig = ISNULL(CONVERT(INT,SUBSTRING(@UltimoId,4,LEN(@UltimoId))),0)
	
END

SELECT @UltimoIdDig = (@UltimoIdDig+1)
SELECT @IdObjeto = CONCAT(@Prefijo,@UltimoIdDig)
	
RETURN @IdObjeto

END

GO


ALTER FUNCTION dbo.ObtenerMontoPedido(
	@IdPedido VARCHAR(20)
)
RETURNS NUMERIC(4,2)
AS
BEGIN 
/*
* OBJETIVO: Obtener el monto de un pedido
* BITACORA: 7/11/2020 CREADO POR: Javier Melara
* */	

DECLARE @MontoProductos NUMERIC(4,2) = 0,
		@MontoCombos NUMERIC(4,2) = 0
		
--Obteniendo monto en productos sueltos

SELECT @MontoProductos =  SUM (pr.Precio*pcd.Cantidad)
FROM PedidosClientesDetalles AS pcd
	JOIN ProductosRestaurante AS pr
		ON pcd.IdObjeto = pr.IdProducto
WHERE pcd.IdPedido = @IdPedido
	
--Obteniendo monto en combos

SELECT  @MontoCombos = SUM(c.Precio*pcd.Cantidad)
FROM PedidosClientesDetalles AS pcd
	JOIN Combos AS c
		ON	pcd.IdObjeto = c.IdCombo
WHERE pcd.IdPedido = @IdPedido 

--Salida

RETURN ISNULL(@MontoProductos,0) + ISNULL(@MontoCombos,0)	

END

GO

CREATE PROCEDURE dbo.ObtenerDetallePedido
@IdPedido VARCHAR(20)
AS
BEGIN
/*
* OBJETIVO: Obtener el desglose de productos y combos que componen el pedido
* BITACORA: 7/11/2020 CREADO POR: Javier Melara
* */	

SELECT pcd.IdObjeto,
	   ISNULL(c.Nombre,pr.Nombre) AS Nombre,
	   pcd.Cantidad,
	   ISNULL(c.Precio,pr.Precio) AS Precio,
	   ISNULL(c.Precio,pr.Precio)*pcd.Cantidad AS SubTotal  
FROM PedidosClientesDetalles AS pcd
	LEFT JOIN Combos AS c
		ON  pcd.IdObjeto = c.IdCombo
	LEFT JOIN ProductosRestaurante AS pr
		ON pcd.IdObjeto = pr.IdProducto 
WHERE pcd.IdPedido = 'PE1'--@IdPedido

END

GO

--ALTER TRIGGER dbo.TrackeoPedidosClientesUpd ON TrackeoPedidosClientes 
--FOR UPDATE 
--AS 
--BEGIN 
--/*
--* OBJETIVO: Insertar en el historial con cada actualizacion de estado de la tabla
--* BITACORA: 8/11/2020 CREADO POR: Javier Melara
--* */	


--DECLARE @IdEtapaNuevo INT,
--		@IdEtapaAntiguo INT
		
		
--		SELECT @IdEtapaAntiguo = i.IdEtapa
--		FROM INSERTED AS i
		
--		SELECT @IdEtapaNuevo = d.IdEtapa 
--		FROM DELETED AS d
		
--		IF @IdEtapaNuevo <>@IdEtapaAntiguo
--		BEGIN
--			INSERT INTO HistorialPedidos(IdPedido, IdEtapa, UsuarioGrabacion)
--			SELECT i.IdPedido,
--				   i.IdEtapa,
--				   CURRENT_USER
--			FROM INSERTED AS i
--		END
		
--END

SELECT *
FROM TrackeoPedidosClientes AS hp
