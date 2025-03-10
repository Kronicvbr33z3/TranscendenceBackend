using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProjectSyndraBackend.Service.Migrations
{
    /// <inheritdoc />
    public partial class AnalysisModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrentChampionLoadouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChampionName = table.Column<string>(type: "text", nullable: false),
                    ChampionId = table.Column<int>(type: "integer", nullable: false),
                    Lane = table.Column<string>(type: "text", nullable: false),
                    Rank = table.Column<string>(type: "text", nullable: false),
                    Patch = table.Column<string>(type: "text", nullable: false),
                    QueueType = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentChampionLoadouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnitWinPercent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NumberOfGames = table.Column<int>(type: "integer", nullable: false),
                    WinRate = table.Column<float>(type: "real", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    CurrentChampionLoadoutId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitWinPercent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitWinPercent_CurrentChampionLoadouts_CurrentChampionLoado~",
                        column: x => x.CurrentChampionLoadoutId,
                        principalTable: "CurrentChampionLoadouts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnitWinPercent_CurrentChampionLoadoutId",
                table: "UnitWinPercent",
                column: "CurrentChampionLoadoutId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnitWinPercent");

            migrationBuilder.DropTable(
                name: "CurrentChampionLoadouts");
        }
    }
}
