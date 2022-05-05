using Microsoft.EntityFrameworkCore.Migrations;

namespace HardwareStatus.API.Migrations
{
    public partial class AddFriendlyName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FriendlyName",
                table: "Devices",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FriendlyName",
                table: "Devices");
        }
    }
}
