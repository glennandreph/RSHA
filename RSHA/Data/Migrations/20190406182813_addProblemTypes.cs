using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RSHA.Data.Migrations
{
    public partial class addProblemTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Problem",
                table: "Requests");

            migrationBuilder.AddColumn<int>(
                name: "ProblemId",
                table: "Requests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProblemTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ProblemId",
                table: "Requests",
                column: "ProblemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_ProblemTypes_ProblemId",
                table: "Requests",
                column: "ProblemId",
                principalTable: "ProblemTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_ProblemTypes_ProblemId",
                table: "Requests");

            migrationBuilder.DropTable(
                name: "ProblemTypes");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ProblemId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ProblemId",
                table: "Requests");

            migrationBuilder.AddColumn<string>(
                name: "Problem",
                table: "Requests",
                nullable: true);
        }
    }
}
