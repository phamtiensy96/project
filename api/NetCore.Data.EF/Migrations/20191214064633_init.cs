using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCore.Data.EF.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FunctionId",
                table: "Permissions",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AppUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "AppUsers");

            migrationBuilder.AlterColumn<string>(
                name: "FunctionId",
                table: "Permissions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);
        }
    }
}
