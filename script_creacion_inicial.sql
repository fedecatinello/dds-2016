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


-- Creo tablas del sistema (chequear datos con base maestra)

CREATE TABLE [NET_A_CERO].[Usuarios] (
    [usr_id] INT IDENTITY(1,1) PRIMARY KEY,
    [usr_usuario] [nvarchar](8) UNIQUE NOT NULL,
    [usr_password] [nvarchar](8) NOT NULL,
    [usr_activo] [bit] NOT NULL DEFAULT 1,
    [usr_intentos] [tinyint] DEFAULT 0,
    [usr_admin] [bit] NOT NULL DEFAULT 0,
)

CREATE TABLE [NET_A_CERO].[Contacto] (
    [cont_id] INT IDENTITY(1,1) PRIMARY KEY,
    [cont_mail] [nvarchar](255) NOT NULL,
    [cont_telefono] [nvarchar](255) NULL,                -- No existe en la maestra
    [cont_calle] [nvarchar](255) NOT NULL,
    [cont_numero_calle] [NUMERIC](18, 0) NOT NULL,
    [cont_piso] [NUMERIC](18, 0),
    [cont_depto] [nvarchar](50),
    [cont_localidad] [nvarchar](255),
    [cont_codigo_postal] [nvarchar](50),
    [cont_usr_id] INT
)
    
CREATE TABLE [NET_A_CERO].[Clientes] (
    [cli_id] INT IDENTITY(1,1) PRIMARY KEY,
    [cli_nombre] [nvarchar](255) NOT NULL,
    [cli_apellido] [nvarchar](255) NOT NULL,
    [cli_dni] [NUMERIC](18, 0) UNIQUE NOT NULL,
    [cli_tipo_dni] [varchar](50) UNIQUE NOT NULL,
    [cli_fecha_nac] [datetime],
    [cli_fecha_alta] [datetime],
    [cli_usr_id] INT
)

CREATE TABLE [NET_A_CERO].[Empresas] (
    [emp_id] INT IDENTITY(1,1) PRIMARY KEY,
    [emp_razon_social] [nvarchar](255) UNIQUE NOT NULL,
    [emp_ciudad] [nvarchar](50),
    [emp_cuit] [nvarchar](50) UNIQUE NOT NULL,
    [emp_nombre_contacto] [nvarchar](255),
    [emp_rubro] [nvarchar](50),
    [emp_fecha_alta] [datetime],
    [emp_usr_id] INT
)

CREATE TABLE [NET_A_CERO].[Publicaciones] (
    [publi_id] INT IDENTITY(1,1) PRIMARY KEY,
    [publi_cod] [NUMERIC](18, 0) NOT NULL,
    [publi_tipo] [nvarchar](255) NOT NULL,          
    [publi_descripcion] [nvarchar](255) NOT NULL,
    [publi_estado] [nvarchar](255) NOT NULL,        
    [publi_stock] [NUMERIC](18, 0) NOT NULL,
    [publi_fec_vencimiento] [datetime] NOT NULL,
    [publi_fec_inicio] [datetime] NOT NULL,
    [publi_fec_finalizacion] [datetime],
    [publi_precio] [NUMERIC](18, 2) NOT NULL,
    [publi_costo] [NUMERIC](18, 2) NOT NULL,
    [publi_preguntas] [bit] DEFAULT 0,
    [publi_envio] [bit] DEFAULT 0,
    [publi_calificacion] [NUMERIC](18, 0) NOT NULL,         --Ver puntaje de la maestra
    [publi_calificacion_detalle] [nvarchar](255),
    [publi_usr_id] INT,
    [publi_visib_id] INT,
    [publi_rubro_id] INT,
    CONSTRAINT [tipo_publicacion] CHECK (publi_tipo IN ('Compra inmediata', 'Subasta')),
    CONSTRAINT [estado_publicacion] CHECK (publi_estado IN ('Borrador', 'Activa', 'Pausada', 'Finalizada')),
    CONSTRAINT [calificacion_publicacion] CHECK (publi_calificacion >= 0 AND publi_calificacion <= 5)
)
    
CREATE TABLE [NET_A_CERO].[Visibilidad] (
    [visib_id] INT IDENTITY(1,1) PRIMARY KEY,
    [visib_cod] [NUMERIC](18, 0) NOT NULL,
    [visib_grado] [nvarchar](50) NOT NULL,
    [visib_precio] [NUMERIC](18, 2) NOT NULL,
    [visib_porcentaje] [NUMERIC](18, 2) NOT NULL,
    CONSTRAINT [grado_visibilidad] CHECK (visib_grado IN ('Comisión por tipo de publicación', 'Comisión por producto vendido', 'Comisión por envío del producto'))
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
    [comp_publi_id] INT,
    [comp_fecha] [datetime],
    [comp_cantidad] [NUMERIC](18, 0)
)

