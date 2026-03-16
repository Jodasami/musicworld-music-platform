----******************************TRIGGER ******************************

USE VentaMusical;

/*--------------------------------------------------------------------*/
/*------------------------- TABLA DE ALBUMES -------------------------*/
/*--------------------------------------------------------------------*/


-- ________________________Insert________________________----
CREATE TRIGGER tr_InsertAlbumes
ON Albumes
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
    SELECT 
        GETDATE(),
        i.NombreAlbum,      -- Nombre del álbum como identificador en la auditoría
        'INSERT',
        'Albumes',
        i.CodigoAlbum,      -- ID del nuevo álbum
        'Nuevo álbum insertado: ' + i.NombreAlbum,
        SUSER_NAME()
    FROM inserted i;
END
GO

-- ________________________Update________________________----
CREATE TRIGGER tr_UpdateAlbumes
ON Albumes
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
    SELECT 
        GETDATE(),
        i.NombreAlbum,
        'UPDATE',
        'Albumes',
        i.CodigoAlbum,      -- ID del álbum actualizado
        'Álbum actualizado: ' + i.NombreAlbum,
        SUSER_NAME()
    FROM inserted i;
END
GO

-- ________________________Delete________________________----
CREATE TRIGGER tr_DeleteAlbumes
ON Albumes
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
    SELECT 
        GETDATE(),
        d.NombreAlbum,
        'DELETE',
        'Albumes',
        d.CodigoAlbum,      -- ID del álbum eliminado
        'Álbum eliminado: ' + d.NombreAlbum,
        SUSER_NAME()
    FROM deleted d;
END
GO

/*--------------------------------------------------------------------*/
/*------------------------- TABLA DE ARTISTAS -------------------------*/
/*--------------------------------------------------------------------*/

-- ________________________Insert________________________----
CREATE TRIGGER tr_InsertArtistas
ON Artistas
AFTER INSERT
AS
BEGIN
    -- Evita que se devuelva el recuento de filas afectadas
    SET NOCOUNT ON;

    -- Inserta el registro de auditoría para la nueva inserción
    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
    SELECT 
        GETDATE(),          -- Fecha y hora actual de la operación
        i.NombreArtistico,  -- Nombre del artista como 'Usuario' que realiza la acción
        'INSERT',           -- Acción realizada
        'Artistas',         -- Tabla afectada
        i.CodigoArtista,    -- ID del nuevo registro insertado
        'Nuevo artista insertado: ' + i.NombreArtistico, -- Detalles de la operación
        SUSER_NAME()        -- Nombre del usuario de la sesión de SQL Server
	FROM inserted i;        -- 'inserted' es una tabla virtual que contiene las nuevas filas
END
GO

-- ________________________Update________________________----
CREATE TRIGGER tr_UpdateArtistas
ON Artistas
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Inserta el registro de auditoría para la actualización
    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
    SELECT 
        GETDATE(),
        i.NombreArtistico,
        'UPDATE',
        'Artistas',
        i.CodigoArtista,    -- ID del registro actualizado
        'Artista actualizado: ' + i.NombreArtistico,
        SUSER_NAME()
    FROM inserted i;
END
GO

-- ________________________Delete________________________----
CREATE TRIGGER tr_DeleteArtistas
ON Artistas
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Inserta el registro de auditoría para la eliminación
    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
    SELECT 
        GETDATE(),
        d.NombreArtistico,  -- Se obtiene el nombre de la tabla 'deleted'
        'DELETE',
        'Artistas',
        d.CodigoArtista,    -- ID del registro eliminado
        'Artista eliminado: ' + d.NombreArtistico,
        SUSER_NAME()
    FROM deleted d;         -- 'deleted' es una tabla virtual que contiene las filas eliminadas
END
GO

/*--------------------------------------------------------------------*/
/*------------------------- TABLA DE CANCIONES -------------------------*/
/*--------------------------------------------------------------------*/

---- ________________________Insert________________________ ----

CREATE TRIGGER tr_Canciones_Insert
ON Canciones
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
    SELECT 
        GETDATE(),
        'Sistema',
        'INSERT',
        'Canciones',
        i.CodigoCancion,
        'Canción agregada',
        'Servidor SQL'
    FROM inserted i;
END


---- ________________________Update________________________ ----

CREATE TRIGGER tr_Canciones_Update
ON Canciones
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
    SELECT 
        GETDATE(),
        'Sistema',
        'UPDATE',
        'Canciones',
        i.CodigoCancion,
        'Canción actualizada',
        'Servidor SQL'
    FROM inserted i
    JOIN deleted d ON i.CodigoCancion = d.CodigoCancion;
END


---- ________________________Delete________________________ ----

CREATE TRIGGER tr_Canciones_Delete
ON Canciones
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
    SELECT 
        GETDATE(),
        'Sistema',
        'DELETE',
        'Canciones',
        d.CodigoCancion,
        'Canción eliminada',
        'Servidor SQL'
    FROM deleted d;
