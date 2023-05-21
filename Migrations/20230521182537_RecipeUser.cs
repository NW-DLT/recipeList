using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace recipeList.Migrations
{
    /// <inheritdoc />
    public partial class RecipeUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recipes_AspNetUsers_UserId",
                table: "recipes");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "recipes",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_recipes_UserId",
                table: "recipes",
                newName: "IX_recipes_userId");

            migrationBuilder.AddForeignKey(
                name: "FK_recipes_AspNetUsers_userId",
                table: "recipes",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recipes_AspNetUsers_userId",
                table: "recipes");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "recipes",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_recipes_userId",
                table: "recipes",
                newName: "IX_recipes_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_recipes_AspNetUsers_UserId",
                table: "recipes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
