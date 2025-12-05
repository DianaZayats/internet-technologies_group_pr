-- Скрипт для исправления колонки CookingTime -> PreparationTime
-- Выполните этот скрипт в SQL Server Management Studio или через dotnet ef database update

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Recipes')
BEGIN
    -- Если существует CookingTime и не существует PreparationTime, переименовываем
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'CookingTime')
        AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'PreparationTime')
    BEGIN
        EXEC sp_rename '[dbo].[Recipes].[CookingTime]', 'PreparationTime', 'COLUMN';
        PRINT 'Колонка CookingTime переименована в PreparationTime';
    END
    
    -- Если оба существуют, удаляем CookingTime
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'CookingTime')
        AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'PreparationTime')
    BEGIN
        ALTER TABLE [Recipes] DROP COLUMN [CookingTime];
        PRINT 'Колонка CookingTime удалена (PreparationTime уже существует)';
    END
    
    -- Если PreparationTime не существует, создаем его
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'PreparationTime')
    BEGIN
        ALTER TABLE [Recipes] ADD [PreparationTime] int NOT NULL DEFAULT 30;
        PRINT 'Колонка PreparationTime создана';
    END
    ELSE
    BEGIN
        PRINT 'Колонка PreparationTime уже существует';
    END
END
ELSE
BEGIN
    PRINT 'Таблица Recipes не найдена';
END

