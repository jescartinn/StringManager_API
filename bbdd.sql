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

-- Insertar jugadores de ejemplo
INSERT INTO Players (Name, LastName, CountryCode)
VALUES 
('Rafael', 'Nadal', 'ESP'),
('Novak', 'Djokovic', 'SRB'),
('Carlos', 'Alcaraz', 'ESP'),
('Iga', 'Swiatek', 'POL'),
('Aryna', 'Sabalenka', 'BLR');
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

-- Insertar raquetas con seriales dinámicos basados en el año actual
INSERT INTO Racquets (PlayerId, Brand, Model, SerialNumber, HeadSize, Notes)
VALUES 
(1, 'Babolat', 'Pure Aero Rafa', 'RN' + CAST(YEAR(GETDATE()) AS VARCHAR) + '-001', 100, 'Raqueta principal'),
(1, 'Babolat', 'Pure Aero Rafa', 'RN' + CAST(YEAR(GETDATE()) AS VARCHAR) + '-002', 100, 'Raqueta de respaldo 1'),
(1, 'Babolat', 'Pure Aero Rafa', 'RN' + CAST(YEAR(GETDATE()) AS VARCHAR) + '-003', 100, 'Raqueta de respaldo 2'),
(2, 'Head', 'Speed Pro', 'ND' + CAST(YEAR(GETDATE()) AS VARCHAR) + '-001', 100, 'Raqueta principal'),
(2, 'Head', 'Speed Pro', 'ND' + CAST(YEAR(GETDATE()) AS VARCHAR) + '-002', 100, 'Raqueta de respaldo'),
(3, 'Babolat', 'Pure Aero', 'CA' + CAST(YEAR(GETDATE()) AS VARCHAR) + '-001', 100, NULL),
(4, 'Tecnifibre', 'Tempo', 'IS' + CAST(YEAR(GETDATE()) AS VARCHAR) + '-001', 98, NULL),
(5, 'Wilson', 'Blade', 'AS' + CAST(YEAR(GETDATE()) AS VARCHAR) + '-001', 98, NULL);
GO

-- Insertar torneos de ejemplo con fechas dinámicas relativas al año actual
-- Australian Open (mediados de enero)
INSERT INTO Tournaments (Name, StartDate, EndDate, Location, Category)
VALUES 
('Australian Open ' + CAST(YEAR(GETDATE()) AS VARCHAR), 
 DATEFROMPARTS(YEAR(GETDATE()), 1, 13), 
 DATEFROMPARTS(YEAR(GETDATE()), 1, 26), 
 'Melbourne, Australia', 'Grand Slam');

-- Miami Open (finales de marzo)
INSERT INTO Tournaments (Name, StartDate, EndDate, Location, Category)
VALUES 
('Miami Open ' + CAST(YEAR(GETDATE()) AS VARCHAR), 
 DATEFROMPARTS(YEAR(GETDATE()), 3, 17), 
 DATEFROMPARTS(YEAR(GETDATE()), 3, 30), 
 'Miami, United States', 'ATP 1000');

-- Madrid Open (principios de mayo)
INSERT INTO Tournaments (Name, StartDate, EndDate, Location, Category)
VALUES 
('Madrid Open ' + CAST(YEAR(GETDATE()) AS VARCHAR), 
 DATEFROMPARTS(YEAR(GETDATE()), 4, 28), 
 DATEFROMPARTS(YEAR(GETDATE()), 5, 11), 
 'Madrid, Spain', 'ATP 1000');

-- Roland Garros (finales de mayo)
INSERT INTO Tournaments (Name, StartDate, EndDate, Location, Category)
VALUES 
('Roland Garros ' + CAST(YEAR(GETDATE()) AS VARCHAR), 
 DATEFROMPARTS(YEAR(GETDATE()), 5, 25), 
 DATEFROMPARTS(YEAR(GETDATE()), 6, 8), 
 'Paris, France', 'Grand Slam');

-- Wimbledon (principios de julio)
INSERT INTO Tournaments (Name, StartDate, EndDate, Location, Category)
VALUES 
('Wimbledon ' + CAST(YEAR(GETDATE()) AS VARCHAR), 
 DATEFROMPARTS(YEAR(GETDATE()), 6, 30), 
 DATEFROMPARTS(YEAR(GETDATE()), 7, 13), 
 'London, United Kingdom', 'Grand Slam');