END

/*--------------------------------------------------------------------*/
/*------------------------- TABLA DE DETALLES -------------------------*/
/*--------------------------------------------------------------------*/

----________________________Insertar________________________----

CREATE TRIGGER trg_InsertDetalleVenta
ON DetalleVenta
AFTER INSERT
AS
BEGIN
    INSERT INTO AuditoriaDetalleVenta (NumeroFactura, CodigoCancion, Subtotal, FechaInsert, UsuarioInserto)
    SELECT 
        i.NumeroFactura,
        i.CodigoCancion,
        i.Subtotal,
        GETDATE(),
        SYSTEM_USER
    FROM inserted i;
END;

----________________________Actualizar________________________----

CREATE TRIGGER tr_ActualizarTotalVenta_DespuesInsertar
ON DetalleVenta
AFTER INSERT
AS
BEGIN
    UPDATE v
    SET v.Total = (
        SELECT SUM(Subtotal)
        FROM DetalleVenta dv
        WHERE dv.NumeroFactura = v.NumeroFactura
    )
    FROM Ventas v
    JOIN inserted i ON v.NumeroFactura = i.NumeroFactura;
END;


----________________________Validar SubTotal no sea Menor a cero________________________----

CREATE TRIGGER tr_ValidarSubtotal_MayorCero
ON DetalleVenta
INSTEAD OF INSERT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM inserted WHERE Subtotal <= 0)
    BEGIN
        RAISERROR('El subtotal debe ser mayor a cero.', 16, 1);
        ROLLBACK;
        RETURN;
    END

    INSERT INTO DetalleVenta (NumeroFactura, CodigoCancion, Subtotal)
    SELECT NumeroFactura, CodigoCancion, Subtotal FROM inserted;
END;


/*--------------------------------------------------------------------*/
/*------------------------- TABLA DE GENEROS -------------------------*/
/*--------------------------------------------------------------------*/

----________________________No Exista Duplicados________________________----

CREATE TRIGGER trg_PreventDuplicateGenero
ON Generos
INSTEAD OF INSERT
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN Generos g ON LOWER(i.Descripcion) = LOWER(g.Descripcion)
    )
    BEGIN
        RAISERROR('Ya existe un género con esa descripción.', 16, 1);
        ROLLBACK;
        RETURN;
    END

    INSERT INTO Generos (Descripcion)
    SELECT Descripcion
    FROM inserted;
END;

----________________________Eliminar________________________

CREATE TRIGGER tr_EvitarEliminarGeneroConCanciones
ON Generos
INSTEAD OF DELETE
AS
BEGIN
    -- Verificar si hay canciones asociadas al género que se intenta eliminar
    IF EXISTS (
        SELECT 1
        FROM deleted d
        JOIN Canciones c ON c.CodigoGenero = d.CodigoGenero
    )
    BEGIN
        RAISERROR('No se puede eliminar el género porque está asociado a una o más canciones.', 16, 1);
        ROLLBACK;
        RETURN;
    END

    DELETE FROM Generos
    WHERE CodigoGenero IN (SELECT CodigoGenero FROM deleted);
END;


/*--------------------------------------------------------------------*/
/*------------------------- TABLA DE USUARIOS -------------------------*/
/*--------------------------------------------------------------------*/

---- ________________________Insert________________________ ----
CREATE TRIGGER trInsertUsuarios
ON Usuarios
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
    SELECT 
        GETDATE(),
        Nombre,
        'INSERT',
        'Usuarios',
        ID,
        'Nuevo usuario insertado',
        Perfil
    FROM inserted;
END

------ ________________________Update________________________ ----
CREATE TRIGGER tr_Usuarios_Update
ON Usuarios
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
    SELECT 
        GETDATE(),
        i.Nombre,
        'UPDATE',
        'Usuarios',
        i.ID,
        'Usuario actualizado',
        i.Perfil
    FROM inserted i
    JOIN deleted d ON i.ID = d.ID;
END


------ ________________________Delete________________________ ----
CREATE TRIGGER tr_Usuarios_Delete
ON Usuarios
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
    SELECT 
        GETDATE(),
        Nombre,
        'DELETE',
        'Usuarios',
        ID,
        'Usuario eliminado',
        Perfil
    FROM deleted;
END


/*--------------------------------------------------------------------*/
/*------------------------- TABLA DE VENTAS -------------------------*/
/*--------------------------------------------------------------------*/


------ ________________________Insert________________________ ----
CREATE TRIGGER trInsertVenta
ON Ventas
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
    SELECT 
        GETDATE(),
        u.ID,
        'INSERT',
        'Ventas',
        i.NumeroFactura,
        'Nueva venta registrada',
        'Sistema de cobros'
    FROM inserted i
	INNER JOIN Usuarios u ON u.ID = i.IDUsuario;
END

----****************************** FIN TRIGGER ******************************
