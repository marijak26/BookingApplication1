using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Migration5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodation_AspNetUsers_BookingApplicationUserId",
                table: "Accommodation");

            migrationBuilder.DropForeignKey(
                name: "FK_Accommodation_Host_HostId",
                table: "Accommodation");

            migrationBuilder.DropForeignKey(
                name: "FK_AccommodationInReservation_Accommodation_AccommodationId",
                table: "AccommodationInReservation");

            migrationBuilder.DropForeignKey(
                name: "FK_AccommodationInReservation_Reservation_ReservationId",
                table: "AccommodationInReservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_AspNetUsers_UserId",
                table: "Reservation");

            migrationBuilder.DropTable(
                name: "Host");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservation",
                table: "Reservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Country",
                table: "Country");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccommodationInReservation",
                table: "AccommodationInReservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accommodation",
                table: "Accommodation");

            migrationBuilder.RenameTable(
                name: "Reservation",
                newName: "Reservations");

            migrationBuilder.RenameTable(
                name: "Country",
                newName: "Countries");

            migrationBuilder.RenameTable(
                name: "AccommodationInReservation",
                newName: "AccommodationInReservations");

            migrationBuilder.RenameTable(
                name: "Accommodation",
                newName: "Accommodations");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_UserId",
                table: "Reservations",
                newName: "IX_Reservations_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AccommodationInReservation_ReservationId",
                table: "AccommodationInReservations",
                newName: "IX_AccommodationInReservations_ReservationId");

            migrationBuilder.RenameIndex(
                name: "IX_AccommodationInReservation_AccommodationId",
                table: "AccommodationInReservations",
                newName: "IX_AccommodationInReservations_AccommodationId");

            migrationBuilder.RenameIndex(
                name: "IX_Accommodation_HostId",
                table: "Accommodations",
                newName: "IX_Accommodations_HostId");

            migrationBuilder.RenameIndex(
                name: "IX_Accommodation_BookingApplicationUserId",
                table: "Accommodations",
                newName: "IX_Accommodations_BookingApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                table: "Countries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccommodationInReservations",
                table: "AccommodationInReservations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accommodations",
                table: "Accommodations",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Hosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hosts_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hosts_CountryId",
                table: "Hosts",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccommodationInReservations_Accommodations_AccommodationId",
                table: "AccommodationInReservations",
                column: "AccommodationId",
                principalTable: "Accommodations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccommodationInReservations_Reservations_ReservationId",
                table: "AccommodationInReservations",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_AspNetUsers_BookingApplicationUserId",
                table: "Accommodations",
                column: "BookingApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_Hosts_HostId",
                table: "Accommodations",
                column: "HostId",
                principalTable: "Hosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_UserId",
                table: "Reservations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccommodationInReservations_Accommodations_AccommodationId",
                table: "AccommodationInReservations");

            migrationBuilder.DropForeignKey(
                name: "FK_AccommodationInReservations_Reservations_ReservationId",
                table: "AccommodationInReservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_AspNetUsers_BookingApplicationUserId",
                table: "Accommodations");

            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_Hosts_HostId",
                table: "Accommodations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_UserId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "Hosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                table: "Countries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accommodations",
                table: "Accommodations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccommodationInReservations",
                table: "AccommodationInReservations");

            migrationBuilder.RenameTable(
                name: "Reservations",
                newName: "Reservation");

            migrationBuilder.RenameTable(
                name: "Countries",
                newName: "Country");

            migrationBuilder.RenameTable(
                name: "Accommodations",
                newName: "Accommodation");

            migrationBuilder.RenameTable(
                name: "AccommodationInReservations",
                newName: "AccommodationInReservation");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_UserId",
                table: "Reservation",
                newName: "IX_Reservation_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Accommodations_HostId",
                table: "Accommodation",
                newName: "IX_Accommodation_HostId");

            migrationBuilder.RenameIndex(
                name: "IX_Accommodations_BookingApplicationUserId",
                table: "Accommodation",
                newName: "IX_Accommodation_BookingApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_AccommodationInReservations_ReservationId",
                table: "AccommodationInReservation",
                newName: "IX_AccommodationInReservation_ReservationId");

            migrationBuilder.RenameIndex(
                name: "IX_AccommodationInReservations_AccommodationId",
                table: "AccommodationInReservation",
                newName: "IX_AccommodationInReservation_AccommodationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservation",
                table: "Reservation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Country",
                table: "Country",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accommodation",
                table: "Accommodation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccommodationInReservation",
                table: "AccommodationInReservation",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Host",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Host", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Host_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Host_CountryId",
                table: "Host",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodation_AspNetUsers_BookingApplicationUserId",
                table: "Accommodation",
                column: "BookingApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodation_Host_HostId",
                table: "Accommodation",
                column: "HostId",
                principalTable: "Host",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccommodationInReservation_Accommodation_AccommodationId",
                table: "AccommodationInReservation",
                column: "AccommodationId",
                principalTable: "Accommodation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccommodationInReservation_Reservation_ReservationId",
                table: "AccommodationInReservation",
                column: "ReservationId",
                principalTable: "Reservation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_AspNetUsers_UserId",
                table: "Reservation",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
