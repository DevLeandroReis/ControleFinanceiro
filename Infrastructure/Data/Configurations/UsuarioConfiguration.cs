using ControleFinanceiro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFinanceiro.Infrastructure.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.Property(u => u.SenhaHash)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.TokenConfirmacaoEmail)
                .HasMaxLength(255);

            builder.Property(u => u.TokenRecuperacaoSenha)
                .HasMaxLength(255);

            // Relacionamentos
            builder.HasMany(u => u.UsuarioContas)
                .WithOne(uc => uc.Usuario)
                .HasForeignKey(uc => uc.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.SolicitacoesEnviadas)
                .WithOne(s => s.Solicitante)
                .HasForeignKey(s => s.SolicitanteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.SolicitacoesRecebidas)
                .WithOne(s => s.Proprietario)
                .HasForeignKey(s => s.ProprietarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}