using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diploma.API.Migrations
{
    /// <inheritdoc />
    public partial class updateMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecepientId",
                table: "Messages");

            migrationBuilder.AddColumn<List<long>>(
                name: "RecepientIds",
                table: "Messages",
                type: "bigint[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecepientIds",
                table: "Messages");

            migrationBuilder.AddColumn<long>(
                name: "RecepientId",
                table: "Messages",
                type: "bigint",
                nullable: true);
        }
    }
}
