using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Migration8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hosts_Countries_CountryId",
                table: "Hosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_UserId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_UserId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "Nights",
                table: "AccommodationInReservations",
                newName: "Category");

            migrationBuilder.AddColumn<Guid>(
                name: "ReservationCartId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

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

            migrationBuilder.CreateTable(
                name: "ReservationCarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationCarts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccommodationInReservationCarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccommodationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReservationCartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nights = table.Column<int>(type: "int", nullable: false),
                    AccommodationInReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccommodationInReservationCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccommodationInReservationCarts_AccommodationInReservations_AccommodationInReservationId",
                        column: x => x.AccommodationInReservationId,
                        principalTable: "AccommodationInReservations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AccommodationInReservationCarts_Accommodations_AccommodationId",
                        column: x => x.AccommodationId,
                        principalTable: "Accommodations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccommodationInReservationCarts_ReservationCarts_ReservationCartId",
                        column: x => x.ReservationCartId,
                        principalTable: "ReservationCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UserId",
                table: "Reservations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ReservationCartId",
                table: "AspNetUsers",
                column: "ReservationCartId");

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationInReservations_HostId",
                table: "AccommodationInReservations",
                column: "HostId");

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationInReservationCarts_AccommodationId",
                table: "AccommodationInReservationCarts",
                column: "AccommodationId");

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationInReservationCarts_AccommodationInReservationId",
                table: "AccommodationInReservationCarts",
                column: "AccommodationInReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationInReservationCarts_ReservationCartId",
                table: "AccommodationInReservationCarts",
                column: "ReservationCartId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationCarts_UserId",
                table: "ReservationCarts",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AccommodationInReservations_Hosts_HostId",
                table: "AccommodationInReservations",
                column: "HostId",
                principalTable: "Hosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ReservationCarts_ReservationCartId",
                table: "AspNetUsers",
                column: "ReservationCartId",
                principalTable: "ReservationCarts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Hosts_Countries_CountryId",
                table: "Hosts",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_UserId",
                table: "Reservations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccommodationInReservations_Hosts_HostId",
                table: "AccommodationInReservations");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ReservationCarts_ReservationCartId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Hosts_Countries_CountryId",
                table: "Hosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_UserId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "AccommodationInReservationCarts");

            migrationBuilder.DropTable(
                name: "ReservationCarts");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_UserId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ReservationCartId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AccommodationInReservations_HostId",
                table: "AccommodationInReservations");

            migrationBuilder.DropColumn(
                name: "ReservationCartId",
                table: "AspNetUsers");

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

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "AccommodationInReservations",
                newName: "Nights");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UserId",
                table: "Reservations",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Hosts_Countries_CountryId",
                table: "Hosts",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_UserId",
                table: "Reservations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
