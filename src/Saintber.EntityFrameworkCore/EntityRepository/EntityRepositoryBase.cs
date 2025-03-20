using System.Transactions;

namespace Saintber.EntityFrameworkCore
{
    /// <summary>
    /// 實體資料存取知識庫共用基底類別。
    /// </summary>
    public abstract class EntityRepositoryBase
    {
        #region == 檢核 ==
        /// <summary>
        /// 檢核物件是否為空值。
        /// </summary>
        /// <typeparam name="T">物件型別。</typeparam>
        /// <param name="obj">待檢核物件。</param>
        /// <param name="name">物件名稱。</param>
        /// <exception cref="ArgumentNullException">若物件為空值時擲回的例外狀況。</exception>
        protected void ThrowIfNull<T>(T obj, string name)
        {
            if (obj == null) throw new ArgumentNullException(name);
        }
        #endregion

        #region == 輔助參數 ==
        /// <summary>
        /// 取得當前國際標準時間。
        /// </summary>
        public static DateTime UtcNow => DbContextExtensions.UtcNow;
        #endregion

        #region == 啟動交易 ==
        /// <summary>
        /// 交易逾時時間。
        /// </summary>
        public virtual TimeSpan TransactionTimeout { get; set; } = new TimeSpan(0, 5, 0);

        /// <summary>
        /// 啟動交易以執行函數。
        /// </summary>
        /// <typeparam name="T">函數回傳型別。</typeparam>
        /// <param name="func">執行函數。</param>
        /// <returns>非同步作業。</returns>
        public Task<T> TransactionAsync<T>(Func<Task<T>> func)
        {
            return TransactionAsync(func, this.TransactionTimeout);
        }
        /// <summary>
        /// 啟動交易以執行函數。
        /// </summary>
        /// <typeparam name="T">函數回傳型別。</typeparam>
        /// <param name="func">執行函數。</param>
        /// <param name="timeout">交易逾時時間。</param>
        /// <returns>非同步作業。</returns>
        public static async Task<T> TransactionAsync<T>(Func<Task<T>> func, TimeSpan timeout)
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.RepeatableRead, Timeout = timeout }
                , TransactionScopeAsyncFlowOption.Enabled))
            {
                var response = await func();
                ts.Complete();
                return response;
            }
        }
        /// <summary>
        /// 啟動交易以執行函數。
        /// </summary>
        /// <param name="func">執行函數。</param>
        /// <returns>非同步作業。</returns>
        public Task TransactionAsync(Func<Task> func)
        {
            return TransactionAsync(func, this.TransactionTimeout);
        }
        /// <summary>
        /// 啟動交易以執行函數。
        /// </summary>
        /// <param name="func">執行函數。</param>
        /// <param name="timeout">交易逾時時間。</param>
        /// <returns>非同步作業。</returns>
        public static async Task TransactionAsync(Func<Task> func, TimeSpan timeout)
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.RepeatableRead, Timeout = timeout }, TransactionScopeAsyncFlowOption.Enabled))
            {
                await func();
                ts.Complete();
            }
        }
        #endregion
    }
}
