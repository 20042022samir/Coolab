using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoolAppProject.Migrations
{
    /// <inheritdoc />
    public partial class addedusertocontact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "CourseContacts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "CourseContacts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Contacts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "Contacts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseContacts_AppUserId1",
                table: "CourseContacts",
                column: "AppUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_AppUserId1",
                table: "Contacts",
                column: "AppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_AspNetUsers_AppUserId1",
                table: "Contacts",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseContacts_AspNetUsers_AppUserId1",
                table: "CourseContacts",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_AspNetUsers_AppUserId1",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseContacts_AspNetUsers_AppUserId1",
                table: "CourseContacts");

            migrationBuilder.DropIndex(
                name: "IX_CourseContacts_AppUserId1",
                table: "CourseContacts");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_AppUserId1",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "CourseContacts");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "CourseContacts");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "Contacts");
        }
    }
}
