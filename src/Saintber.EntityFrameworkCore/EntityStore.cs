using Microsoft.EntityFrameworkCore;

namespace Saintber.EntityFrameworkCore
{
    /// <summary>
    /// 實體資料存取庫。
    /// </summary>
    /// <typeparam name="TDbContext">資料庫連線實體型別。</typeparam>
    /// <typeparam name="T">實體資料型別。</typeparam>
    public class EntityStore<TDbContext, T> : IEntityStore<T>
        where TDbContext : DbContext
        where T : class
    {
        private readonly TDbContext dbContext;

        public EntityStore(TDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            dbContext.Set<T>().AddRange(entities);
            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public Task<IQueryable<T>> GetAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(dbContext.Set<T>().AsQueryable());
        }

        public async Task UpdateAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                dbContext.Entry(entity).State = EntityState.Modified;
            }
            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            dbContext.Set<T>().RemoveRange(entities);
            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
