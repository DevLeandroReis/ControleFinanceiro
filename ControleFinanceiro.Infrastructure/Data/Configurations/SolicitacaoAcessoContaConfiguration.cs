using ControleFinanceiro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFinanceiro.Infrastructure.Data.Configurations
{
    public class SolicitacaoAcessoContaConfiguration : IEntityTypeConfiguration<SolicitacaoAcessoConta>
    {
        public void Configure(EntityTypeBuilder<SolicitacaoAcessoConta> builder)
        {
            builder.ToTable("SolicitacoesAcessoConta");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Mensagem)
                .HasMaxLength(500);

            // Relacionamentos
            builder.HasOne(s => s.Solicitante)
                .WithMany(u => u.SolicitacoesEnviadas)
                .HasForeignKey(s => s.SolicitanteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Proprietario)
                .WithMany(u => u.SolicitacoesRecebidas)
                .HasForeignKey(s => s.ProprietarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Conta)
                .WithMany(c => c.Solicitacoes)
                .HasForeignKey(s => s.ContaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}