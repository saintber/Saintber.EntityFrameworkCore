using Microsoft.Extensions.DependencyInjection;
using Saintber.EntityFrameworkCore.Abstractions;

namespace Saintber.EntityFrameworkCore.UnitTests
{
    /// <summary>
    /// <see cref="IEntityStore{T}"/> 測試。
    /// </summary>
    public class EntityStoreTestsBase
    {
        protected IServiceProvider Provider = null!;

        /// <summary>
        /// CRUD 正流程測試。
        /// </summary>
        [TestMethod]
        public async Task CRUDTest()
        {
            var scope = Provider.CreateScope();
            var store = scope.ServiceProvider.GetRequiredService<IEntityStore<TestEntityNoInfo>>();

            // 建立實體資料
            await store.CreateAsync(new TestEntityNoInfo());
            var entity = (await store.GetAsync()).SingleOrDefault();

            Assert.IsNotNull(entity, $"{nameof(IEntityStore<TestEntityNoInfo>)} 建立實體資料測試失敗 - 未建立資料。");
            Assert.AreEqual(1, entity.Id, $"{nameof(IEntityStore<TestEntityNoInfo>)} 建立實體資料測試失敗。");

            // 修改實體資料
            entity.Id = 2;
            await store.UpdateAsync(entity);
            entity = (await store.GetAsync()).SingleOrDefault();

            Assert.IsNotNull(entity, $"{nameof(IEntityStore<TestEntityNoInfo>)} 異動實體資料測試失敗 - 資料不存在。");
            Assert.AreEqual(2, entity.Id, $"{nameof(IEntityStore<TestEntityNoInfo>)} 異動實體資料測試失敗。");

            // 刪除實體資料
            await store.DeleteAsync(entity);
            entity = (await store.GetAsync()).SingleOrDefault();

            Assert.IsNull(entity, $"{nameof(IEntityStore<TestEntityNoInfo>)} 刪除實體資料測試失敗 - 資料未刪除。");
        }

