CREATE DATABASE  IF NOT EXISTS `NET_A_CERO`;
USE `NET_A_CERO`;

DROP TABLE IF EXISTS `Rol_x_Funcionalidad`;
DROP TABLE IF EXISTS `Usuarios_x_Rol`;
DROP TABLE IF EXISTS `Preguntas`;
DROP TABLE IF EXISTS `Ofertas_x_Subasta`;
DROP TABLE IF EXISTS `Compras`;
DROP TABLE IF EXISTS `Calificacion`;
DROP TABLE IF EXISTS `Items`;
DROP TABLE IF EXISTS `Facturas`;
DROP TABLE IF EXISTS `Rubro_x_Publicacion`;
DROP TABLE IF EXISTS `Publicaciones`;
DROP TABLE IF EXISTS `Visibilidad`;
DROP TABLE IF EXISTS `Estado`;
DROP TABLE IF EXISTS `Visibilidad`;
DROP TABLE IF EXISTS `Roles`;
DROP TABLE IF EXISTS `Funcionalidades`;
DROP TABLE IF EXISTS `Empresas`;
DROP TABLE IF EXISTS `Rubros`;
DROP TABLE IF EXISTS `Clientes`;
DROP TABLE IF EXISTS `Contacto`;
DROP TABLE IF EXISTS `Usuarios`;

-- Creo tablas del sistema

CREATE TABLE `Usuarios` (
    `usr_id` INT auto_increment PRIMARY KEY,
    `usr_usuario` nvarchar(50),  
    `usr_password` nvarchar(150) NOT NULL default '565339bc4d33d72817b583024112eb7f5cdf3e5eef0252d6ec1b9c9a94e12bb3',  
    `usr_activo` bit(1) NOT NULL DEFAULT 1,
    `usr_intentos` tinyint DEFAULT 0,
    `usr_admin` bit(1) NOT NULL DEFAULT 0
);

CREATE TABLE `Contacto` (
    `cont_id` INT AUTO_INCREMENT PRIMARY KEY,
    `cont_mail` nvarchar(255) NOT NULL,
    `cont_telefono` nvarchar(255) NULL,  -- No existe en la maestra
    `cont_calle` nvarchar(255) NOT NULL,
    `cont_numero_calle` NUMERIC(18, 0) NOT NULL,
    `cont_piso` NUMERIC(18, 0),
    `cont_depto` nvarchar(50),
    `cont_localidad` nvarchar(255) default 'Buenos Aires',  -- No existe en la maestra
    `cont_codigo_postal` nvarchar(50) NOT NULL
);
    
CREATE TABLE `Clientes` (
   `cli_id` INT AUTO_INCREMENT PRIMARY KEY,
    `cli_nombre` nvarchar(255) NOT NULL,
    `cli_apellido` nvarchar(255) NOT NULL,
    `cli_dni` NUMERIC(18, 0) NOT NULL,
    `cli_tipo_dni` varchar(50) default 'DNI - Documento Nacional de Identidad',
    `cli_fecha_nac` datetime,
    `cli_fecha_alta` datetime,
    `cli_activo` bit(1) NOT NULL DEFAULT 1,
    `cli_usr_id` INT,
	`cli_cont_id` INT,
    CONSTRAINT `tipo_dni` CHECK (`cli_tipo_dni ` IN ('DNI - Documento Nacional de Identidad', 'LC - Libreta Civica', 'LE - Libreta de Enrolamiento', 'Pasaporte'))
);

CREATE TABLE `Empresas` (
    `emp_id` INT AUTO_INCREMENT PRIMARY KEY,
    `emp_razon_social` nvarchar(255) NOT NULL,
    `emp_ciudad` nvarchar(50),
    `emp_cuit` nvarchar(50) NOT NULL,
    `emp_nombre_contacto` nvarchar(255) default 'Nombre Contacto',        -- No existe en la maestra
    `emp_rubro` INT,                                             
    `emp_fecha_alta` datetime,
    `emp_activo` bit(1) NOT NULL DEFAULT 1,
    `emp_usr_id` INT,
	`emp_cont_id` INT
);

CREATE TABLE `Publicaciones` (
    `publi_id` NUMERIC(18, 0) PRIMARY KEY,
    `publi_tipo` nvarchar(255) NOT NULL,          
    `publi_descripcion` nvarchar(255) NOT NULL,
    `publi_stock` NUMERIC(18, 0) NOT NULL,
	`publi_fec_vencimiento` datetime,
	`publi_fec_inicio` datetime NOT NULL,
    `publi_precio` NUMERIC(18, 2) NOT NULL,
    `publi_costo_pagado` NUMERIC(18, 2) NOT NULL,                                -- No existe en la maestra
    `publi_preguntas` bit(1) DEFAULT 1,
    `publi_usr_id` INT,
	`publi_visib_id` NUMERIC(18, 0),
	`publi_estado_id` INT,
    CONSTRAINT `tipo_publicacion` CHECK (`publi_tipo` IN ('Compra inmediata', 'Subasta'))
);

