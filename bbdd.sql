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
    Logo NVARCHAR(200) NULL,
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
    ('Jannik', 'Sinner', 'IT'),
    ('Novak', 'Djokovic', 'RS'),
    ('Carlos', 'Alcaraz', 'ES'),
    ('Alexander', 'Zverev', 'DE'),
    ('Daniil', 'Medvedev', 'RU'),
    ('Andrey', 'Rublev', 'RU'),
    ('Casper', 'Ruud', 'NO'),
    ('Hubert', 'Hurkacz', 'PL'),
    ('Grigor', 'Dimitrov', 'BG'),
    ('Alex', 'de Minaur', 'AU'),
    ('Stefanos', 'Tsitsipas', 'GR'),
    ('Taylor', 'Fritz', 'US'),
    ('Tommy', 'Paul', 'US'),
    ('Ben', 'Shelton', 'US'),
    ('Holger', 'Rune', 'DK'),
    ('Frances', 'Tiafoe', 'US'),
    ('Karen', 'Khachanov', 'RU'),
    ('Ugo', 'Humbert', 'FR'),
    ('Félix', 'Auger-Aliassime', 'CA'),
    ('Nicolas', 'Jarry', 'CL'),
    ('Sebastian', 'Korda', 'US'),
    ('Sebastian', 'Baez', 'AR'),
    ('Adrian', 'Mannarino', 'FR'),
    ('Lorenzo', 'Musetti', 'IT'),
    ('Alexander', 'Bublik', 'KZ'),
    ('Jan-Lennard', 'Struff', 'DE'),
    ('Tallon', 'Griekspoor', 'NL'),
    ('Jack', 'Draper', 'GB'),
    ('Francisco', 'Cerundolo', 'AR'),
    ('Alejandro', 'Tabilo', 'CL'),
    ('Mariano', 'Navone', 'AR'),
    ('Arthur', 'Fils', 'FR'),
    ('Alexei', 'Popyrin', 'AU'),
    ('Felix', 'Auger-Aliassime', 'CA'),
    ('Flavio', 'Cobolli', 'IT'),
    ('Tomas Martin', 'Etcheverry', 'AR'),
    ('Nuno', 'Borges', 'PT'),
    ('Cameron', 'Norrie', 'GB'),
    ('Alejandro', 'Davidovich Fokina', 'ES'),
    ('Roman', 'Safiullin', 'RU'),
    ('Pavel', 'Kotov', 'RU'),
    ('Daniel Elahi', 'Galan', 'CO'),
    ('Arthur', 'Rinderknech', 'FR'),
    ('Lorenzo', 'Sonego', 'IT'),
    ('Matteo', 'Arnaldi', 'IT'),
    ('Gael', 'Monfils', 'FR'),
    ('Laslo', 'Djere', 'RS'),
    ('Fabian', 'Marozsan', 'HU'),
    ('Zhizhen', 'Zhang', 'CN'),
    ('Jakub', 'Mensik', 'CZ'),
    ('Jiri', 'Lehecka', 'CZ'),
    ('Brandon', 'Nakashima', 'US'),
    ('Jordan', 'Thompson', 'AU'),
    ('Kei', 'Nishikori', 'JP'),
    ('Luciano', 'Darderi', 'IT'),
    ('Marcos', 'Giron', 'US'),
    ('Max', 'Purcell', 'AU'),
    ('Matteo', 'Berrettini', 'IT'),
    ('Dominik', 'Koepfer', 'DE'),
    ('Giovanni', 'Mpetshi Perricard', 'FR'),
    ('Alexandre', 'Muller', 'FR'),
    ('Reilly', 'Opelka', 'US'),
    ('Yoshihito', 'Nishioka', 'JP'),
    ('Roberto', 'Carballes Baena', 'ES'),
    ('Botic', 'Van de Zandschulp', 'NL'),
    ('David', 'Goffin', 'BE'),
    ('Thiago', 'Monteiro', 'BR'),
    ('Marton', 'Fucsovics', 'HU'),
    ('Roberto', 'Bautista Agut', 'ES'),
    ('Emil', 'Ruusuvuori', 'FI'),
    ('Taro', 'Daniel', 'JP'),
    ('Jaume', 'Munar', 'ES'),
    ('Miomir', 'Kecmanovic', 'RS'),
    ('Zizou', 'Bergs', 'BE'),
    ('Christopher', 'Eubanks', 'US'),
    ('Yannick', 'Hanfmann', 'DE'),
    ('Luca', 'Van Assche', 'FR'),
    ('Stan', 'Wawrinka', 'CH'),
    ('Gabriel', 'Diallo', 'CA'),
    ('Alexander', 'Shevchenko', 'KZ'),
    ('Tomas', 'Machac', 'CZ'),
    ('Daniel', 'Altmaier', 'DE'),
    ('Marin', 'Cilic', 'HR'),
    ('Rinky', 'Hijikata', 'AU'),
    ('Thanasi', 'Kokkinakis', 'AU'),
    ('Bernabe', 'Zapata Miralles', 'ES'),
    ('J.J.', 'Wolf', 'US'),
    ('Francisco', 'Comesana', 'AR'),
    ('Pedro', 'Martinez', 'ES'),
    ('Federico', 'Coria', 'AR'),
    ('Albert Ramos', 'Viñolas', 'ES'),
    ('Yunchaokete', 'Bu', 'CN'),
    ('Thiago', 'Seyboth Wild', 'BR'),
    ('Denis', 'Shapovalov', 'CA'),
    ('Hamad', 'Medjedovic', 'RS'),
    ('James', 'Duckworth', 'AU'),
    ('Dusan', 'Lajovic', 'RS'),
    ('Facundo', 'Diaz Acosta', 'AR');
