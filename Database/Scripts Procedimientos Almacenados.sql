------****************************** PROCEDIMIENTOS ALMACENADOS ******************************

USE VentaMusical;

/*--------------------------------------------------------------------*/
/*------------------------- TABLA DE ALBUM -------------------------*/
/*--------------------------------------------------------------------*/

---- ________________________Insertar Album________________________----
CREATE PROCEDURE spInsertarAlbum
    @CodigoArtista  INT,
    @NombreAlbum    VARCHAR(150),
    @AnoLanzamiento INT,
    @Imagen         VARCHAR(150)
AS
BEGIN
    
    IF NOT EXISTS (SELECT 1 FROM Artistas WHERE CodigoArtista = @CodigoArtista)
    BEGIN
        
        RAISERROR('Error: El artista con el código especificado no existe.', 16, 1);
        RETURN;
    END

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO Albumes (CodigoArtista, NombreAlbum, AnoLanzamiento, Imagen)
        VALUES (@CodigoArtista, @NombreAlbum, @AnoLanzamiento, @Imagen);
        
        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
    END CATCH
END
GO

---- ________________________Actualizar Album________________________----
CREATE PROCEDURE spActualizarAlbum
    @CodigoAlbum    INT,
    @CodigoArtista  INT,
    @NombreAlbum    VARCHAR(150),
    @AnoLanzamiento INT,
    @Imagen         VARCHAR(150)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Artistas WHERE CodigoArtista = @CodigoArtista)
    BEGIN
        RAISERROR('Error: El artista con el código especificado no existe.', 16, 1);
        RETURN;
    END

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE Albumes
        SET
            CodigoArtista = @CodigoArtista,
            NombreAlbum = @NombreAlbum,
            AnoLanzamiento = @AnoLanzamiento,
            Imagen = @Imagen
        WHERE
            CodigoAlbum = @CodigoAlbum;

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
    END CATCH
END
GO

---- ________________________Eliminar Album________________________----
CREATE PROCEDURE spEliminarAlbum
    @CodigoAlbum INT
AS
BEGIN
    
   DELETE FROM Albumes
    WHERE CodigoAlbum = @CodigoAlbum;
END
GO

---- ________________________Listar todos los Albumes________________________----
CREATE PROCEDURE spListarAlbumes
AS
BEGIN
    SELECT CodigoAlbum, CodigoArtista, NombreAlbum, AnoLanzamiento, Imagen
    FROM Albumes
    ORDER BY NombreAlbum;
END
GO

---- ________________________Listar Album por ID________________________----
CREATE PROCEDURE spListarAlbumPorID
    @CodigoAlbum INT
AS
BEGIN
    SELECT CodigoAlbum, CodigoArtista, NombreAlbum, AnoLanzamiento, Imagen
    FROM Albumes
    WHERE CodigoAlbum = @CodigoAlbum;
END
GO

/*--------------------------------------------------------------------*/
/*------------------------- TABLA DE ARTISTAS -------------------------*/
/*--------------------------------------------------------------------*/

---- ________________________Agregar Artista________________________ ----
CREATE PROCEDURE spAgregarArtista
    @NombreArtistico    VARCHAR(100),
    @FechaNacimiento    VARCHAR(100),
    @NombreReal         VARCHAR(100),
    @Nacionalidad       VARCHAR(100),
    @Foto               VARCHAR(100),
    @LinkBiografia      VARCHAR(MAX)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO ARTISTAS (NombreArtistico, FechaNacimiento, NombreReal, Nacionalidad, Foto, LinkBiografia)
        VALUES (@NombreArtistico, @FechaNacimiento, @NombreReal, @Nacionalidad, @Foto, @LinkBiografia);

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR('Error al agregar al artista: %s', 16, 1, @ErrorMessage);
    END CATCH
END;
GO

