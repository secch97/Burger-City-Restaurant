USE [master]



CREATE DATABASE ProyectoASP_Restaurante
GO

/* BITACORA: 19/09/2020 CREADO POR: Javier Melara
*  OBJETIVO: Crear tablas en la BD ProyectoASP_Restaurante*/


USE ProyectoASP_Restaurante

GO

-- Creacion de objetos

CREATE TABLE CuentasClientes(
	IdCliente VARCHAR(20) NOT NULL , --Alfanumerico
	Nombres VARCHAR(100),
	Apellidos VARCHAR(100),
	TelefonoFijo VARCHAR(9),
	TelefonoCelular VARCHAR(9),
	Correo  VARCHAR(255),
	Validado BIT,
	Clave VARCHAR(500),
	Direccion VARCHAR(255),
	DireccionEntrega VARCHAR(255),
	Fecha DATETIME
)

GO

CREATE TABLE RolesEmpleados(
	IdRol INT NOT NULL IDENTITY,
	Rol VARCHAR(50)
)

GO

CREATE TABLE CuentasEmpleados(
	Usuario VARCHAR(100) NOT NULL,
	IdRol INT,
	Nombres VARCHAR(255),
	Apellidos VARCHAR(255),
	Clave VARCHAR(500)
)

GO

CREATE TABLE CategoriasProductos(
	IdCategoria VARCHAR(20) NOT NULL,
	Nombre VARCHAR(100),
	Descripcion VARCHAR(500) -- Opcional 
)

GO

CREATE TABLE EstadosProductos(
	IdEstado INT NOT NULL IDENTITY,
	Nombre VARCHAR(100)
)

GO

CREATE TABLE ProductosRestaurante(
	IdProducto VARCHAR(20) NOT NULL,
	IdCategoria VARCHAR(20),
	Nombre VARCHAR(100),
	Descripcion VARCHAR(500),--Opcional
	Precio numeric(4,2),
	IdEstado INT 
)

GO

CREATE TABLE CombosProductos(
	IdCombo VARCHAR(20) NOT NULL,
	Correlativo INT IDENTITY,
	IdProducto VARCHAR(20),
	Nombre VARCHAR(50),
	Descripcion VARCHAR(500),
	Precio numeric(4,2),
	IdEstado INT 	
)

GO

CREATE TABLE OfertasProductos(
	IdOferta varchar(20) NOT NULL,
	IdObjeto VARCHAR(20) NOT NULL, --	Producto completo o Combo
	Nombre VARCHAR(100),
	Activo BIT,
	Fecha DATETIME
)

GO

CREATE TABLE PedidosClientes(
	IdPedido VARCHAR(20) NOT NULL,
	IdCliente VARCHAR(20) NOT NULL,
	TelefonoContacto VARCHAR(9),
	DireccionEntrega VARCHAR(500),
	Fecha DATETIME,
	Cancelado BIT
)

CREATE TABLE PedidosClientesDetalles(
	Correlativo INT NOT NULL IDENTITY,
	IdPedido VARCHAR(20) NOT NULL,
	IdObjeto VARCHAR(20) NOT NULL --Producto completo o Combo
)

GO

CREATE TABLE EtapasPedidos(
	IdEtapa INT NOT NULL IDENTITY,
	Nombre VARCHAR(50)
)

GO

CREATE TABLE TrackeoPedidosClientes(
	IdPedido VARCHAR(20) NOT NULL,
	Correlativo INT IDENTITY,
	IdEtapa INT,
	Fecha DATETIME 	
)

GO

CREATE TABLE HistorialPedidos(
	IdHistorial  INT IDENTITY,
	IdPedido VARCHAR(20),
	Fecha DATETIME,
	UsuarioGrabacion VARCHAR(20)
)
	
CREATE TABLE PromocionesCarrusel(
	IdItem INT NOT NULL IDENTITY,
    Nombre VARCHAR(100),
	RutaImagen VARCHAR(100)
)
    
