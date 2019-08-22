using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RSHA.Data.Migrations
{
    public partial class updatedRequestModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "Requests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MechanicAssigned",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestCreated",
                table: "Requests",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestScheduled",
                table: "Requests",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "MechanicAssigned",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "RequestCreated",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "RequestScheduled",
                table: "Requests");
        }
    }
}
