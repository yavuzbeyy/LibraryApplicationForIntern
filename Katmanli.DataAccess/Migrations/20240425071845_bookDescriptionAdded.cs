using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Katmanli.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class bookDescriptionAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 25, 10, 18, 45, 599, DateTimeKind.Local).AddTicks(3676));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 25, 10, 18, 45, 599, DateTimeKind.Local).AddTicks(3691));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Books");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 25, 10, 8, 20, 283, DateTimeKind.Local).AddTicks(1036));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 25, 10, 8, 20, 283, DateTimeKind.Local).AddTicks(1054));
        }
    }
}
