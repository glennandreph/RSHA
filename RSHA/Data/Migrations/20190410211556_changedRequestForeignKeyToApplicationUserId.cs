using Microsoft.EntityFrameworkCore.Migrations;

namespace RSHA.Data.Migrations
{
    public partial class changedRequestForeignKeyToApplicationUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "Requests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CustomerId",
                table: "Requests",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_CustomerId",
                table: "Requests",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_CustomerId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_CustomerId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Requests");
        }
    }
}
