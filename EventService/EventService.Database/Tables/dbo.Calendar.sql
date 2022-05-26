CREATE TABLE [dbo].[Calendar]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Year] SMALLINT NOT NULL,
	[Name] NVARCHAR(250) NOT NULL
)

GO

CREATE INDEX [IX_Calendar_Year] ON [dbo].[Calendar] ([Year] ASC);
