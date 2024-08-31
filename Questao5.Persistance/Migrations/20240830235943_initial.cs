using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Questao5.Persistance.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContaCorrente",
                columns: table => new
                {
                    IdContaCorrente = table.Column<Guid>(type: "TEXT", nullable: false),
                    Numero = table.Column<int>(type: "INTEGER", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContaCorrente", x => x.IdContaCorrente);
                });

            migrationBuilder.CreateTable(
                name: "IdEmpotencia",
                columns: table => new
                {
                    ChaveIdempotencia = table.Column<Guid>(type: "TEXT", nullable: false),
                    Requisicao = table.Column<string>(type: "TEXT", nullable: true),
                    Resultado = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdEmpotencia", x => x.ChaveIdempotencia);
                });

            migrationBuilder.CreateTable(
                name: "Movimento",
                columns: table => new
                {
                    IdMovimento = table.Column<Guid>(type: "TEXT", nullable: false),
                    IdContaCorrente = table.Column<Guid>(type: "TEXT", nullable: false),
                    DataMovimento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TipoMovimento = table.Column<char>(type: "TEXT", nullable: false),
                    Valor = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimento", x => x.IdMovimento);
                    table.ForeignKey(
                        name: "FK_Movimento_ContaCorrente_IdContaCorrente",
                        column: x => x.IdContaCorrente,
                        principalTable: "ContaCorrente",
                        principalColumn: "IdContaCorrente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movimento_IdContaCorrente",
                table: "Movimento",
                column: "IdContaCorrente");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdEmpotencia");

            migrationBuilder.DropTable(
                name: "Movimento");

            migrationBuilder.DropTable(
                name: "ContaCorrente");
        }
    }
}
