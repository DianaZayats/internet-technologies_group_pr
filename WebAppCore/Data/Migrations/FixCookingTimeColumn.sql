-- Скрипт для исправления колонки CookingTime -> PreparationTime
-- Выполните этот скрипт в SQL Server Management Studio или через dotnet ef database update

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Recipes')
BEGIN
    -- Переименовуємо CookingTime в PreparationTime, якщо існує
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'CookingTime')
        AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'PreparationTime')
    BEGIN
        EXEC sp_rename '[dbo].[Recipes].[CookingTime]', 'PreparationTime', 'COLUMN';
    END
    
    -- Якщо CookingTime все ще існує після переименування, видаляємо його
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'CookingTime')
        AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'PreparationTime')
    BEGIN
        ALTER TABLE [Recipes] DROP COLUMN [CookingTime];
    END
    
    -- Якщо PreparationTime не існує, створюємо його
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'PreparationTime')
    BEGIN
        ALTER TABLE [Recipes] ADD [PreparationTime] int NOT NULL DEFAULT 30;
    END
END

