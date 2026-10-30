
-- Select the database
USE EnhanzerDB;
GO

-- Create Location_Details table
IF OBJECT_ID('dbo.Location_Details', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Location_Details
    (
        LocationId INT IDENTITY(1,1) PRIMARY KEY,

        Location_Code NVARCHAR(50) NOT NULL,

        Location_Name NVARCHAR(150) NOT NULL,

        CONSTRAINT UQ_Location_Code
            UNIQUE (Location_Code)
    );
END
GO
