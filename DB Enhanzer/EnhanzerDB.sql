
-- ==========================================
-- STEP 1: CREATE DATABASE
-- ==========================================

IF NOT EXISTS (
    SELECT 1
    FROM sys.databases
    WHERE name = 'EnhanzerDB'
)
BEGIN
    CREATE DATABASE EnhanzerDB;
END
GO

-- Select our database
USE EnhanzerDB;
GO
