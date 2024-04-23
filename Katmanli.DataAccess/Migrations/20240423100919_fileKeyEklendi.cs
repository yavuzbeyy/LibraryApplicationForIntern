using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Katmanli.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fileKeyEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileKey",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 23, 13, 9, 19, 58, DateTimeKind.Local).AddTicks(9726));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 23, 13, 9, 19, 58, DateTimeKind.Local).AddTicks(9743));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 23, 13, 9, 19, 58, DateTimeKind.Local).AddTicks(9744));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileKey",
                table: "Books");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 21, 21, 23, 49, 468, DateTimeKind.Local).AddTicks(5487));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 21, 21, 23, 49, 468, DateTimeKind.Local).AddTicks(5500));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 21, 21, 23, 49, 468, DateTimeKind.Local).AddTicks(5501));
        }
    }
}
