using Microsoft.EntityFrameworkCore.Migrations;

namespace WorldsBelly.DataAccess.Migrations
{
    public partial class recipeCommentTranslations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentTranslations_Languages_LanguageId",
                table: "CommentTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentTranslations_RecipeComments_CommentId",
                table: "CommentTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentTranslations",
                table: "CommentTranslations");

            migrationBuilder.RenameTable(
                name: "CommentTranslations",
                newName: "RecipeCommentTranslations");

            migrationBuilder.RenameIndex(
                name: "IX_CommentTranslations_LanguageId",
                table: "RecipeCommentTranslations",
                newName: "IX_RecipeCommentTranslations_LanguageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeCommentTranslations",
                table: "RecipeCommentTranslations",
                columns: new[] { "CommentId", "LanguageId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeCommentTranslations_Languages_LanguageId",
                table: "RecipeCommentTranslations",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeCommentTranslations_RecipeComments_CommentId",
                table: "RecipeCommentTranslations",
                column: "CommentId",
                principalTable: "RecipeComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeCommentTranslations_Languages_LanguageId",
                table: "RecipeCommentTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeCommentTranslations_RecipeComments_CommentId",
                table: "RecipeCommentTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeCommentTranslations",
                table: "RecipeCommentTranslations");

            migrationBuilder.RenameTable(
                name: "RecipeCommentTranslations",
                newName: "CommentTranslations");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeCommentTranslations_LanguageId",
                table: "CommentTranslations",
                newName: "IX_CommentTranslations_LanguageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentTranslations",
                table: "CommentTranslations",
                columns: new[] { "CommentId", "LanguageId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CommentTranslations_Languages_LanguageId",
                table: "CommentTranslations",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentTranslations_RecipeComments_CommentId",
                table: "CommentTranslations",
                column: "CommentId",
                principalTable: "RecipeComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
