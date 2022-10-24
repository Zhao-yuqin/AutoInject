using System;

namespace Qin.AutoInject
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AutoInjectAttribute : Attribute
    {
        public AutoInjectAttribute(InjectType injectType) : this(injectType, null)
        {
        }

        public AutoInjectAttribute(InjectType injectType = InjectType.ScopedDependency, Type? interfaceType = null)
        {
            Type = interfaceType;
            InjectType = injectType;
        }

        /// <summary>
        /// 注入类型
        /// </summary>
        public InjectType InjectType { get; set; }

        /// <summary>
        /// 依赖接口
        /// </summary>
        public Type? Type { get; set; }
    }
}
