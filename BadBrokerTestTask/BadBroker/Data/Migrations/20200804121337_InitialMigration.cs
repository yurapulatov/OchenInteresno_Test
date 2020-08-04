using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BadBroker.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "currency",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_currency", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rate",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    base_currency_id = table.Column<int>(nullable: false),
                    result_currency_id = table.Column<int>(nullable: false),
                    rate_date = table.Column<DateTime>(nullable: false),
                    rate_value = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rate", x => x.id);
                    table.ForeignKey(
                        name: "FK_rate_currency_base_currency_id",
                        column: x => x.base_currency_id,
                        principalTable: "currency",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rate_currency_result_currency_id",
                        column: x => x.result_currency_id,
                        principalTable: "currency",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_rate_base_currency_id",
                table: "rate",
                column: "base_currency_id");

            migrationBuilder.CreateIndex(
                name: "IX_rate_rate_date",
                table: "rate",
                column: "rate_date");

            migrationBuilder.CreateIndex(
                name: "IX_rate_result_currency_id",
                table: "rate",
                column: "result_currency_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rate");

            migrationBuilder.DropTable(
                name: "currency");
        }
    }
}
