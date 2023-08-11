using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RapidPay.Core.Migrations
{
    public partial class Add_Payment_Fee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Fee",
                table: "Payments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fee",
                table: "Payments");
        }
    }
}
