using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace recipeList.Migrations
{
    /// <inheritdoc />
    public partial class LinqFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_recipes_Recipeid",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_Recipeid",
                table: "products");

            migrationBuilder.DropColumn(
                name: "Recipeid",
                table: "products");

            migrationBuilder.CreateTable(
                name: "ProductRecipe",
                columns: table => new
                {
                    Recipesid = table.Column<int>(type: "int", nullable: false),
                    productsid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRecipe", x => new { x.Recipesid, x.productsid });
                    table.ForeignKey(
                        name: "FK_ProductRecipe_products_productsid",
                        column: x => x.productsid,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductRecipe_recipes_Recipesid",
                        column: x => x.Recipesid,
                        principalTable: "recipes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecipe_productsid",
                table: "ProductRecipe",
                column: "productsid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductRecipe");

            migrationBuilder.AddColumn<int>(
                name: "Recipeid",
                table: "products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_products_Recipeid",
                table: "products",
                column: "Recipeid");

            migrationBuilder.AddForeignKey(
                name: "FK_products_recipes_Recipeid",
                table: "products",
                column: "Recipeid",
                principalTable: "recipes",
                principalColumn: "id");
        }
    }
}
