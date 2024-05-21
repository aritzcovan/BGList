using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBGList.Migrations
{
    /// <inheritdoc />
    public partial class update_OwnerUsers_column_BoardGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OwnerUsers",
                table: "BoardGames",
                newName: "OwnedUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OwnedUsers",
                table: "BoardGames",
                newName: "OwnerUsers");
        }
    }
}
