using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MassageManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class TableBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TherapistId",
                table: "Bookings",
                column: "TherapistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Therapists_TherapistId",
                table: "Bookings",
                column: "TherapistId",
                principalTable: "Therapists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Therapists_TherapistId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_TherapistId",
                table: "Bookings");
        }
    }
}
