using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RunClub.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleNormalizedName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "257289ac-2e4a-4e79-967e-415cc1e3939e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "262d3617-c4db-4a31-8559-34189a1b86b7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d40c5850-bf86-4899-95bc-bf4651e90d66");

            migrationBuilder.RenameColumn(
                name: "ProgressDateTime",
                table: "ProgressRecords",
                newName: "ProgressTime");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "Roles",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "RoleNormalizedName",
                table: "Roles",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ProgressDate",
                table: "ProgressRecords",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "ProgressDetails",
                table: "ProgressRecords",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "674236fe-1b32-43b9-9473-e8d94c426e1d", null, "Coach", "COACH" },
                    { "6fee3675-42a2-4d77-8d58-f48b2e55b765", null, "Runner", "RUNNER" },
                    { "78ffa353-52f8-4113-8204-856999bd3411", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "674236fe-1b32-43b9-9473-e8d94c426e1d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6fee3675-42a2-4d77-8d58-f48b2e55b765");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78ffa353-52f8-4113-8204-856999bd3411");

            migrationBuilder.DropColumn(
                name: "RoleNormalizedName",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "ProgressDate",
                table: "ProgressRecords");

            migrationBuilder.DropColumn(
                name: "ProgressDetails",
                table: "ProgressRecords");

            migrationBuilder.RenameColumn(
                name: "ProgressTime",
                table: "ProgressRecords",
                newName: "ProgressDateTime");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "Roles",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "257289ac-2e4a-4e79-967e-415cc1e3939e", null, "Runner", "RUNNER" },
                    { "262d3617-c4db-4a31-8559-34189a1b86b7", null, "Admin", "ADMIN" },
                    { "d40c5850-bf86-4899-95bc-bf4651e90d66", null, "Coach", "COACH" }
                });
        }
    }
}
