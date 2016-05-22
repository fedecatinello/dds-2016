--Me conecto a la base de datos a usar
USE [GD1C2016]
GO

--Creo el esquema y lo comento para que no rompa cuando se ejecute nuevamente
--CREATE SCHEMA NET_A_CERO AUTHORIZATION [gd]
--GO

--Valido si las tablas existen, si asi fuere las dropeo

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Items'))
    DROP TABLE NET_A_CERO.Items
    
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Facturas'))
    DROP TABLE NET_A_CERO.Facturas
        
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Ofertas_x_Subasta'))
    DROP TABLE NET_A_CERO.Ofertas_x_Subasta

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Compras'))
    DROP TABLE NET_A_CERO.Compras
    
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Publicaciones'))
    DROP TABLE NET_A_CERO.Publicaciones

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Visibilidad'))
    DROP TABLE NET_A_CERO.Visibilidad

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Rubros'))
    DROP TABLE NET_A_CERO.Rubros

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Clientes'))
    DROP TABLE NET_A_CERO.Clientes

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Empresas'))
    DROP TABLE NET_A_CERO.Empresas
    
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Contacto'))
    DROP TABLE NET_A_CERO.Contacto
    
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Rol_x_Funcionalidad'))
    DROP TABLE NET_A_CERO.Rol_x_Funcionalidad
    
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Funcionalidades'))
    DROP TABLE NET_A_CERO.Funcionalidades
    
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Usuarios_x_Rol'))
    DROP TABLE NET_A_CERO.Usuarios_x_Rol
    
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Usuarios'))
    DROP TABLE NET_A_CERO.Usuarios

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.Roles'))
    DROP TABLE NET_A_CERO.Roles


-- Creo tablas del sistema

CREATE TABLE [NET_A_CERO].[Usuarios] (
    [usr_id] INT IDENTITY(1,1) PRIMARY KEY,
    [usr_usuario] as isnull('USER' + CAST(usr_id AS NVARCHAR(10)),'X'),  --equivale a un [nvarchar](50)
    [usr_password] [nvarchar](150) NOT NULL default '559aead08264d5795d3909718cdd05abd49572e84fe55590eef31a88a08fdffd',  --A en SHA 256
    [usr_activo] [bit] NOT NULL DEFAULT 1,
    [usr_intentos] [tinyint] DEFAULT 0,
    [usr_admin] [bit] NOT NULL DEFAULT 0,
)

CREATE TABLE [NET_A_CERO].[Contacto] (
    [cont_id] INT IDENTITY(1,1) PRIMARY KEY,
    [cont_mail] [nvarchar](255) NOT NULL,
    [cont_telefono] [nvarchar](255) NULL default FLOOR(RAND() * POWER(CAST(10 as BIGINT), 10)),  -- No existe en la maestra
    [cont_calle] [nvarchar](255) NOT NULL,
    [cont_numero_calle] [NUMERIC](18, 0) NOT NULL,
    [cont_piso] [NUMERIC](18, 0),
    [cont_depto] [nvarchar](50),
    [cont_localidad] [nvarchar](255) default 'Buenos Aires',  -- No existe en la maestra
    [cont_codigo_postal] [nvarchar](50) NOT NULL,
    [cont_usr_id] INT
)
    
CREATE TABLE [NET_A_CERO].[Clientes] (
    [cli_id] INT IDENTITY(1,1) PRIMARY KEY,
    [cli_nombre] [nvarchar](255) NOT NULL,
    [cli_apellido] [nvarchar](255) NOT NULL,
    [cli_dni] [NUMERIC](18, 0) NOT NULL,
    [cli_tipo_dni] [varchar](50) default 'DNI - Documento Nacional de Identidad',
    [cli_fecha_nac] [datetime],
    [cli_fecha_alta] [datetime],
    [cli_usr_id] INT,
    CONSTRAINT [tipo_dni] CHECK (cli_tipo_dni IN ('DNI - Documento Nacional de Identidad', 'LC - Libreta Civica', 'LE - Libreta de Enrolamiento', 'Pasaporte'))
)

