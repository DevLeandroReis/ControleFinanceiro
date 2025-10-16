using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleFinanceiro.Migrations
{
    /// <inheritdoc />
    public partial class RemoverContaIdDeCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_Contas_ContaId",
                table: "Categorias");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_ContaId",
                table: "Categorias");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_Nome_ContaId",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "ContaId",
                table: "Categorias");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Nome",
                table: "Categorias",
                column: "Nome",
                unique: true,
                filter: "\"IsDeleted\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categorias_Nome",
                table: "Categorias");

            migrationBuilder.AddColumn<Guid>(
                name: "ContaId",
                table: "Categorias",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_ContaId",
                table: "Categorias",
                column: "ContaId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Nome_ContaId",
                table: "Categorias",
                columns: new[] { "Nome", "ContaId" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_Contas_ContaId",
                table: "Categorias",
                column: "ContaId",
                principalTable: "Contas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
