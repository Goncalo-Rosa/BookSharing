using Microsoft.EntityFrameworkCore.Migrations;

namespace BookSharing.Migrations.Book
{
    public partial class Genero_Idioma_table_add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "genero",
                table: "Books",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "idioma",
                table: "Books",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "genero",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "idioma",
                table: "Books");
        }
    }
}
