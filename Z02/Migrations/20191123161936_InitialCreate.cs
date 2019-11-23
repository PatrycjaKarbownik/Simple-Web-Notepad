using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Z02.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "karbownik");

            migrationBuilder.CreateTable(
                name: "Category",
                schema: "karbownik",
                columns: table => new
                {
                    CategoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Note",
                schema: "karbownik",
                columns: table => new
                {
                    NoteID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoteDate = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(maxLength: 64, nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.NoteID);
                });

            migrationBuilder.CreateTable(
                name: "NoteCategory",
                schema: "karbownik",
                columns: table => new
                {
                    NoteID = table.Column<int>(nullable: false),
                    CategoryID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteCategory", x => new { x.CategoryID, x.NoteID });
                    table.ForeignKey(
                        name: "FK_NoteCategory_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalSchema: "karbownik",
                        principalTable: "Category",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoteCategory_Note_NoteID",
                        column: x => x.NoteID,
                        principalSchema: "karbownik",
                        principalTable: "Note",
                        principalColumn: "NoteID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NoteCategory_NoteID",
                schema: "karbownik",
                table: "NoteCategory",
                column: "NoteID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoteCategory",
                schema: "karbownik");

            migrationBuilder.DropTable(
                name: "Category",
                schema: "karbownik");

            migrationBuilder.DropTable(
                name: "Note",
                schema: "karbownik");
        }
    }
}
