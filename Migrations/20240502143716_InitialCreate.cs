using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Translation",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginalText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TranslatedText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    translated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OriginalLanguageID = table.Column<int>(type: "int", nullable: true),
                    TranslatedLanguageID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Translation_Language_OriginalLanguageID",
                        column: x => x.OriginalLanguageID,
                        principalTable: "Language",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Translation_Language_TranslatedLanguageID",
                        column: x => x.TranslatedLanguageID,
                        principalTable: "Language",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Translation_OriginalLanguageID",
                table: "Translation",
                column: "OriginalLanguageID");

            migrationBuilder.CreateIndex(
                name: "IX_Translation_TranslatedLanguageID",
                table: "Translation",
                column: "TranslatedLanguageID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Translation");

            migrationBuilder.DropTable(
                name: "Language");
        }
    }
}
