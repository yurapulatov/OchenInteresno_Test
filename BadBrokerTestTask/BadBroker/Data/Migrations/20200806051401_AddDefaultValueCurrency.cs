using Microsoft.EntityFrameworkCore.Migrations;

namespace BadBroker.Data.Migrations
{
    public partial class AddDefaultValueCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
INSERT INTO currency (code, name, access_base, access_result)
VALUES 
('USD', 'Dollar USA', true, false),
('RUB', 'Russian ruble', false, true),
('GBP', 'Pound sterling', false, true),
('EUR', 'Euro', false, true),
('JPY', 'Japanese yen', false, true)
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
TRUNCATE TABLE currency
");
        }
    }
}
