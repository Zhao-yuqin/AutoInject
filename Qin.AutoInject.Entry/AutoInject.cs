using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Qin.AutoInject
{
    public static class AutoInject
    {
        private static Type DEFAULT_DEPENDENCY_TYPE;
        private static Type DPENDENCY_SCOPED_TYPE;
        private static Type DPENDENCY_SINGLETON_TYPE;
        private static Type DPENDENCY_TRANSIENT_TYPE;
        private static Type INSTANCE_SCOPED_TYPE;
        private static Type INSTANCE_SINGLETON_TYPE;
        private static Type INSTANCE_TRANSIENT_TYPE;
        private static Type OBSOLETE_DEPENDENCY_TYPE;

        static Func<Type, bool> IS_AUTOINJECT = (type) => DEFAULT_DEPENDENCY_TYPE.IsAssignableFrom(type);

        private static void AutoInjectByInjectType(this IServiceCollection services, Type type, InjectType injectType, Type serviceType = null)
        {
            switch (injectType)
            {
                case InjectType.SingletonDependency: services.AddSingleton(serviceType, implementationType: type); break;
                case InjectType.ScopedDependency: services.AddScoped(serviceType, type); break;
                case InjectType.TransientDependency: services.AddTransient(serviceType, type); break;
                case InjectType.SingletonInstance: services.AddSingleton(type); break;
                case InjectType.ScopedInstance: services.AddScoped(type); break;
                case InjectType.TransientInstance: services.AddTransient(type); break;
                case InjectType.Obsolete: break;
                default: throw new ArgumentOutOfRangeException("无效的注入特性");
            }
        }

        private static InjectType GetLifetimeType(Type lifetimeType)
        {
            InjectType injectType = InjectType.SingletonDependency;
            if (lifetimeType.Equals(DPENDENCY_SINGLETON_TYPE))
            {
                injectType = InjectType.SingletonDependency;
            }
            else if (lifetimeType.Equals(DPENDENCY_SCOPED_TYPE))
            {
                injectType = InjectType.ScopedDependency;
            }
            else if (lifetimeType.Equals(DPENDENCY_TRANSIENT_TYPE))
            {
                injectType = InjectType.TransientDependency;
            }
            else if (lifetimeType.Equals(INSTANCE_SINGLETON_TYPE))
            {
                injectType = InjectType.SingletonInstance;
            }
            else if (lifetimeType.Equals(INSTANCE_SCOPED_TYPE))
            {
                injectType = InjectType.ScopedInstance;
            }
            else if (lifetimeType.Equals(INSTANCE_TRANSIENT_TYPE))
            {
                injectType = InjectType.TransientInstance;
            }
            else
            {
                throw new Exception("查询IDependency接口失败");
            }

            return injectType;
        }

        /// <summary>
        /// 通过标签完成自动注入
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="type">类的类型</param>
        /// <param name="attr">标签</param>
        private static void InjectByAttribute(this IServiceCollection services, Type type, AutoInjectAttribute attr)
        {
            var injectType = attr.InjectType;
            var serviceType = attr.Type;
            services.AutoInjectByInjectType(type, injectType, serviceType);
        }

        /// <summary>
        /// 通过接口实现自动注入
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="type">类的的Type</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="Exception"/>
        private static void InjectByInterface(this IServiceCollection services, Type type)
        {
            var interfaces = type.GetInterfaces().ToList();
            if (interfaces == null) throw new ArgumentNullException("获取接口失败");
            if (interfaces.Count == 0) return;
            if (interfaces.Contains(OBSOLETE_DEPENDENCY_TYPE)) return;

            if (interfaces.Count > 0)
            {
                int index = interfaces.IndexOf(DEFAULT_DEPENDENCY_TYPE);
                if (index < 2) throw new Exception("无有效注入接口,未找到继承自IDependency的接口");
                var serviceType = interfaces[index - 2];
                var lifetimeType = interfaces[index - 1];

                if (lifetimeType == null) throw new Exception("无有效注入接口,未找到继承自IDependency的接口");
                var injectType = GetLifetimeType(lifetimeType);
                services.AutoInjectByInjectType(type, injectType, serviceType);
            }
        }

        internal static Assembly[] GetDependencyAssemblies(string assemblyContainsName)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var dlls = Directory.GetFiles(path, "*.dll");
            return dlls.Select(Assembly.LoadFrom)
                       .Where(d => d.FullName.Contains(assemblyContainsName))
                       .ToArray();
        }

        internal static Type[] GetDependencyTypes(this Assembly source)
        {
            return source.GetTypes()
                         .Where(d => d.IsClass && !d.IsAbstract)
                         .Where(d => IS_AUTOINJECT(d)
                                  || d.GetCustomAttribute<AutoInjectAttribute>() != null)
                         .ToArray();
        }

        static AutoInject()
        {
            DPENDENCY_SINGLETON_TYPE = typeof(ISingletonDependency);
            DPENDENCY_SCOPED_TYPE = typeof(IScopedDependency);
            DPENDENCY_TRANSIENT_TYPE = typeof(ITransientDependency);
            INSTANCE_SINGLETON_TYPE = typeof(ISingletonInstance);
            INSTANCE_SCOPED_TYPE = typeof(IScopedInstance);
            INSTANCE_TRANSIENT_TYPE = typeof(ITransientInstance);
            DEFAULT_DEPENDENCY_TYPE = typeof(IDependency);
            OBSOLETE_DEPENDENCY_TYPE = typeof(IObsoleteDependency);
        }

        /// <summary>
        /// AutoInject Entry
        /// </summary>
        public static IServiceCollection Inject(this IServiceCollection services, string assemblyContainsName = "")
        {
            var assemblies = GetDependencyAssemblies(assemblyContainsName);
            return services.Inject(assemblies);
        }
        public static IServiceCollection Inject(this IServiceCollection services, Assembly assembly)
        {
            return services.Inject(new Assembly[] { assembly });
        }
        public static IServiceCollection Inject(this IServiceCollection services, IEnumerable<string> dllFiles)
        {
            var assemblise = dllFiles.Where(d => d.EndsWith(".dll")).Select(Assembly.LoadFrom).ToList();
            return services.Inject(assemblise);
        }
        public static IServiceCollection Inject(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            var errors = new List<string>();

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetDependencyTypes();

                foreach (var type in types)
                {
                    var attr = type.GetCustomAttribute<AutoInjectAttribute>();
                    try
                    {
                        if (attr?.Type != null)
                            services.InjectByAttribute(type, attr);
                        else
                            services.InjectByInterface(type);
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"[{type.Name}] 注入失败 => {ex.Message}");
                    }
                }
            }
            return services;
        }
    }
}