using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MassageManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class ReviseBookingModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PayerId",
                table: "Bookings",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Bookings",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "Bookings",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayerId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Bookings");
        }
    }
}
