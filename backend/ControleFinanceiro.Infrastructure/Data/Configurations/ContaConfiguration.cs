using ControleFinanceiro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFinanceiro.Infrastructure.Data.Configurations
{
    public class ContaConfiguration : IEntityTypeConfiguration<Conta>
    {
        public void Configure(EntityTypeBuilder<Conta> builder)
        {
            builder.ToTable("Contas");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Descricao)
                .HasMaxLength(500);

            // Relacionamentos
            builder.HasOne(c => c.Proprietario)
                .WithMany()
                .HasForeignKey(c => c.ProprietarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.UsuarioContas)
                .WithOne(uc => uc.Conta)
                .HasForeignKey(uc => uc.ContaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Lancamentos)
                .WithOne(l => l.Conta)
                .HasForeignKey(l => l.ContaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Solicitacoes)
                .WithOne(s => s.Conta)
                .HasForeignKey(s => s.ContaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}