---- ________________________Actualizar Artista________________________ ----
CREATE PROCEDURE spActualizarArtista
    @CodigoArtista      INT,
    @NombreArtistico    VARCHAR(100),
    @FechaNacimiento    VARCHAR(100),
    @NombreReal         VARCHAR(100),
    @Nacionalidad       VARCHAR(100),
    @Foto               VARCHAR(100),
    @LinkBiografia      VARCHAR(MAX)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE ARTISTAS
        SET 
            NombreArtistico = @NombreArtistico,
            FechaNacimiento = @FechaNacimiento,
            NombreReal = @NombreReal,
            Nacionalidad = @Nacionalidad,
            Foto = @Foto,
            LinkBiografia = @LinkBiografia
        WHERE 
            CodigoArtista = @CodigoArtista;

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR('Error al actualizar el artista: %s', 16, 1, @ErrorMessage);
    END CATCH
END;
GO

---- ________________________Eliminar Artista________________________ ----
CREATE PROCEDURE spEliminarArtista
    @CodigoArtista INT
AS
BEGIN
    DELETE
    FROM Artistas
    WHERE CodigoArtista = @CodigoArtista;
END;
GO

---- ________________________Mostrar Artistas________________________ ----
CREATE PROCEDURE spMostrarTodosArtistas
AS
BEGIN
    SELECT CodigoArtista, NombreArtistico, FechaNacimiento, NombreReal, Nacionalidad, Foto, LinkBiografia
    FROM Artistas;
END;
GO

---- ________________________Mostrar Artista Por Id________________________ ----
CREATE PROCEDURE spMostrarArtistaPorId
    @CodigoArtista INT
AS
BEGIN
    SELECT CodigoArtista, NombreArtistico, FechaNacimiento, NombreReal, Nacionalidad, Foto, LinkBiografia
    FROM Artistas
    WHERE CodigoArtista = @CodigoArtista;
END;
GO

/*--------------------------------------------------------------------*/
/*------------------------- TABLA DE CANCIONES -------------------------*/
/*--------------------------------------------------------------------*/


------________________________Insertar________________________----

CREATE PROCEDURE spAgregarCancion
    @CodigoGenero         INT,
    @CodigoAlbum          INT,
    @NombreCancion        VARCHAR(150),
    @LinkVideo            VARCHAR(MAX),
    @Precio               DECIMAL(10,2),
    @CantidadDisponible   INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO Canciones 
        (CodigoGenero, CodigoAlbum, NombreCancion, LinkVideo, Precio, CantidadDisponible)
        VALUES 
        (@CodigoGenero, @CodigoAlbum, @NombreCancion, @LinkVideo, @Precio, @CantidadDisponible);

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();
        RAISERROR('Error al agregar la canción: %s', @ErrorSeverity, @ErrorState, @ErrorMessage);
    END CATCH
END

------________________________Actualizar________________________----

CREATE PROCEDURE spActualizarCancion
    @CodigoCancion        INT,
    @CodigoGenero         INT,
    @CodigoAlbum          INT,
    @NombreCancion        VARCHAR(150),
    @LinkVideo            VARCHAR(MAX),
    @Precio               DECIMAL(10,2),
    @CantidadDisponible   INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE Canciones
        SET
            CodigoGenero = @CodigoGenero,
            CodigoAlbum = @CodigoAlbum,
            NombreCancion = @NombreCancion,
            LinkVideo = @LinkVideo,
            Precio = @Precio,
            CantidadDisponible = @CantidadDisponible
        WHERE CodigoCancion = @CodigoCancion;

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();
        RAISERROR('Error al actualizar la canción: %s', @ErrorSeverity, @ErrorState, @ErrorMessage);
    END CATCH
END


------________________________Eliminar________________________----

CREATE PROCEDURE spEliminarCancion
    @CodigoCancion INT
AS
BEGIN
    DELETE FROM Canciones
    WHERE CodigoCancion = @CodigoCancion;
END


------________________________Buscar Todas________________________----

CREATE PROCEDURE spMostrarTodasCanciones
AS
BEGIN
    SELECT CodigoCancion, CodigoGenero, CodigoAlbum, NombreCancion, LinkVideo, Precio, CantidadDisponible
    FROM Canciones;
