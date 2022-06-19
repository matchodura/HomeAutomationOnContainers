using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Network.API.Migrations
{
    public partial class DeviceKeyUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_MosquittoDevices_MosquittoDeviceId",
                table: "Devices");

            migrationBuilder.RenameColumn(
                name: "MosquittoDeviceId",
                table: "Devices",
                newName: "MosquittoDeviceID");

            migrationBuilder.RenameIndex(
                name: "IX_Devices_MosquittoDeviceId",
                table: "Devices",
                newName: "IX_Devices_MosquittoDeviceID");

            migrationBuilder.AlterColumn<int>(
                name: "MosquittoDeviceID",
                table: "Devices",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_MosquittoDevices_MosquittoDeviceID",
                table: "Devices",
                column: "MosquittoDeviceID",
                principalTable: "MosquittoDevices",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_MosquittoDevices_MosquittoDeviceID",
                table: "Devices");

            migrationBuilder.RenameColumn(
                name: "MosquittoDeviceID",
                table: "Devices",
                newName: "MosquittoDeviceId");

            migrationBuilder.RenameIndex(
                name: "IX_Devices_MosquittoDeviceID",
                table: "Devices",
                newName: "IX_Devices_MosquittoDeviceId");

            migrationBuilder.AlterColumn<int>(
                name: "MosquittoDeviceId",
                table: "Devices",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_MosquittoDevices_MosquittoDeviceId",
                table: "Devices",
                column: "MosquittoDeviceId",
                principalTable: "MosquittoDevices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