CREATE TABLE [NET_A_CERO].[Empresas] (
    [emp_id] INT IDENTITY(1,1) PRIMARY KEY,
    [emp_razon_social] [nvarchar](255) UNIQUE NOT NULL,
    [emp_ciudad] [nvarchar](50),
    [emp_cuit] [nvarchar](50) UNIQUE NOT NULL,
    [emp_nombre_contacto] [nvarchar](255) default 'Nombre Contacto',        -- No existe en la maestra
    [emp_rubro] [nvarchar](50),                                             
    [emp_fecha_alta] [datetime],
    [emp_usr_id] INT
)

CREATE TABLE [NET_A_CERO].[Publicaciones] (
    [publi_id] [NUMERIC](18, 0) IDENTITY(1, 1) PRIMARY KEY,
    [publi_tipo] [nvarchar](255) NOT NULL,          
    [publi_descripcion] [nvarchar](255) NOT NULL,
    [publi_estado] [nvarchar](255) NOT NULL,        
    [publi_stock] [NUMERIC](18, 0) NOT NULL,
    [publi_fec_vencimiento] [datetime],
    [publi_fec_inicio] [datetime] NOT NULL,
    [publi_precio] [NUMERIC](18, 2) NOT NULL,
    [publi_costo] [NUMERIC](18, 2),                                 -- No existe en la maestra
    [publi_preguntas] [bit] DEFAULT 1,
    [publi_envio] [bit] DEFAULT 0,
    [publi_calificacion] [NUMERIC](18, 0),     
    [publi_calificacion_detalle] [nvarchar](255),
    [publi_usr_id] INT,
    [publi_visib_id] NUMERIC(18, 0),
    [publi_rubro_id] INT,
    CONSTRAINT [tipo_publicacion] CHECK (publi_tipo IN ('Compra inmediata', 'Subasta')),
    CONSTRAINT [estado_publicacion] CHECK (publi_estado IN ('Borrador', 'Activa', 'Pausada', 'Finalizada')),
    CONSTRAINT [calificacion_publicacion] CHECK (publi_calificacion >= 0 AND publi_calificacion <= 5)
)
    
CREATE TABLE [NET_A_CERO].[Visibilidad] (
    [visib_id] [NUMERIC](18, 0) PRIMARY KEY,
    [visib_desc] [nvarchar](255),
    [visib_grado] [nvarchar](50) NOT NULL,
    [visib_precio] [NUMERIC](18, 2) NOT NULL,
    [visib_porcentaje] [NUMERIC](18, 2) NOT NULL,
    CONSTRAINT [descripcion_visibilidad] CHECK (visib_desc IN ('Oro', 'Plata', 'Bronce', 'Platino', 'Gratis')),
    CONSTRAINT [grado_visibilidad] CHECK (visib_grado IN ('Comisión por tipo de publicación', 'Comisión por producto vendido', 'Comisión por envío del producto')) --Es necesario esto?
)

CREATE TABLE [NET_A_CERO].[Funcionalidades] (
    [func_id] INT IDENTITY(1,1) PRIMARY KEY,
    [func_nombre] [nvarchar](255) NOT NULL       
)

CREATE TABLE [NET_A_CERO].[Roles] (
    [rol_id] INT IDENTITY(1,1) PRIMARY KEY,
    [rol_nombre] [nvarchar](20) NOT NULL,  
    [rol_activo] [bit] NOT NULL DEFAULT 1,
    CONSTRAINT [tipo_rol] CHECK (rol_nombre IN ('Administrativo', 'Empresa', 'Cliente'))
)

CREATE TABLE [NET_A_CERO].[Rol_x_Funcionalidad] (
    [id] INTEGER IDENTITY(1,1) PRIMARY KEY,
    [rol_id] INTEGER,
    [func_id] INTEGER
)

CREATE TABLE [NET_A_CERO].[Usuarios_x_Rol] (
    [id] INTEGER IDENTITY(1,1) PRIMARY KEY,
    [usr_id] INTEGER,
    [rol_id] INTEGER
)

