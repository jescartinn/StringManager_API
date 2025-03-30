-- Crear la base de datos
CREATE DATABASE StringManagerDb;
GO

-- Usar la base de datos creada
USE StringManagerDb;
GO

-- Crear tabla de Usuarios
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(100) NOT NULL,
    Role NVARCHAR(20) NOT NULL DEFAULT 'User',
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    LastLoginAt DATETIME NULL,
    IsActive BIT NOT NULL DEFAULT 1
);
GO

-- Crear índices únicos para usuarios
CREATE UNIQUE INDEX IX_Users_Username ON Users (Username);
CREATE UNIQUE INDEX IX_Users_Email ON Users (Email);
GO

-- Crear tabla de Jugadores
CREATE TABLE Players (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    CountryCode NVARCHAR(3) NULL
);
GO

-- Crear tabla de Raquetas
CREATE TABLE Racquets (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PlayerId INT NOT NULL,
    Brand NVARCHAR(100) NOT NULL,
    Model NVARCHAR(100) NOT NULL,
    SerialNumber NVARCHAR(50) NULL,
    HeadSize FLOAT NULL,
    Notes NVARCHAR(500) NULL,
    CONSTRAINT FK_Racquets_Players FOREIGN KEY (PlayerId) 
        REFERENCES Players (Id) ON DELETE NO ACTION
);
GO

-- Crear tabla de Tipos de Cuerdas
CREATE TABLE StringTypes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Brand NVARCHAR(100) NOT NULL,
    Model NVARCHAR(100) NOT NULL,
    Gauge NVARCHAR(20) NULL,
    Material NVARCHAR(50) NULL,
    Color NVARCHAR(50) NULL
);
GO

-- Crear tabla de Encordadores
CREATE TABLE Stringers (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NULL,
    PhoneNumber NVARCHAR(20) NULL
);
GO

-- Crear tabla de Torneos
CREATE TABLE Tournaments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(200) NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    Location NVARCHAR(200) NULL,
    Category NVARCHAR(50) NULL
);
GO

-- Crear tabla de Trabajos de Encordado
CREATE TABLE StringJobs (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PlayerId INT NOT NULL,
    RacquetId INT NOT NULL,
    MainStringId INT NULL,
    CrossStringId INT NULL,
    StringerId INT NULL,
    TournamentId INT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CompletedAt DATETIME NULL,
    MainTension FLOAT NOT NULL,
    CrossTension FLOAT NULL,
    IsTensionInKg BIT NOT NULL DEFAULT 1,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending',
    Notes NVARCHAR(1000) NULL,
    Priority INT NULL,
    CONSTRAINT FK_StringJobs_Players FOREIGN KEY (PlayerId) 
        REFERENCES Players (Id) ON DELETE NO ACTION,
    CONSTRAINT FK_StringJobs_Racquets FOREIGN KEY (RacquetId) 
        REFERENCES Racquets (Id) ON DELETE NO ACTION,
    CONSTRAINT FK_StringJobs_MainString FOREIGN KEY (MainStringId) 
        REFERENCES StringTypes (Id) ON DELETE NO ACTION,
    CONSTRAINT FK_StringJobs_CrossString FOREIGN KEY (CrossStringId) 
        REFERENCES StringTypes (Id) ON DELETE NO ACTION,
    CONSTRAINT FK_StringJobs_Stringers FOREIGN KEY (StringerId) 
        REFERENCES Stringers (Id) ON DELETE NO ACTION,
    CONSTRAINT FK_StringJobs_Tournaments FOREIGN KEY (TournamentId) 
        REFERENCES Tournaments (Id) ON DELETE NO ACTION
);
GO

-- Crear índices para mejorar el rendimiento
CREATE INDEX IX_Racquets_PlayerId ON Racquets (PlayerId);
CREATE INDEX IX_StringJobs_PlayerId ON StringJobs (PlayerId);
CREATE INDEX IX_StringJobs_RacquetId ON StringJobs (RacquetId);
CREATE INDEX IX_StringJobs_MainStringId ON StringJobs (MainStringId);
CREATE INDEX IX_StringJobs_CrossStringId ON StringJobs (CrossStringId);
CREATE INDEX IX_StringJobs_StringerId ON StringJobs (StringerId);
CREATE INDEX IX_StringJobs_TournamentId ON StringJobs (TournamentId);
CREATE INDEX IX_StringJobs_Status ON StringJobs (Status);
CREATE INDEX IX_StringJobs_CreatedAt ON StringJobs (CreatedAt);
CREATE INDEX IX_Tournaments_StartDate_EndDate ON Tournaments (StartDate, EndDate);
GO

-- Insertar usuarios de ejemplo
INSERT INTO Users (Username, Email, PasswordHash, Role, CreatedAt, IsActive)
VALUES 
    ('admin', 'admin@stringmanager.com', '$2a$11$tFQyiuaLLMBooDDbC6dfzOP8nd3JhSkq8k/EUJiX6bcjOGag5WSBu', 'Admin', GETDATE(), 1),
    ('stringer', 'stringer@stringmanager.com', '$2a$11$EMrMnmQ8/Dpmcrr8foXXMudmKCeicoYBZjdY1ouelnSARvQXxk1cG', 'Stringer', GETDATE(), 1),
    ('user', 'user@stringmanager.com', '$2a$11$3RrCcxVSnKSYCWAJqFyGfegoogVG0AuUmUfFPVLxaO5OEdKAlES1S', 'User', GETDATE(), 1);
GO