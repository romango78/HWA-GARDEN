/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

IF NOT EXISTS(SELECT 1 FROM sys.database_principals WHERE name = 'app_user')
BEGIN
    CREATE USER [app_user]
	FOR LOGIN [HwaGardenApp]
	WITH DEFAULT_SCHEMA = dbo;

    EXEC sp_addrolemember N'db_datawriter', N'app_user';

    GRANT SELECT ON SCHEMA::eso TO [app_user];

    GRANT INSERT ON SCHEMA::eso TO [app_user];

    GRANT DELETE ON SCHEMA::eso TO [app_user];

    GRANT UPDATE ON SCHEMA::eso TO [app_user];

    GRANT EXECUTE ON SCHEMA::eso TO [app_user];
END
