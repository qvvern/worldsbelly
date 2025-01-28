using Microsoft.EntityFrameworkCore.Migrations;

namespace WorldsBelly.DataAccess.Migrations
{
    public partial class revertgtomg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OnePieceInMilligram",
                table: "Ingredients",
                newName: "OnePieceInGram");

            migrationBuilder.RenameColumn(
                name: "OneMilliliterInMilligram",
                table: "Ingredients",
                newName: "OneMilliliterInGram");

            migrationBuilder.RenameColumn(
                name: "OneCentimeterInMilligram",
                table: "Ingredients",
                newName: "OneCentimeterInGram");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OnePieceInGram",
                table: "Ingredients",
                newName: "OnePieceInMilligram");

            migrationBuilder.RenameColumn(
                name: "OneMilliliterInGram",
                table: "Ingredients",
                newName: "OneMilliliterInMilligram");

            migrationBuilder.RenameColumn(
                name: "OneCentimeterInGram",
                table: "Ingredients",
                newName: "OneCentimeterInMilligram");
        }
    }
}
