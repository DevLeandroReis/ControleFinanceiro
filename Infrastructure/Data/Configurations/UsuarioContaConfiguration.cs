using ControleFinanceiro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFinanceiro.Infrastructure.Data.Configurations
{
    public class UsuarioContaConfiguration : IEntityTypeConfiguration<UsuarioConta>
    {
        public void Configure(EntityTypeBuilder<UsuarioConta> builder)
        {
            builder.ToTable("UsuarioContas");

            builder.HasKey(uc => uc.Id);

            // �ndice �nico para evitar duplica��o de usu�rio/conta
            builder.HasIndex(uc => new { uc.UsuarioId, uc.ContaId })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            // Relacionamentos j� configurados nas outras entidades
            builder.HasOne(uc => uc.Usuario)
                .WithMany(u => u.UsuarioContas)
                .HasForeignKey(uc => uc.UsuarioId);

            builder.HasOne(uc => uc.Conta)
                .WithMany(c => c.UsuarioContas)
                .HasForeignKey(uc => uc.ContaId);
        }
    }
}