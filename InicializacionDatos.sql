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
VALUES ('TA160457',1,'Javier Isaac','Melara López','jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=')

--Etapas
INSERT INTO EtapasPedidos(Nombre) VALUES('Solicitud')

