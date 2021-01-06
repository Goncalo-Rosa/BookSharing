using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookSharing.Migrations.Book
{
    public partial class Books_Initial_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmailCreator = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    BookName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    BookDescription = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    Author = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    Contact = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
