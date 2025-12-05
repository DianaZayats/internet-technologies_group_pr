using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipeIdToPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_RecipeId",
                table: "Posts",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Recipes_RecipeId",
                table: "Posts",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Recipes_RecipeId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_RecipeId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "Posts");
        }
    }
}