END


------________________________Buscar Por ID________________________----

CREATE PROCEDURE spMostrarCancionPorCodigo
    @CodigoCancion INT
AS
BEGIN
    SELECT CodigoCancion, CodigoGenero, CodigoAlbum, NombreCancion, LinkVideo, Precio, CantidadDisponible
    FROM Canciones
    WHERE CodigoCancion = @CodigoCancion;
END


/*--------------------------------------------------------------------*/
/*------------------------- TABLA DE AUDITORIA -------------------------*/
/*--------------------------------------------------------------------*/


------________________________Mostrar Todos________________________----

CREATE PROCEDURE spMostrarAuditoria
AS
BEGIN
    SELECT 
        ID,
        FechaRegistro,
        Usuario,
        Accion,
        TablaAfectada,
        IDRegistroAfectado,
        Detalles,
        IPUsuario
    FROM Auditoria
    ORDER BY FechaRegistro DESC;
END


------________________________Mostrar Tabla Afectada________________________----

CREATE PROCEDURE spMostrarAuditoriaPorTabla
    @TablaAfectada VARCHAR(100)
AS
BEGIN
    SELECT 
        ID,
        FechaRegistro,
        Usuario,
        Accion,
        TablaAfectada,
        IDRegistroAfectado,
        Detalles,
        IPUsuario
    FROM Auditoria
    WHERE TablaAfectada = @TablaAfectada
    ORDER BY FechaRegistro DESC;
END


------________________________Mostrar Por Rango de Fecha________________________----

CREATE PROCEDURE spMostrarAuditoriaPorFecha
    @FechaInicio DATETIME,
    @FechaFin    DATETIME
AS
BEGIN
    SELECT 
        ID,
        FechaRegistro,
        Usuario,
        Accion,
        TablaAfectada,
        IDRegistroAfectado,
        Detalles,
        IPUsuario
    FROM Auditoria
    WHERE FechaRegistro BETWEEN @FechaInicio AND @FechaFin
    ORDER BY FechaRegistro DESC;
END


------________________________Dato Importante________________________----

----Como es una tabla de auditoria no se debe permitir las acciones de insertar datos, 
----modificarlos o eliminarlos. Solamente se permite consultarlos.



/*--------------------------------------------------------------------*/
/*------------------------- TABLA DE DETALLES -------------------------*/
/*--------------------------------------------------------------------*/


------________________________Insertar________________________----

CREATE PROCEDURE InsertarDetalleVenta    
	@NumeroFactura  INT, 
	@CodigoCancion   INT,
	@Subtotal		DECIMAl(10,2)

AS
BEGIN
    IF EXISTS (SELECT 1 FROM Ventas WHERE NumeroFactura = @NumeroFactura)
       AND EXISTS (SELECT 1 FROM Canciones WHERE CodigoCancion = @CodigoCancion)
    BEGIN
        INSERT INTO DetalleVenta (NumeroFactura, CodigoCancion, Subtotal)
        VALUES (@NumeroFactura, @CodigoCancion, @Subtotal);
    END
    ELSE
    BEGIN
        PRINT 'Error: Factura o Canción no existente';
    END
END;

--EXEC InsertarDetalleVenta
--	@NumeroFactura = ,
--	@CodigoCancion = ,
--	@Subtotal = ;


------________________________Editar________________________----

CREATE PROCEDURE EditarDetalleVenta
    @IDDetalle INT,
    @NumeroFactura INT,
    @CodigoCancion INT,
    @Subtotal DECIMAL(10,2)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM DetalleVenta WHERE IDDetalle = @IDDetalle)
    BEGIN
        UPDATE DetalleVenta
        SET NumeroFactura = @NumeroFactura,
            CodigoCancion = @CodigoCancion,
            Subtotal = @Subtotal
        WHERE IDDetalle = @IDDetalle;
    END
    ELSE
    BEGIN
        PRINT 'No se encontró el detalle de venta con ese ID.';
    END
END;


