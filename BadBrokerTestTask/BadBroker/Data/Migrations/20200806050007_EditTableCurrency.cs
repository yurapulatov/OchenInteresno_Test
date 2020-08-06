using Microsoft.EntityFrameworkCore.Migrations;

namespace BadBroker.Data.Migrations
{
    public partial class EditTableCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "currency",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "access_base",
                table: "currency",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "access_result",
                table: "currency",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_currency_code",
                table: "currency",
                column: "code");
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_currency_code",
                table: "currency");

            migrationBuilder.DropColumn(
                name: "access_base",
                table: "currency");

            migrationBuilder.DropColumn(
                name: "access_result",
                table: "currency");

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "currency",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
