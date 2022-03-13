using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeControl.API.Migrations
{
    public partial class updatedb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Items_ItemId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Values_ValueId",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "ValueId",
                table: "Rooms",
                newName: "RoomValueId");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "Rooms",
                newName: "RoomItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_ValueId",
                table: "Rooms",
                newName: "IX_Rooms_RoomValueId");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_ItemId",
                table: "Rooms",
                newName: "IX_Rooms_RoomItemId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Items_RoomItemId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Values_RoomValueId",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "RoomValueId",
                table: "Rooms",
                newName: "ValueId");

            migrationBuilder.RenameColumn(
                name: "RoomItemId",
                table: "Rooms",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_RoomValueId",
                table: "Rooms",
                newName: "IX_Rooms_ValueId");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_RoomItemId",
                table: "Rooms",
                newName: "IX_Rooms_ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Items_ItemId",
                table: "Rooms",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Values_ValueId",
                table: "Rooms",
                column: "ValueId",
                principalTable: "Values",
                principalColumn: "Id");
        }
    }
}
