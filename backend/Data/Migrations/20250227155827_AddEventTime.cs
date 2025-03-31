using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RunClub.Migrations
{
    /// <inheritdoc />
    public partial class AddEventTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1955c25b-0b3b-4bbc-8f4b-3219cb2943fa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4ff98108-ed9c-4029-ac0c-dd30c531012b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d277e833-cb6a-4e69-aace-7a342b6c2cf9");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EventTime",
                table: "Events",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "EventTime",
                table: "Events");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1955c25b-0b3b-4bbc-8f4b-3219cb2943fa", null, "Runner", "RUNNER" },
                    { "4ff98108-ed9c-4029-ac0c-dd30c531012b", null, "Coach", "COACH" },
                    { "d277e833-cb6a-4e69-aace-7a342b6c2cf9", null, "Admin", "ADMIN" }
                });
        }
    }
}
