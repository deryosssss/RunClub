using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RunClub.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Users_UserId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProgressRecords_Users_UserId",
                table: "ProgressRecords");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_ProgressRecords_UserId",
                table: "ProgressRecords");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_UserId",
                table: "Enrollments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "ProgressRecords");

            migrationBuilder.RenameColumn(
                name: "ProgressDetails",
                table: "ProgressRecords",
                newName: "ProgressDateTime");

            migrationBuilder.AddColumn<double>(
                name: "DistanceCovered",
                table: "ProgressRecords",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TimeTaken",
                table: "ProgressRecords",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "ProgressRecords",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "EnrollmentDate",
                table: "Enrollments",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Enrollments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiry",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1955c25b-0b3b-4bbc-8f4b-3219cb2943fa", null, "Runner", "RUNNER" },
                    { "4ff98108-ed9c-4029-ac0c-dd30c531012b", null, "Coach", "COACH" },
                    { "d277e833-cb6a-4e69-aace-7a342b6c2cf9", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProgressRecords_UserId1",
                table: "ProgressRecords",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_UserId1",
                table: "Enrollments",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RoleId",
                table: "AspNetUsers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Roles_RoleId",
                table: "AspNetUsers",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_AspNetUsers_UserId1",
                table: "Enrollments",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProgressRecords_AspNetUsers_UserId1",
                table: "ProgressRecords",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Roles_RoleId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_AspNetUsers_UserId1",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProgressRecords_AspNetUsers_UserId1",
                table: "ProgressRecords");

            migrationBuilder.DropIndex(
                name: "IX_ProgressRecords_UserId1",
                table: "ProgressRecords");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_UserId1",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RoleId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins");

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

            migrationBuilder.DropColumn(
                name: "DistanceCovered",
                table: "ProgressRecords");

            migrationBuilder.DropColumn(
                name: "TimeTaken",
                table: "ProgressRecords");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "ProgressRecords");

            migrationBuilder.DropColumn(
                name: "EnrollmentDate",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiry",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ProgressDateTime",
                table: "ProgressRecords",
                newName: "ProgressDetails");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "ProgressRecords",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins",
                columns: new[] { "UserId", "LoginProvider", "ProviderKey" });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", null, "Admin", "ADMIN" },
                    { "2", null, "Coach", "COACH" },
                    { "3", null, "Runner", "RUNNER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProgressRecords_UserId",
                table: "ProgressRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_UserId",
                table: "Enrollments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Users_UserId",
                table: "Enrollments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProgressRecords_Users_UserId",
                table: "ProgressRecords",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
