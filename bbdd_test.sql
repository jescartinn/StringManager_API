-- Use the StringManagerDb database
USE StringManagerDb;
GO

-- Add sample stringers
INSERT INTO Stringers (Name, LastName, Email, PhoneNumber)
VALUES
    ('Juan', 'Martinez', 'juan.martinez@stringteam.com', '+34612345678'),
    ('Emma', 'Wilson', 'emma.wilson@stringteam.com', '+447891234567'),
    ('Roberto', 'Ferrari', 'roberto.ferrari@stringteam.com', '+393471234567'),
    ('Sophie', 'Dubois', 'sophie.dubois@stringteam.com', '+33712345678'),
    ('Michael', 'Schmidt', 'michael.schmidt@stringteam.com', '+491721234567');
GO

-- Add sample string types
INSERT INTO StringTypes (Brand, Model, Gauge, Material, Color)
VALUES
    ('Babolat', 'RPM Blast', '1.25mm/17', 'Polyester', 'Black'),
    ('Luxilon', 'Alu Power', '1.25mm/16L', 'Polyester', 'Silver'),
    ('Wilson', 'NXT', '1.30mm/16', 'Multifilament', 'Natural'),
    ('Tecnifibre', 'X-One Biphase', '1.24mm/17', 'Multifilament', 'Natural'),
    ('Solinco', 'Hyper-G', '1.25mm/17', 'Polyester', 'Green'),
    ('Head', 'Lynx', '1.25mm/17', 'Polyester', 'Grey'),
    ('Yonex', 'Poly Tour Pro', '1.25mm/17', 'Polyester', 'Blue'),
    ('Gamma', 'Professional', '1.32mm/16', 'Synthetic Gut', 'Natural'),
    ('Prince', 'Synthetic Gut', '1.30mm/16', 'Synthetic Gut', 'White'),
    ('Head', 'Hawk', '1.25mm/17', 'Polyester', 'Grey'),
    ('Wilson', 'Sensation', '1.30mm/16', 'Multifilament', 'Natural'),
    ('Babolat', 'VS Touch', '1.30mm/16', 'Natural Gut', 'Natural'),
    ('Luxilon', 'Element', '1.25mm/17', 'Polyester', 'Bronze'),
    ('Tecnifibre', 'Pro Red Code', '1.25mm/17', 'Polyester', 'Red'),
    ('Solinco', 'Tour Bite', '1.25mm/17', 'Polyester', 'Silver');
GO

-- Add sample tournaments
INSERT INTO Tournaments (Name, StartDate, EndDate, Location, Category)
VALUES
    ('Australian Open', '2025-01-20', '2025-02-02', 'Melbourne, Australia', 'Grand Slam'),
    ('Roland Garros', '2025-05-25', '2025-06-08', 'Paris, France', 'Grand Slam'),
    ('Wimbledon', '2025-06-30', '2025-07-13', 'London, UK', 'Grand Slam'),
    ('US Open', '2025-08-25', '2025-09-07', 'New York, USA', 'Grand Slam'),
    ('Madrid Open', '2025-04-30', '2025-05-09', 'Madrid, Spain', 'ATP 1000'),
    ('Italian Open', '2025-05-12', '2025-05-19', 'Rome, Italy', 'ATP 1000'),
    ('Monte-Carlo Masters', '2025-04-11', '2025-04-19', 'Monte Carlo, Monaco', 'ATP 1000'),
    ('Miami Open', '2025-03-24', '2025-04-06', 'Miami, USA', 'ATP 1000'),
    ('Indian Wells Masters', '2025-03-11', '2025-03-22', 'Indian Wells, USA', 'ATP 1000'),
    -- Current tournament (adjust dates as needed)
    ('Barcelona Open', '2025-04-15', '2025-04-23', 'Barcelona, Spain', 'ATP 500');
GO

-- Add sample racquets for top players
-- Novak Djokovic racquets (ID 2)
INSERT INTO Racquets (PlayerId, Brand, Model, SerialNumber, HeadSize, Notes)
VALUES
    (2, 'Head', 'Speed Pro', 'ND2023-001', 100, 'Main racquet'),
    (2, 'Head', 'Speed Pro', 'ND2023-002', 100, 'Backup racquet'),
    (2, 'Head', 'Speed Pro', 'ND2023-003', 100, 'Backup racquet');

