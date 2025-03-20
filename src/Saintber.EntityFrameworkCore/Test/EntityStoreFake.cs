using Microsoft.EntityFrameworkCore;
using Pic.Package.Abstractions;
using Saintber.EntityFrameworkCore.Abstractions;

namespace Saintber.EntityFrameworkCore.Test
{
    /// <summary>
    /// 以記憶體虛擬 <see cref="IEntityStore{T}"/> 存取庫。
    /// </summary>
    [Obsolete("此類別已過時，請使 SQLite 進行 EntityRepository 測試")]
    public class EntityStoreFake<T> : EntityStoreFake<T, string>
        where T : class
    {
        public EntityStoreFake(
            IAlterUserProvider<string> userIdProvider) : base(userIdProvider)
        {
        }
    }

    /// <summary>
    /// 以記憶體虛擬 <see cref="IEntityStore{T}"/> 存取庫。
    /// </summary>
    /// <typeparam name="T">實體資料型別。</typeparam>
    /// <typeparam name="TAlterUser">資料異動人員資訊型別。</typeparam>
    [Obsolete("此類別已過時，請使 SQLite 進行 EntityRepository 測試")]
    public class EntityStoreFake<T, TAlterUser> : IEntityStore<T>
        where T : class
    {
        protected readonly DbContextFake<T> context;
        private readonly IAlterUserProvider<TAlterUser> userIdProvider;

        public EntityStoreFake
            (IAlterUserProvider<TAlterUser> userIdProvider)
        {
            this.userIdProvider = userIdProvider;
            context = DbContextFake<T>.Create();
        }

        /// <summary>
        /// 取得或設定取得資料異動時間函式。
        /// </summary>
        public Func<DateTime> CurrentTime { get; set; } = () => DbContextExtensions.UtcNow;

        /// <summary>
        /// 建立實體資料，目前僅支援 Guid, int 與 long。
        /// </summary>
        /// <param name="entities">建立實體資料清單。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>非同步作業。</returns>
        public async Task CreateAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            var propertyId = typeof(T).GetProperty("Id");
            var propertyCreateTime = typeof(T).GetProperty(nameof(IHasCreateInfoGetter.CreateTime));
            var propertyCreateUser = typeof(T).GetProperty(nameof(IHasCreateInfoGetter.CreateUser));
            var propertyUpdateTime = typeof(T).GetProperty(nameof(IHasUpdateInfoGetter.UpdateTime));
            var propertyUpdateUser = typeof(T).GetProperty(nameof(IHasUpdateInfoGetter.UpdateUser));
            var propertyDeleted = typeof(T).GetProperty(nameof(IHasDeletedGetter.Deleted));

            var alterUser = await userIdProvider.GetAlterUserAsync(cancellationToken).ConfigureAwait(false);
            var fullEntities = await context.Entities.ToListAsync(cancellationToken).ConfigureAwait(false);
            foreach (var entity in entities)
            {
                var id = propertyId?.GetValue(entity);
                if ((id is int intId && intId == default))
                {
                    var max = fullEntities.Max(x => (int?)propertyId?.GetValue(x)) ?? 0;
                    propertyId?.SetValue(entity, max + 1);
                }
                else if (id is long longId && longId == default)
                {
                    var max = fullEntities.Max(x => (long?)propertyId?.GetValue(x)) ?? 0;
                    propertyId?.SetValue(entity, max + 1);
                }
                else if (id is Guid guidId && guidId == default)
                {
                    if (guidId == Guid.Empty) propertyId?.SetValue(entity, Guid.NewGuid());
                }
                propertyCreateUser?.SetValue(entity, alterUser);
                propertyCreateTime?.SetValue(entity, DbContextExtensions.UtcNow);
                propertyUpdateUser?.SetValue(entity, alterUser);
                propertyUpdateTime?.SetValue(entity, DbContextExtensions.UtcNow);
                propertyDeleted?.SetValue(entity, false);
            }
            context.Entities.AddRange(entities);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public Task<IQueryable<T>> GetAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(context.Entities.AsQueryable());
        }

        public async Task UpdateAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            var propertyUpdateTime = typeof(T).GetProperty(nameof(IHasUpdateInfoGetter.UpdateTime));
            var propertyUpdateUser = typeof(T).GetProperty(nameof(IHasUpdateInfoGetter.UpdateUser));

            var utcNow = new DateTimeOffset(DateTime.Now).ToUniversalTime().DateTime;
            var userId = await userIdProvider.GetAlterUserAsync(cancellationToken).ConfigureAwait(false);
            foreach (var entity in entities)
            {
                propertyUpdateUser?.SetValue(entity, userId);
                propertyUpdateTime?.SetValue(entity, utcNow);
                context.Entry(entity).State = EntityState.Modified;
            }
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            var propertyUpdateTime = typeof(T).GetProperty(nameof(IHasUpdateInfoGetter.UpdateTime));
            var propertyUpdateUser = typeof(T).GetProperty(nameof(IHasUpdateInfoGetter.UpdateUser));
            var propertyDeleted = typeof(T).GetProperty(nameof(IHasDeletedGetter.Deleted));

            if (propertyDeleted == null)
            {
                context.Entities.RemoveRange(entities);
            }
            else
            {
                var utcNow = new DateTimeOffset(DateTime.Now).ToUniversalTime().DateTime;
                var userId = await userIdProvider.GetAlterUserAsync(cancellationToken).ConfigureAwait(false);

                foreach (var entity in entities)
                {
                    propertyDeleted?.SetValue(entity, true);
                    propertyUpdateTime?.SetValue(entity, utcNow);
                    propertyUpdateUser?.SetValue(entity, userId);
                    context.Entry(entity).State = EntityState.Modified;
                }
            }
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
