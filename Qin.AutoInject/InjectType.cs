namespace Qin.AutoInject
{
    /// <summary>
    /// 自动注入类型
    /// </summary>
    public enum InjectType
    {
        /// <summary>
        /// 1 : 单例服务
        /// </summary>
        SingletonDependency = 1,

        /// <summary>
        /// 2 : 作用域服务
        /// </summary>
        ScopedDependency,

        /// <summary>
        /// 3 : 瞬时服务
        /// </summary>
        TransientDependency,

        /// <summary>
        /// 4： 单例对象
        /// </summary>
        SingletonInstance,

        /// <summary>
        /// 5 : 作用域对象
        /// </summary>
        ScopedInstance,

        /// <summary>
        /// 6 : 瞬时对象
        /// </summary>
        TransientInstance,

        /// <summary>
        /// 7 : 弃用的，将不会添加到容器
        /// </summary>
        Obsolete
    }
}
