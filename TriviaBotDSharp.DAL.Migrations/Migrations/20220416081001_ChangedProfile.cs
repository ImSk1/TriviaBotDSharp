using Microsoft.EntityFrameworkCore.Migrations;

namespace TriviaBotDSharp.DAL.Migrations.Migrations
{
    public partial class ChangedProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Profiles");

            migrationBuilder.AddColumn<int>(
                name: "CorrectAnswers",
                table: "Profiles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscordId",
                table: "Profiles",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GuildId",
                table: "Profiles",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "WrongAnswers",
                table: "Profiles",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswers",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "DiscordId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "GuildId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "WrongAnswers",
                table: "Profiles");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