--EXEC EditarDetalleVenta 
--    @IDDetalle = ,
--    @NumeroFactura = ,
--    @CodigoCancion = ,
--    @Subtotal = ;

------________________________Eliminar________________________----

CREATE PROCEDURE EliminarDetalleVenta
    @IDDetalle INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM DetalleVenta WHERE IDDetalle = @IDDetalle)
    BEGIN
        DELETE FROM DetalleVenta
        WHERE IDDetalle = @IDDetalle;
    END
    ELSE
    BEGIN
        PRINT 'No se encontró el detalle de venta con ese ID.';
    END
END;

--EXEC EliminarDetalleVenta @IDDetalle = ;

------________________________Lista________________________----

CREATE PROCEDURE ListarDetalleVenta
AS
BEGIN
    SELECT IDDetalle, NumeroFactura, CodigoCancion, Subtotal
    FROM DetalleVenta
    ORDER BY IDDetalle;
END;

--EXEC ListarDetalleVenta;


/*--------------------------------------------------------------------*/
/*------------------------- TABLA DE GENEROS -------------------------*/
/*--------------------------------------------------------------------*/

------________________________Insertar________________________----

CREATE PROCEDURE InsertarGenero
	@Descripcion VARCHAR(100)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Generos WHERE Descripcion = @Descripcion)
    BEGIN
        INSERT INTO Generos (Descripcion)
        VALUES (@Descripcion);
    END
    ELSE
    BEGIN
        PRINT 'El género ya existe.';
    END
END;

--EXEC InsertarGenero @Descripcion = 'Rock';


------________________________Lista________________________----

CREATE PROCEDURE ListaGeneros
AS
BEGIN
	SELECT CodigoGenero, Descripcion
	FROM Generos
	ORDER BY Descripcion;
END;

--EXEC ListaGeneros;


------________________________Editar________________________----

CREATE PROCEDURE EditarGenero
	@CodigoGenero		INT,
	@NuevaDescripcion	VARCHAR(100)
AS
BEGIN

	UPDATE Generos
	SET Descripcion = @NuevaDescripcion
	WHERE @CodigoGenero = @CodigoGenero;
END;

--EXEC EditarGenero @CodigoGenero  = 2, @NuevaDescripcion = 'Pop Latino';


------________________________Eliminar________________________----

CREATE PROCEDURE EliminarGenero
    @CodigoGenero INT
AS
BEGIN
    DELETE FROM Generos
    WHERE CodigoGenero = @CodigoGenero;
END;

--EXEC EliminarGenero @Codigogenero = ;


/*--------------------------------------------------------------------*/
/*------------------------- TABLA DE USUARIOS -------------------------*/
/*--------------------------------------------------------------------*/


------ ________________________Agregar Usuario________________________ -----
CREATE PROCEDURE spAgregarUsuario
	@NumeroIdentificacion	VARCHAR (30),
	@Nombre					VARCHAR(30),
	@Apellido				VARCHAR(50),
	@Genero					NVARCHAR(10),
	@CorreoElectronico		VARCHAR(255),
	@TipoTarjeta			NVARCHAR(25),
	@DineroDisponible		DECIMAL(10,2),
	@NumeroTarjeta			VARCHAR(25),
	@Contrasena				VARCHAR(30),
	@Perfil					NVARCHAR(25)

AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO Usuarios 
		( NumeroIdentificacion, Nombre, Apellido, Genero, CorreoElectronico, TipoTarjeta,
		  DineroDisponible, NumeroTarjeta, Contrasena, Perfil )

        VALUES 
		( @NumeroIdentificacion, @Nombre, @Apellido, @Genero, @CorreoElectronico, @TipoTarjeta,
		  @DineroDisponible, @NumeroTarjeta, @Contrasena, @Perfil );

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
		DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();
        RAISERROR('Error al agregar al usuario: %s', @ErrorSeverity, @ErrorState, @ErrorMessage)
    END CATCH
END


