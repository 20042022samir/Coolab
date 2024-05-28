using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoolAppProject.Migrations
{
    /// <inheritdoc />
    public partial class addedcreatedbyuserblog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CreatedByUser",
                table: "Blogs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUser",
                table: "Blogs");
        }
    }
}
