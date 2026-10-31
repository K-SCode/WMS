using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoodsReceipt_Supplier_SupplierNip",
                table: "GoodsReceipt");

            migrationBuilder.DropIndex(
                name: "IX_GoodsReceipt_InvoiceNumber",
                table: "GoodsReceipt");

            migrationBuilder.DropIndex(
                name: "IX_GoodsReceipt_SupplierNip",
                table: "GoodsReceipt");

            migrationBuilder.AlterColumn<string>(
                name: "UnitQuantity",
                table: "GoodsReceiptsItems",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTask_Barcode",
                table: "WarehouseTask",
                column: "Barcode");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTask_Status_Source",
                table: "WarehouseTask",
                columns: new[] { "Status", "Source" });

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceipt_SupplierNip_InvoiceNumber",
                table: "GoodsReceipt",
                columns: new[] { "SupplierNip", "InvoiceNumber" });

            migrationBuilder.AddForeignKey(
                name: "FK_GoodsReceipt_Supplier_SupplierNip",
                table: "GoodsReceipt",
                column: "SupplierNip",
                principalTable: "Supplier",
                principalColumn: "Nip",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoodsReceipt_Supplier_SupplierNip",
                table: "GoodsReceipt");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseTask_Barcode",
                table: "WarehouseTask");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseTask_Status_Source",
                table: "WarehouseTask");

            migrationBuilder.DropIndex(
                name: "IX_GoodsReceipt_SupplierNip_InvoiceNumber",
                table: "GoodsReceipt");

            migrationBuilder.AlterColumn<string>(
                name: "UnitQuantity",
                table: "GoodsReceiptsItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceipt_InvoiceNumber",
                table: "GoodsReceipt",
                column: "InvoiceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceipt_SupplierNip",
                table: "GoodsReceipt",
                column: "SupplierNip");

            migrationBuilder.AddForeignKey(
                name: "FK_GoodsReceipt_Supplier_SupplierNip",
                table: "GoodsReceipt",
                column: "SupplierNip",
                principalTable: "Supplier",
                principalColumn: "Nip",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
