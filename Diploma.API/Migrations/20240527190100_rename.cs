using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diploma.API.Migrations
{
    /// <inheritdoc />
    public partial class rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Hashes_HashId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "HashId",
                table: "Users",
                newName: "SaltId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_HashId",
                table: "Users",
                newName: "IX_Users_SaltId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Hashes_SaltId",
                table: "Users",
                column: "SaltId",
                principalTable: "Hashes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Hashes_SaltId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "SaltId",
                table: "Users",
                newName: "HashId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_SaltId",
                table: "Users",
                newName: "IX_Users_HashId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Hashes_HashId",
                table: "Users",
                column: "HashId",
                principalTable: "Hashes",
                principalColumn: "Id");
        }
    }
}
