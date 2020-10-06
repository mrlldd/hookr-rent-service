using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HookrTelegramBot.Migrations
{
    public partial class AddOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedById = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_TelegramUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "TelegramUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_TelegramUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "TelegramUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderedHookahs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedById = table.Column<int>(nullable: true),
                    OrderId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderedHookahs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderedHookahs_TelegramUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "TelegramUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderedHookahs_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderedHookahs_Hookahs_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Hookahs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderedHookahs_TelegramUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "TelegramUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderedTobaccos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedById = table.Column<int>(nullable: true),
                    OrderId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderedTobaccos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderedTobaccos_TelegramUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "TelegramUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderedTobaccos_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderedTobaccos_Tobaccos_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Tobaccos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderedTobaccos_TelegramUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "TelegramUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderedHookahs_CreatedById",
                table: "OrderedHookahs",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedHookahs_OrderId",
                table: "OrderedHookahs",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedHookahs_ProductId",
                table: "OrderedHookahs",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderedHookahs_UpdatedById",
                table: "OrderedHookahs",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedTobaccos_CreatedById",
                table: "OrderedTobaccos",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedTobaccos_OrderId",
                table: "OrderedTobaccos",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedTobaccos_ProductId",
                table: "OrderedTobaccos",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderedTobaccos_UpdatedById",
                table: "OrderedTobaccos",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreatedById",
                table: "Orders",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UpdatedById",
                table: "Orders",
                column: "UpdatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderedHookahs");

            migrationBuilder.DropTable(
                name: "OrderedTobaccos");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
