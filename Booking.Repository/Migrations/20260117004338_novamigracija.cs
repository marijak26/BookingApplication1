using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class novamigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Accommodations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Accommodations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
