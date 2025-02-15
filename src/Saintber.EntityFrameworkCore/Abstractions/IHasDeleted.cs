namespace Pic.Package.Abstractions
{
    /// <summary>
    /// 取得或設定「資料是否遭到刪除」欄位介面。
    /// </summary>
    public interface IHasDeleted : IHasDeletedGetter, IHasDeletedSetter
    { }

    /// <summary>
    /// 取得「是否遭到刪除」欄位介面。
    /// </summary>
    public interface IHasDeletedGetter
    {
        /// <summary>
        /// 取得資料是否遭到刪除。
        /// </summary>
        bool Deleted { get; }
    }

    /// <summary>
    /// 設定「是否遭到刪除」欄位介面。
    /// </summary>
    public interface IHasDeletedSetter
    {
        /// <summary>
        /// 設定資料是否遭到刪除。
        /// </summary>
        bool Deleted { get; }
    }
}
