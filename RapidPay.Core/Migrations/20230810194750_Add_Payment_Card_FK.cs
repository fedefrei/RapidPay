using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RapidPay.Core.Migrations
{
    public partial class Add_Payment_Card_FK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Payments_CardId",
                table: "Payments",
                column: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Cards_CardId",
                table: "Payments",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Cards_CardId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CardId",
                table: "Payments");
        }
    }
}
