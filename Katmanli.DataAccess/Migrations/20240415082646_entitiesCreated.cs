using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Katmanli.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class entitiesCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 15, 11, 26, 46, 336, DateTimeKind.Local).AddTicks(6282));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 15, 11, 26, 46, 336, DateTimeKind.Local).AddTicks(6296));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 14, 14, 29, 58, 286, DateTimeKind.Local).AddTicks(2884));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 14, 14, 29, 58, 286, DateTimeKind.Local).AddTicks(2899));
        }
    }
}
