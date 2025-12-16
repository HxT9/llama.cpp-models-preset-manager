using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace llama.cpp_models_preset_manager.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AIModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Path = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Flag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KVConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KVConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AIModelFlag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AiModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    Flag = table.Column<string>(type: "TEXT", nullable: false),
                    FlagValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIModelFlag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AIModelFlag_AIModel_AiModelId",
                        column: x => x.AiModelId,
                        principalTable: "AIModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AIModel_Name",
                table: "AIModel",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AIModelFlag_AiModelId",
                table: "AIModelFlag",
                column: "AiModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Flag_Name",
                table: "Flag",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KVConfig_Key",
                table: "KVConfig",
                column: "Key",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AIModelFlag");

            migrationBuilder.DropTable(
                name: "Flag");

            migrationBuilder.DropTable(
                name: "KVConfig");

            migrationBuilder.DropTable(
                name: "AIModel");
        }
    }
}