CREATE TABLE [NET_A_CERO].[Compras] (
    [comp_id] INTEGER IDENTITY(1,1) PRIMARY KEY,
    [comp_cli_id] INTEGER,
    [comp_publi_id] [NUMERIC](18, 0),
    [comp_fecha] [datetime],
    [comp_cantidad] [NUMERIC](18, 0)
)

CREATE TABLE [NET_A_CERO].[Rubros] (
    [rubro_id] INTEGER IDENTITY(1,1) PRIMARY KEY,
    [rubro_desc_corta] [nvarchar](50),
    [rubro_desc_larga] [nvarchar](255) NOT NULL
)

CREATE TABLE [NET_A_CERO].[Ofertas_x_Subasta] (
    [sub_id] INTEGER IDENTITY(1,1) PRIMARY KEY,
    [sub_usr_id] INT NOT NULL,
    [sub_monto] [NUMERIC](18, 2) NOT NULL,
    [sub_fecha] [datetime] NOT NULL,
    [sub_ganador] [bit] NOT NULL DEFAULT 0,
    [sub_publi_id] NUMERIC(18, 0)
)

CREATE TABLE [NET_A_CERO].[Facturas] (
    [fact_id] [NUMERIC](18, 0) PRIMARY KEY,
    [fact_fecha] [datetime] NOT NULL,
    [fact_monto] [NUMERIC](18, 2) NOT NULL,
    [fact_destinatario] INT NOT NULL,
    [fact_forma_pago] [varchar](20) NOT NULL,
    [fact_publi_id] NUMERIC(18, 0),
    CONSTRAINT [forma_pago] CHECK (fact_forma_pago IN ('Efectivo', 'Crédito', 'Débito', 'Sin especificar'))
)

CREATE TABLE [NET_A_CERO].[Items] (
    [item_id] INT IDENTITY(1,1) PRIMARY KEY,
    [item_cantidad] [NUMERIC](18, 0) NOT NULL,
    [item_tipo] [nvarchar](255),
    [item_monto] [NUMERIC](18, 2) NOT NULL,
    [item_fact_id] NUMERIC(18, 0)
)


/* FKs */
 
ALTER TABLE [NET_A_CERO].[Clientes] ADD CONSTRAINT cliente_usuario FOREIGN KEY (cli_usr_id) REFERENCES [NET_A_CERO].[Usuarios](usr_id)

ALTER TABLE [NET_A_CERO].[Empresas] ADD CONSTRAINT empresa_usuario FOREIGN KEY (emp_usr_id) REFERENCES [NET_A_CERO].[Usuarios](usr_id)

ALTER TABLE [NET_A_CERO].[Contacto] ADD CONSTRAINT contacto_usuario FOREIGN KEY (cont_usr_id) REFERENCES [NET_A_CERO].[Usuarios](usr_id)

ALTER TABLE [NET_A_CERO].[Usuarios_x_Rol] ADD CONSTRAINT usuario_rol_usuario FOREIGN KEY (usr_id) REFERENCES [NET_A_CERO].[Usuarios](usr_id)

ALTER TABLE [NET_A_CERO].[Usuarios_x_Rol] ADD CONSTRAINT rol_rol_usuario FOREIGN KEY (rol_id) REFERENCES [NET_A_CERO].[Roles](rol_id)

ALTER TABLE [NET_A_CERO].[Usuarios_x_Rol] ADD CONSTRAINT unique_rol_usuario UNIQUE(usr_id, rol_id)

ALTER TABLE [NET_A_CERO].[Rol_x_Funcionalidad] ADD CONSTRAINT funcionalidad_rol_funcionalidad FOREIGN KEY (func_id) REFERENCES [NET_A_CERO].[Funcionalidades](func_id)

ALTER TABLE [NET_A_CERO].[Rol_x_Funcionalidad] ADD CONSTRAINT rol_rol_funcionalidad FOREIGN KEY (rol_id) REFERENCES [NET_A_CERO].[Roles](rol_id)

