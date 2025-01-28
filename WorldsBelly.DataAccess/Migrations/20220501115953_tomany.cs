using Microsoft.EntityFrameworkCore.Migrations;

namespace WorldsBelly.DataAccess.Migrations
{
    public partial class tomany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TagSelectableCategories_TextId",
                table: "TagSelectableCategories");

            migrationBuilder.AlterColumn<int>(
                name: "TextId",
                table: "TagSelectableCategories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_TagSelectableCategories_TextId",
                table: "TagSelectableCategories",
                column: "TextId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TagSelectableCategories_TextId",
                table: "TagSelectableCategories");

            migrationBuilder.AlterColumn<int>(
                name: "TextId",
                table: "TagSelectableCategories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TagSelectableCategories_TextId",
                table: "TagSelectableCategories",
                column: "TextId",
                unique: true);
        }
    }
}
