using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CityOS.Migrations
{
    /// <inheritdoc />
    public partial class AddDataSourceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "income_plan",
                type: "timestamp",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "income_plan",
                type: "timestamp",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "income_execution",
                type: "timestamp",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "income_execution",
                type: "timestamp",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "data_source",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    city_id = table.Column<int>(type: "integer", nullable: false),
                    dataset_kind = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    source_url = table.Column<string>(type: "text", nullable: false),
                    file_type = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    last_loaded_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    last_checksum = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_source", x => x.id);
                    table.ForeignKey(
                        name: "FK_data_source_city_city_id",
                        column: x => x.city_id,
                        principalTable: "city",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_data_source_city_id",
                table: "data_source",
                column: "city_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "data_source");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "income_plan");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "income_plan");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "income_execution");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "income_execution");
        }
    }
}