-- Carlos Alcaraz racquets (ID 3)
INSERT INTO Racquets (PlayerId, Brand, Model, SerialNumber, HeadSize, Notes)
VALUES
    (3, 'Babolat', 'Pure Aero VS', 'CA2023-001', 98, 'Main racquet'),
    (3, 'Babolat', 'Pure Aero VS', 'CA2023-002', 98, 'Backup racquet'),
    (3, 'Babolat', 'Pure Aero VS', 'CA2023-003', 98, 'Backup racquet');

-- Jannik Sinner racquets (ID 1)
INSERT INTO Racquets (PlayerId, Brand, Model, SerialNumber, HeadSize, Notes)
VALUES
    (1, 'Head', 'Speed Pro', 'JS2023-001', 100, 'Main racquet'),
    (1, 'Head', 'Speed Pro', 'JS2023-002', 100, 'Backup racquet'),
    (1, 'Head', 'Speed Pro', 'JS2023-003', 100, 'Backup racquet');

-- Iga Swiatek racquets (ID 102)
INSERT INTO Racquets (PlayerId, Brand, Model, SerialNumber, HeadSize, Notes)
VALUES
    (102, 'Tecnifibre', 'Tempo', 'IS2023-001', 100, 'Main racquet'),
    (102, 'Tecnifibre', 'Tempo', 'IS2023-002', 100, 'Backup racquet');

-- Coco Gauff racquets (ID 103)
INSERT INTO Racquets (PlayerId, Brand, Model, SerialNumber, HeadSize, Notes)
VALUES
    (103, 'Wilson', 'Blade 98', 'CG2023-001', 98, 'Main racquet'),
    (103, 'Wilson', 'Blade 98', 'CG2023-002', 98, 'Backup racquet');

-- Alexander Zverev (ID 4)
INSERT INTO Racquets (PlayerId, Brand, Model, SerialNumber, HeadSize, Notes)
VALUES
    (4, 'Head', 'Gravity Pro', 'AZ2023-001', 100, 'Main racquet'),
    (4, 'Head', 'Gravity Pro', 'AZ2023-002', 100, 'Backup racquet');

-- Aryna Sabalenka (ID 101)
INSERT INTO Racquets (PlayerId, Brand, Model, SerialNumber, HeadSize, Notes)
VALUES
    (101, 'Wilson', 'Blade 98', 'AS2023-001', 98, 'Main racquet'),
    (101, 'Wilson', 'Blade 98', 'AS2023-002', 98, 'Backup racquet');

-- Daniil Medvedev (ID 5)
INSERT INTO Racquets (PlayerId, Brand, Model, SerialNumber, HeadSize, Notes)
VALUES
    (5, 'Tecnifibre', 'T-Fight 305', 'DM2023-001', 100, 'Main racquet'),
    (5, 'Tecnifibre', 'T-Fight 305', 'DM2023-002', 100, 'Backup racquet');

-- Elena Rybakina (ID 104)
INSERT INTO Racquets (PlayerId, Brand, Model, SerialNumber, HeadSize, Notes)
VALUES
    (104, 'Wilson', 'Blade 98', 'ER2023-001', 98, 'Main racquet'),
    (104, 'Wilson', 'Blade 98', 'ER2023-002', 98, 'Backup racquet');

-- Andrey Rublev (ID 6)
INSERT INTO Racquets (PlayerId, Brand, Model, SerialNumber, HeadSize, Notes)
VALUES
    (6, 'Head', 'Gravity Pro', 'AR2023-001', 100, 'Main racquet'),
    (6, 'Head', 'Gravity Pro', 'AR2023-002', 100, 'Backup racquet');
GO