CREATE TABLE `Estado` (
    `estado_id` INT AUTO_INCREMENT PRIMARY KEY,
    `estado_desc` nvarchar(255) NOT NULL,
    CONSTRAINT `estado_publi` CHECK (`estado_desc` IN ('Borrador', 'Activa', 'Pausada', 'Finalizada'))
);
    
CREATE TABLE `Visibilidad` (
    `visib_id` NUMERIC(18, 0) PRIMARY KEY,
    `visib_desc` nvarchar(255),
    `visib_grado` nvarchar(50),
    `visib_precio` NUMERIC(18, 2) NOT NULL,
    `visib_porcentaje` NUMERIC(18, 2) NOT NULL,
    `visib_envios` bit(1) DEFAULT 1,
    `visib_activo` bit(1) NOT NULL DEFAULT 1,
    CONSTRAINT `descripcion_visibilidad` CHECK (`visib_desc` IN ('Oro', 'Plata', 'Bronce', 'Platino', 'Gratis')),
    CONSTRAINT `grado_visibilidad` CHECK (`visib_grado` IN ('Comisión por tipo de publicación', 'Comisión por producto vendido', 'Comisión por envío del producto'))
);

CREATE TABLE `Funcionalidades` (
    `func_id` INT AUTO_INCREMENT PRIMARY KEY,
    `func_nombre` nvarchar(255) NOT NULL       
);

CREATE TABLE `Roles` (
    `rol_id` INT AUTO_INCREMENT PRIMARY KEY,
    `rol_nombre` nvarchar(20) NOT NULL,  
    `rol_activo` bit(1) NOT NULL DEFAULT 1
);

CREATE TABLE `Rol_x_Funcionalidad` (
    `rol_id` INTEGER,
    `func_id` INTEGER,
    PRIMARY KEY (`rol_id` , `func_id`)
);

CREATE TABLE `Usuarios_x_Rol` (
    `usr_id` INTEGER,
    `rol_id` INTEGER,
    PRIMARY KEY (`usr_id`, `rol_id`)
);

CREATE TABLE `Compras` (
	`comp_id` INTEGER AUTO_INCREMENT PRIMARY KEY,
    `comp_usr_id` INTEGER,
	`comp_publi_id` NUMERIC(18, 0),
	`comp_fecha` datetime,
	`comp_cantidad` NUMERIC(18, 0),
	`comp_monto` NUMERIC(18, 2),
	`comp_calif_id` NUMERIC(18, 0)
);

CREATE TABLE `Calificacion` (
    `calif_id` NUMERIC(18, 0) PRIMARY KEY,
	`calif_cant_estrellas` NUMERIC(18,0) NOT NULL,
	`calif_desc` nvarchar(255),
    CONSTRAINT `calificacion_publicacion` CHECK (`calif_cant_estrellas` >= 0 AND `calif_cant_estrellas` <= 5)
);

CREATE TABLE `Rubros` (
    `rubro_id` INTEGER AUTO_INCREMENT PRIMARY KEY,
    `rubro_desc_corta` nvarchar(50),
    `rubro_desc_larga` nvarchar(255) NOT NULL
);

CREATE TABLE `Rubro_x_Publicacion` (
    `rubro_id` INTEGER,
    `publi_id` NUMERIC(18, 0),
    PRIMARY KEY (`rubro_id`, `publi_id`)
);

CREATE TABLE `Ofertas_x_Subasta` (
    `sub_id` INTEGER AUTO_INCREMENT PRIMARY KEY,
    `sub_usr_id` INT,
    `sub_monto` NUMERIC(18, 2) NOT NULL,
    `sub_fecha` datetime NOT NULL,
    `sub_ganador` bit(1) NOT NULL DEFAULT 0,
    `sub_publi_id` NUMERIC(18, 0)
);

CREATE TABLE `Facturas` (
    `fact_id` NUMERIC(18, 0) PRIMARY KEY,
    `fact_fecha` datetime NOT NULL,
    `fact_monto` NUMERIC(18, 2) NOT NULL,
    `fact_destinatario` INT,
    `fact_forma_pago` varchar(20) NOT NULL,
	`fact_publi_id` NUMERIC(18, 0),
    CONSTRAINT `forma_pago` CHECK (`fact_forma_pago` IN ('Efectivo', 'Crédito', 'Débito', 'Sin especificar'))
);

CREATE TABLE `Items` (
    `item_id` INT AUTO_INCREMENT PRIMARY KEY,
    `item_cantidad` NUMERIC(18, 0) NOT NULL,
    `item_tipo` nvarchar(255),
    `item_monto` NUMERIC(18, 2) NOT NULL,
    `item_fact_id` NUMERIC(18, 0)
);

CREATE TABLE `Preguntas` (
    `preg_id` INT AUTO_INCREMENT PRIMARY KEY,
	`preg_desc` nvarchar(255),
    `preg_resp` nvarchar(255),
    `preg_resp_fecha` datetime,
    `preg_usr_id` INT,
    `preg_publi_id` NUMERIC(18, 0)
);


/* FKs */
 
ALTER TABLE `Clientes` ADD CONSTRAINT `cliente_usuario` FOREIGN KEY (`cli_usr_id`) REFERENCES `Usuarios` (`usr_id`);

