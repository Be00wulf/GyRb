using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GyRb.Migrations
{
    /// <inheritdoc />
    public partial class ThumbnailUrl_Fn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Pages");

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Pages",                     //posts?
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
