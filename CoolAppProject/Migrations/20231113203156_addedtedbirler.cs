using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoolAppProject.Migrations
{
    /// <inheritdoc />
    public partial class addedtedbirler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tedbirler",
                table: "ContactWords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TedbirlerDesc",
                table: "ContactWords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tedbirler",
                table: "ContactWords");

            migrationBuilder.DropColumn(
                name: "TedbirlerDesc",
                table: "ContactWords");
        }
    }
}
