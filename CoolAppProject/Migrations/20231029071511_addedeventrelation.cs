using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoolAppProject.Migrations
{
    /// <inheritdoc />
    public partial class addedeventrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "EventContacts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EvenntId",
                table: "EventContacts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "EventContacts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EventContacts_EventId",
                table: "EventContacts",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventContacts_Events_EventId",
                table: "EventContacts",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventContacts_Events_EventId",
                table: "EventContacts");

            migrationBuilder.DropIndex(
                name: "IX_EventContacts_EventId",
                table: "EventContacts");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "EventContacts");

            migrationBuilder.DropColumn(
                name: "EvenntId",
                table: "EventContacts");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "EventContacts");
        }
    }
}
