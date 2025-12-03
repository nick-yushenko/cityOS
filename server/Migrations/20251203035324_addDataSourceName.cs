using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityOS.Migrations
{
    /// <inheritdoc />
    public partial class addDataSourceName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "data_source",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "data_source");
        }
    }
}
