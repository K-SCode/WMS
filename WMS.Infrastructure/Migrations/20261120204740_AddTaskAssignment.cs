using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AssignedAt",
                table: "WarehouseTask",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "AssignedUserId",
                table: "WarehouseTask",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTask_AssignedUserId_Status",
                table: "WarehouseTask",
                columns: new[] { "AssignedUserId", "Status" });

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseTask_AspNetUsers_AssignedUserId",
                table: "WarehouseTask",
                column: "AssignedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseTask_AspNetUsers_AssignedUserId",
                table: "WarehouseTask");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseTask_AssignedUserId_Status",
                table: "WarehouseTask");

            migrationBuilder.DropColumn(
                name: "AssignedAt",
                table: "WarehouseTask");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "WarehouseTask");
        }
    }
}