ALTER TABLE [NET_A_CERO].[Rol_x_Funcionalidad] ADD CONSTRAINT unique_rol_funcionalidad UNIQUE(func_id, rol_id)

ALTER TABLE [NET_A_CERO].[Ofertas_x_Subasta] ADD CONSTRAINT subasta_publicacion FOREIGN KEY (sub_publi_id) REFERENCES [NET_A_CERO].[Publicaciones](publi_id)

ALTER TABLE [NET_A_CERO].[Compras] ADD CONSTRAINT compras_usuario FOREIGN KEY (comp_cli_id) REFERENCES [NET_A_CERO].[Clientes](cli_id)

ALTER TABLE [NET_A_CERO].[Compras] ADD CONSTRAINT compras_publicacion FOREIGN KEY (comp_publi_id) REFERENCES [NET_A_CERO].[Publicaciones](publi_id)

ALTER TABLE [NET_A_CERO].[Compras] ADD CONSTRAINT compras_unique UNIQUE(comp_cli_id, comp_publi_id)

ALTER TABLE [NET_A_CERO].[Publicaciones] ADD CONSTRAINT visibilidad_publicacion FOREIGN KEY (publi_visib_id) REFERENCES [NET_A_CERO].[Visibilidad](visib_id)

ALTER TABLE [NET_A_CERO].[Publicaciones] ADD CONSTRAINT rubro_publicacion FOREIGN KEY (publi_rubro_id) REFERENCES [NET_A_CERO].[Rubros](rubro_id)

ALTER TABLE [NET_A_CERO].[Publicaciones] ADD CONSTRAINT usuario_publicacion FOREIGN KEY (publi_usr_id) REFERENCES [NET_A_CERO].[Usuarios](usr_id)

ALTER TABLE [NET_A_CERO].[Facturas] ADD CONSTRAINT factura_publicacion FOREIGN KEY (fact_publi_id) REFERENCES [NET_A_CERO].[Publicaciones](publi_id)

ALTER TABLE [NET_A_CERO].[Items] ADD CONSTRAINT item_factura FOREIGN KEY (item_fact_id) REFERENCES [NET_A_CERO].[Facturas](fact_id)



/******************************
*         MIGRACION           *
******************************/


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.pr_crear_usuario'))
    DROP PROCEDURE NET_A_CERO.pr_crear_usuario
GO
    
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.pr_crear_contacto_cliente'))
    DROP PROCEDURE NET_A_CERO.pr_crear_contacto_cliente
GO
    
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NET_A_CERO.pr_crear_contacto_empresa'))
    DROP PROCEDURE NET_A_CERO.pr_crear_contacto_empresa
GO    

-- Creo procedure para crear usuarios

CREATE PROCEDURE NET_A_CERO.pr_crear_usuario
    @usr_id numeric(18,0) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO NET_A_CERO.Usuarios default values
    SET @usr_id = SCOPE_IDENTITY();
END
GO

    
--Creo procedure para crear contacto de un cliente (recibe el nombre, apellido y dni)

CREATE PROCEDURE NET_A_CERO.pr_crear_contacto_cliente

  @cli_dni numeric(18,0)

AS
BEGIN
    
    INSERT INTO NET_A_CERO.Contacto (cont_mail, cont_calle, cont_numero_calle, cont_piso, cont_depto, cont_codigo_postal)
           SELECT DISTINCT Cli_Mail, Cli_Dom_Calle, Cli_Nro_Calle, Cli_Piso, Cli_Depto, Cli_Cod_Postal
           FROM gd_esquema.Maestra
                  WHERE Cli_Dni = @cli_dni
    
    UPDATE NET_A_CERO.Contacto SET cont_usr_id = 
                (SELECT cli_usr_id 
                FROM NET_A_CERO.Clientes 
                    WHERE cli_dni = @cli_dni)
END
GO


--Creo procedimiento para crear contacto de una empresa (recibe razon social y cuit)

