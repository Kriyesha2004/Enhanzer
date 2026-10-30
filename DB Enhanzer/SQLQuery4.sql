
USE EnhanzerDB;
GO

-- Insert sample locations
IF NOT EXISTS (
    SELECT 1 FROM dbo.Location_Details
    WHERE Location_Code = 'LOC001'
)
BEGIN
    INSERT INTO dbo.Location_Details
        (Location_Code, Location_Name)
    VALUES
        ('LOC001', 'Colombo Main Branch');
END

IF NOT EXISTS (
    SELECT 1 FROM dbo.Location_Details
    WHERE Location_Code = 'LOC002'
)
BEGIN
    INSERT INTO dbo.Location_Details
        (Location_Code, Location_Name)
    VALUES
        ('LOC002', 'Negombo Branch');
END

IF NOT EXISTS (
    SELECT 1 FROM dbo.Location_Details
    WHERE Location_Code = 'LOC003'
)
BEGIN
    INSERT INTO dbo.Location_Details
        (Location_Code, Location_Name)
    VALUES
        ('LOC003', 'Kandy Branch');
END
GO