-- Torneo actual (que incluye la fecha actual)
INSERT INTO Tournaments (Name, StartDate, EndDate, Location, Category)
VALUES 
('Current Tournament ' + CAST(YEAR(GETDATE()) AS VARCHAR), 
 DATEADD(DAY, -6, GETDATE()), 
 DATEADD(DAY, 4, GETDATE()), 
 'Current Location, Country', 'ATP 1000');
GO

-- Crear una tabla temporal para almacenar el ID del torneo actual
CREATE TABLE #CurrentTournament (Id INT);

-- Insertar el ID del torneo actual en la tabla temporal
INSERT INTO #CurrentTournament (Id)
SELECT Id 
FROM Tournaments 
WHERE GETDATE() BETWEEN StartDate AND EndDate;

-- Si no hay torneo actual, usar el último torneo
IF (SELECT COUNT(*) FROM #CurrentTournament) = 0
BEGIN
    DELETE FROM #CurrentTournament;
    INSERT INTO #CurrentTournament (Id)
    SELECT TOP 1 Id 
    FROM Tournaments 
    ORDER BY StartDate DESC;
END

-- Insertar trabajos de encordado completados en días pasados
INSERT INTO StringJobs (PlayerId, RacquetId, MainStringId, CrossStringId, StringerId, TournamentId, 
                       MainTension, CrossTension, IsTensionInKg, Status, Priority, CreatedAt, CompletedAt)
SELECT 
    1, 1, 1, 4, 1, Id, 25, 23, 1, 'Completed', 1, 
    DATEADD(DAY, -5, GETDATE()), DATEADD(DAY, -5, GETDATE())
FROM #CurrentTournament;

INSERT INTO StringJobs (PlayerId, RacquetId, MainStringId, CrossStringId, StringerId, TournamentId, 
                       MainTension, CrossTension, IsTensionInKg, Status, Priority, CreatedAt, CompletedAt)
SELECT 
    5, 8, 6, NULL, 2, Id, 23, NULL, 1, 'Completed', 2, 
    DATEADD(DAY, -1, GETDATE()), DATEADD(DAY, -1, GETDATE())
FROM #CurrentTournament;

-- Insertar trabajos pendientes o en progreso
INSERT INTO StringJobs (PlayerId, RacquetId, MainStringId, CrossStringId, StringerId, TournamentId, 
                       MainTension, CrossTension, IsTensionInKg, Status, Priority, CreatedAt)
SELECT 
    1, 2, 1, 4, 1, Id, 25, 23, 1, 'Pending', 1, GETDATE()
FROM #CurrentTournament;

INSERT INTO StringJobs (PlayerId, RacquetId, MainStringId, CrossStringId, StringerId, TournamentId, 
                       MainTension, CrossTension, IsTensionInKg, Status, Priority, CreatedAt)
SELECT 
    2, 4, 2, NULL, 2, Id, 24, NULL, 1, 'InProgress', 1, 
    DATEADD(HOUR, -3, GETDATE())
FROM #CurrentTournament;

INSERT INTO StringJobs (PlayerId, RacquetId, MainStringId, CrossStringId, StringerId, TournamentId, 
                       MainTension, CrossTension, IsTensionInKg, Status, Priority, CreatedAt)
SELECT 
    3, 6, 5, NULL, 3, Id, 25, NULL, 1, 'Pending', 2, 
    DATEADD(HOUR, -1, GETDATE())
FROM #CurrentTournament;

INSERT INTO StringJobs (PlayerId, RacquetId, MainStringId, CrossStringId, StringerId, TournamentId, 
                       MainTension, CrossTension, IsTensionInKg, Status, Priority, CreatedAt)
SELECT 
    4, 7, 3, NULL, 1, Id, 24, NULL, 1, 'Pending', 3, 
    DATEADD(HOUR, -2, GETDATE())
FROM #CurrentTournament;

-- Insertar trabajos de encordado completados hoy (para las estadísticas del dashboard)
INSERT INTO StringJobs (PlayerId, RacquetId, MainStringId, CrossStringId, StringerId, TournamentId, 
                       MainTension, CrossTension, IsTensionInKg, Status, Priority, CreatedAt, CompletedAt)
SELECT 
    1, 3, 1, 4, 1, Id, 25, 23, 1, 'Completed', 1, 
    DATEADD(HOUR, -5, GETDATE()), DATEADD(HOUR, -4, GETDATE())
FROM #CurrentTournament;

INSERT INTO StringJobs (PlayerId, RacquetId, MainStringId, CrossStringId, StringerId, TournamentId, 
                       MainTension, CrossTension, IsTensionInKg, Status, Priority, CreatedAt, CompletedAt)
SELECT 
    2, 5, 2, NULL, 2, Id, 24, NULL, 1, 'Completed', 1, 
    DATEADD(HOUR, -4, GETDATE()), DATEADD(HOUR, -3, GETDATE())
FROM #CurrentTournament;

INSERT INTO StringJobs (PlayerId, RacquetId, MainStringId, CrossStringId, StringerId, TournamentId, 
                       MainTension, CrossTension, IsTensionInKg, Status, Priority, CreatedAt, CompletedAt)
SELECT 
    3, 6, 5, NULL, 3, Id, 25, NULL, 1, 'Completed', 2, 
    DATEADD(HOUR, -3, GETDATE()), DATEADD(HOUR, -2, GETDATE())
FROM #CurrentTournament;

INSERT INTO StringJobs (PlayerId, RacquetId, MainStringId, CrossStringId, StringerId, TournamentId, 
                       MainTension, CrossTension, IsTensionInKg, Status, Priority, CreatedAt, CompletedAt)
SELECT 
    4, 7, 3, NULL, 1, Id, 24, NULL, 1, 'Completed', 3, 
    DATEADD(HOUR, -2, GETDATE()), DATEADD(HOUR, -1, GETDATE())
FROM #CurrentTournament;

INSERT INTO StringJobs (PlayerId, RacquetId, MainStringId, CrossStringId, StringerId, TournamentId, 
                       MainTension, CrossTension, IsTensionInKg, Status, Priority, CreatedAt, CompletedAt)
SELECT 
    5, 8, 6, NULL, 2, Id, 23, NULL, 1, 'Completed', 2, 
    DATEADD(MINUTE, -90, GETDATE()), DATEADD(MINUTE, -30, GETDATE())
FROM #CurrentTournament;

INSERT INTO StringJobs (PlayerId, RacquetId, MainStringId, CrossStringId, StringerId, TournamentId, 
                       MainTension, CrossTension, IsTensionInKg, Status, Priority, CreatedAt, CompletedAt)
SELECT 
    1, 1, 1, 4, 3, Id, 25, 23, 1, 'Completed', 1, 
    DATEADD(MINUTE, -75, GETDATE()), DATEADD(MINUTE, -15, GETDATE())
FROM #CurrentTournament;

INSERT INTO StringJobs (PlayerId, RacquetId, MainStringId, CrossStringId, StringerId, TournamentId, 
                       MainTension, CrossTension, IsTensionInKg, Status, Priority, CreatedAt, CompletedAt)
SELECT 
    2, 4, 2, NULL, 1, Id, 24, NULL, 1, 'Completed', 1, 
    DATEADD(MINUTE, -45, GETDATE()), DATEADD(MINUTE, -10, GETDATE())
FROM #CurrentTournament;

INSERT INTO StringJobs (PlayerId, RacquetId, MainStringId, CrossStringId, StringerId, TournamentId, 
                       MainTension, CrossTension, IsTensionInKg, Status, Priority, CreatedAt, CompletedAt)
SELECT 
    3, 6, 5, NULL, 2, Id, 25, NULL, 1, 'Completed', 2, 
    DATEADD(MINUTE, -30, GETDATE()), DATEADD(MINUTE, -5, GETDATE())
FROM #CurrentTournament;

-- Eliminar la tabla temporal
DROP TABLE #CurrentTournament;
GO

-- Imprimir resumen
PRINT 'Base de datos StringManagerDb creada con éxito.';
PRINT 'Tablas creadas: Users, Players, Racquets, StringTypes, Stringers, Tournaments, StringJobs';
PRINT 'Datos de muestra insertados correctamente con fechas dinámicas basadas en la fecha actual.';
GO