CREATE TABLE [NET_A_CERO].[Rubros] (
    [rubro_id] INTEGER IDENTITY(1,1) PRIMARY KEY,
    [rubro_desc_corta] [nvarchar](50) NOT NULL,
    [rubro_desc_larga] [nvarchar](255) NOT NULL
)

CREATE TABLE [NET_A_CERO].[Ofertas_x_Subasta] (
    [sub_id] INTEGER IDENTITY(1,1) PRIMARY KEY,
    [sub_usr_id] INT NOT NULL,
    [sub_monto] [NUMERIC](18, 2) NOT NULL,
    [sub_fecha] [datetime] NOT NULL,
    [sub_ganador] [bit] NOT NULL DEFAULT 0,
    [sub_publi_id] INT
)

CREATE TABLE [NET_A_CERO].[Facturas] (
    [fact_id] INT IDENTITY(1,1) PRIMARY KEY,
    [fact_cod] [NUMERIC](18, 0) NOT NULL,
    [fact_detalle] [varchar](255),
    [fact_fecha] [datetime] NOT NULL,
    [fact_monto] [NUMERIC](18, 2) NOT NULL,
    [fact_destinatario] INT NOT NULL,
    [fact_forma_pago] [varchar](20) NOT NULL,
    [fact_publi_id] INT,
    CONSTRAINT [forma_pago] CHECK (fact_forma_pago IN ('Efectivo', 'Crédito', 'Débito', 'Sin especificar'))
)

CREATE TABLE [NET_A_CERO].[Items] (
    [item_id] INT IDENTITY(1,1) PRIMARY KEY,
    [item_cantidad] [NUMERIC](18, 0) NOT NULL,
    [item_tipo] [nvarchar](255) NOT NULL,
    [item_monto] [NUMERIC](18, 2) NOT NULL,
    [item_fact_id] INT
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


/*MIGRACION DE PUBLICACIONES*/
CREATE PROCEDURE migracion_publicaciones AS
BEGIN 
    DECLARE 
    @publi_cod NUMERIC(18, 0), 
    @publi_tipo nvarchar(255),          
    @publi_descripcion nvarchar(255),
    @publi_estado nvarchar(255),        
    @publi_stock NUMERIC(18, 0),
    @publi_fec_vencimiento datetime,
    @publi_fec_inicio datetime, 
    @publi_precio NUMERIC(18, 2),
    @publi_calificacion NUMERIC(18, 0),
    @publi_calificacion_detalle nvarchar(255)

    -- Declaración del cursor
    DECLARE cursor_publicaciones CURSOR FOR
        SELECT   Publicacion_Cod, Publicacion_Tipo, Publicacion_Descripcion, Publicacion_Estado, Publicacion_Stock, Publicacion_Fecha_Venc, Publicacion_Fecha, Publicacion_Precio, Calificacion_Cant_Estrellas, Calificacion_Descripcion    
        FROM gd_esquema.Maestra 
        WHERE Publicacion_Cod IS NOT NULL
            AND Publicacion_Tipo IS NOT NULL
            AND Publicacion_Descripcion IS NOT NULL
            AND Publicacion_Estado IS NOT NULL
            AND Publicacion_Stock IS NOT NULL       
            AND Publicacion_Precio IS NOT NULL
            AND Calificacion_Cant_Estrellas IS NOT NULL
            
        -- Apertura del cursor
        OPEN cursor_publicaciones

            -- Lectura de la primera fila del cursor
            FETCH NEXT FROM cursor_publicaciones INTO 
                @publi_cod,  
                @publi_tipo,          
                @publi_descripcion,
                @publi_estado,        
                @publi_stock,
                @publi_fec_vencimiento,
                @publi_fec_inicio,          
                @publi_precio,
                @publi_calificacion,
                @publi_calificacion_detalle
            WHILE @@FETCH_STATUS = 0 
            BEGIN
                INSERT INTO NET_A_CERO.Publicaciones ( publi_cod, publi_tipo, publi_descripcion, publi_estado, publi_stock, publi_fec_vencimiento, publi_fec_inicio, publi_precio,publi_costo, publi_calificacion, publi_calificacion_detalle)
                VALUES (@publi_cod,   @publi_tipo, @publi_descripcion, 'Activa', @publi_stock, @publi_fec_vencimiento, @publi_fec_inicio, @publi_precio, 0, @publi_calificacion/2, @publi_calificacion_detalle)
                FETCH NEXT FROM cursor_publicaciones INTO 
                @publi_cod,  
                @publi_tipo,          
                @publi_descripcion,
                @publi_estado,        
                @publi_stock,
                @publi_fec_vencimiento,
                @publi_fec_inicio,
                @publi_precio,
                @publi_calificacion,
                @publi_calificacion_detalle 
            END
        -- Cierre del cursor
        CLOSE cursor_publicaciones
    -- Liberar los recursos
    DEALLOCATE cursor_publicaciones
END

/* PARA EJECUTARLO */
--EXEC migracion_publicaciones

EXEC migracion_publicaciones







 

