using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorldsBelly.DataAccess.Migrations
{
    public partial class changerating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeCalculatedRatings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeRatings",
                table: "RecipeRatings");

            migrationBuilder.DropIndex(
                name: "IX_RecipeRatings_RecipeId",
                table: "RecipeRatings");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RecipeRatings");

            migrationBuilder.DropColumn(
                name: "Bitter",
                table: "RecipeRatings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "RecipeRatings");

            migrationBuilder.DropColumn(
                name: "Flavor",
                table: "RecipeRatings");

            migrationBuilder.DropColumn(
                name: "Salty",
                table: "RecipeRatings");

            migrationBuilder.DropColumn(
                name: "Sour",
                table: "RecipeRatings");

            migrationBuilder.DropColumn(
                name: "Spices",
                table: "RecipeRatings");

            migrationBuilder.DropColumn(
                name: "Sweet",
                table: "RecipeRatings");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "RecipeRatings");

            migrationBuilder.AddColumn<double>(
                name: "Bitter",
                table: "Recipes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CalculatedRating",
                table: "Recipes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "CalculatedTotalRatings",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Flavor",
                table: "Recipes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Salty",
                table: "Recipes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Sour",
                table: "Recipes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Spices",
                table: "Recipes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Sweet",
                table: "Recipes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "Rating",
                table: "RecipeRatings",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeRatings",
                table: "RecipeRatings",
                columns: new[] { "RecipeId", "UserId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeRatings",
                table: "RecipeRatings");

            migrationBuilder.DropColumn(
                name: "Bitter",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "CalculatedRating",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "CalculatedTotalRatings",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Flavor",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Salty",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Sour",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Spices",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Sweet",
                table: "Recipes");

            migrationBuilder.AlterColumn<double>(
                name: "Rating",
                table: "RecipeRatings",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "RecipeRatings",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<double>(
                name: "Bitter",
                table: "RecipeRatings",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "RecipeRatings",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<double>(
                name: "Flavor",
                table: "RecipeRatings",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Salty",
                table: "RecipeRatings",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Sour",
                table: "RecipeRatings",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Spices",
                table: "RecipeRatings",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Sweet",
                table: "RecipeRatings",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "RecipeRatings",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeRatings",
                table: "RecipeRatings",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "RecipeCalculatedRatings",
                columns: table => new
                {
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    Bitter = table.Column<double>(type: "float", nullable: false),
                    Flavor = table.Column<double>(type: "float", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    Salty = table.Column<double>(type: "float", nullable: false),
                    Sour = table.Column<double>(type: "float", nullable: false),
                    Spices = table.Column<double>(type: "float", nullable: false),
                    Sweet = table.Column<double>(type: "float", nullable: false),
                    TotalFlavorRatings = table.Column<int>(type: "int", nullable: false),
                    TotalRatings = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeCalculatedRatings", x => x.RecipeId);
                    table.ForeignKey(
                        name: "FK_RecipeCalculatedRatings_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeRatings_RecipeId",
                table: "RecipeRatings",
                column: "RecipeId");
        }
    }
}
