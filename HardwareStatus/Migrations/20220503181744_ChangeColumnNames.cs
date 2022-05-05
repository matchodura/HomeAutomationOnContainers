using Microsoft.EntityFrameworkCore.Migrations;

namespace HardwareStatus.API.Migrations
{
    public partial class ChangeColumnNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FriendlyName",
                table: "Devices");

            migrationBuilder.RenameColumn(
                name: "DeviceType",
                table: "Devices",
                newName: "HardwareType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HardwareType",
                table: "Devices",
                newName: "DeviceType");

            migrationBuilder.AddColumn<string>(
                name: "FriendlyName",
                table: "Devices",
                type: "text",
                nullable: true);
        }
    }
}
