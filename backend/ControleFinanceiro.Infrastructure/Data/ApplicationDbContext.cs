using ControleFinanceiro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ControleFinanceiro.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Lancamento> Lancamentos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Conta> Contas { get; set; }
        public DbSet<UsuarioConta> UsuarioContas { get; set; }
        public DbSet<SolicitacaoAcessoConta> SolicitacoesAcessoConta { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplicar configurações
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Configurar filtros globais para soft delete
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(
                        GetIsDeletedFilter(entityType.ClrType));
                }
            }
        }

        private static LambdaExpression GetIsDeletedFilter(Type entityType)
        {
            var parameter = System.Linq.Expressions.Expression.Parameter(entityType, "e");
            var property = System.Linq.Expressions.Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
            var constant = System.Linq.Expressions.Expression.Constant(false);
            var equal = System.Linq.Expressions.Expression.Equal(property, constant);
            return System.Linq.Expressions.Expression.Lambda(equal, parameter);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}