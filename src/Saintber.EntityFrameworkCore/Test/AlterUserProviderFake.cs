using Microsoft.Extensions.DependencyInjection;
using Saintber.EntityFrameworkCore.Abstractions;

namespace Saintber.EntityFrameworkCore.Test
{
    /// <summary>
    /// 虛擬資料異動人員資訊提供者。
    /// </summary>
    public class AlterUserProviderFake : IAlterUserProvider
    {
        /// <summary>
        /// 以 <paramref name="userInfo"/> 建構 <see cref="AlterUserProviderFake"/> 的新執行個體，
        /// <see cref="GetAlterUserAsync(CancellationToken)"/> 方法提供資料異動人員資訊回傳指定的 <paramref name="userInfo"/>。
        /// </summary>
        /// <param name="userInfo"><see cref="GetAlterUserAsync(CancellationToken)"/> 的回傳值。</param>
        public AlterUserProviderFake(string userInfo = "1")
        {
            UserInfo = userInfo;
        }

        /// <summary>
        /// 取得或設定 <see cref="GetAlterUserAsync(CancellationToken)"/> 的回傳值。
        /// </summary>
        public string UserInfo { get; set; }

        /// <summary>
        /// 取得資料異動人員資訊。
        /// </summary>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>資料異動人員資訊。</returns>
        public Task<string> GetAlterUserAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult("1");
        }
    }

    public static class AlterUserProviderFakeExtensions
    {
        /// <summary>
        /// 加入虛擬資料異動人員資訊提供者。
        /// </summary>
        /// <param name="services">註冊服務的集合。</param>
        /// <param name="userInfo">異動人員資訊。</param>
        /// <returns>註冊服務的集合。</returns>
        public static IServiceCollection AddAlterUserProviderFake(this IServiceCollection services
            , string userInfo = "1")
            => services.AddSingleton<IAlterUserProvider>(new AlterUserProviderFake(userInfo));
    }
}
