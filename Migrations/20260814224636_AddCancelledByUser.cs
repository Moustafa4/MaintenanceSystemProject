using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaintenanceSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddCancelledByUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CancelledByUserId",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CancelledByUserId",
                table: "Tickets",
                column: "CancelledByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_CancelledByUserId",
                table: "Tickets",
                column: "CancelledByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_CancelledByUserId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_CancelledByUserId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "CancelledByUserId",
                table: "Tickets");
        }
    }
}