        /// <summary>
        /// 異動資訊自動附加測試。
        /// </summary>
        [TestMethod]
        public async Task AlterInfoTest()
        {
            var scope = Provider.CreateScope();
            var store = scope.ServiceProvider.GetRequiredService<IEntityStore<TestEntityHasInfo>>();
            var userProvider = scope.ServiceProvider.GetRequiredService<IAlterUserProvider<string>>();

            // 建立實體資料
            var dateTime = DbContextExtensions.UtcNow.AddSeconds(-1);
            await store.CreateAsync(new TestEntityHasInfo());
            var entity = (await store.GetAsync()).SingleOrDefault();

            Assert.IsNotNull(entity, $"{nameof(IEntityStore<TestEntityNoInfo>)} 建立實體資料測試失敗 - 未建立資料");
            Assert.AreEqual(1, entity.Id, $"{nameof(IEntityStore<TestEntityNoInfo>)} 建立實體資料測試失敗");

            Assert.IsTrue(entity.CreateTime > dateTime, $"建立實體資料測試失敗 - 預期大於 {dateTime:yyyy/MM/dd HH:mm:ss.fff}，" +
                $"實際 {entity.CreateTime:yyyy/MM/dd HH:mm:ss.fff}");
            Assert.AreEqual(await userProvider.GetAlterUserAsync(), entity.CreateUser, $"{nameof(IEntityStore<TestEntityNoInfo>)} 建立實體資料測試失敗");
            Assert.IsTrue(entity.UpdateTime > dateTime, $"建立實體資料測試失敗 - 預期大於 {dateTime:yyyy/MM/dd HH:mm:ss.fff}，" +
                $"實際 {entity.UpdateTime:yyyy/MM/dd HH:mm:ss.fff}");
            Assert.AreEqual(await userProvider.GetAlterUserAsync(), entity.CreateUser, $"{nameof(IEntityStore<TestEntityNoInfo>)} 建立實體資料測試失敗");
            Assert.AreEqual(false, entity.Deleted, "建立實體資料測試失敗");

            // 修改實體資料
            entity.Id = 2;
            dateTime = DbContextExtensions.UtcNow.AddSeconds(-1);
            await store.UpdateAsync(entity);
            entity = (await store.GetAsync()).SingleOrDefault();

            Assert.IsNotNull(entity, $"{nameof(IEntityStore<TestEntityNoInfo>)} 異動實體資料測試失敗 - 資料不存在");
            Assert.AreEqual(2, entity.Id, $"{nameof(IEntityStore<TestEntityNoInfo>)} 異動實體資料測試失敗。");

            Assert.IsTrue(entity.CreateTime > dateTime, $"異動實體資料測試失敗 - 預期大於 {dateTime:yyyy/MM/dd HH:mm:ss.fff}，" +
                $"實際 {entity.CreateTime:yyyy/MM/dd HH:mm:ss.fff}");
            Assert.AreEqual(await userProvider.GetAlterUserAsync(), entity.CreateUser, $"{nameof(IEntityStore<TestEntityNoInfo>)} 異動實體資料測試失敗");
            Assert.IsTrue(entity.UpdateTime > dateTime, $"異動實體資料測試失敗 - 預期大於 {dateTime:yyyy/MM/dd HH:mm:ss.fff}，" +
                $"實際 {entity.UpdateTime:yyyy/MM/dd HH:mm:ss.fff}");
            Assert.AreEqual(await userProvider.GetAlterUserAsync(), entity.CreateUser, $"{nameof(IEntityStore<TestEntityNoInfo>)} 異動實體資料測試失敗");
            Assert.AreEqual(false, entity.Deleted, "異動實體資料測試失敗");

            // 刪除實體資料
            dateTime = DbContextExtensions.UtcNow.AddSeconds(-1);
            await store.DeleteAsync(entity);
            entity = (await store.GetAsync()).SingleOrDefault();

            Assert.IsNotNull(entity, $"{nameof(IEntityStore<TestEntityNoInfo>)} 刪除實體資料測試失敗 - 資料遭到實際刪除");

            Assert.IsTrue((entity.CreateTime) > dateTime, $"刪除實體資料測試失敗 - 預期大於 {dateTime:yyyy/MM/dd HH:mm:ss.fff}，" +
                $"實際 {entity.CreateTime:yyyy/MM/dd HH:mm:ss.fff}");
            Assert.AreEqual(await userProvider.GetAlterUserAsync(), entity.CreateUser, $"{nameof(IEntityStore<TestEntityNoInfo>)} 刪除實體資料測試失敗");
            Assert.IsTrue(entity.UpdateTime > dateTime, $"刪除實體資料測試失敗 - 預期大於 {dateTime:yyyy/MM/dd HH:mm:ss.fff}，" +
                $"實際 {entity.UpdateTime:yyyy/MM/dd HH:mm:ss.fff}");
            Assert.AreEqual(await userProvider.GetAlterUserAsync(), entity.CreateUser, $"{nameof(IEntityStore<TestEntityNoInfo>)} 刪除實體資料測試失敗");
            Assert.AreEqual(true, entity.Deleted);
        }

        /// <summary>
        /// 自動識別碼測試。
        /// </summary>
        [TestMethod]
        public async Task IdTest()
        {
            var scope = Provider.CreateScope();
            var intStore = scope.ServiceProvider.GetRequiredService<IEntityStore<TestEntityNoInfo>>();
            var guidStore = scope.ServiceProvider.GetRequiredService<IEntityStore<TestEntityGuid>>();

            // int 實體資料
            await intStore.CreateAsync(new TestEntityNoInfo());
            await intStore.CreateAsync(new TestEntityNoInfo());
            var entitiesInt = await intStore.GetAsync();
            Assert.IsTrue(entitiesInt.Any(x => x.Id == 1) && entitiesInt.Any(x => x.Id == 2), "int 格式識別碼產生失敗");

            // Guid 實體資料
            await guidStore.CreateAsync(new TestEntityGuid());
            await guidStore.CreateAsync(new TestEntityGuid());
            var entitiesGuid = await guidStore.GetAsync();
            Assert.IsFalse(entitiesGuid.First().Id == entitiesGuid.Skip(1).First().Id, "Guid 格式識別碼產生失敗");
        }
    }

    /// <summary>
    /// 無軟刪除與異動資訊的測試實體資料。
    /// </summary>
    public class TestEntityNoInfo
    {
        public int Id { get; set; } = default!;
    }

    /// <summary>
    /// 有軟刪除與異動資訊的測試實體資料。
    /// </summary>
    public class TestEntityHasInfo
    {
        public int Id { get; set; } = default!;

        public DateTime CreateTime { get; set; }

        public string CreateUser { get; set; } = default!;

        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; } = default!;

        public bool Deleted { get; set; }
    }

    /// <summary>
    /// Guid 識別碼的測試實體資料。
    /// </summary>
    public class TestEntityGuid
    {
        public Guid Id { get; set; } = default!;
    }

}
