using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logging.API.Migrations
{
    public partial class UpdateDHTWithSensorName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Topic",
                table: "DHTs",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Topic",
                table: "DHTs");
        }
    }
}
