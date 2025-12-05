using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameCookingTimeToPreparationTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Переименовываем колонку CookingTime в PreparationTime
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Recipes')
                BEGIN
                    -- Если существует CookingTime и не существует PreparationTime, переименовываем
                    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'CookingTime')
                        AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'PreparationTime')
                    BEGIN
                        EXEC sp_rename '[dbo].[Recipes].[CookingTime]', 'PreparationTime', 'COLUMN';
                    END
                    
                    -- Если оба существуют, удаляем CookingTime
                    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'CookingTime')
                        AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'PreparationTime')
                    BEGIN
                        ALTER TABLE [Recipes] DROP COLUMN [CookingTime];
                    END
                    
                    -- Если PreparationTime не существует, создаем его (на случай, если CookingTime тоже нет)
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'PreparationTime')
                    BEGIN
                        ALTER TABLE [Recipes] ADD [PreparationTime] int NOT NULL DEFAULT 30;
                    END
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Откат: переименовываем обратно (если нужно)
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Recipes')
                BEGIN
                    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'PreparationTime')
                        AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'CookingTime')
                    BEGIN
                        EXEC sp_rename '[dbo].[Recipes].[PreparationTime]', 'CookingTime', 'COLUMN';
                    END
                END
            ");
        }
    }
}

