namespace Qin.AutoInject
{
    /// <summary>
    /// 自动化依赖注入接口
    /// </summary>
    public interface IDependency
    {
    }

    /// <summary>
    /// 弃用的，将不会添加到容器
    /// </summary>
    public interface IObsoleteDependency : IDependency
    {
    }

    /// <summary>
    /// 作用域模式
    /// </summary>
    public interface IScopedDependency : IDependency
    {
    }

    /// <summary>
    /// 作用域实例
    /// </summary>
    public interface IScopedInstance : IDependency
    {
    }

    /// <summary>
    /// 单例模式
    /// </summary>
    public interface ISingletonDependency : IDependency
    {
    }

    /// <summary>
    /// 单例实例
    /// </summary>
    public interface ISingletonInstance : IDependency
    {
    }

    /// <summary>
    /// 瞬时模式
    /// </summary>
    public interface ITransientDependency : IDependency
    {
    }

    /// <summary>
    /// 瞬时实例
    /// </summary>
    public interface ITransientInstance : IDependency
    {
    }
}
