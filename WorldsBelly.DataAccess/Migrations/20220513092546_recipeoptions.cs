using Microsoft.EntityFrameworkCore.Migrations;

namespace WorldsBelly.DataAccess.Migrations
{
    public partial class recipeoptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Difficulty",
                table: "Recipes",
                newName: "DifficultyId");

            migrationBuilder.AddColumn<int>(
                name: "AgeGroupId",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BestServedId",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConsumerId",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RecipeAgeGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeAgeGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeAgeGroup_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RecipeBestServed",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeBestServed", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeBestServed_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RecipeConsumer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeConsumer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeConsumer_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RecipeDifficulty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeDifficulty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeDifficulty_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_AgeGroupId",
                table: "Recipes",
                column: "AgeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_BestServedId",
                table: "Recipes",
                column: "BestServedId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ConsumerId",
                table: "Recipes",
                column: "ConsumerId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_DifficultyId",
                table: "Recipes",
                column: "DifficultyId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeAgeGroup_TagId",
                table: "RecipeAgeGroup",
                column: "TagId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeBestServed_TagId",
                table: "RecipeBestServed",
                column: "TagId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeConsumer_TagId",
                table: "RecipeConsumer",
                column: "TagId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeDifficulty_TagId",
                table: "RecipeDifficulty",
                column: "TagId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_RecipeAgeGroup_AgeGroupId",
                table: "Recipes",
                column: "AgeGroupId",
                principalTable: "RecipeAgeGroup",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_RecipeBestServed_BestServedId",
                table: "Recipes",
                column: "BestServedId",
                principalTable: "RecipeBestServed",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_RecipeConsumer_ConsumerId",
                table: "Recipes",
                column: "ConsumerId",
                principalTable: "RecipeConsumer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_RecipeDifficulty_DifficultyId",
                table: "Recipes",
                column: "DifficultyId",
                principalTable: "RecipeDifficulty",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_RecipeAgeGroup_AgeGroupId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_RecipeBestServed_BestServedId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_RecipeConsumer_ConsumerId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_RecipeDifficulty_DifficultyId",
                table: "Recipes");

            migrationBuilder.DropTable(
                name: "RecipeAgeGroup");

            migrationBuilder.DropTable(
                name: "RecipeBestServed");

            migrationBuilder.DropTable(
                name: "RecipeConsumer");

            migrationBuilder.DropTable(
                name: "RecipeDifficulty");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_AgeGroupId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_BestServedId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_ConsumerId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_DifficultyId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "AgeGroupId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "BestServedId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "ConsumerId",
                table: "Recipes");

            migrationBuilder.RenameColumn(
                name: "DifficultyId",
                table: "Recipes",
                newName: "Difficulty");
        }
    }
}
