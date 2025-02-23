using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Saintber.EntityFrameworkCore.Test
{
    /// <summary>
    /// 虛擬資料庫連線實體，配合 <see cref="EntityStoreFake{T}" 使用。/>
    /// </summary>
    /// <typeparam name="TEntity">實體資料型別。</typeparam>
    public class DbContextFake<TEntity> : DbContext
        where TEntity : class
    {
        public DbContextFake(DbContextOptions<DbContextFake<TEntity>> options)
            : base(options)
        {
        }

        /// <summary>
        /// 測試資料集。
        /// </summary>
        public virtual DbSet<TEntity> Entities { get; set; } = default!;

        /// <summary>
        /// 建立虛擬資料庫連線實體。
        /// </summary>
        /// <returns>虛擬資料庫連線實體。</returns>
        public static DbContextFake<TEntity> Create()
        {
            var _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<DbContextFake<TEntity>>()
                .UseSqlite(_connection)
                .Options;

            var _context = new DbContextFake<TEntity>(options);
            _context.Database.EnsureCreated();
            return _context;
        }
    }
}
