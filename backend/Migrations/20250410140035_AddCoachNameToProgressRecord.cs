using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunClubAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCoachNameToProgressRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoachName",
                table: "ProgressRecords",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoachName",
                table: "ProgressRecords");
        }
    }
}
