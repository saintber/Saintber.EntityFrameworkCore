namespace Pic.Package.Abstractions
{
    #region == 預設型別 ==
    /// <summary>
    /// 取得或設定建立資訊介面。
    /// </summary>
    public interface IHasCreateInfo : IHasCreateInfoGetter, IHasCreateInfoSetter { }

    /// <summary>
    /// 取得建立資訊介面。
    /// </summary>
    public interface IHasCreateInfoGetter : IHasCreateInfoGetter<string> { }

    /// <summary>
    /// 設定建立資訊介面。
    /// </summary>
    public interface IHasCreateInfoSetter : IHasCreateInfoSetter<string> { }
    #endregion

    #region == 泛型型別 ==
    /// <summary>
    /// 取得或設定建立資訊介面。
    /// </summary>
    /// <typeparam name="TUser">資料異動人員資訊型別。</typeparam>
    public interface IHasCreateInfo<TUser> : IHasCreateInfoGetter<TUser>, IHasCreateInfoSetter<TUser>
    { }

    /// <summary>
    /// 取得建立資訊介面。
    /// </summary>
    /// <typeparam name="TUser">資料異動人員資訊型別。</typeparam>
    public interface IHasCreateInfoGetter<TUser>
    {
        /// <summary>
        /// 建立時間。
        /// </summary>
        DateTime CreateTime { get; }

        /// <summary>
        /// 建立人員。
        /// </summary>
        TUser CreateUser { get; }
    }

    /// <summary>
    /// 設定建立資訊介面。
    /// </summary>
    /// <typeparam name="TUser">資料異動人員資訊型別。</typeparam>
    public interface IHasCreateInfoSetter<TUser>
    {
        /// <summary>
        /// 建立時間。
        /// </summary>
        DateTime CreateTime { set; }

        /// <summary>
        /// 建立人員。
        /// </summary>
        TUser CreateUser { set; }
    }
    #endregion
}
