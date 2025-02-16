namespace Saintber.EntityFrameworkCore
{
    /// <summary>
    /// 實體資料條件取得存取庫介面。
    /// </summary>
    /// <typeparam name="TEntity">實體資料型別。</typeparam>
    /// <typeparam name="TModel">資料模型型別。</typeparam>
    /// <typeparam name="TFilterModel">篩選資料模型型別。</typeparam>
    public interface IGetEntityStore<TEntity, TModel, TFilterModel> : IEntityStore<TEntity>
    {
        /// <summary>
        /// 取得符合篩選條件的實體資料。
        /// </summary>
        /// <param name="model">篩選資料模型。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>符合篩選條件的實體資料清單。</returns>
        Task<IQueryable<TEntity>> GetAsync(TFilterModel model, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得允許存取範圍內，符合篩選條件的實體資料。
        /// </summary>
        /// <param name="model">篩選資料模型。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>允許存取且符合篩選條件的實體資料清單。</returns>
        Task<IQueryable<TEntity>> GetAllowAsync(TFilterModel model, CancellationToken cancellationToken = default);

        /// <summary>
        /// 由查詢表達式取得資料模型清單。
        /// </summary>
        /// <param name="query">查詢表達式。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>資料模型清單。</returns>
        Task<IEnumerable<TModel>> GetAsync(IQueryable<TEntity> query, CancellationToken cancellationToken = default);
    }
}
