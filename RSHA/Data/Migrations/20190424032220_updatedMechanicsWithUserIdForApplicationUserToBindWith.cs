using Microsoft.EntityFrameworkCore.Migrations;

namespace RSHA.Data.Migrations
{
    public partial class updatedMechanicsWithUserIdForApplicationUserToBindWith : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Mechanics",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mechanics_UserId",
                table: "Mechanics",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mechanics_AspNetUsers_UserId",
                table: "Mechanics",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mechanics_AspNetUsers_UserId",
                table: "Mechanics");

            migrationBuilder.DropIndex(
                name: "IX_Mechanics_UserId",
                table: "Mechanics");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Mechanics");
        }
    }
}
