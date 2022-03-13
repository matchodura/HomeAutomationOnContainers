using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeControl.API.Migrations
{
    public partial class updatedb1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Items_RoomItemId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Values_RoomValueId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_RoomItemId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_RoomValueId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RoomItemId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RoomValueId",
                table: "Rooms");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomItemId",
                table: "Rooms",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomValueId",
                table: "Rooms",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomItemId",
                table: "Rooms",
                column: "RoomItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomValueId",
                table: "Rooms",
                column: "RoomValueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Items_RoomItemId",
                table: "Rooms",
                column: "RoomItemId",
                principalTable: "Items",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Values_RoomValueId",
                table: "Rooms",
                column: "RoomValueId",
                principalTable: "Values",
                principalColumn: "Id");
        }
    }
}
