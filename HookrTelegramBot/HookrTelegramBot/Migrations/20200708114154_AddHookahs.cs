using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HookrTelegramBot.Migrations
{
    public partial class AddHookahs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hookahs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hookahs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hookahs_TelegramUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "TelegramUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Hookahs_TelegramUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "TelegramUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hookahs_CreatedById",
                table: "Hookahs",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Hookahs_UpdatedById",
                table: "Hookahs",
                column: "UpdatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hookahs");
        }
    }
}
