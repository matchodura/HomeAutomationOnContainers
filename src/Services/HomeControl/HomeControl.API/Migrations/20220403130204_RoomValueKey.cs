using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeControl.API.Migrations
{
    public partial class RoomValueKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Values_RoomId",
                table: "Values",
                column: "RoomId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Values_Rooms_RoomId",
                table: "Values",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Values_Rooms_RoomId",
                table: "Values");

            migrationBuilder.DropIndex(
                name: "IX_Values_RoomId",
                table: "Values");
        }
    }
}
