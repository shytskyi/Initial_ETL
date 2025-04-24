-- 1. Створюємо базу даних
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'OpenDataBase')
BEGIN
    CREATE DATABASE OpenDataBase;
END;
GO

-- 2. Переходимо в контекст нової бази
USE OpenDataBase;
GO

-- 3. Створюємо таблицю OpenTable Server1
CREATE TABLE dbo.OpenTable
(
    ID INT IDENTITY(1,1) PRIMARY KEY,                           -- Унікальний ідентифікатор
    Name NVARCHAR(200)    NOT NULL,                             -- Назва
    Description NVARCHAR(1000) NULL,                            -- Опис (може бути NULL)
    CreatedDate DATETIME2  NOT NULL DEFAULT SYSUTCDATETIME(),   -- Дата створення
    ModifiedDate DATETIME2 NULL,                                -- Дата останньої зміни
    IsActive BIT         NOT NULL DEFAULT 1                     -- Активність
);
GO

 -- 4. Створюємо таблицю OpenTable2 Server2
CREATE TABLE dbo.OpenTable2
(
    ID INT IDENTITY(1,1) PRIMARY KEY,                           -- Унікальний ідентифікатор
    Name2 NVARCHAR(255)    NOT NULL,                            -- Назва
    Description NVARCHAR(1000) NULL,                            -- Опис (може бути NULL)
    CreatedDate DATETIME2  NOT NULL DEFAULT SYSUTCDATETIME(),   -- Дата створення
    ModifiedDate DATETIME2 NULL,                                -- Дата останньої зміни
    IsActive BIT         NOT NULL DEFAULT 1,                    -- Активність
    SomeField NVARCHAR(100) NULL                                -- Додаткове поле
);
GO

USE OpenDataBase;
GO

INSERT INTO dbo.OpenTable (Name, Description)
VALUES
    ('Alpha',   'First sample record'),
    ('Bravo',   'Second sample record'),
    ('Charlie', 'Third sample record'),
    ('Delta',   'Fourth sample record'),
    ('Echo',    'Fifth sample record');
GO

-- Перевіримо вставлені дані
SELECT * FROM dbo.OpenTable;
GO


SELECT 
	TABLE_SCHEMA
	,TABLE_NAME
	,COLUMN_NAME
	,DATA_TYPE
	,COLLATION_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'OpenTable' AND TABLE_SCHEMA = 'dbo'
--WHERE TABLE_NAME = 'OpenTable2' AND TABLE_SCHEMA = 'dbo'