using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProductRelationFromGoodsReceiptsItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoodsReceiptsItems_Products_ProductId",
                table: "GoodsReceiptsItems");

            migrationBuilder.DropIndex(
                name: "IX_GoodsReceiptsItems_ProductId",
                table: "GoodsReceiptsItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "GoodsReceiptsItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "GoodsReceiptsItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptsItems_ProductId",
                table: "GoodsReceiptsItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_GoodsReceiptsItems_Products_ProductId",
                table: "GoodsReceiptsItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
