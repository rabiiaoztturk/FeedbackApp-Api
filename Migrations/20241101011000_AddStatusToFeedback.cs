using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FeedbackApp.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Feedbacks",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Feedbacks");
        }
    }
}
