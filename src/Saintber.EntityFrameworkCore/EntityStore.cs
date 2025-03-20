using Microsoft.EntityFrameworkCore;

namespace Saintber.EntityFrameworkCore
{
    /// <summary>
    /// 實體資料存取庫。
    /// </summary>
    /// <typeparam name="TDbContext">資料庫連線實體型別。</typeparam>
    /// <typeparam name="TEntity">實體資料型別。</typeparam>
    [Obsolete("此類別已過時，請使用 EntityRepository 進行開發，並透過 SQLite 進行 EntityRepository 測試")]
    public class EntityStore<TDbContext, TEntity> : IEntityStore<TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        private readonly TDbContext dbContext;

        public EntityStore(TDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            dbContext.Set<TEntity>().AddRange(entities);
            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public Task<IQueryable<TEntity>> GetAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(dbContext.Set<TEntity>().AsQueryable());
        }

        public async Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                dbContext.Entry(entity).State = EntityState.Modified;
            }
            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            dbContext.Set<TEntity>().RemoveRange(entities);
            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
