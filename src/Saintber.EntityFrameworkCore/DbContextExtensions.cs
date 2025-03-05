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
            changeTracker.ToSaveChangeEntities(
                new Dictionary<string, object?>
                {
                    { nameof(IHasCreateInfoGetter<TUserId>.CreateTime), UtcNow },
                    { nameof(IHasCreateInfoGetter<TUserId>.CreateUser), userId },
                    { nameof(IHasUpdateInfoGetter<TUserId>.UpdateTime), UtcNow },
                    { nameof(IHasUpdateInfoGetter<TUserId>.UpdateUser), userId },
                    { nameof(IHasDeletedGetter.Deleted), false }
                }, new Dictionary<string, object?>
                {
                    { nameof(IHasUpdateInfoGetter<TUserId>.UpdateTime), UtcNow },
                    { nameof(IHasUpdateInfoGetter<TUserId>.UpdateUser), userId }
                }, new Tuple<string, bool>(nameof(IHasDeletedGetter.Deleted), true));
        }


        /// <summary>
        /// 套用儲存變更時的通用資訊，由使用者自行指定資料欄位與資料值，刪除部分提供軟刪除處理。
        /// </summary>
        /// <param name="changeTracker">儲存變更的追蹤元件。</param>
        /// <param name="createFields">建立異動欄位清單，若為 null 則不處理建立資料。</param>
        /// <param name="updateFields">更新異動欄位清單，若為 null 則不處理異動資料。</param>
        /// <param name="deleteField">軟刪除欄位名稱與設定值，設定值須為 True 或 False，若不設定軟刪除欄位則不處理刪除資料。</param>
        public static void ToSaveChangeEntities(
            this ChangeTracker changeTracker
            , Dictionary<string, object?>? createFields = null
            , Dictionary<string, object?>? updateFields = null
            , Tuple<string, bool>? deleteField = null)
        {
            changeTracker.ToSaveChangeEntities(
                createFields == null ? default : (entity) =>
                {
                    foreach (var field in createFields)
                    {
                        entity.TrySetValue(field.Key, field.Value);
                    }
                },
                updateFields == null ? default : (entity) =>
                {
                    foreach (var field in updateFields)
                    {
                        entity.TrySetValue(field.Key, field.Value);
                    }
                },
                deleteField == null ? default : (entity) =>
                {
                    if (entity.Metadata.FindProperty(deleteField.Item1) != null)
                    {
                        entity.Property(deleteField.Item1).CurrentValue = deleteField.Item2;
                        entity.State = EntityState.Modified;
                    }
                });
        }

        /// <summary>
        /// 套用儲存變更時的通用資訊，由使用者自行指定處理動作，
        /// 函式呼叫順序依序為 <paramref name="createAction"/>
        /// , <paramref name="deleteAction"/>, <paramref name="updateAction"/>。
        /// </summary>
        /// <param name="changeTracker">儲存變更的追蹤元件。</param>
        /// <param name="createAction">建立處理函式。</param>
        /// <param name="updateAction">異動處理函式。</param>
        /// <param name="deleteAction">刪除處理函式。</param>
        public static void ToSaveChangeEntities(
            this ChangeTracker changeTracker
            , Action<EntityEntry>? createAction = null
            , Action<EntityEntry>? updateAction = null
            , Action<EntityEntry>? deleteAction = null)
        {
            // 建立
            if (createAction != null)
            {
                var AddedEntities = changeTracker.Entries()
                    .Where(entity => entity.State == EntityState.Added)
                    .ToList();

                AddedEntities.ForEach(createAction);
            }

            // 刪除
            if (deleteAction != null)
            {
                var DeletedEntities = changeTracker.Entries()
                    .Where(entity => entity.State == EntityState.Deleted)
                    .ToList();

                DeletedEntities.ForEach(deleteAction);
            }

            // 修改
            if (updateAction != null)
            {
                var EditedEntities = changeTracker.Entries()
                    .Where(entity => entity.State == EntityState.Modified)
                    .ToList();

                EditedEntities.ForEach(updateAction);
            }
        }
    }
}
