using Microsoft.EntityFrameworkCore.Migrations;

namespace HookrTelegramBot.Migrations
{
    public partial class WebAppUserIdAutoincrement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TelegramWebAppUsers_TelegramBotUsers_BotUserId",
                table: "TelegramWebAppUsers");

            migrationBuilder.DropIndex(
                name: "IX_TelegramWebAppUsers_BotUserId",
                table: "TelegramWebAppUsers");

            migrationBuilder.CreateIndex(
                name: "IX_TelegramBotUsers_WebAppUserId",
                table: "TelegramBotUsers",
                column: "WebAppUserId",
                unique: true,
                filter: "[WebAppUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_TelegramBotUsers_TelegramWebAppUsers_WebAppUserId",
                table: "TelegramBotUsers",
                column: "WebAppUserId",
                principalTable: "TelegramWebAppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TelegramBotUsers_TelegramWebAppUsers_WebAppUserId",
                table: "TelegramBotUsers");

            migrationBuilder.DropIndex(
                name: "IX_TelegramBotUsers_WebAppUserId",
                table: "TelegramBotUsers");

            migrationBuilder.CreateIndex(
                name: "IX_TelegramWebAppUsers_BotUserId",
                table: "TelegramWebAppUsers",
                column: "BotUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TelegramWebAppUsers_TelegramBotUsers_BotUserId",
                table: "TelegramWebAppUsers",
                column: "BotUserId",
                principalTable: "TelegramBotUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
