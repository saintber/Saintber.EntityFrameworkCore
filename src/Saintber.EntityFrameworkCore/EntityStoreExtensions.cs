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
        /// 取得存取範圍內所有實體資料的查詢表達式。
        /// </summary>
        /// <typeparam name="T">實體資料型別。</typeparam>
        /// <typeparam name="TModel">資料模型型別。</typeparam>
        /// <typeparam name="TFilterModel">篩選資料模型型別。</typeparam>
        /// <param name="store">實體資料條件取得存取庫介面。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>取得存取範圍內所有實體資料的查詢表達式。<</returns>
        public static async Task<IQueryable<T>> GetAllowAsync<T, TModel, TFilterModel>(this IGetEntityStore<T, TModel, TFilterModel> store
            , CancellationToken cancellationToken = default)
            where TFilterModel : new()
            => await store.GetAllowAsync(new TFilterModel { }, cancellationToken).ConfigureAwait(false);
    }
}
