USE VentaMusical
go
/*------------------------ GENEROS ------------------------*/

INSERT INTO Generos (Descripcion) VALUES 
('Metal')
go

INSERT INTO Generos (Descripcion) VALUES 
('Pop')
go

INSERT INTO Generos (Descripcion) VALUES 
('Electronica')
go

INSERT INTO Generos (Descripcion) VALUES 
('House')
go

INSERT INTO Generos (Descripcion) VALUES 
('Acústico')
go

/*------------------------ ARTISTAS ------------------------*/

INSERT INTO Artistas (NombreArtistico, FechaNacimiento, NombreReal, Nacionalidad, Foto, LinkBiografia) VALUES
('Powerwolf', '2003-02-16', 'Powerwolf', 'Alemania', 'https://cdn-images.dzcdn.net/images/artist/3f1d3a2d3853a6743481ea58bde88055/1900x1900-000000-80-0-0.jpg', 'https://es.wikipedia.org/wiki/Powerwolf')
go

INSERT INTO Artistas (NombreArtistico, FechaNacimiento, NombreReal, Nacionalidad, Foto, LinkBiografia) VALUES
('Trivium', '1999-02-16', 'Trivium', 'USA', 'https://i.scdn.co/image/ab6761610000e5ebe3d54be763612bdcf50e56eb', 'https://en.wikipedia.org/wiki/Trivium_(band)')
go


/*------------------------ ALBUMES ------------------------*/

INSERT INTO Albumes (CodigoArtista, NombreAlbum, AnoLanzamiento, Imagen) VALUES
(1, 'Blessed & Possessed', 2015, 'https://th.bing.com/th/id/OSK.wp4FjpdLgbcGl58FdDvwSy_4mOPyRREgdxZu3R7Rah4?w=152&h=152&c=7&o=6&dpr=1.3&pid=SANGAM')
go

INSERT INTO Albumes (CodigoArtista, NombreAlbum, AnoLanzamiento, Imagen) VALUES
(2, 'In the court of the dragon', 2021, 'https://upload.wikimedia.org/wikipedia/en/f/ff/TriviumIntheCourtoftheDragon.jpg')
go


/*------------------------ CANCIONES ------------------------*/

INSERT INTO Canciones (CodigoGenero, CodigoAlbum, NombreCancion, LinkVideo, Precio, CantidadDisponible) VALUES
(1, 1, 'Armata Strigoi', 'https://open.spotify.com/embed/track/0Dvud5QKnPMk8yqGcGAuyE', 12000, 100)
go

INSERT INTO Canciones (CodigoGenero, CodigoAlbum, NombreCancion, LinkVideo, Precio, CantidadDisponible) VALUES
(1, 1, 'Blessed & Possessed', 'https://open.spotify.com/embed/track/7557f74AkpP5Lj4u8R9n3N?utm_source=generator', 12000, 2)
go

INSERT INTO Canciones (CodigoGenero, CodigoAlbum, NombreCancion, LinkVideo, Precio, CantidadDisponible) VALUES
(1, 1, 'Sainted by the storm', 'https://www.youtube.com/watch?v=1SIhedUuqjE', 12000, 2)
go

INSERT INTO Canciones (CodigoGenero, CodigoAlbum, NombreCancion, LinkVideo, Precio, CantidadDisponible) VALUES
(1, 2, 'Shadow of the abbatoir', 'https://www.youtube.com/embed/Hc_td_w7dvc?si=jd9ACXZWZe4yd6K2', 12000, 2)
go

/*------------------------ USUARIOS ------------------------*/

INSERT INTO Usuarios(NumeroIdentificacion, Nombre, Apellido, Genero, CorreoElectronico, TipoTarjeta,
    DineroDisponible, Contrasena, Perfil, NumeroTarjeta) VALUES
('123456789', 'Juan', 'Pérez', 'Masculino', 'admin@example.com', 'Visa', 0, 'clave123', 'Admin', '0000-0000-0000-0000');
go

INSERT INTO Usuarios(NumeroIdentificacion, Nombre, Apellido, Genero, CorreoElectronico, TipoTarjeta,
    DineroDisponible, Contrasena, Perfil, NumeroTarjeta) VALUES
('987654321', 'Ana', 'Ramírez', 'Femenino', 'usuario@example.com', 'MasterCard', 0, 'ana456', 'Usuario', '0000-0000-0000-0000');
go

INSERT INTO Usuarios(NumeroIdentificacion, Nombre, Apellido, Genero, CorreoElectronico, TipoTarjeta,
    DineroDisponible, Contrasena, Perfil, NumeroTarjeta) VALUES
('456123789', 'Carlos', 'Soto', 'Masculino', 'admin2@example.com', 'Visa', 0, 'carlos789', 'Admin', '0000-0000-0000-0000');
go

INSERT INTO Usuarios(NumeroIdentificacion, Nombre, Apellido, Genero, CorreoElectronico, TipoTarjeta,
    DineroDisponible, Contrasena, Perfil, NumeroTarjeta) VALUES
('321654987', 'Lucía', 'Morales', 'Femenino', 'contador@example.com', 'Visa', 0, 'lucia321', 'Contabilidad', '0000-0000-0000-0000');
go
