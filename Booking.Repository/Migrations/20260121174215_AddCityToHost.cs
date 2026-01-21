using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddCityToHost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hosts_Countries_CountryId",
                table: "Hosts");

            migrationBuilder.AddColumn<Guid>(
                name: "CityId",
                table: "Hosts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hosts_CityId",
                table: "Hosts",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hosts_Cities_CityId",
                table: "Hosts",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Hosts_Countries_CountryId",
                table: "Hosts",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hosts_Cities_CityId",
                table: "Hosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Hosts_Countries_CountryId",
                table: "Hosts");

            migrationBuilder.DropIndex(
                name: "IX_Hosts_CityId",
                table: "Hosts");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Hosts");

            migrationBuilder.AddForeignKey(
                name: "FK_Hosts_Countries_CountryId",
                table: "Hosts",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
