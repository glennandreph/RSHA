using Microsoft.EntityFrameworkCore.Migrations;

namespace RSHA.Data.Migrations
{
    public partial class addForeignKeyToRequestWithMechanicsAndRequestViewModelAddedWithMechanicsTryAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Requests_MechanicAssigned",
                table: "Requests",
                column: "MechanicAssigned");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Mechanics_MechanicAssigned",
                table: "Requests",
                column: "MechanicAssigned",
                principalTable: "Mechanics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Mechanics_MechanicAssigned",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_MechanicAssigned",
                table: "Requests");
        }
    }
}
