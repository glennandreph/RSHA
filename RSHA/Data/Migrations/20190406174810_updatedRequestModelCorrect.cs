using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RSHA.Data.Migrations
{
    public partial class updatedRequestModelCorrect : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequestScheduled",
                table: "Requests",
                newName: "RequestScheduledTime");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Requests",
                newName: "PhoneNumber");

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestScheduledDate",
                table: "Requests",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestScheduledDate",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "RequestScheduledTime",
                table: "Requests",
                newName: "RequestScheduled");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Requests",
                newName: "Phone");
        }
    }
}