-- Add sample string jobs with Price and IsPaid fields
INSERT INTO StringJobs (PlayerId, RacquetId, MainStringId, CrossStringId, StringerId, TournamentId, CreatedAt, CompletedAt, MainTension, CrossTension, IsTensionInKg, Logo, Status, Price, IsPaid, Notes, Priority)
VALUES
    -- Completed jobs
    (1, 7, 1, NULL, 1, 1, '2025-01-20 08:00:00', '2025-01-20 08:30:00', 25.0, NULL, 1, 'Head White', 'Completed', 30.00, 1, 'First round preparation', 1),
    (1, 8, 1, NULL, 1, 1, '2025-01-21 09:00:00', '2025-01-21 09:35:00', 25.0, NULL, 1, 'Head White', 'Completed', 30.00, 1, 'Second round preparation', 1),
    (2, 1, 2, NULL, 2, 1, '2025-01-20 10:00:00', '2025-01-20 10:40:00', 26.5, NULL, 1, 'Head Black', 'Completed', 30.00, 1, 'First round preparation', 1),
    (3, 4, 5, NULL, 3, 1, '2025-01-20 11:00:00', '2025-01-20 11:45:00', 25.0, NULL, 1, 'Babolat White', 'Completed', 30.00, 1, 'First round preparation', 1),
    (101, 16, 3, 4, 4, 1, '2025-01-20 12:00:00', '2025-01-20 12:30:00', 24.0, 23.0, 1, 'Wilson White', 'Completed', 35.00, 1, 'First round preparation', 1),
    (102, 14, 6, NULL, 5, 1, '2025-01-20 13:00:00', '2025-01-20 13:40:00', 26.0, NULL, 1, 'Tecnifibre White', 'Completed', 30.00, 1, 'First round preparation', 1),
    
    -- Roland Garros jobs
    (1, 7, 1, NULL, 1, 2, '2025-05-25 08:00:00', '2025-05-25 08:30:00', 25.0, NULL, 1, 'Head White', 'Completed', 30.00, 1, 'First round preparation', 1),
    (2, 1, 2, NULL, 2, 2, '2025-05-25 09:00:00', '2025-05-25 09:30:00', 26.5, NULL, 1, 'Head White', 'Completed', 30.00, 1, 'First round preparation', 1),
    
    -- Wimbledon jobs
    (1, 7, 1, NULL, 1, 3, '2025-06-30 08:00:00', '2025-06-30 08:30:00', 26.0, NULL, 1, 'Head Black', 'Completed', 30.00, 1, 'First round preparation', 1),
    (2, 2, 2, NULL, 2, 3, '2025-06-30 09:00:00', '2025-06-30 09:30:00', 27.0, NULL, 1, 'Head Black', 'Completed', 30.00, 1, 'First round preparation', 1),
    
    -- US Open jobs
    (1, 7, 1, NULL, 1, 4, '2025-08-25 08:00:00', '2025-08-25 08:30:00', 25.0, NULL, 1, 'Head White', 'Completed', 30.00, 1, 'First round preparation', 1),
    (2, 1, 2, NULL, 2, 4, '2025-08-25 09:00:00', '2025-08-25 09:30:00', 26.5, NULL, 1, 'Head White', 'Completed', 30.00, 1, 'First round preparation', 1),
    
    -- Current pending jobs for the Barcelona Open
    (1, 7, 1, NULL, 1, 10, '2025-04-15 08:00:00', NULL, 25.0, NULL, 1, 'Head Red', 'Pending', 30.00, 0, 'First round preparation', 1),
    (2, 1, 2, NULL, 2, 10, '2025-04-15 08:30:00', NULL, 26.5, NULL, 1, 'Head White', 'Pending', 30.00, 0, 'First round preparation', 1),
    (3, 4, 5, NULL, 3, 10, '2025-04-15 09:00:00', NULL, 25.0, NULL, 1, 'Babolat White', 'Pending', 30.00, 0, 'First round preparation', 1),
    (101, 16, 3, 4, 4, 10, '2025-04-15 09:30:00', NULL, 24.0, 23.0, 1, 'Wilson Black', 'Pending', 35.00, 0, 'First round preparation', 1),
    (102, 14, 6, NULL, 5, 10, '2025-04-15 10:00:00', NULL, 26.0, NULL, 1, 'Tecnifibre White', 'Pending', 30.00, 0, 'First round preparation', 1),
    
    -- Jobs in progress for the Barcelona Open
    (4, 18, 7, NULL, 1, 10, '2025-04-15 10:30:00', NULL, 25.5, NULL, 1, 'Head Black', 'InProgress', 30.00, 0, 'First round preparation', 1),
    (5, 20, 8, NULL, 2, 10, '2025-04-15 11:00:00', NULL, 27.0, NULL, 1, 'Tecnifibre White', 'InProgress', 30.00, 0, 'First round preparation', 2),

    -- Add a few cancelled jobs
    (6, 22, 9, NULL, 3, 10, '2025-04-15 11:30:00', NULL, 26.0, NULL, 1, 'Head White', 'Cancelled', 30.00, 0, 'Cancelled due to player withdrawal', 2),
    (104, 18, 10, NULL, 4, 10, '2025-04-15 12:00:00', NULL, 25.0, NULL, 1, 'Wilson Black', 'Cancelled', 30.00, 0, 'Player changed racquet', 3);
GO