namespace Saintber.EntityFrameworkCore
{
    /// <summary>
    /// 實體資料條件取得存取庫共用基底類別。
    /// </summary>
    /// <typeparam name="TEntity">實體資料型別。</typeparam>
    /// <typeparam name="TFilterModel">篩選資料模型型別。</typeparam>
    public abstract class EntityStoreBase<TEntity, TFilterModel>
        : IEntityStore<TEntity, TFilterModel>
    {
        protected readonly IEntityStore<TEntity> entityStore;

        public EntityStoreBase(IEntityStore<TEntity> entityStore)
        {
            this.entityStore = entityStore;
        }

        public Task CreateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            => entityStore.CreateAsync(entities, cancellationToken);

        public Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            => entityStore.DeleteAsync(entities, cancellationToken);

        public abstract Task<IQueryable<TEntity>> GetAllowAsync(TFilterModel model, CancellationToken cancellationToken = default);

        public abstract Task<IQueryable<TEntity>> GetAsync(TFilterModel model, CancellationToken cancellationToken = default);

        public virtual Task<IQueryable<TEntity>> GetAsync(CancellationToken cancellationToken = default)
            => entityStore.GetAsync(cancellationToken);

        public virtual Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            => entityStore.UpdateAsync(entities, cancellationToken);
    }

    /// <summary>
    /// 實體資料條件取得存取庫共用基底類別。
    /// </summary>
    /// <typeparam name="TEntity">實體資料型別。</typeparam>
    /// <typeparam name="TModel">資料模型型別。</typeparam>
    /// <typeparam name="TFilterModel">篩選資料模型型別。</typeparam>
    public abstract class EntityStoreBase<TEntity, TModel, TFilterModel>
        : EntityStoreBase<TEntity, TFilterModel>
        , IEntityStore<TEntity, TModel, TFilterModel>
    {
        public EntityStoreBase(IEntityStore<TEntity, TFilterModel> entityStore)
            : base(entityStore) { }

        public abstract Task<IEnumerable<TModel>> GetAsync(IQueryable<TEntity> query, CancellationToken cancellationToken = default);
    }
}
