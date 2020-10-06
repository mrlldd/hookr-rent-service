using Microsoft.EntityFrameworkCore.Migrations;

namespace HookrTelegramBot.Migrations
{
    public partial class DuplicatesFixOnOrdering : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderedTobaccos_ProductId",
                table: "OrderedTobaccos");

            migrationBuilder.DropIndex(
                name: "IX_OrderedHookahs_ProductId",
                table: "OrderedHookahs");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedTobaccos_ProductId",
                table: "OrderedTobaccos",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedHookahs_ProductId",
                table: "OrderedHookahs",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderedTobaccos_ProductId",
                table: "OrderedTobaccos");

            migrationBuilder.DropIndex(
                name: "IX_OrderedHookahs_ProductId",
                table: "OrderedHookahs");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedTobaccos_ProductId",
                table: "OrderedTobaccos",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderedHookahs_ProductId",
                table: "OrderedHookahs",
                column: "ProductId",
                unique: true);
        }
    }
}
