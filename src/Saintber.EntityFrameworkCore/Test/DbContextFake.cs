using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

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

    /// <summary>
    /// 虛擬資料庫連線實體。/>
    /// </summary>
    public abstract class DbContextFakeBase<TDbContext> : DbContext
        where TDbContext : DbContext
    {
        public DbContextFakeBase(DbContextOptions<TDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.AmbientTransactionWarning));
        }
    }

    public static class DbContextFakeExtensions
    {
        /// <summary>
        /// 加入虛擬資料庫連線實體。
        /// </summary>
        /// <param name="services">註冊服務的集合。</param>
        /// <param name="options">資料庫選項。</param>
        /// <returns>註冊服務的集合。</returns>
        public static IServiceCollection AddDbContextFake<TDbContext>(
            this IServiceCollection services)
            where TDbContext : DbContext
        {
            services.AddScoped(provider =>
            {
                var _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();

                var options = new DbContextOptionsBuilder<TDbContext>()
                    .UseSqlite(_connection)
                    .Options;

                var _context = (TDbContext?)Activator.CreateInstance(typeof(TDbContext), options)
                    ?? throw new TypeInitializationException(typeof(TDbContext).FullName, null);
                _context.Database.EnsureCreated();
                return _context;
            });
            return services;
        }
    }
}
