using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorldsBelly.DataAccess.Migrations
{
    public partial class reciperating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalSaved",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalViews",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "RecipeRatings",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "RecipeRatings",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalFlavorRatings",
                table: "RecipeCalculatedRatings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalRatings",
                table: "RecipeCalculatedRatings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalSaved",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "TotalViews",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "RecipeRatings");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "RecipeRatings");

            migrationBuilder.DropColumn(
                name: "TotalFlavorRatings",
                table: "RecipeCalculatedRatings");

            migrationBuilder.DropColumn(
                name: "TotalRatings",
                table: "RecipeCalculatedRatings");
        }
    }
}
