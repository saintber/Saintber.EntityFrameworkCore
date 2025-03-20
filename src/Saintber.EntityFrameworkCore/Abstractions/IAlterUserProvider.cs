namespace Saintber.EntityFrameworkCore.Abstractions
{
    /// <summary>
    /// 資料異動人員資訊提供者。
    /// </summary>
    [Obsolete("此方法已過時，請使用 Saintber.Abstractions 套件的 IUserIdProvider / IUserNameProvider 介面")]
    public interface IAlterUserProvider : IAlterUserProvider<string> { }

    /// <summary>
    /// 資料異動人員資訊提供者。
    /// </summary>
    /// <typeparam name="TUser">資料異動人員資訊型別。</typeparam>
    [Obsolete("此方法已過時，請使用 Saintber.Abstractions 套件的 IUserIdProvider / IUserNameProvider 介面")]
    public interface IAlterUserProvider<TUser>
    {
        /// <summary>
        /// 取得資料異動人員資訊。
        /// </summary>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>資料異動人員資訊。</returns>
        Task<TUser> GetAlterUserAsync(CancellationToken cancellationToken = default);
    }
}
