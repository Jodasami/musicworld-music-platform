----************************************ CREACION BD ************************************
CREATE DATABASE VentaMusical;
GO

USE VentaMusical;
GO

CREATE TABLE Usuarios(
	ID						INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	NumeroIdentificacion	VARCHAR (30) UNIQUE NOT NULL,
	Nombre					VARCHAR(30) NOT  NULL,
	Apellido				VARCHAR(50) NOT  NULL,
	Genero					NVARCHAR(10)  NOT NULL,
	CorreoElectronico		VARCHAR(255) NOT NULL,
	TipoTarjeta				NVARCHAR(25)  NOT NULL,
	DineroDisponible		DECIMAL(10,2) NOT NULL,
	NumeroTarjeta			VARCHAR(25) NOT NULL,
	Contrasena				VARCHAR(30) NOT NULL,
	Perfil					NVARCHAR(25)  NOT NULL
);
GO

CREATE TABLE Generos(
	CodigoGenero		INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Descripcion			VARCHAR(100) NOT NULL
);
GO

CREATE TABLE Artistas(
	CodigoArtista		INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	NombreArtistico		VARCHAR(100)NOT NULL,
	FechaNacimiento		VARCHAR(100)NOT NULL,
	NombreReal			VARCHAR(100)NOT NULL,
	Nacionalidad		VARCHAR(100)NOT NULL,
	Foto				VARCHAR(100)NOT NULL,
	LinkBiografia		VARCHAR(MAX)NOT NULL,
);
GO

CREATE TABLE Albumes(
	CodigoAlbum		INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	CodigoArtista	INT NOT NULL,
	NombreAlbum		VARCHAR(150)NOT NULL,
	AnoLanzamiento	INT NOT NULL,
	Imagen			VARCHAR(150)NOT NULL,
	FOREIGN KEY (CodigoArtista) REFERENCES Artistas(CodigoArtista) ON DELETE CASCADE
);
GO

CREATE TABLE Canciones(
	CodigoCancion		 INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	CodigoGenero		 INT NOT NULL,
	CodigoAlbum			 INT NOT NULL,
	NombreCancion		 VARCHAR(150) NOT NULL,
	LinkVideo			 VARCHAR(MAX) NOT NULL,
	Precio				 DECIMAL(10,2)	NOT NULL,
	CantidadDisponible	 INT NOT NULL,
	FOREIGN KEY (CodigoGenero) REFERENCES Generos(CodigoGenero),
	FOREIGN KEY (CodigoAlbum) REFERENCES Albumes(CodigoAlbum) ON DELETE CASCADE
);
GO

CREATE TABLE Ventas(
    NumeroFactura     INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    IDUsuario         INT NOT NULL,
    FechaCompra       DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
    Total             DECIMAL(10,2) NOT NULL,
    TipoPago          NVARCHAR(25)  NOT NULL,
    FOREIGN KEY (IDUsuario) REFERENCES Usuarios(ID) ON DELETE NO ACTION
);
GO

CREATE TABLE DetalleVenta(
	IDDetalle		INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	NumeroFactura	INT NOT NULL,
	CodigoCancion	INT NOT NULL,
	Subtotal		DECIMAL(10,2) NOT NULL,
	FOREIGN KEY (NumeroFactura) REFERENCES Ventas(NumeroFactura),
	FOREIGN KEY (CodigoCancion) REFERENCES Canciones(CodigoCancion) 
);
GO

CREATE TABLE Auditoria(
	ID					INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	FechaRegistro		DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
	Usuario				VARCHAR(100)	NOT NULL,
	Accion				NVARCHAR (30)	NOT NULL,
	TablaAfectada		VARCHAR(100) NOT NULL,
	IDRegistroAfectado	INT NOT NULL,
	Detalles			VARCHAR(100) NOT NULL,
	IPUsuario			VARCHAR(35) NOT NULL
);
GO

---- CREACI�N DE INDEX ------------;

-- --indices para la tabla Usuarios
CREATE INDEX IX_Usuarios_NumeroIdentificacion ON Usuarios(NumeroIdentificacion);
CREATE INDEX IX_Usuarios_CorreoElectronico ON Usuarios(CorreoElectronico);

-- --indices para la tabla Generos
CREATE INDEX IX_Generos_Descripcion ON Generos(Descripcion);

-- --indices para la tabla Artistas
CREATE INDEX IX_Artistas_NombreArtistico ON Artistas(NombreArtistico);

-- --indices para la tabla Albumes
CREATE INDEX IX_Albumes_CodigoArtista ON Albumes(CodigoArtista);
CREATE INDEX IX_Albumes_NombreAlbum ON Albumes(NombreAlbum);

-- --indices para la tabla Canciones
CREATE INDEX IX_Canciones_CodigoGenero ON Canciones(CodigoGenero);
CREATE INDEX IX_Canciones_CodigoAlbum ON Canciones(CodigoAlbum);
CREATE INDEX IX_Canciones_NombreCancion ON Canciones(NombreCancion);

-- --indices para la tabla Ventas
CREATE INDEX IX_Ventas_IDUsuario ON Ventas(IDUsuario);
CREATE INDEX IX_Ventas_FechaCompra ON Ventas(FechaCompra);

-- --indices para la tabla DetalleVenta
CREATE INDEX IX_DetalleVenta_NumeroFactura ON DetalleVenta(NumeroFactura);
CREATE INDEX IX_DetalleVenta_CodigoCancion ON DetalleVenta(CodigoCancion);

-- --indices para la tabla Auditoria
CREATE INDEX IX_Auditoria_Usuario ON Auditoria(Usuario);
CREATE INDEX IX_Auditoria_FechaRegistro ON Auditoria(FechaRegistro);
CREATE INDEX IX_Auditoria_TablaAfectada ON Auditoria(TablaAfectada);


----************************************ FIN CREACION BD ************************************