using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace recipeList.Migrations
{
    /// <inheritdoc />
    public partial class UserRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "recipes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_recipes_UserId",
                table: "recipes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_recipes_AspNetUsers_UserId",
                table: "recipes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recipes_AspNetUsers_UserId",
                table: "recipes");

            migrationBuilder.DropIndex(
                name: "IX_recipes_UserId",
                table: "recipes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "recipes");
        }
    }
}
