using Microsoft.EntityFrameworkCore.Migrations;

namespace WorldsBelly.DataAccess.Migrations
{
    public partial class deletecomment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentTranslations_Comments_CommentId",
                table: "CommentTranslations");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentTranslations_Comments_CommentId",
                table: "CommentTranslations",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentTranslations_Comments_CommentId",
                table: "CommentTranslations");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentTranslations_Comments_CommentId",
                table: "CommentTranslations",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");
        }
    }
}
