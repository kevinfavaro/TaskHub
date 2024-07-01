USE master;
GO;
CREATE DATABASE TaskHub;
GO;
USE TaskHub;
CREATE TABLE Usuarios (
    UsuarioId INT PRIMARY KEY,
    Funcao VARCHAR(50) NOT NULL,
	Ativo BIT NOT NULL DEFAULT(1)
);
INSERT INTO Usuarios (UsuarioId, Funcao)
VALUES (1, 'DEV');
INSERT INTO Usuarios (UsuarioId, Funcao)
VALUES (2, 'PO');
INSERT INTO Usuarios (UsuarioId, Funcao)
VALUES (3, 'Gerente');
GO;