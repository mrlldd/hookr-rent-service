using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HookrTelegramBot.Migrations
{
    public partial class RevertedWebAppUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HookahPhotos_TelegramBotUsers_CreatedById",
                table: "HookahPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_HookahPhotos_TelegramBotUsers_DeletedById",
                table: "HookahPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_HookahPhotos_TelegramBotUsers_UpdatedById",
                table: "HookahPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_Hookahs_TelegramBotUsers_CreatedById",
                table: "Hookahs");

            migrationBuilder.DropForeignKey(
                name: "FK_Hookahs_TelegramBotUsers_UpdatedById",
                table: "Hookahs");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedHookahs_TelegramBotUsers_CreatedById",
                table: "OrderedHookahs");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedHookahs_TelegramBotUsers_UpdatedById",
                table: "OrderedHookahs");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedTobaccos_TelegramBotUsers_CreatedById",
                table: "OrderedTobaccos");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedTobaccos_TelegramBotUsers_UpdatedById",
                table: "OrderedTobaccos");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_TelegramBotUsers_CreatedById",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_TelegramBotUsers_DeletedById",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_TelegramBotUsers_UpdatedById",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_TelegramWebAppUsers_WebAppUserId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_TobaccoPhotos_TelegramBotUsers_CreatedById",
                table: "TobaccoPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_TobaccoPhotos_TelegramBotUsers_DeletedById",
                table: "TobaccoPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_TobaccoPhotos_TelegramBotUsers_UpdatedById",
                table: "TobaccoPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_Tobaccos_TelegramBotUsers_CreatedById",
                table: "Tobaccos");

            migrationBuilder.DropForeignKey(
                name: "FK_Tobaccos_TelegramBotUsers_UpdatedById",
                table: "Tobaccos");

            migrationBuilder.DropTable(
                name: "TelegramBotUsers");

            migrationBuilder.DropTable(
                name: "TelegramWebAppUsers");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_WebAppUserId",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "WebAppUserId",
                table: "RefreshTokens");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "RefreshTokens",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TelegramUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false, defaultValue: 1),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    PhotoUrl = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramUsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HookahPhotos_TelegramUsers_CreatedById",
                table: "HookahPhotos",
                column: "CreatedById",
                principalTable: "TelegramUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HookahPhotos_TelegramUsers_DeletedById",
                table: "HookahPhotos",
                column: "DeletedById",
                principalTable: "TelegramUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HookahPhotos_TelegramUsers_UpdatedById",
                table: "HookahPhotos",
                column: "UpdatedById",
                principalTable: "TelegramUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Hookahs_TelegramUsers_CreatedById",
                table: "Hookahs",
                column: "CreatedById",
                principalTable: "TelegramUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Hookahs_TelegramUsers_UpdatedById",
                table: "Hookahs",
                column: "UpdatedById",
                principalTable: "TelegramUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedHookahs_TelegramUsers_CreatedById",
                table: "OrderedHookahs",
                column: "CreatedById",
                principalTable: "TelegramUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedHookahs_TelegramUsers_UpdatedById",
                table: "OrderedHookahs",
                column: "UpdatedById",
                principalTable: "TelegramUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedTobaccos_TelegramUsers_CreatedById",
                table: "OrderedTobaccos",
                column: "CreatedById",
                principalTable: "TelegramUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedTobaccos_TelegramUsers_UpdatedById",
                table: "OrderedTobaccos",
                column: "UpdatedById",
                principalTable: "TelegramUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_TelegramUsers_CreatedById",
                table: "Orders",
                column: "CreatedById",
                principalTable: "TelegramUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_TelegramUsers_DeletedById",
                table: "Orders",
                column: "DeletedById",
                principalTable: "TelegramUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_TelegramUsers_UpdatedById",
                table: "Orders",
                column: "UpdatedById",
                principalTable: "TelegramUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_TelegramUsers_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "TelegramUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TobaccoPhotos_TelegramUsers_CreatedById",
                table: "TobaccoPhotos",
                column: "CreatedById",
                principalTable: "TelegramUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TobaccoPhotos_TelegramUsers_DeletedById",
                table: "TobaccoPhotos",
                column: "DeletedById",
                principalTable: "TelegramUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TobaccoPhotos_TelegramUsers_UpdatedById",
                table: "TobaccoPhotos",
                column: "UpdatedById",
                principalTable: "TelegramUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tobaccos_TelegramUsers_CreatedById",
                table: "Tobaccos",
                column: "CreatedById",
                principalTable: "TelegramUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tobaccos_TelegramUsers_UpdatedById",
                table: "Tobaccos",
                column: "UpdatedById",
                principalTable: "TelegramUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HookahPhotos_TelegramUsers_CreatedById",
                table: "HookahPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_HookahPhotos_TelegramUsers_DeletedById",
                table: "HookahPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_HookahPhotos_TelegramUsers_UpdatedById",
                table: "HookahPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_Hookahs_TelegramUsers_CreatedById",
                table: "Hookahs");

            migrationBuilder.DropForeignKey(
                name: "FK_Hookahs_TelegramUsers_UpdatedById",
                table: "Hookahs");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedHookahs_TelegramUsers_CreatedById",
                table: "OrderedHookahs");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedHookahs_TelegramUsers_UpdatedById",
                table: "OrderedHookahs");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedTobaccos_TelegramUsers_CreatedById",
                table: "OrderedTobaccos");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedTobaccos_TelegramUsers_UpdatedById",
                table: "OrderedTobaccos");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_TelegramUsers_CreatedById",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_TelegramUsers_DeletedById",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_TelegramUsers_UpdatedById",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_TelegramUsers_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_TobaccoPhotos_TelegramUsers_CreatedById",
                table: "TobaccoPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_TobaccoPhotos_TelegramUsers_DeletedById",
                table: "TobaccoPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_TobaccoPhotos_TelegramUsers_UpdatedById",
                table: "TobaccoPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_Tobaccos_TelegramUsers_CreatedById",
                table: "Tobaccos");

            migrationBuilder.DropForeignKey(
                name: "FK_Tobaccos_TelegramUsers_UpdatedById",
                table: "Tobaccos");

            migrationBuilder.DropTable(
                name: "TelegramUsers");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RefreshTokens");

            migrationBuilder.AddColumn<int>(
                name: "WebAppUserId",
                table: "RefreshTokens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TelegramWebAppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BotUserId = table.Column<int>(type: "int", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramWebAppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TelegramBotUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebAppUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramBotUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelegramBotUsers_TelegramWebAppUsers_WebAppUserId",
                        column: x => x.WebAppUserId,
                        principalTable: "TelegramWebAppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_WebAppUserId",
                table: "RefreshTokens",
                column: "WebAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TelegramBotUsers_WebAppUserId",
                table: "TelegramBotUsers",
                column: "WebAppUserId",
                unique: true,
                filter: "[WebAppUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_HookahPhotos_TelegramBotUsers_CreatedById",
                table: "HookahPhotos",
                column: "CreatedById",
                principalTable: "TelegramBotUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HookahPhotos_TelegramBotUsers_DeletedById",
                table: "HookahPhotos",
                column: "DeletedById",
                principalTable: "TelegramBotUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HookahPhotos_TelegramBotUsers_UpdatedById",
                table: "HookahPhotos",
                column: "UpdatedById",
                principalTable: "TelegramBotUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Hookahs_TelegramBotUsers_CreatedById",
                table: "Hookahs",
                column: "CreatedById",
                principalTable: "TelegramBotUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Hookahs_TelegramBotUsers_UpdatedById",
                table: "Hookahs",
                column: "UpdatedById",
                principalTable: "TelegramBotUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedHookahs_TelegramBotUsers_CreatedById",
                table: "OrderedHookahs",
                column: "CreatedById",
                principalTable: "TelegramBotUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedHookahs_TelegramBotUsers_UpdatedById",
                table: "OrderedHookahs",
                column: "UpdatedById",
                principalTable: "TelegramBotUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedTobaccos_TelegramBotUsers_CreatedById",
                table: "OrderedTobaccos",
                column: "CreatedById",
                principalTable: "TelegramBotUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedTobaccos_TelegramBotUsers_UpdatedById",
                table: "OrderedTobaccos",
                column: "UpdatedById",
                principalTable: "TelegramBotUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_TelegramBotUsers_CreatedById",
                table: "Orders",
                column: "CreatedById",
                principalTable: "TelegramBotUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_TelegramBotUsers_DeletedById",
                table: "Orders",
                column: "DeletedById",
                principalTable: "TelegramBotUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_TelegramBotUsers_UpdatedById",
                table: "Orders",
                column: "UpdatedById",
                principalTable: "TelegramBotUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_TelegramWebAppUsers_WebAppUserId",
                table: "RefreshTokens",
                column: "WebAppUserId",
                principalTable: "TelegramWebAppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TobaccoPhotos_TelegramBotUsers_CreatedById",
                table: "TobaccoPhotos",
                column: "CreatedById",
                principalTable: "TelegramBotUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TobaccoPhotos_TelegramBotUsers_DeletedById",
                table: "TobaccoPhotos",
                column: "DeletedById",
                principalTable: "TelegramBotUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TobaccoPhotos_TelegramBotUsers_UpdatedById",
                table: "TobaccoPhotos",
                column: "UpdatedById",
                principalTable: "TelegramBotUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tobaccos_TelegramBotUsers_CreatedById",
                table: "Tobaccos",
                column: "CreatedById",
                principalTable: "TelegramBotUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tobaccos_TelegramBotUsers_UpdatedById",
                table: "Tobaccos",
                column: "UpdatedById",
                principalTable: "TelegramBotUsers",
                principalColumn: "Id");
        }
    }
}