GO

-- Insert Top 100 WTA Players (Women's)
INSERT INTO Players (Name, LastName, CountryCode)
VALUES
    ('Aryna', 'Sabalenka', 'BY'),
    ('Iga', 'Swiatek', 'PL'),
    ('Coco', 'Gauff', 'US'),
    ('Elena', 'Rybakina', 'KZ'),
    ('Jasmine', 'Paolini', 'IT'),
    ('Jessica', 'Pegula', 'US'),
    ('Qinwen', 'Zheng', 'CN'),
    ('Marketa', 'Vondrousova', 'CZ'),
    ('Maria', 'Sakkari', 'GR'),
    ('Ons', 'Jabeur', 'TN'),
    ('Danielle', 'Collins', 'US'),
    ('Daria', 'Kasatkina', 'RU'),
    ('Beatriz', 'Haddad Maia', 'BR'),
    ('Jelena', 'Ostapenko', 'LV'),
    ('Madison', 'Keys', 'US'),
    ('Liudmila', 'Samsonova', 'RU'),
    ('Emma', 'Navarro', 'US'),
    ('Elina', 'Svitolina', 'UA'),
    ('Marta', 'Kostyuk', 'UA'),
    ('Mirra', 'Andreeva', 'RU'),
    ('Victoria', 'Azarenka', 'BY'),
    ('Caroline', 'Garcia', 'FR'),
    ('Ekaterina', 'Alexandrova', 'RU'),
    ('Diana', 'Shnaider', 'RU'),
    ('Anastasia', 'Pavlyuchenkova', 'RU'),
    ('Anna', 'Kalinskaya', 'RU'),
    ('Barbora', 'Krejcikova', 'CZ'),
    ('Leylah', 'Fernandez', 'CA'),
    ('Linda', 'Noskova', 'CZ'),
    ('Dayana', 'Yastremska', 'UA'),
    ('Paula', 'Badosa', 'ES'),
    ('Katie', 'Boulter', 'GB'),
    ('Elise', 'Mertens', 'BE'),
    ('Yulia', 'Putintseva', 'KZ'),
    ('Katerina', 'Siniakova', 'CZ'),
    ('Donna', 'Vekic', 'HR'),
    ('Sorana', 'Cirstea', 'RO'),
    ('Caroline', 'Dolehide', 'US'),
    ('Sloane', 'Stephens', 'US'),
    ('Magdalena', 'Frech', 'PL'),
    ('Lesia', 'Tsurenko', 'UA'),
    ('Marie', 'Bouzkova', 'CZ'),
    ('Clara', 'Burel', 'FR'),
    ('Anhelina', 'Kalinina', 'UA'),
    ('Sara', 'Sorribes Tormo', 'ES'),
    ('Varvara', 'Gracheva', 'FR'),
    ('Amanda', 'Anisimova', 'US'),
    ('Karolina', 'Muchova', 'CZ'),
    ('Anastasia', 'Potapova', 'RU'),
    ('Peyton', 'Stearns', 'US'),
    ('Magda', 'Linette', 'PL'),
    ('Laura', 'Siegemund', 'DE'),
    ('Wang', 'Xinyu', 'CN'),
    ('Bianca', 'Andreescu', 'CA'),
    ('Diane', 'Parry', 'FR'),
    ('Carolina', 'Maria', 'PT'),
    ('Rebecca', 'Marino', 'CA'),
    ('Viktoriya', 'Tomova', 'BG'),
    ('Lucia', 'Bronzetti', 'IT'),
    ('Olga', 'Danilovic', 'RS'),
    ('Jaqueline', 'Cristian', 'RO'),
    ('Clara', 'Tauson', 'DK'),
    ('Erika', 'Andreeva', 'RU'),
    ('Wang', 'Yafan', 'CN'),
    ('Camila', 'Osorio', 'CO'),
    ('Taylor', 'Townsend', 'US'),
    ('Bernarda', 'Pera', 'US'),
    ('Nadia', 'Podoroska', 'AR'),
    ('Arantxa', 'Rus', 'NL'),
    ('Mayar', 'Sherif', 'EG'),
    ('Xiyu', 'Wang', 'CN'),
    ('Kamilla', 'Rakhimova', 'RU'),
    ('Martina', 'Trevisan', 'IT'),
    ('McCartney', 'Kessler', 'US'),
    ('Anna', 'Blinkova', 'RU'),
    ('Sofia', 'Kenin', 'US'),
    ('Elisabetta', 'Cocciaretto', 'IT'),
    ('Robin', 'Montgomery', 'US'),
    ('Greet', 'Minnen', 'BE'),
    ('Tatjana', 'Maria', 'DE'),
    ('Maria Lourdes', 'Carle', 'AR'),
    ('Tamara', 'Zidansek', 'SI'),
    ('Petra', 'Martic', 'HR'),
    ('Oceane', 'Dodin', 'FR'),
    ('Aliaksandra', 'Sasnovich', 'BY'),
    ('Ajla', 'Tomljanovic', 'AU'),
    ('Tamara', 'Korpatsch', 'DE'),
    ('Viktorija', 'Golubic', 'CH'),
    ('Cristina', 'Bucsa', 'ES'),
    ('Brenda', 'Fruhvirtova', 'CZ'),
    ('Alycia', 'Parks', 'US'),
    ('Maria', 'Angelica', 'PE'),
    ('Emma', 'Raducanu', 'GB'),
    ('Naomi', 'Osaka', 'JP'),
    ('Ashlyn', 'Krueger', 'US'),
    ('Zhuoxuan', 'Bai', 'CN'),
    ('Yue', 'Yuan', 'CN'),
    ('Linda', 'Fruhvirtova', 'CZ');
GO