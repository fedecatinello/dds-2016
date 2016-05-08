--Me conecto a la base de datos a usar
USE [GD1C2016]
GO

--Creo el esquema y lo comento para que no rompa cuando se ejecute nuevamente
--CREATE SCHEMA NET_A_CERO AUTHORIZATION [gd]
--GO

--Valido si las tablas existen, si asi fuere las dropeo
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Usuarios'))
	drop table NET_A_CERO.Usuarios

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Clientes'))
	drop table NET_A_CERO.Clientes

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Empresas'))
	drop table NET_A_CERO.Empresas

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Roles'))
	drop table NET_A_CERO.Roles

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Rol_Usuario'))
	drop table NET_A_CERO.Rol_Usuario

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Funcionalidades'))
	drop table NET_A_CERO.Funcionalidades

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Funcionalidad_Rol'))
	drop table NET_A_CERO.Funcionalidad_Rol

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Publicaciones'))
	drop table NET_A_CERO.Publicaciones

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Visibilidad_Publicacion'))
	drop table NET_A_CERO.Visibilidad_Publicacion

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Facturas'))
	drop table NET_A_CERO.Facturas

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Items'))
	drop table NET_A_CERO.Items

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Rubros'))
	drop table NET_A_CERO.Rubros

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Compras'))
	drop table NET_A_CERO.Compras

--FALTAN VER DOS TABLAS EN DUDA

-- Creo tablas del sistema (chequear datos con base maestra)
create table NET_A_CERO.Empresas (
	emp_id INT PRIMARY KEY,
	emp_razon NVARCHAR(255) UNIQUE NOT NULL,
	emp_mail NVARCHAR(50),
	emp_telefono INT,
	emp_calle NVARCHAR(100),
	emp_piso NUMERIC(18,0),
	emp_depto NVARCHAR(50),
	emp_localidad NVARCHAR(50),
	emp_codigo_postal NVARCHAR(50),
	emp_ciudad NVARCHAR(50),
	emp_cuit NVARCHAR(50) UNIQUE NOT NULL,
	emp_fecha_creacion DATETIME,
	emp_nombre_contacto VARCHAR(50),
	emp_rubro NVARCHAR(255)
)



create table NET_A_CERO.Usuarios (
	usr_id INT PRIMARY KEY,
	usr_username VARCHAR(8) UNIQUE NOT NULL,
	usr_password VARCHAR(8) NOT NULL,
	usr_estado BIT NOT NULL,
	usr_tipo INT FOREIGN KEY REFERENCES NET_A_CERO.Empresas,
	usr_intentos INT DEFAULT(0)
)




 

