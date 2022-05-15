USE ProyectoASP_Restaurante
GO

--SCRIPT DE INICIALIZACION DE DATOS

-- Roles
INSERT INTO RolesEmpleados
(Rol) 
VALUES('Administrador')

--Administrador

INSERT INTO CuentasEmpleados
(Usuario,IdRol,Nombres,Apellidos,Clave) 
VALUES ('CC160510',1,'Saúl Ernesto','Castillo Chamagua','jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=')

--Etapas
INSERT INTO EtapasPedidos(Nombre) VALUES('Solicitud')