GO
	
	/*Restricciones de objetos
	
	*Llaves primarias*/
	
	ALTER TABLE CuentasClientes
	ADD CONSTRAINT PK_CuentasClientes
	PRIMARY KEY(IdCliente)
	
	GO
	
	ALTER TABLE CuentasClientes
	ADD CONSTRAINT DF_CuentasClientes_Fecha
	DEFAULT GETDATE() FOR Fecha
	
	GO
	
	ALTER TABLE RolesEmpleados
	ADD CONSTRAINT PK_RolesEmpleados
	PRIMARY KEY (IdRol)
	
	GO
	
	ALTER TABLE CuentasEmpleados
	ADD CONSTRAINT PK_CuentasEmpleados
	PRIMARY KEY (Usuario)
	
	GO
	
	ALTER TABLE CategoriasProductos
	ADD CONSTRAINT PK_CategoriasProductos
	PRIMARY KEY(IdCategoria)
	
	GO
	
	ALTER TABLE EstadosProductos
	ADD CONSTRAINT PK_EstadosProductos
	PRIMARY KEY (IdEstado)
	
	GO
	
	ALTER TABLE ProductosRestaurante
	ADD CONSTRAINT PK_ProductosRestaurante
	PRIMARY KEY(IdProducto)
	
	GO
	
	ALTER TABLE CombosProductos
	ADD CONSTRAINT PK_CombosProductos
	PRIMARY KEY(IdCombo,Correlativo)
	
	GO
	
	ALTER TABLE OfertasProductos
	ADD CONSTRAINT PK_OfertasProductos
	PRIMARY KEY (IdOferta,IdObjeto)
	
	GO
	
	ALTER TABLE OfertasProductos
	ADD CONSTRAINT DF_OfertasProductos_Fecha
	DEFAULT GETDATE() FOR Fecha
	
	GO
	
	ALTER TABLE PedidosClientes
	ADD CONSTRAINT PK_PedidosClientes
	PRIMARY KEY (IdPedido)
	
	ALTER TABLE PedidosClientesDetalles
	ADD CONSTRAINT PK_PedidosClientesDetalles
	PRIMARY KEY (Correlativo)
	
	GO
	
	ALTER TABLE PedidosClientes
	ADD CONSTRAINT DF_PedidosClientes_Fecha
	DEFAULT GETDATE() FOR Fecha
	
	GO
	
	ALTER TABLE EtapasPedidos
	ADD CONSTRAINT PK_EtapasPedidos
	PRIMARY KEY (IdEtapa)
	
	GO
	
	ALTER TABLE TrackeoPedidosClientes
	ADD CONSTRAINT PK_TrackeoPedidosClientes
	PRIMARY KEY (IdPedido,Correlativo)
	
	GO
	
	ALTER TABLE TrackeoPedidosClientes
	ADD CONSTRAINT DF_TrackeoPedidosClientes_Fecha
	DEFAULT GETDATE() FOR Fecha
	
	GO
	
	ALTER TABLE HistorialPedidos
	ADD CONSTRAINT PK_HistorialPedidos
	PRIMARY KEY (IdHistorial)
	
	GO
	
	ALTER TABLE HistorialPedidos
	ADD CONSTRAINT DF_HistorialPedidos_Fecha
	DEFAULT GETDATE() FOR Fecha
	
	GO
	
	ALTER TABLE PromocionesCarrusel
	ADD CONSTRAINT PK_PromocionesCarrusel
	PRIMARY KEY (IdItem)
	
	GO
	
	/*Llaves For�neas */
	
	--CuentasClientes
	--CuentasEmpleados
	ALTER TABLE CuentasEmpleados
	ADD CONSTRAINT FK_CuentasEmpleados_RolesEmpleados
	FOREIGN KEY (IdRol) REFERENCES RolesEmpleados(IdRol)
	
	GO
	
	--ProductosRestaurante
	
	ALTER TABLE ProductosRestaurante
	ADD CONSTRAINT FK_ProductosRestaurante_CategoriasProductos
	FOREIGN KEY (IdCategoria) REFERENCES CategoriasProductos(IdCategoria)
	
	GO
	
	
	ALTER TABLE ProductosRestaurante
	ADD CONSTRAINT FK_ProductosRestaurante_EstadosProductos
	FOREIGN KEY (IdEstado) REFERENCES EstadosProductos(IdEstado)
	
	GO
	
	--CombosProductos
	
	ALTER TABLE CombosProductos
	ADD CONSTRAINT FK_CombosProductos_ProductosRestaurante
	FOREIGN KEY (IdProducto) REFERENCES ProductosRestaurante(IdProducto)
	
	GO
	
	ALTER TABLE CombosProductos
	ADD CONSTRAINT FK_CombosProductos_EstadosProductos
	FOREIGN KEY (IdEstado) REFERENCES EstadosProductos(IdEstado)
	
	
	--PedidosClientes
	
	ALTER TABLE PedidosClientes
	ADD CONSTRAINT FK_PedidosClientes_CuentasClientes
	FOREIGN KEY (IdCliente) REFERENCES CuentasClientes(IdCliente)
	
	GO

	--PedidosClientesDetalles

	ALTER TABLE PedidosClientesDetalles
	ADD CONSTRAINT FK_PedidosClientesDetalles_PedidosClientes
	FOREIGN KEY (IdPedido) REFERENCES PedidosClientes(IdPedido)
	
	
	--HistorialPedidos
	
	ALTER TABLE HistorialPedidos
	ADD CONSTRAINT FK_HistorialPedidos_PedidosClientes
	FOREIGN KEY (IdPedido) REFERENCES PedidosClientes(IdPedido)
	
	GO
	
	--TrackeoPedidosClientes
	ALTER TABLE TrackeoPedidosClientes
	ADD CONSTRAINT FK_TrackeoPedidosClientes_PedidosClientes
	FOREIGN KEY (IdPedido) REFERENCES PedidosClientes(IdPedido)
	
	GO
	
	ALTER TABLE TrackeoPedidosClientes
	ADD CONSTRAINT FK_TrackeoPedidosClientes_EtapasPedidos
	FOREIGN KEY (IdEtapa) REFERENCES EtapasPedidos(IdEtapa)
	
	
	
	/*FIN DEL SCRIPT*/