using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diploma.API.Migrations
{
    /// <inheritdoc />
    public partial class anotherupdateMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecepientIds",
                table: "Messages",
                newName: "RecepientInTelegramIds");

            migrationBuilder.AddColumn<List<Guid?>>(
                name: "RecepientInWebIds",
                table: "Messages",
                type: "uuid[]",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Messages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecepientInWebIds",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "RecepientInTelegramIds",
                table: "Messages",
                newName: "RecepientIds");
        }
    }
}
