using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunClubAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitEventCoachUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoachName",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CoachPhotoUrl",
                table: "Events");

            migrationBuilder.AddColumn<int>(
                name: "CoachId",
                table: "Events",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_CoachId",
                table: "Events",
                column: "CoachId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Coaches_CoachId",
                table: "Events",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Coaches_CoachId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_CoachId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "Events");

            migrationBuilder.AddColumn<string>(
                name: "CoachName",
                table: "Events",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoachPhotoUrl",
                table: "Events",
                type: "TEXT",
                nullable: true);
        }
    }
}
