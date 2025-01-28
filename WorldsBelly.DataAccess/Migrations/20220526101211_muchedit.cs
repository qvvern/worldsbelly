using Microsoft.EntityFrameworkCore.Migrations;

namespace WorldsBelly.DataAccess.Migrations
{
    public partial class muchedit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LimitedToRecipe",
                table: "Tags",
                newName: "IncludeAlways");

            migrationBuilder.RenameColumn(
                name: "LimitedToIngredient",
                table: "Tags",
                newName: "Hidden");

            migrationBuilder.AddColumn<bool>(
                name: "ExcludeAlways",
                table: "Tags",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AmountPerPortionDefaultMeasurement",
                table: "RecipeIngredients",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExcludeAlways",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "AmountPerPortionDefaultMeasurement",
                table: "RecipeIngredients");

            migrationBuilder.RenameColumn(
                name: "IncludeAlways",
                table: "Tags",
                newName: "LimitedToRecipe");

            migrationBuilder.RenameColumn(
                name: "Hidden",
                table: "Tags",
                newName: "LimitedToIngredient");
        }
    }
}
