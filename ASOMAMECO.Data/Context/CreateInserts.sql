-- Crear la base de datos
CREATE DATABASE ASOMAMECO;

USE ASOMAMECO;

-- Crear la tabla Usuarios
CREATE TABLE Usuarios (
    Id INT PRIMARY KEY,
    UsuarioName NVARCHAR(200) NOT NULL,
    Contrasenia NVARCHAR(20) NOT NULL,
    Rol NVarchar (30) NOT NULL,
    Email NVARCHAR(100) NULL
    
);

-- Crear la tabla Miembros
CREATE TABLE Miembros (
    Id INT PRIMARY KEY,
    Nombre NVARCHAR(200) NOT NULL,
    Cedula NVARCHAR(20) NOT NULL,
	Activo BIT NOT NULL,
    Estado BIT NOT NULL,
    Email NVARCHAR(100) NULL,
    Telefono NVARCHAR(20) NULL
);

-- Crear índices únicos para Miembros
CREATE UNIQUE INDEX IX_Miembros_NumeroAsociado ON Miembros(Id);


-- Crear la tabla Eventos
CREATE TABLE Eventos (
    Id INT PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Fecha DATETIME NOT NULL,
    Descripcion NVARCHAR(500) NULL,
    Activo BIT NOT NULL
);

-- Crear la tabla Asistencias
CREATE TABLE Asistencias (
    Id INT PRIMARY KEY,
    MiembroId INT NOT NULL,
    EventoId INT NOT NULL,
    FechaHoraRegistro DATETIME NOT NULL,
    CONSTRAINT FK_Asistencias_Miembros FOREIGN KEY (MiembroId) REFERENCES Miembros(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Asistencias_Eventos FOREIGN KEY (EventoId) REFERENCES Eventos(Id) ON DELETE NO ACTION
);

-- Insertar usuarios 
INSERT INTO Usuarios (Id, UsuarioName, Contrasenia, Rol, Email) 
VALUES 
(1, 'Admin', 'admin1234', 'Administrador', 'admin@ejemplo.com'),
(2, 'Delegado1', 'delegado1234', 'Delegado', 'delegado1@ejemplo.com'),
(3, 'Delegado2', 'delegado5678', 'Delegado', 'delegado2@ejemplo.com');

-- Crear índice único compuesto para evitar registros duplicados
CREATE UNIQUE INDEX IX_Asistencias_MiembroId_EventoId ON Asistencias(MiembroId, EventoId);




-- Primero exporta tu Excel a CSV
LOAD DATA INFILE 'C:/Users/chus2/Downloads/Padron Asociados.csv'
INTO TABLE Miembros
FIELDS TERMINATED BY ';'  -- El delimitador es ';'
ENCLOSED BY ''            -- No hay comillas alrededor de los campos
LINES TERMINATED BY '\n'  -- Las líneas terminan con un salto de línea
IGNORE 1 ROWS             -- Ignorar la primera fila (encabezados)
(Id, Nombre, Cedula, @Estatus1, @Estado2, Email, Telefono)
SET Activo = CASE WHEN @Estatus1 = 'Activo' THEN 1 ELSE 0 END,
    Estado = CASE WHEN @Estado2 = 'Confirmado' THEN 1 ELSE 0 END;

-- Verificar la creación exitosa
SELECT 'Base de datos ASOMAMECO creada exitosamente.' AS Mensaje;