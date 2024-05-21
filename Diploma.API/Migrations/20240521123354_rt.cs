using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diploma.API.Migrations
{
    /// <inheritdoc />
    public partial class rt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<long?>>(
                name: "RecepientIds",
                table: "Messages",
                type: "bigint[]",
                nullable: false,
                oldClrType: typeof(List<long?>),
                oldType: "bigint[]",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<long?>>(
                name: "RecepientIds",
                table: "Messages",
                type: "bigint[]",
                nullable: true,
                oldClrType: typeof(List<long?>),
                oldType: "bigint[]");
        }
    }
}