-------- ________________________Actualizar Usuario________________________ ----
CREATE PROCEDURE spActualizarUsuario
    @ID					  INT,
    @NumeroIdentificacion VARCHAR(30),
    @Nombre               VARCHAR(30),
    @Apellido             VARCHAR(50),
    @Genero               NVARCHAR(10),
    @CorreoElectronico    VARCHAR(255),
    @TipoTarjeta          NVARCHAR(25),
    @DineroDisponible     DECIMAL(10,2),
    @NumeroTarjeta        VARCHAR(25),
    @Contrasena           VARCHAR(30),
    @Perfil               NVARCHAR(25)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE Usuarios
        SET 
            NumeroIdentificacion = @NumeroIdentificacion,
            Nombre = @Nombre,
            Apellido = @Apellido,
            Genero = @Genero,
            CorreoElectronico = @CorreoElectronico,
            TipoTarjeta = @TipoTarjeta,
            DineroDisponible = @DineroDisponible,
            NumeroTarjeta = @NumeroTarjeta,
            Contrasena = @Contrasena,
            Perfil = @Perfil
        WHERE 
            ID = @ID;

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;

        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();
        RAISERROR('Error al actualizar el usuario: %s', @ErrorSeverity, @ErrorState, @ErrorMessage);
    END CATCH
END



-------- ________________________Eliminar Usuario________________________ ----
CREATE PROCEDURE spEliminarUsuario
    @Id INT
AS
BEGIN
    DELETE
    FROM Usuarios
    WHERE ID = @Id
END


-------- ________________________Mostrar Usuarios________________________ ----
CREATE PROCEDURE spMostrarTodosUsuarios
   
AS
BEGIN
    SELECT Id, NumeroIdentificacion, Nombre, Apellido, Genero, CorreoElectronico, TipoTarjeta,
		   DineroDisponible, NumeroTarjeta, Contrasena, Perfil
    FROM Usuarios
END



-------- ________________________Mostrar Usuario Por Id________________________ ----
CREATE PROCEDURE spMostrarUsuariosPorId
    @Id INT
AS
BEGIN
    SELECT Id, NumeroIdentificacion, Nombre, Apellido, Genero, CorreoElectronico, TipoTarjeta,
		   DineroDisponible, NumeroTarjeta, Contrasena, Perfil
    FROM Usuarios
    WHERE ID = @Id
END



/*--------------------------------------------------------------------*/
/*------------------------- TABLA DE VENTAS -------------------------*/
/*--------------------------------------------------------------------*/

------ ________________________Agregar Venta________________________ ----
CREATE PROCEDURE spAgregarVenta
    @IDUsuario         INT,
    @FechaCompra       DATETIME,
    @Total             DECIMAL(10,2),
    @TipoPago          NVARCHAR(25)

AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO Ventas 
		( IDUsuario, FechaCompra, Total, TipoPago)

        VALUES 
		( @IDUsuario, @FechaCompra, @Total, @TipoPago );

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
		DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();
        RAISERROR('Error al realizar la venta: %s', @ErrorSeverity, @ErrorState, @ErrorMessage)
    END CATCH
END


------ ________________________Actualizar Venta________________________ ----
--/* 
--No implementada pues no considero que sea correcto poder actualizar una venta 
--(alterar una factura)
--*/

------ ________________________Eliminar Venta________________________ ----
--/* 
--No implementada pues no considero que sea correcto poder eliminar una venta 
--(alterar una factura)
--*/

------ ________________________Mostrar Ventas________________________ ----
CREATE PROCEDURE spMostrarVentas
   
AS
BEGIN
    SELECT NumeroFactura, IDUsuario, FechaCompra, Total, TipoPago
    FROM Ventas
END


------ ________________________Mostrar Venta Por Id________________________ ----
CREATE PROCEDURE spMostrarVentaPorId
    @NumeroFactura INT
AS
BEGIN
    SELECT NumeroFactura, IDUsuario, FechaCompra, Total, TipoPago
    FROM Ventas
    WHERE NumeroFactura = @NumeroFactura
END

------****************************** FIN PROCEDIMIENTOS ALMACENADOS *****************************