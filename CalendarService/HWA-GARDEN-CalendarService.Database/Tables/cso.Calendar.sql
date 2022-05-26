CREATE TABLE [cso].[Calendar]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Year] SMALLINT NOT NULL,
	[Name] NVARCHAR(250) NOT NULL,
	[Description] NVARCHAR(1024) NULL
)

GO

CREATE INDEX [IX_Calendar_Year] ON [cso].[Calendar] ([Year] ASC);
