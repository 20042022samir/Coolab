using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoolAppProject.Migrations
{
    /// <inheritdoc />
    public partial class addedhomepagewords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HomeWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Saheler = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionSaheler = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecialCourse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Semiarlar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeminarlarDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecialEvent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Teachers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeachersDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommentDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Blogs = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BlogsDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MakeContact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeWords", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomeWords");
        }
    }
}
