using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddedCheckInAndCheckOutDateToModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nights",
                table: "AccommodationInReservations");

            migrationBuilder.DropColumn(
                name: "Nights",
                table: "AccommodationInReservationCarts");

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate",
                table: "AccommodationInReservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate",
                table: "AccommodationInReservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate",
                table: "AccommodationInReservationCarts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate",
                table: "AccommodationInReservationCarts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "AccommodationInReservations");

            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "AccommodationInReservations");

            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "AccommodationInReservationCarts");

            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "AccommodationInReservationCarts");

            migrationBuilder.AddColumn<int>(
                name: "Nights",
                table: "AccommodationInReservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Nights",
                table: "AccommodationInReservationCarts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
