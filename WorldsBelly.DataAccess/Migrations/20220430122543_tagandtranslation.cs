using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorldsBelly.DataAccess.Migrations
{
    public partial class tagandtranslation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelatedTagId",
                table: "TagSelectables");

            migrationBuilder.RenameColumn(
                name: "StepId",
                table: "TagSelectables",
                newName: "OrderIndex");

            migrationBuilder.AddColumn<bool>(
                name: "DontAddTag",
                table: "TagSelectables",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ExcludedTags",
                table: "TagSelectables",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EnglishTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextPlural = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnglishTranslations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TagSelectableCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleId = table.Column<int>(type: "int", nullable: false),
                    TextId = table.Column<int>(type: "int", nullable: false),
                    ExcludedTags = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagSelectableCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TagSelectableCategories_EnglishTranslations_TextId",
                        column: x => x.TextId,
                        principalTable: "EnglishTranslations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TagSelectableCategories_EnglishTranslations_TitleId",
                        column: x => x.TitleId,
                        principalTable: "EnglishTranslations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Translations",
                columns: table => new
                {
                    EnglishTranslationId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextPlural = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translations", x => new { x.EnglishTranslationId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_Translations_EnglishTranslations_EnglishTranslationId",
                        column: x => x.EnglishTranslationId,
                        principalTable: "EnglishTranslations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Translations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TagSelectableCategoryToTagSelectable",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    TagSelectablesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagSelectableCategoryToTagSelectable", x => new { x.CategoriesId, x.TagSelectablesId });
                    table.ForeignKey(
                        name: "FK_TagSelectableCategoryToTagSelectable_TagSelectableCategories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "TagSelectableCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagSelectableCategoryToTagSelectable_TagSelectables_TagSelectablesId",
                        column: x => x.TagSelectablesId,
                        principalTable: "TagSelectables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TagSelectableChoiceOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagId = table.Column<int>(type: "int", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    TagSelectableCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagSelectableChoiceOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TagSelectableChoiceOrders_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TagSelectableChoiceOrders_TagSelectableCategories_TagSelectableCategoryId",
                        column: x => x.TagSelectableCategoryId,
                        principalTable: "TagSelectableCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TagSelectableCategories_TextId",
                table: "TagSelectableCategories",
                column: "TextId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TagSelectableCategories_TitleId",
                table: "TagSelectableCategories",
                column: "TitleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TagSelectableCategoryToTagSelectable_TagSelectablesId",
                table: "TagSelectableCategoryToTagSelectable",
                column: "TagSelectablesId");

            migrationBuilder.CreateIndex(
                name: "IX_TagSelectableChoiceOrders_TagId",
                table: "TagSelectableChoiceOrders",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_TagSelectableChoiceOrders_TagSelectableCategoryId",
                table: "TagSelectableChoiceOrders",
                column: "TagSelectableCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_LanguageId",
                table: "Translations",
                column: "LanguageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TagSelectableCategoryToTagSelectable");

            migrationBuilder.DropTable(
                name: "TagSelectableChoiceOrders");

            migrationBuilder.DropTable(
                name: "Translations");

            migrationBuilder.DropTable(
                name: "TagSelectableCategories");

            migrationBuilder.DropTable(
                name: "EnglishTranslations");

            migrationBuilder.DropColumn(
                name: "DontAddTag",
                table: "TagSelectables");

            migrationBuilder.DropColumn(
                name: "ExcludedTags",
                table: "TagSelectables");

            migrationBuilder.RenameColumn(
                name: "OrderIndex",
                table: "TagSelectables",
                newName: "StepId");

            migrationBuilder.AddColumn<int>(
                name: "RelatedTagId",
                table: "TagSelectables",
                type: "int",
                nullable: true);
        }
    }
}
