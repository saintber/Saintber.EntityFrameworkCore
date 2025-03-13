namespace Saintber.EntityFrameworkCore.Abstractions
{
    /// <summary>
    /// 定義一個篩選器介面，用於篩選「已授權」的資料項目。
    /// </summary>
    /// <typeparam name="T">要篩選的資料類型。</typeparam>
    public interface IAssignedFilter<T>
    {
        /// <summary>
        /// 根據使用者被授予的權限，篩選可存取的資料項目。
        /// </summary>
        /// <param name="query">待篩選的查詢物件。</param>
        /// <param name="cancellationToken">可選的取消標記，用於取消非同步操作。</param>
        /// <returns>篩選後的 <see cref="IQueryable{T}"/> 查詢結果。</returns>
        Task<IQueryable<T>> WhereAsync(IQueryable<T> query, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 定義一個篩選器介面，用於篩選「已授權」的資料項目。
    /// </summary>
    /// <typeparam name="T">要篩選的資料類型。</typeparam>
    /// <typeparam name="TFilter">額外的篩選條件型別。</typeparam>
    public interface IAssignedFilter<T, TFilter>
    {
        /// <summary>
        /// 根據使用者被授予的權限，篩選可存取的資料項目。
        /// </summary>
        /// <param name="query">待篩選的查詢物件。</param>
        /// <param name="filter">篩選條件，用於進一步限制查詢結果。</param>
        /// <param name="cancellationToken">可選的取消標記，用於取消非同步操作。</param>
        /// <returns>篩選後的 <see cref="IQueryable{T}"/> 查詢結果。</returns>
        Task<IQueryable<T>> WhereAsync(IQueryable<T> query, TFilter filter, CancellationToken cancellationToken = default);
    }
}
