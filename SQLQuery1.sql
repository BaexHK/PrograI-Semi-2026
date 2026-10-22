CREATE DATABASE PrograISemi2026;
GO

USE PrograISemi2026;
GO

IF OBJECT_ID('usuarios_usss087526', 'U') IS NOT NULL
    DROP TABLE usuarios_usss087526;
GO

CREATE TABLE usuarios_usss087526 (
    idUsuario INT IDENTITY(1,1) PRIMARY KEY,
    usuario VARCHAR(35) NOT NULL,
    clave VARCHAR(35) NOT NULL,
    nombre VARCHAR(65) NOT NULL,
    dirección VARCHAR(85) NOT NULL,
    teléfono VARCHAR(9) NOT NULL
);
GO

INSERT INTO usuarios_usss087526 (usuario, clave, nombre, dirección, teléfono) VALUES 
('jfuentes', 'Admin123*', 'Brandon Jazirth Fuentes', 'Usulután, El Salvador', '7123-4567'),
('vmerino', 'Clave2026#', 'Valeria Merino', 'San Miguel, El Salvador', '7987-6543'),
('user_test', 'Prueba987$', 'Usuario de Prueba', 'Usulután, El Salvador', '7555-1234');
GO