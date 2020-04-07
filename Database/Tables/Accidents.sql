CREATE TABLE [dbo].[Accidents]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Title] NVARCHAR(150) NOT NULL,
	[Description] NVARCHAR(5000) NOT NULL,
	[Link] NVARCHAR(250) NOT NULL,
	[Date] DATE NOT NULL,

	[ProfessionId] INT NULL,
	[DangerId] INT NULL,
	[SourceDangerId] INT NULL,

	FOREIGN KEY ([ProfessionId]) REFERENCES [dbo].[Professions]([Id]),
	FOREIGN KEY ([DangerId]) REFERENCES [dbo].[Dangers]([Id]),
	FOREIGN KEY ([SourceDangerId]) REFERENCES [dbo].[SourceDangers]([Id]),
)
