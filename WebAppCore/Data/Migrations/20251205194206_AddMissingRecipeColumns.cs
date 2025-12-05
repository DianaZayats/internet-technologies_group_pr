using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingRecipeColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Додаємо відсутні колонки до таблиці Recipes, якщо вони не існують
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Recipes')
                BEGIN
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'PreparationTime')
                        ALTER TABLE [Recipes] ADD [PreparationTime] int NOT NULL DEFAULT 30;
                    
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'Servings')
                        ALTER TABLE [Recipes] ADD [Servings] int NOT NULL DEFAULT 4;
                    
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'DietType')
                        ALTER TABLE [Recipes] ADD [DietType] nvarchar(50) NULL;
                    
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'Cuisine')
                        ALTER TABLE [Recipes] ADD [Cuisine] nvarchar(50) NULL;
                    
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'BudgetLevel')
                        ALTER TABLE [Recipes] ADD [BudgetLevel] nvarchar(20) NULL;
                    
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'Calories')
                        ALTER TABLE [Recipes] ADD [Calories] int NULL;
                    
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'IsPremium')
                        ALTER TABLE [Recipes] ADD [IsPremium] bit NOT NULL DEFAULT 0;
                    
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND name = 'IsPublic')
                        ALTER TABLE [Recipes] ADD [IsPublic] bit NOT NULL DEFAULT 1;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
