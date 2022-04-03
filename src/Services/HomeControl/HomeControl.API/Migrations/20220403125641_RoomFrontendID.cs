using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeControl.API.Migrations
{
    public partial class RoomFrontendID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FrontendID",
                table: "Rooms",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FrontendID",
                table: "Rooms");
        }
    }
}
