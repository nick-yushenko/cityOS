using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CityOS.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "city",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_city", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "income_source",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    external_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_income_source", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "budget_version",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    city_id = table.Column<int>(type: "integer", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    revision_date = table.Column<DateTime>(type: "date", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "income_execution",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    revision_id = table.Column<int>(type: "integer", nullable: false),
                    income_source_id = table.Column<int>(type: "integer", nullable: false),
                    report_period = table.Column<DateTime>(type: "date", nullable: false),
                    executed_value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    execution_percent = table.Column<decimal>(type: "numeric(7,4)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_income_execution", x => x.id);
                    table.ForeignKey(
                        name: "FK_income_execution_budget_version_revision_id",
                        column: x => x.revision_id,
                        principalTable: "budget_version",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_income_execution_income_source_income_source_id",
                        column: x => x.income_source_id,
                        principalTable: "income_source",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "income_plan",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    revision_id = table.Column<int>(type: "integer", nullable: false),
                    income_source_id = table.Column<int>(type: "integer", nullable: false),
                    planned_value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    planned_share_percent = table.Column<decimal>(type: "numeric(7,4)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_income_plan", x => x.id);
                    table.ForeignKey(
                        name: "FK_income_plan_budget_version_revision_id",
                        column: x => x.revision_id,
                        principalTable: "budget_version",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_income_plan_income_source_income_source_id",
                        column: x => x.income_source_id,
                        principalTable: "income_source",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_budget_version_city_id",
                table: "budget_version",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "IX_income_execution_income_source_id",
                table: "income_execution",
                column: "income_source_id");

            migrationBuilder.CreateIndex(
                name: "ux_execution_revision_source_period",
                table: "income_execution",
                columns: new[] { "revision_id", "income_source_id", "report_period" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_income_plan_income_source_id",
                table: "income_plan",
                column: "income_source_id");

            migrationBuilder.CreateIndex(
                name: "ux_budget_income_revision_source",
                table: "income_plan",
                columns: new[] { "revision_id", "income_source_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "income_execution");

            migrationBuilder.DropTable(
                name: "income_plan");

            migrationBuilder.DropTable(
                name: "budget_version");

            migrationBuilder.DropTable(
                name: "income_source");

            migrationBuilder.DropTable(
                name: "city");
        }
    }
}
