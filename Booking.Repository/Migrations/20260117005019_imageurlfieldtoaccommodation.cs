using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class imageurlfieldtoaccommodation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Accommodations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Accommodations");
        }
    }
}
