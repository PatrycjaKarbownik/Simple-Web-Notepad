using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Z02.Migrations
{
    public partial class AddRowVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "karbownik",
                table: "Note",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "karbownik",
                table: "Note",
                rowVersion: true,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Note_Title",
                schema: "karbownik",
                table: "Note",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_Title",
                schema: "karbownik",
                table: "Category",
                column: "Title",
                unique: true,
                filter: "[Title] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Note_Title",
                schema: "karbownik",
                table: "Note");

            migrationBuilder.DropIndex(
                name: "IX_Category_Title",
                schema: "karbownik",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "karbownik",
                table: "Note");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "karbownik",
                table: "Note",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 64);
        }
    }
}
