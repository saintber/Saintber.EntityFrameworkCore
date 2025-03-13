namespace Saintber.EntityFrameworkCore.Abstractions
{
    /// <summary>
    /// 定義篩選器介面，用於根據使用者的存取權限範圍，篩選可存取的資料項目。
    /// </summary>
    /// <typeparam name="T">要篩選的資料型別。</typeparam>
    public interface IAccessibleFilter<T>
    {
        /// <summary>
        /// 根據使用者的存取權限，篩選符合條件的資料項目。
        /// </summary>
        /// <param name="query">待篩選的查詢物件。</param>
        /// <param name="cancellationToken">可選的取消標記，用於取消非同步操作。</param>
        /// <returns>篩選後的 <see cref="IQueryable{T}"/> 查詢結果。</returns>
        Task<IQueryable<T>> WhereAsync(IQueryable<T> query, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 定義篩選器介面，用於根據使用者的存取權限範圍，篩選可存取的資料項目。
    /// </summary>
    /// <typeparam name="T">要篩選的資料型別。</typeparam>
    /// <typeparam name="TFilter">額外的篩選條件型別。</typeparam>
    public interface IAccessibleFilter<T, TFilter>
    {
        /// <summary>
        /// 根據使用者的存取權限，篩選符合條件的資料項目。
        /// </summary>
        /// <param name="query">待篩選的查詢物件。</param>
        /// <param name="filter">篩選條件，用於進一步限制查詢結果。</param>
        /// <param name="cancellationToken">可選的取消標記，用於取消非同步操作。</param>
        /// <returns>篩選後的 <see cref="IQueryable{T}"/> 查詢結果。</returns>
        Task<IQueryable<T>> WhereAsync(IQueryable<T> query, TFilter filter, CancellationToken cancellationToken = default);
    }
}
