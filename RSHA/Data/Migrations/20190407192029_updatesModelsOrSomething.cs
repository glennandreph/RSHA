using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RSHA.Data.Migrations
{
    public partial class updatesModelsOrSomething : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestScheduledTime",
                table: "Requests");

            migrationBuilder.AlterColumn<string>(
                name: "CarLicensePlate",
                table: "Requests",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CarLicensePlate",
                table: "Requests",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestScheduledTime",
                table: "Requests",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
