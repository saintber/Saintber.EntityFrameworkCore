using System.Collections;
using System.Linq.Expressions;

namespace Saintber.EntityFrameworkCore
{
    public static class LinqExtensions
    {
        /// <summary>
        /// 若已有查詢表達式 <paramref name="querySource"/> 則接續篩選，若未建立查詢表達式 <paramref name="querySource"/>
        /// 則以完整的查詢 <paramref name="queryFull"/> 建立查詢表達式後篩選，並回傳篩選後的查詢表達式。
        /// </summary>
        /// <typeparam name="TSource">實體資料型別。</typeparam>
        /// <param name="querySource">查詢表達式。</param>
        /// <param name="queryFull">完整實體資料查詢表達式。</param>
        /// <param name="predicate">篩選判斷式。</param>
        /// <returns>篩選後的查詢表達式。</returns>
        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> querySource
            , IQueryable<TSource> queryFull, Expression<Func<TSource, bool>> predicate)
            => (querySource ?? queryFull).Where(predicate);

        /// <summary>
        /// 在 <paramref name="shouldFilter"/> 為 true 時，確保 <paramref name="source"/> 不為 null，
        /// 若為 null，則拋出 <paramref name="entityName"/> 的查詢不受支援的例外狀況，
        /// 這通常是因為未註冊對應的查詢存取庫 (Store)。
        /// </summary>
        /// <typeparam name="TSource">實體資料型別。</typeparam>
        /// <param name="source">查詢表達式。</param>
        /// <param name="shouldFilter">是否執行篩選。</param>
        /// <param name="entityName">查詢的實體名稱 (用於錯誤訊息)。</param>
        /// <returns>回傳 <paramref name="shouldFilter"/>，以便用於條件篩選。</returns>
        /// <exception cref="NotSupportedException">
        /// 若 <paramref name="shouldFilter"/> 為 true，且 <paramref name="source"/> 為 null，
        /// 則拋出例外，表示 <paramref name="entityName"/> 的查詢不受支援，
        /// 可能是因為未註冊對應的查詢存取庫 (Store)。
        /// </exception>
        public static bool EnsureQueryableAvailable<TSource>(this IQueryable<TSource> source, bool shouldFilter, string entityName)
        {
            if (source == null && shouldFilter)
                throw new NotSupportedException($"Querying for '{entityName}' is not supported because the corresponding repository is not registered.");
            return shouldFilter;
        }

        /// <summary>
        /// 確保篩選條件，若 <paramref name="shouldFilter"/> 為 true，則執行篩選。
        /// </summary>
        /// <typeparam name="TSource">實體資料型別。</typeparam>
        /// <param name="sources">查詢表達式。</param>
        /// <param name="sourceName">實體資料集合的名稱。</param>
        /// <param name="queryFull">完整實體資料查詢表達式。</param>
        /// <param name="filters">篩選條件清單，包含條件判斷與篩選表達式。</param>
        /// <returns>篩選後的查詢表達式。</returns>
        public static IQueryable<TSource> ApplyFilters<TSource>(this IQueryable<TSource> sources
            , string sourceName, IQueryable<TSource> queryFull
            , params (bool shouldFilter, Func<Expression<Func<TSource, bool>>>)[] filters)
        {
            foreach (var (shouldFilter, filter) in filters)
            {
                if (queryFull.EnsureQueryableAvailable(shouldFilter, sourceName))
                    sources = sources.Where(queryFull, filter());
            }
            return sources;
        }
    }
}
