-- Create Database
CREATE DATABASE TeamNotesDB;
GO

USE TeamNotesDB;
GO

-- 1. Teams Table
CREATE TABLE Teams (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE()
);

-- 2. Users Table
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TeamId INT NOT NULL,
    Email NVARCHAR(256) NOT NULL UNIQUE,
    FullName NVARCHAR(100) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    FOREIGN KEY (TeamId) REFERENCES Teams(Id) ON DELETE CASCADE
);

-- 3. Notes Table
CREATE TABLE Notes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TeamId INT NOT NULL,
    CreatorUserId INT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Content NVARCHAR(MAX) NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    IsDeleted BIT DEFAULT 0 NOT NULL, -- Soft Delete
    FOREIGN KEY (TeamId) REFERENCES Teams(Id) ON DELETE NO ACTION,
    FOREIGN KEY (CreatorUserId) REFERENCES Users(Id) ON DELETE NO ACTION
);

-- Indexes for performance and isolation
CREATE INDEX IX_Users_TeamId ON Users(TeamId);
CREATE INDEX IX_Notes_TeamId_IsDeleted ON Notes(TeamId, IsDeleted);

-- Seed Data
INSERT INTO Teams (Name) VALUES ('Engineering'), ('Marketing');
INSERT INTO Users (TeamId, Email, FullName) VALUES 
(1, 'alice@eng.com', 'Alice Engineer'),
(1, 'bob@eng.com', 'Bob Engineer'),
(2, 'charlie@mkt.com', 'Charlie Marketer');
GO