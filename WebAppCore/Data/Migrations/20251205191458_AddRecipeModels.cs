using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipeModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            // FirstName та LastName можуть вже існувати - пропускаємо додавання, якщо вони вже є
            // Використовуємо SQL для перевірки наявності колонок
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'FirstName')
                BEGIN
                    ALTER TABLE [AspNetUsers] ADD [FirstName] nvarchar(max) NOT NULL DEFAULT '';
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = 'LastName')
                BEGIN
                    ALTER TABLE [AspNetUsers] ADD [LastName] nvarchar(max) NOT NULL DEFAULT '';
                END
            ");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            // Створюємо таблицю Posts тільки якщо вона не існує
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Posts')
                BEGIN
                    CREATE TABLE [Posts] (
                        [Id] int NOT NULL IDENTITY,
                        [Title] nvarchar(200) NOT NULL,
                        [Content] nvarchar(max) NOT NULL,
                        [AuthorId] nvarchar(450) NOT NULL,
                        [CreatedAt] datetime2 NOT NULL,
                        [UpdatedAt] datetime2 NULL,
                        CONSTRAINT [PK_Posts] PRIMARY KEY ([Id]),
                        CONSTRAINT [FK_Posts_AspNetUsers_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
                    );
                    CREATE INDEX [IX_Posts_AuthorId] ON [Posts] ([AuthorId]);
                END
            ");

            // Створюємо таблицю Recipes тільки якщо вона не існує
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Recipes')
                BEGIN
                    CREATE TABLE [Recipes] (
                        [Id] int NOT NULL IDENTITY,
                        [Name] nvarchar(200) NOT NULL,
                        [Description] nvarchar(2000) NOT NULL,
                        [Category] nvarchar(50) NOT NULL,
                        [Difficulty] nvarchar(20) NOT NULL,
                        [PreparationTime] int NOT NULL,
                        [Servings] int NOT NULL,
                        [ImageUrl] nvarchar(500) NULL,
                        [DietType] nvarchar(50) NULL,
                        [Cuisine] nvarchar(50) NULL,
                        [BudgetLevel] nvarchar(20) NULL,
                        [Calories] int NULL,
                        [IsPremium] bit NOT NULL,
                        [IsPublic] bit NOT NULL,
                        [AuthorId] nvarchar(450) NOT NULL,
                        [CreatedAt] datetime2 NOT NULL,
                        [UpdatedAt] datetime2 NULL,
                        CONSTRAINT [PK_Recipes] PRIMARY KEY ([Id]),
                        CONSTRAINT [FK_Recipes_AspNetUsers_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
                    );
                    CREATE INDEX [IX_Recipes_AuthorId] ON [Recipes] ([AuthorId]);
                END
            ");

            // Додаємо відсутні колонки до існуючої таблиці Recipes, якщо вони не існують
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

            // Створюємо таблицю ShoppingListItems тільки якщо вона не існує
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ShoppingListItems')
                BEGIN
                    CREATE TABLE [ShoppingListItems] (
                        [Id] int NOT NULL IDENTITY,
                        [Name] nvarchar(200) NOT NULL,
                        [Quantity] nvarchar(50) NULL,
                        [Unit] nvarchar(50) NULL,
                        [UserId] nvarchar(450) NOT NULL,
                        [IsBought] bit NOT NULL,
                        [CreatedAt] datetime2 NOT NULL,
                        CONSTRAINT [PK_ShoppingListItems] PRIMARY KEY ([Id]),
                        CONSTRAINT [FK_ShoppingListItems_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
                    );
                    CREATE INDEX [IX_ShoppingListItems_UserId] ON [ShoppingListItems] ([UserId]);
                END
            ");

            // Створюємо таблицю Comment тільки якщо вона не існує
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Comment')
                BEGIN
                    CREATE TABLE [Comment] (
                        [Id] int NOT NULL IDENTITY,
                        [PostId] int NOT NULL,
                        [AuthorId] nvarchar(450) NOT NULL,
                        [Content] nvarchar(max) NOT NULL,
                        [CreatedAt] datetime2 NOT NULL,
                        CONSTRAINT [PK_Comment] PRIMARY KEY ([Id]),
                        CONSTRAINT [FK_Comment_AspNetUsers_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
                        CONSTRAINT [FK_Comment_Posts_PostId] FOREIGN KEY ([PostId]) REFERENCES [Posts] ([Id]) ON DELETE CASCADE
                    );
                    CREATE INDEX [IX_Comment_AuthorId] ON [Comment] ([AuthorId]);
                    CREATE INDEX [IX_Comment_PostId] ON [Comment] ([PostId]);
                END
            ");

            // Створюємо таблицю Ingredients тільки якщо вона не існує
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Ingredients')
                BEGIN
                    CREATE TABLE [Ingredients] (
                        [Id] int NOT NULL IDENTITY,
                        [Name] nvarchar(200) NOT NULL,
                        [Quantity] nvarchar(50) NULL,
                        [Unit] nvarchar(50) NULL,
                        [RecipeId] int NOT NULL,
                        CONSTRAINT [PK_Ingredients] PRIMARY KEY ([Id]),
                        CONSTRAINT [FK_Ingredients_Recipes_RecipeId] FOREIGN KEY ([RecipeId]) REFERENCES [Recipes] ([Id]) ON DELETE CASCADE
                    );
                    CREATE INDEX [IX_Ingredients_RecipeId] ON [Ingredients] ([RecipeId]);
                END
            ");

            // Створюємо таблицю RecipeComments тільки якщо вона не існує
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RecipeComments')
                BEGIN
                    CREATE TABLE [RecipeComments] (
                        [Id] int NOT NULL IDENTITY,
                        [Content] nvarchar(2000) NOT NULL,
                        [RecipeId] int NOT NULL,
                        [AuthorId] nvarchar(450) NOT NULL,
                        [CreatedAt] datetime2 NOT NULL,
                        [UpdatedAt] datetime2 NULL,
                        CONSTRAINT [PK_RecipeComments] PRIMARY KEY ([Id]),
                        CONSTRAINT [FK_RecipeComments_AspNetUsers_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
                        CONSTRAINT [FK_RecipeComments_Recipes_RecipeId] FOREIGN KEY ([RecipeId]) REFERENCES [Recipes] ([Id]) ON DELETE CASCADE
                    );
                    CREATE INDEX [IX_RecipeComments_AuthorId] ON [RecipeComments] ([AuthorId]);
                    CREATE INDEX [IX_RecipeComments_RecipeId] ON [RecipeComments] ([RecipeId]);
                END
            ");

            // Створюємо таблицю RecipeFavorites тільки якщо вона не існує
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RecipeFavorites')
                BEGIN
                    CREATE TABLE [RecipeFavorites] (
                        [Id] int NOT NULL IDENTITY,
                        [RecipeId] int NOT NULL,
                        [UserId] nvarchar(450) NOT NULL,
                        [CreatedAt] datetime2 NOT NULL,
                        CONSTRAINT [PK_RecipeFavorites] PRIMARY KEY ([Id]),
                        CONSTRAINT [FK_RecipeFavorites_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
                        CONSTRAINT [FK_RecipeFavorites_Recipes_RecipeId] FOREIGN KEY ([RecipeId]) REFERENCES [Recipes] ([Id]) ON DELETE CASCADE
                    );
                    CREATE UNIQUE INDEX [IX_RecipeFavorites_RecipeId_UserId] ON [RecipeFavorites] ([RecipeId], [UserId]);
                    CREATE INDEX [IX_RecipeFavorites_UserId] ON [RecipeFavorites] ([UserId]);
                END
            ");

            // Створюємо таблицю RecipeRatings тільки якщо вона не існує
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RecipeRatings')
                BEGIN
                    CREATE TABLE [RecipeRatings] (
                        [Id] int NOT NULL IDENTITY,
                        [Rating] int NOT NULL,
                        [RecipeId] int NOT NULL,
                        [UserId] nvarchar(450) NOT NULL,
                        [CreatedAt] datetime2 NOT NULL,
                        [UpdatedAt] datetime2 NULL,
                        CONSTRAINT [PK_RecipeRatings] PRIMARY KEY ([Id]),
                        CONSTRAINT [FK_RecipeRatings_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
                        CONSTRAINT [FK_RecipeRatings_Recipes_RecipeId] FOREIGN KEY ([RecipeId]) REFERENCES [Recipes] ([Id]) ON DELETE CASCADE
                    );
                    CREATE UNIQUE INDEX [IX_RecipeRatings_RecipeId_UserId] ON [RecipeRatings] ([RecipeId], [UserId]);
                    CREATE INDEX [IX_RecipeRatings_UserId] ON [RecipeRatings] ([UserId]);
                END
            ");

            // Всі індекси вже створені в SQL блоках вище
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "RecipeComments");

            migrationBuilder.DropTable(
                name: "RecipeFavorites");

            migrationBuilder.DropTable(
                name: "RecipeRatings");

            migrationBuilder.DropTable(
                name: "ShoppingListItems");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
