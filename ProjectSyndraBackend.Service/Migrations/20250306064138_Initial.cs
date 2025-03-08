﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProjectSyndraBackend.Service.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrentDataParameters",
                columns: table => new
                {
                    CurrentDataParametersId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Patch = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentDataParameters", x => x.CurrentDataParametersId);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    MatchId = table.Column<string>(type: "text", nullable: false),
                    MatchDate = table.Column<long>(type: "bigint", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    Patch = table.Column<string>(type: "text", nullable: true),
                    EndOfGameResult = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.MatchId);
                });

            migrationBuilder.CreateTable(
                name: "Runes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PrimaryStyle = table.Column<int>(type: "integer", nullable: false),
                    SubStyle = table.Column<int>(type: "integer", nullable: false),
                    Perk0 = table.Column<int>(type: "integer", nullable: false),
                    Perk1 = table.Column<int>(type: "integer", nullable: false),
                    Perk2 = table.Column<int>(type: "integer", nullable: false),
                    Perk3 = table.Column<int>(type: "integer", nullable: false),
                    Perk4 = table.Column<int>(type: "integer", nullable: false),
                    Perk5 = table.Column<int>(type: "integer", nullable: false),
                    RuneVars0 = table.Column<int[]>(type: "integer[]", nullable: false),
                    RuneVars1 = table.Column<int[]>(type: "integer[]", nullable: false),
                    RuneVars2 = table.Column<int[]>(type: "integer[]", nullable: false),
                    RuneVars3 = table.Column<int[]>(type: "integer[]", nullable: false),
                    RuneVars4 = table.Column<int[]>(type: "integer[]", nullable: false),
                    RuneVars5 = table.Column<int[]>(type: "integer[]", nullable: false),
                    StatDefense = table.Column<int>(type: "integer", nullable: false),
                    StatFlex = table.Column<int>(type: "integer", nullable: false),
                    StatOffense = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Runes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Summoners",
                columns: table => new
                {
                    SummonerId = table.Column<string>(type: "text", nullable: false),
                    RiotSummonerId = table.Column<string>(type: "text", nullable: true),
                    SummonerName = table.Column<string>(type: "text", nullable: true),
                    ProfileIconId = table.Column<int>(type: "integer", nullable: false),
                    SummonerLevel = table.Column<long>(type: "bigint", nullable: false),
                    RevisionDate = table.Column<long>(type: "bigint", nullable: false),
                    Puuid = table.Column<string>(type: "text", nullable: true),
                    GameName = table.Column<string>(type: "text", nullable: true),
                    TagLine = table.Column<string>(type: "text", nullable: true),
                    AccountId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Summoners", x => x.SummonerId);
                });

            migrationBuilder.CreateTable(
                name: "MatchDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Kills = table.Column<int>(type: "integer", nullable: false),
                    Deaths = table.Column<int>(type: "integer", nullable: false),
                    Assists = table.Column<int>(type: "integer", nullable: false),
                    Win = table.Column<bool>(type: "boolean", nullable: false),
                    Lane = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    SummonerSpell1 = table.Column<int>(type: "integer", nullable: false),
                    SummonerSpell2 = table.Column<int>(type: "integer", nullable: false),
                    ChampionId = table.Column<int>(type: "integer", nullable: false),
                    ChampionName = table.Column<string>(type: "text", nullable: false),
                    Items = table.Column<List<int>>(type: "integer[]", nullable: false),
                    RunesId = table.Column<int>(type: "integer", nullable: false),
                    MatchId = table.Column<string>(type: "text", nullable: false),
                    SummonerId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchDetails_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "MatchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchDetails_Runes_RunesId",
                        column: x => x.RunesId,
                        principalTable: "Runes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchDetails_Summoners_SummonerId",
                        column: x => x.SummonerId,
                        principalTable: "Summoners",
                        principalColumn: "SummonerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchSummoners",
                columns: table => new
                {
                    MatchId = table.Column<string>(type: "text", nullable: false),
                    SummonerId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchSummoners", x => new { x.MatchId, x.SummonerId });
                    table.ForeignKey(
                        name: "FK_MatchSummoners_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "MatchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchSummoners_Summoners_SummonerId",
                        column: x => x.SummonerId,
                        principalTable: "Summoners",
                        principalColumn: "SummonerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchDetails_MatchId",
                table: "MatchDetails",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchDetails_RunesId",
                table: "MatchDetails",
                column: "RunesId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchDetails_SummonerId",
                table: "MatchDetails",
                column: "SummonerId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchSummoners_SummonerId",
                table: "MatchSummoners",
                column: "SummonerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrentDataParameters");

            migrationBuilder.DropTable(
                name: "MatchDetails");

            migrationBuilder.DropTable(
                name: "MatchSummoners");

            migrationBuilder.DropTable(
                name: "Runes");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Summoners");
        }
    }
}
