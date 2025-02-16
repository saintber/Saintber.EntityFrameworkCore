namespace Saintber.EntityFrameworkCore
{
    /// <summary>
    /// 實體資料存取庫。
    /// </summary>
    /// <typeparam name="TEntity">實體資料型別。</typeparam>
    public interface IEntityStore<TEntity>
    {
        /// <summary>
        /// 建立實體資料。
        /// </summary>
        /// <param name="entities">實體資料清單。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>非同步作業。</returns>
        Task CreateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得所有實體資料。
        /// </summary>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>實體資料清單。</returns>
        Task<IQueryable<TEntity>> GetAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 刪除實體資料。
        /// </summary>
        /// <param name="entities">實體資料清單。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>非同步作業。</returns>
        Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// 異動實體資料。
        /// </summary>
        /// <param name="entities">實體資料清單。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>非同步作業。</returns>
        Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    }
}
