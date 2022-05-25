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

IF NOT EXISTS(SELECT 1 FROM sys.database_principals WHERE name = 'es_user')
BEGIN
    CREATE USER [es_user]
    FROM LOGIN [HwaGardenApp]
    WITH DEFAULT_SCHEMA = eso;

    GRANT CONNECT TO [es_user];

    GRANT SELECT ON SCHEMA::eso TO [es_user];

    GRANT INSERT ON SCHEMA::eso TO [es_user];

    GRANT DELETE ON SCHEMA::eso TO [es_user];

    GRANT UPDATE ON SCHEMA::eso TO [es_user];

    GRANT EXECUTE ON SCHEMA::eso TO [es_user];
END
