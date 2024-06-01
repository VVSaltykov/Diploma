using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diploma.API.Migrations
{
    /// <inheritdoc />
    public partial class anotherUpdateMessagesFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
