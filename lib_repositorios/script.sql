CREATE DATABASE db_tienda_de_discos;
GO
USE db_tienda_de_discos;
GO

CREATE TABLE [Clientes] (
    [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [NombreCliente] NVARCHAR(150) NOT NULL,
    [ApellidoCliente] NVARCHAR(150) NOT NULL,
    [DireccionCliente] NVARCHAR(150) NOT NULL,
    [TelefonoCliente] NVARCHAR(150) NOT NULL
);

CREATE TABLE [Artistas] (
    [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [NombreArtista] NVARCHAR(150) NOT NULL,
    [GeneroMusical] NVARCHAR(150) NOT NULL
);

CREATE TABLE [Marcas] (
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [NombreMarca] NVARCHAR(100) NOT NULL,
    [PaginaWeb] NVARCHAR(255) NOT NULL
);

CREATE TABLE [Discos] (
    [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Artista] INT NOT NULL,
    [Marca] INT NOT NULL,
    [NombreDisco] NVARCHAR(150) NOT NULL,
    [DuracionDisco] NVARCHAR(20) NOT NULL,
    [FechaLanzamiento] SMALLDATETIME NOT NULL,
    FOREIGN KEY ([Artista]) REFERENCES [Artistas]([Id]),
    FOREIGN KEY ([Marca]) REFERENCES [Marcas]([Id])
);

CREATE TABLE [Formatos] (
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [TipoFormato] NVARCHAR(100) NOT NULL,
    [Material] NVARCHAR(100) NOT NULL
);

CREATE TABLE [Pagos] (
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [TipoPago] NVARCHAR(50) NOT NULL,
    [Pais_Disponibilidad] NVARCHAR(100) NOT NULL
);

CREATE TABLE [Ordenes] (
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [Fecha] SMALLDATETIME NOT NULL,
    [Cliente] INT NOT NULL,
    [Pago] INT NOT NULL,
    [MontoTotal] DECIMAL(10,2) NOT NULL,
    FOREIGN KEY ([Cliente]) REFERENCES [Clientes]([Id]),
    FOREIGN KEY ([Pago]) REFERENCES [Pagos]([Id])
);

CREATE TABLE [OrdenesDiscos] (
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [Orden] INT NOT NULL,
    [Disco] INT NOT NULL,
    [Formato] INT NOT NULL,
    [Cantidad] INT NOT NULL,
    [ValorUnitario] DECIMAL(10,2) NOT NULL,
    FOREIGN KEY ([Orden]) REFERENCES [Ordenes]([Id]),
    FOREIGN KEY ([Disco]) REFERENCES [Discos]([Id]),
    FOREIGN KEY ([Formato]) REFERENCES [Formatos]([Id])
);

CREATE TABLE [Auditorias] (
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [Entidad] varchar(200) NOT NULL,
    [Operacion] varchar(200) NOT NULL,
    [Fecha] smalldatetime NOT NULL,
    [Datos] varchar(200) NOT NULL
);
go

CREATE TABLE [Roles] (
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [NombreRol] varchar(50) NOT NULL,
[Descripcion] varchar(200) NOT NULL
   
);
go

CREATE TABLE [Usuarios] (
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [Nombre] varchar(50) NOT NULL,
    [Apellido] varchar(50) NOT NULL,
    [Email]  varchar(100)NOT NULL unique,
    [NombreUsuario] varchar(50) NOT NULL,
[Contrase�a] varchar(50) NOT NULL,
[Rol] int NOT NULL

   
    FOREIGN KEY (Rol) REFERENCES Roles(Id)  
);
go




------------------------------------------------------------------------------------------------------------
INSERT INTO [Clientes] (NombreCliente, ApellidoCliente, DireccionCliente, TelefonoCliente)
VALUES ('Juan', 'Perez', 'Calle 45', '304258299');

INSERT INTO [Artistas] ([NombreArtista], [GeneroMusical])
VALUES ('The Doors', 'Rock');

INSERT INTO [Marcas] ([NombreMarca], [PaginaWeb])
VALUES ('Sony', 'www.sony.com');

INSERT INTO [Pagos] ([TipoPago], [Pais_Disponibilidad])
VALUES ('Tarjeta de Cr�dito', 'Colombia');

INSERT INTO [Discos] ([Artista], [Marca], [NombreDisco], [DuracionDisco], [FechaLanzamiento])
VALUES (1, 1, 'Love Street', '3:20', '1968-07-02');

INSERT INTO [Formatos] ([TipoFormato], [Material])
VALUES ('Vinilo', 'Aluminio');

INSERT INTO [Ordenes] ([Fecha], [Cliente], [Pago], [MontoTotal])
VALUES ('2024-11-02', 1, 1, 1000);

INSERT INTO [OrdenesDiscos] ([Orden], [Disco], [Formato], [Cantidad], [ValorUnitario])
VALUES (1, 1, 1, 1, 100);


INSERT INTO [Roles] (NombreRol, Descripcion)
VALUES ('Administrador', 'crear, editar, eliminar');
INSERT INTO [Roles] (NombreRol, Descripcion)
VALUES ('Cliente', 'realizar una compra');

INSERT INTO [Usuarios] (Nombre, Apellido, Email, NombreUsuario, Contrase�a, Rol)
VALUES ('mario', 'correa', 'mario@gmail.com', 'admin', '123', 1);

INSERT INTO [Usuarios] (Nombre, Apellido, Email, NombreUsuario, Contrase�a, Rol)
VALUES ('juan', 'alvarez', 'juan@gmail.com', 'juan567', 'Contrase�a2', 2);
