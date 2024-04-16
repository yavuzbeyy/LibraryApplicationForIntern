using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Katmanli.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class isAvailableAddedOnBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isAvailable",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 16, 12, 16, 38, 963, DateTimeKind.Local).AddTicks(5900));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 16, 12, 16, 38, 963, DateTimeKind.Local).AddTicks(5916));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 16, 12, 16, 38, 963, DateTimeKind.Local).AddTicks(5918));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isAvailable",
                table: "Books");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 15, 11, 31, 21, 802, DateTimeKind.Local).AddTicks(4631));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 15, 11, 31, 21, 802, DateTimeKind.Local).AddTicks(4647));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 15, 11, 31, 21, 802, DateTimeKind.Local).AddTicks(4648));
        }
    }
}