CREATE PROCEDURE NET_A_CERO.pr_crear_contacto_empresa
(
    @emp_razon_social nvarchar(255),
    @emp_cuit nvarchar(50)
)
AS
BEGIN
    INSERT INTO NET_A_CERO.Contacto (cont_mail, cont_calle, cont_numero_calle, cont_piso, cont_depto, cont_codigo_postal)
           SELECT DISTINCT Publ_Empresa_Mail, Publ_Empresa_Dom_Calle, Publ_Empresa_Nro_Calle, Publ_Empresa_Piso, Publ_Empresa_Depto, Publ_Empresa_Cod_Postal
           FROM gd_esquema.Maestra
                  WHERE Publ_Empresa_Razon_Social = @emp_razon_social
                  AND Publ_Empresa_Cuit = @emp_cuit
    
    UPDATE NET_A_CERO.Contacto SET cont_usr_id = 
                (SELECT emp_usr_id 
                FROM NET_A_CERO.Empresas 
                    WHERE emp_razon_social = @emp_razon_social
                    AND emp_cuit = @emp_cuit)

END
GO


/** Migracion de Empresas **/

INSERT INTO NET_A_CERO.Empresas (emp_razon_social, emp_cuit, emp_fecha_alta)
    SELECT DISTINCT Publ_Empresa_Razon_Social, Publ_Empresa_Cuit, Publ_Empresa_Fecha_Creacion 
    FROM gd_esquema.Maestra     
        WHERE Publ_Empresa_Razon_Social IS NOT NULL
        AND Publ_Empresa_Cuit IS NOT NULL

    
/** Agrego usuarios asociados a las empresas **/

DECLARE @row_pos_emp numeric(18,0)
DECLARE @row_count_emp numeric(18,0)
SELECT @row_count_emp = COUNT(*) FROM NET_A_CERO.Empresas
SET @row_pos_emp = 1

WHILE (@row_pos_emp <= @row_count_emp)
BEGIN
    DECLARE @emp_id numeric(18,0)
	DECLARE @emp_razon nvarchar(255)
	DECLARE @emp_cuit nvarchar(50)
	SET @emp_razon = (SELECT DISTINCT emp_razon_social FROM NET_A_CERO.Empresas WHERE emp_id = @row_pos_emp)
	SET @emp_cuit = (SELECT DISTINCT emp_cuit FROM NET_A_CERO.Empresas WHERE emp_id = @row_pos_emp)
    --Agrego usuario de empresa
    EXEC NET_A_CERO.pr_crear_usuario @emp_id OUTPUT
    UPDATE NET_A_CERO.Empresas SET emp_usr_id = @emp_id WHERE emp_id = @row_pos_emp
    --Agrego contacto de empresa
    EXEC NET_A_CERO.pr_crear_contacto_empresa @emp_razon, @emp_cuit 
    SET @row_pos_emp = @row_pos_emp + 1
END
    
    
/** Migracion de Clientes que vendieron **/

INSERT INTO NET_A_CERO.Clientes (cli_nombre, cli_apellido, cli_dni, cli_fecha_nac, cli_fecha_alta)
    SELECT DISTINCT Publ_Cli_Nombre, Publ_Cli_Apeliido, Publ_Cli_Dni, Publ_Cli_Fecha_Nac, GETDATE() 
    FROM gd_esquema.Maestra     
    WHERE Publ_Cli_Nombre IS NOT NULL
    AND Publ_Cli_Apeliido IS NOT NULL
    AND Publ_Cli_Dni IS NOT NULL

    
/** Migracion de Clientes que compraron **/

INSERT INTO NET_A_CERO.Clientes (cli_nombre, cli_apellido, cli_dni, cli_fecha_nac, cli_fecha_alta)
    SELECT DISTINCT Cli_Nombre, Cli_Apeliido, Cli_Dni, Cli_Fecha_Nac, GETDATE() 
    FROM gd_esquema.Maestra     
    WHERE Cli_Nombre IS NOT NULL
    AND Cli_Apeliido IS NOT NULL
    AND Cli_Dni IS NOT NULL
    
    
/** Agrego usuarios asociados a los clientes **/

DECLARE @row_pos_cli numeric(18,0)
DECLARE @row_count_cli numeric(18,0)
SELECT @row_count_cli = COUNT(*) FROM NET_A_CERO.Clientes
SET @row_pos_cli = 1

