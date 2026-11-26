using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoteKeeper.Infrastructure.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class MigracaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false, collation: "Latin1_General_CI_AS"),
                    CriadaEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Prioridade = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Cor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Fixada = table.Column<bool>(type: "bit", nullable: false),
                    Arquivada = table.Column<bool>(type: "bit", nullable: false),
                    Excluida = table.Column<bool>(type: "bit", nullable: false),
                    ExcluidaEm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LembreteEm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CriadaEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadaEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotasCategorias",
                columns: table => new
                {
                    NotaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoriaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotasCategorias", x => new { x.NotaId, x.CategoriaId });
                    table.ForeignKey(
                        name: "FK_NotasCategorias_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotasCategorias_Notas_NotaId",
                        column: x => x.NotaId,
                        principalTable: "Notas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Titulo",
                table: "Categorias",
                column: "Titulo");

            migrationBuilder.CreateIndex(
                name: "IX_Notas_Excluida_Arquivada",
                table: "Notas",
                columns: new[] { "Excluida", "Arquivada" });

            migrationBuilder.CreateIndex(
                name: "IX_Notas_LembreteEm",
                table: "Notas",
                column: "LembreteEm");

            migrationBuilder.CreateIndex(
                name: "IX_NotasCategorias_CategoriaId",
                table: "NotasCategorias",
                column: "CategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotasCategorias");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Notas");
        }
    }
}
