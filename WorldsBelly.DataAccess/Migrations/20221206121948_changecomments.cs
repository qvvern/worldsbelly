using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorldsBelly.DataAccess.Migrations
{
    public partial class changecomments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentTranslations_Comments_CommentId",
                table: "CommentTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeComments_Comments_CommentId",
                table: "RecipeComments");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeComments",
                table: "RecipeComments");

            migrationBuilder.DropIndex(
                name: "IX_RecipeComments_CommentId",
                table: "RecipeComments");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "RecipeComments",
                newName: "RootId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "RecipeComments",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "RecipeComments",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "RecipeComments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "RecipeComments",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedByUserId",
                table: "RecipeComments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "RecipeComments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedAt",
                table: "RecipeComments",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedByUserId",
                table: "RecipeComments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentCommentId",
                table: "RecipeComments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "RecipeComments",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeComments",
                table: "RecipeComments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeComments_CreatedByUserId",
                table: "RecipeComments",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeComments_DeletedByUserId",
                table: "RecipeComments",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeComments_ModifiedByUserId",
                table: "RecipeComments",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeComments_RecipeId",
                table: "RecipeComments",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeComments_UserId",
                table: "RecipeComments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentTranslations_RecipeComments_CommentId",
                table: "CommentTranslations",
                column: "CommentId",
                principalTable: "RecipeComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeComments_Users_CreatedByUserId",
                table: "RecipeComments",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeComments_Users_DeletedByUserId",
                table: "RecipeComments",
                column: "DeletedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeComments_Users_ModifiedByUserId",
                table: "RecipeComments",
                column: "ModifiedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeComments_Users_UserId",
                table: "RecipeComments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentTranslations_RecipeComments_CommentId",
                table: "CommentTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeComments_Users_CreatedByUserId",
                table: "RecipeComments");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeComments_Users_DeletedByUserId",
                table: "RecipeComments");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeComments_Users_ModifiedByUserId",
                table: "RecipeComments");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeComments_Users_UserId",
                table: "RecipeComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeComments",
                table: "RecipeComments");

            migrationBuilder.DropIndex(
                name: "IX_RecipeComments_CreatedByUserId",
                table: "RecipeComments");

            migrationBuilder.DropIndex(
                name: "IX_RecipeComments_DeletedByUserId",
                table: "RecipeComments");

            migrationBuilder.DropIndex(
                name: "IX_RecipeComments_ModifiedByUserId",
                table: "RecipeComments");

            migrationBuilder.DropIndex(
                name: "IX_RecipeComments_RecipeId",
                table: "RecipeComments");

            migrationBuilder.DropIndex(
                name: "IX_RecipeComments_UserId",
                table: "RecipeComments");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RecipeComments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "RecipeComments");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "RecipeComments");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "RecipeComments");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "RecipeComments");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "RecipeComments");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "RecipeComments");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "RecipeComments");

            migrationBuilder.DropColumn(
                name: "ParentCommentId",
                table: "RecipeComments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RecipeComments");

            migrationBuilder.RenameColumn(
                name: "RootId",
                table: "RecipeComments",
                newName: "CommentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeComments",
                table: "RecipeComments",
                columns: new[] { "RecipeId", "CommentId" });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "getdate()"),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedByUserId = table.Column<int>(type: "int", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ParentCommentId = table.Column<int>(type: "int", nullable: true),
                    RootId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeComments_CommentId",
                table: "RecipeComments",
                column: "CommentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CreatedByUserId",
                table: "Comments",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_DeletedByUserId",
                table: "Comments",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ModifiedByUserId",
                table: "Comments",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentTranslations_Comments_CommentId",
                table: "CommentTranslations",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeComments_Comments_CommentId",
                table: "RecipeComments",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
