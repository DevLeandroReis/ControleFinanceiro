using ControleFinanceiro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFinanceiro.Infrastructure.Data.Configurations
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categorias");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Descricao)
                .HasMaxLength(500);

            builder.Property(x => x.Cor)
                .HasMaxLength(7); // Para códigos de cores hex (#FFFFFF)

            builder.Property(x => x.Ativo)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp with time zone");

            builder.Property(x => x.UpdatedAt)
                .IsRequired()
                .HasColumnType("timestamp with time zone");

            builder.Property(x => x.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            // Índices
            builder.HasIndex(x => new { x.Nome, x.ContaId }).IsUnique()
                .HasFilter("[IsDeleted] = 0");
            builder.HasIndex(x => x.Ativo);
            builder.HasIndex(x => x.IsDeleted);
            builder.HasIndex(x => x.ContaId);

            // Relacionamentos
            builder.HasOne(x => x.Conta)
                .WithMany(c => c.Categorias)
                .HasForeignKey(x => x.ContaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Lancamentos)
                .WithOne(x => x.Categoria)
                .HasForeignKey(x => x.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}