using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWarehouseTaskToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "GoodsReceiptsItems");

            migrationBuilder.CreateTable(
                name: "WarehouseTask",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Barcode = table.Column<string>(type: "char(13)", unicode: false, fixedLength: true, maxLength: 13, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    GoodsReceiptId = table.Column<int>(type: "int", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    StockStatus = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseTask_GoodsReceipt_GoodsReceiptId",
                        column: x => x.GoodsReceiptId,
                        principalTable: "GoodsReceipt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseTask_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseTask_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTask_GoodsReceiptId",
                table: "WarehouseTask",
                column: "GoodsReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTask_LocationId",
                table: "WarehouseTask",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTask_OrderId",
                table: "WarehouseTask",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTask_StockStatus_CreatedAt",
                table: "WarehouseTask",
                columns: new[] { "StockStatus", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WarehouseTask");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "GoodsReceiptsItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
