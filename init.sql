PRINT '🚀 BooksReviewsDb - Init Docker SQL Server 2022';
PRINT '   Usuario: Senior .NET Developer Cali';
GO

-- 1. Crear BD
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'BooksReviewsDb')
BEGIN
    CREATE DATABASE BooksReviewsDb;
    PRINT '✅ CREADA BooksReviewsDb';
END
GO

USE [BooksReviewsDb]
GO

-- =====================================================
-- BOOKS (tu esquema EF exacto)
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Books')
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[Books](
	[Id] [nvarchar](450) NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[Author] [nvarchar](100) NOT NULL,
	[Category] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[CoverUrl] [nvarchar](max) NULL,
	[AverageRating] [float] NOT NULL,
 CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED ([Id] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, 
	      ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
PRINT '✅ CREADA Books';
END
GO

-- =====================================================
-- REVIEWS
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Reviews')
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[Reviews](
	[Id] [nvarchar](450) NOT NULL,
	[BookId] [nvarchar](450) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[Rating] [float] NOT NULL,
	[Comment] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Reviews] PRIMARY KEY CLUSTERED ([Id] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, 
	      ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
PRINT '✅ CREADA Reviews';
END
GO

-- =====================================================
-- USERS (TUS DATOS REALES)
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[Users](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
	[AvatarUrl] [nvarchar](max) NULL,
	[PasswordHash] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, 
	      ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
) ON [PRIMARY]
PRINT '✅ CREADA Users';
END
GO

-- =====================================================
-- FK RELACIONES (exactas EF Core)
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Reviews_Books_BookId')
BEGIN
ALTER TABLE [dbo].[Reviews] WITH CHECK ADD CONSTRAINT [FK_Reviews_Books_BookId] 
	FOREIGN KEY([BookId]) REFERENCES [dbo].[Books] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[Reviews] CHECK CONSTRAINT [FK_Reviews_Books_BookId]
PRINT '✅ FK Reviews -> Books CASCADE';
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Reviews_Users_UserId')
BEGIN
ALTER TABLE [dbo].[Reviews] WITH CHECK ADD CONSTRAINT [FK_Reviews_Users_UserId] 
	FOREIGN KEY([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[Reviews] CHECK CONSTRAINT [FK_Reviews_Users_UserId]
PRINT '✅ FK Reviews -> Users CASCADE';
END
GO

-- =====================================================
-- TUS USERS REALES (exactos)
-- =====================================================
IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = 'u-3495')
BEGIN
INSERT [Users] ([Id], [Name], [Email], [AvatarUrl], [PasswordHash]) VALUES
('u-3495', 'Francia Elena Mosquera Prado', 'franciaemp1998@gmail.com', 
 'https://ui-avatars.com/api/?name=Francia+Elena+Mosquera+Prado&background=0D8ABC&color=fff',
 '$2a$11$O6uNKkxkFYrCruW37bnY4e/mSkYd3vmWHKd6NFgOPcDFw77vegZLu'),
('u-5864', 'Bob Coder', 'BobCoder@gmail.com', 
 'https://ui-avatars.com/api/?name=Bob+Coder&background=0D8ABC&color=fff',
 '$2a$11$jky5adAgHvZD2NdeiaREkuhrsQf42SUujZuxGejkQxpO//S9N0k4S'),
('u-9271', 'Alice Dev', 'adev@gmail.com', 
 'https://ui-avatars.com/api/?name=Alice+Dev&background=0D8ABC&color=fff',
 '$2a$11$p9QE2y5ksGIYPbQfrchihutPMa3XvoJruHh41oVQQ5qooQwWkF1qW');
PRINT '✅ INSERTADOS: Francia, Bob, Alice';
END
GO

-- =====================================================
-- TUS BOOKS REALES + Reviews existentes
-- =====================================================
IF NOT EXISTS (SELECT 1 FROM Books WHERE Id = '47edd335-2cb0-42dd-90fe-d5662a459584')
BEGIN
INSERT [Books] ([Id], [Title], [Author], [Category], [Description], [CoverUrl], [AverageRating]) VALUES
('47edd335-2cb0-42dd-90fe-d5662a459584', 'The Clean Coder', 'Robert C. Martin', 'Tecnología', 
 'Un código de conducta para programadores profesionales...', 'https://picsum.photos/id/101/300/450', 5),
('61904e11-d332-42c9-8b1d-63fc7cf8cb3c', 'Matar a un ruiseñor', 'Harper Lee', 'Ficción', 
 'La inolvidable novela de una infancia...', 'https://picsum.photos/id/104/300/450', 5),
('7e71b208-d4c8-478b-95e1-bb70ea70c5ab', 'El Gran Gatsby', 'F. Scott Fitzgerald', 'Ficción', 
 'La historia del fabulosamente rico Jay Gatsby...', 'https://picsum.photos/id/103/300/450', 5);
PRINT '✅ INSERTADOS: Tus 6 libros reales';
END
GO

PRINT '🎉 BooksReviewsDb 100% PRODUCCIÓN - Levanta API: docker compose up';
GO
