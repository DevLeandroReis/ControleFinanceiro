using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFinanceiro.Infrastructure.Data.Configurations
{
    public class LancamentoConfiguration : IEntityTypeConfiguration<Lancamento>
    {
        public void Configure(EntityTypeBuilder<Lancamento> builder)
        {
            builder.ToTable("Lancamentos");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Descricao)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Valor)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.DataVencimento)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(x => x.DataPagamento)
                .HasColumnType("date");

            builder.Property(x => x.Tipo)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(StatusLancamento.Pendente);

            builder.Property(x => x.Observacoes)
                .HasMaxLength(1000);

            builder.Property(x => x.EhRecorrente)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.TipoRecorrencia)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(TipoRecorrencia.Nenhuma);

            builder.Property(x => x.QuantidadeParcelas);
            builder.Property(x => x.ParcelaAtual);
            builder.Property(x => x.LancamentoPaiId);

            builder.Property(x => x.CategoriaId)
                .IsRequired();

            builder.Property(x => x.ContaId)
                .IsRequired();

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
            builder.HasIndex(x => x.DataVencimento);
            builder.HasIndex(x => x.DataPagamento);
            builder.HasIndex(x => x.Tipo);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.CategoriaId);
            builder.HasIndex(x => x.ContaId);
            builder.HasIndex(x => x.EhRecorrente);
            builder.HasIndex(x => x.LancamentoPaiId);
            builder.HasIndex(x => x.IsDeleted);

            // Relacionamentos
            builder.HasOne(x => x.Categoria)
                .WithMany(x => x.Lancamentos)
                .HasForeignKey(x => x.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Conta)
                .WithMany(c => c.Lancamentos)
                .HasForeignKey(x => x.ContaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.LancamentoPai)
                .WithMany(x => x.LancamentosFilhos)
                .HasForeignKey(x => x.LancamentoPaiId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}