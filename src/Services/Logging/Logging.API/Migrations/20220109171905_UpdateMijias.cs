using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logging.API.Migrations
{
    public partial class UpdateMijias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "Mijias",
                newName: "TimestampLinux");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeStampUTC",
                table: "Mijias",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeStampUTC",
                table: "Mijias");

            migrationBuilder.RenameColumn(
                name: "TimestampLinux",
                table: "Mijias",
                newName: "Timestamp");
        }
    }
}
