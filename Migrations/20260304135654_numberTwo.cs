using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace videoapi.Migrations
{
    /// <inheritdoc />
    public partial class numberTwo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Videos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Videos",
                type: "time(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "thumbnailPath",
                table: "Videos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "thumbnailPath",
                table: "Videos");
        }
    }
}
