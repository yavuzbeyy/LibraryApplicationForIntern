using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Katmanli.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class groupname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserMessageGroupsId",
                table: "UserMessages");

            migrationBuilder.AddColumn<string>(
                name: "groupName",
                table: "UserMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 5, 3, 15, 4, 24, 121, DateTimeKind.Local).AddTicks(3542));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 5, 3, 15, 4, 24, 121, DateTimeKind.Local).AddTicks(3559));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "groupName",
                table: "UserMessages");

            migrationBuilder.AddColumn<int>(
                name: "UserMessageGroupsId",
                table: "UserMessages",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 5, 3, 14, 18, 31, 743, DateTimeKind.Local).AddTicks(3080));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 5, 3, 14, 18, 31, 743, DateTimeKind.Local).AddTicks(3096));
        }
    }
}
