CREATE DATABASE ComputerGamesLibrary;
GO

USE ComputerGamesLibrary;
GO

CREATE TABLE Users (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Username VARCHAR(50) NOT NULL UNIQUE,
    HashedPassword VARCHAR(70) NOT NULL
);

CREATE TABLE UserComputerGames (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Title VARCHAR(100) NOT NULL,
    Genre VARCHAR(50) NOT NULL,
    YearPublished INT NOT NULL,
    Price INT NOT NULL,
    UserId INT NOT NULL
);

ALTER TABLE UserComputerGames
ADD FOREIGN KEY (UserId) REFERENCES Users(ID);