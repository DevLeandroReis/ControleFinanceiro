using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleFinanceiro.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategoriasPadrao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Categorias padrão (globais) - RECEITA - Destacadas (mais utilizadas)
            var categorias = new[]
            {
                // Receitas - Destacadas
                ("'Salário'", "'Recebimento de salário mensal'", "'#28a745'", "true", "true", "1"),
                ("'Freelance'", "'Trabalhos e projetos freelance'", "'#20c997'", "true", "true", "1"),
                ("'Investimentos'", "'Rendimentos de investimentos'", "'#17a2b8'", "true", "true", "1"),
                
                // Receitas - Não destacadas
                ("'Vendas'", "'Receita de vendas de produtos ou serviços'", "'#6c757d'", "true", "false", "1"),
                ("'Bonificação'", "'Bônus e gratificações'", "'#6610f2'", "true", "false", "1"),
                ("'Aluguel Recebido'", "'Recebimento de aluguel de imóveis'", "'#fd7e14'", "true", "false", "1"),
                ("'Outras Receitas'", "'Receitas diversas'", "'#e83e8c'", "true", "false", "1"),
                
                // Despesas - Destacadas
                ("'Alimentação'", "'Supermercado e alimentação em geral'", "'#ffc107'", "true", "true", "2"),
                ("'Transporte'", "'Combustível, transporte público, uber'", "'#007bff'", "true", "true", "2"),
                ("'Moradia'", "'Aluguel, condomínio, IPTU'", "'#dc3545'", "true", "true", "2"),
                ("'Saúde'", "'Plano de saúde, medicamentos, consultas'", "'#dc3545'", "true", "true", "2"),
                ("'Contas de Consumo'", "'Luz, água, gás, internet, telefone'", "'#fd7e14'", "true", "true", "2"),
                ("'Educação'", "'Mensalidade escolar, cursos, livros'", "'#17a2b8'", "true", "true", "2"),
                
                // Despesas - Não destacadas
                ("'Lazer'", "'Cinema, shows, viagens, hobbies'", "'#e83e8c'", "true", "false", "2"),
                ("'Vestuário'", "'Roupas, calçados, acessórios'", "'#6610f2'", "true", "false", "2"),
                ("'Cuidados Pessoais'", "'Salão, barbearia, cosméticos'", "'#fd7e14'", "true", "false", "2"),
                ("'Seguros'", "'Seguro de vida, carro, residencial'", "'#6c757d'", "true", "false", "2"),
                ("'Impostos e Taxas'", "'Impostos diversos e taxas governamentais'", "'#dc3545'", "true", "false", "2"),
                ("'Presentes e Doações'", "'Presentes e contribuições beneficentes'", "'#e83e8c'", "true", "false", "2"),
                ("'Pets'", "'Veterinário, ração, acessórios para pets'", "'#ffc107'", "true", "false", "2"),
                ("'Manutenção Veículo'", "'Manutenção e reparos do veículo'", "'#007bff'", "true", "false", "2"),
                ("'Assinaturas'", "'Netflix, Spotify, revistas e outros serviços'", "'#6610f2'", "true", "false", "2"),
                ("'Outras Despesas'", "'Despesas diversas'", "'#6c757d'", "true", "false", "2")
            };

            foreach (var (nome, descricao, cor, ativo, destacada, tipo) in categorias)
            {
                migrationBuilder.Sql($@"
                    INSERT INTO ""Categorias"" (""Id"", ""Nome"", ""Descricao"", ""Cor"", ""Ativo"", ""Destacada"", ""Tipo"", ""CreatedAt"", ""UpdatedAt"", ""IsDeleted"")
                    SELECT 
                        gen_random_uuid(),
                        {nome},
                        {descricao},
                        {cor},
                        {ativo},
                        {destacada},
                        {tipo},
                        timezone('utc', now()),
                        timezone('utc', now()),
                        false
                    WHERE NOT EXISTS (
                        SELECT 1 FROM ""Categorias"" cat 
                        WHERE cat.""Nome"" = {nome} AND cat.""IsDeleted"" = false
                    );
                ");
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove as categorias padrão criadas por esta migration
            var categoriasPadrao = new[]
            {
                // Receitas
                "Salário", "Freelance", "Investimentos", "Vendas", "Bonificação", 
                "Aluguel Recebido", "Outras Receitas",
                // Despesas
                "Alimentação", "Transporte", "Moradia", "Saúde", "Contas de Consumo",
                "Educação", "Lazer", "Vestuário", "Cuidados Pessoais", "Seguros",
                "Impostos e Taxas", "Presentes e Doações", "Pets", "Manutenção Veículo",
                "Assinaturas", "Outras Despesas"
            };

            foreach (var categoria in categoriasPadrao)
            {
                migrationBuilder.Sql($@"
                    DELETE FROM ""Categorias"" 
                    WHERE ""Nome"" = '{categoria}';
                ");
            }
        }
    }
}