ALTER TABLE `Clientes` ADD CONSTRAINT `cliente_contacto` FOREIGN KEY (`cli_cont_id`) REFERENCES `Contacto`(`cont_id`);

ALTER TABLE `Empresas` ADD CONSTRAINT `empresa_usuario` FOREIGN KEY (`emp_usr_id`) REFERENCES `Usuarios`(`usr_id`);

ALTER TABLE `Empresas` ADD CONSTRAINT `rubro_empresa` FOREIGN KEY (`emp_rubro`) REFERENCES `Rubros`(`rubro_id`);

ALTER TABLE `Empresas` ADD CONSTRAINT `empresa_contacto` FOREIGN KEY (`emp_cont_id`) REFERENCES `Contacto`(`cont_id`);

ALTER TABLE `Usuarios_x_Rol` ADD CONSTRAINT `usuario_rol_usuario` FOREIGN KEY (`usr_id`) REFERENCES `Usuarios`(`usr_id`);

ALTER TABLE `Usuarios_x_Rol` ADD CONSTRAINT `rol_rol_usuario` FOREIGN KEY (`rol_id`) REFERENCES `Roles`(`rol_id`);

ALTER TABLE `Usuarios_x_Rol` ADD CONSTRAINT `unique_rol_usuario` UNIQUE(`usr_id`, `rol_id`);

ALTER TABLE `Rol_x_Funcionalidad` ADD CONSTRAINT `funcionalidad_rol_funcionalidad` FOREIGN KEY (`func_id`) REFERENCES `Funcionalidades`(`func_id`);

ALTER TABLE `Rol_x_Funcionalidad` ADD CONSTRAINT `rol_rol_funcionalidad` FOREIGN KEY (`rol_id`) REFERENCES `Roles`(`rol_id`);

ALTER TABLE `Rol_x_Funcionalidad` ADD CONSTRAINT `unique_rol_funcionalidad` UNIQUE(`func_id`, `rol_id`);

ALTER TABLE `Ofertas_x_Subasta` ADD CONSTRAINT `subasta_publicacion` FOREIGN KEY (`sub_publi_id`) REFERENCES `Publicaciones`(`publi_id`);

ALTER TABLE `Ofertas_x_Subasta` ADD CONSTRAINT `subasta_usuario` FOREIGN KEY (`sub_usr_id`) REFERENCES `Usuarios`(`usr_id`);

ALTER TABLE `Compras` ADD CONSTRAINT `compras_usuario` FOREIGN KEY (`comp_usr_id`) REFERENCES `Usuarios`(`usr_id`);

ALTER TABLE `Compras` ADD CONSTRAINT `compras_publicacion` FOREIGN KEY (`comp_publi_id`) REFERENCES `Publicaciones`(`publi_id`);

ALTER TABLE `Compras` ADD CONSTRAINT `compras_calificacion` FOREIGN KEY (`comp_calif_id`) REFERENCES `Calificacion`(`calif_id`);

ALTER TABLE `Compras` ADD CONSTRAINT `compras_unique` UNIQUE(`comp_usr_id`, `comp_publi_id`);

ALTER TABLE `Publicaciones` ADD CONSTRAINT `visibilidad_publicacion` FOREIGN KEY (`publi_visib_id`) REFERENCES `Visibilidad`(`visib_id`);

ALTER TABLE `Publicaciones` ADD CONSTRAINT `usuario_publicacion` FOREIGN KEY (`publi_usr_id`) REFERENCES `Usuarios`(`usr_id`);

ALTER TABLE `Publicaciones` ADD CONSTRAINT `estado_publicacion` FOREIGN KEY (`publi_estado_id`) REFERENCES `Estado`(`estado_id`);

ALTER TABLE `Rubro_x_Publicacion` ADD CONSTRAINT `rubro_publicacion_rubro` FOREIGN KEY (`rubro_id`) REFERENCES `Rubros`(`rubro_id`);

ALTER TABLE `Rubro_x_Publicacion` ADD CONSTRAINT `publicacion_publicacion_rubro` FOREIGN KEY (`publi_id`) REFERENCES `Publicaciones`(`publi_id`);

ALTER TABLE `Rubro_x_Publicacion` ADD CONSTRAINT `unique_publicacion_rubro` UNIQUE(`rubro_id`, `publi_id`);

ALTER TABLE `Facturas` ADD CONSTRAINT `factura_publicacion` FOREIGN KEY (`fact_publi_id`) REFERENCES `Publicaciones`(`publi_id`);

ALTER TABLE `Items` ADD CONSTRAINT `item_factura` FOREIGN KEY (`item_fact_id`) REFERENCES `Facturas`(`fact_id`);

ALTER TABLE `Preguntas` ADD CONSTRAINT `pregunta_usuario` FOREIGN KEY (`preg_usr_id`) REFERENCES `Usuarios`(`usr_id`);

ALTER TABLE `Preguntas` ADD CONSTRAINT `pregunta_publicacion` FOREIGN KEY (`preg_publi_id`) REFERENCES `Publicaciones`(`publi_id`);