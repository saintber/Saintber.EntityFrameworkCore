using Microsoft.EntityFrameworkCore;

namespace Saintber.EntityFrameworkCore
{
    [Obsolete("此類別已過時，請使用 EntityRepository 進行開發，並透過 SQLite 進行 EntityRepository 測試")]
    public static class EntityStoreExtensions
    {
        /// <summary>
        /// 建立實體資料。
        /// </summary>
        /// <typeparam name="TEntity">實體資料型別。</typeparam>
        /// <param name="store">實體資料存取庫。</param>
        /// <param name="entity">實體資料建立資料。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>非同步作業。</returns>
        public static Task CreateAsync<TEntity>(this IEntityStore<TEntity> store, TEntity entity, CancellationToken cancellationToken = default)
            => store.CreateAsync(new[] { entity }, cancellationToken);

        /// <summary>
        /// 異動實體資料。
        /// </summary>
        /// <typeparam name="TEntity">實體資料型別。</typeparam>
        /// <param name="store">實體資料存取庫。</param>
        /// <param name="entity">實體資料異動資料。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>非同步作業。</returns>
        public static Task UpdateAsync<TEntity>(this IEntityStore<TEntity> store, TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class
            => store.UpdateAsync(new[] { entity }, cancellationToken);

        /// <summary>
        /// 刪除實體資料。
        /// </summary>
        /// <typeparam name="TEntity">實體資料型別。</typeparam>
        /// <param name="store">實體資料存取庫。</param>
        /// <param name="entity">實體資料刪除資料。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>非同步作業。</returns>
        public static Task DeleteAsync<TEntity>(this IEntityStore<TEntity> store, TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class
            => store.DeleteAsync(new[] { entity }, cancellationToken);

        /// <summary>
        /// 取得可存取範圍內所有實體資料的查詢表達式。
        /// </summary>
        /// <typeparam name="TEntity">實體資料型別。</typeparam>
        /// <typeparam name="TFilterModel">篩選資料模型型別。</typeparam>
        /// <param name="store">實體資料存取庫介面。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>表示可存取範圍內所有實體資料的查詢表達式。</returns>
        public static Task<IQueryable<TEntity>> GetAllowedAsync<TEntity, TFilterModel>(this IEntityStore<TEntity, TFilterModel> store
            , CancellationToken cancellationToken = default)
            where TFilterModel : new()
            => store.GetAllowedAsync(new TFilterModel { }, cancellationToken);

        /// <summary>
        /// 取得符合篩選條件的資料模型清單。
        /// </summary>
        /// <typeparam name="TEntity">實體資料型別。</typeparam>
        /// <typeparam name="TFilterModel">篩選資料模型型別。</typeparam>
        /// <param name="store">實體資料存取庫介面。</param>
        /// <param name="filter">篩選條件。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>符合篩選條件的資料模型清單。</returns>
        public static async Task<List<TEntity>> GetListAsync<TEntity, TFilterModel>(this IEntityStore<TEntity, TFilterModel> store
            , TFilterModel filter, CancellationToken cancellationToken = default)
        {
            var queryable = await store.GetAsync(filter, cancellationToken).ConfigureAwait(false);
            return await queryable.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 取得可存取範圍內符合篩選條件的資料模型清單。
        /// </summary>
        /// <typeparam name="TEntity">資料模型型別。</typeparam>
        /// <typeparam name="TFilterModel">篩選資料模型型別。</typeparam>
        /// <param name="store">實體資料存取庫介面。</param>
        /// <param name="filter">篩選條件。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>可存取範圍內符合篩選條件的資料模型清單。</returns>
        public static async Task<List<TEntity>> GetAllowedListAsync<TEntity, TFilterModel>(this IEntityStore<TEntity, TFilterModel> store
            , TFilterModel filter, CancellationToken cancellationToken = default)
        {
            var queryable = await store.GetAllowedAsync(filter, cancellationToken).ConfigureAwait(false);
            return await queryable.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 取得符合篩選條件的單筆資料模型，若未取到資料模型則回傳 Null，若超過一筆則擲回例外狀況。
        /// </summary>
        /// <typeparam name="TEntity">資料模型型別。</typeparam>
        /// <typeparam name="TFilterModel">篩選資料模型型別。</typeparam>
        /// <param name="store">實體資料存取庫介面。</param>
        /// <param name="filter">篩選條件。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>符合篩選條件的單筆資料模型。</returns>
        public static async Task<TEntity?> SingleOrDefaultAsync<TEntity, TFilterModel>(this IEntityStore<TEntity, TFilterModel> store
            , TFilterModel filter, CancellationToken cancellationToken = default)
            => (await store.GetListAsync(filter, cancellationToken).ConfigureAwait(false)).SingleOrDefault();

        /// <summary>
        /// 取得可存取範圍內符合篩選條件的單筆資料模型，若未取到資料模型則回傳 Null，若超過一筆則擲回例外狀況。
        /// </summary>
        /// <typeparam name="TEntity">資料模型型別。</typeparam>
        /// <typeparam name="TFilterModel">篩選資料模型型別。</typeparam>
        /// <param name="store">實體資料存取庫介面。</param>
        /// <param name="filter">篩選條件。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>可存取範圍內符合篩選條件的單筆資料模型。</returns>
        public static async Task<TEntity?> SingleOrDefaultAllowedAsync<TEntity, TFilterModel>(this IEntityStore<TEntity, TFilterModel> store
            , TFilterModel filter, CancellationToken cancellationToken = default)
            => (await store.GetAllowedListAsync(filter, cancellationToken).ConfigureAwait(false)).SingleOrDefault();
    }
}
