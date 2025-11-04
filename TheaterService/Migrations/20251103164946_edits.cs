using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheaterService.Migrations
{
    /// <inheritdoc />
    public partial class edits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "Seats",
                newName: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ShowTimeSeats_Showtime_Id",
                table: "ShowTimeSeats",
                column: "Showtime_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowTimeSeats_Showtimes_Showtime_Id",
                table: "ShowTimeSeats",
                column: "Showtime_Id",
                principalTable: "Showtimes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowTimeSeats_Showtimes_Showtime_Id",
                table: "ShowTimeSeats");

            migrationBuilder.DropIndex(
                name: "IX_ShowTimeSeats_Showtime_Id",
                table: "ShowTimeSeats");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Seats",
                newName: "isActive");
        }
    }
}
