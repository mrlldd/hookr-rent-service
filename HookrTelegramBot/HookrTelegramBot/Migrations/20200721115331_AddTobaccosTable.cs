using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HookrTelegramBot.Migrations
{
    public partial class AddTobaccosTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tobaccos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedById = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tobaccos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tobaccos_TelegramUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "TelegramUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tobaccos_TelegramUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "TelegramUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tobaccos_CreatedById",
                table: "Tobaccos",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Tobaccos_UpdatedById",
                table: "Tobaccos",
                column: "UpdatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tobaccos");
        }
    }
}
