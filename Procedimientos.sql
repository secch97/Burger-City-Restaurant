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
