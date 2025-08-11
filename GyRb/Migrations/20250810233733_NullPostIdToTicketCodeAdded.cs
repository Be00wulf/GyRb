using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GyRb.Migrations
{
    /// <inheritdoc />
    public partial class NullPostIdToTicketCodeAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "TicketCodes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketCodes_PostId",
                table: "TicketCodes",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketCodes_Posts_PostId",
                table: "TicketCodes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketCodes_Posts_PostId",
                table: "TicketCodes");

            migrationBuilder.DropIndex(
                name: "IX_TicketCodes_PostId",
                table: "TicketCodes");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "TicketCodes");
        }
    }
}
