using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleFinanceiro.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Cor = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lancamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Valor = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DataVencimento = table.Column<DateTime>(type: "date", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "date", nullable: true),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    Observacoes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    EhRecorrente = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    TipoRecorrencia = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    QuantidadeParcelas = table.Column<int>(type: "integer", nullable: true),
                    ParcelaAtual = table.Column<int>(type: "integer", nullable: true),
                    LancamentoPaiId = table.Column<Guid>(type: "uuid", nullable: true),
                    CategoriaId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lancamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lancamentos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lancamentos_Lancamentos_LancamentoPaiId",
                        column: x => x.LancamentoPaiId,
                        principalTable: "Lancamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Ativo",
                table: "Categorias",
                column: "Ativo");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_IsDeleted",
                table: "Categorias",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Nome",
                table: "Categorias",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lancamentos_CategoriaId",
                table: "Lancamentos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Lancamentos_DataPagamento",
                table: "Lancamentos",
                column: "DataPagamento");

            migrationBuilder.CreateIndex(
                name: "IX_Lancamentos_DataVencimento",
                table: "Lancamentos",
                column: "DataVencimento");

            migrationBuilder.CreateIndex(
                name: "IX_Lancamentos_EhRecorrente",
                table: "Lancamentos",
                column: "EhRecorrente");

            migrationBuilder.CreateIndex(
                name: "IX_Lancamentos_IsDeleted",
                table: "Lancamentos",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Lancamentos_LancamentoPaiId",
                table: "Lancamentos",
                column: "LancamentoPaiId");

            migrationBuilder.CreateIndex(
                name: "IX_Lancamentos_Status",
                table: "Lancamentos",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Lancamentos_Tipo",
                table: "Lancamentos",
                column: "Tipo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lancamentos");

            migrationBuilder.DropTable(
                name: "Categorias");
        }
    }
}
