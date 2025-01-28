using Microsoft.EntityFrameworkCore.Migrations;

namespace WorldsBelly.DataAccess.Migrations
{
    public partial class dbup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PortionAmount",
                table: "Recipes",
                newName: "ServingAmount");

            migrationBuilder.RenameColumn(
                name: "AmountPerPortionDefaultMeasurement",
                table: "RecipeIngredients",
                newName: "AmountPerServingDefaultNutrientMeasurement");

            migrationBuilder.AddColumn<double>(
                name: "CalculatedServingWeight",
                table: "Recipes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Measurements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "Ingredients",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Countries",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalculatedServingWeight",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Countries");

            migrationBuilder.RenameColumn(
                name: "ServingAmount",
                table: "Recipes",
                newName: "PortionAmount");

            migrationBuilder.RenameColumn(
                name: "AmountPerServingDefaultNutrientMeasurement",
                table: "RecipeIngredients",
                newName: "AmountPerPortionDefaultMeasurement");
        }
    }
}
