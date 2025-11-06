IF DB_ID('LibraryDB') IS NULL
BEGIN
  CREATE DATABASE LibraryDB;
END
GO

USE LibraryDB;
GO

IF OBJECT_ID('dbo.Books', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.Books (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Author NVARCHAR(200) NOT NULL,
    Pages INT NOT NULL,
    Availible BIT NOT NULL
  );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Books WHERE Title = 'The Hobbit')
BEGIN
  INSERT INTO dbo.Books (Title, Author, Pages, Availible) VALUES
    ('The Hobbit', 'J.R.R. Tolkien', 310, 1),
    ('The Fellowship of the Ring', 'J.R.R. Tolkien', 423, 1),
    ('The Two Towers', 'J.R.R. Tolkien', 352, 1),
    ('The Return of the King', 'J.R.R. Tolkien', 416, 1);
END
GO

IF OBJECT_ID('dbo.Auth', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.Auth (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Email NVARCHAR(200) NOT NULL UNIQUE,
    PasswordHash VARBINARY(MAX) NOT NULL,
    PasswordSalt VARBINARY(MAX) NOT NULL
  );
END
GO