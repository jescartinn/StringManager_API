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

-- Insert Top 100 ATP Players (Men's)
INSERT INTO Players (Name, LastName, CountryCode)
VALUES
    ('Jannik', 'Sinner', 'ITA'),
    ('Novak', 'Djokovic', 'SRB'),
    ('Carlos', 'Alcaraz', 'ESP'),
    ('Alexander', 'Zverev', 'GER'),
    ('Daniil', 'Medvedev', 'RUS'),
    ('Andrey', 'Rublev', 'RUS'),
    ('Casper', 'Ruud', 'NOR'),
    ('Hubert', 'Hurkacz', 'POL'),
    ('Grigor', 'Dimitrov', 'BUL'),
    ('Alex', 'de Minaur', 'AUS'),
    ('Stefanos', 'Tsitsipas', 'GRE'),
    ('Taylor', 'Fritz', 'USA'),
    ('Tommy', 'Paul', 'USA'),
    ('Ben', 'Shelton', 'USA'),
    ('Holger', 'Rune', 'DEN'),
    ('Frances', 'Tiafoe', 'USA'),
    ('Karen', 'Khachanov', 'RUS'),
    ('Ugo', 'Humbert', 'FRA'),
    ('Félix', 'Auger-Aliassime', 'CAN'),
    ('Nicolas', 'Jarry', 'CHI'),
    ('Sebastian', 'Korda', 'USA'),
    ('Sebastian', 'Baez', 'ARG'),
    ('Adrian', 'Mannarino', 'FRA'),
    ('Lorenzo', 'Musetti', 'ITA'),
    ('Alexander', 'Bublik', 'KAZ'),
    ('Jan-Lennard', 'Struff', 'GER'),
    ('Tallon', 'Griekspoor', 'NED'),
    ('Jack', 'Draper', 'GBR'),
    ('Francisco', 'Cerundolo', 'ARG'),
    ('Alejandro', 'Tabilo', 'CHI'),
    ('Mariano', 'Navone', 'ARG'),
    ('Arthur', 'Fils', 'FRA'),
    ('Alexei', 'Popyrin', 'AUS'),
    ('Felix', 'Auger-Aliassime', 'CAN'),
    ('Flavio', 'Cobolli', 'ITA'),
    ('Tomas Martin', 'Etcheverry', 'ARG'),
    ('Nuno', 'Borges', 'POR'),
    ('Cameron', 'Norrie', 'GBR'),
    ('Alejandro', 'Davidovich Fokina', 'ESP'),
    ('Roman', 'Safiullin', 'RUS'),
    ('Pavel', 'Kotov', 'RUS'),
    ('Daniel Elahi', 'Galan', 'COL'),
    ('Arthur', 'Rinderknech', 'FRA'),
    ('Lorenzo', 'Sonego', 'ITA'),
    ('Matteo', 'Arnaldi', 'ITA'),
    ('Gael', 'Monfils', 'FRA'),
    ('Laslo', 'Djere', 'SRB'),
    ('Fabian', 'Marozsan', 'HUN'),
    ('Zhizhen', 'Zhang', 'CHN'),
    ('Jakub', 'Mensik', 'CZE'),
    ('Jiri', 'Lehecka', 'CZE'),
    ('Brandon', 'Nakashima', 'USA'),
    ('Jordan', 'Thompson', 'AUS'),
    ('Kei', 'Nishikori', 'JPN'),
    ('Luciano', 'Darderi', 'ITA'),
    ('Marcos', 'Giron', 'USA'),
    ('Max', 'Purcell', 'AUS'),
    ('Matteo', 'Berrettini', 'ITA'),
    ('Dominik', 'Koepfer', 'GER'),
    ('Giovanni', 'Mpetshi Perricard', 'FRA'),
    ('Alexandre', 'Muller', 'FRA'),
    ('Reilly', 'Opelka', 'USA'),
    ('Yoshihito', 'Nishioka', 'JPN'),
    ('Roberto', 'Carballes Baena', 'ESP'),
    ('Botic', 'Van de Zandschulp', 'NED'),
    ('David', 'Goffin', 'BEL'),
    ('Thiago', 'Monteiro', 'BRA'),
    ('Marton', 'Fucsovics', 'HUN'),
    ('Roberto', 'Bautista Agut', 'ESP'),
    ('Emil', 'Ruusuvuori', 'FIN'),
    ('Taro', 'Daniel', 'JPN'),
    ('Jaume', 'Munar', 'ESP'),
    ('Miomir', 'Kecmanovic', 'SRB'),
    ('Zizou', 'Bergs', 'BEL'),
    ('Christopher', 'Eubanks', 'USA'),
    ('Yannick', 'Hanfmann', 'GER'),
    ('Luca', 'Van Assche', 'FRA'),
    ('Stan', 'Wawrinka', 'SUI'),
    ('Gabriel', 'Diallo', 'CAN'),
    ('Alexander', 'Shevchenko', 'KAZ'),
    ('Tomas', 'Machac', 'CZE'),
    ('Daniel', 'Altmaier', 'GER'),
    ('Marin', 'Cilic', 'CRO'),
    ('Rinky', 'Hijikata', 'AUS'),
    ('Thanasi', 'Kokkinakis', 'AUS'),
    ('Bernabe', 'Zapata Miralles', 'ESP'),
    ('J.J.', 'Wolf', 'USA'),
    ('Francisco', 'Comesana', 'ARG'),
    ('Pedro', 'Martinez', 'ESP'),
    ('Federico', 'Coria', 'ARG'),
    ('Albert Ramos', 'Viñolas', 'ESP'),
    ('Yunchaokete', 'Bu', 'CHN'),
    ('Thiago', 'Seyboth Wild', 'BRA'),
    ('Denis', 'Shapovalov', 'CAN'),
    ('Hamad', 'Medjedovic', 'SRB'),
    ('James', 'Duckworth', 'AUS'),
    ('Dusan', 'Lajovic', 'SRB'),
    ('Facundo', 'Diaz Acosta', 'ARG');
GO

-- Insert Top 100 WTA Players (Women's)
INSERT INTO Players (Name, LastName, CountryCode)
VALUES
    ('Aryna', 'Sabalenka', 'BLR'),
    ('Iga', 'Swiatek', 'POL'),
    ('Coco', 'Gauff', 'USA'),
    ('Elena', 'Rybakina', 'KAZ'),
    ('Jasmine', 'Paolini', 'ITA'),
    ('Jessica', 'Pegula', 'USA'),
    ('Qinwen', 'Zheng', 'CHN'),
    ('Marketa', 'Vondrousova', 'CZE'),
    ('Maria', 'Sakkari', 'GRE'),
    ('Ons', 'Jabeur', 'TUN'),
    ('Danielle', 'Collins', 'USA'),
    ('Daria', 'Kasatkina', 'RUS'),
    ('Beatriz', 'Haddad Maia', 'BRA'),
    ('Jelena', 'Ostapenko', 'LAT'),
    ('Madison', 'Keys', 'USA'),
    ('Liudmila', 'Samsonova', 'RUS'),
    ('Emma', 'Navarro', 'USA'),
    ('Elina', 'Svitolina', 'UKR'),
    ('Marta', 'Kostyuk', 'UKR'),
    ('Mirra', 'Andreeva', 'RUS'),
    ('Victoria', 'Azarenka', 'BLR'),
    ('Caroline', 'Garcia', 'FRA'),
    ('Ekaterina', 'Alexandrova', 'RUS'),
    ('Diana', 'Shnaider', 'RUS'),
    ('Anastasia', 'Pavlyuchenkova', 'RUS'),
    ('Anna', 'Kalinskaya', 'RUS'),
    ('Barbora', 'Krejcikova', 'CZE'),
    ('Leylah', 'Fernandez', 'CAN'),
    ('Linda', 'Noskova', 'CZE'),
    ('Dayana', 'Yastremska', 'UKR'),
    ('Paula', 'Badosa', 'ESP'),
    ('Katie', 'Boulter', 'GBR'),
    ('Elise', 'Mertens', 'BEL'),
    ('Yulia', 'Putintseva', 'KAZ'),
    ('Katerina', 'Siniakova', 'CZE'),
    ('Donna', 'Vekic', 'CRO'),
    ('Sorana', 'Cirstea', 'ROU'),
    ('Caroline', 'Dolehide', 'USA'),
    ('Sloane', 'Stephens', 'USA'),
    ('Magdalena', 'Frech', 'POL'),
    ('Lesia', 'Tsurenko', 'UKR'),
    ('Marie', 'Bouzkova', 'CZE'),
    ('Clara', 'Burel', 'FRA'),
    ('Anhelina', 'Kalinina', 'UKR'),
    ('Sara', 'Sorribes Tormo', 'ESP'),
    ('Varvara', 'Gracheva', 'FRA'),
    ('Amanda', 'Anisimova', 'USA'),
    ('Karolina', 'Muchova', 'CZE'),
    ('Anastasia', 'Potapova', 'RUS'),
    ('Peyton', 'Stearns', 'USA'),
    ('Magda', 'Linette', 'POL'),
    ('Laura', 'Siegemund', 'GER'),
    ('Wang', 'Xinyu', 'CHN'),
    ('Bianca', 'Andreescu', 'CAN'),
    ('Diane', 'Parry', 'FRA'),
    ('Carolina', 'Maria', 'POR'),
    ('Rebecca', 'Marino', 'CAN'),
    ('Viktoriya', 'Tomova', 'BUL'),
    ('Lucia', 'Bronzetti', 'ITA'),
    ('Olga', 'Danilovic', 'SRB'),
    ('Jaqueline', 'Cristian', 'ROU'),
    ('Clara', 'Tauson', 'DEN'),
    ('Erika', 'Andreeva', 'RUS'),
    ('Wang', 'Yafan', 'CHN'),
    ('Camila', 'Osorio', 'COL'),
    ('Taylor', 'Townsend', 'USA'),
    ('Bernarda', 'Pera', 'USA'),
    ('Nadia', 'Podoroska', 'ARG'),
    ('Arantxa', 'Rus', 'NED'),
    ('Mayar', 'Sherif', 'EGY'),
    ('Xiyu', 'Wang', 'CHN'),
    ('Kamilla', 'Rakhimova', 'RUS'),
    ('Martina', 'Trevisan', 'ITA'),
    ('McCartney', 'Kessler', 'USA'),
    ('Anna', 'Blinkova', 'RUS'),
    ('Sofia', 'Kenin', 'USA'),
    ('Elisabetta', 'Cocciaretto', 'ITA'),
    ('Robin', 'Montgomery', 'USA'),
    ('Greet', 'Minnen', 'BEL'),
    ('Tatjana', 'Maria', 'GER'),
    ('Maria Lourdes', 'Carle', 'ARG'),
    ('Tamara', 'Zidansek', 'SLO'),
    ('Petra', 'Martic', 'CRO'),
    ('Oceane', 'Dodin', 'FRA'),
    ('Aliaksandra', 'Sasnovich', 'BLR'),
    ('Ajla', 'Tomljanovic', 'AUS'),
    ('Tamara', 'Korpatsch', 'GER'),
    ('Viktorija', 'Golubic', 'SUI'),
    ('Cristina', 'Bucsa', 'ESP'),
    ('Brenda', 'Fruhvirtova', 'CZE'),
    ('Alycia', 'Parks', 'USA'),
    ('Maria', 'Angelica', 'PER'),
    ('Emma', 'Raducanu', 'GBR'),
    ('Naomi', 'Osaka', 'JPN'),
    ('Ashlyn', 'Krueger', 'USA'),
    ('Zhuoxuan', 'Bai', 'CHN'),
    ('Yue', 'Yuan', 'CHN'),
    ('Linda', 'Fruhvirtova', 'CZE');
GO