WHILE (@row_pos_cli <= @row_count_cli)
BEGIN
    DECLARE @cli_id numeric(18,0)
	DECLARE @cli_dni numeric(18,0)
	SET @cli_dni = (SELECT DISTINCT cli_dni FROM NET_A_CERO.Clientes WHERE cli_id = @row_pos_cli)
    --Agrego usuario de cliente
    EXEC NET_A_CERO.pr_crear_usuario @cli_id OUTPUT
    UPDATE NET_A_CERO.Clientes SET cli_usr_id = @cli_id WHERE cli_id = @row_pos_cli
    --Agrego contacto de cliente
    EXEC NET_A_CERO.pr_crear_contacto_cliente @cli_dni
    SET @row_pos_cli = @row_pos_cli + 1
END


/** Migracion de Roles **/

INSERT INTO NET_A_CERO.Roles(rol_nombre, rol_activo)
    VALUES ('Administrativo', 1)

INSERT INTO NET_A_CERO.Roles(rol_nombre, rol_activo)
    VALUES ('Empresa', 1)

INSERT INTO NET_A_CERO.Roles(rol_nombre, rol_activo)
    VALUES ('Cliente', 1)


/** Migracion de Usuarios_x_Rol **/

INSERT INTO NET_A_CERO.Usuarios_x_Rol(rol_id, usr_id)
    SELECT (SELECT rol_id FROM NET_A_CERO.Roles WHERE rol_nombre = 'Empresa'), emp_usr_id FROM NET_A_CERO.Empresas
    
INSERT INTO NET_A_CERO.Usuarios_x_Rol(rol_id, usr_id)
    SELECT (SELECT rol_id FROM NET_A_CERO.Roles WHERE rol_nombre = 'Cliente'), cli_usr_id FROM NET_A_CERO.Clientes



/** Migración de Funcionalidades **/

INSERT INTO NET_A_CERO.Funcionalidades(func_nombre)
    VALUES ('Comprar');

INSERT INTO NET_A_CERO.Funcionalidades(func_nombre)
    VALUES ('Ofertar');

INSERT INTO NET_A_CERO.Funcionalidades(func_nombre)
    VALUES ('Vender');

INSERT INTO NET_A_CERO.Funcionalidades(func_nombre)
    VALUES ('Calificar vendedor');

INSERT INTO NET_A_CERO.Funcionalidades(func_nombre)
    VALUES ('Preguntar');

INSERT INTO NET_A_CERO.Funcionalidades(func_nombre)
    VALUES ('Agregar rol');

INSERT INTO NET_A_CERO.Funcionalidades(func_nombre)
    VALUES ('Deshabilitar rol');

INSERT INTO NET_A_CERO.Funcionalidades(func_nombre)
    VALUES ('Editar rol');

INSERT INTO NET_A_CERO.Funcionalidades(func_nombre)
    VALUES ('Crear usuario');

INSERT INTO NET_A_CERO.Funcionalidades(func_nombre)
    VALUES ('Editar usuario');

INSERT INTO NET_A_CERO.Funcionalidades(func_nombre)
    VALUES ('Habilitar usuario');

INSERT INTO NET_A_CERO.Funcionalidades(func_nombre)
    VALUES ('Deshabilitar usuario');

INSERT INTO NET_A_CERO.Funcionalidades(func_nombre)
    VALUES ('Generar factura');


-- Agrego al administrador todas las funcionalidades del sistema

BEGIN TRANSACTION
        DECLARE @cont int;
        SET @cont = 0;
        
        WHILE((SELECT COUNT(*) FROM NET_A_CERO.Funcionalidades) > @cont)
        BEGIN
                SET @cont = @cont + 1;
                INSERT INTO NET_A_CERO.Rol_x_Funcionalidad (func_id, rol_id)
                    VALUES (@cont, (SELECT rol_id FROM NET_A_CERO.Roles WHERE rol_nombre = 'Administrativo'))
        END
COMMIT



