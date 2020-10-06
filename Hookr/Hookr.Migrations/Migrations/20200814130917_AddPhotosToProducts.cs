using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HookrTelegramBot.Migrations
{
    public partial class AddPhotosToProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HookahPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedById = table.Column<int>(nullable: true),
                    TelegramFileId = table.Column<string>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedById = table.Column<int>(nullable: true),
                    HookahId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HookahPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HookahPhotos_TelegramUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "TelegramUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HookahPhotos_TelegramUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "TelegramUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HookahPhotos_Hookahs_HookahId",
                        column: x => x.HookahId,
                        principalTable: "Hookahs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HookahPhotos_TelegramUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "TelegramUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TobaccoPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedById = table.Column<int>(nullable: true),
                    TelegramFileId = table.Column<string>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedById = table.Column<int>(nullable: true),
                    TobaccoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TobaccoPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TobaccoPhotos_TelegramUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "TelegramUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TobaccoPhotos_TelegramUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "TelegramUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TobaccoPhotos_Tobaccos_TobaccoId",
                        column: x => x.TobaccoId,
                        principalTable: "Tobaccos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TobaccoPhotos_TelegramUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "TelegramUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HookahPhotos_CreatedById",
                table: "HookahPhotos",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_HookahPhotos_DeletedById",
                table: "HookahPhotos",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_HookahPhotos_HookahId",
                table: "HookahPhotos",
                column: "HookahId");

            migrationBuilder.CreateIndex(
                name: "IX_HookahPhotos_UpdatedById",
                table: "HookahPhotos",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TobaccoPhotos_CreatedById",
                table: "TobaccoPhotos",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TobaccoPhotos_DeletedById",
                table: "TobaccoPhotos",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_TobaccoPhotos_TobaccoId",
                table: "TobaccoPhotos",
                column: "TobaccoId");

            migrationBuilder.CreateIndex(
                name: "IX_TobaccoPhotos_UpdatedById",
                table: "TobaccoPhotos",
                column: "UpdatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HookahPhotos");

            migrationBuilder.DropTable(
                name: "TobaccoPhotos");
        }
    }
}
