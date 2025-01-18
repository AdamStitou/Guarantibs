using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guarantib.Migrations
{
    /// <inheritdoc />
    public partial class suivisMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "suivis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Etape = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateSuivis = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dialogue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_suivis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProduitSuivis",
                columns: table => new
                {
                    ProduitsId = table.Column<int>(type: "int", nullable: false),
                    suivisId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProduitSuivis", x => new { x.ProduitsId, x.suivisId });
                    table.ForeignKey(
                        name: "FK_ProduitSuivis_produits_ProduitsId",
                        column: x => x.ProduitsId,
                        principalTable: "produits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProduitSuivis_suivis_suivisId",
                        column: x => x.suivisId,
                        principalTable: "suivis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfessionnelSuivis",
                columns: table => new
                {
                    ProfessionnelsId = table.Column<int>(type: "int", nullable: false),
                    suivisId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessionnelSuivis", x => new { x.ProfessionnelsId, x.suivisId });
                    table.ForeignKey(
                        name: "FK_ProfessionnelSuivis_professionnels_ProfessionnelsId",
                        column: x => x.ProfessionnelsId,
                        principalTable: "professionnels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfessionnelSuivis_suivis_suivisId",
                        column: x => x.suivisId,
                        principalTable: "suivis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProduitSuivis_suivisId",
                table: "ProduitSuivis",
                column: "suivisId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionnelSuivis_suivisId",
                table: "ProfessionnelSuivis",
                column: "suivisId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProduitSuivis");

            migrationBuilder.DropTable(
                name: "ProfessionnelSuivis");

            migrationBuilder.DropTable(
                name: "suivis");
        }
    }
}