/** Migracion de Rol_x_Funcionalidad **/

INSERT INTO NET_A_CERO.Rol_x_Funcionalidad(func_id, rol_id)   
    VALUES(1,2);
    
INSERT INTO NET_A_CERO.Rol_x_Funcionalidad(func_id, rol_id)   
    VALUES(2,2);

INSERT INTO NET_A_CERO.Rol_x_Funcionalidad(func_id, rol_id)   
    VALUES(3,2);

INSERT INTO NET_A_CERO.Rol_x_Funcionalidad(func_id, rol_id) 
    VALUES(4,2);
    
INSERT INTO NET_A_CERO.Rol_x_Funcionalidad(func_id, rol_id) 
    VALUES(5,2);
    
INSERT INTO NET_A_CERO.Rol_x_Funcionalidad(func_id, rol_id) 
    VALUES(13,2);

INSERT INTO NET_A_CERO.Rol_x_Funcionalidad(func_id, rol_id) 
    VALUES(3,3);

INSERT INTO NET_A_CERO.Rol_x_Funcionalidad(func_id, rol_id) 
    VALUES(13,3);



/** Migración de Visibilidad **/  --Falta ver que onda el grado de la visibilidad

INSERT INTO NET_A_CERO.Visibilidad(visib_id, visib_desc, visib_grado, visib_precio, visib_porcentaje)
        SELECT DISTINCT Publicacion_Visibilidad_Cod, Publicacion_Visibilidad_Desc, 'Comisión por tipo de publicación', Publicacion_Visibilidad_Precio, Publicacion_Visibilidad_Porcentaje 
    FROM gd_esquema.Maestra
GO

-- RECORDAR:   CONSTRAINT [grado_visibilidad] CHECK (visib_grado IN ('Comisión por tipo de publicación', 'Comisión por producto vendido', 'Comisión por envío del producto'))


/** Migración de Rubro **/   

INSERT INTO NET_A_CERO.Rubros(rubro_desc_larga)
    SELECT DISTINCT Publicacion_Rubro_Descripcion 
    FROM gd_esquema.Maestra 
    WHERE Publicacion_Rubro_Descripcion IS NOT NULL
GO


/** Migración de Publicaciones **/

-- Creo funcion para discriminar PUBLICACIONES de clientes y empresas

IF (OBJECT_ID('NET_A_CERO.generar_id_publicacion') IS NOT NULL)
    DROP FUNCTION NET_A_CERO.generar_id_publicacion
GO

CREATE FUNCTION NET_A_CERO.generar_id_publicacion
(
    @dni NUMERIC(18,0),
    @emp_razon_social nvarchar(255)
)
RETURNS numeric(18,0)
AS
BEGIN
    DECLARE @id NUMERIC(18,0)
    IF @dni IS NULL
        BEGIN
            SELECT @id = emp_usr_id FROM NET_A_CERO.Empresas WHERE emp_razon_social = @emp_razon_social
        END
    ELSE
        BEGIN
            SELECT @id = cli_usr_id FROM NET_A_CERO.Clientes WHERE cli_dni = @dni
        END
    RETURN @id
END
GO


-- Migración de publicaciones  --FIJARSE CON QUE DATOS COMPLETAR EL COSTO

IF (OBJECT_ID('NET_A_CERO.fc_estado_publicacion') IS NOT NULL)
    DROP FUNCTION NET_A_CERO.fc_estado_publicacion
GO

CREATE FUNCTION NET_A_CERO.fc_estado_publicacion 
(
	@estado nvarchar(255)
)
RETURNS nvarchar(255)
AS
BEGIN
    DECLARE @estado_publi nvarchar(255)
    IF @estado = 'Publicada'
        BEGIN
            SET @estado_publi = 'Activa'
        END
    ELSE
        BEGIN
            SET @estado_publi = 'Borrador'
        END
    RETURN @estado_publi
END
GO


SET IDENTITY_INSERT NET_A_CERO.Publicaciones ON;
GO

