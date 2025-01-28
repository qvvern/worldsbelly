using Microsoft.EntityFrameworkCore.Migrations;

namespace WorldsBelly.DataAccess.Migrations
{
    public partial class approvalcreatedby : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "RecipeApprovals",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "RecipeApprovals");
        }
    }
}
