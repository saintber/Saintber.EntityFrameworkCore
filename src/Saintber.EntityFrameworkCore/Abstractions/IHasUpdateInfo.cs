namespace Pic.Package.Abstractions
{
    #region == 預設型別 ==
    /// <summary>
    /// 取得或設定異動資訊介面。
    /// </summary>
    public interface IHasUpdateInfo : IHasUpdateInfoGetter, IHasUpdateInfoSetter { }

    /// <summary>
    /// 取得異動資訊介面。
    /// </summary>
    public interface IHasUpdateInfoGetter : IHasUpdateInfoGetter<string> { }

    /// <summary>
    /// 設定異動資訊介面。
    /// </summary>
    public interface IHasUpdateInfoSetter : IHasUpdateInfoSetter<string> { }
    #endregion

    #region == 泛型型別 ==
    /// <summary>
    /// 取得或設定異動資訊介面。
    /// </summary>
    /// <typeparam name="TUser">資料異動人員資訊型別。</typeparam>
    public interface IHasUpdateInfo<TUser> : IHasUpdateInfoGetter<TUser>, IHasUpdateInfoSetter<TUser>
    { }

    /// <summary>
    /// 取得異動資訊介面。
    /// </summary>
    /// <typeparam name="TUser">資料異動人員資訊型別。</typeparam>
    public interface IHasUpdateInfoGetter<TUser>
    {
        /// <summary>
        /// 異動時間。
        /// </summary>
        DateTime UpdateTime { get; }

        /// <summary>
        /// 異動人員。
        /// </summary>
        TUser UpdateUser { get; }
    }

    /// <summary>
    /// 設定異動資訊介面。
    /// </summary>
    /// <typeparam name="TUser">資料異動人員資訊型別。</typeparam>
    public interface IHasUpdateInfoSetter<TUser>
    {
        /// <summary>
        /// 異動時間。
        /// </summary>
        DateTime UpdateTime { set; }

        /// <summary>
        /// 異動人員。
        /// </summary>
        TUser UpdateUser { set; }
    }
    #endregion
}
