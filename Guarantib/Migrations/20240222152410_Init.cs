using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guarantib.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "marques",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lien = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marques", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "produits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NumFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumAkuito = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumSerie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsClose = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_produits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "professionnels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    job = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KeepLoggedIn = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_professionnels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientProduit",
                columns: table => new
                {
                    ClientsId = table.Column<int>(type: "int", nullable: false),
                    ProduitsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProduit", x => new { x.ClientsId, x.ProduitsId });
                    table.ForeignKey(
                        name: "FK_ClientProduit_clients_ClientsId",
                        column: x => x.ClientsId,
                        principalTable: "clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientProduit_produits_ProduitsId",
                        column: x => x.ProduitsId,
                        principalTable: "produits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MarqueProduit",
                columns: table => new
                {
                    ProduitsId = table.Column<int>(type: "int", nullable: false),
                    marquesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarqueProduit", x => new { x.ProduitsId, x.marquesId });
                    table.ForeignKey(
                        name: "FK_MarqueProduit_marques_marquesId",
                        column: x => x.marquesId,
                        principalTable: "marques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MarqueProduit_produits_ProduitsId",
                        column: x => x.ProduitsId,
                        principalTable: "produits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProduitProfessionnel",
                columns: table => new
                {
                    ProduitsId = table.Column<int>(type: "int", nullable: false),
                    ProfessionnelsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProduitProfessionnel", x => new { x.ProduitsId, x.ProfessionnelsId });
                    table.ForeignKey(
                        name: "FK_ProduitProfessionnel_produits_ProduitsId",
                        column: x => x.ProduitsId,
                        principalTable: "produits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProduitProfessionnel_professionnels_ProfessionnelsId",
                        column: x => x.ProfessionnelsId,
                        principalTable: "professionnels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientProduit_ProduitsId",
                table: "ClientProduit",
                column: "ProduitsId");

            migrationBuilder.CreateIndex(
                name: "IX_MarqueProduit_marquesId",
                table: "MarqueProduit",
                column: "marquesId");

            migrationBuilder.CreateIndex(
                name: "IX_ProduitProfessionnel_ProfessionnelsId",
                table: "ProduitProfessionnel",
                column: "ProfessionnelsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientProduit");

            migrationBuilder.DropTable(
                name: "MarqueProduit");

            migrationBuilder.DropTable(
                name: "ProduitProfessionnel");

            migrationBuilder.DropTable(
                name: "clients");

            migrationBuilder.DropTable(
                name: "marques");

            migrationBuilder.DropTable(
                name: "produits");

            migrationBuilder.DropTable(
                name: "professionnels");
        }
    }
}
