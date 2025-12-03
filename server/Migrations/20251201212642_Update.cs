using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CityOS.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_income_execution_budget_version_revision_id",
                table: "income_execution");

            migrationBuilder.DropForeignKey(
                name: "FK_income_plan_budget_version_revision_id",
                table: "income_plan");

            migrationBuilder.DropTable(
                name: "budget_version");

            migrationBuilder.RenameColumn(
                name: "planned_value",
                table: "income_plan",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "planned_share_percent",
                table: "income_plan",
                newName: "share_percent");

            migrationBuilder.RenameColumn(
                name: "executed_value",
                table: "income_execution",
                newName: "value");

            migrationBuilder.CreateTable(
                name: "budget",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    city_id = table.Column<int>(type: "integer", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_budget", x => x.id);
                    table.ForeignKey(
                        name: "FK_budget_city_city_id",
                        column: x => x.city_id,
                        principalTable: "city",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "budget_revision",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    budget_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    revision_date = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_budget_revision", x => x.id);
                    table.ForeignKey(
                        name: "FK_budget_revision_budget_budget_id",
                        column: x => x.budget_id,
                        principalTable: "budget",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ux_budget_city_year",
                table: "budget",
                columns: new[] { "city_id", "year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_budget_revision_budget_id",
                table: "budget_revision",
                column: "budget_id");

            migrationBuilder.AddForeignKey(
                name: "FK_income_execution_budget_revision_revision_id",
                table: "income_execution",
                column: "revision_id",
                principalTable: "budget_revision",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_income_plan_budget_revision_revision_id",
                table: "income_plan",
                column: "revision_id",
                principalTable: "budget_revision",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_income_execution_budget_revision_revision_id",
                table: "income_execution");

            migrationBuilder.DropForeignKey(
                name: "FK_income_plan_budget_revision_revision_id",
                table: "income_plan");

            migrationBuilder.DropTable(
                name: "budget_revision");

            migrationBuilder.DropTable(
                name: "budget");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "income_plan",
                newName: "planned_value");

            migrationBuilder.RenameColumn(
                name: "share_percent",
                table: "income_plan",
                newName: "planned_share_percent");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "income_execution",
                newName: "executed_value");

            migrationBuilder.CreateTable(
                name: "budget_version",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    city_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    revision_date = table.Column<DateTime>(type: "date", nullable: true),
                    year = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_budget_version", x => x.id);
                    table.ForeignKey(
                        name: "FK_budget_version_city_city_id",
                        column: x => x.city_id,
                        principalTable: "city",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_budget_version_city_id",
                table: "budget_version",
                column: "city_id");

            migrationBuilder.AddForeignKey(
                name: "FK_income_execution_budget_version_revision_id",
                table: "income_execution",
                column: "revision_id",
                principalTable: "budget_version",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_income_plan_budget_version_revision_id",
                table: "income_plan",
                column: "revision_id",
                principalTable: "budget_version",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