INSERT INTO NET_A_CERO.Publicaciones (publi_id, publi_tipo, publi_descripcion, publi_estado, publi_stock, publi_fec_vencimiento, publi_fec_inicio, publi_precio, publi_calificacion, publi_calificacion_detalle, publi_usr_id, publi_visib_id, publi_rubro_id) 
    SELECT DISTINCT Publicacion_Cod, Publicacion_Tipo, Publicacion_Descripcion, NET_A_CERO.fc_estado_publicacion(Publicacion_Estado), Publicacion_Stock, Publicacion_Fecha_Venc, Publicacion_Fecha, 
                    Publicacion_Precio, Calificacion_Cant_Estrellas/2, Calificacion_Descripcion, NET_A_CERO.generar_id_publicacion(Publ_Cli_Dni, Publ_Empresa_Razon_Social), Publicacion_Visibilidad_Cod, (SELECT rubro_id FROM NET_A_CERO.Rubros r WHERE Publicacion_Rubro_Descripcion = r.rubro_desc_larga) 
    FROM gd_esquema.Maestra
    WHERE Publicacion_Rubro_Descripcion IS NOT NULL

SET IDENTITY_INSERT NET_A_CERO.Publicaciones OFF;
GO


/** Migracion de Compras **/
INSERT INTO NET_A_CERO.Compras(comp_cli_id, comp_publi_id, comp_fecha, comp_cantidad)
    SELECT (SELECT cli_id FROM NET_A_CERO.Clientes c WHERE c.cli_dni = Cli_Dni), Publicacion_Cod, Compra_Fecha, Compra_Cantidad
    FROM gd_esquema.Maestra
    WHERE Compra_Cantidad IS NOT NULL
    


/** Migracion de Ofertas_x_Subasta **/
INSERT INTO NET_A_CERO.Ofertas_x_Subasta(sub_usr_id, sub_monto, sub_fecha, sub_publi_id)
    SELECT (SELECT cli_usr_id FROM NET_A_CERO.Clientes c WHERE c.cli_dni = Cli_Dni), Oferta_Monto, Oferta_Fecha, Publicacion_Cod
    FROM gd_esquema.Maestra
    WHERE Oferta_Monto IS NOT NULL



/** Migración de Items **/    --TODO CUAL ES EL TIPO DE UN ITEM? VERLO!!!!!!!!!

INSERT INTO NET_A_CERO.Items(item_cantidad, item_monto, item_fact_id)
    SELECT DISTINCT Item_Factura_Monto, Item_Factura_Cantidad, Factura_Nro
    FROM gd_esquema.Maestra 
    WHERE Factura_Nro IS NOT NULL



/** Migración de Facturas realizadas al comprador (cuando se cierra la publicacion) **/

IF (OBJECT_ID('NET_A_CERO.factura_cliente_empresa') IS NOT NULL)
    DROP FUNCTION NET_A_CERO.factura_cliente_empresa
GO

CREATE FUNCTION NET_A_CERO.factura_cliente_empresa
(
	@dni NUMERIC(18,0),
	@emp_razon_social nvarchar(255)
)
RETURNS numeric(18,0)
AS
BEGIN
	DECLARE @id NUMERIC(18,0)
	IF @dni IS NULL
		BEGIN
			SELECT @id = emp_usr_id FROM NET_A_CERO.Empresas WHERE emp_razon_social = @emp_razon_social
		END
	ELSE
		BEGIN
			SELECT @id = cli_usr_id FROM NET_A_CERO.Clientes WHERE cli_dni = @dni
		END
	RETURN @id
END
GO


INSERT INTO NET_A_CERO.Facturas(fact_id, fact_fecha, fact_monto, fact_destinatario, fact_forma_pago, fact_publi_id)
    SELECT DISTINCT Factura_Nro, Factura_Fecha, Factura_Total, NET_A_CERO.factura_cliente_empresa(Publ_Cli_Dni, Publ_Empresa_Razon_Social), Forma_Pago_Desc, Publicacion_Cod
    FROM gd_esquema.Maestra 
    WHERE Factura_Nro IS NOT NULL
	AND Publicacion_Cod IS NOT NULL