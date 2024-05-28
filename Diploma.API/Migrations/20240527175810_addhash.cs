using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diploma.API.Migrations
{
    /// <inheritdoc />
    public partial class addhash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HashId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Hashes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HashSalt = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hashes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_HashId",
                table: "Users",
                column: "HashId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Hashes_HashId",
                table: "Users",
                column: "HashId",
                principalTable: "Hashes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Hashes_HashId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Hashes");

            migrationBuilder.DropIndex(
                name: "IX_Users_HashId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HashId",
                table: "Users");
        }
    }
}
