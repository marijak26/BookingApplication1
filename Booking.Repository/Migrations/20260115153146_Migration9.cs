using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Migration9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccommodationInReservationCarts_AccommodationInReservations_AccommodationInReservationId",
                table: "AccommodationInReservationCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_AccommodationInReservations_Hosts_HostId",
                table: "AccommodationInReservations");

            migrationBuilder.DropIndex(
                name: "IX_AccommodationInReservations_HostId",
                table: "AccommodationInReservations");

            migrationBuilder.DropIndex(
                name: "IX_AccommodationInReservationCarts_AccommodationInReservationId",
                table: "AccommodationInReservationCarts");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AccommodationInReservations");

            migrationBuilder.DropColumn(
                name: "HostId",
                table: "AccommodationInReservations");

            migrationBuilder.DropColumn(
                name: "IsRented",
                table: "AccommodationInReservations");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AccommodationInReservations");

            migrationBuilder.DropColumn(
                name: "PricePerNight",
                table: "AccommodationInReservations");

            migrationBuilder.DropColumn(
                name: "AccommodationInReservationId",
                table: "AccommodationInReservationCarts");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "AccommodationInReservations",
                newName: "Nights");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nights",
                table: "AccommodationInReservations",
                newName: "Category");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AccommodationInReservations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "HostId",
                table: "AccommodationInReservations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsRented",
                table: "AccommodationInReservations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AccommodationInReservations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PricePerNight",
                table: "AccommodationInReservations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "AccommodationInReservationId",
                table: "AccommodationInReservationCarts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationInReservations_HostId",
                table: "AccommodationInReservations",
                column: "HostId");

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationInReservationCarts_AccommodationInReservationId",
                table: "AccommodationInReservationCarts",
                column: "AccommodationInReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccommodationInReservationCarts_AccommodationInReservations_AccommodationInReservationId",
                table: "AccommodationInReservationCarts",
                column: "AccommodationInReservationId",
                principalTable: "AccommodationInReservations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccommodationInReservations_Hosts_HostId",
                table: "AccommodationInReservations",
                column: "HostId",
                principalTable: "Hosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
