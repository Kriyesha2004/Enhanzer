SELECT
    COUNT(*) AS TotalRecords,
    COUNT(DISTINCT Location_Code) AS UniqueLocationCodes
FROM dbo.Location_Details;