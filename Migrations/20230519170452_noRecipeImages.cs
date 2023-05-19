using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace recipeList.Migrations
{
    /// <inheritdoc />
    public partial class noRecipeImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_recipes_Recipeid",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_Recipeid",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "Recipeid",
                table: "Image");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Recipeid",
                table: "Image",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Image_Recipeid",
                table: "Image",
                column: "Recipeid");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_recipes_Recipeid",
                table: "Image",
                column: "Recipeid",
                principalTable: "recipes",
                principalColumn: "id");
        }
    }
}
