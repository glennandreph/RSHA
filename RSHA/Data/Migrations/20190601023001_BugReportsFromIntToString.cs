using Microsoft.EntityFrameworkCore.Migrations;

namespace RSHA.Data.Migrations
{
    public partial class BugReportsFromIntToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "BugRapports",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "BugRapports",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
