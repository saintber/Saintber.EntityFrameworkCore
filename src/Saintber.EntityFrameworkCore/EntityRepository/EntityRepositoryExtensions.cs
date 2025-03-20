namespace Saintber.EntityFrameworkCore
{
    /// <summary>
    /// 實體資料存取庫擴充方法。
    /// </summary>
    public static class EntityRepositoryExtensions
    {
        #region == 更新戳記 ==
        /// <summary>
        /// 轉換時間戳記至字串。
        /// </summary>
        /// <param name="time">時間戳記。</param>
        /// <returns>異動戳記字串。</returns>
        public static string ToStamp(this DateTime time) => time.ToString("yyyy/MM/dd HH:mm:ss.ff");

        /// <summary>
        /// 轉換時間戳記至字串。
        /// </summary>
        /// <param name="time">時間戳記。</param>
        /// <returns>異動戳記字串。</returns>
        public static string? ToStamp(this DateTime? time) => time?.ToStamp();

        /// <summary>
        /// 轉換時間戳記至字串。
        /// </summary>
        /// <param name="time">時間戳記。</param>
        /// <returns>異動戳記字串。</returns>
        public static string ToStamp(this DateTimeOffset time) => time.ToUniversalTime().ToString("yyyy/MM/dd HH:mm:ss.ff");

        /// <summary>
        /// 轉換時間戳記至字串。
        /// </summary>
        /// <param name="time">時間戳記。</param>
        /// <returns>異動戳記字串。</returns>
        public static string? ToStamp(this DateTimeOffset? time) => time?.ToUniversalTime().ToStamp();
        #endregion
    }
}
