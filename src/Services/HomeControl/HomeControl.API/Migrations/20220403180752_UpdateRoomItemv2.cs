using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeControl.API.Migrations
{
    public partial class UpdateRoomItemv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Items",
                type: "text",
                nullable: true);
             
            migrationBuilder.AddForeignKey(
                name: "FK_Items_Rooms_RoomId",
                table: "Items",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Rooms_RoomId",
                table: "Items");
            

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Items");
        }
    }
}
