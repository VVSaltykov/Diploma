using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diploma.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMessagesFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Messages_MessagesId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_MessagesId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "MessagesId",
                table: "Files");

            migrationBuilder.AddColumn<List<string>>(
                name: "FilesIds",
                table: "Messages",
                type: "text[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilesIds",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "MessagesId",
                table: "Files",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_MessagesId",
                table: "Files",
                column: "MessagesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Messages_MessagesId",
                table: "Files",
                column: "MessagesId",
                principalTable: "Messages",
                principalColumn: "Id");
        }
    }
}
