using Microsoft.EntityFrameworkCore;

namespace Saintber.EntityFrameworkCore
{
    public static class EntityStoreExtensions
    {
        /// <summary>
        /// 建立實體資料。
        /// </summary>
        /// <typeparam name="T">實體資料型別。</typeparam>
        /// <param name="store">實體資料存取庫。</param>
        /// <param name="entity">實體資料建立資料。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>非同步作業。</returns>
        public static Task CreateAsync<T>(this IEntityStore<T> store, T entity, CancellationToken cancellationToken = default)
            => store.CreateAsync(new[] { entity }, cancellationToken);

        /// <summary>
        /// 異動實體資料。
        /// </summary>
        /// <typeparam name="T">實體資料型別。</typeparam>
        /// <param name="store">實體資料存取庫。</param>
        /// <param name="entity">實體資料異動資料。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>非同步作業。</returns>
        public static Task UpdateAsync<T>(this IEntityStore<T> store, T entity, CancellationToken cancellationToken = default)
            where T : class
            => store.UpdateAsync(new[] { entity }, cancellationToken);

        /// <summary>
        /// 刪除實體資料。
        /// </summary>
        /// <typeparam name="T">實體資料型別。</typeparam>
        /// <param name="store">實體資料存取庫。</param>
        /// <param name="entity">實體資料刪除資料。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>非同步作業。</returns>
        public static Task DeleteAsync<T>(this IEntityStore<T> store, T entity, CancellationToken cancellationToken = default)
            where T : class
            => store.DeleteAsync(new[] { entity }, cancellationToken);

        /// <summary>
        /// 取得可存取範圍內所有實體資料的查詢表達式。
        /// </summary>
        /// <typeparam name="T">實體資料型別。</typeparam>
        /// <typeparam name="TFilterModel">篩選資料模型型別。</typeparam>
        /// <param name="store">實體資料存取庫介面。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>表示可存取範圍內所有實體資料的查詢表達式。</returns>
        public static Task<IQueryable<T>> GetAllowedAsync<T, TFilterModel>(this IEntityStore<T, TFilterModel> store
            , CancellationToken cancellationToken = default)
            where TFilterModel : new()
            => store.GetAllowedAsync(new TFilterModel { }, cancellationToken);

        /// <summary>
        /// 取得可存取範圍內符合篩選條件的實體資料清單。
        /// </summary>
        /// <typeparam name="T">實體資料型別。</typeparam>
        /// <typeparam name="TFilterModel">篩選資料模型型別。</typeparam>
        /// <param name="store">實體資料存取庫介面。</param>
        /// <param name="filter">篩選條件。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>包含可存取範圍內符合篩選條件的實體資料清單。</returns>
        public static async Task<List<T>> GetAllowedListAsync<T, TFilterModel>(this IEntityStore<T, TFilterModel> store
            , TFilterModel filter, CancellationToken cancellationToken = default)
            where TFilterModel : new()
        {
            var queryable = await store.GetAllowedAsync(filter, cancellationToken).ConfigureAwait(false);
            return await queryable.ToListAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
