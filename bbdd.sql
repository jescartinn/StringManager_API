-- Crear la base de datos
CREATE DATABASE StringManagerDb;
GO

-- Usar la base de datos creada
USE StringManagerDb;
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

-- Crear datos de ejemplo para pruebas

-- Insertar jugadores de ejemplo
INSERT INTO Players (Name, LastName, CountryCode)
VALUES 
('Rafael', 'Nadal', 'ESP'),
('Novak', 'Djokovic', 'SRB'),
('Carlos', 'Alcaraz', 'ESP'),
('Iga', 'Swiatek', 'POL'),
('Aryna', 'Sabalenka', 'BLR');
GO

-- Insertar raquetas de ejemplo
INSERT INTO Racquets (PlayerId, Brand, Model, SerialNumber, HeadSize, Notes)
VALUES 
(1, 'Babolat', 'Pure Aero Rafa', 'RN2023-001', 100, 'Raqueta principal'),
(1, 'Babolat', 'Pure Aero Rafa', 'RN2023-002', 100, 'Raqueta de respaldo 1'),
(1, 'Babolat', 'Pure Aero Rafa', 'RN2023-003', 100, 'Raqueta de respaldo 2'),
(2, 'Head', 'Speed Pro', 'ND2023-001', 100, 'Raqueta principal'),
(2, 'Head', 'Speed Pro', 'ND2023-002', 100, 'Raqueta de respaldo'),
(3, 'Babolat', 'Pure Aero', 'CA2023-001', 100, NULL),
(4, 'Tecnifibre', 'Tempo', 'IS2023-001', 98, NULL),
(5, 'Wilson', 'Blade', 'AS2023-001', 98, NULL);
GO

-- Insertar tipos de cuerdas de ejemplo
INSERT INTO StringTypes (Brand, Model, Gauge, Material, Color)
VALUES 
('Babolat', 'RPM Blast', '1.30mm', 'Polyester', 'Black'),
('Luxilon', 'ALU Power', '1.25mm', 'Polyester', 'Silver'),
('Tecnifibre', 'Pro Red Code', '1.25mm', 'Polyester', 'Red'),
('Wilson', 'Natural Gut', '1.30mm', 'Natural Gut', 'Natural'),
('Solinco', 'Hyper-G', '1.25mm', 'Polyester', 'Green'),
('Head', 'Hawk', '1.25mm', 'Polyester', 'Grey');
GO

-- Insertar encordadores de ejemplo
INSERT INTO Stringers (Name, LastName, Email, PhoneNumber)
VALUES 
('Juan', 'Pérez', 'juan.perez@stringteam.com', '+1234567890'),
('María', 'González', 'maria.gonzalez@stringteam.com', '+1234567891'),
('Carlos', 'Rodríguez', 'carlos.rodriguez@stringteam.com', '+1234567892');
GO

-- Insertar torneos de ejemplo
INSERT INTO Tournaments (Name, StartDate, EndDate, Location, Category)
VALUES 
('Australian Open 2025', '2025-01-13', '2025-01-26', 'Melbourne, Australia', 'Grand Slam'),
('Roland Garros 2025', '2025-05-19', '2025-06-01', 'Paris, France', 'Grand Slam'),
('Wimbledon 2025', '2025-06-30', '2025-07-13', 'London, United Kingdom', 'Grand Slam'),
('US Open 2025', '2025-08-25', '2025-09-07', 'New York, United States', 'Grand Slam'),
('Madrid Open 2025', '2025-04-28', '2025-05-05', 'Madrid, Spain', 'ATP 1000');
GO

-- Insertar trabajos de encordado de ejemplo
INSERT INTO StringJobs (PlayerId, RacquetId, MainStringId, CrossStringId, StringerId, TournamentId, 
                       MainTension, CrossTension, IsTensionInKg, Status, Priority, CreatedAt)
VALUES 
(1, 1, 1, 4, 1, 1, 25, 23, 1, 'Completed', 1, DATEADD(DAY, -5, GETDATE())),
(1, 2, 1, 4, 1, 1, 25, 23, 1, 'Pending', 1, GETDATE()),
(2, 4, 2, NULL, 2, 1, 24, NULL, 1, 'InProgress', 1, DATEADD(HOUR, -3, GETDATE())),
(3, 6, 5, NULL, 3, 1, 25, NULL, 1, 'Pending', 2, DATEADD(HOUR, -1, GETDATE())),
(4, 7, 3, NULL, 1, 1, 24, NULL, 1, 'Pending', 3, DATEADD(HOUR, -2, GETDATE())),
(5, 8, 6, NULL, 2, 1, 23, NULL, 1, 'Completed', 2, DATEADD(DAY, -1, GETDATE()));
GO