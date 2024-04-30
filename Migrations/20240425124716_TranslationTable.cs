using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using WebApp.Models;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class TranslationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        migrationBuilder.CreateTable(
                name: "Translation",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginalText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TranslatedText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    translated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OriginalLanguageID = table.Column<int>(type: "int", nullable: false),
                    TranslatedLanguageID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Translation_Language_OriginalLanguageID",
                        column: x => x.OriginalLanguageID,
                        principalTable: "Language",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Translation_Language_TranslatedLanguageID",
                        column: x => x.TranslatedLanguageID,
                        principalTable: "Language",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
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
        }
    }
}
