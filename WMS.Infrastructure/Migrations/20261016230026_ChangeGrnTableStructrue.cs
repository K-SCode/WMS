using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeGrnTableStructrue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoodsReceipt_Supplier_SupplierId",
                table: "GoodsReceipt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Supplier",
                table: "Supplier");

            migrationBuilder.DropIndex(
                name: "IX_Supplier_NIP",
                table: "Supplier");

            migrationBuilder.DropIndex(
                name: "IX_GoodsReceipt_SupplierId",
                table: "GoodsReceipt");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Supplier");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "GoodsReceipt");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "Stocks",
                type: "decimal(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "GoodsReceiptsItems",
                type: "nchar(13)",
                fixedLength: true,
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SupplierNip",
                table: "GoodsReceipt",
                type: "nchar(10)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Supplier",
                table: "Supplier",
                column: "NIP");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptsItems_Barcode",
                table: "GoodsReceiptsItems",
                column: "Barcode");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceipt_SupplierNip",
                table: "GoodsReceipt",
                column: "SupplierNip");

            migrationBuilder.AddForeignKey(
                name: "FK_GoodsReceipt_Supplier_SupplierNip",
                table: "GoodsReceipt",
                column: "SupplierNip",
                principalTable: "Supplier",
                principalColumn: "NIP",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoodsReceipt_Supplier_SupplierNip",
                table: "GoodsReceipt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Supplier",
                table: "Supplier");

            migrationBuilder.DropIndex(
                name: "IX_GoodsReceiptsItems_Barcode",
                table: "GoodsReceiptsItems");

            migrationBuilder.DropIndex(
                name: "IX_GoodsReceipt_SupplierNip",
                table: "GoodsReceipt");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "GoodsReceiptsItems");

            migrationBuilder.DropColumn(
                name: "SupplierNip",
                table: "GoodsReceipt");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Supplier",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "Stocks",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)",
                oldPrecision: 18,
                oldScale: 3);

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "GoodsReceipt",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Supplier",
                table: "Supplier",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_NIP",
                table: "Supplier",
                column: "NIP",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceipt_SupplierId",
                table: "GoodsReceipt",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_GoodsReceipt_Supplier_SupplierId",
                table: "GoodsReceipt",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
