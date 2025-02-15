using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Pic.Package.Abstractions;

namespace Saintber.EntityFrameworkCore
{
    /// <summary>
    /// 資料庫連線實體擴充函式。
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// 取得無 <see cref="DateTime.Kind"/> 資訊的 UTC 現在時間。
        /// </summary>
        public static DateTime UtcNow => new DateTimeOffset(DateTime.Now).ToUniversalTime().DateTime;

        /// <summary>
        /// 嘗試將值 <paramref name="value"/> 設定至實體資料的 <paramref name="name"/> 欄位，若欄位不存在則略過。
        /// </summary>
        /// <param name="entityEntry">實體資料。</param>
        /// <param name="name">目標欄位名稱。</param>
        /// <param name="value">目標資料值。</param>
        static void TrySetValue(this EntityEntry entityEntry, string name, object? value)
        {
            var property = entityEntry.Metadata.FindProperty(name);
            if (property != null) entityEntry.Property(name).CurrentValue = value;
        }

        /// <summary>
        /// 套用儲存變更時的通用資訊，包含 <see cref="IHasCreateInfoGetter{T}"/>、<see cref="IHasUpdateInfoGetter{T}"/> 
        /// 與 <see cref="IHasDeleted"/> 的相關欄位。
        /// </summary>
        /// <typeparam name="TUserId">使用者識別碼型別。</typeparam>
        /// <param name="changeTracker">儲存變更的追蹤元件。</param>
        /// <param name="userId">使用者識別碼。</param>
        public static void ToSaveChangeEntities<TUserId>(this ChangeTracker changeTracker, TUserId userId)
        {
            // 建立
            var AddedEntities = changeTracker.Entries()
                .Where(entity => entity.State == EntityState.Added)
                .ToList();

            AddedEntities.ForEach(entity =>
            {
                entity.TrySetValue(nameof(IHasCreateInfoGetter<string>.CreateTime), UtcNow);
                entity.TrySetValue(nameof(IHasCreateInfoGetter<string>.CreateUser), userId);
                entity.TrySetValue(nameof(IHasUpdateInfoGetter<string>.UpdateTime), UtcNow);
                entity.TrySetValue(nameof(IHasUpdateInfoGetter<string>.UpdateUser), userId);
                entity.TrySetValue(nameof(IHasDeletedGetter.Deleted), false);
            });

            // 刪除
            var DeletedEntities = changeTracker.Entries()
                .Where(entity => entity.State == EntityState.Deleted)
                .ToList();

            DeletedEntities.ForEach(entity =>
            {
                if (entity.Metadata.FindProperty(nameof(IHasDeletedGetter.Deleted)) != null)
                {
                    entity.Property(nameof(IHasDeletedGetter.Deleted)).CurrentValue = true;
                    entity.State = EntityState.Modified;
                }
            });

            // 修改
            var EditedEntities = changeTracker.Entries()
                .Where(entity => entity.State == EntityState.Modified)
                .ToList();

            EditedEntities.ForEach(entity =>
            {
                entity.TrySetValue(nameof(IHasUpdateInfoGetter<string>.UpdateTime), UtcNow);
                entity.TrySetValue(nameof(IHasUpdateInfoGetter<string>.UpdateUser), userId);
            });
        }
    }
}
