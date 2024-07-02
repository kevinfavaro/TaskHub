USE master
GO
CREATE DATABASE TaskHub
GO
USE TaskHub
GO
CREATE TABLE Usuarios (
    UsuarioId INT PRIMARY KEY IDENTITY(1,1),
    Funcao VARCHAR(50) NOT NULL,
	Ativo BIT NOT NULL DEFAULT(1)
)
INSERT INTO Usuarios (Funcao)
VALUES ('DEV');
INSERT INTO Usuarios (Funcao)
VALUES ('PO');
INSERT INTO Usuarios (Funcao)
VALUES ('Gerente');
GO
CREATE TABLE Projetos (
    ProjetoId INT PRIMARY KEY IDENTITY(1,1),
    Nome VARCHAR(100) NOT NULL,
    Descricao TEXT,
    UsuarioId INT NOT NULL,
	Ativo Bit NOT NULL DEFAULT(1),
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(UsuarioId)
)
GO
CREATE TABLE Tarefas (
    TarefaId INT PRIMARY KEY IDENTITY(1,1),
    Titulo VARCHAR(100) NOT NULL,
    Descricao TEXT,
    DataVencimento DATE,
    DataConcluido DATE,
    Status TINYINT NOT NULL,
    Prioridade TINYINT,
    ProjetoId INT NOT NULL,
	UsuarioId INT NOT NULL,
	Ativa Bit NOT NULL DEFAULT(1),
    FOREIGN KEY (ProjetoId) REFERENCES Projetos(ProjetoId),
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(UsuarioId)
)
GO
CREATE TABLE Historico (
    HistoricoId INT PRIMARY KEY IDENTITY(1,1),
    TarefaId INT NOT NULL,
    Detalhes TEXT,
    DataAtualizacao DATETIME,
    UsuarioId INT,
    FOREIGN KEY (TarefaId) REFERENCES Tarefas(TarefaId),
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(UsuarioId)
)
GO
CREATE TABLE Comentarios (
    ComentarioId INT PRIMARY KEY IDENTITY(1,1),
    TarefaId INT NOT NULL,
    Texto TEXT,
    DataComentario DATETIME NOT NULL DEFAULT(GETDATE()),
    UsuarioId INT,
	Ativo Bit NOT NULL DEFAULT(1),
    FOREIGN KEY (TarefaId) REFERENCES Tarefas(TarefaId),
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(UsuarioId)
)
GO